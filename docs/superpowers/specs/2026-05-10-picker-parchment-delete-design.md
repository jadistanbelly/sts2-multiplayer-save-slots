# Picker Parchment UI And Delete Design

> **Archived historical spec:** This design note is preserved for context only. It is not an active spec or current work item. See `docs/superpowers/README.md` for archive policy.

## Summary

Update the split-panel picker toward the approved Scroll Card Menu direction and add a selected-save delete action. The UI should feel closer to Slay the Spire menus by using parchment/card colors, gold headers, teal primary actions, and red destructive actions.

## UI Direction

The picker keeps the current split-panel structure:

- left column: `Start New Run` and saved campaign rows
- right column: selected campaign preview
- bottom row: picker actions

The visual styling shifts from dark dashboard panels to parchment menu cards:

- warmer tan/brown panel backgrounds
- tighter square-ish card corners
- gold text and borders
- teal primary actions for `Continue` and `Start New Run`
- red destructive styling for `Delete` and permanent cleanup

Godot `StyleBoxFlat` cannot reproduce the exact ragged STS card silhouette, so this pass approximates it with parchment colors, heavier shadows, lower corner radius, and button color hierarchy.

## Delete Flow

When an existing campaign is selected, the picker shows `Delete`.

Selecting `Delete` opens a confirmation modal:

- title: `Delete Save Slot?`
- body identifies the selected campaign and explains that it will be moved out of the picker
- actions: `Cancel` and `Delete`

Confirming delete archives the campaign instead of permanently deleting it. The campaign directory moves from:

```text
MultiplayerSaveSlots/saves/<campaign-id>/
```

to:

```text
MultiplayerSaveSlots/deleted/<timestamp>-<campaign-id>/
```

The campaign id is removed from `index.json`, so the campaign disappears from the picker immediately.

## Permanent Cleanup

If archived deleted saves exist, the picker shows `Clear Deleted Saves`.

Selecting `Clear Deleted Saves` opens a separate confirmation modal:

- title: `Clear Deleted Saves?`
- body explains that archived deleted saves will be permanently removed
- actions: `Cancel` and `Clear Deleted Saves`

Confirming permanently removes the `MultiplayerSaveSlots/deleted/` tree after symlink/reparse-point safety checks.

## Safety

Delete and cleanup must fail closed:

- campaign ids must be valid 32-digit GUID strings
- the campaign must still be indexed before archive
- source and destination paths must remain inside the save bank
- archive and cleanup operations reject symlinks/reparse points before mutation
- archive destination names are deterministic and receive a numeric suffix if needed
- failed delete or cleanup shows an error and does not continue host flow

This pass does not add restore UI. The archive exists as an accidental-delete safety net and can be manually recovered from disk if needed. Permanent cleanup keeps archives from growing without bound.

## Non-Goals

- no campaign rename
- no restore-from-trash UI
- no automatic age-based pruning
- no changes to active save activation, sync, or recovery behavior
- no bundled STS2 art assets

## Testing

Add tests for:

- archiving a campaign removes it from the active picker list and moves its directory under `deleted/`
- archived destinations receive a suffix when the first destination exists
- archive rejects symlinked campaign trees before mutation
- clearing deleted saves removes the archive tree
- clearing deleted saves rejects symlinked archive trees before mutation
- controller exposes delete and clear actions
- picker modal exposes delete/cleanup confirmation helper methods

Full verification:

```bash
git diff --check
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
scripts/release-local.sh --package-only v0.1.0
```
