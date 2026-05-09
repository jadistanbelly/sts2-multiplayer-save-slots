#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd -P)"
script="$repo_root/scripts/smoke-setup-local.sh"

pass_count=0
tmp_dirs=()

new_tmp_dir() {
  local __name="$1"
  local __tmp
  __tmp="$(mktemp -d "${TMPDIR:-/tmp}/smoke-setup-test.XXXXXX")"
  tmp_dirs+=("$__tmp")
  printf -v "$__name" '%s' "$__tmp"
}

cleanup_tmp_dirs() {
  [[ "${KEEP_TMP:-0}" == "1" ]] && return 0

  local dir
  for dir in "${tmp_dirs[@]}"; do
    rm -rf "$dir"
  done
}

trap cleanup_tmp_dirs EXIT

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

assert_contains() {
  local path="$1"
  local text="$2"
  grep -Fq "$text" "$path" || fail "expected $path to contain $text"
}

make_package() {
  local zip_path="$1"
  local version="${2:-9.9.9}"
  python3 - "$zip_path" "$version" <<'PY'
from pathlib import Path
import sys
import zipfile

zip_path = Path(sys.argv[1])
version = sys.argv[2]
zip_path.parent.mkdir(parents=True, exist_ok=True)
manifest = f"""{{
  "id": "MultiplayerSaveSlots",
  "name": "Multiplayer Save Slots",
  "author": "test",
  "description": "test package",
  "version": "{version}",
  "has_pck": false,
  "has_dll": true,
  "dependencies": [],
  "affects_gameplay": false
}}
"""

with zipfile.ZipFile(zip_path, "w", zipfile.ZIP_DEFLATED) as archive:
    archive.writestr("MultiplayerSaveSlots/MultiplayerSaveSlots.json", manifest)
    archive.writestr("MultiplayerSaveSlots/MultiplayerSaveSlots.dll", "fake dll")
PY
}

make_bad_package_missing_dll() {
  local zip_path="$1"
  python3 - "$zip_path" <<'PY'
from pathlib import Path
import sys
import zipfile

zip_path = Path(sys.argv[1])
zip_path.parent.mkdir(parents=True, exist_ok=True)

with zipfile.ZipFile(zip_path, "w", zipfile.ZIP_DEFLATED) as archive:
    archive.writestr("MultiplayerSaveSlots/MultiplayerSaveSlots.json", '{"version":"9.9.9"}\n')
PY
}

make_malicious_package() {
  local zip_path="$1"
  local unsafe_entry="$2"
  python3 - "$zip_path" "$unsafe_entry" <<'PY'
from pathlib import Path
import sys
import zipfile

zip_path = Path(sys.argv[1])
unsafe_entry = sys.argv[2]
zip_path.parent.mkdir(parents=True, exist_ok=True)
manifest = '{"version":"9.9.9"}\n'

with zipfile.ZipFile(zip_path, "w", zipfile.ZIP_DEFLATED) as archive:
    archive.writestr("MultiplayerSaveSlots/MultiplayerSaveSlots.json", manifest)
    archive.writestr("MultiplayerSaveSlots/MultiplayerSaveSlots.dll", "fake dll")
    archive.writestr(unsafe_entry, "unsafe payload")
PY
}

assert_install_rejects_and_preserves() {
  local package="$1"
  local sts2="$2"
  local smoke="$3"
  local out="$4"
  local err="$5"

  if "$script" install --tag v9.9.9 --package "$package" --sts2-path "$sts2" --artifacts-root "$smoke" >"$out" 2>"$err"; then
    fail "malicious package unexpectedly succeeded: $package"
  fi

  assert_contains "$err" "unsafe package entry"
  grep -Fq 'known good manifest' "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.json" || fail "malicious package removed existing manifest"
  grep -Fq 'known good dll' "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.dll" || fail "malicious package removed existing DLL"
}

test_install_package() {
  local tmp
  new_tmp_dir tmp
  local package="$tmp/MultiplayerSaveSlots-v9.9.9.zip"
  local bad_package="$tmp/bad.zip"
  local mismatched_package="$tmp/mismatch.zip"
  local backslash_package="$tmp/backslash.zip"
  local drive_package="$tmp/drive.zip"
  local absolute_package="$tmp/absolute.zip"
  local traversal_package="$tmp/traversal.zip"
  local sts2="$tmp/sts2"
  local smoke="$tmp/smoke"
  local out="$tmp/install.out"
  local bad_out="$tmp/bad.out"
  local bad_err="$tmp/bad.err"
  local mismatch_out="$tmp/mismatch.out"
  local mismatch_err="$tmp/mismatch.err"
  local fake_bin="$tmp/bin"
  local final_move_out="$tmp/final-move.out"
  local final_move_err="$tmp/final-move.err"

  make_package "$package"
  make_package "$mismatched_package" "1.2.3"
  make_bad_package_missing_dll "$bad_package"
  make_malicious_package "$backslash_package" "MultiplayerSaveSlots/..\\..\\evil.txt"
  make_malicious_package "$drive_package" "MultiplayerSaveSlots/C:\\evil.txt"
  make_malicious_package "$absolute_package" "/absolute.txt"
  make_malicious_package "$traversal_package" "MultiplayerSaveSlots/../evil.txt"

  "$script" install --tag v9.9.9 --package "$package" --sts2-path "$sts2" --artifacts-root "$smoke" >"$out"

  assert_file "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.json"
  assert_file "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.dll"
  assert_contains "$out" "installed manifest version: 9.9.9"

  printf 'known good manifest\n' > "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.json"
  printf 'known good dll\n' > "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.dll"

  if "$script" install --tag v9.9.9 --package "$bad_package" --sts2-path "$sts2" --artifacts-root "$smoke" >"$bad_out" 2>"$bad_err"; then
    fail "bad package unexpectedly succeeded"
  fi

  assert_contains "$bad_err" "missing package entries"
  grep -Fq 'known good manifest' "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.json" || fail "bad package removed existing manifest"
  grep -Fq 'known good dll' "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.dll" || fail "bad package removed existing DLL"

  if "$script" install --tag v9.9.9 --package "$mismatched_package" --sts2-path "$sts2" --artifacts-root "$smoke" >"$mismatch_out" 2>"$mismatch_err"; then
    fail "mismatched version package unexpectedly succeeded"
  fi

  assert_contains "$mismatch_err" "manifest version"
  grep -Fq 'known good manifest' "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.json" || fail "mismatched package removed existing manifest"
  grep -Fq 'known good dll' "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.dll" || fail "mismatched package removed existing DLL"

  local install_file_tmp
  new_tmp_dir install_file_tmp
  local install_file_sts2="$install_file_tmp/sts2"
  local install_file_out="$install_file_tmp/file.out"
  local install_file_err="$install_file_tmp/file.err"
  mkdir -p "$install_file_sts2/mods"
  printf 'existing install target file\n' > "$install_file_sts2/mods/MultiplayerSaveSlots"

  if "$script" install --tag v9.9.9 --package "$package" --sts2-path "$install_file_sts2" --artifacts-root "$install_file_tmp/smoke" >"$install_file_out" 2>"$install_file_err"; then
    fail "install with existing target file unexpectedly succeeded"
  fi

  assert_contains "$install_file_err" "existing install target must be a real directory"
  grep -Fq 'existing install target file' "$install_file_sts2/mods/MultiplayerSaveSlots" || fail "install target file was removed or replaced"

  local install_symlink_tmp
  new_tmp_dir install_symlink_tmp
  local install_symlink_sts2="$install_symlink_tmp/sts2"
  local install_symlink_out="$install_symlink_tmp/symlink.out"
  local install_symlink_err="$install_symlink_tmp/symlink.err"
  mkdir -p "$install_symlink_sts2/mods" "$install_symlink_tmp/target-install"
  printf 'symlink target manifest\n' > "$install_symlink_tmp/target-install/MultiplayerSaveSlots.json"
  ln -s "$install_symlink_tmp/target-install" "$install_symlink_sts2/mods/MultiplayerSaveSlots"

  if "$script" install --tag v9.9.9 --package "$package" --sts2-path "$install_symlink_sts2" --artifacts-root "$install_symlink_tmp/smoke" >"$install_symlink_out" 2>"$install_symlink_err"; then
    fail "install with existing target symlink unexpectedly succeeded"
  fi

  assert_contains "$install_symlink_err" "existing install target must be a real directory"
  [[ -L "$install_symlink_sts2/mods/MultiplayerSaveSlots" ]] || fail "install target symlink was removed or replaced"
  grep -Fq 'symlink target manifest' "$install_symlink_tmp/target-install/MultiplayerSaveSlots.json" || fail "install target symlink destination was mutated"

  local install_broken_tmp
  new_tmp_dir install_broken_tmp
  local install_broken_sts2="$install_broken_tmp/sts2"
  local install_broken_out="$install_broken_tmp/broken.out"
  local install_broken_err="$install_broken_tmp/broken.err"
  mkdir -p "$install_broken_sts2/mods"
  ln -s "$install_broken_tmp/missing-install-target" "$install_broken_sts2/mods/MultiplayerSaveSlots"

  if "$script" install --tag v9.9.9 --package "$package" --sts2-path "$install_broken_sts2" --artifacts-root "$install_broken_tmp/smoke" >"$install_broken_out" 2>"$install_broken_err"; then
    fail "install with existing broken target symlink unexpectedly succeeded"
  fi

  assert_contains "$install_broken_err" "existing install target must be a real directory"
  [[ -L "$install_broken_sts2/mods/MultiplayerSaveSlots" ]] || fail "broken install target symlink was removed or replaced"

  assert_install_rejects_and_preserves "$backslash_package" "$sts2" "$smoke" "$tmp/backslash.out" "$tmp/backslash.err"
  assert_install_rejects_and_preserves "$drive_package" "$sts2" "$smoke" "$tmp/drive.out" "$tmp/drive.err"
  assert_install_rejects_and_preserves "$absolute_package" "$sts2" "$smoke" "$tmp/absolute.out" "$tmp/absolute.err"
  assert_install_rejects_and_preserves "$traversal_package" "$sts2" "$smoke" "$tmp/traversal.out" "$tmp/traversal.err"

  [[ ! -e "$sts2/evil.txt" ]] || fail "malicious package wrote outside STS2 mod directory"
  [[ ! -e "$sts2/mods/evil.txt" ]] || fail "malicious package wrote outside MultiplayerSaveSlots mod directory"
  [[ ! -e "$sts2/mods/absolute.txt" ]] || fail "malicious package wrote absolute entry under mods"

  mkdir -p "$fake_bin"
  local real_mv
  real_mv="$(command -v mv)"
  local real_rm
  real_rm="$(command -v rm)"
  cat > "$fake_bin/mv" <<SH
#!/usr/bin/env bash
if [[ "\${1-}" == *".MultiplayerSaveSlots.install."*"/MultiplayerSaveSlots" && "\${2-}" == "$sts2/mods/MultiplayerSaveSlots" ]]; then
  printf 'simulated final install move failure\n' >&2
  exit 42
fi

exec "$real_mv" "\$@"
SH
  chmod +x "$fake_bin/mv"
  cat > "$fake_bin/rm" <<SH
#!/usr/bin/env bash
if [[ "\${1-}" == "-rf" && "\${2-}" == "$sts2/mods/MultiplayerSaveSlots" ]]; then
  printf 'simulated install cleanup failure\n' >&2
  exit 46
fi

exec "$real_rm" "\$@"
SH
  chmod +x "$fake_bin/rm"

  if PATH="$fake_bin:$PATH" "$script" install --tag v9.9.9 --package "$package" --sts2-path "$sts2" --artifacts-root "$smoke" >"$final_move_out" 2>"$final_move_err"; then
    fail "install with final move failure unexpectedly succeeded"
  fi

  assert_contains "$final_move_err" "failed to move staged install into place"
  grep -Fq 'known good manifest' "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.json" || fail "final move failure removed existing manifest"
  grep -Fq 'known good dll' "$sts2/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.dll" || fail "final move failure removed existing DLL"
  pass "install copies package payload"
}

