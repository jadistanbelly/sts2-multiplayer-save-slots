# Picker Icon And Polish Follow-Up Implementation Plan

> **Archived historical plan:** This document is preserved for context only. It is not an active implementation plan; unchecked checklist items below do not indicate pending work. See `docs/superpowers/README.md` for archive policy.

> **Archived worker note:** This was the original execution guidance. Do not execute this document as current work unless a new issue or branch explicitly reactivates it.

**Goal:** Repair missing selected-character metadata from saved run payloads and make the split-panel picker look less like an external utility.

**Architecture:** Keep metadata repair in `ActivatedCampaignMetadataRepair` so existing saves gain character ids after activation without manual file edits. Keep Godot resource loading in `MultiplayerSavePickerModal`, but move fallback badge text into `MultiplayerSavePickerModel` so it is testable without a running Godot scene. Keep package output as DLL + manifest with no copied game art.

**Tech Stack:** C# `net9.0`, Godot UI controls, STS2 runtime resources, local hand-rolled test runner.

---

### Task 1: Repair Missing Character IDs

**Files:**
- Modify: `src/Runtime/ActivatedCampaignMetadataRepair.cs`
- Test: `tests/ActiveSaveSwitcherTests.cs`

- [ ] **Step 1: Write the failing test**

Add this test to `ActiveSaveSwitcherTests.All()`:

```csharp
tests.Add(new TestCase("metadata repair fills missing selected character ids", MetadataRepairFillsMissingSelectedCharacterIds));
```

Add this test body:

```csharp
private static void MetadataRepairFillsMissingSelectedCharacterIds()
{
    using var temp = new TempDirectory();
    var source = Path.Combine(temp.Path, "source.save");
    File.WriteAllText(source, "campaign");
    var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
    var metadata = bank.CreateCampaign(new CampaignCreateRequest(
        MultiplayerGameMode.Standard,
        [new PlayerIdentity("Steam:1", "Alice"), new PlayerIdentity("Steam:2", "Bob")],
        source,
        DateTimeOffset.Parse("2026-05-08T00:00:00Z")));
    var repair = new ActivatedCampaignMetadataRepair(
        bank,
        new FakeCampaignMetadataExtractor(new CampaignMetadataSnapshot(
            [new PlayerIdentity("Steam:1", "Alice", "CHARACTER.SILENT"), new PlayerIdentity("Steam:2", "Bob", "CHARACTER.IRONCLAD")],
            "Floor 5")));

    repair.RepairActivatedCampaign(metadata.CampaignId, DateTimeOffset.Parse("2026-05-08T12:00:00Z"));

    var updated = bank.GetCampaign(metadata.CampaignId);
    AssertEx.Equal("CHARACTER.SILENT", updated.Roster[0].SelectedCharacterId);
    AssertEx.Equal("CHARACTER.IRONCLAD", updated.Roster[1].SelectedCharacterId);
    AssertEx.Equal("Floor 5", updated.ActOrFloor);
}
```

- [ ] **Step 2: Verify red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: FAIL because repair preserves non-empty rosters without merging selected character ids.

- [ ] **Step 3: Implement repair merge**

Update `ActivatedCampaignMetadataRepair` so a non-empty existing roster can merge a captured roster when every player has a stable id, counts match, and ids match exactly. Preserve existing values when captured values are blank; use captured `SelectedCharacterId` when existing is blank.

- [ ] **Step 4: Verify green**

Run the same test command. Expected: all tests pass.

### Task 2: Add Regent And Player Badge Fallback

**Files:**
- Modify: `src/UI/MultiplayerSavePickerModel.cs`
- Modify: `src/UI/MultiplayerSavePickerModal.cs`
- Test: `tests/HostFlowControllerTests.cs`
- Test: `tests/HostFlowPatchTests.cs`

- [ ] **Step 1: Write failing tests**

Extend existing badge tests so:

```csharp
AssertEx.Equal("RG", badgeText.Invoke(null, ["CHARACTER.REGENT"]));
```

Extend icon-path tests so:

```csharp
AssertEx.Equal("res://images/ui/top_panel/character_icon_regent.png", characterIconPath.Invoke(null, ["CHARACTER.REGENT"]));
```

Add a model test that a player with no character id receives an initial badge:

```csharp
var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
    "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
    MultiplayerGameMode.Standard,
    "Alice",
    [new PlayerIdentity("Steam:1", "Alice")],
    DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
    DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
    null,
    "checksum",
    "Floor 1"));

AssertEx.Equal("A", row.Details!.RosterEntries[0].BadgeText);
```

- [ ] **Step 2: Verify red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: FAIL because Regent and roster badge text are not implemented.

- [ ] **Step 3: Implement display mapping**

Add `CHARACTER.REGENT` to character display name, badge, and icon path mappings. Add `BadgeText` to `MultiplayerSavePickerRosterEntry`; for missing or unknown character ids, use the first letter of the player display name or `?` if no useful name exists.

- [ ] **Step 4: Verify green**

Run the same test command. Expected: all tests pass.

### Task 3: Picker Visual Polish

**Files:**
- Modify: `src/UI/ModalUiStyling.cs`
- Modify: `src/UI/MultiplayerSavePickerModal.cs`
- Test: `tests/HostFlowPatchTests.cs`

- [ ] **Step 1: Write compile-surface tests**

Add reflection checks for:

- `CreatePreviewSectionTitle`
- `CreatePreviewFrame`
- `SetSelectedCampaignButton`

- [ ] **Step 2: Verify red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: FAIL because the helper methods do not exist.

- [ ] **Step 3: Implement visual polish**

Replace the harsh divider with a framed right preview panel. Make selected rows use the focused gold border by storing the selected button and calling `SetSelectedCampaignButton`. Move party indicators into compact rows using native icon texture when available and a styled player badge otherwise. Reduce preview text sizes and group run details under section labels.

- [ ] **Step 4: Validate**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
scripts/release-local.sh --package-only v0.1.0
scripts/smoke-setup-local.sh install --tag v0.1.0
```

Expected: tests pass, package builds, and local install succeeds.
