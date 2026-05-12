# Picker UI Polish Implementation Plan

> **Archived historical plan:** This document is preserved for context only. It is not an active implementation plan; unchecked checklist items below do not indicate pending work. See `docs/superpowers/README.md` for archive policy.

> **Archived worker note:** This was the original execution guidance. Do not execute this document as current work unless a new issue or branch explicitly reactivates it.

**Goal:** Make the split-panel multiplayer save picker feel more native and prevent character icons from overflowing the preview.

**Architecture:** Keep data formatting in `MultiplayerSavePickerModel`. Keep Godot layout and resource loading in `MultiplayerSavePickerModal`. Add fixed icon-slot helpers so loaded STS2 textures and fallback badges share the same bounded 42x42 layout.

**Tech Stack:** C# `net9.0`, Godot C# UI nodes, STS2 runtime resources, existing reflection-based tests.

---

### Task 1: Add Icon Slot Tests

**Files:**
- Modify: `tests/HostFlowPatchTests.cs`

- [x] **Step 1: Write the failing test**

Add a new test case named `picker modal wraps character indicators in fixed slot`.

```csharp
yield return new TestCase("picker modal wraps character indicators in fixed slot", PickerModalWrapsCharacterIndicatorsInFixedSlot);
```

Add the test method:

```csharp
private static void PickerModalWrapsCharacterIndicatorsInFixedSlot()
{
    var modalType = typeof(MultiplayerSaveGameModeMap).Assembly.GetType("MultiplayerSaveSlots.UI.MultiplayerSavePickerModal");
    AssertEx.True(modalType is not null);
    var getCharacterIconSize = modalType!.GetMethod("GetCharacterIconSize", BindingFlags.Static | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("GetCharacterIconSize helper was not found");
    var createCharacterIconSlot = modalType.GetMethod("CreateCharacterIconSlot", BindingFlags.Static | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("CreateCharacterIconSlot helper was not found");
    var createCharacterIndicator = modalType.GetMethod("CreateCharacterIndicator", BindingFlags.Static | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("CreateCharacterIndicator helper was not found");

    var iconSize = getCharacterIconSize.Invoke(null, [])!;

    AssertEx.Equal("Godot.PanelContainer", createCharacterIconSlot.ReturnType.FullName);
    AssertEx.Equal("Godot.Control", createCharacterIndicator.ReturnType.FullName);
    AssertEx.Equal(42f, GetVector2Component(iconSize, "X"));
    AssertEx.Equal(42f, GetVector2Component(iconSize, "Y"));
}
```

- [x] **Step 2: Run the test to verify it fails**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: FAIL because `CreateCharacterIconSlot` is not present.

### Task 2: Implement Fixed Character Icon Slots

**Files:**
- Modify: `src/UI/MultiplayerSavePickerModal.cs`
- Modify: `src/UI/ModalUiStyling.cs`

- [x] **Step 1: Add slot styling helper**

Add `StyleIconSlotPanel(PanelContainer panel)` to `ModalUiStyling`. It should use a transparent or near-transparent panel style with no border so icons sit in a fixed layout box without adding another visible card.

- [x] **Step 2: Wrap icons and badges in a slot**

In `MultiplayerSavePickerModal`, add a private static `CreateCharacterIconSlot(Control child)` helper that:

- creates a `PanelContainer`
- sets `CustomMinimumSize` to `GetCharacterIconSize()`
- sets horizontal and vertical size flags to `ShrinkBegin`
- clips children
- applies `ModalUiStyling.StyleIconSlotPanel`
- adds the child

Update `CreateCharacterBadge` to return an unframed fixed-size label or panel inside the slot. Update `CreateCharacterIconTextureRect` to return only the texture node sized to `GetCharacterIconSize()`. Update `CreateCharacterIndicator` to return `CreateCharacterIconSlot(...)` for both texture and badge paths.

- [x] **Step 3: Run tests to verify green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

### Task 3: Polish Split-Panel Layout

**Files:**
- Modify: `src/UI/MultiplayerSavePickerModal.cs`
- Modify: `src/UI/ModalUiStyling.cs`
- Modify: `tests/HostFlowPatchTests.cs`

- [x] **Step 1: Add compile-surface tests for polish helpers**

Add a reflection test that proves the modal exposes helpers named `CreateCampaignListFrame`, `CreatePreviewContent`, and `CreateRosterPreviewRow`.

- [x] **Step 2: Run the test to verify it fails**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: FAIL until the missing helper names exist.

- [x] **Step 3: Implement layout polish**

Refine the existing split panel:

- wrap the left list in a styled frame matching the preview frame
- keep the right preview clipped inside its frame
- use smaller preview body text for summary lines
- reduce the large boxed-dashboard feel by making the inner icon slot invisible and using one panel frame per column
- preserve `Start New Run`, selected-row highlight, `Cancel`, and `Continue` behavior

- [x] **Step 4: Run tests to verify green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

### Task 4: Package Verification

**Files:**
- No source edits expected.

- [x] **Step 1: Run full docs/code checks**

Run:

```bash
git diff --check
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
scripts/release-local.sh --package-only v0.1.0
```

Expected: whitespace check clean, 149+ C# tests pass, package-only release completes.

- [x] **Step 2: Review final diff**

Run:

```bash
git diff --stat
git status --short --branch
```

Expected: only planned UI/test/plan files changed.
