# Campaign Picker Details Design

## Summary

Phase 8 adds an on-demand details surface to the Multiplayer Save Slots picker. Phase 7 stores real roster and progress metadata, but the picker still shows only compact row text. This phase makes the full stored metadata visible without making the main picker rows noisy.

## Goals

- Keep the existing compact picker row labels unchanged.
- Add a separate campaign details action for existing campaign rows.
- Show the full stored roster in original order with no four-player assumption.
- Show known progress, player count, creation time, last-played time, and campaign id in the details view.
- Keep `Start New Run` as a single direct action with no details button.
- Keep campaign activation, save swapping, recovery, and sync behavior unchanged.

## Non-Goals

- Do not add party compatibility warnings in this phase.
- Do not compare the current lobby against stored campaign metadata.
- Do not repair older campaign metadata.
- Do not rewrite STS2 multiplayer save payloads.
- Do not add new STS2 or Harmony hooks.

## User-Facing Behavior

The picker remains compact:

```text
Start New Run

Alice, Bob +2
Floor 18 - 4 players
Details
```

Selecting the campaign row still activates the campaign and continues the vanilla load flow. Selecting `Details` opens an overlay on top of the picker.

The details overlay shows:

```text
Alice, Bob +2
Floor 18 - 4 players

Progress: Floor 18
Players: 4
Created: 2026-05-09 18:21 UTC
Last played: 2026-05-09 18:44 UTC
Campaign id: aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa

Roster
1. Alice
2. Bob
3. Casey
4. Dana
```

If progress is unknown, the overlay shows `Progress: Unknown`. If the roster is empty, it shows one roster line: `Unknown party`.

## Model Changes

`MultiplayerSavePickerRow` gains an optional details model. Campaign rows populate it from `CampaignMetadata`; `Start New Run` leaves it null.

The details model contains display-ready strings only:

- `Title`
- `Subtitle`
- `SummaryLines`
- `RosterLines`

This keeps Godot UI code thin and keeps formatting testable without requiring Godot runtime tests.

## UI Changes

`MultiplayerSavePickerModal` renders campaign rows as a horizontal row:

- a large existing campaign action button
- a smaller `Details` button when details are available

The details overlay is owned by the picker modal, not by save runtime services. Closing the details overlay returns to the same picker without rebuilding the model or touching save state.

## Error Handling

Details rendering must be fail-soft:

- missing progress displays `Progress: Unknown`
- empty roster displays `Unknown party`
- missing or blank player names display `Unknown`

Details must never block campaign activation. If a row has no details model, the UI simply omits the details button.

## Testing

Core model tests should verify:

- campaign rows include a details model
- details include every roster member for 4+ player campaigns
- missing progress becomes `Progress: Unknown`
- empty roster details show `Unknown party`
- `Start New Run` has no details model

Build verification covers the Godot UI compile surface. Existing host-flow and storage tests cover that campaign activation and sync behavior remain unchanged.

## Documentation

Update the README status and smoke checklist to mention that campaign rows have a Details action for viewing the full roster and progress metadata.

## Future Work

- Add metadata repair for campaigns created before live roster extraction.
- Add compatibility warnings after a load-lobby hook can compare current participants with stored original roster metadata.
- Expand progress extraction if future STS2 builds expose better structured run state.