test_missing_package_fails() {
  local tmp
  new_tmp_dir tmp
  local out="$tmp/missing.out"
  local err="$tmp/missing.err"

  if "$script" install --tag v9.9.8 --sts2-path "$tmp/sts2" --artifacts-root "$tmp/smoke" >"$out" 2>"$err"; then
    fail "missing package unexpectedly succeeded"
  fi

  assert_contains "$err" "run scripts/release-local.sh --package-only v9.9.8"
  pass "missing package explains release command"
}

test_invalid_fixture_name_fails() {
  local tmp
  new_tmp_dir tmp
  local active="$tmp/profile/current_run_mp.save"
  mkdir -p "$(dirname "$active")"
  printf 'active save\n' > "$active"

  local invalid_name
  local invalid_index=0
  for invalid_name in ../bad . .. CON prn.txt COM1 LPT9 bad.; do
    local out="$tmp/invalid-$invalid_index.out"
    local err="$tmp/invalid-$invalid_index.err"
    if "$script" capture-fixture --name "$invalid_name" --active-save-path "$active" --artifacts-root "$tmp/smoke-$invalid_index" >"$out" 2>"$err"; then
      fail "invalid fixture name $invalid_name unexpectedly succeeded"
    fi

    assert_contains "$err" "fixture name must match"
    invalid_index=$((invalid_index + 1))
  done

  local wrong_active="$tmp/profile/not_current.save"
  local wrong_out="$tmp/wrong-active.out"
  local wrong_err="$tmp/wrong-active.err"
  printf 'wrong active save\n' > "$wrong_active"

  if "$script" capture-fixture --name valid-name --active-save-path "$wrong_active" --artifacts-root "$tmp/smoke-wrong-active" >"$wrong_out" 2>"$wrong_err"; then
    fail "capture with invalid active save filename unexpectedly succeeded"
  fi

  assert_contains "$wrong_err" "active save path basename must be current_run_mp.save"
  pass "invalid fixture name rejected"
}

