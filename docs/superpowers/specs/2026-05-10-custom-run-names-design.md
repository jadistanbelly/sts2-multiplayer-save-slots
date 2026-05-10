# Custom Run Names Design

## Goal

Let the host assign a custom name to a multiplayer save slot so runs with the same players, characters, act, or floor can still be identified quickly.

The custom name replaces the auto roster label as the primary picker title. The generated roster label remains visible as secondary context in the selected-save preview and details.

## User Experience

The selected-save preview uses the approved Option A layout:

- The preview title shows the custom name when one exists.
- A small edit icon appears directly beside the preview title.
- The icon opens a compact rename modal.
- The rename modal has one text input, `Save`, and `Cancel`.
- Saving a blank or whitespace-only name clears the custom name and restores the auto roster label as the primary title.

The bottom action area does not gain another button. Archive, Delete, and Continue keep their current placement.

## Data Model

Add an optional custom-name field to `CampaignMetadata`.

The existing generated `Label` continues to store the roster-derived label. This keeps metadata repair and roster changes independent from the user's custom name.

Picker rows derive display text like this:

1. Primary title: `CustomName` when present, otherwise `Label`.
2. Secondary subtitle: existing progress/player text.
3. Preview context: auto roster label appears under the custom title when custom name is present.

Archived saves keep their custom names because they retain the same metadata file.

## Runtime Flow

Add a controller method that updates the selected campaign name by campaign id:

- Normalize input by trimming whitespace.
- Store `null` for empty input.
- Persist metadata with the updated custom name.
- Refresh the picker model after a successful rename.
- Show the existing error popup on failure.

Renaming is allowed for active saved campaigns. Archived campaign renaming is out of scope for this pass.

## UI Notes

The edit affordance should feel like a small native glyph, not a modern toolbar button. Use a compact text glyph or small icon-like button beside the title, styled with existing parchment/gold button colors and no extra footer action.

The rename modal should be small and focused. It should not include explanatory text unless needed for an error.

## Safety And Compatibility

Existing metadata without a custom-name field must deserialize normally and behave exactly as before.

Renaming changes only campaign metadata. It must not touch active save payloads, archives, backups, or active-state files.

## Tests

Cover:

- Metadata persists a custom run name and treats omitted names as null.
- Picker row title uses custom name when present.
- Picker preview still exposes the generated roster label as context.
- Clearing the name restores the generated roster label as title.
- Controller rename persists changes and reports failures.
- UI exposes a preview edit affordance and rename modal helper methods.

