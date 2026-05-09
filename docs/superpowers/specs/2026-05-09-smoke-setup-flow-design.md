# Smoke Setup Flow Design

## Summary

Phase 6 adds automation-assisted in-game smoke setup for Multiplayer Save Slots. It does not try to click through STS2 or automate Steam multiplayer. Instead, it automates the repeatable and risky setup work around packaging, installing the mod, backing up live save state, and applying known save fixtures before a short manual in-game checklist.

This keeps the smoke test grounded in the real game install while avoiding brittle GUI automation and avoiding fake STS2 save payloads.

## Goals

- Install the locally built release zip into the STS2 mods folder with the same layout users receive.
- Validate that the installed mod folder contains `MultiplayerSaveSlots.json` and `MultiplayerSaveSlots.dll`.
- Back up live multiplayer save state before applying any fixture.
- Support local, ignored smoke fixtures created from real STS2 save payloads.
- Apply fixture states for manual validation of the picker and recovery paths.
- Generate a smoke-test report/checklist with exact paths, fixture name, and remaining manual steps.
- Keep the process safe enough to run repeatedly during development.

## Non-Goals

- Do not automate STS2 menu clicks in this phase.
- Do not automate Steam lobby creation, friend joins, or two-client networking.
- Do not generate synthetic STS2 run save payloads.
- Do not commit captured user save payloads to the repository.
- Do not overwrite live `current_run_mp.save` or `MultiplayerSaveSlots/` state without a timestamped backup.
- Do not add runtime mod behavior or dev-only in-game smoke code in this phase.

## Architecture

Add a local smoke setup script at `scripts/smoke-setup-local.sh`. The script uses the existing `scripts/release-local.sh --package-only <tag>` output as its package source, extracts the drop-in `MultiplayerSaveSlots/` folder into the local STS2 `mods` directory, and manages smoke fixtures under ignored local artifact paths.

The smoke script is an operator helper. It prepares the environment, then prints the remaining manual game steps. It should not launch STS2 by default.

## Command Shape

The command contract should support these flows:

```bash
scripts/smoke-setup-local.sh install --tag v0.1.0
scripts/smoke-setup-local.sh capture-fixture --name baseline-standard --active-save-path <path>
scripts/smoke-setup-local.sh apply-fixture --name unmanaged-active --active-save-path <path>
scripts/smoke-setup-local.sh report --fixture unmanaged-active
```

`--tag` should refer to a package produced by `scripts/release-local.sh --package-only <tag>`.

`--active-save-path` should be explicit for fixture capture/apply operations. STS2 derives the active multiplayer path through `RunSaveManager.GetRunSavePath(CurrentProfileId, "current_run_mp.save")` inside the game process, but an external shell script cannot reliably know the active profile. Requiring an explicit path prevents mutating the wrong profile.

The script may provide default STS2 install path detection for the mods folder because `MultiplayerSaveSlots.csproj` already has OS-specific STS2 install defaults. Any detected path should be printed before mutation.

## Fixture Model

Fixtures are local artifacts, not repository data. They should live under an ignored path such as:

```text
artifacts/smoke/
  backups/
    <timestamp>/
  fixtures/
    <fixture-name>/
      active/
        current_run_mp.save
      bank/
        MultiplayerSaveSlots/
          active-state.json
          index.json
          saves/
```

Fixtures should be created from real local game state with `capture-fixture`. A fixture may contain:

- only an unmanaged `current_run_mp.save`
- a managed active save plus `MultiplayerSaveSlots/active-state.json`
- one or more bank campaigns under `MultiplayerSaveSlots/saves/`

The script should not create or mutate save payload internals. It should only copy files and directories.

## Install Flow

The install command should:

1. Require `git`, `python3`, and the existing package artifact.
2. If the package artifact is missing, fail with an instruction to run `scripts/release-local.sh --package-only <tag>`.
3. Resolve the STS2 install path from a flag or known local defaults.
4. Create `<STS2>/mods/MultiplayerSaveSlots/`.
5. Remove old files inside only that mod folder.
6. Extract/copy the zip payload into the mods folder.
7. Verify the installed folder contains:

```text
<STS2>/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.json
<STS2>/mods/MultiplayerSaveSlots/MultiplayerSaveSlots.dll
```

8. Print the exact installed manifest version and DLL path.

## Fixture Apply Flow

The apply command should:

1. Require an explicit `--active-save-path`.
2. Derive the save directory from that path.
3. Derive the mod bank path as `<save-directory>/MultiplayerSaveSlots`.
4. Verify the requested fixture exists.
5. Create a timestamped backup containing any existing:
   - `current_run_mp.save`
   - `MultiplayerSaveSlots/`
6. Apply fixture files with whole-file/directory copies.
7. Print the backup location and a short manual checklist for the chosen fixture.

If the fixture does not include bank state, the script should remove or move aside existing bank state only after it has been backed up.

## Report Flow

The report command should generate or print a concise checklist with:

- installed mod path
- package tag or zip path
- active save path
- fixture name
- backup path
- expected in-game observations
- fields for pass/fail notes

The report can be Markdown under `artifacts/smoke/reports/<timestamp>-<fixture>.md`.

## Safety Rules

- Never mutate a save directory unless `--active-save-path` is provided.
- Never overwrite or remove live save state before creating a backup.
- Never follow a fixture name containing path traversal or shell metacharacters.
- Never write outside the resolved mod folder, artifact folder, active save file, and derived bank folder.
- Print all target paths before mutation.
- Keep all generated smoke artifacts ignored by git.

## Testing

Automated tests should focus on script behavior against temporary directories:

- package install copies the expected zip payload into a fake STS2 mods folder
- missing package produces a clear error
- fixture capture copies active save and bank directories into `artifacts/smoke/fixtures/<name>`
- fixture apply creates a backup before mutation
- fixture apply restores unmanaged active save state
- invalid fixture names are rejected
- report generation contains paths and manual checklist items

The test should not require launching STS2.

## Documentation

README smoke-test docs should change from manual file-copy instructions to the setup script:

```bash
scripts/release-local.sh --package-only v0.1.0
scripts/smoke-setup-local.sh install --tag v0.1.0
scripts/smoke-setup-local.sh apply-fixture --name unmanaged-active --active-save-path <path>
```

The docs should make clear that this phase automates setup and fixtures, not the game UI.

## Future Work

This setup flow leaves room for later automation:

- a dev-only in-game smoke harness that writes pass/fail reports from inside STS2
- optional `xdotool` or image-based local GUI automation for narrow menu checks
- richer fixture capture helpers once live roster/progress metadata extraction exists
