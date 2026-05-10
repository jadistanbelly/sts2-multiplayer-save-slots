# Custom Run Names Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add custom run names that replace the auto roster label as the picker title while preserving roster/progress context in the preview and details.

**Architecture:** Store a nullable `CustomName` on campaign metadata while keeping `Label` as the generated roster label. The picker model derives display title from `CustomName ?? Label`, and runtime rename flows update only metadata through the existing controller/bank boundary. The Godot picker adds an Option A title-row edit icon that opens a compact rename modal without adding footer buttons.

**Tech Stack:** C# `net9.0`, `System.Text.Json`, existing no-NuGet console test harness, Godot UI nodes (`Button`, `Label`, `LineEdit`, containers), existing modal styling helpers.

---

## File Structure

- Modify `src/Core/CampaignMetadata.cs` - add nullable `CustomName` positional property with a default value for old JSON compatibility.
- Modify `src/Storage/MultiplayerSaveBank.cs` - add normalized metadata-only rename API.
- Modify `src/Runtime/HostFlowContinuations.cs` - expose rename through `IHostFlowSaveBank`.
- Modify `src/Runtime/Sts2HostFlowRuntime.cs` - implement rename in `Sts2SaveBankAdapter`.
- Modify `src/Runtime/HostFlowController.cs` - add UI-facing `RenameCampaign(...)` operation result wrapper.
- Modify `src/UI/MultiplayerSavePickerModel.cs` - derive row title and preview context from custom name plus generated label.
- Modify `src/UI/ModalUiStyling.cs` - add `StyleTextInput(LineEdit input)` for the rename modal.
- Modify `src/UI/MultiplayerSavePickerModal.cs` - add title-row edit icon, rename modal, and refresh flow.
- Modify `tests/MultiplayerSaveBankTests.cs` - metadata persistence and old JSON compatibility coverage.
- Modify `tests/HostFlowControllerTests.cs` - picker model and controller rename coverage.
- Modify `tests/HostFlowPatchTests.cs` - UI helper/styling reflection coverage.
- Modify `README.md` - document custom names in user-facing feature list.

---

### Task 1: Metadata And Storage Rename API

**Files:**
- Modify: `src/Core/CampaignMetadata.cs`
- Modify: `src/Storage/MultiplayerSaveBank.cs`
- Test: `tests/MultiplayerSaveBankTests.cs`

- [ ] **Step 1: Register failing storage tests**

In `tests/MultiplayerSaveBankTests.cs`, add registrations after `save bank persists metadata contents and json`:

```csharp
tests.Add(new TestCase("save bank renames campaign with normalized custom name", RenamesCampaignWithNormalizedCustomName));
tests.Add(new TestCase("save bank reads old metadata without custom name", ReadsOldMetadataWithoutCustomName));
```

- [ ] **Step 2: Add failing storage tests**

Add these methods near `PersistsMetadataContentsAndJson()`:

