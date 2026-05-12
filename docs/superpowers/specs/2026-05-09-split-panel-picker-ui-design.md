# Split Panel Picker UI Design

> **Archived historical spec:** This design note is preserved for context only. It is not an active spec or current work item. See `docs/superpowers/README.md` for archive policy.

## Summary

Replace the current campaign picker list-with-details-button layout with a split-panel picker. The goal is to make save selection easier to scan and compare while keeping the UI visually compatible with Slay the Spire 2 menus.

## Goals

- Keep the first release conservative and native-looking.
- Keep `Start New Run` obvious and available without selecting a campaign.
- Show campaign rows in a left-side list.
- Show the selected campaign's full details in a right-side preview.
- Use STS2 runtime character icons when available.
- Fall back to compact native-styled character badges when icon loading fails.
- Avoid emojis and external-looking icon packs.
- Preserve all current save activation, recovery, and sync behavior.

## Non-Goals

- Do not add campaign rename/delete controls in this phase.
- Do not create or redistribute copied STS2 art assets in the mod package.
- Do not add generated art assets.
- Do not change save metadata schema beyond existing character-id support.
- Do not rewrite recovery modal behavior except where shared styling helpers are reused.

## Layout

The picker remains a centered modal titled `Multiplayer Saves - Standard`, `Daily`, or `Custom`.

The body is split into two columns:

- Left panel: action list
- Right panel: selected run preview

The left panel starts with a full-width `Start New Run` button. Campaign rows appear below it in last-played order. A selected campaign row is visually highlighted with the existing warm gold focus language.

Campaign rows show:

- compact roster label
- progress and player count
- short id only when needed for duplicate-looking rows
- one compact character indicator per captured character, capped to fit the row

The right preview shows:

- selected campaign label
- progress and player count
- full roster with character names/icons when available
- created time
- last played time
- short campaign id
- save fingerprint

The bottom action row contains:

- `Cancel`
- `Continue` when an existing campaign is selected

Selecting `Start New Run` continues through the existing start-new flow immediately. Selecting a campaign row only changes the selected preview; pressing `Continue` activates it.

## Icon Strategy

The UI tries to load native STS2 character icons at runtime:

- `CHARACTER.IRONCLAD` -> `res://images/ui/top_panel/character_icon_ironclad.png`
- `CHARACTER.SILENT` -> `res://images/ui/top_panel/character_icon_silent.png`
- `CHARACTER.DEFECT` -> `res://images/ui/top_panel/character_icon_defect.png`
- `CHARACTER.NECROBINDER` -> `res://images/ui/top_panel/character_icon_necrobinder.png`

The mod does not package copies of those icons. It references the installed game's resources through Godot `ResourceLoader`.

If an icon cannot be loaded, the UI renders a compact badge:

- `IC` for Ironclad
- `SI` for Silent
- `DE` for Defect
- `NE` for Necrobinder
- `??` for unknown characters

Badges use the same panel colors, gold border, and body font as the rest of the modal.

## Model

Keep display string preparation in `MultiplayerSavePickerModel`.

Add selected-run friendly display fields only where they reduce Godot UI complexity:

- selected campaign row identity
- full summary lines
- roster display lines
- character ids already captured on `PlayerIdentity`

Do not make the model depend on Godot texture or resource types. Icon loading belongs in the Godot modal layer.

## Error Handling

The UI must fail soft:

- Missing icons render badges.
- Missing selected character id renders no icon or an unknown badge depending on available context.
- Missing roster still shows `Unknown party`.
- Missing progress shows `Progress: Unknown`.
- If campaign selection fails during `Continue`, existing recovery/error handling remains in charge.

## Testing

Add model tests for:

- split-panel model starts with `Start New Run`
- first campaign is selected by default when present
- empty campaign list renders a preview message
- duplicate-looking rows keep short id disambiguation
- character badge labels are stable

Add Godot compile-surface tests for:

- split-panel modal helper methods exist
- native icon path mapping is stable
- badge fallback helper exists

Use the existing full test command and local package validation:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
scripts/release-local.sh --package-only v0.1.0
```

Manual smoke test:

1. Launch STS2 with the locally installed package.
2. Open `Multiplayer -> Host -> Standard`.
3. Confirm the split-panel picker appears.
4. Select each campaign row and confirm the right preview updates.
5. Confirm character icons appear for captured characters, or badges appear if icons fail to load.
6. Confirm `Continue` loads the selected save.
7. Confirm `Start New Run` still follows the existing start-new/recovery flow.
