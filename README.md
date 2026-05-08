# Multiplayer Save Slots

Slay the Spire 2 mod in development to let a host keep multiple multiplayer campaign saves instead of abandoning the single vanilla co-op save.

## Status

Private development. Phase 3 adds save sync after vanilla multiplayer save writes.

The picker can start a new vanilla run or activate an existing campaign payload from the save bank before resuming the vanilla host flow. After vanilla writes `current_run_mp.save`, the mod syncs existing selected campaigns back to the bank and finalizes newly started runs as separate bank campaigns. Roster labels for newly finalized runs are still placeholder metadata until live lobby roster extraction is added.

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