test_capture_fixture_copies_active_and_bank() {
  local tmp
  new_tmp_dir tmp
  local active="$tmp/profile/current_run_mp.save"
  local bank="$tmp/profile/MultiplayerSaveSlots"
  local smoke="$tmp/smoke"
  local out="$tmp/capture.out"

  mkdir -p "$bank/saves/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
  printf 'active save\n' > "$active"
  printf '{"campaignIds":["aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"]}\n' > "$bank/index.json"
  printf 'bank payload\n' > "$bank/saves/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/multiplayer_run.save"

  "$script" capture-fixture --name managed-active --active-save-path "$active" --artifacts-root "$smoke" >"$out"

  assert_file "$smoke/fixtures/managed-active/active/current_run_mp.save"
  assert_file "$smoke/fixtures/managed-active/bank/MultiplayerSaveSlots/index.json"
  assert_file "$smoke/fixtures/managed-active/bank/MultiplayerSaveSlots/saves/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/multiplayer_run.save"

  local malformed_tmp
  new_tmp_dir malformed_tmp
  local malformed_active="$malformed_tmp/profile/current_run_mp.save"
  local malformed_smoke="$malformed_tmp/smoke"
  local malformed_bank="$malformed_tmp/profile/MultiplayerSaveSlots"
  local malformed_target="$malformed_tmp/bank-target"
  local malformed_out="$malformed_tmp/malformed.out"
  local malformed_err="$malformed_tmp/malformed.err"

  mkdir -p "$(dirname "$malformed_active")"
  printf 'active save\n' > "$malformed_active"
  printf 'bank path as file\n' > "$malformed_bank"

  if "$script" capture-fixture --name bank-file --active-save-path "$malformed_active" --artifacts-root "$malformed_smoke" >"$malformed_out" 2>"$malformed_err"; then
    fail "capture with live bank file unexpectedly succeeded"
  fi

  assert_contains "$malformed_err" "live bank path must be a real directory"
  grep -Fq 'bank path as file' "$malformed_bank" || fail "capture changed live bank file"
  [[ ! -e "$malformed_smoke/fixtures/bank-file" ]] || fail "capture with live bank file created final fixture"

  rm -f "$malformed_bank"
  mkdir -p "$malformed_target"
  printf 'target bank\n' > "$malformed_target/index.json"
  ln -s "$malformed_target" "$malformed_bank"

  if "$script" capture-fixture --name bank-symlink --active-save-path "$malformed_active" --artifacts-root "$malformed_smoke" >"$malformed_out" 2>"$malformed_err"; then
    fail "capture with live bank symlink unexpectedly succeeded"
  fi

  assert_contains "$malformed_err" "live bank path must be a real directory"
  [[ -L "$malformed_bank" ]] || fail "capture changed live bank symlink"
  [[ ! -e "$malformed_smoke/fixtures/bank-symlink" ]] || fail "capture with live bank symlink created final fixture"

  rm -f "$malformed_bank"
  ln -s "$malformed_tmp/missing-bank-target" "$malformed_bank"

  if "$script" capture-fixture --name bank-broken-symlink --active-save-path "$malformed_active" --artifacts-root "$malformed_smoke" >"$malformed_out" 2>"$malformed_err"; then
    fail "capture with broken live bank symlink unexpectedly succeeded"
  fi

  assert_contains "$malformed_err" "live bank path must be a real directory"
  [[ -L "$malformed_bank" ]] || fail "capture changed broken live bank symlink"
  [[ ! -e "$malformed_smoke/fixtures/bank-broken-symlink" ]] || fail "capture with broken live bank symlink created final fixture"

  rm -f "$malformed_bank"
  mkdir -p "$malformed_bank"
  ln -s "$malformed_active" "$malformed_bank/index.json"

  if "$script" capture-fixture --name bank-inner-symlink --active-save-path "$malformed_active" --artifacts-root "$malformed_smoke" >"$malformed_out" 2>"$malformed_err"; then
    fail "capture with live bank tree symlink unexpectedly succeeded"
  fi

  assert_contains "$malformed_err" "live bank contains symlink"
  [[ -L "$malformed_bank/index.json" ]] || fail "capture changed live bank tree symlink"
  [[ ! -e "$malformed_smoke/fixtures/bank-inner-symlink" ]] || fail "capture with live bank tree symlink created final fixture"

  local active_symlink_tmp
  new_tmp_dir active_symlink_tmp
  local active_symlink_active="$active_symlink_tmp/profile/current_run_mp.save"
  local active_symlink_target="$active_symlink_tmp/target-current_run_mp.save"
  local active_symlink_smoke="$active_symlink_tmp/smoke"
  local active_symlink_out="$active_symlink_tmp/active-symlink.out"
  local active_symlink_err="$active_symlink_tmp/active-symlink.err"

  mkdir -p "$(dirname "$active_symlink_active")"
  printf 'capture active symlink target\n' > "$active_symlink_target"
  ln -s "$active_symlink_target" "$active_symlink_active"

  if "$script" capture-fixture --name active-symlink --active-save-path "$active_symlink_active" --artifacts-root "$active_symlink_smoke" >"$active_symlink_out" 2>"$active_symlink_err"; then
    fail "capture with active save symlink unexpectedly succeeded"
  fi

  assert_contains "$active_symlink_err" "active save path must be a real file"
  [[ -L "$active_symlink_active" ]] || fail "capture changed active save symlink"
  grep -Fq 'capture active symlink target' "$active_symlink_target" || fail "capture changed active save symlink target"
  [[ ! -e "$active_symlink_smoke/fixtures/active-symlink" ]] || fail "capture with active save symlink created final fixture"

  local namespace_tmp
  new_tmp_dir namespace_tmp
  local namespace_active="$namespace_tmp/profile/current_run_mp.save"
  local namespace_bank="$namespace_tmp/profile/MultiplayerSaveSlots"
  local namespace_smoke="$namespace_tmp/smoke"
  local namespace_external="$namespace_tmp/external-fixtures"
  local namespace_out="$namespace_tmp/namespace.out"
  local namespace_err="$namespace_tmp/namespace.err"

  mkdir -p "$(dirname "$namespace_active")" "$namespace_bank" "$namespace_smoke" "$namespace_external"
  printf 'namespace active before capture\n' > "$namespace_active"
  printf 'namespace bank before capture\n' > "$namespace_bank/index.json"
  ln -s "$namespace_external" "$namespace_smoke/fixtures"

  if "$script" capture-fixture --name namespace-link --active-save-path "$namespace_active" --artifacts-root "$namespace_smoke" >"$namespace_out" 2>"$namespace_err"; then
    fail "capture with symlinked fixtures root unexpectedly succeeded"
  fi

  assert_contains "$namespace_err" "fixture namespace path must not contain symlinks"
  grep -Fq 'namespace active before capture' "$namespace_active" || fail "capture namespace symlink changed active save"
  grep -Fq 'namespace bank before capture' "$namespace_bank/index.json" || fail "capture namespace symlink changed bank"
  [[ -L "$namespace_smoke/fixtures" ]] || fail "capture namespace symlink was removed or replaced"
  [[ ! -e "$namespace_external/namespace-link" ]] || fail "capture with symlinked fixtures root created external fixture"

  local fail_tmp
  new_tmp_dir fail_tmp
  local fail_active="$fail_tmp/profile/current_run_mp.save"
  local fail_bank="$fail_tmp/profile/MultiplayerSaveSlots"
  local fail_smoke="$fail_tmp/smoke"
  local fail_bin="$fail_tmp/bin"
  local fail_out="$fail_tmp/fail.out"
  local fail_err="$fail_tmp/fail.err"

  mkdir -p "$fail_bank" "$fail_bin"
  printf 'active save\n' > "$fail_active"
  printf 'bank payload\n' > "$fail_bank/index.json"

  local real_cp
  real_cp="$(command -v cp)"
  cat > "$fail_bin/cp" <<SH
#!/usr/bin/env bash
if [[ "\${1-}" == "-a" && "\${2-}" == "$fail_bank" ]]; then
  printf 'simulated bank capture copy failure\n' >&2
  exit 44
fi

exec "$real_cp" "\$@"
SH
  chmod +x "$fail_bin/cp"

  if PATH="$fail_bin:$PATH" "$script" capture-fixture --name partial-capture --active-save-path "$fail_active" --artifacts-root "$fail_smoke" >"$fail_out" 2>"$fail_err"; then
    fail "capture with failing bank copy unexpectedly succeeded"
  fi

  assert_contains "$fail_err" "failed to copy fixture bank"
  [[ ! -e "$fail_smoke/fixtures/partial-capture" ]] || fail "failed capture left final fixture directory"
  pass "capture fixture copies active save and bank"
}

