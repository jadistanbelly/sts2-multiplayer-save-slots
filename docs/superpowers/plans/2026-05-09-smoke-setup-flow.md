# Smoke Setup Flow Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add local smoke setup automation that installs the release zip, captures/applies real save fixtures safely, and generates manual in-game smoke reports.

**Architecture:** Keep smoke automation outside the mod runtime. Add one Bash operator script that uses Python standard-library helpers for path, JSON, zip, and report work; add one shell test script that exercises the smoke script against temporary directories; update README smoke-test docs to use the setup script instead of manual DLL copying.

**Tech Stack:** Bash, `git`, `python3`, .NET SDK, existing `scripts/release-local.sh`, local STS2 install paths, and temporary-directory shell tests.

---

## Scope Check

This plan implements:

- local install of the release zip into a STS2 mods folder
- local fixture capture from real active multiplayer save state
- local fixture apply with timestamped backups before mutation
- local Markdown smoke reports/checklists
- shell tests using temporary fake STS2 directories and fake package zips
- README smoke setup documentation

This plan does not implement:

- GUI automation
- Steam lobby automation
- two-client multiplayer automation
- synthetic STS2 save payload generation
- runtime smoke mode inside the mod
- committed user save fixtures

## File Structure

- Create: `scripts/smoke-setup-local.sh` - operator script for install, capture-fixture, apply-fixture, and report commands.
- Create: `tests/smoke-setup-local-tests.sh` - shell tests for the smoke setup script using temporary directories.
- Modify: `README.md` - replace manual smoke setup instructions with release-package and smoke-setup commands.

## Task 1: Smoke Setup Script And Shell Tests

**Files:**
- Create: `scripts/smoke-setup-local.sh`
- Create: `tests/smoke-setup-local-tests.sh`

- [ ] **Step 1: Write the failing shell tests**

Create `tests/smoke-setup-local-tests.sh`:

```bash
#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd -P)"
script="$repo_root/scripts/smoke-setup-local.sh"

pass_count=0

fail() {
  printf 'FAIL %s\n' "$*" >&2
  exit 1
}

pass() {
  pass_count=$((pass_count + 1))
  printf 'PASS %s\n' "$*"
}

assert_file() {
  [[ -f "$1" ]] || fail "expected file $1"
}

assert_dir() {
  [[ -d "$1" ]] || fail "expected directory $1"
}

assert_contains() {
  local path="$1"
  local text="$2"
  grep -Fq "$text" "$path" || fail "expected $path to contain $text"
}

make_package() {
  local zip_path="$1"
  python3 - "$zip_path" <<'PY'
from pathlib import Path
import sys
import zipfile

zip_path = Path(sys.argv[1])
zip_path.parent.mkdir(parents=True, exist_ok=True)
manifest = """{
  "id": "MultiplayerSaveSlots",
  "name": "Multiplayer Save Slots",
  "author": "test",
  "description": "test package",
  "version": "9.9.9",
  "has_pck": false,
  "has_dll": true,
  "dependencies": [],
  "affects_gameplay": false
}
"""

with zipfile.ZipFile(zip_path, "w", zipfile.ZIP_DEFLATED) as archive:
    archive.writestr("MultiplayerSaveSlots/MultiplayerSaveSlots.json", manifest)
    archive.writestr("MultiplayerSaveSlots/MultiplayerSaveSlots.dll", "fake dll")
PY
}

test_install_package() {
  local tmp
  tmp="$(mktemp -d)"
  local package="$tmp/MultiplayerSaveSlots-v9.9.9.zip"
  local sts2="$tmp/sts2"
  local smoke="$tmp/smoke"

  make_package "$package"

  "$script" install --tag v9.9.9 --package "$package" --sts2-path "$sts2" --artifacts-root "$smoke" >/tmp/smoke-install.out

  assert_file "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.json"
  assert_file "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.dll"
  assert_contains /tmp/smoke-install.out "installed manifest version: 9.9.9"
  pass "install copies package payload"
}

test_missing_package_fails() {
  local tmp
  tmp="$(mktemp -d)"

  if "$script" install --tag v9.9.8 --package "$tmp/missing.zip" --sts2-path "$tmp/sts2" >/tmp/smoke-missing.out 2>/tmp/smoke-missing.err; then
    fail "missing package unexpectedly succeeded"
  fi

  assert_contains /tmp/smoke-missing.err "run scripts/release-local.sh --package-only v9.9.8"
  pass "missing package explains release command"
}

test_invalid_fixture_name_fails() {
  local tmp
  tmp="$(mktemp -d)"
  local active="$tmp/profile/current_run_mp.save"
  mkdir -p "$(dirname "$active")"
  printf 'active save\n' > "$active"

  if "$script" capture-fixture --name ../bad --active-save-path "$active" --artifacts-root "$tmp/smoke" >/tmp/smoke-invalid.out 2>/tmp/smoke-invalid.err; then
    fail "invalid fixture name unexpectedly succeeded"
  fi

  assert_contains /tmp/smoke-invalid.err "fixture name must match"
  pass "invalid fixture name rejected"
}

test_capture_fixture_copies_active_and_bank() {
  local tmp
  tmp="$(mktemp -d)"
  local active="$tmp/profile/current_run_mp.save"
  local bank="$tmp/profile/MultiplayerSaveSlots"
  local smoke="$tmp/smoke"

  mkdir -p "$bank/saves/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
  printf 'active save\n' > "$active"
  printf '{"campaignIds":["aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"]}\n' > "$bank/index.json"
  printf 'bank payload\n' > "$bank/saves/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/multiplayer_run.save"

  "$script" capture-fixture --name managed-active --active-save-path "$active" --artifacts-root "$smoke" >/tmp/smoke-capture.out

  assert_file "$smoke/fixtures/managed-active/active/current_run_mp.save"
  assert_file "$smoke/fixtures/managed-active/bank/MultiplayerSaveSlots/index.json"
  assert_file "$smoke/fixtures/managed-active/bank/MultiplayerSaveSlots/saves/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/multiplayer_run.save"
  pass "capture fixture copies active save and bank"
}

test_apply_fixture_backs_up_before_mutation() {
  local tmp
  tmp="$(mktemp -d)"
  local active="$tmp/profile/current_run_mp.save"
  local bank="$tmp/profile/MultiplayerSaveSlots"
  local smoke="$tmp/smoke"
  local fixture="$smoke/fixtures/unmanaged-active"

  mkdir -p "$bank" "$fixture/active"
  printf 'old active\n' > "$active"
  printf 'old index\n' > "$bank/index.json"
  printf 'fixture active\n' > "$fixture/active/current_run_mp.save"

  "$script" apply-fixture --name unmanaged-active --active-save-path "$active" --artifacts-root "$smoke" >/tmp/smoke-apply.out

  grep -Fq 'fixture active' "$active" || fail "active save was not replaced by fixture"
  [[ ! -e "$bank/index.json" ]] || fail "bank state should be removed when fixture has no bank"
  local backup_count
  backup_count="$(find "$smoke/backups" -mindepth 1 -maxdepth 1 -type d | wc -l)"
  [[ "$backup_count" -eq 1 ]] || fail "expected one backup directory, got $backup_count"
  local backup_dir
  backup_dir="$(find "$smoke/backups" -mindepth 1 -maxdepth 1 -type d | head -n 1)"
  assert_file "$backup_dir/current_run_mp.save"
  assert_file "$backup_dir/MultiplayerSaveSlots/index.json"
  pass "apply fixture backs up and mutates save state"
}

test_apply_fixture_writes_report() {
  local tmp
  tmp="$(mktemp -d)"
  local active="$tmp/profile/current_run_mp.save"
  local smoke="$tmp/smoke"
  local fixture="$smoke/fixtures/unmanaged-active"

  mkdir -p "$fixture/active" "$(dirname "$active")"
  printf 'fixture active\n' > "$fixture/active/current_run_mp.save"

  "$script" apply-fixture --name unmanaged-active --active-save-path "$active" --artifacts-root "$smoke" >/tmp/smoke-report.out

  local report
  report="$(find "$smoke/reports" -type f -name '*-unmanaged-active.md' | head -n 1)"
  [[ -n "$report" ]] || fail "expected smoke report"
  assert_contains "$report" "Manual Checklist"
  assert_contains "$report" "Open Multiplayer"
  assert_contains "$report" "Fixture: unmanaged-active"
  pass "apply fixture writes smoke report"
}

test_apply_fixture_uses_unique_backups() {
  local tmp
  tmp="$(mktemp -d)"
  local active="$tmp/profile/current_run_mp.save"
  local smoke="$tmp/smoke"
  local fixture="$smoke/fixtures/unmanaged-active"

  mkdir -p "$fixture/active" "$(dirname "$active")"
  printf 'original active\n' > "$active"
  printf 'fixture active\n' > "$fixture/active/current_run_mp.save"

  "$script" apply-fixture --name unmanaged-active --active-save-path "$active" --artifacts-root "$smoke" >/tmp/smoke-unique-1.out
  printf 'second live active\n' > "$active"
  "$script" apply-fixture --name unmanaged-active --active-save-path "$active" --artifacts-root "$smoke" >/tmp/smoke-unique-2.out

  local backup_count
  backup_count="$(find "$smoke/backups" -mindepth 1 -maxdepth 1 -type d | wc -l)"
  [[ "$backup_count" -eq 2 ]] || fail "expected two backup directories, got $backup_count"
  pass "apply fixture uses unique backups"
}

test_install_package
test_missing_package_fails
test_invalid_fixture_name_fails
test_capture_fixture_copies_active_and_bank
test_apply_fixture_backs_up_before_mutation
test_apply_fixture_writes_report
test_apply_fixture_uses_unique_backups

printf '%s/%s smoke setup tests passed\n' "$pass_count" "$pass_count"
```

