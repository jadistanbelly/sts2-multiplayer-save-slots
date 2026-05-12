# Activated Metadata Repair Implementation Plan

> **Archived historical plan:** This document is preserved for context only. It is not an active implementation plan; unchecked checklist items below do not indicate pending work. See `docs/superpowers/README.md` for archive policy.

> **Archived worker note:** This was the original execution guidance. Do not execute this document as current work unless a new issue or branch explicitly reactivates it.

**Goal:** Repair missing campaign display metadata after an existing bank campaign is activated into the vanilla active multiplayer save path.

**Architecture:** Add a small fail-soft runtime repair service that reads active-save metadata through `ICampaignMetadataExtractor` and updates only bank metadata. Thread that service into `HostFlowController` after successful activation, preserving existing save mutation and rollback behavior.

**Tech Stack:** C# `net9.0`, existing storage/runtime services, existing console test runner, existing smoke setup shell tests.

---

### Task 1: Add Metadata Repair Service

**Files:**
- Create: `src/Runtime/ActivatedCampaignMetadataRepair.cs`
- Modify: `src/Runtime/HostFlowContinuations.cs`
- Modify: `tests/ActiveSaveSwitcherTests.cs`

- [ ] **Step 1: Write failing repair service tests**

Add tests for repairing empty roster/progress metadata, preserving existing roster while refreshing progress, and fail-soft behavior when the extractor throws.

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: tests fail because the repair service type does not exist.

- [ ] **Step 2: Implement the service**

Create `IActivatedCampaignMetadataRepair`, `NoOpActivatedCampaignMetadataRepair`, and `ActivatedCampaignMetadataRepair`. The concrete service should catch extraction/update failures, log them, and preserve save payloads.

- [ ] **Step 3: Verify service tests**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 4: Commit**

```bash
git add src/Runtime/ActivatedCampaignMetadataRepair.cs src/Runtime/HostFlowContinuations.cs tests/ActiveSaveSwitcherTests.cs
git commit -m "feat: repair activated campaign metadata"
```

### Task 2: Thread Repair Through Existing Selection

**Files:**
- Modify: `src/Runtime/HostFlowController.cs`
- Modify: `src/Runtime/Sts2HostFlowRuntime.cs`
- Modify: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing controller tests**

Add tests proving existing-campaign selection invokes repair after activation and continues if repair throws.

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: tests fail because `HostFlowController` does not call a repair service.

- [ ] **Step 2: Implement controller and runtime wiring**

Add an optional `IActivatedCampaignMetadataRepair` dependency to `HostFlowController`. Call it after activation succeeds and before `LoadExistingRun`. Wire STS2 runtime controller creation to pass `ActivatedCampaignMetadataRepair`.

- [ ] **Step 3: Verify controller tests**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 4: Commit**

```bash
git add src/Runtime/HostFlowController.cs src/Runtime/Sts2HostFlowRuntime.cs tests/HostFlowControllerTests.cs
git commit -m "feat: repair metadata after campaign activation"
```

### Task 3: Document And Verify

**Files:**
- Modify: `README.md`

- [ ] **Step 1: Update README status and smoke checklist**

Mention Phase 9 metadata repair and add a smoke note for selecting an older `Unknown party` campaign, then reopening the picker to confirm repaired metadata when STS2 exposes it.

- [ ] **Step 2: Run full verification**

Run:

```bash
rg -n "Phase 9|metadata repair|Unknown party" README.md
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
tests/smoke-setup-local-tests.sh
bash -n scripts/smoke-setup-local.sh tests/smoke-setup-local-tests.sh
git diff --check
```

Expected: README contains the new text, build succeeds, unit tests pass, smoke setup tests pass, shell syntax passes, and `git diff --check` prints nothing.

- [ ] **Step 3: Commit**

```bash
git add README.md
git commit -m "docs: document activated metadata repair"
```
