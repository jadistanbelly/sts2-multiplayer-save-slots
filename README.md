# Multiplayer Save Slots

Slay the Spire 2 mod in development to let a host keep multiple multiplayer campaign saves instead of abandoning the single vanilla co-op save.

## Status

Private development. Phase 4 adds active-save recovery choices before replacing the vanilla multiplayer save.

The picker can start a new vanilla run or activate an existing campaign payload from the save bank before resuming the vanilla host flow. After vanilla writes `current_run_mp.save`, the mod syncs existing selected campaigns back to the bank and finalizes newly started runs as separate bank campaigns.

If an active multiplayer save is unmanaged or has unsynced changes, the picker now offers conservative recovery actions before continuing. It can duplicate an unmanaged active save into the bank or sync active changes back to the owning campaign. Roster labels for newly finalized or duplicated runs are still placeholder metadata until live lobby roster extraction is added.

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

## In-Game Smoke Test

1. Build the mod.
2. Copy `MultiplayerSaveSlots.json` and `bin/Debug/MultiplayerSaveSlots.dll` to `<STS2>/mods/MultiplayerSaveSlots/`.
3. Launch STS2 with mods enabled.
4. Open `Multiplayer`.
5. Confirm `Host` is visible even when a current multiplayer save exists.
6. Select `Host -> Standard`.
7. Confirm the `Multiplayer Saves - Standard` picker appears.
8. Choose `Start New Run`.
9. Confirm vanilla hosting continues.
10. Progress far enough for STS2 to write the multiplayer run save.
11. Confirm `MultiplayerSaveSlots/index.json` gains a new campaign id.
12. Re-open `Host -> Standard` and confirm the new campaign appears in the picker.
13. With an unmanaged `current_run_mp.save` present, choose a host action and confirm the recovery modal offers `Duplicate Active Save`.
14. With a managed active save changed after activation, choose a different campaign and confirm the recovery modal offers `Sync Active Save`.
