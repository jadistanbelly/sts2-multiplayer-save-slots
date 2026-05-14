#!/usr/bin/env python3

from __future__ import annotations

import argparse
import datetime as dt
import html
import json
import os
import re
import sys
import urllib.parse
import urllib.request
from dataclasses import dataclass, field
from html.parser import HTMLParser


DEFAULT_NEXUS_ORIGIN = "https://www.nexusmods.com"
DEFAULT_POSTS_URL = "https://www.nexusmods.com/slaythespire2/mods/887?tab=posts"
DEFAULT_GAME_ID = "8916"
DEFAULT_MOD_ID = "887"
DEFAULT_OBJECT_TYPE = "1"
DEFAULT_POSTS_THREAD_ID = "16873160"
DEFAULT_PAGE_SIZE = 10
BASE_LABELS = ["nexus-post", "needs-triage"]
BUG_LABEL = "probable-bug"
VOID_TAGS = {"area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track", "wbr"}


@dataclass(frozen=True)
class NexusComment:
    id: str
    author: str
    body: str
    created_at: str
    updated_at: str
    parent_id: str | None
    url: str


@dataclass(frozen=True)
class CommentGroup:
    root: NexusComment
    replies: list[NexusComment] = field(default_factory=list)


class _LegacyCommentParser(HTMLParser):
    def __init__(self, posts_url: str):
        super().__init__(convert_charrefs=True)
        self.posts_url = posts_url
        self.comments: list[dict[str, str | None]] = []
        self._comment_by_id: dict[str, dict[str, str | None]] = {}
        self._depth = 0
        self._active_comments: list[tuple[str, int]] = []
        self._capture_author_for: str | None = None
        self._capture_author_depth: int | None = None
        self._capture_body_for: str | None = None
        self._capture_body_depth: int | None = None
        self._author_chunks: list[str] = []
        self._body_chunks: list[str] = []

    def handle_starttag(self, tag: str, attrs: list[tuple[str, str | None]]) -> None:
        attrs_dict = {name: value or "" for name, value in attrs}
        start_depth = self._depth
        comment_id = self._comment_id_from_attrs(tag, attrs_dict)
        if comment_id:
            parent_id = self._active_comments[-1][0] if self._active_comments else None
            comment = {
                "id": comment_id,
                "author": None,
                "body": None,
                "created_at": None,
                "updated_at": None,
                "parent_id": parent_id,
                "url": build_nexus_post_url(self.posts_url, comment_id),
            }
            self.comments.append(comment)
            self._comment_by_id[comment_id] = comment
            self._active_comments.append((comment_id, start_depth))

        if tag not in VOID_TAGS:
            self._depth += 1

        current_id = self._active_comments[-1][0] if self._active_comments else None
        classes = set(attrs_dict.get("class", "").split())

        if current_id and tag == "span" and "comment-name" in classes:
            self._capture_author_for = current_id
            self._capture_author_depth = start_depth
            self._author_chunks = []

        content_id = attrs_dict.get("id", "")
        body_match = re.fullmatch(r"comment-content-(\d+)", content_id)
        if tag == "div" and body_match:
            self._capture_body_for = body_match.group(1)
            self._capture_body_depth = start_depth
            self._body_chunks = []

        if tag == "time" and current_id and attrs_dict.get("data-date"):
            created_at = unix_to_iso(attrs_dict["data-date"])
            comment = self._comment_by_id[current_id]
            comment["created_at"] = created_at
            comment["updated_at"] = created_at

        if self._capture_body_for and tag == "br":
            self._body_chunks.append("\n")
        if self._capture_body_for and tag == "span" and "wbbtab" in classes:
            self._body_chunks.append("\t")

    def handle_endtag(self, tag: str) -> None:
        end_depth = max(0, self._depth - 1)

        if self._capture_author_for and tag == "span" and self._capture_author_depth == end_depth:
            comment = self._comment_by_id[self._capture_author_for]
            comment["author"] = normalize_text("".join(self._author_chunks)) or "unknown"
            self._capture_author_for = None
            self._capture_author_depth = None
            self._author_chunks = []

        if self._capture_body_for and tag == "div" and self._capture_body_depth == end_depth:
            comment = self._comment_by_id[self._capture_body_for]
            comment["body"] = normalize_text("".join(self._body_chunks))
            self._capture_body_for = None
            self._capture_body_depth = None
            self._body_chunks = []

        self._depth = end_depth
        if tag == "li":
            while self._active_comments and self._depth <= self._active_comments[-1][1]:
                self._active_comments.pop()

    def handle_data(self, data: str) -> None:
        if self._capture_author_for:
            self._author_chunks.append(data)
        if self._capture_body_for:
            self._body_chunks.append(data)

    def _comment_id_from_attrs(self, tag: str, attrs: dict[str, str]) -> str | None:
        if tag != "li" or "comment" not in attrs.get("class", "").split():
            return None
        match = re.fullmatch(r"comment-(\d+)", attrs.get("id", ""))
        return match.group(1) if match else None

