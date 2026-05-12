# Picker Parchment Delete Implementation Plan

> **Archived historical plan:** This document is preserved for context only. It is not an active implementation plan; unchecked checklist items below do not indicate pending work. See `docs/superpowers/README.md` for archive policy.

> **Archived worker note:** This was the original execution guidance. Do not execute this document as current work unless a new issue or branch explicitly reactivates it.

**Goal:** Add bounded trash deletion to the multiplayer save picker and shift picker styling toward the approved parchment menu direction.

**Architecture:** Add archive/delete primitives to `MultiplayerSaveBank` and expose them through `IHostFlowSaveBank` and `HostFlowController`. Keep all destructive UI confirmation in `MultiplayerSavePickerModal`; successful actions rebuild the picker model so rows disappear immediately. Keep visual styling in `ModalUiStyling`.

**Tech Stack:** C# `net9.0`, Godot C# UI nodes, existing plain C# test harness, local file-system save bank.

---

### Task 1: Save Bank Bounded Trash

**Files:**
- Modify: `src/Storage/SaveBankPaths.cs`
- Modify: `src/Storage/MultiplayerSaveBank.cs`
- Modify: `tests/MultiplayerSaveBankTests.cs`

- [ ] **Step 1: Write failing save-bank tests**

Add tests for archive, suffix collision, archive symlink rejection, clear deleted saves, and clear symlink rejection.

- [ ] **Step 2: Run tests and confirm red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile/test failure because archive APIs do not exist.

- [ ] **Step 3: Implement archive paths and bank APIs**

Add `DeletedDirectory`, archive destination generation, `ArchiveCampaign`, `HasDeletedCampaigns`, and `ClearDeletedCampaigns`.

- [ ] **Step 4: Run tests and confirm green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

### Task 2: Controller Delete Actions

**Files:**
- Modify: `src/Runtime/HostFlowContinuations.cs`
- Modify: `src/Runtime/HostFlowController.cs`
- Modify: `src/Runtime/Sts2HostFlowRuntime.cs`
- Modify: `src/UI/MultiplayerSavePickerModel.cs`
- Modify: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing controller tests**

Add tests proving the picker model exposes deleted-save availability, controller archive calls bank delete, controller clear calls bank cleanup, and failures return failed `OperationResult`.

- [ ] **Step 2: Run tests and confirm red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile/test failure because controller/delete APIs do not exist.

- [ ] **Step 3: Implement controller APIs**

Extend `IHostFlowSaveBank`, `Sts2SaveBankAdapter`, fake bank, `HostFlowController`, and `MultiplayerSavePickerModel`.

- [ ] **Step 4: Run tests and confirm green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

### Task 3: Picker Delete UI And Parchment Styling

**Files:**
- Modify: `src/UI/MultiplayerSavePickerModal.cs`
- Modify: `src/UI/ModalUiStyling.cs`
- Modify: `tests/HostFlowPatchTests.cs`

- [ ] **Step 1: Write failing UI helper tests**

Add reflection coverage for `ShowDeleteConfirmation`, `ShowClearDeletedConfirmation`, `DeleteSelectedCampaign`, `ClearDeletedCampaigns`, `StylePrimaryButton`, and `StyleDangerButton`.

- [ ] **Step 2: Run tests and confirm red**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: failure until helpers exist.

- [ ] **Step 3: Implement UI**

Add `Delete` and `Clear Deleted Saves` buttons, confirmation overlays, model refresh after successful operations, and parchment/teal/red styling.

- [ ] **Step 4: Run tests and confirm green**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

### Task 4: Final Verification

**Files:**
- No source edits expected.

- [ ] **Step 1: Run full verification**

Run:

```bash
git diff --check
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
scripts/release-local.sh --package-only v0.1.0
```

Expected: whitespace check clean, all C# tests pass, package-only release completes.

- [ ] **Step 2: Review diff and push PR update**

Run:

```bash
git diff --stat
git status --short --branch
git push
```

Expected: only planned spec/plan/storage/runtime/UI/test files changed, PR #16 updated.
