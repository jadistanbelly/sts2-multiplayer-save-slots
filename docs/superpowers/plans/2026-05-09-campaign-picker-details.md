# Campaign Picker Details Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add an on-demand campaign details view to the multiplayer save picker so the full stored roster and metadata are visible without cluttering compact rows.

**Architecture:** Keep all formatting in `src/UI/MultiplayerSavePickerModel.cs` so it can be covered by plain unit tests. Keep Godot code in `src/UI/MultiplayerSavePickerModal.cs` as a thin presenter that renders existing row actions plus a per-campaign Details overlay. No storage, runtime, Harmony, or save-mutation behavior changes.

**Tech Stack:** C# `net9.0`, Godot UI controls, existing console test runner, existing smoke setup shell tests.

---

### Task 1: Add Picker Details Model

**Files:**
- Modify: `src/UI/MultiplayerSavePickerModel.cs`
- Modify: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing model tests**

Add tests proving campaign rows include full details, empty rosters render a safe fallback, and `Start New Run` has no details model.

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: the new tests fail because `MultiplayerSavePickerRow` has no details model.

- [ ] **Step 2: Implement display model**

Add `MultiplayerSavePickerDetails` with `Title`, `Subtitle`, `SummaryLines`, and `RosterLines`. Add `Details` to `MultiplayerSavePickerRow`. Populate it only for campaign rows.

- [ ] **Step 3: Verify model tests**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 4: Commit**

```bash
git add src/UI/MultiplayerSavePickerModel.cs tests/HostFlowControllerTests.cs
git commit -m "feat: add campaign picker details model"
```

### Task 2: Render Details In Picker

**Files:**
- Modify: `src/UI/MultiplayerSavePickerModal.cs`

- [ ] **Step 1: Update row rendering**

Render campaign rows with the existing main action button plus a `Details` button when `row.Details` is not null. Keep `Start New Run` as the direct full-width row.

- [ ] **Step 2: Add details overlay**

Add a picker-owned overlay panel that shows the selected row details and a close button. Closing the overlay removes only the overlay and returns to the same picker.

- [ ] **Step 3: Verify compile and behavior surface**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: build succeeds with 0 warnings/errors and all tests pass.

- [ ] **Step 4: Commit**

```bash
git add src/UI/MultiplayerSavePickerModal.cs
git commit -m "feat: show campaign details in picker"
```

### Task 3: Document Phase 8

**Files:**
- Modify: `README.md`

- [ ] **Step 1: Update README status and smoke checklist**

Mention that Phase 8 adds a Details action for full roster/progress metadata. Add one smoke checklist item verifying the Details overlay shows the full roster for a 4+ player campaign.

- [ ] **Step 2: Verify docs and full branch**

Run:

```bash
rg -n "Phase 8|Details|full roster" README.md
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
tests/smoke-setup-local-tests.sh
git diff --check
```

Expected: README contains the new status/checklist text, build succeeds, unit tests pass, smoke setup tests pass, and `git diff --check` prints nothing.

- [ ] **Step 3: Commit**

```bash
git add README.md
git commit -m "docs: document campaign picker details"
```
