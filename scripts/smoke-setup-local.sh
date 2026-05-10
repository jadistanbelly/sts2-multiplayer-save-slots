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

best_effort_rm() {
  local target
  for target in "$@"; do
    [[ -n "$target" ]] || continue
    rm -rf "$target" 2>/dev/null || python3 - "$target" <<'PY' || true
import os
import shutil
import sys

for target in sys.argv[1:]:
    if not os.path.lexists(target):
        continue
    if os.path.isdir(target) and not os.path.islink(target):
        shutil.rmtree(target, ignore_errors=True)
    else:
        try:
            os.unlink(target)
        except FileNotFoundError:
            pass
PY
  done
}

require_value() {
  local flag="$1"
  local value="${2-}"
  [[ -n "$value" ]] || die "$flag requires a value"
}

repo_root() {
  git rev-parse --show-toplevel 2>/dev/null || die "not inside a git repository"
}

timestamp() {
  date -u +"%Y%m%dT%H%M%SZ"
}

absolute_path() {
  python3 - "$1" <<'PY'
from pathlib import Path
import sys

path = Path(sys.argv[1]).expanduser()
if not path.is_absolute():
    path = Path.cwd() / path
print(path.as_posix())
PY
}

validate_fixture_name() {
  local name="$1"
  local upper
  local device_base
  upper="$(printf '%s' "$name" | LC_ALL=C tr '[:lower:]' '[:upper:]')"
  device_base="${upper%%.*}"

  [[ "$name" =~ ^[A-Za-z0-9._-]+$ && "$name" != "." && "$name" != ".." && "$name" != *. ]] || die "fixture name must match ^[A-Za-z0-9._-]+$"

  case "$device_base" in
    CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9])
      die "fixture name must match ^[A-Za-z0-9._-]+$"
      ;;
  esac
}

validate_active_save_path() {
  local path="$1"
  [[ "$(basename "$path")" == "current_run_mp.save" ]] || die "active save path basename must be current_run_mp.save"
}

reject_symlinks_in_tree() {
  python3 - "$1" <<'PY'
from pathlib import Path
import sys

root = Path(sys.argv[1])
for path in root.rglob("*"):
    if path.is_symlink():
        print(path, file=sys.stderr)
        sys.exit(1)
PY
}

reject_fixture_namespace_symlinks() {
  local path="$1"
  local mode="${2:-include-leaf}"
  python3 - "$path" "$mode" <<'PY' || die "fixture namespace path must not contain symlinks: $path"
from pathlib import Path
import sys

path = Path(sys.argv[1]).expanduser()
mode = sys.argv[2]
if not path.is_absolute():
    path = Path.cwd() / path

current = Path(path.anchor)
parts = list(path.parts[1:])
if mode == "parents-only":
    parts = parts[:-1]

for part in parts:
    current = current / part
    if current.is_symlink():
        sys.exit(1)
    if not current.exists():
        break
PY
}

validate_tag() {
  local tag="$1"
  local context="$2"
  [[ "$tag" =~ ^v[0-9]+\.[0-9]+\.[0-9]+$ ]] || die "$context requires --tag vX.Y.Z"
}

require_artifacts_root() {
  local root="$1"
  [[ -n "$root" ]] || die "artifacts root is empty"
  mkdir -p "$root"
}

default_sts2_path() {
  case "$(uname -s)" in
    Linux*) printf '%s\n' "$HOME/.local/share/Steam/steamapps/common/Slay the Spire 2" ;;
    Darwin*) printf '%s\n' "$HOME/Library/Application Support/Steam/steamapps/common/Slay the Spire 2" ;;
    MINGW*|MSYS*|CYGWIN*) printf '%s\n' "C:/Program Files (x86)/Steam/steamapps/common/Slay the Spire 2" ;;
    *) die "cannot infer STS2 path for this platform; pass --sts2-path" ;;
  esac
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

