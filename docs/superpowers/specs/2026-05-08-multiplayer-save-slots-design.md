# Multiplayer Save Slots Design

> **Archived historical spec:** This design note is preserved for context only. It is not an active spec or current work item. See `docs/superpowers/README.md` for archive policy.

## Summary

This mod adds multiple host-owned multiplayer campaign saves to Slay the Spire 2 without replacing the game's multiplayer rules. Today the host can only maintain one active multiplayer run. Starting a different run with another friend group requires abandoning the current multiplayer save.

The v1 design keeps the game's existing host flow and inserts a save picker after gamemode selection:

```text
Main Menu -> Multiplayer -> Host -> Select Gamemode -> Multiplayer Saves
```

The picker lets the host start a new multiplayer run or activate an existing campaign save for the selected gamemode.

## Goals

- Let a host maintain multiple multiplayer saves at once.
- Preserve the existing STS2 multiplayer host flow as much as possible.
- Support Standard, Daily, and Custom multiplayer runs.
- Auto-label saves from the original lobby roster.
- Support large rosters created by separate "remove multiplayer limit" style mods.
- Avoid bypassing vanilla party validation in v1.
- Protect user saves with backups, ownership tracking, and checksum checks.

## Non-Goals

- Do not modify `multisave-multiplayer` or use it as the setup model.
- Do not bypass vanilla "same original party" resume behavior in v1.
- Do not add or substitute players into an existing campaign.
- Do not require direct integration with unlimited-player mods.
- Do not rewrite internal save contents unless later testing proves it is necessary.
- Do not build an external launcher-only save swapper.

## User Flow

1. The player launches STS2.
2. The player chooses `Multiplayer`.
3. The player chooses `Host`.
4. The player chooses a gamemode: `Standard`, `Daily`, or `Custom`.
5. The mod shows a `Multiplayer Saves` picker for that gamemode.
6. The player chooses one of:
   - `Start New Run`
   - an existing campaign save
7. For an existing save, the mod activates that save, then returns control to the normal STS2 host/load flow.
8. For a new run, the mod creates or reserves a new bank entry and lets STS2 initialize the lobby/run normally.

The picker should show compact campaign rows:

```text
buddy1 - Floor 18 - 2 players
buddy1 + buddy2 - Floor 7 - 3 players
buddy1 + buddy2 + 9 more - Act 2 - 11 players
```

The full original roster appears in a details or expanded area.

## Save Model

The mod owns a separate save bank outside the game's single active multiplayer save.

```text
MultiSaves/
  index.json
  saves/
    <campaign-id>/
      metadata.json
      multiplayer_run.save
      backup/
```

Each campaign stores:

- stable campaign id
- gamemode: `Standard`, `Daily`, or `Custom`
- auto label generated from the original roster
- original player roster with no fixed maximum player count
- created timestamp
- last-played timestamp
- active/inactive status
- active-save ownership/checksum metadata
- the actual multiplayer run save payload copied from the vanilla save
- best-effort run details, such as act/floor, if safely readable

The active vanilla multiplayer save remains the one STS2 expects. The mod swaps the selected bank save into that active path before loading/hosting and syncs active changes back to the selected bank entry after vanilla save writes.

## Technical Approach

Use a "Save Bank + Active Slot Swap" architecture.

The mod does not make STS2 directly understand multiple multiplayer save filenames. Instead, it keeps multiple campaign saves in the mod bank and copies the selected one into the vanilla multiplayer save location when needed.

This keeps compatibility with:

- vanilla multiplayer load validation
- fixed original party requirements
- Steam lobby behavior
- Steam/local save sync behavior, as far as practical
- unlimited-player mods, because roster metadata does not assume a max of 4

The main tradeoff is that swap and sync code must be conservative. The mod must avoid syncing the wrong active file back into the wrong campaign.

## Components

### MultiplayerSaveBank

Owns the save bank file layout.

Responsibilities:

- create bank directories
- read/write `index.json`
- read/write `metadata.json`
- create campaign entries
- list campaigns by gamemode
- create timestamped backups
- calculate file checksums

### ActiveSaveSwitcher

Owns activation and sync-back.

Responsibilities:

- find the vanilla multiplayer run save path
- copy a selected campaign save into the vanilla active multiplayer save slot
- record which campaign owns the active slot
- record checksums before and after activation
- sync the active save back to the selected campaign after vanilla save writes
- stop before overwrite when ownership/checksum state is ambiguous

### CampaignMetadataExtractor

Reads safe metadata where possible.

Responsibilities:

- capture lobby roster when the campaign is created
- generate compact labels from roster names
- store full player identifiers when available
- extract gamemode and basic run progress if safely readable
- degrade gracefully if fields move in future STS2 builds

Metadata extraction must never be required to preserve the raw save payload.

### MultiplayerSavePickerScreen

Provides the in-game campaign picker after gamemode selection.