```csharp
private static void RenamesCampaignWithNormalizedCustomName()
{
    using var temp = new TempDirectory();
    var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
    var bank = new MultiplayerSaveBank(paths);
    var metadata = bank.CreateCampaign(new CampaignCreateRequest(
        MultiplayerGameMode.Standard,
        [new PlayerIdentity("steam:1", "phatstatss"), new PlayerIdentity("steam:2", "Magical Crocs")],
        CreatePayload(temp.Path, "vanilla.save", "run-payload"),
        DateTimeOffset.Parse("2026-05-10T12:00:00Z"),
        "Floor 5"));

    var renamed = bank.RenameCampaign(metadata.CampaignId, "  Friday Poison Run  ");

    AssertEx.Equal("Friday Poison Run", renamed.CustomName);
    AssertEx.Equal("phatstatss, Magical Crocs", renamed.Label);
    var roundTrip = bank.GetCampaign(metadata.CampaignId);
    AssertEx.Equal("Friday Poison Run", roundTrip.CustomName);
    AssertEx.Equal("phatstatss, Magical Crocs", roundTrip.Label);

    var cleared = bank.RenameCampaign(metadata.CampaignId, "   ");
    AssertEx.Equal(null, cleared.CustomName);
    AssertEx.Equal("phatstatss, Magical Crocs", cleared.Label);
}

private static void ReadsOldMetadataWithoutCustomName()
{
    using var temp = new TempDirectory();
    var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
    var bank = new MultiplayerSaveBank(paths);
    var metadata = bank.CreateCampaign(new CampaignCreateRequest(
        MultiplayerGameMode.Standard,
        [new PlayerIdentity("steam:1", "phatstatss")],
        CreatePayload(temp.Path, "vanilla.save", "run-payload"),
        DateTimeOffset.Parse("2026-05-10T12:00:00Z")));

    var jsonWithoutCustomName = """
        {
          "campaignId": "__ID__",
          "gameMode": "Standard",
          "label": "phatstatss",
          "roster": [
            {
              "stableId": "steam:1",
              "displayName": "phatstatss",
              "selectedCharacterId": null
            }
          ],
          "createdAtUtc": "2026-05-10T12:00:00+00:00",
          "lastPlayedAtUtc": "2026-05-10T12:00:00+00:00",
          "activeChecksum": null,
          "payloadChecksum": "payload",
          "actOrFloor": null
        }
        """.Replace("__ID__", metadata.CampaignId, StringComparison.Ordinal);
    File.WriteAllText(paths.MetadataPath(metadata.CampaignId), jsonWithoutCustomName);

    var roundTrip = bank.GetCampaign(metadata.CampaignId);

    AssertEx.Equal(null, roundTrip.CustomName);
    AssertEx.Equal("phatstatss", roundTrip.Label);
}
```

- [ ] **Step 3: Run tests and verify red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile failure because `CampaignMetadata.CustomName` and `MultiplayerSaveBank.RenameCampaign` do not exist.

- [ ] **Step 4: Add metadata property**

Replace `src/Core/CampaignMetadata.cs` with:

```csharp
namespace MultiplayerSaveSlots.Core;

public sealed record CampaignMetadata(
    string CampaignId,
    MultiplayerGameMode GameMode,
    string Label,
    IReadOnlyList<PlayerIdentity> Roster,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset LastPlayedAtUtc,
    string? ActiveChecksum,
    string? PayloadChecksum,
    string? ActOrFloor,
    string? CustomName = null);
```

- [ ] **Step 5: Add storage rename API**

In `src/Storage/MultiplayerSaveBank.cs`, add this method after `UpdateMetadata(...)`:

```csharp
public CampaignMetadata RenameCampaign(string campaignId, string? customName)
{
    var metadata = GetCampaign(campaignId);
    EnsureCampaignIndexed(campaignId);
    var updated = metadata with { CustomName = NormalizeCustomName(customName) };
    UpdateMetadata(updated);
    return updated;
}
```

Add this helper near `NormalizeIndex(...)`:

```csharp
private static string? NormalizeCustomName(string? customName)
{
    if (string.IsNullOrWhiteSpace(customName))
        return null;

    return customName.Trim();
}
```

- [ ] **Step 6: Run tests and verify green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass, including the two new storage tests.

- [ ] **Step 7: Commit Task 1**

```bash
git add src/Core/CampaignMetadata.cs src/Storage/MultiplayerSaveBank.cs tests/MultiplayerSaveBankTests.cs
git commit -m "feat: store custom run names"
```

---

### Task 2: Picker Model Display Rules

**Files:**
- Modify: `src/UI/MultiplayerSavePickerModel.cs`
- Test: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Register failing picker model tests**

In `tests/HostFlowControllerTests.cs`, add after `picker campaign row includes full details`:

```csharp
yield return new TestCase("picker campaign row uses custom run name", PickerCampaignRowUsesCustomRunName);
yield return new TestCase("picker campaign row falls back when custom run name is cleared", PickerCampaignRowFallsBackWhenCustomRunNameCleared);
```

- [ ] **Step 2: Add failing picker model tests**

Add near the existing picker row/detail tests:

```csharp
private static void PickerCampaignRowUsesCustomRunName()
{
    var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
        "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
        MultiplayerGameMode.Standard,
        "phatstatss, Magical Crocs",
        [new PlayerIdentity("steam:1", "phatstatss"), new PlayerIdentity("steam:2", "Magical Crocs")],
        DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
        DateTimeOffset.Parse("2026-05-08T01:00:00Z"),
        null,
        "payload",
        "Floor 5",
        "Friday Poison Run"));

    AssertEx.Equal("Friday Poison Run", row.Title);
    AssertEx.Equal("Floor 5 - 2 players", row.Subtitle);
    AssertEx.Equal("Friday Poison Run", row.Details!.Title);
    AssertEx.Equal("phatstatss, Magical Crocs", row.Details.AutoLabel);
}

private static void PickerCampaignRowFallsBackWhenCustomRunNameCleared()
{
    var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
        "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
        MultiplayerGameMode.Standard,
        "phatstatss, Magical Crocs",
        [new PlayerIdentity("steam:1", "phatstatss"), new PlayerIdentity("steam:2", "Magical Crocs")],
        DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
        DateTimeOffset.Parse("2026-05-08T01:00:00Z"),
        null,
        "payload",
        "Floor 5",
        null));

    AssertEx.Equal("phatstatss, Magical Crocs", row.Title);
    AssertEx.Equal(null, row.Details!.AutoLabel);
}
```

- [ ] **Step 3: Run tests and verify red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile failure because `MultiplayerSavePickerDetails.AutoLabel` does not exist, or assertion failures because custom names are not used.

- [ ] **Step 4: Extend picker details model**

In `src/UI/MultiplayerSavePickerModel.cs`, change `MultiplayerSavePickerDetails` to:

```csharp
public sealed record MultiplayerSavePickerDetails(
    string Title,
    string Subtitle,
    IReadOnlyList<string> SummaryLines,
    IReadOnlyList<string> RosterLines,
    string? AutoLabel = null)
{
    public IReadOnlyList<MultiplayerSavePickerRosterEntry> RosterEntries { get; init; } = [];
}
```

- [ ] **Step 5: Derive row display title**

In `MultiplayerSavePickerRow.Campaign(...)`, replace `metadata.Label` with a display title:

```csharp
public static MultiplayerSavePickerRow Campaign(CampaignMetadata metadata)
{
    var subtitle = BuildSubtitle(metadata);
    var title = DisplayTitle(metadata);

    return new MultiplayerSavePickerRow(
        PickerRowKind.Campaign,
        title,
        subtitle,
        metadata.CampaignId,
        BuildDetails(metadata, title, subtitle));
}
```

In `ArchivedCampaign(...)`, use the same title:

```csharp
public static MultiplayerSavePickerRow ArchivedCampaign(MultiplayerSaveSlots.Core.ArchivedCampaign archived)
{
    var metadata = archived.Metadata;
    var subtitle = BuildSubtitle(metadata);
    var title = DisplayTitle(metadata);

    return new MultiplayerSavePickerRow(
        PickerRowKind.ArchivedCampaign,
        title,
        subtitle,
        metadata.CampaignId,
        BuildDetails(metadata, title, subtitle),
        archived.ArchiveKey);
}
```

Add helper:

```csharp
private static string DisplayTitle(CampaignMetadata metadata) =>
    string.IsNullOrWhiteSpace(metadata.CustomName) ? metadata.Label : metadata.CustomName.Trim();
```

- [ ] **Step 6: Preserve auto label in details**

Change `BuildDetails` signature and body:

```csharp
private static MultiplayerSavePickerDetails BuildDetails(CampaignMetadata metadata, string title, string subtitle)
{
    var progress = string.IsNullOrWhiteSpace(metadata.ActOrFloor) ? "Unknown" : metadata.ActOrFloor.Trim();
    var summaryLines = new[]
    {
        $"Progress: {progress}",
        $"Players: {metadata.Roster.Count}",
        $"Created: {FormatTimestamp(metadata.CreatedAtUtc)}",
        $"Last played: {FormatTimestamp(metadata.LastPlayedAtUtc)}",
        $"Campaign id: {ShortValue(metadata.CampaignId)}",
        $"Save fingerprint: {ShortValue(metadata.PayloadChecksum ?? metadata.ActiveChecksum)}"
    };

    var rosterEntries = metadata.Roster.Count == 0
        ? [new MultiplayerSavePickerRosterEntry("Unknown party", null, "?", false)]
        : metadata.Roster
            .Select((player, index) => new MultiplayerSavePickerRosterEntry(
                $"{index + 1}. {DisplayName(player)}",
                player.SelectedCharacterId,
                BadgeText(player),
                true))
            .ToArray();
    var rosterLines = rosterEntries.Select(entry => entry.Text).ToArray();
    var autoLabel = string.Equals(title, metadata.Label, StringComparison.Ordinal)
        ? null
        : metadata.Label;

    return new MultiplayerSavePickerDetails(title, subtitle, summaryLines, rosterLines, autoLabel)
    {
        RosterEntries = rosterEntries
    };
}
```