restore_apply_from_backup() {
  local active_save_path="$1"
  local bank_dir="$2"
  local backup_dir="$3"
  local moved_bank_parent="${4:-}"
  local save_dir
  local moved_bank_dir=""
  save_dir="$(dirname "$active_save_path")"

  mkdir -p "$save_dir"

  if [[ -f "$backup_dir/current_run_mp.save" ]]; then
    cp "$backup_dir/current_run_mp.save" "$active_save_path" || return 1
  else
    rm -f "$active_save_path" || return 1
  fi

  if [[ -n "$moved_bank_parent" ]]; then
    moved_bank_dir="$moved_bank_parent/MultiplayerSaveSlots"
  fi

  if [[ -n "$moved_bank_dir" && -d "$moved_bank_dir" ]]; then
    if [[ -e "$bank_dir" || -L "$bank_dir" ]]; then
      rm -rf "$bank_dir" || return 1
    fi
    if mv "$moved_bank_dir" "$bank_dir"; then
      return 0
    fi
  fi

  if [[ -d "$backup_dir/MultiplayerSaveSlots" ]]; then
    rm -rf "$bank_dir" || return 1
    cp -a "$backup_dir/MultiplayerSaveSlots" "$bank_dir" || return 1
  else
    rm -rf "$bank_dir" || return 1
  fi
}

rollback_apply_failure() {
  local message="$1"
  local active_save_path="$2"
  local bank_dir="$3"
  local backup_dir="$4"
  local bank_stage_parent="${5:-}"
  local moved_bank_parent="${6:-}"
  local active_stage_parent="${7:-}"

  if restore_apply_from_backup "$active_save_path" "$bank_dir" "$backup_dir" "$moved_bank_parent"; then
    best_effort_rm "$active_stage_parent" "$bank_stage_parent" "$moved_bank_parent"
    die "$message"
  fi

  best_effort_rm "$active_stage_parent" "$bank_stage_parent"
  die "$message; failed to restore pre-mutation state from $backup_dir"
}

new_backup_dir() {
  local artifacts_root="$1"
  local fixture="$2"
  local backup_root="$artifacts_root/backups/$fixture"
  local base="$backup_root/$(timestamp)"
  local candidate="$base"
  local counter=1

  mkdir -p "$backup_root"
  while ! mkdir "$candidate" 2>/dev/null; do
    [[ -e "$candidate" ]] || die "failed to create backup directory: $candidate"
    candidate="$base-$counter"
    counter=$((counter + 1))
  done

  printf '%s\n' "$candidate"
}

new_report_path() {
  local artifacts_root="$1"
  local fixture="$2"
  local report_dir="$artifacts_root/reports"
  local base="$report_dir/$(timestamp)-$fixture"
  local candidate="$base.md"
  local counter=1

  mkdir -p "$report_dir"
  while ! (set -o noclobber; : > "$candidate") 2>/dev/null; do
    [[ -e "$candidate" ]] || die "failed to create report file: $candidate"
    candidate="$base-$counter.md"
    counter=$((counter + 1))
  done

  printf '%s\n' "$candidate"
}

latest_backup_for_fixture() {
  local artifacts_root="$1"
  local fixture="$2"
  local backup_root="$artifacts_root/backups/$fixture"

  [[ -d "$backup_root" ]] || return 0
  python3 - "$backup_root" <<'PY'
from pathlib import Path
import re
import sys

backup_root = Path(sys.argv[1])
pattern = re.compile(r"^(\d{8}T\d{6}Z)(?:-(\d+))?$")
candidates = []

for path in backup_root.iterdir():
    if not path.is_dir():
        continue
    match = pattern.match(path.name)
    if match:
        suffix = int(match.group(2) or "0")
        candidates.append((match.group(1), suffix, path.name, path))
    else:
        stat = path.stat()
        candidates.append(("", -1, f"{stat.st_mtime_ns:020d}-{path.name}", path))

if candidates:
    print(max(candidates)[3])
PY
}