Responsibilities:

- show `Start New Run`
- show existing campaigns for the selected gamemode
- show compact labels and player counts
- show full roster/details on demand
- warn when a campaign appears incompatible with the current lobby state
- avoid assuming a four-player maximum

### HostFlowPatches

Harmony patches around the multiplayer host flow.

Likely investigation targets from the STS2 assembly:

- `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerHostSubmenu`
- `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerSubmenu`
- `MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NMultiplayerLoadGameScreen`
- Standard, Daily, and Custom multiplayer start methods

Responsibilities:

- intercept after gamemode selection
- open the save picker
- resume vanilla host/load flow after the user selects a campaign
- fall back to vanilla behavior if the mod cannot safely activate a save

### SaveSyncPatches

Harmony patches around multiplayer save completion.

Likely investigation targets from the STS2 assembly:

- `MegaCrit.Sts2.Core.Saves.Managers.RunSaveManager`
- `MegaCrit.Sts2.Core.Saves.SaveManager`
- methods near `SaveRun`

Responsibilities:

- detect when vanilla STS2 writes the active multiplayer save
- sync the active save back to the selected campaign bank entry
- create backups before sync-back
- verify checksum/ownership before writing to the bank

### CompatibilityGuards

Protects users from ambiguous states.

Responsibilities:

- verify expected save paths exist
- verify active-save ownership before sync-back
- detect checksum mismatches
- handle missing or malformed mod metadata
- handle STS2 updates that move private members or methods
- log clear diagnostics
- fall back to vanilla behavior instead of risking a destructive overwrite

## Safety Rules

- Never overwrite a campaign save without a backup.
- Never overwrite the active vanilla save without a backup.
- Never sync the active save back to a campaign unless ownership/checksum state matches expectations.
- If Steam Cloud or STS2 changes the active save unexpectedly, stop and ask the user to choose how to recover.
- Keep the raw multiplayer save payload intact.
- Treat metadata as disposable and repairable; treat save payloads as authoritative.
- Prefer whole-file copy operations over save-content mutation.

## Party Compatibility

Vanilla STS2 appears to require the same original party to continue a multiplayer run. V1 should preserve that behavior.

The mod stores the original roster for display and compatibility warnings, but it does not attempt to:

- add new players to an old run
- remove missing players from an old run
- substitute a player
- rewrite run ownership

For large lobbies created by other mods, the roster model and UI must support arbitrary player counts. Save labels should remain compact:

```text
firstPlayer + secondPlayer + N more
```

## Error Handling

When the mod detects an unsafe condition, it should prefer user-visible protection over automatic repair.

Examples:

- Active vanilla save checksum does not match the selected campaign.
- Selected campaign payload is missing.
- STS2 save path cannot be found.
- Campaign metadata exists but payload is missing.
- Active save appears to belong to a different campaign.
- Save sync hook fires without an active selected campaign.

The user-facing choice should be limited and concrete, such as:

- keep vanilla behavior
- restore from backup
- duplicate active save into a new campaign
- cancel and return to menu

## Testing Plan

File-level tests:

- create a new campaign entry
- generate compact labels for 1, 2, 4, and many players
- activate a campaign into a fake vanilla save path
- sync active save back to the correct campaign
- reject sync-back on checksum mismatch
- create backups before activation and sync
- list campaigns by gamemode

In-game smoke tests:

- Standard multiplayer: start a new campaign, save, exit, resume
- Standard multiplayer: create two separate campaigns and switch between them
- Daily multiplayer: verify picker placement and activation
- Custom multiplayer: verify picker placement and activation
- Existing vanilla multiplayer save: import or protect without overwrite
- Large roster metadata: verify picker remains readable

Regression tests after STS2 updates:

- host flow patch still opens picker after gamemode selection
- save sync patch still fires after multiplayer save writes
- vanilla fallback works if patches fail

## Open Implementation Questions

- Exact vanilla multiplayer save path and cloud/local sync interaction need confirmation in code.
- Exact hook point after Standard/Daily/Custom gamemode selection needs confirmation by inspecting STS2 types/methods.
- Exact save completion hook needs confirmation by inspecting `RunSaveManager` and `SaveManager`.
- The picker can likely be implemented with Godot UI nodes, but final UI construction should follow the game's existing screen patterns once those nodes are inspected.

## Implementation Order

1. Scaffold a clean STS2 mod using the local `autopath` project as a shape reference.
2. Build and test `MultiplayerSaveBank` with fake paths.
3. Build and test `ActiveSaveSwitcher` with fake active save files.
4. Add metadata and compact label generation.
5. Investigate vanilla save path and host flow hooks.
6. Patch host flow to show a minimal picker.
7. Wire activation into existing load/start flow.
8. Patch save sync-back after vanilla save writes.
9. Add safety prompts and fallback behavior.
10. Run in-game smoke tests.
