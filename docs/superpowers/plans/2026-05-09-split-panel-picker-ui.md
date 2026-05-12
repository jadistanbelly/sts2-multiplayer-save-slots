# Split Panel Picker UI Implementation Plan

> **Archived historical plan:** This document is preserved for context only. It is not an active implementation plan; unchecked checklist items below do not indicate pending work. See `docs/superpowers/README.md` for archive policy.

> **Archived worker note:** This was the original execution guidance. Do not execute this document as current work unless a new issue or branch explicitly reactivates it.

**Goal:** Replace the current list-plus-details picker with a native-looking split-panel picker that previews selected campaigns and uses STS2 character icons with badge fallback.

**Architecture:** Keep picker data formatting and fallback labels in `MultiplayerSavePickerModel` so tests do not need a running Godot scene. Keep Godot resource loading, panel layout, row selection state, and Continue button behavior in `MultiplayerSavePickerModal`. Preserve the existing `SelectStartNewRun` and `SelectExistingCampaign` controller calls.

**Tech Stack:** C# `net9.0`, Godot UI controls, Harmony-backed runtime, local hand-rolled test runner.

---

### Task 1: Add Picker Model Helpers

**Files:**
- Modify: `src/UI/MultiplayerSavePickerModel.cs`
- Test: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing tests**

Add tests proving that the model exposes campaign rows, a default selected campaign, an empty preview message, and stable badge labels:

```csharp
yield return new TestCase("picker model exposes default selected campaign", PickerModelExposesDefaultSelectedCampaign);
yield return new TestCase("picker model describes empty campaign list", PickerModelDescribesEmptyCampaignList);
yield return new TestCase("picker character badge labels are stable", PickerCharacterBadgeLabelsAreStable);
```

- [ ] **Step 2: Run test to verify it fails**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: FAIL because model helpers do not exist.

- [ ] **Step 3: Implement model helpers**

Add properties/methods equivalent to:

```csharp
public IReadOnlyList<MultiplayerSavePickerRow> CampaignRows =>
    Rows.Where(row => row.Kind == PickerRowKind.Campaign).ToList();

public MultiplayerSavePickerRow? DefaultSelectedCampaign =>
    CampaignRows.FirstOrDefault();

public static string EmptyPreviewTitle => "No saved runs";

public static string CharacterBadgeText(string? selectedCharacterId)
{
    if (string.IsNullOrWhiteSpace(selectedCharacterId))
        return "??";

    return selectedCharacterId.Trim().ToUpperInvariant() switch
    {
        "CHARACTER.IRONCLAD" => "IC",
        "CHARACTER.SILENT" => "SI",
        "CHARACTER.DEFECT" => "DE",
        "CHARACTER.NECROBINDER" => "NE",
        _ => "??"
    };
}
```

Badge labels:

- `CHARACTER.IRONCLAD` -> `IC`
- `CHARACTER.SILENT` -> `SI`
- `CHARACTER.DEFECT` -> `DE`
- `CHARACTER.NECROBINDER` -> `NE`
- unknown/missing -> `??`

- [ ] **Step 4: Run test to verify pass**

Run the same test command. Expected: PASS.

### Task 2: Add Native Character Icon Mapping

**Files:**
- Modify: `src/UI/MultiplayerSavePickerModal.cs`
- Test: `tests/HostFlowPatchTests.cs`

- [ ] **Step 1: Write failing tests**

Add tests that reflect private static helpers:

```csharp
yield return new TestCase("picker modal maps native character icon paths", PickerModalMapsNativeCharacterIconPaths);
yield return new TestCase("picker modal exposes badge fallback helper", PickerModalExposesBadgeFallbackHelper);
```

Expected icon paths:

```text
res://images/ui/top_panel/character_icon_ironclad.png
res://images/ui/top_panel/character_icon_silent.png
res://images/ui/top_panel/character_icon_defect.png
res://images/ui/top_panel/character_icon_necrobinder.png
```

- [ ] **Step 2: Run test to verify it fails**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: FAIL because helpers do not exist.