- [ ] **Step 2: Make the test script executable**

Run:

```bash
chmod +x tests/smoke-setup-local-tests.sh
```

- [ ] **Step 3: Run tests to verify they fail because the smoke setup script is missing**

Run:

```bash
tests/smoke-setup-local-tests.sh
```

Expected: FAIL with a message that `scripts/smoke-setup-local.sh` is missing or cannot be executed.

- [ ] **Step 4: Create `scripts/smoke-setup-local.sh`**

Create `scripts/smoke-setup-local.sh`:

```bash
#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'USAGE'
Usage:
  scripts/smoke-setup-local.sh install --tag vX.Y.Z [--package <zip>] [--sts2-path <path>] [--artifacts-root <path>]
  scripts/smoke-setup-local.sh capture-fixture --name <fixture> --active-save-path <path> [--artifacts-root <path>]
  scripts/smoke-setup-local.sh apply-fixture --name <fixture> --active-save-path <path> [--artifacts-root <path>]
  scripts/smoke-setup-local.sh report --fixture <fixture> [--active-save-path <path>] [--tag vX.Y.Z] [--sts2-path <path>] [--artifacts-root <path>]

Examples:
  scripts/release-local.sh --package-only v0.1.0
  scripts/smoke-setup-local.sh install --tag v0.1.0
  scripts/smoke-setup-local.sh capture-fixture --name unmanaged-active --active-save-path "$HOME/.local/share/Steam/steamapps/common/Slay the Spire 2/preferences/profile_1/saves/current_run_mp.save"
  scripts/smoke-setup-local.sh apply-fixture --name unmanaged-active --active-save-path "$HOME/.local/share/Steam/steamapps/common/Slay the Spire 2/preferences/profile_1/saves/current_run_mp.save"
USAGE
}

die() {
  printf 'smoke-setup-local: %s\n' "$*" >&2
  exit 1
}

info() {
  printf 'smoke-setup-local: %s\n' "$*"
}

need_command() {
  command -v "$1" >/dev/null 2>&1 || die "missing required command: $1"
}

repo_root() {
  git rev-parse --show-toplevel 2>/dev/null || die "not inside a git repository"
}

timestamp() {
  date -u +"%Y%m%dT%H%M%SZ"
}

new_backup_dir() {
  local artifacts_root="$1"
  local fixture="$2"
  local base="$artifacts_root/backups/$(timestamp)-$fixture"
  local candidate="$base"
  local counter=1

  while [[ -e "$candidate" ]]; do
    candidate="$base-$counter"
    counter=$((counter + 1))
  done

  mkdir -p "$candidate"
  printf '%s\n' "$candidate"
}

absolute_path() {
  python3 - "$1" <<'PY'
from pathlib import Path
import sys

print(Path(sys.argv[1]).expanduser().resolve())
PY
}

validate_fixture_name() {
  local name="$1"
  [[ "$name" =~ ^[A-Za-z0-9._-]+$ ]] || die "fixture name must match ^[A-Za-z0-9._-]+$"
}

default_sts2_path() {
  case "$(uname -s)" in
    Linux*) printf '%s\n' "$HOME/.local/share/Steam/steamapps/common/Slay the Spire 2" ;;
    Darwin*) printf '%s\n' "$HOME/Library/Application Support/Steam/steamapps/common/Slay the Spire 2" ;;
    MINGW*|MSYS*|CYGWIN*) printf '%s\n' "C:/Program Files (x86)/Steam/steamapps/common/Slay the Spire 2" ;;
    *) die "cannot infer STS2 path for this platform; pass --sts2-path" ;;
  esac
}

copy_dir_replace() {
  local source="$1"
  local target="$2"
  rm -rf "$target"
  mkdir -p "$(dirname "$target")"
  cp -a "$source" "$target"
}

json_field() {
  local path="$1"
  local field="$2"
  python3 - "$path" "$field" <<'PY'
import json
import sys

with open(sys.argv[1], "r", encoding="utf-8") as handle:
    data = json.load(handle)

print(data.get(sys.argv[2], ""))
PY
}

write_report() {
  local artifacts_root="$1"
  local fixture="$2"
  local active_save_path="$3"
  local backup_path="$4"
  local package_label="$5"
  local sts2_path="$6"

  local report_dir="$artifacts_root/reports"
  local report_path="$report_dir/$(timestamp)-$fixture.md"
  mkdir -p "$report_dir"

  cat > "$report_path" <<REPORT
# Multiplayer Save Slots Smoke Report

- Fixture: $fixture
- Active save path: $active_save_path
- Backup path: $backup_path
- Package: $package_label
- STS2 path: $sts2_path

## Manual Checklist

- [ ] Launch STS2 with mods enabled.
- [ ] Open Multiplayer.
- [ ] Confirm Host is visible even when a current multiplayer save exists.
- [ ] Select Host -> Standard.
- [ ] Confirm the Multiplayer Saves picker appears.
- [ ] Confirm the expected recovery option appears for this fixture.
- [ ] Record pass/fail notes below.

## Notes

REPORT

  info "smoke report: $report_path"
}

require_artifacts_root() {
  local root="$1"
  [[ -n "$root" ]] || die "artifacts root is empty"
  mkdir -p "$root"
}

install_command() {
  local tag=""
  local package_path=""
  local sts2_path=""
  local artifacts_root="artifacts/smoke"

  while [[ $# -gt 0 ]]; do
    case "$1" in
      --tag) tag="${2:-}"; shift 2 ;;
      --package) package_path="${2:-}"; shift 2 ;;
      --sts2-path) sts2_path="${2:-}"; shift 2 ;;
      --artifacts-root) artifacts_root="${2:-}"; shift 2 ;;
      *) die "unknown install argument: $1" ;;
    esac
  done

  [[ "$tag" =~ ^v[0-9]+\.[0-9]+\.[0-9]+$ ]] || die "install requires --tag vX.Y.Z"
  require_artifacts_root "$artifacts_root"

  if [[ -z "$package_path" ]]; then
    package_path="artifacts/release/MultiplayerSaveSlots-$tag.zip"
  fi

  if [[ ! -f "$package_path" ]]; then
    die "missing package $package_path; run scripts/release-local.sh --package-only $tag"
  fi

  if [[ -z "$sts2_path" ]]; then
    sts2_path="$(default_sts2_path)"
  fi

  local mods_dir="$sts2_path/mods"
  local mod_dir="$mods_dir/MultiplayerSaveSlots"

  info "package: $package_path"
  info "STS2 path: $sts2_path"
  info "installing to: $mod_dir"

  rm -rf "$mod_dir"
  mkdir -p "$mods_dir"

  python3 - "$package_path" "$mods_dir" <<'PY'
from pathlib import Path
import sys
import zipfile

zip_path = Path(sys.argv[1])
mods_dir = Path(sys.argv[2])

with zipfile.ZipFile(zip_path, "r") as archive:
    names = [item.filename for item in archive.infolist() if not item.is_dir()]
    required = {
        "MultiplayerSaveSlots/MultiplayerSaveSlots.json",
        "MultiplayerSaveSlots/MultiplayerSaveSlots.dll",
    }
    missing = sorted(required - set(names))
    if missing:
        raise SystemExit(f"missing package entries: {missing}")
    for item in archive.infolist():
        if item.filename.startswith("MultiplayerSaveSlots/"):
            archive.extract(item, mods_dir)
PY

  local manifest="$mod_dir/MultiplayerSaveSlots.json"
  local dll="$mod_dir/MultiplayerSaveSlots.dll"
  [[ -f "$manifest" ]] || die "installed manifest missing: $manifest"
  [[ -f "$dll" ]] || die "installed DLL missing: $dll"

  local manifest_version
  manifest_version="$(json_field "$manifest" version)"
  info "installed manifest version: $manifest_version"
  info "installed DLL: $dll"
}

capture_fixture_command() {
  local fixture=""
  local active_save_path=""
  local artifacts_root="artifacts/smoke"

  while [[ $# -gt 0 ]]; do
    case "$1" in
      --name) fixture="${2:-}"; shift 2 ;;
      --active-save-path) active_save_path="${2:-}"; shift 2 ;;
      --artifacts-root) artifacts_root="${2:-}"; shift 2 ;;
      *) die "unknown capture-fixture argument: $1" ;;
    esac
  done

  validate_fixture_name "$fixture"
  [[ -n "$active_save_path" ]] || die "capture-fixture requires --active-save-path"
  active_save_path="$(absolute_path "$active_save_path")"
  [[ -f "$active_save_path" ]] || die "active save missing: $active_save_path"
  require_artifacts_root "$artifacts_root"

  local save_dir
  save_dir="$(dirname "$active_save_path")"
  local bank_dir="$save_dir/MultiplayerSaveSlots"
  local fixture_dir="$artifacts_root/fixtures/$fixture"
  [[ ! -e "$fixture_dir" ]] || die "fixture already exists: $fixture"

  mkdir -p "$fixture_dir/active"
  cp "$active_save_path" "$fixture_dir/active/current_run_mp.save"

  if [[ -d "$bank_dir" ]]; then
    mkdir -p "$fixture_dir/bank"
    cp -a "$bank_dir" "$fixture_dir/bank/MultiplayerSaveSlots"
  fi

  info "captured fixture: $fixture_dir"
}

apply_fixture_command() {
  local fixture=""
  local active_save_path=""
  local artifacts_root="artifacts/smoke"

  while [[ $# -gt 0 ]]; do
    case "$1" in
      --name) fixture="${2:-}"; shift 2 ;;
      --active-save-path) active_save_path="${2:-}"; shift 2 ;;
      --artifacts-root) artifacts_root="${2:-}"; shift 2 ;;
      *) die "unknown apply-fixture argument: $1" ;;
    esac
  done

  validate_fixture_name "$fixture"
  [[ -n "$active_save_path" ]] || die "apply-fixture requires --active-save-path"
  active_save_path="$(absolute_path "$active_save_path")"
  require_artifacts_root "$artifacts_root"

  local save_dir
  save_dir="$(dirname "$active_save_path")"
  local bank_dir="$save_dir/MultiplayerSaveSlots"
  local fixture_dir="$artifacts_root/fixtures/$fixture"
  local fixture_active="$fixture_dir/active/current_run_mp.save"
  local fixture_bank="$fixture_dir/bank/MultiplayerSaveSlots"
  [[ -f "$fixture_active" ]] || die "fixture active save missing: $fixture_active"

  local backup_dir
  backup_dir="$(new_backup_dir "$artifacts_root" "$fixture")"

  info "active save path: $active_save_path"
  info "bank path: $bank_dir"
  info "backup path: $backup_dir"

  if [[ -f "$active_save_path" ]]; then
    cp "$active_save_path" "$backup_dir/current_run_mp.save"
  fi

  if [[ -d "$bank_dir" ]]; then
    cp -a "$bank_dir" "$backup_dir/MultiplayerSaveSlots"
  fi

  mkdir -p "$save_dir"
  cp "$fixture_active" "$active_save_path"

  if [[ -d "$fixture_bank" ]]; then
    copy_dir_replace "$fixture_bank" "$bank_dir"
  else
    rm -rf "$bank_dir"
  fi

  write_report "$artifacts_root" "$fixture" "$active_save_path" "$backup_dir" "fixture apply" ""
  info "applied fixture: $fixture"
}

report_command() {
  local fixture=""
  local active_save_path=""
  local tag=""
  local sts2_path=""
  local artifacts_root="artifacts/smoke"

  while [[ $# -gt 0 ]]; do
    case "$1" in
      --fixture) fixture="${2:-}"; shift 2 ;;
      --active-save-path) active_save_path="${2:-}"; shift 2 ;;
      --tag) tag="${2:-}"; shift 2 ;;
      --sts2-path) sts2_path="${2:-}"; shift 2 ;;
      --artifacts-root) artifacts_root="${2:-}"; shift 2 ;;
      *) die "unknown report argument: $1" ;;
    esac
  done

  validate_fixture_name "$fixture"
  require_artifacts_root "$artifacts_root"

  local backup_path=""
  if [[ -d "$artifacts_root/backups" ]]; then
    backup_path="$(find "$artifacts_root/backups" -mindepth 1 -maxdepth 1 -type d -name "*-$fixture" | sort | tail -n 1)"
  fi

  write_report "$artifacts_root" "$fixture" "$active_save_path" "$backup_path" "$tag" "$sts2_path"
}

main() {
  need_command git
  need_command python3

  local root
  root="$(repo_root)"
  cd "$root"

  local command="${1:-}"
  [[ -n "$command" ]] || { usage >&2; exit 2; }
  shift

  case "$command" in
    install) install_command "$@" ;;
    capture-fixture) capture_fixture_command "$@" ;;
    apply-fixture) apply_fixture_command "$@" ;;
    report) report_command "$@" ;;
    -h|--help|help) usage ;;
    *) die "unknown command: $command" ;;
  esac
}

main "$@"
```

