#!/usr/bin/env node

const DEFAULT_POSTS_URL = "https://www.nexusmods.com/slaythespire2/mods/887?tab=posts";
const DEFAULT_GRAPHQL_URL = "https://api.nexusmods.com/v2/graphql";
const DEFAULT_LABELS = ["nexus-post"];

export function normalizeGraphqlThreadComments(nodes, { postsUrl = DEFAULT_POSTS_URL } = {}) {
  return flattenGraphqlComments(nodes)
    .filter(comment => !comment.isDiscarded && !comment.hiddenAt && typeof comment.body === "string" && comment.body.trim())
    .map(comment => ({
      id: String(comment.id),
      author: comment.creator?.name || "unknown",
      authorMemberId: comment.creator?.memberId ?? null,
      body: comment.body.trim(),
      createdAt: comment.createdAt,
      updatedAt: comment.updatedAt,
      parentId: comment.parent?.id ? String(comment.parent.id) : null,
      url: buildNexusPostUrl(postsUrl, String(comment.id)),
    }));
}

export function buildIssueTitle(comment) {
  const firstLine = comment.body
    .split(/\r?\n/)
    .map(line => line.trim())
    .find(Boolean) || "Nexus post";
  const compactLine = firstLine.replace(/\s+/g, " ");
  const prefix = `[Nexus] ${comment.author}: `;
  const maxSummaryLength = 100 - prefix.length;
  const summary = compactLine.length > maxSummaryLength
    ? `${compactLine.slice(0, Math.max(0, maxSummaryLength - 3))}...`
    : compactLine;

  return `${prefix}${summary}`;
}

export function buildIssueBody(comment) {
  return [
    markerForComment(comment.id),
    "",
    "Synced from Nexus Mods.",
    "",
    `Author: ${comment.author}${comment.authorMemberId ? ` (${comment.authorMemberId})` : ""}`,
    `Created: ${comment.createdAt}`,
    `Updated: ${comment.updatedAt}`,
    `Source: ${comment.url}`,
    "",
    "Use `/nexus-reply` in a new issue comment to post a public reply back to this Nexus thread.",
    "",
    "---",
    "",
    comment.body,
    "",
  ].join("\n");
}

export function extractNexusCommentIdFromIssueBody(body) {
  return body?.match(/<!--\s*nexus-comment-id:([A-Za-z0-9:_-]+)\s*-->/)?.[1] ?? null;
}

export function parseNexusReplyCommand(body) {
  const match = body.match(/^\/nexus-reply(?:\s+([A-Za-z0-9:_-]+))?(?:\r?\n([\s\S]*))?$/);
  if (!match)
    return null;

  const replyBody = (match[2] || "").trim();
  return {
    commentId: match[1] || null,
    body: replyBody,
  };
}

export async function fetchNexusThreadComments({
  commentThreadId,
  graphqlToken,
  graphqlUrl = DEFAULT_GRAPHQL_URL,
  first = 50,
  fetchImpl = fetch,
}) {
  const query = `
    query NexusCommentThread($commentThreadId: ID!, $first: Int!) {
      commentThread(commentThreadId: $commentThreadId) {
        comments(first: $first) {
          nodes {
            id
            body
            createdAt
            updatedAt
            hiddenAt
            isDiscarded
            creator {
              name
              memberId
            }
            parent {
              id
            }
            replies(first: $first) {
              nodes {
                id
                body
                createdAt
                updatedAt
                hiddenAt
                isDiscarded
                creator {
                  name
                  memberId
                }
                parent {
                  id
                }
              }
            }
          }
        }
      }
    }
  `;

  const data = await nexusGraphql({
    graphqlUrl,
    graphqlToken,
    query,
    variables: { commentThreadId, first },
    fetchImpl,
  });

  return data.commentThread.comments.nodes;
}

export async function createNexusReply({
  commentThreadId,
  replyToId,
  body,
  graphqlToken,
  graphqlUrl = DEFAULT_GRAPHQL_URL,
  fetchImpl = fetch,
}) {
  const query = `
    mutation NexusCreateComment($commentThreadId: ID!, $body: String!, $replyToId: ID) {
      createComment(commentThreadId: $commentThreadId, body: $body, replyToId: $replyToId) {
        comment {
          id
          body
          createdAt
        }
      }
    }
  `;

  const data = await nexusGraphql({
    graphqlUrl,
    graphqlToken,
    query,
    variables: { commentThreadId, body, replyToId },
    fetchImpl,
  });

  return data.createComment.comment;
}

