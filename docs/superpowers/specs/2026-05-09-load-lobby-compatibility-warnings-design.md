# Load-Lobby Compatibility Warnings Design

> **Archived historical spec:** This design note is preserved for context only. It is not an active spec or current work item. See `docs/superpowers/README.md` for archive policy.

## Summary

Phase 10 adds a conservative warning when an existing campaign's stored original roster appears to differ from the current load lobby participants. The warning runs at the host's embark attempt, after the campaign has already been safely activated and after vanilla has built the load lobby. It does not rewrite saves, does not alter lobby membership, and does not replace vanilla validation.

The warning is intentionally one-shot for the same mismatch. The first embark attempt shows the warning and returns to the lobby; a second attempt with the same participant set proceeds to vanilla's normal behavior.

## Goals

- Compare a selected campaign's stored roster against the current load lobby participants.
- Warn about missing original players and extra current participants when stable player ids are available.
- Avoid warnings when roster metadata is empty or lacks stable ids.
- Avoid blocking forever: show one warning per exact selected-campaign/current-lobby mismatch.
- Preserve vanilla party validation and all save-safety behavior.
- Keep comparison logic testable without Godot or STS2 runtime objects.

## Non-Goals

- Do not add, remove, or substitute players.
- Do not bypass vanilla "same original party" rules.
- Do not rewrite `SerializableRun`, `multiplayer_run.save`, or `current_run_mp.save`.
- Do not warn from the picker before a load lobby exists.
- Do not show compatibility warnings for `Start New Run`.

## User-Facing Behavior

When the host selects an existing campaign, the flow remains unchanged until the loaded-run lobby appears.

If the host presses Embark and the current participant ids do not match the stored campaign roster, the mod shows a warning:

```text
Multiplayer Save Slots

This lobby may not match the selected campaign's original party.

Missing original players: Alice
Extra current players: Casey

Press Embark again to continue anyway, or adjust the lobby before starting.
```

The first matching warning returns `false` from the embark permission check so the host stays in the lobby. Pressing Embark again with the same selected campaign and same participant id set lets vanilla validation continue. If the participant set changes, the warning can appear again.

## Architecture

### CampaignCompatibilityChecker

A pure runtime component compares:

- expected roster from `CampaignMetadata.Roster`
- current roster from the load lobby

It only compares stable ids. If expected or current stable ids are unavailable, it returns no warning to avoid false positives.

The result contains:

- stable warning key
- title
- message
- missing original player display text
- extra current player display text

### HostFlowSession

The session stores the last acknowledged compatibility warning key. It resets when a new campaign or new-run flow is selected. This lets the first embark attempt warn and the second identical attempt proceed.

### LoadLobbyCompatibilityGuard

The guard owns the runtime decision:

1. If there is no selected campaign, allow.
2. If the campaign metadata cannot be loaded, allow and log.
3. If current lobby participants cannot be read, allow and log.
4. If comparison returns no warning, allow.
5. If the warning key is already acknowledged, allow.
6. Otherwise record the key, show the warning, and return false.

### LoadLobbyCompatibilityPatch

Patch `NMultiplayerLoadGameScreen.ShouldAllowRunToBegin()` by wrapping the returned `Task<bool>`.

The wrapper:

1. awaits vanilla's result
2. returns `false` immediately if vanilla denied the run
3. runs the compatibility guard only when vanilla allowed the run
4. returns the guard result

The runtime reads `NMultiplayerLoadGameScreen._runLobby`, then builds current participants from `LoadRunLobby.ConnectedPlayerIds`, `LoadRunLobby.NetService.Platform`, and the local player id.

## Error Handling

Compatibility warnings must fail open. Any exception while reading lobby state, loading campaign metadata, resolving names, or showing the warning should be logged and should not block vanilla flow.

The feature is a warning layer only. Save activation, recovery, sync, metadata repair, and vanilla lobby validation keep priority.

## Testing

Core tests:

- matching stable ids produce no warning
- missing expected players produce a warning
- extra current players produce a warning
- empty expected roster produces no warning
- roster entries without stable ids produce no warning
- warning keys are stable for identical participant sets

Runtime tests:

- guard returns false and records a warning the first time
- guard returns true on the second identical mismatch
- guard returns true when no selected campaign exists
- async patch helper preserves vanilla `false`
- async patch helper applies guard after vanilla `true`

Build verification covers the STS2/Harmony compile surface.

## Documentation

Update README status and the smoke checklist with an embark-time compatibility-warning check for existing campaign loads.

## Future Work

- Replace the one-shot warning with an explicit two-button continue/cancel modal if the STS2 UI API provides a clean confirm dialog.
- Expand comparison details if STS2 exposes richer load-lobby player state.
- Add in-game smoke fixtures that exercise mismatched participants when a reliable multiplayer harness exists.