- [ ] **Step 5: Make the smoke setup script executable**

Run:

```bash
chmod +x scripts/smoke-setup-local.sh
```

- [ ] **Step 6: Run shell tests to verify green**

Run:

```bash
tests/smoke-setup-local-tests.sh
```

Expected: output includes:

```text
PASS install copies package payload
PASS missing package explains release command
PASS invalid fixture name rejected
PASS capture fixture copies active save and bank
PASS apply fixture backs up and mutates save state
PASS apply fixture writes smoke report
PASS apply fixture uses unique backups
7/7 smoke setup tests passed
```

- [ ] **Step 7: Run existing build and tests**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: build exits 0 with 0 warnings/errors and tests report `88/88 tests passed`.

- [ ] **Step 8: Commit smoke setup script and shell tests**

Run:

```bash
git add scripts/smoke-setup-local.sh tests/smoke-setup-local-tests.sh
git commit -m "test: add smoke setup automation"
```

## Task 2: Smoke Setup Documentation

**Files:**
- Modify: `README.md`

- [ ] **Step 1: Verify current smoke section still uses manual copy**

Run:

```bash
rg -n "Copy `MultiplayerSaveSlots.json`|bin/Debug/MultiplayerSaveSlots.dll|In-Game Smoke Test" README.md
```

