# Archive Restore And Delete Implementation Plan

> **Archived historical plan:** This document is preserved for context only. It is not an active implementation plan; unchecked checklist items below do not indicate pending work. See `docs/superpowers/README.md` for archive policy.

> **Archived worker note:** This was the original execution guidance. Do not execute this document as current work unless a new issue or branch explicitly reactivates it.

**Goal:** Add explicit archive, restore, and permanent delete management to the multiplayer save picker.

**Architecture:** Extend the save bank with archive listing, restore, active permanent delete, and archived permanent delete APIs. Expose those operations through `HostFlowController`, then render active and archived picker views with the existing code-built Godot modal. Keep all filesystem mutation guarded by existing path safety helpers.

**Tech Stack:** C# `net9.0`, Godot C# UI nodes, existing plain C# test harness, local filesystem save bank.

---

### Task 1: Storage Archive Management APIs

**Files:**
- Create: `src/Core/ArchivedCampaign.cs`
- Modify: `src/Storage/SaveBankPaths.cs`
- Modify: `src/Storage/MultiplayerSaveBank.cs`
- Modify: `tests/MultiplayerSaveBankTests.cs`

- [ ] **Step 1: Write failing storage tests**

Add tests for archived listing, restore, restore conflict, active permanent delete, archived permanent delete, and archive-key path validation.

- [ ] **Step 2: Verify red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile/test failures because `ArchivedCampaign`, `ListArchivedCampaigns`, `RestoreArchivedCampaign`, `DeleteCampaign`, and `DeleteArchivedCampaign` do not exist yet.

- [ ] **Step 3: Implement storage APIs**

Add `ArchivedCampaign(string ArchiveKey, CampaignMetadata Metadata)`.

Add `SaveBankPaths.ArchivedCampaignDirectory(string archiveKey)` with single-segment archive key validation.

Add `MultiplayerSaveBank` APIs:

```csharp
public IReadOnlyList<ArchivedCampaign> ListArchivedCampaigns(MultiplayerGameMode gameMode)
public CampaignMetadata RestoreArchivedCampaign(string archiveKey)
public void DeleteCampaign(string campaignId)
public void DeleteArchivedCampaign(string archiveKey)
```

Keep `ClearDeletedCampaigns()` as the mass archive cleanup API for this pass.

- [ ] **Step 4: Verify green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

### Task 2: Controller And Picker Model Plumbing

**Files:**
- Modify: `src/Runtime/HostFlowContinuations.cs`
- Modify: `src/Runtime/HostFlowController.cs`
- Modify: `src/Runtime/Sts2HostFlowRuntime.cs`
- Modify: `src/UI/MultiplayerSavePickerModel.cs`
- Modify: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing controller/model tests**

Add tests proving:

- archive picker model contains archived rows
- controller restores archived campaigns
- controller permanently deletes active campaigns
- controller permanently deletes archived campaigns
- controller reports failures for restore/delete operations

- [ ] **Step 2: Verify red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile/test failures until runtime and model APIs exist.

- [ ] **Step 3: Implement controller/model APIs**

Extend `IHostFlowSaveBank` and `Sts2SaveBankAdapter` with archive list, restore, active delete, and archived delete methods.

Add `HostFlowController.BuildArchivePickerModel`, `RestoreArchivedCampaign`, `DeleteCampaign`, and `DeleteArchivedCampaign`.

Extend `PickerRowKind` with `ArchivedCampaign`, add `ArchiveKey` to `MultiplayerSavePickerRow`, and add `MultiplayerSavePickerRow.ArchivedCampaigns(...)`.

- [ ] **Step 4: Verify green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

### Task 3: Picker UI Archive View

**Files:**
- Modify: `src/UI/MultiplayerSavePickerModal.cs`
- Modify: `tests/HostFlowPatchTests.cs`

- [ ] **Step 1: Write failing UI helper tests**

Add reflection tests for:

```text
ShowArchives
BuildArchiveFooterActions
CreateActiveCampaignActions
CreateArchivedCampaignActions
ShowArchiveConfirmation
ShowActiveDeleteConfirmation
ShowArchivedDeleteConfirmation
RestoreSelectedArchive
DeleteSelectedCampaign
DeleteSelectedArchive
```

- [ ] **Step 2: Verify red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: missing helper failures.

- [ ] **Step 3: Implement active/archive UI**

Active view:

- rename selected-save `Delete` behavior to `Archive`
- add red permanent `Delete`
- keep `Continue`
- show footer `Archives` button when archives exist

Archive view:

- title is `Archived Saves - <mode>`
- list archived rows
- show selected archive details
- selected archive actions are `Restore` and red `Delete`
- footer has `Back` left and red `Delete All Archives` right when archives exist

- [ ] **Step 4: Verify green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

### Task 4: Final Verification And PR Update

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
git add docs/superpowers/specs/2026-05-10-archive-restore-delete-design.md docs/superpowers/plans/2026-05-10-archive-restore-delete.md src/Core/ArchivedCampaign.cs src/Storage/SaveBankPaths.cs src/Storage/MultiplayerSaveBank.cs src/Runtime/HostFlowContinuations.cs src/Runtime/HostFlowController.cs src/Runtime/Sts2HostFlowRuntime.cs src/UI/MultiplayerSavePickerModel.cs src/UI/MultiplayerSavePickerModal.cs tests/MultiplayerSaveBankTests.cs tests/HostFlowControllerTests.cs tests/HostFlowPatchTests.cs
git commit -m "feat: add archive restore management"
git push
```

Expected: PR #16 is updated with Bucket 2 archive restore/delete behavior.