- [ ] **Step 3: Implement helpers**

Add private helpers:

```csharp
private static string? CharacterIconPath(string? selectedCharacterId)
{
    if (string.IsNullOrWhiteSpace(selectedCharacterId))
        return null;

    return selectedCharacterId.Trim().ToUpperInvariant() switch
    {
        "CHARACTER.IRONCLAD" => "res://images/ui/top_panel/character_icon_ironclad.png",
        "CHARACTER.SILENT" => "res://images/ui/top_panel/character_icon_silent.png",
        "CHARACTER.DEFECT" => "res://images/ui/top_panel/character_icon_defect.png",
        "CHARACTER.NECROBINDER" => "res://images/ui/top_panel/character_icon_necrobinder.png",
        _ => null
    };
}

private static Texture2D? TryLoadCharacterIcon(string? selectedCharacterId)
{
    var path = CharacterIconPath(selectedCharacterId);
    if (path is null || !ResourceLoader.Exists(path))
        return null;

    return ResourceLoader.Load<Texture2D>(path);
}

private static Control CreateCharacterBadge(string? selectedCharacterId)
{
    var badge = new Label
    {
        Text = MultiplayerSavePickerModel.CharacterBadgeText(selectedCharacterId),
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
        CustomMinimumSize = new Vector2(38, 34)
    };
    ModalUiStyling.StyleBody(badge, 16);
    return badge;
}

private static Control CreateCharacterIndicator(string? selectedCharacterId)
{
    var texture = TryLoadCharacterIcon(selectedCharacterId);
    if (texture is null)
        return CreateCharacterBadge(selectedCharacterId);

    return new TextureRect
    {
        Texture = texture,
        ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional,
        StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
        CustomMinimumSize = new Vector2(34, 34)
    };
}
```

`CreateCharacterIndicator` should try `ResourceLoader.Load<Texture2D>(path)` first and fall back to `CreateCharacterBadge`.

- [ ] **Step 4: Run test to verify pass**

Run the same test command. Expected: PASS.

### Task 3: Build Split Panel Modal

**Files:**
- Modify: `src/UI/MultiplayerSavePickerModal.cs`
- Test: `tests/HostFlowPatchTests.cs`

- [ ] **Step 1: Write failing tests**

Add compile-surface tests for helpers:

```csharp
yield return new TestCase("picker modal exposes split panel helpers", PickerModalExposesSplitPanelHelpers);
```

Assert methods exist:

- `BuildCampaignList`
- `BuildPreviewPanel`
- `SelectCampaignPreview`
- `ContinueSelectedCampaign`

- [ ] **Step 2: Run test to verify it fails**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: FAIL because split-panel helpers do not exist.

- [ ] **Step 3: Implement split panel**

Update `BuildUi`:

- title stays at top
- `Start New Run` is a button at top of the left column
- campaign rows update `_selectedCampaign` and rebuild preview
- right column shows the selected campaign details
- `Continue` calls `SelectRow(_selectedCampaign)` and is disabled when no campaign exists
- `Cancel` remains bottom-level

Keep `SelectRow` as the single place that calls controller methods and recovery handling.

- [ ] **Step 4: Run test to verify pass**

Run the same test command. Expected: PASS.

### Task 4: Validate and Install

**Files:**
- Modify: `README.md` if manual smoke checklist text needs updating

- [ ] **Step 1: Run full tests**

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 2: Build package**

```bash
scripts/release-local.sh --package-only v0.1.0
```

Expected: Release build succeeds, tests pass, zip is created.

- [ ] **Step 3: Install local smoke build**

```bash
scripts/smoke-setup-local.sh install --tag v0.1.0
```

Expected: package installs into the local STS2 mods folder.

- [ ] **Step 4: Commit implementation**

```bash
git add README.md src/UI/MultiplayerSavePickerModel.cs src/UI/MultiplayerSavePickerModal.cs tests/HostFlowControllerTests.cs tests/HostFlowPatchTests.cs
git commit -m "feat: redesign picker as split panel"
```

Expected: branch contains the UI redesign commits and a clean worktree.