def parse_legacy_comments(html_text: str, posts_url: str = DEFAULT_POSTS_URL) -> list[NexusComment]:
    parser = _LegacyCommentParser(posts_url)
    parser.feed(html_text)
    parsed = []
    for comment in parser.comments:
        body = str(comment.get("body") or "").strip()
        if not body:
            continue

        created_at = str(comment.get("created_at") or "")
        parsed.append(
            NexusComment(
                id=str(comment["id"]),
                author=str(comment.get("author") or "unknown"),
                body=body,
                created_at=created_at,
                updated_at=str(comment.get("updated_at") or created_at),
                parent_id=str(comment["parent_id"]) if comment.get("parent_id") else None,
                url=str(comment["url"]),
            )
        )
    return parsed


def group_comments_by_root(comments: list[NexusComment]) -> list[CommentGroup]:
    by_id = {comment.id: comment for comment in comments}
    roots: list[NexusComment] = []
    replies_by_root: dict[str, list[NexusComment]] = {}

    for comment in comments:
        root_id = root_id_for(comment, by_id)
        if root_id == comment.id:
            roots.append(comment)
            replies_by_root.setdefault(comment.id, [])
        else:
            replies_by_root.setdefault(root_id, []).append(comment)

    return [CommentGroup(root=root, replies=replies_by_root.get(root.id, [])) for root in roots]


def root_id_for(comment: NexusComment, by_id: dict[str, NexusComment]) -> str:
    current = comment
    seen = {comment.id}
    while current.parent_id and current.parent_id in by_id and current.parent_id not in seen:
        current = by_id[current.parent_id]
        seen.add(current.id)
    return current.id


def build_issue_title(comment: NexusComment) -> str:
    first_line = next((line.strip() for line in comment.body.splitlines() if line.strip()), "Nexus post")
    compact = re.sub(r"\s+", " ", first_line)
    prefix = f"[Nexus] {comment.author}: "
    max_summary_length = 100 - len(prefix)
    summary = compact if len(compact) <= max_summary_length else compact[: max(0, max_summary_length - 3)] + "..."
    return prefix + summary


def build_issue_body(comment: NexusComment) -> str:
    return "\n".join(
        [
            marker_for_comment(comment.id),
            "",
            "Synced from Nexus Mods Posts.",
            "",
            f"Author: {comment.author}",
            f"Created: {comment.created_at}",
            f"Updated: {comment.updated_at}",
            f"Source: {comment.url}",
            "",
            "Use GitHub labels/comments to triage this post. Nexus replies are mirrored here as GitHub comments.",
            "",
            "---",
            "",
            comment.body,
            "",
        ]
    )


