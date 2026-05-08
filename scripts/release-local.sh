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

  git fetch origin main:refs/remotes/origin/main --tags

  local_main="$(git rev-parse main)"
  remote_main="$(git rev-parse origin/main)"
  [[ "$local_main" == "$remote_main" ]] || die "local main is not synced with origin/main"

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