Expected: output includes the current manual copy instruction.

- [ ] **Step 2: Replace README smoke section**

Replace the existing `## In-Game Smoke Test` section with:

````markdown
## Automation-Assisted In-Game Smoke Test

The smoke setup flow automates packaging, local mod installation, save-state backup, and fixture application. It does not automate STS2 menu clicks or Steam multiplayer.

First build the drop-in package:

```bash
scripts/release-local.sh --package-only v0.1.0
```

Install that package into the local STS2 mods folder:

```bash
scripts/smoke-setup-local.sh install --tag v0.1.0
```

Fixtures are local artifacts created from real STS2 save files and are ignored by git. Capture a fixture from the active multiplayer save path you want to reuse:

```bash
ACTIVE_SAVE="$HOME/.local/share/Steam/steamapps/common/Slay the Spire 2/preferences/profile_1/saves/current_run_mp.save"
scripts/smoke-setup-local.sh capture-fixture --name unmanaged-active --active-save-path "$ACTIVE_SAVE"
```

Apply a fixture before launching STS2. The script backs up existing `current_run_mp.save` and `MultiplayerSaveSlots/` state before mutation:

```bash
ACTIVE_SAVE="$HOME/.local/share/Steam/steamapps/common/Slay the Spire 2/preferences/profile_1/saves/current_run_mp.save"
scripts/smoke-setup-local.sh apply-fixture --name unmanaged-active --active-save-path "$ACTIVE_SAVE"
```

