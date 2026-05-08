# Local Release Flow Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a local, repeatable release flow that builds, tests, packages, tags, and publishes a GitHub Release from a developer machine with STS2 installed.

**Architecture:** Keep release automation outside the mod runtime by adding one Bash script under `scripts/` and release instructions in `README.md`. The script validates the requested version tag, uses Python standard-library helpers for JSON and zip handling, builds/tests locally against the installed STS2 assemblies, stages a drop-in mod folder, and publishes only after all validation succeeds.

**Tech Stack:** Bash, `git`, `gh`, `python3`, .NET SDK targeting `net9.0`, existing no-NuGet C# test harness, and local STS2 reference assemblies.

---

## Scope Check

This plan implements:

- `scripts/release-local.sh`
- `--package-only` validation mode
- ignored local release artifacts
- README release documentation
- verification commands for package-only releases

This plan does not implement:

- GitHub Actions release automation
- a self-hosted runner
- committed STS2 reference assemblies
- a user-facing installer
- runtime mod behavior changes
- automatic manifest version bumps

## File Structure

- Create: `scripts/release-local.sh` - local release validation, package, tag, and GitHub Release publisher.
- Modify: `.gitignore` - ignore generated release artifacts.
- Modify: `README.md` - document local releases and the future "merge PR #N and tag vX.Y.Z" command contract.

## Task 1: Local Release Script

**Files:**
- Create: `scripts/release-local.sh`
- Modify: `.gitignore`

- [ ] **Step 1: Run the missing script to establish the failing checkpoint**

Run:

```bash
scripts/release-local.sh --package-only v0.1.0
```

Expected: command fails because `scripts/release-local.sh` does not exist.

- [ ] **Step 2: Add `artifacts/` to `.gitignore`**

Append this line to `.gitignore`:

```gitignore
artifacts/
```

- [ ] **Step 3: Create `scripts/release-local.sh`**

Create the `scripts/` directory if needed and add this exact script:

```bash
#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'USAGE'
Usage:
  scripts/release-local.sh [--package-only] vX.Y.Z

Examples:
  scripts/release-local.sh --package-only v0.1.0
  scripts/release-local.sh v0.1.0
USAGE
}

die() {
  printf 'release-local: %s\n' "$*" >&2
  exit 1
}

info() {
  printf 'release-local: %s\n' "$*"
}

need_command() {
  command -v "$1" >/dev/null 2>&1 || die "missing required command: $1"
}

verify_main_synced() {
  git fetch origin main:refs/remotes/origin/main --tags

  local_main="$(git rev-parse main)"
  remote_main="$(git rev-parse origin/main)"
  [[ "$local_main" == "$remote_main" ]] || die "local main is not synced with origin/main"
}

package_only=false

if [[ $# -eq 2 && "$1" == "--package-only" ]]; then
  package_only=true
  tag="$2"
elif [[ $# -eq 1 ]]; then
  tag="$1"
else
  usage >&2
  exit 2
fi

if [[ ! "$tag" =~ ^v[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
  die "tag must match v<major>.<minor>.<patch>, got '$tag'"
fi

version="${tag#v}"

need_command git
need_command dotnet
need_command python3

repo_root="$(git rev-parse --show-toplevel 2>/dev/null)" || die "not inside a git repository"
cd "$repo_root"

manifest_path="MultiplayerSaveSlots.json"
solution_path="MultiplayerSaveSlots.sln"
test_project="tests/MultiplayerSaveSlots.Tests.csproj"
dll_path="bin/Release/MultiplayerSaveSlots.dll"
artifact_root="artifacts/release"
stage_root="$artifact_root/staging"
payload_root="$stage_root/MultiplayerSaveSlots"
zip_path="$artifact_root/MultiplayerSaveSlots-$tag.zip"

[[ -f "$manifest_path" ]] || die "missing $manifest_path"
[[ -f "$solution_path" ]] || die "missing $solution_path"
[[ -f "$test_project" ]] || die "missing $test_project"

manifest_version="$(
  python3 - "$manifest_path" <<'PY'
import json
import sys

with open(sys.argv[1], "r", encoding="utf-8") as handle:
    data = json.load(handle)

print(data.get("version", ""))
PY
)"

if [[ "$manifest_version" != "$version" ]]; then
  die "$manifest_path version '$manifest_version' does not match tag '$tag'"
fi

if [[ "$package_only" == false ]]; then
  need_command gh

  if ! gh auth status >/dev/null 2>&1; then
    die "GitHub CLI is not authenticated; run 'gh auth login'"
  fi

  current_branch="$(git rev-parse --abbrev-ref HEAD)"
  [[ "$current_branch" == "main" ]] || die "full release must run from main, currently on '$current_branch'"

  if [[ -n "$(git status --porcelain)" ]]; then
    die "full release requires a clean worktree"
  fi

  verify_main_synced

  if git rev-parse -q --verify "refs/tags/$tag" >/dev/null; then
    die "local tag already exists: $tag"
  fi

  if git ls-remote --exit-code --tags origin "refs/tags/$tag" >/dev/null 2>&1; then
    die "remote tag already exists: $tag"
  fi

  if gh release view "$tag" >/dev/null 2>&1; then
    die "GitHub Release already exists for $tag; inspect it with 'gh release view $tag'"
  fi
fi

info "building Release configuration"
DOTNET_ROLL_FORWARD=Major dotnet build "$solution_path" -c Release

info "running tests"
DOTNET_ROLL_FORWARD=Major dotnet run --project "$test_project" -c Release

[[ -f "$dll_path" ]] || die "expected built DLL at $dll_path"

rm -rf "$artifact_root"
mkdir -p "$payload_root"
cp "$manifest_path" "$payload_root/MultiplayerSaveSlots.json"
cp "$dll_path" "$payload_root/MultiplayerSaveSlots.dll"

info "creating $zip_path"
python3 - "$zip_path" "$stage_root" <<'PY'
from pathlib import Path
import sys
import zipfile

zip_path = Path(sys.argv[1])
stage_root = Path(sys.argv[2])

zip_path.parent.mkdir(parents=True, exist_ok=True)

with zipfile.ZipFile(zip_path, "w", zipfile.ZIP_DEFLATED) as archive:
    for path in sorted(stage_root.rglob("*")):
        if path.is_file():
            archive.write(path, path.relative_to(stage_root).as_posix())
PY

info "verifying zip contents"
python3 - "$zip_path" <<'PY'
from pathlib import Path
import sys
import zipfile

zip_path = Path(sys.argv[1])
expected = {
    "MultiplayerSaveSlots/MultiplayerSaveSlots.json",
    "MultiplayerSaveSlots/MultiplayerSaveSlots.dll",
}

with zipfile.ZipFile(zip_path, "r") as archive:
    actual = {item.filename for item in archive.infolist() if not item.is_dir()}

if actual != expected:
    print(f"unexpected zip contents in {zip_path}", file=sys.stderr)
    print("expected:", file=sys.stderr)
    for name in sorted(expected):
        print(f"  {name}", file=sys.stderr)
    print("actual:", file=sys.stderr)
    for name in sorted(actual):
        print(f"  {name}", file=sys.stderr)
    sys.exit(1)
PY

if [[ "$package_only" == true ]]; then
  info "package-only release artifact ready: $zip_path"
  exit 0
fi

if [[ -n "$(git status --porcelain)" ]]; then
  die "full release requires a clean worktree before tagging"
fi

verify_main_synced

info "creating annotated tag $tag"
git tag -a "$tag" -m "Release $tag"

info "pushing tag $tag"
git push origin "$tag"

info "creating GitHub Release $tag"
if ! gh release create "$tag" "$zip_path" --title "Multiplayer Save Slots $tag" --generate-notes; then
  cat >&2 <<RECOVERY
release-local: tag '$tag' was pushed, but GitHub Release creation failed.
release-local: inspect the tag with:
release-local:   git ls-remote --tags origin refs/tags/$tag
release-local: retry release creation with:
release-local:   gh release create $tag $zip_path --title "Multiplayer Save Slots $tag" --generate-notes
RECOVERY
  exit 1
fi

release_url="$(gh release view "$tag" --json url --jq .url)"
info "GitHub Release published: $release_url"
info "uploaded asset: $zip_path"
```

- [ ] **Step 4: Make the script executable**

Run:

```bash
chmod +x scripts/release-local.sh
```

- [ ] **Step 5: Run package-only validation**

Run:

```bash
scripts/release-local.sh --package-only v0.1.0
```

Expected: command exits 0, tests pass, and `artifacts/release/MultiplayerSaveSlots-v0.1.0.zip` exists.

- [ ] **Step 6: Verify the zip layout independently**

Run:

```bash
python3 - <<'PY'
from pathlib import Path
import zipfile

zip_path = Path("artifacts/release/MultiplayerSaveSlots-v0.1.0.zip")
expected = {
    "MultiplayerSaveSlots/MultiplayerSaveSlots.json",
    "MultiplayerSaveSlots/MultiplayerSaveSlots.dll",
}

with zipfile.ZipFile(zip_path, "r") as archive:
    actual = {item.filename for item in archive.infolist() if not item.is_dir()}

if actual != expected:
    raise SystemExit(f"unexpected zip contents: {sorted(actual)}")

print("zip layout ok")
PY
```

Expected: prints `zip layout ok`.

- [ ] **Step 7: Commit release script and artifact ignore**

Run:

```bash
git add .gitignore scripts/release-local.sh
git commit -m "build: add local release script"
```

## Task 2: Release Documentation

**Files:**
- Modify: `README.md`

- [ ] **Step 1: Check that README has no release process section yet**

Run:

```bash
rg -n "^## Release|release-local|merge PR #N" README.md
```

Expected: command exits nonzero or prints no release-flow instructions.

- [ ] **Step 2: Add this section after the Build section**

Insert this Markdown after the Build section and before `## In-Game Smoke Test`:

````markdown
## Release

Releases are built locally because the project references STS2 assemblies from a local game install. A normal GitHub-hosted runner does not have `sts2.dll`, `GodotSharp.dll`, or `0Harmony.dll`.

Required local tools:

- .NET SDK that can target `net9.0`
- `python3`
- `git`
- authenticated GitHub CLI: `gh auth login`
- Slay the Spire 2 installed in the path used by `MultiplayerSaveSlots.csproj`

Before publishing a release, update `MultiplayerSaveSlots.json` so its `version` matches the release tag without the leading `v`.

Validate the package without creating a tag or GitHub Release:

```bash
scripts/release-local.sh --package-only v0.1.0
```

Publish a real release from a clean, synced `main` checkout:

```bash
scripts/release-local.sh v0.1.0
```

The published asset is a drop-in zip named `MultiplayerSaveSlots-vX.Y.Z.zip`. Users should extract the included `MultiplayerSaveSlots/` folder into their STS2 mods directory.

Future operator prompt contract:

```text
When I say "merge PR #N and tag vX.Y.Z":
1. Merge PR #N.
2. Update local main with a fast-forward pull.
3. Confirm main is clean and synced with origin/main.
4. Confirm MultiplayerSaveSlots.json has version X.Y.Z.
5. Run scripts/release-local.sh vX.Y.Z.
6. Report the GitHub Release URL and zip asset name.
```
````

- [ ] **Step 3: Verify the README release instructions are discoverable**

Run:

```bash
rg -n "^## Release|scripts/release-local.sh|merge PR #N and tag vX.Y.Z|python3|gh auth login" README.md
```

Expected: prints matches for the Release heading, both release commands, the future prompt contract, `python3`, and `gh auth login`.

- [ ] **Step 4: Re-run package-only validation after docs change**

Run:

```bash
scripts/release-local.sh --package-only v0.1.0
```

Expected: command exits 0 and recreates `artifacts/release/MultiplayerSaveSlots-v0.1.0.zip`.

- [ ] **Step 5: Commit README release docs**

Run:

```bash
git add README.md
git commit -m "docs: document local release process"
```

## Task 3: Final Verification

**Files:**
- Verify: `.gitignore`
- Verify: `scripts/release-local.sh`
- Verify: `README.md`
- Verify: `artifacts/release/MultiplayerSaveSlots-v0.1.0.zip`

- [ ] **Step 1: Run the normal build**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
```

Expected: build exits 0.

- [ ] **Step 2: Run the normal test suite**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: tests exit 0.

- [ ] **Step 3: Run release package validation**

Run:

```bash
scripts/release-local.sh --package-only v0.1.0
```

Expected: build and tests exit 0 inside the script, and the package is created at `artifacts/release/MultiplayerSaveSlots-v0.1.0.zip`.

- [ ] **Step 4: Confirm generated artifacts are ignored**

Run:

```bash
git status --short --ignored artifacts
```

Expected: output contains ignored `artifacts/` entries prefixed with `!!`, not untracked entries prefixed with `??`.

- [ ] **Step 5: Confirm branch diff is limited to release-flow files**

Run:

```bash
git show --stat --oneline HEAD~2..HEAD
git status --short --branch
```

Expected: recent commits include only `.gitignore`, `scripts/release-local.sh`, `README.md`, `docs/superpowers/specs/2026-05-08-local-release-flow-design.md`, and `docs/superpowers/plans/2026-05-08-local-release-flow.md`, with no uncommitted tracked changes.
