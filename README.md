# Multiplayer Save Slots

Slay the Spire 2 mod in development to let a host keep multiple multiplayer campaign saves instead of abandoning the single vanilla co-op save.

## Status

Private development. Phase 9 adds best-effort metadata repair for older campaign entries after activation.

The picker can start a new vanilla run or activate an existing campaign payload from the save bank before resuming the vanilla host flow. After vanilla writes `current_run_mp.save`, the mod syncs existing selected campaigns back to the bank and finalizes newly started runs as separate bank campaigns.

If an active multiplayer save is unmanaged or has unsynced changes, the picker now offers conservative recovery actions before continuing. It can duplicate an unmanaged active save into the bank or sync active changes back to the owning campaign.

Newly finalized or duplicated runs use live lobby/save metadata where STS2 exposes it. Campaign titles compact large rosters as `First, Second +N`, picker subtitles include best-effort progress such as `Floor 18` when safely readable, and each campaign row has a `Details` action for viewing the full stored roster and metadata. Older `Unknown party` campaigns can self-repair missing display metadata after they are selected and safely activated.

## Design

See `docs/superpowers/specs/2026-05-08-multiplayer-save-slots-design.md`.

## Build

Requires a .NET SDK that can target `net9.0` and Slay the Spire 2 installed through Steam.

```bash
dotnet build MultiplayerSaveSlots.sln
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

If your machine has the .NET 9 targeting pack but only a newer runtime installed, run with:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

## Release

Releases are built locally because the project references STS2 assemblies from a local game install. A normal GitHub-hosted runner does not have `sts2.dll`, `GodotSharp.dll`, or `0Harmony.dll`.

Package validation requires:

- .NET SDK that can target `net9.0`
- `python3`
- `git`
- Slay the Spire 2 installed in the path used by `MultiplayerSaveSlots.csproj`

Publishing a full release additionally requires authenticated GitHub CLI via `gh auth login`.

Before running either release command, update `MultiplayerSaveSlots.json` so its `version` matches the release tag without the leading `v`.

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
11. Confirm the picker shows real roster labels, compacts 4+ player runs as `First, Second +N`, and omits progress when no safe act/floor value is available.
12. Open `Details` on a campaign row and confirm it shows progress, player count, timestamps, campaign id, and the full roster for a 4+ player campaign.
13. Select an older `Unknown party` campaign, then re-open the picker and confirm metadata repair updates the row if STS2 exposes roster/progress data.
14. With an unmanaged `current_run_mp.save` fixture applied, choose a host action and confirm the recovery modal offers `Duplicate Active Save`.
15. With a managed active save changed after activation, choose a different campaign and confirm the recovery modal offers `Sync Active Save`.

Smoke reports and backups are written under `artifacts/smoke/`.
