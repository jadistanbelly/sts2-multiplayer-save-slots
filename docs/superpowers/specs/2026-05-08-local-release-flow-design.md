# Local Release Flow Design

> **Archived historical spec:** This design note is preserved for context only. It is not an active spec or current work item. See `docs/superpowers/README.md` for archive policy.

## Summary

Phase 5 adds a local, repeatable release flow for Multiplayer Save Slots. The release cannot be built on a normal GitHub-hosted runner because the mod project references STS2 assemblies from a local game install. Instead, releases are built from the developer machine that already has STS2 installed, then published to GitHub with `gh`.

The user-facing install remains a zip release. Users download the zip, extract the included `MultiplayerSaveSlots/` folder into their STS2 mods directory, and launch the game with mods enabled.

## Goals

- Produce a deterministic drop-in release zip from a clean local checkout.
- Keep the release source of truth on `main`.
- Support the operator flow: merge a PR, pull `main`, tag a version, publish a GitHub Release.
- Verify build and tests before creating a tag or release.
- Fail before publishing if manifest version, tag, branch, sync state, or zip contents are wrong.
- Document future prompt wording clearly enough that "merge PR #N and tag vX.Y.Z" can be executed without rediscovering the process.

## Non-Goals

- Do not add GitHub Actions release automation in this phase.
- Do not require a self-hosted runner.
- Do not commit or upload STS2 reference assemblies.
- Do not build a user-facing installer.
- Do not change runtime mod behavior.
- Do not automatically bump versions unless explicitly requested.

## Architecture

Add a local release script under `scripts/` and README documentation that describes the exact release process. The script owns validation, build, packaging, tagging, and GitHub Release publication. It assumes the current machine can already build the repo because STS2 is installed at the paths used by `MultiplayerSaveSlots.csproj`.

The script should use `python3` standard-library helpers for manifest JSON parsing and zip creation so it does not depend on a separate `zip` binary.

The flow is intentionally tag-driven but locally executed. A release is only published when the operator passes an explicit version tag such as `v0.1.0`.

## Release Command Contract

The primary command should be:

```bash
scripts/release-local.sh v0.1.0
```

The non-publishing validation command should be:

```bash
scripts/release-local.sh --package-only v0.1.0
```

Given a tag `vX.Y.Z`, the script must require `MultiplayerSaveSlots.json` to contain `"version": "X.Y.Z"`. The script should not silently edit the manifest because releases should be explicit and reviewable in PRs.

`--package-only` should run manifest validation, build, tests, staging, zip creation, and zip content verification. It should skip `main` branch checks, clean-worktree checks, remote sync checks, tag creation, tag push, and GitHub Release publication so it can be used safely while developing or reviewing a release-flow PR.

For future automation prompts, the documented operator instruction should be:

```text
When I say "merge PR #N and tag vX.Y.Z":
1. Merge PR #N.
2. Update local main with a fast-forward pull.
3. Confirm main is clean and synced with origin/main.
4. Confirm MultiplayerSaveSlots.json has version X.Y.Z.
5. Run scripts/release-local.sh vX.Y.Z.
6. Report the GitHub Release URL and zip asset name.
```

## Script Behavior

The local release script should:

1. Parse a single version tag argument matching `v<major>.<minor>.<patch>`.
2. Require the current branch to be `main`.
3. Require a clean worktree.
4. Fetch tags and `origin/main`.
5. Require local `main` and `origin/main` to point at the same commit.
6. Require the requested tag to not already exist locally or remotely.
7. Read `MultiplayerSaveSlots.json` and verify its `version` matches the tag without the leading `v`.
8. Remove and recreate a local release staging directory.
9. Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln -c Release
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj -c Release
```

10. Stage the release payload as:

```text
MultiplayerSaveSlots/
  MultiplayerSaveSlots.json
  MultiplayerSaveSlots.dll
```

11. Create a zip named `MultiplayerSaveSlots-vX.Y.Z.zip`.
12. Verify the zip contains exactly the expected top-level mod folder and required files.
13. Create an annotated git tag at the current commit.
14. Push the tag.
15. Create a GitHub Release with `gh release create`.
16. Upload the zip asset as part of that release.
17. Print the release URL and asset path.

## Data Flow

```text
main commit
  -> manifest version check
  -> Release build
  -> test run
  -> staging/MultiplayerSaveSlots/
  -> zip asset
  -> git tag
  -> GitHub Release
```

The script should create generated files under a disposable local output directory such as `artifacts/release/`. That directory should be ignored by git.

## Error Handling

The script should fail before mutating remote state whenever possible. Validation should happen before tag creation and release publication.

If a tag push succeeds but GitHub Release creation fails, the script should print clear recovery commands instead of deleting the tag automatically. Remote tag deletion is destructive enough that it should remain an explicit operator decision.

If the release already exists, the script should fail and point the operator to `gh release view <tag>`.

If `gh` is missing or unauthenticated, the script should fail before building and explain that `gh auth login` is required.

If STS2 reference assemblies are missing, the `dotnet build` failure is acceptable, but the README should explain that releases must be run from a machine with STS2 installed.

## Testing

Phase 5 should add a package-only validation path that verifies the release script can create the expected zip layout without publishing.

Manual verification should include:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj -c Release
scripts/release-local.sh --package-only v0.1.0
```

The final release path should be tested with a real tag only when publishing an actual release.

## Documentation

README release documentation should include:

- the local STS2 install requirement
- required tools: `.NET SDK`, `python3`, `git`, and authenticated `gh`
- how to update `MultiplayerSaveSlots.json` version before release
- how to run package-only validation
- how to publish a tagged release
- the exact future prompt contract for "merge PR #N and tag vX.Y.Z"
