# Campaign Metadata Extraction Design

## Summary

Phase 7 adds real campaign metadata for Multiplayer Save Slots. Newly finalized or duplicated campaigns should no longer be labeled with placeholder roster data when the live multiplayer lobby exposes player information. The picker should also include best-effort run progress, such as act or floor, when that information can be read without mutating or depending on the raw STS2 save payload.

The phase is intentionally display-focused. Roster metadata helps players identify which group a campaign belongs to. Progress metadata helps distinguish repeated groups and large parties that otherwise produce similar labels.

## Goals

- Capture the live multiplayer lobby roster when creating a new campaign.
- Store the full captured roster with no fixed maximum player count.
- Generate compact campaign labels in the `First, Second +N` style for large rosters.
- Preserve useful labels for one-player and two-player campaigns.
- Extract act/floor progress as best-effort metadata when it is safely readable.
- Display roster labels and progress compactly in the picker.
- Keep campaign creation and active-save recovery usable if metadata extraction fails.

## Non-Goals

- Do not rewrite STS2 multiplayer save payloads.
- Do not alter vanilla lobby membership, party validation, or ownership rules.
- Do not require progress extraction for campaign creation.
- Do not add a full expanded details UI in this phase.
- Do not assume multiplayer lobbies are limited to four players.
- Do not make metadata extraction block save preservation.

## User-Facing Display

The picker title should be the compact roster label.

For one player:

```text
Alice
```

For two players:

```text
Alice, Bob
```

For three or more players:

```text
Alice, Bob +2
```

The picker subtitle should include progress first when known, followed by player count:

```text
Floor 18 - 4 players
```

When progress is unknown, omit the `Unknown progress` text and show only player count:

```text
4 players
```

When roster capture is unavailable, keep the existing safe fallback:

```text
Unknown party
0 players
```

This keeps the row compact while still distinguishing similar rosters by run progress when available.

## Metadata Model

The existing `CampaignMetadata` model already has the fields this phase needs:

- `Label`
- `Roster`
- `ActOrFloor`

Phase 7 should populate those fields from a single runtime metadata snapshot where possible. The snapshot should contain:

- `IReadOnlyList<PlayerIdentity> Roster`
- `string? ActOrFloor`

The save bank should continue treating the raw save payload as authoritative. Metadata is helpful display data and may be missing, stale, or repaired later.

## Roster Extraction

Roster extraction should happen at the runtime boundary where the mod can observe the live multiplayer lobby or host/load flow objects. The implementation should isolate STS2-specific reflection or Harmony-facing code from core storage code.

Preferred sources from prior hook discovery are:

- `StartRunLobby.get_Players()` for new-run lobbies
- load-lobby or host-flow screen state if STS2 exposes player identities there
- connected-player ids only as a fallback when display names are unavailable

Each captured player should become a `PlayerIdentity`:

- `StableId` should be populated when STS2 exposes a durable id.
- `DisplayName` should use the best available visible player name.
- Blank or missing display names should normalize to `Unknown` in the labeler, while the raw roster entry remains stored.

The roster extractor should return an empty roster rather than throwing when the expected STS2 members are unavailable.

## Progress Extraction

Progress extraction is best-effort. The goal is to display useful run position text, not to validate or transform the save.

Acceptable sources are:

- readable active-save metadata already exposed by STS2 runtime objects
- safe JSON or structured parsing of the active multiplayer save if the format is readable
- existing STS2 save/run manager objects if they expose act, floor, or equivalent progress

The extracted string should be concise and user-facing:

```text
Floor 18
Act 2
```

If no safe value is available, store `null`. Do not display placeholder progress text in the picker.

Progress parsing must not:

- change the save payload
- require private field names in core storage code
- fail campaign creation
- treat malformed or unknown save contents as a fatal condition

## Data Flow

Starting a new run:

1. The host chooses `Start New Run`.
2. The runtime captures the best available metadata snapshot from the live lobby.
3. STS2 creates or updates the active multiplayer save.
4. Save sync finalizes the pending campaign using the captured roster and any progress readable from the active save.
5. The bank writes campaign metadata and payload together.