async function nexusGraphql({ graphqlUrl, graphqlToken, query, variables, fetchImpl }) {
  const headers = {
    "Content-Type": "application/json",
    "User-Agent": "MultiplayerSaveSlots GitHub automation",
  };
  if (graphqlToken)
    headers.Authorization = `Bearer ${graphqlToken}`;

  const response = await fetchImpl(graphqlUrl, {
    method: "POST",
    headers,
    body: JSON.stringify({ query, variables }),
  });

  const text = await response.text();
  let payload;
  try {
    payload = JSON.parse(text);
  } catch {
    throw new Error(`Nexus GraphQL returned non-JSON response: ${response.status} ${text.slice(0, 300)}`);
  }

  if (!response.ok || payload.errors?.length) {
    const messages = payload.errors?.map(error => error.message).join("; ") || text;
    throw new Error(`Nexus GraphQL request failed: ${response.status} ${messages}`);
  }

  return payload.data;
}

async function syncMode(env) {
  const commentThreadId = env.NEXUSMODS_COMMENT_THREAD_ID;
  if (!commentThreadId) {
    console.log("NEXUSMODS_COMMENT_THREAD_ID is not set; skipping Nexus post sync.");
    return;
  }

  const githubToken = requireEnv(env, "GITHUB_TOKEN");
  const repoFullName = requireEnv(env, "GITHUB_REPOSITORY");
  const postsUrl = env.NEXUSMODS_POSTS_URL || DEFAULT_POSTS_URL;
  const labels = parseLabels(env.NEXUS_ISSUE_LABELS);
  const nodes = await fetchNexusThreadComments({
    commentThreadId,
    graphqlToken: env.NEXUSMODS_GRAPHQL_TOKEN,
    graphqlUrl: env.NEXUSMODS_GRAPHQL_URL || DEFAULT_GRAPHQL_URL,
  });
  const comments = normalizeGraphqlThreadComments(nodes, { postsUrl });
  const github = createGitHubClient({
    token: githubToken,
    repoFullName,
    apiUrl: env.GITHUB_API_URL || "https://api.github.com",
  });

  await ensureLabels(github, labels);

  let created = 0;
  for (const comment of comments) {
    const existingIssue = await findIssueForComment(github, comment.id);
    if (existingIssue) {
      console.log(`Nexus comment ${comment.id} already synced to #${existingIssue.number}`);
      continue;
    }

    const issue = await github.request(`/issues`, {
      method: "POST",
      body: {
        title: buildIssueTitle(comment),
        body: buildIssueBody(comment),
        labels,
      },
    });
    created++;
    console.log(`Created GitHub issue #${issue.number} for Nexus comment ${comment.id}`);
  }

  console.log(`Nexus post sync complete. ${comments.length} comments checked, ${created} issues created.`);
}

async function replyMode(env, stdinBody) {
  const authorAssociation = env.GITHUB_AUTHOR_ASSOCIATION;
  const allowedAssociations = new Set(["OWNER", "MEMBER", "COLLABORATOR"]);
  if (authorAssociation && !allowedAssociations.has(authorAssociation))
    throw new Error(`Refusing Nexus reply from untrusted GitHub author association: ${authorAssociation}`);

  const command = parseNexusReplyCommand(stdinBody || env.GITHUB_COMMENT_BODY || "");
  if (!command) {
    console.log("No /nexus-reply command found; skipping.");
    return;
  }
  if (!command.body)
    throw new Error("The /nexus-reply command needs a reply body on the following line.");

  const commentThreadId = requireEnv(env, "NEXUSMODS_COMMENT_THREAD_ID");
  const graphqlToken = requireEnv(env, "NEXUSMODS_GRAPHQL_TOKEN");
  const githubToken = requireEnv(env, "GITHUB_TOKEN");
  const repoFullName = requireEnv(env, "GITHUB_REPOSITORY");
  const issueNumber = requireEnv(env, "GITHUB_ISSUE_NUMBER");
  const issueBody = env.GITHUB_ISSUE_BODY || "";
  const replyToId = command.commentId || extractNexusCommentIdFromIssueBody(issueBody);
  if (!replyToId)
    throw new Error("Could not find a Nexus comment id. Use /nexus-reply <comment-id> or run this on a synced Nexus issue.");

  const reply = await createNexusReply({
    commentThreadId,
    replyToId,
    body: command.body,
    graphqlToken,
    graphqlUrl: env.NEXUSMODS_GRAPHQL_URL || DEFAULT_GRAPHQL_URL,
  });

  const github = createGitHubClient({
    token: githubToken,
    repoFullName,
    apiUrl: env.GITHUB_API_URL || "https://api.github.com",
  });
  await github.request(`/issues/${issueNumber}/comments`, {
    method: "POST",
    body: {
      body: `Posted Nexus reply ${reply.id} in response to Nexus comment ${replyToId}.`,
    },
  });
  console.log(`Posted Nexus reply ${reply.id} in response to ${replyToId}.`);
}