- [ ] **Step 7: Run tests and verify green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass, including custom title and fallback coverage.

- [ ] **Step 8: Commit Task 2**

```bash
git add src/UI/MultiplayerSavePickerModel.cs tests/HostFlowControllerTests.cs
git commit -m "feat: display custom run names"
```

---

### Task 3: Controller Rename Flow

**Files:**
- Modify: `src/Runtime/HostFlowContinuations.cs`
- Modify: `src/Runtime/Sts2HostFlowRuntime.cs`
- Modify: `src/Runtime/HostFlowController.cs`
- Test: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Register failing controller tests**

In `tests/HostFlowControllerTests.cs`, add after `controller reports archive campaign failure`:

```csharp
yield return new TestCase("controller renames selected campaign", ControllerRenamesSelectedCampaign);
yield return new TestCase("controller reports rename campaign failure", ControllerReportsRenameCampaignFailure);
```

- [ ] **Step 2: Add failing controller tests**

Add near archive/delete controller tests:

```csharp
private static void ControllerRenamesSelectedCampaign()
{
    var bank = new FakeHostFlowSaveBank();

    var result = CreateController(bank).RenameCampaign(
        "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
        "  Friday Poison Run  ");

    AssertEx.True(result.Success);
    AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", bank.RenamedCampaignId);
    AssertEx.Equal("  Friday Poison Run  ", bank.RenamedCustomName);
}

private static void ControllerReportsRenameCampaignFailure()
{
    var bank = new FakeHostFlowSaveBank { RenameFailure = "rename failed" };

    var result = CreateController(bank).RenameCampaign(
        "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
        "Friday Poison Run");

    AssertEx.False(result.Success);
    AssertEx.Equal("rename failed", result.ErrorMessage);
}
```

- [ ] **Step 3: Extend fake save bank**

In `FakeHostFlowSaveBank`, add fields:

```csharp
public string? RenamedCampaignId { get; private set; }
public string? RenamedCustomName { get; private set; }
public string? RenameFailure { get; init; }
```

Add method:

```csharp
public CampaignMetadata RenameCampaign(string campaignId, string? customName)
{
    if (RenameFailure is not null)
        throw new InvalidOperationException(RenameFailure);

    RenamedCampaignId = campaignId;
    RenamedCustomName = customName;
    return Campaigns.FirstOrDefault(campaign => campaign.CampaignId == campaignId)
        ?? CampaignWithRoster([]);
}
```

- [ ] **Step 4: Run tests and verify red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile failure because `IHostFlowSaveBank.RenameCampaign` and `HostFlowController.RenameCampaign` do not exist.

- [ ] **Step 5: Add rename to runtime interface**

In `src/Runtime/HostFlowContinuations.cs`, add to `IHostFlowSaveBank`:

```csharp
CampaignMetadata RenameCampaign(string campaignId, string? customName);
```

- [ ] **Step 6: Implement adapter rename**

In `src/Runtime/Sts2HostFlowRuntime.cs`, add to `Sts2SaveBankAdapter` after `RestoreArchivedCampaign(...)`:

```csharp
public CampaignMetadata RenameCampaign(string campaignId, string? customName) =>
    _bank.RenameCampaign(campaignId, customName);
```

- [ ] **Step 7: Add controller method**

In `src/Runtime/HostFlowController.cs`, add after `RestoreArchivedCampaign(...)`:

```csharp
public OperationResult RenameCampaign(string campaignId, string? customName)
{
    try
    {
        _bank.RenameCampaign(campaignId, customName);
        return OperationResult.Ok();
    }
    catch (Exception ex)
    {
        return OperationResult.Fail(ex.Message);
    }
}
```

- [ ] **Step 8: Run tests and verify green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass, including controller rename success and failure.

- [ ] **Step 9: Commit Task 3**

```bash
git add src/Runtime/HostFlowContinuations.cs src/Runtime/Sts2HostFlowRuntime.cs src/Runtime/HostFlowController.cs tests/HostFlowControllerTests.cs
git commit -m "feat: expose campaign rename flow"
```

---

### Task 4: Option A Rename UI

**Files:**
- Modify: `src/UI/ModalUiStyling.cs`
- Modify: `src/UI/MultiplayerSavePickerModal.cs`
- Test: `tests/HostFlowPatchTests.cs`

- [ ] **Step 1: Register failing UI reflection tests**

In `tests/HostFlowPatchTests.cs`, add after `picker modal exposes active and archived save action helpers`:

```csharp
yield return new TestCase("picker modal exposes custom rename helpers", PickerModalExposesCustomRenameHelpers);
yield return new TestCase("modal styling exposes text input styling", ModalStylingExposesTextInputStyling);
```

- [ ] **Step 2: Add failing UI reflection tests**

Add near the picker modal helper reflection tests:

```csharp
private static void PickerModalExposesCustomRenameHelpers()
{
    var modalType = typeof(MultiplayerSaveGameModeMap).Assembly.GetType("MultiplayerSaveSlots.UI.MultiplayerSavePickerModal");
    AssertEx.True(modalType is not null);

    foreach (var methodName in new[]
    {
        "CreatePreviewTitleRow",
        "CreateRenameIconButton",
        "ShowRenameModal",
        "RenameSelectedCampaign"
    })
    {
        var method = modalType!.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
        AssertEx.True(method is not null, $"{methodName} helper was not found");
    }
}

private static void ModalStylingExposesTextInputStyling()
{
    var stylingType = typeof(MultiplayerSaveGameModeMap).Assembly.GetType("MultiplayerSaveSlots.UI.ModalUiStyling");
    AssertEx.True(stylingType is not null);

    var method = stylingType!.GetMethod("StyleTextInput", BindingFlags.Static | BindingFlags.Public);
    AssertEx.True(method is not null, "StyleTextInput helper was not found");
}
```

- [ ] **Step 3: Run tests and verify red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: failures for missing rename helper methods and `StyleTextInput`.

- [ ] **Step 4: Add text input styling**

In `src/UI/ModalUiStyling.cs`, add after `StyleDangerButton(...)`:

```csharp
public static void StyleTextInput(LineEdit input)
{
    input.AddThemeStyleboxOverride("normal", CreateButtonStyle(new Color(0.20f, 0.12f, 0.08f, 1f), PanelBorder, 2));
    input.AddThemeStyleboxOverride("focus", CreateButtonStyle(new Color(0.24f, 0.15f, 0.09f, 1f), FocusBorder, 3));
    input.AddThemeColorOverride("font_color", BodyText);
    input.AddThemeColorOverride("font_placeholder_color", new Color(0.98f, 0.94f, 0.84f, 0.55f));
    input.AddThemeFontSizeOverride("font_size", 22);
    input.CustomMinimumSize = new Vector2(480, 46);
}
```

- [ ] **Step 5: Add rename button field**

In `src/UI/MultiplayerSavePickerModal.cs`, add a field beside other button fields:

```csharp
private Button? _renameButton;
```

In `RenderPreview(...)`, set it to null with the other preview buttons:

```csharp
_renameButton = null;
```

- [ ] **Step 6: Replace preview title rendering**

In `RenderPreview(...)`, replace:

```csharp
_previewRoot.AddChild(CreatePreviewLabel(details.Title, 27, HorizontalAlignment.Center));
_previewRoot.AddChild(CreatePreviewLabel(details.Subtitle, 19, HorizontalAlignment.Center));
```

with:

```csharp
_previewRoot.AddChild(CreatePreviewTitleRow(row, details));
if (!string.IsNullOrWhiteSpace(details.AutoLabel))
    _previewRoot.AddChild(CreatePreviewLabel(details.AutoLabel, 17, HorizontalAlignment.Center));
_previewRoot.AddChild(CreatePreviewLabel(details.Subtitle, 19, HorizontalAlignment.Center));
```

- [ ] **Step 7: Add title row and edit icon helpers**

Add these methods near `CreatePreviewContent()`:

```csharp
private Control CreatePreviewTitleRow(MultiplayerSavePickerRow row, MultiplayerSavePickerDetails details)
{
    var titleRow = new HBoxContainer
    {
        SizeFlagsHorizontal = SizeFlags.ExpandFill,
        Alignment = BoxContainer.AlignmentMode.Center
    };
    titleRow.AddThemeConstantOverride("separation", 8);

    var title = CreatePreviewLabel(details.Title, 27, HorizontalAlignment.Center);
    title.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
    titleRow.AddChild(title);

    if (row.Kind == PickerRowKind.Campaign)
    {
        _renameButton = CreateRenameIconButton();
        titleRow.AddChild(_renameButton);
    }

    return titleRow;
}

private Button CreateRenameIconButton()
{
    var button = new Button
    {
        Text = "✎",
        TooltipText = "Rename",
        CustomMinimumSize = new Vector2(40, 34),
        SizeFlagsHorizontal = SizeFlags.ShrinkBegin,
        SizeFlagsVertical = SizeFlags.ShrinkCenter
    };
    ModalUiStyling.StyleButton(button);
    button.AddThemeFontSizeOverride("font_size", 18);
    button.Pressed += ShowRenameModal;
    return button;
}
```

The `✎` glyph is intentionally non-ASCII because this UI affordance is an icon-like edit marker.

- [ ] **Step 8: Add rename modal**

Add after `ShowConfirmation(...)`:

```csharp
private void ShowRenameModal()
{
    if (_selectedCampaign?.Kind != PickerRowKind.Campaign)
        return;

    CloseDetails();

    var overlay = new Control
    {
        Name = "MultiplayerSaveRename",
        MouseFilter = MouseFilterEnum.Stop
    };
    overlay.SetAnchorsPreset(LayoutPreset.FullRect);
    AddChild(overlay);
    _detailsOverlay = overlay;

    var panel = ModalUiStyling.CreatePanel(new Vector2(620, 270), 310, 135);
    overlay.AddChild(panel);

    var root = new VBoxContainer
    {
        SizeFlagsHorizontal = SizeFlags.ExpandFill,
        SizeFlagsVertical = SizeFlags.ExpandFill
    };
    root.AddThemeConstantOverride("separation", 14);
    panel.AddChild(root);

    var title = new Label
    {
        Text = "Rename Save Slot",
        HorizontalAlignment = HorizontalAlignment.Center,
        AutowrapMode = TextServer.AutowrapMode.WordSmart
    };
    ModalUiStyling.StyleTitle(title);
    root.AddChild(title);

    var input = new LineEdit
    {
        Text = _selectedCampaign.Title,
        PlaceholderText = "Run name"
    };
    ModalUiStyling.StyleTextInput(input);
    root.AddChild(input);

    var actions = new HBoxContainer
    {
        SizeFlagsHorizontal = SizeFlags.ExpandFill
    };
    actions.AddThemeConstantOverride("separation", 12);
    root.AddChild(actions);

    var cancel = new Button { Text = "Cancel", CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44) };
    ModalUiStyling.StyleButton(cancel);
    cancel.Pressed += CloseDetails;
    actions.AddChild(cancel);

    actions.AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

    var save = new Button { Text = "Save", CustomMinimumSize = new Vector2(GetActionButtonWidth(), 44) };
    ModalUiStyling.StylePrimaryButton(save);
    save.Pressed += () => RenameSelectedCampaign(input.Text);
    actions.AddChild(save);

    input.GrabFocus();
    input.SelectAll();
}
```

- [ ] **Step 9: Add rename action**

Add near other selected-campaign actions:

```csharp
private void RenameSelectedCampaign(string? customName)
{
    var campaignId = _selectedCampaign?.CampaignId;
    if (string.IsNullOrWhiteSpace(campaignId))
        return;

    var result = _controller.RenameCampaign(campaignId, customName);
    if (!result.Success)
    {
        ShowError(result.ErrorMessage ?? "Unable to rename multiplayer save.");
        return;
    }

    RefreshPicker();
}
```

- [ ] **Step 10: Include auto label in details modal**

Replace `BuildDetailsBody(...)` with:

```csharp
private static string BuildDetailsBody(MultiplayerSavePickerDetails details)
{
    var summary = string.Join('\n', details.SummaryLines);
    var roster = $"Roster\n{string.Join('\n', details.RosterLines)}";
    return string.IsNullOrWhiteSpace(details.AutoLabel)
        ? $"{summary}\n\n{roster}"
        : $"{details.AutoLabel}\n\n{summary}\n\n{roster}";
}
```

- [ ] **Step 11: Run tests and verify green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass, including UI reflection tests.

- [ ] **Step 12: Commit Task 4**

```bash
git add src/UI/ModalUiStyling.cs src/UI/MultiplayerSavePickerModal.cs tests/HostFlowPatchTests.cs
git commit -m "feat: add run rename picker UI"
```

---

### Task 5: Documentation And Full Verification

**Files:**
- Modify: `README.md`

- [ ] **Step 1: Update README feature list**

In `README.md`, under `## What It Does`, add this bullet after the save picker bullet:

```markdown
- Lets the host rename save slots so themed or similar-looking runs are easier to distinguish.
```

- [ ] **Step 2: Update usage notes**

In `README.md`, under `## How To Use`, replace step 5 with:

```markdown
5. For an existing campaign, inspect the preview, use the small edit icon beside the title to rename the slot if needed, and press `Continue`.
```

- [ ] **Step 3: Run whitespace check**

Run:

```bash
git diff --check
```

Expected: no output and exit code 0.

- [ ] **Step 4: Run full build**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
```

Expected: build succeeds with 0 errors.

- [ ] **Step 5: Run full test harness**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 6: Run package-only release check**

Run:

```bash
scripts/release-local.sh --package-only v0.1.0
```

Expected: release artifact is created at `artifacts/release/MultiplayerSaveSlots-v0.1.0.zip`.

- [ ] **Step 7: Run smoke setup tests**

Run:

```bash
tests/smoke-setup-local-tests.sh
```

Expected: all smoke setup tests pass.

- [ ] **Step 8: Commit Task 5**

```bash
git add README.md
git commit -m "docs: document custom run names"
```

- [ ] **Step 9: Prepare PR**

Run:

```bash
git status --short --branch
git log --oneline --decorate -5
```

Expected: branch is clean and contains Task 1-5 commits on top of `main`.

Create a PR with summary:

```markdown
## Summary
- Adds custom run names stored on campaign metadata.
- Shows custom names as picker titles while preserving roster/progress context.
- Adds a small title edit icon and compact rename modal.

## Test Plan
- [x] `git diff --check`
- [x] `DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln`
- [x] `DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj`
- [x] `scripts/release-local.sh --package-only v0.1.0`
- [x] `tests/smoke-setup-local-tests.sh`
```

---

## Self-Review

Spec coverage:

- Custom name replaces the roster label as primary picker title: Task 2.
- Roster label stays visible in preview/details: Task 2 and Task 4.
- Option A edit icon next to preview title: Task 4.
- Compact rename modal with Save/Cancel: Task 4.
- Blank name clears custom name: Task 1 and Task 3.
- Metadata-only rename with old JSON compatibility: Task 1.
- Controller and UI failure handling: Task 3 and Task 4.
- Tests and docs: Task 1 through Task 5.

Placeholder scan: no unresolved markers or unspecified implementation steps are intentionally left in this plan.

Type consistency:

- Metadata property name is `CustomName`.
- Storage method is `MultiplayerSaveBank.RenameCampaign(string campaignId, string? customName)`.
- Runtime interface method is `IHostFlowSaveBank.RenameCampaign(string campaignId, string? customName)`.
- Controller method is `HostFlowController.RenameCampaign(string campaignId, string? customName)`.
- Details context property is `MultiplayerSavePickerDetails.AutoLabel`.