write_report() {
  local artifacts_root="$1"
  local fixture="$2"
  local active_save_path="$3"
  local backup_path="$4"
  local package_label="$5"
  local sts2_path="$6"
  local report_path

  [[ -n "$active_save_path" ]] || active_save_path="not provided"
  [[ -n "$backup_path" ]] || backup_path="not provided"
  [[ -n "$package_label" ]] || package_label="not provided"
  [[ -n "$sts2_path" ]] || sts2_path="not provided"

  report_path="$(new_report_path "$artifacts_root" "$fixture")"
  cat > "$report_path" <<REPORT
# Multiplayer Save Slots Smoke Report

- Fixture: $fixture
- Active save path: $active_save_path
- Backup path: $backup_path
- Package/tag: $package_label
- STS2 path: $sts2_path

## Manual Checklist

### Solo Host-Picker Gate

- [ ] Launch STS2 with mods enabled.
- [ ] Open Multiplayer.
- [ ] Confirm Host is visible even when a current multiplayer save exists.
- [ ] Select Host -> Standard.
- [ ] Confirm the Multiplayer Saves - Standard picker appears.
- [ ] Choose Start New Run.
- [ ] Confirm hosting continues to the multiplayer lobby or character-select screen.
- [ ] If RemoveMultiplayerPlayerLimit is installed, confirm the log records the RMP hosted lobby path/capacity.
- [ ] Note: STS2 does not write a multiplayer run save from a solo host lobby; save creation requires at least one joined player and a run that actually begins.

### Two-Player Save Lifecycle

- [ ] Invite at least one other player and begin a Standard multiplayer run.
- [ ] Progress far enough for STS2 to write the multiplayer run save.
- [ ] Confirm MultiplayerSaveSlots/index.json gains a new campaign id.
- [ ] Re-open Host -> Standard and confirm the new campaign appears in the picker.
- [ ] Confirm the picker shows real roster labels and omits progress when no safe act/floor value is available.
- [ ] Select the campaign row and confirm the preview shows progress, player count, timestamps, campaign id, save fingerprint, and the full roster.
- [ ] Confirm selected-character icons or badge fallbacks stay compact in the preview.
- [ ] Press Continue and confirm the selected save loads.

### Expanded Roster And Recovery

- [ ] With a 4+ player campaign, confirm roster labels compact as First, Second +N.
- [ ] Select a 4+ player campaign row and confirm the preview shows the full roster without clipping.
- [ ] Select an older Unknown party campaign, then re-open the picker and confirm metadata repair updates the row if STS2 exposes roster/progress data.
- [ ] In an existing campaign's loaded-run lobby, change the participant set if possible, press Embark, and confirm the compatibility warning appears once before the next identical Embark attempt proceeds to vanilla validation.
- [ ] With an unmanaged current_run_mp.save fixture applied, choose a host action and confirm the recovery modal offers Duplicate Active Save.
- [ ] With a managed active save changed after activation, choose a different campaign and confirm the recovery modal offers Sync Active Save.
- [ ] Record pass/fail notes below.

## Notes

REPORT

  info "smoke report: $report_path"
}

extract_package_payload() {
  local package_path="$1"
  local staging_root="$2"

  python3 - "$package_path" "$staging_root" <<'PY'
from pathlib import Path, PurePosixPath
import shutil
import sys
import zipfile

zip_path = Path(sys.argv[1])
staging_root = Path(sys.argv[2]).resolve()
package_root = (staging_root / "MultiplayerSaveSlots").resolve()
required = {
    "MultiplayerSaveSlots/MultiplayerSaveSlots.json",
    "MultiplayerSaveSlots/MultiplayerSaveSlots.dll",
}

with zipfile.ZipFile(zip_path, "r") as archive:
    file_names = {item.filename for item in archive.infolist() if not item.is_dir()}
    missing = sorted(required - file_names)
    if missing:
        print(f"missing package entries: {', '.join(missing)}", file=sys.stderr)
        sys.exit(1)

    for item in archive.infolist():
        raw_name = item.filename
        entry_path = PurePosixPath(raw_name)
        if (
            "\\" in raw_name
            or ":" in raw_name
            or entry_path.is_absolute()
            or ".." in entry_path.parts
        ):
            print(f"unsafe package entry: {raw_name}", file=sys.stderr)
            sys.exit(1)

        if not raw_name.startswith("MultiplayerSaveSlots/"):
            continue

        if not entry_path.parts or entry_path.parts[0] != "MultiplayerSaveSlots":
            print(f"unsafe package entry: {raw_name}", file=sys.stderr)
            sys.exit(1)

        target = staging_root.joinpath(*entry_path.parts).resolve()
        if target != package_root and package_root not in target.parents:
            print(f"unsafe package entry: {raw_name}", file=sys.stderr)
            sys.exit(1)

        if item.is_dir():
            target.mkdir(parents=True, exist_ok=True)
            continue

        target.parent.mkdir(parents=True, exist_ok=True)
        with archive.open(item, "r") as source, target.open("wb") as destination:
            shutil.copyfileobj(source, destination)
PY
}

