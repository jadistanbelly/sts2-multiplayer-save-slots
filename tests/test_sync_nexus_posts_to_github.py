import textwrap
import unittest
from contextlib import redirect_stderr
from io import StringIO
from unittest import mock

import scripts.sync_nexus_posts_to_github as sync
from scripts.sync_nexus_posts_to_github import (
    build_issue_body,
    build_issue_labels,
    build_issue_title,
    build_reply_comment_body,
    group_comments_by_root,
    is_cloudflare_challenge,
    is_probable_bug,
    parse_legacy_comments,
)


LEGACY_COMMENTS_HTML = textwrap.dedent(
    """
    <ol>
      <li class="comment" id="comment-169576439">
        <div class="comment-head clearfix">
          <span class="comment-name"><a href="https://next.nexusmods.com/profile/Sulfur4677">Sulfur4677</a></span>
        </div>
        <div class="comment-content">
          <time class="dst-date-adjust" data-date="1778688433">13 May 2026, 4:07PM</time>
          <div class="comment-content-text" id="comment-content-169576439">
            Hi,<br>the mod itself pops up but you get this error while loading:<br>
            <code>[ERROR] Exception thrown when calling mod initializer<br>
            HarmonyLib.HarmonyException: Ambiguous match</code>
          </div>
        </div>
        <ol>
          <li class="comment" id="comment-169591664">
            <div class="comment-head clearfix">
              <span class="comment-name"><a href="https://next.nexusmods.com/profile/asixet">asixet</a></span>
            </div>
            <div class="comment-content">
              <time class="dst-date-adjust" data-date="1778714034">13 May 2026, 11:13PM</time>
              <div class="comment-content-text" id="comment-content-169591664">
                Hi,&nbsp;<br>Thanks for the detailed log. This should be fixed now.
              </div>
            </div>
          </li>
          <li class="comment" id="comment-169591736">
            <div class="comment-head clearfix">
              <span class="comment-name"><a href="https://next.nexusmods.com/profile/Sulfur4677">Sulfur4677</a></span>
            </div>
            <div class="comment-content">
              <time class="dst-date-adjust" data-date="1778714128">13 May 2026, 11:15PM</time>
              <div class="comment-content-text" id="comment-content-169591736">
                Hi,&nbsp;<br><br>I see that it's fixed now. Thanks!
              </div>
            </div>
          </li>
        </ol>
      </li>
    </ol>
    """
)

NEXUS_WIDGET_SIBLING_REPLIES_HTML = textwrap.dedent(
    """
    <ol>
      <li class="comment" id="comment-170129321">
        <div class="comment-head clearfix">
          <span class="comment-name"><a href="https://next.nexusmods.com/profile/QrowWasTaken">QrowWasTaken</a></span>
        </div>
        <div class="comment-content">
          <time class="dst-date-adjust" data-date="1779784699">26 May 2026, 8:38AM</time>
          <div class="comment-content-text" id="comment-content-170129321">
            Original report.
          </div>
        </div>
        </div>
        </div>
        <ol class="comment-kids">
          <li class="comment comment-author" id="comment-170166845">
            <div class="comment-head clearfix">
              <span class="comment-name"><a href="https://next.nexusmods.com/profile/asixet">asixet</a></span>
            </div>
            <div class="comment-content">
              <time class="dst-date-adjust" data-date="1779857746">27 May 2026, 4:55AM</time>
              <div class="comment-content-text" id="comment-content-170166845">
                First maintainer reply.
              </div>
            </div>
          </li>
          <li class="comment comment-author" id="comment-170207702">
            <div class="comment-head clearfix">
              <span class="comment-name"><a href="https://next.nexusmods.com/profile/asixet">asixet</a></span>
            </div>
            <div class="comment-content">
              <time class="dst-date-adjust" data-date="1779943995">28 May 2026, 4:53AM</time>
              <div class="comment-content-text" id="comment-content-170207702">
                Follow-up maintainer reply.
              </div>
            </div>
          </li>
        </ol>
      </li>
    </ol>
    """
)

NEXUS_WIDGET_REPLY_CONTINUATION_HTML = textwrap.dedent(
    """
    <div>
    <div>
    <ol>
      <li class="comment" id="comment-170129321">
        <div class="comment-head clearfix">
          <span class="comment-name"><a href="https://next.nexusmods.com/profile/QrowWasTaken">QrowWasTaken</a></span>
        </div>
        <div class="comment-content">
          <time class="dst-date-adjust" data-date="1779784699">26 May 2026, 8:38AM</time>
          <div class="comment-content-text" id="comment-content-170129321">
            Original report.
          </div>
        </div>
        </div>
        </div>
        </div>
        <ol class="comment-kids">
          <li class="comment comment-author" id="comment-170166845">
            <div class="comment-head clearfix">
              <span class="comment-name"><a href="https://next.nexusmods.com/profile/asixet">asixet</a></span>
            </div>
            <div class="comment-content">
              <time class="dst-date-adjust" data-date="1779857746">27 May 2026, 4:55AM</time>
              <div class="comment-content-text" id="comment-content-170166845">
                First maintainer reply.
              </div>
            </div>
          </li>
          <li class="comment comment-author" id="comment-170207702">
            <div class="comment-head clearfix">
              <span class="comment-name"><a href="https://next.nexusmods.com/profile/asixet">asixet</a></span>
            </div>
            <div class="comment-content">
              <time class="dst-date-adjust" data-date="1779943995">28 May 2026, 4:53AM</time>
              <div class="comment-content-text" id="comment-content-170207702">
                Second maintainer reply with markup that unbalances parser depth.
              </div>
            </div>
          </li>
          <li class="comment comment-author" id="comment-170292092">
            <div class="comment-head clearfix">
              <span class="comment-name"><a href="https://next.nexusmods.com/profile/asixet">asixet</a></span>
            </div>
            <div class="comment-content">
              <time class="dst-date-adjust" data-date="1780117772">30 May 2026, 5:09AM</time>
              <div class="comment-content-text" id="comment-content-170292092">
                Hi again, please test this build.
              </div>
            </div>
          </li>
        </ol>
      </li>
    </ol>
    </div>
    </div>
    """
)


