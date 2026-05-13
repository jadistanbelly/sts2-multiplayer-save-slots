import assert from "node:assert/strict";
import { test } from "node:test";

import {
  buildIssueBody,
  buildIssueTitle,
  extractNexusCommentIdFromIssueBody,
  normalizeGraphqlThreadComments,
  parseNexusReplyCommand,
} from "../scripts/sync-nexus-posts-to-github.mjs";

test("normalizes GraphQL thread comments into syncable Nexus posts", () => {
  const comments = normalizeGraphqlThreadComments([
    {
      id: "123",
      body: "Hi,\nthis crashes on startup",
      createdAt: "2026-05-13T20:07:00Z",
      updatedAt: "2026-05-13T20:08:00Z",
      hiddenAt: null,
      isDiscarded: false,
      parent: null,
      creator: { name: "Sulfur4677", memberId: 456 },
    },
    {
      id: "124",
      body: "hidden",
      createdAt: "2026-05-13T20:09:00Z",
      updatedAt: "2026-05-13T20:09:00Z",
      hiddenAt: "2026-05-13T20:10:00Z",
      isDiscarded: false,
      parent: null,
      creator: { name: "Someone", memberId: 789 },
    },
  ], {
    postsUrl: "https://www.nexusmods.com/slaythespire2/mods/887?tab=posts",
  });

  assert.deepEqual(comments, [
    {
      id: "123",
      author: "Sulfur4677",
      authorMemberId: 456,
      body: "Hi,\nthis crashes on startup",
      createdAt: "2026-05-13T20:07:00Z",
      updatedAt: "2026-05-13T20:08:00Z",
      parentId: null,
      url: "https://www.nexusmods.com/slaythespire2/mods/887?tab=posts&comment_id=123",
    },
  ]);
});

test("builds an issue title and body with a durable Nexus marker", () => {
  const comment = {
    id: "123",
    author: "Sulfur4677",
    authorMemberId: 456,
    body: "Hi,\nthis crashes on startup",
    createdAt: "2026-05-13T20:07:00Z",
    updatedAt: "2026-05-13T20:08:00Z",
    parentId: null,
    url: "https://www.nexusmods.com/slaythespire2/mods/887?tab=posts&comment_id=123",
  };

  assert.equal(buildIssueTitle(comment), "[Nexus] Sulfur4677: Hi,");

  const body = buildIssueBody(comment);
  assert.match(body, /<!-- nexus-comment-id:123 -->/);
  assert.match(body, /Author: Sulfur4677/);
  assert.match(body, /this crashes on startup/);
  assert.equal(extractNexusCommentIdFromIssueBody(body), "123");
});

test("parses guarded Nexus reply commands", () => {
  assert.deepEqual(parseNexusReplyCommand("/nexus-reply 123\nFixed in v0.2.1."), {
    commentId: "123",
    body: "Fixed in v0.2.1.",
  });

  assert.deepEqual(parseNexusReplyCommand("/nexus-reply\nFixed in v0.2.1."), {
    commentId: null,
    body: "Fixed in v0.2.1.",
  });

  assert.equal(parseNexusReplyCommand("Looks good"), null);
});
