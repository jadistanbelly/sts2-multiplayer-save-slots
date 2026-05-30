# Nexus and GitHub Automation

This repo has GitHub Actions for Nexus release upload and Nexus post tracking.

## Release Upload

`.github/workflows/publish-nexus.yml` runs when a GitHub Release is published. It downloads the release asset named `MultiplayerSaveSlots-vX.Y.Z.zip` and uploads it to Nexus Mods with `Nexus-Mods/upload-action@v1.0.0-beta.5`.

Set these GitHub repository settings before relying on it:

- Secret `NEXUSMODS_API_KEY`: Nexus Mods API key used by the upload action.
- Variable `NEXUSMODS_FILE_GROUP_ID`: Nexus file update group ID for the main file.

The workflow skips draft and prerelease GitHub Releases. It also skips if `NEXUSMODS_FILE_GROUP_ID` is not set.

## Nexus Posts to GitHub Issues

`.github/workflows/sync-nexus-posts.yml` runs every six hours and can also be run manually. It reads the Nexus legacy Posts widget and creates one GitHub issue per top-level Nexus post. Nexus replies under that post are mirrored as GitHub issue comments.

The workflow does not need a Nexus secret. It uses a browser-like fetch because normal GitHub Actions HTTP requests are blocked by Cloudflare on the public Nexus HTML endpoint.

Scheduled runs treat Nexus fetch failures such as intermittent Cloudflare HTTP 403 responses as skipped syncs instead of failed builds. Manual workflow runs still fail on fetch errors so they can be debugged directly.

Optional GitHub repository variables:

- `NEXUSMODS_POSTS_URL`: public Nexus Posts URL. Defaults to `https://www.nexusmods.com/slaythespire2/mods/887?tab=posts`.
- `NEXUSMODS_GAME_ID`: Nexus game ID. Defaults to `8916`.
- `NEXUSMODS_MOD_ID`: Nexus mod ID. Defaults to `887`.
- `NEXUSMODS_POSTS_THREAD_ID`: Nexus legacy Posts thread ID. Defaults to `16873160`.

Synced issues and comments include a hidden marker like `nexus-comment-id:123` so reruns do not create duplicates. New issues receive `nexus-post` and `needs-triage`; posts that look like crash/error reports also receive `probable-bug`.

Run a local dry-run with:

```bash
python -m pip install curl_cffi
python scripts/sync_nexus_posts_to_github.py dry-run
```

## GitHub Issues to Nexus Replies

Bidirectional replies are intentionally disabled for now.

The mod Posts and Bugs tabs are backed by Nexus legacy web widgets, not the Nexus GraphQL comment system. Read-only syncing is stable enough for GitHub Actions, but posting back would require mimicking logged-in browser requests and is likely to be brittle.