class NexusLegacyPostTests(unittest.TestCase):
    def test_parses_legacy_comment_html_with_reply_parents(self):
        comments = parse_legacy_comments(
            LEGACY_COMMENTS_HTML,
            posts_url="https://www.nexusmods.com/slaythespire2/mods/887?tab=posts",
        )

        self.assertEqual([comment.id for comment in comments], ["169576439", "169591664", "169591736"])
        self.assertEqual(comments[0].author, "Sulfur4677")
        self.assertEqual(comments[0].parent_id, None)
        self.assertEqual(comments[0].created_at, "2026-05-13T16:07:13Z")
        self.assertIn("[ERROR] Exception thrown", comments[0].body)
        self.assertEqual(comments[0].url, "https://www.nexusmods.com/slaythespire2/mods/887?tab=posts&comment_id=169576439")

        self.assertEqual(comments[1].author, "asixet")
        self.assertEqual(comments[1].parent_id, "169576439")
        self.assertEqual(comments[1].body, "Hi,\nThanks for the detailed log. This should be fixed now.")

    def test_groups_replies_under_their_top_level_comment(self):
        comments = parse_legacy_comments(LEGACY_COMMENTS_HTML)

        groups = group_comments_by_root(comments)

        self.assertEqual(len(groups), 1)
        self.assertEqual(groups[0].root.id, "169576439")
        self.assertEqual([reply.id for reply in groups[0].replies], ["169591664", "169591736"])

    def test_groups_sibling_replies_under_parent_when_widget_fragment_depth_is_unbalanced(self):
        comments = parse_legacy_comments(NEXUS_WIDGET_SIBLING_REPLIES_HTML)

        groups = group_comments_by_root(comments)

        self.assertEqual(len(groups), 1)
        self.assertEqual(groups[0].root.id, "170129321")
        self.assertEqual([reply.id for reply in groups[0].replies], ["170166845", "170207702"])

    def test_groups_later_reply_under_comment_kids_parent_when_depth_tracker_loses_parent(self):
        comments = parse_legacy_comments(NEXUS_WIDGET_REPLY_CONTINUATION_HTML)

        groups = group_comments_by_root(comments)

        self.assertEqual(len(groups), 1)
        self.assertEqual(groups[0].root.id, "170129321")
        self.assertEqual([reply.id for reply in groups[0].replies], ["170166845", "170207702", "170292092"])

    def test_builds_issue_content_and_probable_bug_labels(self):
        root = parse_legacy_comments(LEGACY_COMMENTS_HTML)[0]

        self.assertEqual(build_issue_title(root), "[Nexus] Sulfur4677: Hi,")
        self.assertTrue(is_probable_bug(root.body))
        self.assertEqual(build_issue_labels(root), ["nexus-post", "needs-triage", "probable-bug"])

        issue_body = build_issue_body(root)
        self.assertIn("<!-- nexus-comment-id:169576439 -->", issue_body)
        self.assertIn("Source: https://www.nexusmods.com/slaythespire2/mods/887?tab=posts&comment_id=169576439", issue_body)

    def test_builds_deduplicatable_reply_comment_body(self):
        reply = parse_legacy_comments(LEGACY_COMMENTS_HTML)[1]

        body = build_reply_comment_body(reply)

        self.assertIn("<!-- nexus-comment-id:169591664 -->", body)
        self.assertIn("Synced reply from Nexus Mods.", body)
        self.assertIn("Thanks for the detailed log", body)

    def test_cloudflare_detection_does_not_match_arbitrary_url_text(self):
        self.assertTrue(is_cloudflare_challenge("<html><head><title>Just a moment...</title></head></html>"))
        self.assertTrue(is_cloudflare_challenge("<script id=\"cf-chl-widget\"></script>"))
        self.assertFalse(is_cloudflare_challenge("Docs mention https://challenges.cloudflare.com without a challenge page."))

    def test_main_allows_fetch_failures_when_configured(self):
        with mock.patch.dict(sync.os.environ, {"NEXUSMODS_ALLOW_FETCH_FAILURE": "1"}), mock.patch(
            "scripts.sync_nexus_posts_to_github.fetch_nexus_posts_html",
            side_effect=RuntimeError("Nexus posts fetch failed: HTTP 403"),
        ), redirect_stderr(StringIO()):
            self.assertEqual(sync.main(["sync"]), 0)

    def test_main_raises_fetch_failures_by_default(self):
        with mock.patch.dict(sync.os.environ, {}, clear=True), mock.patch(
            "scripts.sync_nexus_posts_to_github.fetch_nexus_posts_html",
            side_effect=RuntimeError("Nexus posts fetch failed: HTTP 403"),
        ):
            with self.assertRaises(RuntimeError):
                sync.main(["sync"])


if __name__ == "__main__":
    unittest.main()
