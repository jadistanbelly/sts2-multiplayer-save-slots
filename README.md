# Multiplayer Save Slots

Slay the Spire 2 mod in development to let a host keep multiple multiplayer campaign saves instead of abandoning the single vanilla co-op save.

## Status

Private development. The current implementation phase is focused on the save-bank and active-slot swap foundation.
Phase 2 adds the first host-flow picker after `Multiplayer -> Host -> Select Gamemode`.

The picker can start a new vanilla run or activate an existing campaign payload from the save bank before resuming the vanilla loaded-run host flow. Save sync after vanilla writes is not complete yet, so newly started runs are not finalized into separate bank campaigns until the next phase.

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
