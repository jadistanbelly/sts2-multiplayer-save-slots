# Picker Quick UI Polish Implementation Plan

> **Archived historical plan:** This document is preserved for context only. It is not an active implementation plan; unchecked checklist items below do not indicate pending work. See `docs/superpowers/README.md` for archive policy.

> **Archived worker note:** This was the original execution guidance. Do not execute this document as current work unless a new issue or branch explicitly reactivates it.

**Goal:** Apply the approved 43/57 picker layout polish with softer corners and consistent action button placement.

**Architecture:** Keep this as a code-built Godot UI polish pass. Add small static helper methods for layout and style constants so tests can verify the visual contract without launching STS2. Do not change storage, archive, restore, or naming behavior.

**Tech Stack:** C# `net9.0`, Godot C# UI nodes, existing reflection-based plain C# test harness.

---

### Task 1: Add UI Polish Contract Tests

**Files:**
- Modify: `tests/HostFlowPatchTests.cs`

- [ ] **Step 1: Add failing tests**

Add a test case named `picker modal exposes quick polish layout constants` that reflects these private/static helpers:

```csharp
foreach (var methodName in new[]
{
    "GetCampaignListFrameWidth",
    "GetCampaignListRowWidth",
    "GetPreviewFrameWidth",
    "GetPreviewContentWidth",
    "GetActionButtonWidth"
})
{
    var method = modalType!.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
    AssertEx.True(method is not null, $"{methodName} helper was not found");
}
```

Add assertions that the campaign frame is narrower than the preview frame, the preview frame is at least 520 pixels wide, and the action button width is at least 190 pixels.

Add a test case named `modal styling exposes rounded card radius` that reflects `ModalUiStyling.GetCardCornerRadius()` and asserts the returned radius is at least 10.

- [ ] **Step 2: Verify red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: tests fail because the helper methods do not exist yet.

### Task 2: Implement B Layout And Styling

**Files:**
- Modify: `src/UI/MultiplayerSavePickerModal.cs`
- Modify: `src/UI/ModalUiStyling.cs`

- [ ] **Step 1: Add picker layout helpers**

In `MultiplayerSavePickerModal`, add private static helpers:

```csharp
private static float GetCampaignListFrameWidth() => 400f;
private static float GetCampaignListRowWidth() => 370f;
private static float GetPreviewFrameWidth() => 530f;
private static float GetPreviewContentWidth() => 510f;
private static float GetActionButtonWidth() => 230f;
```

Use those helpers for the campaign list frame, row buttons, preview frame, preview root, selected-save action buttons, footer cancel button, and footer clear-deleted button.

- [ ] **Step 2: Add rounded styling helper**

In `ModalUiStyling`, add:

```csharp
public static int GetCardCornerRadius() => 12;
```

Use it in `CreatePanelStyle`, `CreateButtonStyle`, and `CreateBadgeStyle` so panels, row cards, buttons, and badges share the softer radius.

- [ ] **Step 3: Verify green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

### Task 3: Final Verification And PR Update

**Files:**
- No source edits expected.

- [ ] **Step 1: Run final verification**

Run:

```bash
git diff --check
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
scripts/release-local.sh --package-only v0.1.0
```

Expected: whitespace check clean, all C# tests pass, package-only release completes.

- [ ] **Step 2: Commit and push**

Run:

```bash
git add docs/superpowers/specs/2026-05-10-picker-quick-ui-polish-design.md docs/superpowers/plans/2026-05-10-picker-quick-ui-polish.md src/UI/MultiplayerSavePickerModal.cs src/UI/ModalUiStyling.cs tests/HostFlowPatchTests.cs
git commit -m "style: polish picker layout"
git push
```

Expected: PR #16 is updated with the bucket 1 polish commit.