Duplicating an unmanaged active save:

1. The recovery flow detects an unmanaged active multiplayer save.
2. The runtime tries to capture current lobby roster if one is available.
3. The active save is duplicated into the bank.
4. Progress is extracted from the active save if safely readable.
5. Missing roster or progress falls back to existing safe defaults.

Syncing an existing campaign:

1. Save sync copies the active save back into the selected campaign.
2. Progress is refreshed from the active save if safely readable.
3. The original roster label remains stable unless an explicit future repair flow updates it.

## Component Changes

### CampaignLabeler

Update compact labels to match the Phase 7 display rule:

- `0` players: `Unknown party`
- `1` player: `First`
- `2` players: `First, Second`
- `3+` players: `First, Second +N`

Names should still be trimmed and blank names should display as `Unknown`.

### CampaignMetadataExtractor

Add a runtime-facing metadata extractor that returns a safe snapshot. The extractor should hide STS2-specific reflection and object-shape checks from the storage layer.

Responsibilities:

- capture roster from live lobby objects
- derive stable ids and display names when available
- extract concise progress text when safely readable
- return partial metadata instead of throwing for missing fields
- log diagnostics for unexpected STS2 object shapes

### MultiplayerSaveBank

Allow campaign creation/finalization paths to receive both roster and progress metadata. The bank should continue to generate labels from roster using `CampaignLabeler`.

The bank should not parse STS2 saves directly.

### Host Flow Runtime

Store the most recent pending metadata snapshot when the player starts a new run. Use that snapshot when finalizing the new campaign after vanilla writes the active save.

For active-save recovery duplication, ask the runtime metadata extractor for current metadata and pass whatever it can safely provide.

### Picker Model

Keep the picker row compact:

- title: campaign label
- subtitle: progress plus player count when progress exists
- subtitle: player count only when progress is missing

Do not show `Unknown progress` for missing progress.

## Error Handling

Metadata extraction must fail soft.

If roster extraction fails:

- create the campaign with an empty roster
- label it `Unknown party`
- log the extraction failure

If progress extraction fails:

- store `ActOrFloor` as `null`
- omit progress from the picker subtitle
- continue the save operation

If metadata extraction code throws unexpectedly:

- catch it at the runtime boundary
- log enough context for hook maintenance
- continue with empty roster and null progress

Raw save copy, activation, backup, and checksum safety remain higher priority than metadata.

## Testing

Core tests:

- labeler returns `First, Second +N` for large rosters
- labeler returns `First, Second` for two-player rosters
- picker subtitle omits progress when `ActOrFloor` is null
- picker subtitle includes progress before player count when known
- save bank persists non-null `ActOrFloor` when provided
- save bank still creates campaigns when roster is empty and progress is null

Runtime tests with fakes:

- new-run finalization uses a pending metadata snapshot
- unmanaged active-save duplication passes captured metadata into campaign creation
- sync-back refreshes progress without changing the original roster label
- extractor failures degrade to empty roster and null progress

In-game smoke checks:

- start a new Standard multiplayer run and verify the campaign label uses lobby names
- create a 4+ player run and verify title uses `First, Second +N`
- resume/sync a run and verify progress appears when readable
- verify save creation still succeeds if progress is absent

## Documentation

Update README status text after implementation so it no longer says roster labels are placeholder metadata. Mention that progress metadata is best-effort and may be omitted when STS2 does not expose a safe value.

The smoke setup docs should gain one short checklist item for verifying:

- real roster names appear in the picker
- large rosters compact to `First, Second +N`
- progress appears only when safely extracted

## Future Work

- Add an expanded details view showing the full roster.
- Add a metadata repair flow for campaigns created before live roster extraction existed.
- Add compatibility warnings comparing the current lobby against the stored original roster.
- Expand progress extraction if future STS2 builds expose better structured run state.
