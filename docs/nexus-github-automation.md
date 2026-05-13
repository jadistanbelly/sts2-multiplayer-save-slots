# Nexus and GitHub Automation

This repo has GitHub Actions for Nexus release upload and Nexus post tracking.

## Release Upload

`.github/workflows/publish-nexus.yml` runs when a GitHub Release is published. It downloads the release asset named `MultiplayerSaveSlots-vX.Y.Z.zip` and uploads it to Nexus Mods with `Nexus-Mods/upload-action@v1.0.0-beta.5`.

Set these GitHub repository settings before relying on it:

- Secret `NEXUSMODS_API_KEY`: Nexus Mods API key used by the upload action.
- Variable `NEXUSMODS_FILE_GROUP_ID`: Nexus file update group ID for the main file.

The workflow skips draft and prerelease GitHub Releases. It also skips if `NEXUSMODS_FILE_GROUP_ID` is not set.

## Nexus Posts to GitHub Issues

`.github/workflows/sync-nexus-posts.yml` runs every six hours and can also be run manually. It reads a Nexus GraphQL comment thread and creates one GitHub issue per new Nexus comment.

Set these GitHub repository settings:

- Variable `NEXUSMODS_COMMENT_THREAD_ID`: Nexus GraphQL comment thread ID for the mod posts thread. This is not necessarily the public `comment_id` query value from a Nexus URL.
- Optional variable `NEXUSMODS_POSTS_URL`: public Nexus Posts URL. Defaults to `https://www.nexusmods.com/slaythespire2/mods/887?tab=posts`.
- Optional secret `NEXUSMODS_GRAPHQL_TOKEN`: OAuth bearer token for Nexus GraphQL if the thread requires authentication.

Synced issues include a hidden marker like `nexus-comment-id:123` so reruns do not create duplicates.

## GitHub Issues to Nexus Replies

`.github/workflows/reply-nexus-post.yml` listens for new GitHub issue comments starting with `/nexus-reply`. It only runs for comments from repository owners, members, or collaborators.

Use either form:

```text
/nexus-reply
Your public Nexus reply text.
```

```text
/nexus-reply 123
Your public Nexus reply text.
```

The first form replies to the Nexus comment ID stored in the synced issue body. The second form replies to an explicit Nexus comment ID.

This workflow requires:

- Variable `NEXUSMODS_COMMENT_THREAD_ID`
- Secret `NEXUSMODS_GRAPHQL_TOKEN`

Replies are public on Nexus Mods. Keep private debugging notes in normal GitHub issue comments that do not start with `/nexus-reply`.
