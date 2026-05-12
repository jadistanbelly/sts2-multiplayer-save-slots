# Activated Metadata Repair Design

> **Archived historical spec:** This design note is preserved for context only. It is not an active spec or current work item. See `docs/superpowers/README.md` for archive policy.

## Summary

Phase 9 repairs stale display metadata for existing bank campaigns when a campaign is selected and activated. This helps campaigns created before live roster extraction existed: once the campaign payload is safely copied into `current_run_mp.save`, the existing active-save metadata extractor can read whatever STS2 exposes and update missing roster/progress metadata in the bank.

The phase is intentionally conservative. It repairs metadata only after the normal activation path succeeds, never before active-save preflight, and never by rewriting the raw multiplayer save payload.

## Goals

- Repair empty roster metadata from the activated campaign payload when STS2 can read roster data.
- Repair or refresh progress metadata from the activated campaign payload when STS2 can read act/floor data.
- Rebuild the campaign label when repaired roster data becomes available.
- Keep existing non-empty original roster metadata stable.
- Keep campaign selection usable if metadata extraction or metadata repair fails.
- Avoid new STS2 hooks.

## Non-Goals

- Do not compare the current load lobby against the stored roster.
- Do not warn about party compatibility in this phase.
- Do not repair metadata before a campaign has been safely activated.
- Do not rewrite `multiplayer_run.save` or `current_run_mp.save`.
- Do not add a manual metadata editor.

## User-Facing Behavior

For an older campaign that currently shows:

```text
Unknown party
0 players
```

the host selects that campaign as usual. After the bank payload has been activated into the vanilla active save path, the mod attempts a best-effort metadata repair. On the next picker open, the row can show the repaired label and progress:

```text
Alice, Bob
Floor 12 - 2 players
```

If repair fails, selection continues exactly as it does today and the row keeps its existing metadata.

## Data Flow

1. Existing campaign selection passes active-save preflight.
2. Load continuation preflight confirms vanilla can prepare the existing-run flow.
3. `ActiveSaveSwitcher.Activate` copies the selected bank payload into `current_run_mp.save` and updates ownership/checksum metadata.
4. The metadata repair service reads the active save through `ICampaignMetadataExtractor`.
5. If the stored roster is empty and the extractor returns roster data, the bank metadata roster and label are updated.
6. If the extractor returns progress, the bank metadata `ActOrFloor` is updated.
7. Vanilla existing-run continuation proceeds even if repair returns no metadata or throws.

## Component Changes

### ActivatedCampaignMetadataRepair

Add a small runtime service that depends on `MultiplayerSaveBank` and `ICampaignMetadataExtractor`.

Responsibilities:

- read current campaign metadata after activation
- capture active-save metadata fail-soft
- update roster only when the stored roster is empty and extracted roster is non-empty
- update progress when extracted progress is non-empty
- rebuild label from repaired roster
- preserve payload, active checksum, payload checksum, timestamps, and game mode

### HostFlowController

Call the repair service after successful activation and before `LoadExistingRun`.

The repair call is non-blocking: exceptions are caught and logged so metadata repair cannot prevent campaign selection.

### Sts2HostFlowRuntime

Wire the runtime controller to use `ActivatedCampaignMetadataRepair` with the existing `Sts2CampaignMetadataExtractor`.

## Error Handling

Metadata repair is optional. If extraction throws, metadata JSON cannot be updated, or no useful metadata is available, the mod logs the failure and continues the selected campaign load.

Activation, rollback, checksum, and recovery safety remain higher priority than metadata quality.

## Testing

Core/runtime tests should verify:

- repair updates empty roster and missing progress metadata
- repair refreshes progress while preserving an existing non-empty roster
- repair fail-soft behavior preserves existing metadata when extraction throws
- `HostFlowController` invokes repair after activation for existing campaigns
- `HostFlowController` continues loading if repair throws

Existing build, unit, smoke setup, shell syntax, and whitespace checks remain the branch gate.

## Documentation

Update the README status and smoke checklist to mention that older `Unknown party` campaigns can self-repair after they are selected and activated.

## Future Work

- Add compatibility warnings after a load-lobby hook can compare current participants with stored original roster metadata.
- Add a manual metadata repair or reset command if users need to force repair without selecting a campaign.
- Expand progress extraction if future STS2 builds expose better structured run state.
