# Multiplayer Save Slots

Slay the Spire 2 mod in development to let a host keep multiple multiplayer campaign saves instead of abandoning the single vanilla co-op save.

## Status

Private development. The current implementation phase is focused on the save-bank and active-slot swap foundation.
No user-facing save picker is complete yet.

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
