# Picker Quick UI Polish Design

> **Archived historical spec:** This design note is preserved for context only. It is not an active spec or current work item. See `docs/superpowers/README.md` for archive policy.

## Summary

Apply the approved Bucket 1 visual polish to the multiplayer save picker without changing save behavior. This pass uses the selected B direction: a slightly narrower save list, a wider preview panel, softer corners, equal-width action buttons, and consistent action placement.

## Scope

This pass includes:

- set the picker body to a 43/57-style split between the left save list and right selected-save preview
- make modal panels, preview frames, row cards, and buttons slightly more rounded while staying close to the Slay the Spire parchment/card feel
- use equal widths for footer/action-row buttons
- keep `Cancel` on the left and place commit/destructive actions on the right
- keep destructive actions visually red, with no non-destructive red buttons

This pass does not include:

- archive restore UI
- direct permanent delete for active saves
- run naming
- changes to save-bank storage behavior
- changes to active-save activation, sync, or recovery behavior

## Layout

The picker keeps the existing split-panel structure. The left campaign list becomes narrower, while the right preview becomes wider to reduce scrolling for party rows and run details. The selected-save `Delete` and `Continue` actions remain in the preview panel, under the selected save details.

The footer remains a global modal action row. It shows `Cancel` on the left. If deleted archives exist, `Clear Deleted Saves` appears on the right as a red destructive action. Any future footer commit action should follow the same right-side pattern.

## Styling

Corners should be moderately rounded, not pill-shaped. The target is less square than the current picker but still compatible with STS-style parchment panels. The row cards and buttons use the same corner radius so the picker feels coherent.

Action button widths should be equal within the same action row. The selected-save action row should use equal `Delete` and `Continue` widths. The footer should not appear lopsided: `Cancel` and `Clear Deleted Saves` use the same width when both are visible.

## Testing

Add reflection-level coverage for the visual constants and helper methods used by the code-built Godot UI:

- left list width is smaller than the right preview width
- the left/right widths approximate the approved 43/57 split
- action button widths are stable and equal
- button/card corner radius is exposed as a stable helper

Final verification remains:

```bash
git diff --check
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
scripts/release-local.sh --package-only v0.1.0
```
