# Multiplayer Save Slots

Host-side Slay the Spire 2 mod for keeping separate multiplayer campaign saves. It manages the host's local `current_run_mp.save` file so different groups or runs do not overwrite the same vanilla co-op save.

## Current Release

Latest release: [v0.2.0](https://github.com/jadistanbelly/sts2-multiplayer-save-slots/releases/tag/v0.2.0)

Download `MultiplayerSaveSlots-v0.2.0.zip`, then extract the included `MultiplayerSaveSlots/` folder into your STS2 `mods/` directory.

## What It Does

- Adds a save picker after `Multiplayer -> Host -> Standard/Daily/Custom`.
- Lets the host rename save slots so themed or similar-looking runs are easier to distinguish.
- Starts a new managed multiplayer campaign or continues an existing one.
- Swaps the selected campaign payload into STS2's active multiplayer save before loading.
- Syncs the active save back to the selected campaign after STS2 writes progress.
- Shows roster, progress, timestamps, short campaign id, save fingerprint, and selected characters when STS2 metadata is readable.
- Adds recovery choices for unmanaged or unsynced active multiplayer saves.
- Warns before embarking when the loaded-run lobby participants do not match the selected campaign roster.

## Save Safety

The mod treats active save replacement as a data-loss risk and fails closed when it cannot prove the operation is safe.

- Active saves, active-state files, and campaign payloads are backed up before replacement.
- Symlinks and reparse points are rejected in active save, state, bank, metadata, payload, and backup paths.
- Tampered restore backup paths are rejected before mutation.
- Malformed or mismatched campaign metadata is skipped instead of blocking picker rendering.
- If an active multiplayer save is unmanaged, the mod offers to duplicate it into the save bank before continuing.
- If a managed active save has unsynced changes, the mod offers to sync it back to its campaign before switching slots.

## How To Use

1. Launch STS2 with mods enabled.
2. Open `Multiplayer -> Host`.
3. Choose `Standard`, `Daily`, or `Custom`.
4. In `Multiplayer Saves`, choose `Start New Run` or select an existing campaign row.
5. For an existing campaign, inspect the preview, use the small edit icon beside the title to rename the slot if needed, and press `Continue`.
6. If a recovery prompt appears, choose `Duplicate Active Save` or `Sync Active Save` before switching.

STS2 does not create a multiplayer save from a solo host lobby. A run must actually begin with at least one joined player before STS2 writes `current_run_mp.save` and the mod can finalize a new save slot.

## Save Bank Location

The save bank is stored beside STS2's active multiplayer save:

```text
<profile saves>/
  current_run_mp.save
  MultiplayerSaveSlots/
    index.json
    active-state.json
    saves/
      <campaign-id>/
        metadata.json
        multiplayer_run.save
        backup/
```

## Known Limits

- This is local host save management, not cloud sync or cross-host campaign transfer.
- Roster, character, and progress labels are best-effort because they depend on what STS2 exposes in current lobby/save data.
- Older saves can appear as `Unknown party` until they are selected and metadata repair can read live STS2 data.
- UI polish is still early; the current priority is safe save switching.

## Build And Test

Requires a .NET SDK that can target `net9.0` and a local Slay the Spire 2 Steam install because the project references STS2 assemblies.

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
tests/smoke-setup-local-tests.sh
```

## Package And Release

Create a local package without tagging:

```bash
scripts/release-local.sh --package-only v0.2.0
```

Publish a full release from a clean, synced `main` checkout:

```bash
scripts/release-local.sh v0.2.0
```

Before publishing, `MultiplayerSaveSlots.json` must have a `version` matching the tag without the leading `v`.

## Manual Smoke Setup

The smoke setup script can install a package and stage local save fixtures. It does not automate STS2 menu clicks or Steam multiplayer.

```bash
scripts/release-local.sh --package-only v0.2.0
scripts/smoke-setup-local.sh install --tag v0.2.0
```

Capture and apply a real local active-save fixture:

```bash
ACTIVE_SAVE="$HOME/.local/share/Steam/steamapps/common/Slay the Spire 2/preferences/profile_1/saves/current_run_mp.save"
scripts/smoke-setup-local.sh capture-fixture --name unmanaged-active --active-save-path "$ACTIVE_SAVE"
scripts/smoke-setup-local.sh apply-fixture --name unmanaged-active --active-save-path "$ACTIVE_SAVE"
```

Smoke reports and backups are written under `artifacts/smoke/`.