def build_reply_comment_body(comment: NexusComment) -> str:
    return "\n".join(
        [
            marker_for_comment(comment.id),
            "",
            "Synced reply from Nexus Mods.",
            "",
            f"Author: {comment.author}",
            f"Created: {comment.created_at}",
            f"Source: {comment.url}",
            "",
            "---",
            "",
            comment.body,
            "",
        ]
    )


def build_issue_labels(root: NexusComment, replies: list[NexusComment] | None = None) -> list[str]:
    labels = list(BASE_LABELS)
    texts = [root.body, *(reply.body for reply in replies or [])]
    if any(is_probable_bug(text) for text in texts):
        labels.append(BUG_LABEL)
    return labels


def is_probable_bug(text: str) -> bool:
    return bool(
        re.search(
            r"\[(ERROR|WARN)\]|exception|stack trace|traceback|crash|crashes|failed|fails|"
            r"does not load|doesn't load|not working|error while loading|harmonyexception|ambiguousmatch",
            text,
            re.IGNORECASE,
        )
    )


def marker_for_comment(comment_id: str) -> str:
    return f"<!-- nexus-comment-id:{comment_id} -->"


def extract_nexus_comment_id_from_issue_body(body: str) -> str | None:
    match = re.search(r"<!--\s*nexus-comment-id:([A-Za-z0-9:_-]+)\s*-->", body or "")
    return match.group(1) if match else None


def build_nexus_post_url(posts_url: str, comment_id: str) -> str:
    parsed = urllib.parse.urlparse(posts_url)
    query = urllib.parse.parse_qs(parsed.query)
    query["tab"] = [query.get("tab", ["posts"])[0] or "posts"]
    query["comment_id"] = [comment_id]
    return urllib.parse.urlunparse(parsed._replace(query=urllib.parse.urlencode(query, doseq=True)))


def normalize_text(value: str) -> str:
    value = html.unescape(value).replace("\xa0", " ")
    value = re.sub(r"[ \t]+\n", "\n", value)
    value = re.sub(r"\n[ \t]+", "\n", value)
    value = re.sub(r"[ \t]{2,}", " ", value)
    value = re.sub(r"\n{3,}", "\n\n", value)
    return "\n".join(line.rstrip() for line in value.splitlines()).strip()


def unix_to_iso(value: str) -> str:
    return dt.datetime.fromtimestamp(int(value), tz=dt.UTC).replace(microsecond=0).isoformat().replace("+00:00", "Z")


def fetch_nexus_posts_html(env: dict[str, str]) -> str:
    try:
        from curl_cffi import requests
    except ImportError as error:
        raise RuntimeError("Missing curl_cffi. Install it with: python -m pip install curl_cffi") from error

    page_size = int(env_value(env, "NEXUSMODS_POSTS_PAGE_SIZE", str(DEFAULT_PAGE_SIZE)))
    max_pages = int(env_value(env, "NEXUSMODS_POSTS_MAX_PAGES", "20"))
    posts_url = env_value(env, "NEXUSMODS_POSTS_URL", DEFAULT_POSTS_URL)
    origin = env_value(env, "NEXUSMODS_ORIGIN", DEFAULT_NEXUS_ORIGIN)
    session = requests.Session(impersonate=env_value(env, "NEXUSMODS_CURL_IMPERSONATE", "chrome136"))
    all_html: list[str] = []
    seen_ids: set[str] = set()

    for page in range(1, max_pages + 1):
        widget_url = build_widget_url(env, origin=origin, page=page, page_size=page_size)
        response = session.get(
            widget_url,
            timeout=30,
            headers={
                "Referer": posts_url,
                "User-Agent": "MultiplayerSaveSlots GitHub automation",
                "X-Requested-With": "XMLHttpRequest",
            },
        )
        text = response.text
        if response.status_code != 200 or is_cloudflare_challenge(text):
            raise RuntimeError(f"Nexus posts fetch failed: HTTP {response.status_code}")

        comments = parse_legacy_comments(text, posts_url=posts_url)
        new_ids = {comment.id for comment in comments} - seen_ids
        if not comments or not new_ids:
            break

        seen_ids.update(new_ids)
        all_html.append(text)
        if len(comments) < page_size:
            break

    return "\n".join(all_html)