Manual in-game checklist after setup:

1. Launch STS2 with mods enabled.
2. Open `Multiplayer`.
3. Confirm `Host` is visible even when a current multiplayer save exists.
4. Select `Host -> Standard`.
5. Confirm the `Multiplayer Saves - Standard` picker appears.
6. Choose `Start New Run`.
7. Confirm vanilla hosting continues.
8. Progress far enough for STS2 to write the multiplayer run save.
9. Confirm `MultiplayerSaveSlots/index.json` gains a new campaign id.
10. Re-open `Host -> Standard` and confirm the new campaign appears in the picker.
11. With an unmanaged `current_run_mp.save` fixture applied, choose a host action and confirm the recovery modal offers `Duplicate Active Save`.
12. With a managed active save changed after activation, choose a different campaign and confirm the recovery modal offers `Sync Active Save`.

Smoke reports and backups are written under `artifacts/smoke/`.
````

- [ ] **Step 3: Verify README smoke docs mention the setup script**

Run:

```bash
rg -n "Automation-Assisted In-Game Smoke Test|smoke-setup-local.sh install|capture-fixture|apply-fixture|artifacts/smoke" README.md
```

Expected: output includes the heading, install command, fixture capture/apply commands, and artifact path.

- [ ] **Step 4: Re-run smoke shell tests after docs change**