install_command() {
  local tag=""
  local package_path=""
  local sts2_path=""
  local artifacts_root="artifacts/smoke"

  while [[ $# -gt 0 ]]; do
    case "$1" in
      --tag) require_value "$1" "${2-}"; tag="$2"; shift 2 ;;
      --package) require_value "$1" "${2-}"; package_path="$2"; shift 2 ;;
      --sts2-path) require_value "$1" "${2-}"; sts2_path="$2"; shift 2 ;;
      --artifacts-root) require_value "$1" "${2-}"; artifacts_root="$2"; shift 2 ;;
      *) die "unknown install argument: $1" ;;
    esac
  done

  validate_tag "$tag" "install"
  [[ -n "$artifacts_root" ]] || die "artifacts root is empty"

  if [[ -z "$package_path" ]]; then
    package_path="artifacts/release/MultiplayerSaveSlots-$tag.zip"
  fi

  if [[ ! -f "$package_path" ]]; then
    die "missing package: $package_path; run scripts/release-local.sh --package-only $tag"
  fi

  if [[ -z "$sts2_path" ]]; then
    sts2_path="$(default_sts2_path)"
  fi

  local mods_dir="$sts2_path/mods"
  local mod_dir="$mods_dir/MultiplayerSaveSlots"
  local staging_parent
  local install_swap_dir=""
  local live_backup_parent=""

  [[ "$(basename "$mod_dir")" == "MultiplayerSaveSlots" ]] || die "refusing unsafe install target: $mod_dir"

  info "package: $package_path"
  info "STS2 path: $sts2_path"
  info "install target: $mod_dir"

  if [[ -e "$mod_dir" || -L "$mod_dir" ]]; then
    [[ -d "$mod_dir" && ! -L "$mod_dir" ]] || die "existing install target must be a real directory: $mod_dir"
  fi

  staging_parent="$(mktemp -d "${TMPDIR:-/tmp}/smoke-setup-install.XXXXXX")"
  if ! extract_package_payload "$package_path" "$staging_parent"; then
    rm -rf "$staging_parent"
    exit 1
  fi

  local staging_mod_dir="$staging_parent/MultiplayerSaveSlots"
  local staging_manifest="$staging_mod_dir/MultiplayerSaveSlots.json"
  local staging_dll="$staging_mod_dir/MultiplayerSaveSlots.dll"

  if [[ ! -f "$staging_manifest" ]]; then
    rm -rf "$staging_parent"
    die "staged manifest missing: $staging_manifest"
  fi

  if [[ ! -f "$staging_dll" ]]; then
    rm -rf "$staging_parent"
    die "staged DLL missing: $staging_dll"
  fi

  local manifest_version
  if ! manifest_version="$(json_field "$staging_manifest" version)"; then
    best_effort_rm "$staging_parent"
    die "staged manifest is not valid JSON: $staging_manifest"
  fi

  if [[ "$manifest_version" != "${tag#v}" ]]; then
    best_effort_rm "$staging_parent"
    die "staged manifest version '$manifest_version' does not match tag '$tag'"
  fi

  mkdir -p "$mods_dir"

  install_swap_dir="$(mktemp -d "$mods_dir/.MultiplayerSaveSlots.install.XXXXXX")" || {
    rm -rf "$staging_parent"
    die "failed to create install staging directory under: $mods_dir"
  }

  local replacement_dir="$install_swap_dir/MultiplayerSaveSlots"
  if ! cp -a "$staging_mod_dir" "$replacement_dir"; then
    rm -rf "$install_swap_dir" "$staging_parent"
    die "failed to stage replacement install under: $mods_dir"
  fi

  [[ -f "$replacement_dir/MultiplayerSaveSlots.json" ]] || {
    rm -rf "$install_swap_dir" "$staging_parent"
    die "staged replacement manifest missing: $replacement_dir/MultiplayerSaveSlots.json"
  }
  [[ -f "$replacement_dir/MultiplayerSaveSlots.dll" ]] || {
    rm -rf "$install_swap_dir" "$staging_parent"
    die "staged replacement DLL missing: $replacement_dir/MultiplayerSaveSlots.dll"
  }

  local live_backup_dir=""
  if [[ -e "$mod_dir" ]]; then
    live_backup_parent="$(mktemp -d "$mods_dir/.MultiplayerSaveSlots.previous.XXXXXX")" || {
      rm -rf "$install_swap_dir" "$staging_parent"
      die "failed to create install rollback directory under: $mods_dir"
    }
    live_backup_dir="$live_backup_parent/MultiplayerSaveSlots"

    if ! mv "$mod_dir" "$live_backup_dir"; then
      rm -rf "$install_swap_dir" "$live_backup_parent" "$staging_parent"
      die "failed to move existing install aside: $mod_dir"
    fi
  fi

  if ! mv "$replacement_dir" "$mod_dir"; then
    best_effort_rm "$mod_dir"

    if [[ -n "$live_backup_dir" && -e "$live_backup_dir" ]]; then
      if ! mv "$live_backup_dir" "$mod_dir"; then
        best_effort_rm "$install_swap_dir" "$staging_parent"
        die "failed to move staged install into place and failed to restore previous install from: $live_backup_dir"
      fi
    fi

    best_effort_rm "$install_swap_dir" "$live_backup_parent" "$staging_parent"
    die "failed to move staged install into place"
  fi

  best_effort_rm "$install_swap_dir" "$live_backup_parent" "$staging_parent"

  local manifest="$mod_dir/MultiplayerSaveSlots.json"
  local dll="$mod_dir/MultiplayerSaveSlots.dll"
  [[ -f "$manifest" ]] || die "installed manifest missing: $manifest"
  [[ -f "$dll" ]] || die "installed DLL missing: $dll"

  info "installed manifest version: $manifest_version"
  info "installed DLL: $dll"
}