function createGitHubClient({ token, repoFullName, apiUrl }) {
  return {
    async request(path, { method = "GET", body } = {}) {
      const response = await fetch(`${apiUrl}/repos/${repoFullName}${path}`, {
        method,
        headers: {
          Accept: "application/vnd.github+json",
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
          "X-GitHub-Api-Version": "2022-11-28",
        },
        body: body === undefined ? undefined : JSON.stringify(body),
      });

      if (response.status === 204)
        return null;

      const text = await response.text();
      const payload = text ? JSON.parse(text) : null;
      if (!response.ok)
        throw new Error(`GitHub request failed: ${method} ${path}: ${response.status} ${JSON.stringify(payload)}`);

      return payload;
    },
    async globalRequest(path) {
      const response = await fetch(`${apiUrl}${path}`, {
        headers: {
          Accept: "application/vnd.github+json",
          Authorization: `Bearer ${token}`,
          "X-GitHub-Api-Version": "2022-11-28",
        },
      });
      const text = await response.text();
      const payload = text ? JSON.parse(text) : null;
      if (!response.ok)
        throw new Error(`GitHub request failed: GET ${path}: ${response.status} ${JSON.stringify(payload)}`);

      return payload;
    },
    repoFullName,
  };
}

async function findIssueForComment(github, commentId) {
  const query = encodeURIComponent(`repo:${github.repoFullName} type:issue in:body "nexus-comment-id:${commentId}"`);
  const result = await github.globalRequest(`/search/issues?q=${query}`);
  return result.items?.[0] || null;
}

async function ensureLabels(github, labels) {
  for (const label of labels) {
    try {
      await github.request(`/labels/${encodeURIComponent(label)}`);
    } catch (error) {
      if (!String(error.message).includes(": 404 "))
        throw error;

      await github.request("/labels", {
        method: "POST",
        body: {
          name: label,
          color: "d73a4a",
          description: "Synced from Nexus Mods posts",
        },
      });
    }
  }
}

function flattenGraphqlComments(nodes) {
  const result = [];
  for (const node of nodes || []) {
    result.push(node);
    result.push(...flattenGraphqlComments(node.replies?.nodes || []));
  }
  return result;
}

function buildNexusPostUrl(postsUrl, commentId) {
  const url = new URL(postsUrl);
  url.searchParams.set("tab", url.searchParams.get("tab") || "posts");
  url.searchParams.set("comment_id", commentId);
  return url.toString();
}

function markerForComment(commentId) {
  return `<!-- nexus-comment-id:${commentId} -->`;
}

function parseLabels(value) {
  if (!value)
    return DEFAULT_LABELS;

  const labels = value.split(",").map(label => label.trim()).filter(Boolean);
  return labels.length ? labels : DEFAULT_LABELS;
}

function requireEnv(env, name) {
  const value = env[name];
  if (!value)
    throw new Error(`Missing required environment variable: ${name}`);

  return value;
}

async function readStdin() {
  const chunks = [];
  for await (const chunk of process.stdin)
    chunks.push(chunk);
  return Buffer.concat(chunks).toString("utf8");
}

async function main() {
  const mode = process.argv[2] || "sync";
  if (mode === "sync") {
    await syncMode(process.env);
    return;
  }

  if (mode === "reply") {
    await replyMode(process.env, await readStdin());
    return;
  }

  throw new Error(`Unknown mode: ${mode}`);
}

if (import.meta.url === `file://${process.argv[1]}`) {
  main().catch(error => {
    console.error(error.message);
    process.exit(1);
  });
}