def build_widget_url(env: dict[str, str], origin: str = DEFAULT_NEXUS_ORIGIN, page: int = 1, page_size: int = DEFAULT_PAGE_SIZE) -> str:
    params = {
        "tabbed": "1",
        "object_id": env_value(env, "NEXUSMODS_MOD_ID", DEFAULT_MOD_ID),
        "game_id": env_value(env, "NEXUSMODS_GAME_ID", DEFAULT_GAME_ID),
        "object_type": env_value(env, "NEXUSMODS_OBJECT_TYPE", DEFAULT_OBJECT_TYPE),
        "thread_id": env_value(env, "NEXUSMODS_POSTS_THREAD_ID", DEFAULT_POSTS_THREAD_ID),
        "skip_opening_post": "0",
        "user_is_blocked": "",
        "searchable": "true",
        "page_size": str(page_size),
        "page": str(page),
    }
    return f"{origin}/Core/Libs/Common/Widgets/CommentContainer?{urllib.parse.urlencode(params)}"


def is_cloudflare_challenge(text: str) -> bool:
    return "<title>Just a moment...</title>" in text or "cf-chl" in text


class GitHubClient:
    def __init__(self, token: str, repo_full_name: str, api_url: str = "https://api.github.com"):
        self.token = token
        self.repo_full_name = repo_full_name
        self.api_url = api_url.rstrip("/")

    def request(self, path: str, method: str = "GET", body: dict | None = None) -> dict | list | None:
        url = f"{self.api_url}/repos/{self.repo_full_name}{path}"
        return self._request_url(url, method=method, body=body)

    def global_request(self, path: str) -> dict | list | None:
        return self._request_url(f"{self.api_url}{path}")

    def _request_url(self, url: str, method: str = "GET", body: dict | None = None) -> dict | list | None:
        data = json.dumps(body).encode("utf-8") if body is not None else None
        request = urllib.request.Request(
            url,
            method=method,
            data=data,
            headers={
                "Accept": "application/vnd.github+json",
                "Authorization": f"Bearer {self.token}",
                "Content-Type": "application/json",
                "X-GitHub-Api-Version": "2022-11-28",
            },
        )
        try:
            with urllib.request.urlopen(request) as response:
                if response.status == 204:
                    return None
                text = response.read().decode("utf-8")
                return json.loads(text) if text else None
        except urllib.error.HTTPError as error:
            detail = error.read().decode("utf-8", errors="replace")
            raise RuntimeError(f"GitHub request failed: {method} {url}: HTTP {error.code} {detail}") from error


def sync_to_github(comments: list[NexusComment], env: dict[str, str], dry_run: bool = False) -> None:
    groups = group_comments_by_root(comments)
    if dry_run:
        print_dry_run(groups)
        return

    github = GitHubClient(
        token=require_env(env, "GITHUB_TOKEN"),
        repo_full_name=require_env(env, "GITHUB_REPOSITORY"),
        api_url=env.get("GITHUB_API_URL", "https://api.github.com"),
    )
    ensure_labels(github, [*BASE_LABELS, BUG_LABEL])

    created_issues = 0
    created_comments = 0
    for group in groups:
        labels = build_issue_labels(group.root, group.replies)
        issue = find_issue_for_comment(github, group.root.id)
        if issue:
            print(f"Nexus post {group.root.id} already synced to #{issue['number']}")
        else:
            issue = github.request(
                "/issues",
                method="POST",
                body={
                    "title": build_issue_title(group.root),
                    "body": build_issue_body(group.root),
                    "labels": labels,
                },
            )
            created_issues += 1
            print(f"Created GitHub issue #{issue['number']} for Nexus post {group.root.id}")

        existing_reply_ids = issue_comment_markers(github, issue["number"])
        for reply in group.replies:
            if reply.id in existing_reply_ids:
                print(f"Nexus reply {reply.id} already synced to #{issue['number']}")
                continue
            github.request(f"/issues/{issue['number']}/comments", method="POST", body={"body": build_reply_comment_body(reply)})
            created_comments += 1
            print(f"Added Nexus reply {reply.id} to GitHub issue #{issue['number']}")

    print(f"Nexus post sync complete. {len(groups)} threads checked, {created_issues} issues created, {created_comments} comments created.")