capture_fixture_command() {
  local fixture=""
  local active_save_path=""
  local artifacts_root="artifacts/smoke"

  while [[ $# -gt 0 ]]; do
    case "$1" in
      --name) require_value "$1" "${2-}"; fixture="$2"; shift 2 ;;
      --active-save-path) require_value "$1" "${2-}"; active_save_path="$2"; shift 2 ;;
      --artifacts-root) require_value "$1" "${2-}"; artifacts_root="$2"; shift 2 ;;
      *) die "unknown capture-fixture argument: $1" ;;
    esac
  done

  validate_fixture_name "$fixture"
  [[ -n "$active_save_path" ]] || die "capture-fixture requires --active-save-path"
  active_save_path="$(absolute_path "$active_save_path")"
  validate_active_save_path "$active_save_path"
  if [[ -e "$active_save_path" || -L "$active_save_path" ]]; then
    [[ -f "$active_save_path" && ! -L "$active_save_path" ]] || die "active save path must be a real file: $active_save_path"
  else
    die "active save missing: $active_save_path"
  fi
  require_artifacts_root "$artifacts_root"

  local save_dir
  save_dir="$(dirname "$active_save_path")"
  local bank_dir="$save_dir/MultiplayerSaveSlots"
  local fixtures_root="$artifacts_root/fixtures"
  local fixture_dir="$artifacts_root/fixtures/$fixture"
  local capture_stage_dir=""

  if [[ -e "$bank_dir" || -L "$bank_dir" ]]; then
    [[ -d "$bank_dir" && ! -L "$bank_dir" ]] || die "live bank path must be a real directory: $bank_dir"
    if ! reject_symlinks_in_tree "$bank_dir"; then
      die "live bank contains symlink: $bank_dir"
    fi
  fi

  reject_fixture_namespace_symlinks "$fixture_dir"
  [[ ! -e "$fixture_dir" ]] || die "fixture already exists: $fixture_dir"

  mkdir -p "$fixtures_root"
  capture_stage_dir="$(mktemp -d "$fixtures_root/.${fixture}.capture.XXXXXX")" || die "failed to create capture staging directory under: $fixtures_root"

  mkdir -p "$capture_stage_dir/active"
  if ! cp "$active_save_path" "$capture_stage_dir/active/current_run_mp.save"; then
    rm -rf "$capture_stage_dir"
    die "failed to copy active save into fixture"
  fi

  if [[ -d "$bank_dir" ]]; then
    mkdir -p "$capture_stage_dir/bank"
    if ! cp -a "$bank_dir" "$capture_stage_dir/bank/MultiplayerSaveSlots"; then
      rm -rf "$capture_stage_dir"
      die "failed to copy fixture bank"
    fi
  fi

  if ! mv "$capture_stage_dir" "$fixture_dir"; then
    rm -rf "$capture_stage_dir"
    die "failed to move captured fixture into place"
  fi

  info "captured fixture: $fixture_dir"
}