test_apply_fixture_backs_up_before_mutation() {
  local tmp
  new_tmp_dir tmp
  local active="$tmp/profile/current_run_mp.save"
  local bank="$tmp/profile/MultiplayerSaveSlots"
  local smoke="$tmp/smoke"
  local fixture="$smoke/fixtures/unmanaged-active"
  local out="$tmp/apply.out"

  mkdir -p "$bank" "$fixture/active"
  printf 'old active\n' > "$active"
  printf 'old index\n' > "$bank/index.json"
  printf 'fixture active\n' > "$fixture/active/current_run_mp.save"

  "$script" apply-fixture --name unmanaged-active --active-save-path "$active" --artifacts-root "$smoke" >"$out"

  grep -Fq 'fixture active' "$active" || fail "active save was not replaced by fixture"
  [[ ! -e "$bank/index.json" ]] || fail "bank state should be removed when fixture has no bank"
  local backup_count
  backup_count="$(find "$smoke/backups/unmanaged-active" -mindepth 1 -maxdepth 1 -type d | wc -l)"
  [[ "$backup_count" -eq 1 ]] || fail "expected one backup directory, got $backup_count"
  local backup_dir
  backup_dir="$(find "$smoke/backups/unmanaged-active" -mindepth 1 -maxdepth 1 -type d | head -n 1)"
  assert_file "$backup_dir/current_run_mp.save"
  assert_file "$backup_dir/MultiplayerSaveSlots/index.json"
  grep -Fq 'old active' "$backup_dir/current_run_mp.save" || fail "backup active save did not preserve pre-mutation content"
  grep -Fq 'old index' "$backup_dir/MultiplayerSaveSlots/index.json" || fail "backup bank did not preserve pre-mutation content"

  local no_bank_rm_tmp
  new_tmp_dir no_bank_rm_tmp
  local no_bank_rm_active="$no_bank_rm_tmp/profile/current_run_mp.save"
  local no_bank_rm_bank="$no_bank_rm_tmp/profile/MultiplayerSaveSlots"
  local no_bank_rm_smoke="$no_bank_rm_tmp/smoke"
  local no_bank_rm_fixture="$no_bank_rm_smoke/fixtures/unmanaged-active"
  local no_bank_rm_bin="$no_bank_rm_tmp/bin"
  local no_bank_rm_out="$no_bank_rm_tmp/no-bank-rm.out"
  local no_bank_rm_err="$no_bank_rm_tmp/no-bank-rm.err"
  local no_bank_rm_marker="$no_bank_rm_tmp/rm-hit"

  mkdir -p "$no_bank_rm_bank" "$no_bank_rm_fixture/active" "$no_bank_rm_bin"
  printf 'live active before no-bank rm failure\n' > "$no_bank_rm_active"
  printf 'live bank before no-bank rm failure\n' > "$no_bank_rm_bank/index.json"
  printf 'fixture active after no-bank rm failure\n' > "$no_bank_rm_fixture/active/current_run_mp.save"

  local real_rm
  real_rm="$(command -v rm)"
  cat > "$no_bank_rm_bin/rm" <<SH
#!/usr/bin/env bash
if [[ "\${1-}" == "-rf" && "\${2-}" == "$no_bank_rm_bank" ]]; then
  "$real_rm" -f "$no_bank_rm_bank/index.json"
  printf 'hit\n' > "$no_bank_rm_marker"
  printf 'simulated partial live bank removal failure\n' >&2
  exit 49
fi

exec "$real_rm" "\$@"
SH
  chmod +x "$no_bank_rm_bin/rm"

  if PATH="$no_bank_rm_bin:$PATH" "$script" apply-fixture --name unmanaged-active --active-save-path "$no_bank_rm_active" --artifacts-root "$no_bank_rm_smoke" >"$no_bank_rm_out" 2>"$no_bank_rm_err"; then
    [[ ! -e "$no_bank_rm_marker" ]] || fail "no-bank apply removed live bank in place"
    grep -Fq 'fixture active after no-bank rm failure' "$no_bank_rm_active" || fail "no-bank apply did not install fixture active save"
    [[ ! -e "$no_bank_rm_bank" ]] || fail "successful no-bank apply should remove live bank"
  else
    grep -Fq 'live active before no-bank rm failure' "$no_bank_rm_active" || fail "failed no-bank apply did not restore active save"
    grep -Fq 'live bank before no-bank rm failure' "$no_bank_rm_bank/index.json" || fail "failed no-bank apply did not preserve live bank"
    fail "no-bank apply failed instead of moving bank aside"
  fi

  local pre_copy_tmp
  new_tmp_dir pre_copy_tmp
  local pre_copy_active="$pre_copy_tmp/profile/current_run_mp.save"
  local pre_copy_bank="$pre_copy_tmp/profile/MultiplayerSaveSlots"
  local pre_copy_smoke="$pre_copy_tmp/smoke"
  local pre_copy_fixture="$pre_copy_smoke/fixtures/managed-active"
  local pre_copy_bin="$pre_copy_tmp/bin"
  local pre_copy_out="$pre_copy_tmp/pre-copy.out"
  local pre_copy_err="$pre_copy_tmp/pre-copy.err"
  local pre_copy_rm_marker="$pre_copy_tmp/live-bank-rm-hit"

  mkdir -p "$pre_copy_bank" "$pre_copy_fixture/active" "$pre_copy_fixture/bank/MultiplayerSaveSlots" "$pre_copy_bin"
  printf 'live active before pre-copy failure\n' > "$pre_copy_active"
  printf 'live bank before pre-copy failure\n' > "$pre_copy_bank/index.json"
  printf 'fixture active before pre-copy failure\n' > "$pre_copy_fixture/active/current_run_mp.save"
  printf 'fixture bank before pre-copy failure\n' > "$pre_copy_fixture/bank/MultiplayerSaveSlots/index.json"

  local real_cp
  real_cp="$(command -v cp)"
  local real_rm
  real_rm="$(command -v rm)"
  cat > "$pre_copy_bin/cp" <<SH
#!/usr/bin/env bash
if [[ "\${1-}" == "$pre_copy_fixture/active/current_run_mp.save" && "\${2-}" == "$pre_copy_tmp/profile/.current_run_mp.save.apply."*"/current_run_mp.save" ]]; then
  printf 'simulated fixture active staging copy failure\n' >&2
  exit 50
fi

exec "$real_cp" "\$@"
SH
  chmod +x "$pre_copy_bin/cp"
  cat > "$pre_copy_bin/rm" <<SH
#!/usr/bin/env bash
if [[ "\${1-}" == "-rf" && "\${2-}" == "$pre_copy_bank" ]]; then
  printf 'hit\n' > "$pre_copy_rm_marker"
  printf 'unexpected live bank removal during pre-mutation failure\n' >&2
  exit 51
fi

exec "$real_rm" "\$@"
SH
  chmod +x "$pre_copy_bin/rm"

  if PATH="$pre_copy_bin:$PATH" "$script" apply-fixture --name managed-active --active-save-path "$pre_copy_active" --artifacts-root "$pre_copy_smoke" >"$pre_copy_out" 2>"$pre_copy_err"; then
    fail "apply with fixture active staging copy failure unexpectedly succeeded"
  fi

  assert_contains "$pre_copy_err" "failed to copy fixture active save into staging"
  grep -Fq 'live active before pre-copy failure' "$pre_copy_active" || fail "pre-mutation active copy failure changed active save"
  grep -Fq 'live bank before pre-copy failure' "$pre_copy_bank/index.json" || fail "pre-mutation active copy failure changed bank"
  [[ ! -e "$pre_copy_rm_marker" ]] || fail "pre-mutation active copy failure attempted live bank removal"
  if compgen -G "$pre_copy_tmp/profile/.MultiplayerSaveSlots.*" >/dev/null || compgen -G "$pre_copy_tmp/profile/.current_run_mp.save.apply.*" >/dev/null; then
    fail "pre-mutation active copy failure left apply temp paths"
  fi

  local active_dir_tmp
  new_tmp_dir active_dir_tmp
  local active_dir_active="$active_dir_tmp/profile/current_run_mp.save"
  local active_dir_bank="$active_dir_tmp/profile/MultiplayerSaveSlots"
  local active_dir_smoke="$active_dir_tmp/smoke"
  local active_dir_fixture="$active_dir_smoke/fixtures/unmanaged-active"
  local active_dir_out="$active_dir_tmp/active-dir.out"
  local active_dir_err="$active_dir_tmp/active-dir.err"

  mkdir -p "$active_dir_active" "$active_dir_bank" "$active_dir_fixture/active"
  printf 'live bank before active dir\n' > "$active_dir_bank/index.json"
  printf 'fixture active for active dir\n' > "$active_dir_fixture/active/current_run_mp.save"

  if "$script" apply-fixture --name unmanaged-active --active-save-path "$active_dir_active" --artifacts-root "$active_dir_smoke" >"$active_dir_out" 2>"$active_dir_err"; then
    fail "apply with active save directory unexpectedly succeeded"
  fi

  assert_contains "$active_dir_err" "active save path must be a real file"
  [[ -d "$active_dir_active" ]] || fail "active save directory was removed or replaced"
  grep -Fq 'live bank before active dir' "$active_dir_bank/index.json" || fail "active save directory case changed bank"
  [[ ! -e "$active_dir_smoke/backups/unmanaged-active" ]] || fail "active save directory case created backup before preflight failed"
  [[ ! -e "$active_dir_smoke/reports" ]] || fail "active save directory case wrote report"

  local active_symlink_tmp
  new_tmp_dir active_symlink_tmp
  local active_symlink_active="$active_symlink_tmp/profile/current_run_mp.save"
  local active_symlink_target="$active_symlink_tmp/target-current_run_mp.save"
  local active_symlink_bank="$active_symlink_tmp/profile/MultiplayerSaveSlots"
  local active_symlink_smoke="$active_symlink_tmp/smoke"
  local active_symlink_fixture="$active_symlink_smoke/fixtures/unmanaged-active"
  local active_symlink_out="$active_symlink_tmp/active-symlink.out"
  local active_symlink_err="$active_symlink_tmp/active-symlink.err"

  mkdir -p "$(dirname "$active_symlink_active")" "$active_symlink_bank" "$active_symlink_fixture/active"
  printf 'active symlink target\n' > "$active_symlink_target"
  ln -s "$active_symlink_target" "$active_symlink_active"
  printf 'live bank before active symlink\n' > "$active_symlink_bank/index.json"
  printf 'fixture active for active symlink\n' > "$active_symlink_fixture/active/current_run_mp.save"

  if "$script" apply-fixture --name unmanaged-active --active-save-path "$active_symlink_active" --artifacts-root "$active_symlink_smoke" >"$active_symlink_out" 2>"$active_symlink_err"; then
    fail "apply with active save symlink unexpectedly succeeded"
  fi

  assert_contains "$active_symlink_err" "active save path must be a real file"
  [[ -L "$active_symlink_active" ]] || fail "active save symlink was removed or replaced"
  grep -Fq 'active symlink target' "$active_symlink_target" || fail "active save symlink target was changed"
  grep -Fq 'live bank before active symlink' "$active_symlink_bank/index.json" || fail "active save symlink case changed bank"
  [[ ! -e "$active_symlink_smoke/backups/unmanaged-active" ]] || fail "active save symlink case created backup before preflight failed"
  [[ ! -e "$active_symlink_smoke/reports" ]] || fail "active save symlink case wrote report"

  local live_file_tmp
  new_tmp_dir live_file_tmp
  local live_file_active="$live_file_tmp/profile/current_run_mp.save"
  local live_file_bank="$live_file_tmp/profile/MultiplayerSaveSlots"
  local live_file_smoke="$live_file_tmp/smoke"
  local live_file_fixture="$live_file_smoke/fixtures/unmanaged-active"
  local live_file_out="$live_file_tmp/live-file.out"
  local live_file_err="$live_file_tmp/live-file.err"

  mkdir -p "$(dirname "$live_file_active")" "$live_file_fixture/active"
  printf 'live active before bank file\n' > "$live_file_active"
  printf 'live bank path as file\n' > "$live_file_bank"
  printf 'fixture active bank file\n' > "$live_file_fixture/active/current_run_mp.save"

  if "$script" apply-fixture --name unmanaged-active --active-save-path "$live_file_active" --artifacts-root "$live_file_smoke" >"$live_file_out" 2>"$live_file_err"; then
    fail "apply with live bank file unexpectedly succeeded"
  fi

  assert_contains "$live_file_err" "live bank path must be a real directory"
  grep -Fq 'live active before bank file' "$live_file_active" || fail "live bank file case changed active save"
  grep -Fq 'live bank path as file' "$live_file_bank" || fail "live bank file was removed or replaced"
  [[ ! -e "$live_file_smoke/backups/unmanaged-active" ]] || fail "live bank file case created backup before preflight failed"

  local live_symlink_tmp
  new_tmp_dir live_symlink_tmp
  local live_symlink_active="$live_symlink_tmp/profile/current_run_mp.save"
  local live_symlink_bank="$live_symlink_tmp/profile/MultiplayerSaveSlots"
  local live_symlink_target="$live_symlink_tmp/target-bank"
  local live_symlink_smoke="$live_symlink_tmp/smoke"
  local live_symlink_fixture="$live_symlink_smoke/fixtures/unmanaged-active"
  local live_symlink_out="$live_symlink_tmp/live-symlink.out"
  local live_symlink_err="$live_symlink_tmp/live-symlink.err"

  mkdir -p "$(dirname "$live_symlink_active")" "$live_symlink_target" "$live_symlink_fixture/active"
  printf 'live active before bank symlink\n' > "$live_symlink_active"
  printf 'target bank content\n' > "$live_symlink_target/index.json"
  ln -s "$live_symlink_target" "$live_symlink_bank"
  printf 'fixture active bank symlink\n' > "$live_symlink_fixture/active/current_run_mp.save"

  if "$script" apply-fixture --name unmanaged-active --active-save-path "$live_symlink_active" --artifacts-root "$live_symlink_smoke" >"$live_symlink_out" 2>"$live_symlink_err"; then
    fail "apply with live bank symlink unexpectedly succeeded"
  fi

  assert_contains "$live_symlink_err" "live bank path must be a real directory"
  grep -Fq 'live active before bank symlink' "$live_symlink_active" || fail "live bank symlink case changed active save"
  [[ -L "$live_symlink_bank" ]] || fail "live bank symlink was removed or replaced"
  [[ ! -e "$live_symlink_smoke/backups/unmanaged-active" ]] || fail "live bank symlink case created backup before preflight failed"

  local malformed_tmp
  new_tmp_dir malformed_tmp
  local malformed_active="$malformed_tmp/profile/current_run_mp.save"
  local malformed_bank="$malformed_tmp/profile/MultiplayerSaveSlots"
  local malformed_smoke="$malformed_tmp/smoke"
  local malformed_fixture="$malformed_smoke/fixtures/malformed-bank"
  local malformed_out="$malformed_tmp/malformed.out"
  local malformed_err="$malformed_tmp/malformed.err"

  mkdir -p "$malformed_bank" "$malformed_fixture/active" "$malformed_fixture/bank"
  printf 'live active before malformed\n' > "$malformed_active"
  printf 'live bank before malformed\n' > "$malformed_bank/index.json"
  printf 'fixture active malformed\n' > "$malformed_fixture/active/current_run_mp.save"
  printf 'not a directory\n' > "$malformed_fixture/bank/MultiplayerSaveSlots"

  if "$script" apply-fixture --name malformed-bank --active-save-path "$malformed_active" --artifacts-root "$malformed_smoke" >"$malformed_out" 2>"$malformed_err"; then
    fail "malformed bank fixture unexpectedly succeeded"
  fi

  assert_contains "$malformed_err" "malformed fixture bank"
  grep -Fq 'live active before malformed' "$malformed_active" || fail "malformed fixture changed active save"
  grep -Fq 'live bank before malformed' "$malformed_bank/index.json" || fail "malformed fixture changed bank"
  [[ ! -e "$malformed_smoke/backups/malformed-bank" ]] || fail "malformed fixture created backup before preflight failed"

  local fixture_namespace_tmp
  new_tmp_dir fixture_namespace_tmp
  local fixture_namespace_active="$fixture_namespace_tmp/profile/current_run_mp.save"
  local fixture_namespace_bank="$fixture_namespace_tmp/profile/MultiplayerSaveSlots"
  local fixture_namespace_smoke="$fixture_namespace_tmp/smoke"
  local fixture_namespace_external="$fixture_namespace_tmp/external-fixtures"
  local fixture_namespace_out="$fixture_namespace_tmp/namespace.out"
  local fixture_namespace_err="$fixture_namespace_tmp/namespace.err"

  mkdir -p "$fixture_namespace_bank" "$fixture_namespace_smoke" "$fixture_namespace_external/unmanaged-active/active"
  printf 'live active before fixture namespace symlink\n' > "$fixture_namespace_active"
  printf 'live bank before fixture namespace symlink\n' > "$fixture_namespace_bank/index.json"
  printf 'external fixture active\n' > "$fixture_namespace_external/unmanaged-active/active/current_run_mp.save"
  ln -s "$fixture_namespace_external" "$fixture_namespace_smoke/fixtures"

  if "$script" apply-fixture --name unmanaged-active --active-save-path "$fixture_namespace_active" --artifacts-root "$fixture_namespace_smoke" >"$fixture_namespace_out" 2>"$fixture_namespace_err"; then
    fail "apply with symlinked fixtures root unexpectedly succeeded"
  fi

  assert_contains "$fixture_namespace_err" "fixture namespace path must not contain symlinks"
  grep -Fq 'live active before fixture namespace symlink' "$fixture_namespace_active" || fail "apply namespace symlink changed active save"
  grep -Fq 'live bank before fixture namespace symlink' "$fixture_namespace_bank/index.json" || fail "apply namespace symlink changed bank"
  [[ ! -e "$fixture_namespace_smoke/backups/unmanaged-active" ]] || fail "apply namespace symlink created backup before preflight failed"

  local fixture_symlink_tmp
  new_tmp_dir fixture_symlink_tmp
  local fixture_symlink_active="$fixture_symlink_tmp/profile/current_run_mp.save"
  local fixture_symlink_bank="$fixture_symlink_tmp/profile/MultiplayerSaveSlots"
  local fixture_symlink_smoke="$fixture_symlink_tmp/smoke"
  local fixture_dir_link="$fixture_symlink_smoke/fixtures/fixture-dir-link"
  local fixture_active_dir_link="$fixture_symlink_smoke/fixtures/active-dir-link"
  local fixture_active_link="$fixture_symlink_smoke/fixtures/active-link"
  local fixture_bank_root_link="$fixture_symlink_smoke/fixtures/root-link"
  local fixture_bank_broken_root_link="$fixture_symlink_smoke/fixtures/broken-root-link"
  local fixture_bank_link="$fixture_symlink_smoke/fixtures/bank-link"
  local fixture_inner_link="$fixture_symlink_smoke/fixtures/inner-link"
  local fixture_symlink_out="$fixture_symlink_tmp/symlink.out"
  local fixture_symlink_err="$fixture_symlink_tmp/symlink.err"

  mkdir -p "$fixture_symlink_bank" "$fixture_active_dir_link" "$fixture_active_link/active" "$fixture_bank_root_link/active" "$fixture_bank_broken_root_link/active" "$fixture_bank_link/active" "$fixture_bank_link/bank" "$fixture_inner_link/active" "$fixture_inner_link/bank/MultiplayerSaveSlots"
  printf 'live active before fixture symlink\n' > "$fixture_symlink_active"
  printf 'live bank before fixture symlink\n' > "$fixture_symlink_bank/index.json"
  mkdir -p "$fixture_symlink_tmp/external-fixture/active" "$fixture_symlink_tmp/external-active-dir"
  printf 'external fixture active\n' > "$fixture_symlink_tmp/external-fixture/active/current_run_mp.save"
  printf 'external active dir save\n' > "$fixture_symlink_tmp/external-active-dir/current_run_mp.save"
  printf 'fixture active real\n' > "$fixture_symlink_tmp/fixture-active-real.save"
  printf 'fixture active root-link\n' > "$fixture_bank_root_link/active/current_run_mp.save"
  printf 'fixture active broken-root-link\n' > "$fixture_bank_broken_root_link/active/current_run_mp.save"
  printf 'fixture active bank-link\n' > "$fixture_bank_link/active/current_run_mp.save"
  printf 'fixture active inner-link\n' > "$fixture_inner_link/active/current_run_mp.save"
  mkdir -p "$fixture_symlink_tmp/fixture-bank-root-real/MultiplayerSaveSlots"
  printf 'fixture bank root real\n' > "$fixture_symlink_tmp/fixture-bank-root-real/MultiplayerSaveSlots/index.json"
  mkdir -p "$fixture_symlink_tmp/fixture-bank-real"
  printf 'fixture bank real\n' > "$fixture_symlink_tmp/fixture-bank-real/index.json"
  ln -s "$fixture_symlink_tmp/external-fixture" "$fixture_dir_link"
  ln -s "$fixture_symlink_tmp/external-active-dir" "$fixture_active_dir_link/active"
  ln -s "$fixture_symlink_tmp/fixture-active-real.save" "$fixture_active_link/active/current_run_mp.save"
  ln -s "$fixture_symlink_tmp/fixture-bank-root-real" "$fixture_bank_root_link/bank"
  ln -s "$fixture_symlink_tmp/missing-fixture-bank-root" "$fixture_bank_broken_root_link/bank"
  ln -s "$fixture_symlink_tmp/fixture-bank-real" "$fixture_bank_link/bank/MultiplayerSaveSlots"
  ln -s "$fixture_symlink_tmp/fixture-active-real.save" "$fixture_inner_link/bank/MultiplayerSaveSlots/index.json"

  if "$script" apply-fixture --name fixture-dir-link --active-save-path "$fixture_symlink_active" --artifacts-root "$fixture_symlink_smoke" >"$fixture_symlink_out" 2>"$fixture_symlink_err"; then
    fail "apply with symlinked fixture directory unexpectedly succeeded"
  fi

  assert_contains "$fixture_symlink_err" "fixture path must be inside a real fixture directory"
  grep -Fq 'live active before fixture symlink' "$fixture_symlink_active" || fail "symlinked fixture directory changed active save"
  grep -Fq 'live bank before fixture symlink' "$fixture_symlink_bank/index.json" || fail "symlinked fixture directory changed bank"
  [[ ! -e "$fixture_symlink_smoke/backups/fixture-dir-link" ]] || fail "symlinked fixture directory created backup before preflight failed"

  if "$script" apply-fixture --name active-dir-link --active-save-path "$fixture_symlink_active" --artifacts-root "$fixture_symlink_smoke" >"$fixture_symlink_out" 2>"$fixture_symlink_err"; then
    fail "apply with symlinked fixture active directory unexpectedly succeeded"
  fi

  assert_contains "$fixture_symlink_err" "fixture path must be inside a real fixture directory"
  grep -Fq 'live active before fixture symlink' "$fixture_symlink_active" || fail "symlinked fixture active directory changed active save"
  grep -Fq 'live bank before fixture symlink' "$fixture_symlink_bank/index.json" || fail "symlinked fixture active directory changed bank"
  [[ ! -e "$fixture_symlink_smoke/backups/active-dir-link" ]] || fail "symlinked fixture active directory created backup before preflight failed"

  if "$script" apply-fixture --name active-link --active-save-path "$fixture_symlink_active" --artifacts-root "$fixture_symlink_smoke" >"$fixture_symlink_out" 2>"$fixture_symlink_err"; then
    fail "apply with symlinked fixture active unexpectedly succeeded"
  fi

  assert_contains "$fixture_symlink_err" "fixture active save must be a real file"
  grep -Fq 'live active before fixture symlink' "$fixture_symlink_active" || fail "symlinked fixture active changed active save"
  grep -Fq 'live bank before fixture symlink' "$fixture_symlink_bank/index.json" || fail "symlinked fixture active changed bank"
  [[ ! -e "$fixture_symlink_smoke/backups/active-link" ]] || fail "symlinked fixture active created backup before preflight failed"

  if "$script" apply-fixture --name root-link --active-save-path "$fixture_symlink_active" --artifacts-root "$fixture_symlink_smoke" >"$fixture_symlink_out" 2>"$fixture_symlink_err"; then
    fail "apply with symlinked fixture bank root unexpectedly succeeded"
  fi

  assert_contains "$fixture_symlink_err" "fixture bank must be a real directory"
  grep -Fq 'live active before fixture symlink' "$fixture_symlink_active" || fail "symlinked fixture bank root changed active save"
  grep -Fq 'live bank before fixture symlink' "$fixture_symlink_bank/index.json" || fail "symlinked fixture bank root changed bank"
  [[ ! -e "$fixture_symlink_smoke/backups/root-link" ]] || fail "symlinked fixture bank root created backup before preflight failed"

  if "$script" apply-fixture --name broken-root-link --active-save-path "$fixture_symlink_active" --artifacts-root "$fixture_symlink_smoke" >"$fixture_symlink_out" 2>"$fixture_symlink_err"; then
    fail "apply with broken symlink fixture bank root unexpectedly succeeded"
  fi

  assert_contains "$fixture_symlink_err" "fixture bank must be a real directory"
  grep -Fq 'live active before fixture symlink' "$fixture_symlink_active" || fail "broken symlink fixture bank root changed active save"
  grep -Fq 'live bank before fixture symlink' "$fixture_symlink_bank/index.json" || fail "broken symlink fixture bank root changed bank"
  [[ ! -e "$fixture_symlink_smoke/backups/broken-root-link" ]] || fail "broken symlink fixture bank root created backup before preflight failed"

  if "$script" apply-fixture --name bank-link --active-save-path "$fixture_symlink_active" --artifacts-root "$fixture_symlink_smoke" >"$fixture_symlink_out" 2>"$fixture_symlink_err"; then
    fail "apply with symlinked fixture bank unexpectedly succeeded"
  fi

  assert_contains "$fixture_symlink_err" "fixture bank must be a real directory"
  grep -Fq 'live active before fixture symlink' "$fixture_symlink_active" || fail "symlinked fixture bank changed active save"
  grep -Fq 'live bank before fixture symlink' "$fixture_symlink_bank/index.json" || fail "symlinked fixture bank changed bank"
  [[ ! -e "$fixture_symlink_smoke/backups/bank-link" ]] || fail "symlinked fixture bank created backup before preflight failed"

  if "$script" apply-fixture --name inner-link --active-save-path "$fixture_symlink_active" --artifacts-root "$fixture_symlink_smoke" >"$fixture_symlink_out" 2>"$fixture_symlink_err"; then
    fail "apply with fixture bank tree symlink unexpectedly succeeded"
  fi

  assert_contains "$fixture_symlink_err" "fixture bank contains symlink"
  grep -Fq 'live active before fixture symlink' "$fixture_symlink_active" || fail "fixture bank tree symlink changed active save"
  grep -Fq 'live bank before fixture symlink' "$fixture_symlink_bank/index.json" || fail "fixture bank tree symlink changed bank"
  [[ ! -e "$fixture_symlink_smoke/backups/inner-link" ]] || fail "fixture bank tree symlink created backup before preflight failed"

  local active_fail_tmp
  new_tmp_dir active_fail_tmp
  local active_fail_active="$active_fail_tmp/profile/current_run_mp.save"
  local active_fail_bank="$active_fail_tmp/profile/MultiplayerSaveSlots"
  local active_fail_smoke="$active_fail_tmp/smoke"
  local active_fail_fixture="$active_fail_smoke/fixtures/managed-active"
  local active_fail_bin="$active_fail_tmp/bin"
  local active_fail_out="$active_fail_tmp/fail-active.out"
  local active_fail_err="$active_fail_tmp/fail-active.err"
  local active_fail_rm_marker="$active_fail_tmp/live-bank-rm-hit"

  mkdir -p "$active_fail_bank" "$active_fail_fixture/active" "$active_fail_fixture/bank/MultiplayerSaveSlots" "$active_fail_bin"
  printf 'live active before active mv failure\n' > "$active_fail_active"
  printf 'live bank before active mv failure\n' > "$active_fail_bank/index.json"
  printf 'fixture active after active mv failure\n' > "$active_fail_fixture/active/current_run_mp.save"
  printf 'fixture bank after active mv failure\n' > "$active_fail_fixture/bank/MultiplayerSaveSlots/index.json"

  local real_mv
  real_mv="$(command -v mv)"
  local real_rm
  real_rm="$(command -v rm)"
  cat > "$active_fail_bin/mv" <<SH
#!/usr/bin/env bash
if [[ "\${1-}" == "$active_fail_tmp/profile/.current_run_mp.save.apply."* && "\${2-}" == "$active_fail_active" ]]; then
  printf 'simulated active save move failure\n' >&2
  exit 45
fi

exec "$real_mv" "\$@"
SH
  chmod +x "$active_fail_bin/mv"
  cat > "$active_fail_bin/rm" <<SH
#!/usr/bin/env bash
if [[ "\${1-}" == "-rf" && "\${2-}" == "$active_fail_bank" ]]; then
  printf 'hit\n' > "$active_fail_rm_marker"
  printf 'unexpected live bank removal during active move failure\n' >&2
  exit 47
fi

exec "$real_rm" "\$@"
SH
  chmod +x "$active_fail_bin/rm"

  if PATH="$active_fail_bin:$PATH" "$script" apply-fixture --name managed-active --active-save-path "$active_fail_active" --artifacts-root "$active_fail_smoke" >"$active_fail_out" 2>"$active_fail_err"; then
    fail "apply with active replacement failure unexpectedly succeeded"
  fi

  assert_contains "$active_fail_err" "failed to move staged active save into place"
  grep -Fq 'live active before active mv failure' "$active_fail_active" || fail "active move failure changed active save"
  grep -Fq 'live bank before active mv failure' "$active_fail_bank/index.json" || fail "active move failure changed bank"
  [[ ! -e "$active_fail_rm_marker" ]] || fail "active move failure attempted live bank removal"
  if compgen -G "$active_fail_tmp/profile/.MultiplayerSaveSlots.*" >/dev/null || compgen -G "$active_fail_tmp/profile/.current_run_mp.save.apply.*" >/dev/null; then
    fail "active move failure left apply temp paths"
  fi

  local fail_tmp
  new_tmp_dir fail_tmp
  local fail_active="$fail_tmp/profile/current_run_mp.save"
  local fail_bank="$fail_tmp/profile/MultiplayerSaveSlots"
  local fail_smoke="$fail_tmp/smoke"
  local fail_fixture="$fail_smoke/fixtures/managed-active"
  local fail_bin="$fail_tmp/bin"
  local fail_out="$fail_tmp/fail.out"
  local fail_err="$fail_tmp/fail.err"

  mkdir -p "$fail_bank" "$fail_fixture/active" "$fail_fixture/bank/MultiplayerSaveSlots" "$fail_bin"
  printf 'live active\n' > "$fail_active"
  printf 'live bank\n' > "$fail_bank/index.json"
  printf 'fixture active\n' > "$fail_fixture/active/current_run_mp.save"
  printf 'fixture bank\n' > "$fail_fixture/bank/MultiplayerSaveSlots/index.json"

  local real_mv
  real_mv="$(command -v mv)"
  local real_rm
  real_rm="$(command -v rm)"
  cat > "$fail_bin/mv" <<SH
#!/usr/bin/env bash
if [[ "\${1-}" == "$fail_tmp/profile/.MultiplayerSaveSlots.apply."*"/MultiplayerSaveSlots" && "\${2-}" == "$fail_bank" ]]; then
  for backup_index in "$fail_smoke"/backups/managed-active/*/MultiplayerSaveSlots/index.json; do
    [[ -e "\$backup_index" ]] && printf 'corrupted copied backup\n' > "\$backup_index"
  done
  printf 'simulated bank replacement failure\n' >&2
  exit 43
fi

exec "$real_mv" "\$@"
SH
  chmod +x "$fail_bin/mv"
  cat > "$fail_bin/rm" <<SH
#!/usr/bin/env bash
if [[ "\${2-}" == "$fail_tmp/profile/.MultiplayerSaveSlots.apply."* || "\${2-}" == "$fail_tmp/profile/.current_run_mp.save.apply."* ]]; then
  printf 'simulated apply cleanup failure\n' >&2
  exit 48
fi

exec "$real_rm" "\$@"
SH
  chmod +x "$fail_bin/rm"

  if PATH="$fail_bin:$PATH" "$script" apply-fixture --name managed-active --active-save-path "$fail_active" --artifacts-root "$fail_smoke" >"$fail_out" 2>"$fail_err"; then
    fail "apply with bank replacement failure unexpectedly succeeded"
  fi

  assert_contains "$fail_err" "failed to move staged bank into place"
  grep -Fq 'live active' "$fail_active" || fail "failed bank replacement did not restore active save"
  grep -Fq 'live bank' "$fail_bank/index.json" || fail "failed bank replacement did not restore bank"
  if compgen -G "$fail_tmp/profile/.MultiplayerSaveSlots.*" >/dev/null || compgen -G "$fail_tmp/profile/.current_run_mp.save.apply.*" >/dev/null; then
    fail "bank replacement failure left apply temp paths"
  fi

  local missing_tmp
  new_tmp_dir missing_tmp
  local missing_active="$missing_tmp/missing-profile/current_run_mp.save"
  local missing_smoke="$missing_tmp/smoke"
  local missing_fixture="$missing_smoke/fixtures/managed-active"
  local missing_out="$missing_tmp/missing.out"

  mkdir -p "$missing_fixture/active" "$missing_fixture/bank/MultiplayerSaveSlots"
  printf 'fixture active for missing profile\n' > "$missing_fixture/active/current_run_mp.save"
  printf 'fixture bank for missing profile\n' > "$missing_fixture/bank/MultiplayerSaveSlots/index.json"

  "$script" apply-fixture --name managed-active --active-save-path "$missing_active" --artifacts-root "$missing_smoke" >"$missing_out"

  grep -Fq 'fixture active for missing profile' "$missing_active" || fail "managed apply did not create active save in missing profile"
  grep -Fq 'fixture bank for missing profile' "$missing_tmp/missing-profile/MultiplayerSaveSlots/index.json" || fail "managed apply did not create bank in missing profile"
  pass "apply fixture backs up and mutates save state"
}

report_path_from_output() {
  sed -n 's/^smoke-setup-local: smoke report: //p' "$1" | tail -n 1
}

test_apply_fixture_writes_report() {
  local tmp
  new_tmp_dir tmp
  local active="$tmp/profile/current_run_mp.save"
  local smoke="$tmp/smoke"
  local fixture="$smoke/fixtures/unmanaged-active"
  local out="$tmp/report.out"
  local report_out_1="$tmp/report-1.out"
  local report_out_2="$tmp/report-2.out"
  local active_report_out="$tmp/report-active.out"
  local fake_bin="$tmp/bin"

  mkdir -p "$fixture/active" "$(dirname "$active")"
  printf 'fixture active\n' > "$fixture/active/current_run_mp.save"
  mkdir -p "$fake_bin"
  cat > "$fake_bin/date" <<'SH'
#!/usr/bin/env bash
printf '20260509T010203Z\n'
SH
  chmod +x "$fake_bin/date"

  PATH="$fake_bin:$PATH" "$script" apply-fixture --name unmanaged-active --active-save-path "$active" --artifacts-root "$smoke" >"$out"

  local report
  report="$(find "$smoke/reports" -type f -name '*-unmanaged-active.md' | head -n 1)"
  [[ -n "$report" ]] || fail "expected smoke report"
  assert_contains "$report" "Manual Checklist"
  assert_contains "$report" "Open Multiplayer"
  assert_contains "$report" "Fixture: unmanaged-active"
  assert_contains "$report" "Solo Host-Picker Gate"
  assert_contains "$report" "Confirm the Multiplayer Saves - Standard picker appears"
  assert_contains "$report" "Confirm hosting continues to the multiplayer lobby or character-select screen"
  assert_contains "$report" "STS2 does not write a multiplayer run save from a solo host lobby"
  assert_contains "$report" "Two-Player Save Lifecycle"
  assert_contains "$report" "Invite at least one other player and begin a Standard multiplayer run"
  assert_contains "$report" "Progress far enough for STS2 to write the multiplayer run save"
  assert_contains "$report" "Confirm MultiplayerSaveSlots/index.json gains a new campaign id"
  assert_contains "$report" "Confirm the picker shows real roster labels and omits progress when no safe act/floor value is available"
  assert_contains "$report" "Open Details on a campaign row and confirm it shows progress, player count, timestamps, campaign id, and the full roster"
  assert_contains "$report" "Expanded Roster And Recovery"
  assert_contains "$report" "With a 4+ player campaign, confirm roster labels compact as First, Second +N"
  assert_contains "$report" "Open Details on a 4+ player campaign row and confirm it shows the full roster"
  assert_contains "$report" "Select an older Unknown party campaign"
  assert_contains "$report" "compatibility warning appears once before the next identical Embark attempt"
  assert_contains "$report" "Duplicate Active Save"
  assert_contains "$report" "Sync Active Save"

  PATH="$fake_bin:$PATH" "$script" report --fixture unmanaged-active --active-save-path "$active" --artifacts-root "$smoke" >"$report_out_1"
  PATH="$fake_bin:$PATH" "$script" report --fixture unmanaged-active --active-save-path "$active" --artifacts-root "$smoke" >"$report_out_2"

  local report_count
  report_count="$(find "$smoke/reports" -type f | wc -l)"
  [[ "$report_count" -eq 3 ]] || fail "expected three unique report files, got $report_count"

  PATH="$fake_bin:$PATH" "$script" report --fixture active --active-save-path "$active" --artifacts-root "$smoke" >"$active_report_out"
  local active_report
  active_report="$(report_path_from_output "$active_report_out")"
  [[ -n "$active_report" ]] || fail "expected active fixture report path"
  assert_contains "$active_report" "Fixture: active"
  assert_contains "$active_report" "Backup path: not provided"
  pass "apply fixture writes smoke report"
}

test_apply_fixture_uses_unique_backups() {
  local tmp
  new_tmp_dir tmp
  local active="$tmp/profile/current_run_mp.save"
  local smoke="$tmp/smoke"
  local fixture="$smoke/fixtures/unmanaged-active"
  local out1="$tmp/unique-1.out"
  local out2="$tmp/unique-2.out"
  local wrong_active="$tmp/profile/not_current.save"
  local wrong_out="$tmp/wrong-apply.out"
  local wrong_err="$tmp/wrong-apply.err"

  mkdir -p "$fixture/active" "$(dirname "$active")"
  printf 'original active\n' > "$active"
  printf 'fixture active\n' > "$fixture/active/current_run_mp.save"
  printf 'wrong active\n' > "$wrong_active"

  "$script" apply-fixture --name unmanaged-active --active-save-path "$active" --artifacts-root "$smoke" >"$out1"
  printf 'second live active\n' > "$active"
  "$script" apply-fixture --name unmanaged-active --active-save-path "$active" --artifacts-root "$smoke" >"$out2"

  if "$script" apply-fixture --name unmanaged-active --active-save-path "$wrong_active" --artifacts-root "$smoke" >"$wrong_out" 2>"$wrong_err"; then
    fail "apply with invalid active save filename unexpectedly succeeded"
  fi

  assert_contains "$wrong_err" "active save path basename must be current_run_mp.save"

  local backup_count
  backup_count="$(find "$smoke/backups/unmanaged-active" -mindepth 1 -maxdepth 1 -type d | wc -l)"
  [[ "$backup_count" -eq 2 ]] || fail "expected two backup directories, got $backup_count"

  local suffix_tmp
  new_tmp_dir suffix_tmp
  local suffix_active="$suffix_tmp/profile/current_run_mp.save"
  local suffix_smoke="$suffix_tmp/smoke"
  local suffix_fixture="$suffix_smoke/fixtures/unmanaged-active"
  local suffix_bin="$suffix_tmp/bin"
  local suffix_report_out="$suffix_tmp/report.out"

  mkdir -p "$suffix_fixture/active" "$(dirname "$suffix_active")" "$suffix_bin"
  printf 'fixture active\n' > "$suffix_fixture/active/current_run_mp.save"
  cat > "$suffix_bin/date" <<'SH'
#!/usr/bin/env bash
printf '20260509T040506Z\n'
SH
  chmod +x "$suffix_bin/date"

  local index
  for index in 0 1 2 3 4 5 6 7 8 9 10; do
    printf 'live %s\n' "$index" > "$suffix_active"
    PATH="$suffix_bin:$PATH" "$script" apply-fixture --name unmanaged-active --active-save-path "$suffix_active" --artifacts-root "$suffix_smoke" >"$suffix_tmp/apply-$index.out"
  done

  PATH="$suffix_bin:$PATH" "$script" report --fixture unmanaged-active --active-save-path "$suffix_active" --artifacts-root "$suffix_smoke" >"$suffix_report_out"
  local suffix_report
  suffix_report="$(report_path_from_output "$suffix_report_out")"
  [[ -n "$suffix_report" ]] || fail "expected suffix report path"
  assert_contains "$suffix_report" "Backup path: $suffix_smoke/backups/unmanaged-active/20260509T040506Z-10"
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