Run:

```bash
tests/smoke-setup-local-tests.sh
```

Expected: `7/7 smoke setup tests passed`.

- [ ] **Step 5: Commit README smoke setup docs**

Run:

```bash
git add README.md
git commit -m "docs: document smoke setup flow"
```

## Task 3: Final Verification

**Files:**
- Verify: `scripts/smoke-setup-local.sh`
- Verify: `tests/smoke-setup-local-tests.sh`
- Verify: `README.md`
- Verify: existing build/test/package flow

- [ ] **Step 1: Run smoke setup shell tests**

Run:

```bash
tests/smoke-setup-local-tests.sh
```

Expected: `7/7 smoke setup tests passed`.

- [ ] **Step 2: Run normal build**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
```

Expected: build exits 0 with 0 warnings/errors.

- [ ] **Step 3: Run normal test suite**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: tests report `88/88 tests passed`.

- [ ] **Step 4: Run package-only release validation**

Run:

```bash
scripts/release-local.sh --package-only v0.1.0
```

Expected: Release build exits 0, Release tests report `88/88 tests passed`, and `artifacts/release/MultiplayerSaveSlots-v0.1.0.zip` is created.

- [ ] **Step 5: Verify smoke install against a fake STS2 directory using the real package**

Run:

```bash
tmp="$(mktemp -d)"
scripts/smoke-setup-local.sh install --tag v0.1.0 --sts2-path "$tmp/sts2"
test -f "$tmp/sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.json"
test -f "$tmp/sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.dll"
```

Expected: all commands exit 0.

- [ ] **Step 6: Verify generated artifacts are ignored**

Run:

```bash
git status --short --ignored artifacts
```

Expected: generated artifact entries are prefixed with `!!`, not `??`.

- [ ] **Step 7: Confirm branch diff is limited to Phase 6 files**

Run:

```bash
git show --stat --oneline HEAD~2..HEAD
git status --short --branch
```

Expected: recent commits include only `scripts/smoke-setup-local.sh`, `tests/smoke-setup-local-tests.sh`, and `README.md`, with no uncommitted tracked changes.