apply_fixture_command() {
  local fixture=""
  local active_save_path=""
  local artifacts_root="artifacts/smoke"

  while [[ $# -gt 0 ]]; do
    case "$1" in
      --name) require_value "$1" "${2-}"; fixture="$2"; shift 2 ;;
      --active-save-path) require_value "$1" "${2-}"; active_save_path="$2"; shift 2 ;;
      --artifacts-root) require_value "$1" "${2-}"; artifacts_root="$2"; shift 2 ;;
      *) die "unknown apply-fixture argument: $1" ;;
    esac
  done

  validate_fixture_name "$fixture"
  [[ -n "$active_save_path" ]] || die "apply-fixture requires --active-save-path"
  active_save_path="$(absolute_path "$active_save_path")"
  validate_active_save_path "$active_save_path"
  require_artifacts_root "$artifacts_root"

  local save_dir
  save_dir="$(dirname "$active_save_path")"
  local bank_dir="$save_dir/MultiplayerSaveSlots"
  local fixture_dir="$artifacts_root/fixtures/$fixture"
  local fixture_active_root="$fixture_dir/active"
  local fixture_active="$fixture_dir/active/current_run_mp.save"
  local fixture_bank_root="$fixture_dir/bank"
  local fixture_bank="$fixture_dir/bank/MultiplayerSaveSlots"
  local has_fixture_bank=false
  local active_stage_parent=""
  local active_stage=""
  local bank_stage_parent=""
  local moved_bank_parent=""
  local staged_bank=""

  [[ -d "$fixture_dir" && ! -L "$fixture_dir" ]] || die "fixture path must be inside a real fixture directory: $fixture_dir"
  [[ -d "$fixture_active_root" && ! -L "$fixture_active_root" ]] || die "fixture path must be inside a real fixture directory: $fixture_active_root"
  reject_fixture_namespace_symlinks "$fixture_active" parents-only
  [[ -f "$fixture_active" && ! -L "$fixture_active" ]] || die "fixture active save must be a real file: $fixture_active"

  if [[ -e "$active_save_path" || -L "$active_save_path" ]]; then
    [[ -f "$active_save_path" && ! -L "$active_save_path" ]] || die "active save path must be a real file: $active_save_path"
  fi

  if [[ -e "$fixture_bank_root" || -L "$fixture_bank_root" ]]; then
    [[ -d "$fixture_bank_root" && ! -L "$fixture_bank_root" ]] || die "malformed fixture bank: fixture bank must be a real directory: $fixture_bank_root"
    [[ -d "$fixture_bank" && ! -L "$fixture_bank" ]] || die "malformed fixture bank: fixture bank must be a real directory: $fixture_bank"
    reject_fixture_namespace_symlinks "$fixture_bank" parents-only
    if ! reject_symlinks_in_tree "$fixture_bank"; then
      die "fixture bank contains symlink: $fixture_bank"
    fi
    has_fixture_bank=true
  fi

  if [[ -e "$bank_dir" || -L "$bank_dir" ]]; then
    [[ -d "$bank_dir" && ! -L "$bank_dir" ]] || die "live bank path must be a real directory: $bank_dir"
  fi

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

  if [[ "$has_fixture_bank" == true ]]; then
    bank_stage_parent="$(mktemp -d "$save_dir/.MultiplayerSaveSlots.apply.XXXXXX")" || die "failed to create bank staging directory under: $save_dir"
    staged_bank="$bank_stage_parent/MultiplayerSaveSlots"

    if ! cp -a "$fixture_bank" "$staged_bank"; then
      rm -rf "$bank_stage_parent"
      die "failed to stage fixture bank: $fixture_bank"
    fi
  fi

  if ! active_stage_parent="$(mktemp -d "$save_dir/.current_run_mp.save.apply.XXXXXX")"; then
    best_effort_rm "$bank_stage_parent"
    die "failed to create active save staging directory under: $save_dir"
  fi
  active_stage="$active_stage_parent/current_run_mp.save"

  if ! cp "$fixture_active" "$active_stage"; then
    best_effort_rm "$active_stage_parent" "$bank_stage_parent"
    die "failed to copy fixture active save into staging"
  fi

  if ! mv "$active_stage" "$active_save_path"; then
    best_effort_rm "$active_stage_parent" "$bank_stage_parent"
    die "failed to move staged active save into place"
  fi

  if [[ -d "$bank_dir" ]]; then
    moved_bank_parent="$(mktemp -d "$save_dir/.MultiplayerSaveSlots.previous.XXXXXX")" || rollback_apply_failure "failed to create bank rollback directory under: $save_dir" "$active_save_path" "$bank_dir" "$backup_dir" "$bank_stage_parent" "$moved_bank_parent" "$active_stage_parent"

    if ! mv "$bank_dir" "$moved_bank_parent/MultiplayerSaveSlots"; then
      rollback_apply_failure "failed to move existing bank aside: $bank_dir" "$active_save_path" "$bank_dir" "$backup_dir" "$bank_stage_parent" "$moved_bank_parent" "$active_stage_parent"
    fi
  fi

  if [[ "$has_fixture_bank" == true ]]; then
    if ! mv "$staged_bank" "$bank_dir"; then
      rollback_apply_failure "failed to move staged bank into place" "$active_save_path" "$bank_dir" "$backup_dir" "$bank_stage_parent" "$moved_bank_parent" "$active_stage_parent"
    fi
  fi

  best_effort_rm "$active_stage_parent" "$bank_stage_parent" "$moved_bank_parent"
  write_report "$artifacts_root" "$fixture" "$active_save_path" "$backup_dir" "not provided" "not provided"
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
      --fixture) require_value "$1" "${2-}"; fixture="$2"; shift 2 ;;
      --active-save-path) require_value "$1" "${2-}"; active_save_path="$2"; shift 2 ;;
      --tag) require_value "$1" "${2-}"; tag="$2"; shift 2 ;;
      --sts2-path) require_value "$1" "${2-}"; sts2_path="$2"; shift 2 ;;
      --artifacts-root) require_value "$1" "${2-}"; artifacts_root="$2"; shift 2 ;;
      *) die "unknown report argument: $1" ;;
    esac
  done

  validate_fixture_name "$fixture"
  if [[ -n "$tag" ]]; then
    validate_tag "$tag" "report"
  fi
  if [[ -n "$active_save_path" ]]; then
    active_save_path="$(absolute_path "$active_save_path")"
  fi
  require_artifacts_root "$artifacts_root"

  local backup_path
  backup_path="$(latest_backup_for_fixture "$artifacts_root" "$fixture")"

  write_report "$artifacts_root" "$fixture" "$active_save_path" "$backup_path" "$tag" "$sts2_path"
}

main() {
  local command="${1:-}"
  [[ -n "$command" ]] || { usage >&2; exit 2; }

  case "$command" in
    -h|--help|help)
      usage
      return 0
      ;;
  esac

  need_command git
  need_command python3

  local root
  root="$(repo_root)"
  cd "$root"

  shift
  case "$command" in
    install) install_command "$@" ;;
    capture-fixture) capture_fixture_command "$@" ;;
    apply-fixture) apply_fixture_command "$@" ;;
    report) report_command "$@" ;;
    *) die "unknown command: $command" ;;
  esac
}

main "$@"
