# Archive Restore And Delete Design

## Summary

Bucket 2 turns the current recoverable delete into explicit archive management. Active saves get both `Archive` and permanent `Delete`; archived saves can be viewed, restored, individually deleted, or mass deleted. Permanent deletion is always behind a confirmation modal.

## User Model

`Archive` means "remove this run from the active picker but keep it recoverable." It is not red.

`Delete` means "permanently remove this run." It is red and always requires confirmation.

`Archives` opens a separate picker view for archived runs. Archived runs do not appear in the active run list until restored.

## Active Picker

The active picker keeps the current split layout. When an active save is selected, the preview panel shows these actions:

- `Archive`: move the selected campaign from active saves into the archive area
- `Delete`: permanently delete the selected active campaign after confirmation
- `Continue`: continue the selected active campaign

The footer keeps `Cancel` on the left. If archived saves exist, an `Archives` button appears on the right. The active picker no longer shows `Clear Deleted Saves` directly.

## Archives View

The archives view uses the same split-panel picker shell with title `Archived Saves - <mode>`.

The left list contains archived runs for the current game mode. Selecting an archived run shows its details in the right preview panel.

When an archived run is selected, the preview panel shows:

- `Restore`: move the archived campaign back into active saves and re-add it to the active index
- `Delete`: permanently delete that archived campaign after confirmation

The footer shows `Back` on the left. If any archived runs exist, `Delete All Archives` appears on the right and permanently clears the archive area after confirmation.

## Storage Behavior

The save bank stores active campaigns under:

```text
MultiplayerSaveSlots/saves/<campaign-id>/
```

Archived campaigns remain under:

```text
MultiplayerSaveSlots/deleted/<archive-key>/
```

`archive-key` is the existing timestamp-prefixed archive directory name. The archived campaign's true campaign id remains in `metadata.json`.

Restore moves the archive directory back to `saves/<campaign-id>/` and appends the campaign id to `index.json`. If the target active directory already exists or the campaign id is already indexed, restore fails and does not overwrite.

Permanent active delete removes `saves/<campaign-id>/` and removes the campaign id from `index.json`.

Permanent archived delete removes `deleted/<archive-key>/`.

Delete all archives removes the whole `deleted/` archive tree.

## Safety

All archive, restore, and permanent delete operations must fail closed:

- campaign ids must be valid 32-digit GUID strings
- archive keys must be single directory names with no path separators
- source and destination paths must stay inside the save bank
- source trees must reject symlinks/reparse points before mutation
- restore must reject existing active targets before mutation
- failed UI actions show an error and do not continue the host flow

## Non-Goals

- no custom run naming
- no auto-pruning archive policy
- no restore from operating-system trash
- no active-save sync behavior changes
- no changes to multiplayer party compatibility checks

## Testing

Add tests for:

- listing archived campaigns by mode
- restoring an archived campaign back into active saves and `index.json`
- restore rejecting existing active targets
- permanent active delete removing the campaign and index entry
- permanent archived delete removing only the selected archive
- controller action forwarding and failure mapping
- picker model archive rows
- picker modal helper surface for archive/restore/delete views

Full verification:

```bash
git diff --check
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
scripts/release-local.sh --package-only v0.1.0
```