def print_dry_run(groups: list[CommentGroup]) -> None:
    for group in groups:
        labels = ", ".join(build_issue_labels(group.root, group.replies))
        print(f"Would sync #{group.root.id} as issue: {build_issue_title(group.root)} [{labels}]")
        for reply in group.replies:
            print(f"  Would sync reply #{reply.id} by {reply.author}")
    print(f"Dry run complete. {len(groups)} Nexus post threads checked.")


def find_issue_for_comment(github: GitHubClient, comment_id: str) -> dict | None:
    query = urllib.parse.quote(f'repo:{github.repo_full_name} type:issue in:body "nexus-comment-id:{comment_id}"')
    result = github.global_request(f"/search/issues?q={query}")
    items = result.get("items", []) if isinstance(result, dict) else []
    return items[0] if items else None


def issue_comment_markers(github: GitHubClient, issue_number: int) -> set[str]:
    comments = github.request(f"/issues/{issue_number}/comments?per_page=100")
    markers = set()
    for comment in comments or []:
        marker = extract_nexus_comment_id_from_issue_body(comment.get("body", ""))
        if marker:
            markers.add(marker)
    return markers


def ensure_labels(github: GitHubClient, labels: list[str]) -> None:
    colors = {
        "nexus-post": "d73a4a",
        "needs-triage": "fbca04",
        "probable-bug": "b60205",
    }
    descriptions = {
        "nexus-post": "Synced from Nexus Mods posts",
        "needs-triage": "Needs maintainer triage",
        "probable-bug": "Nexus post appears to report a bug",
    }
    for label in labels:
        try:
            github.request(f"/labels/{urllib.parse.quote(label)}")
        except RuntimeError as error:
            if "HTTP 404" not in str(error):
                raise
            github.request(
                "/labels",
                method="POST",
                body={
                    "name": label,
                    "color": colors.get(label, "ededed"),
                    "description": descriptions.get(label, ""),
                },
            )


def require_env(env: dict[str, str], name: str) -> str:
    value = env.get(name)
    if not value:
        raise RuntimeError(f"Missing required environment variable: {name}")
    return value


def load_comments(args: argparse.Namespace, env: dict[str, str]) -> list[NexusComment]:
    posts_url = env_value(env, "NEXUSMODS_POSTS_URL", DEFAULT_POSTS_URL)
    if args.html_file:
        with open(args.html_file, encoding="utf-8") as file:
            html_text = file.read()
    else:
        html_text = fetch_nexus_posts_html(env)
    return parse_legacy_comments(html_text, posts_url=posts_url)


def main(argv: list[str] | None = None) -> int:
    parser = argparse.ArgumentParser(description="Sync Nexus Mods legacy Posts comments to GitHub issues.")
    parser.add_argument("mode", nargs="?", choices=["sync", "dry-run"], default="sync")
    parser.add_argument("--html-file", help="Read a saved Nexus CommentContainer HTML response instead of fetching live posts.")
    args = parser.parse_args(argv)

    comments = load_comments(args, os.environ)
    sync_to_github(comments, os.environ, dry_run=args.mode == "dry-run")
    return 0


def env_value(env: dict[str, str], name: str, default: str) -> str:
    value = env.get(name)
    return value if value else default


if __name__ == "__main__":
    try:
        raise SystemExit(main())
    except Exception as error:
        print(error, file=sys.stderr)
        raise SystemExit(1)
