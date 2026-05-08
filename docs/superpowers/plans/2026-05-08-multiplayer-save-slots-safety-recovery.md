# Multiplayer Save Slots Safety Recovery Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add user-visible recovery choices when Multiplayer Save Slots detects an active multiplayer save that cannot safely be replaced.

**Architecture:** Keep save safety decisions in testable runtime models and keep Godot UI as a thin presenter. `HostFlowController` will expose recovery models and retry helpers. A storage-backed recovery service will support two conservative actions: duplicate the active vanilla save into a bank campaign, or sync unsynced active changes back to the owning campaign. The picker will show a small recovery modal instead of only an error popup when recovery actions are available.

**Tech Stack:** C# `net9.0`, Harmony/Godot UI nodes, existing save-bank and active-save switcher services, `System.Text.Json`, and the no-NuGet console test harness.

---

## Scope Check

This plan implements:

- recovery models for unmanaged active saves and unsynced managed saves
- controller retry flow after a recovery action succeeds
- storage-backed recovery actions using existing bank, claim, and sync-back behavior
- picker UI recovery modal with concrete action buttons and cancel
- docs and smoke-test notes for recovery paths

This plan does not implement:

- backup browsing or manual backup restore UI
- live roster extraction
- save payload parsing
- in-game automated UI tests
- multi-step conflict resolution beyond one recovery action and retry

## File Structure

- Create: `src/Runtime/ActiveSaveRecovery.cs` - recovery action/model interfaces and storage-backed service.
- Modify: `src/Runtime/HostFlowController.cs` - inject recovery service and add recovery retry helpers.
- Modify: `src/Runtime/Sts2HostFlowRuntime.cs` - pass `ActiveSaveRecoveryService` into the host-flow controller.
- Create: `src/UI/MultiplayerSaveRecoveryModal.cs` - code-built recovery modal with action buttons.
- Modify: `src/UI/MultiplayerSavePickerModal.cs` - show recovery modal when a picker action fails and recovery actions exist.
- Modify: `tests/HostFlowControllerTests.cs` - pure controller tests for recovery models and retry behavior.
- Modify: `tests/ActiveSaveSwitcherTests.cs` - storage-backed recovery service tests.
- Modify: `README.md` - update Phase 4 status and smoke test.

## Task 1: Recovery Models And Controller Retry Flow

**Files:**
- Create: `src/Runtime/ActiveSaveRecovery.cs`
- Modify: `src/Runtime/HostFlowController.cs`
- Test: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing controller tests**

Add these test registrations to `HostFlowControllerTests.All()`:

```csharp
yield return new TestCase("controller exposes recovery model", ControllerExposesRecoveryModel);
yield return new TestCase("controller recovers then starts new run", ControllerRecoversThenStartsNewRun);
yield return new TestCase("controller recovers then selects existing campaign", ControllerRecoversThenSelectsExistingCampaign);
yield return new TestCase("controller stops when recovery fails", ControllerStopsWhenRecoveryFails);
```

Add these test methods:

```csharp
private static void ControllerExposesRecoveryModel()
{
    var recovery = new FakeActiveSaveRecovery();
    var controller = CreateController(new FakeHostFlowSaveBank(), recovery: recovery);

    var model = controller.BuildRecoveryModel(MultiplayerGameMode.Standard);

    AssertEx.Equal("Active multiplayer save needs attention", model.Title);
    AssertEx.Equal(1, model.Options.Count);
    AssertEx.Equal(ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign, model.Options[0].Kind);
    AssertEx.Equal(MultiplayerGameMode.Standard, recovery.LastBuildGameMode);
}

private static void ControllerRecoversThenStartsNewRun()
{
    var recovery = new FakeActiveSaveRecovery();
    var continuation = new FakeHostFlowContinuation();
    var preflight = new FakeActiveSavePreflight { Failure = "Current multiplayer save is not managed" };
    var controller = CreateController(
        new FakeHostFlowSaveBank(),
        continuation: continuation,
        preflight: preflight,
        recovery: recovery);

    var result = controller.RecoverAndSelectStartNewRun(
        ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign,
        MultiplayerGameMode.Custom);

    AssertEx.True(result.Success);
    AssertEx.Equal(ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign, recovery.LastRecoveredAction);
    AssertEx.Equal(MultiplayerGameMode.Custom, recovery.LastRecoveredGameMode);
    AssertEx.Equal(1, continuation.StartNewRunCount);
}

private static void ControllerRecoversThenSelectsExistingCampaign()
{
    var recovery = new FakeActiveSaveRecovery();
    var activator = new FakeActiveSaveActivator();
    var continuation = new FakeHostFlowContinuation();
    var preflight = new FakeActiveSavePreflight { Failure = "Active save has unsynced changes" };
    var controller = CreateController(
        new FakeHostFlowSaveBank(),
        activator,
        continuation,
        preflight: preflight,
        recovery: recovery);

    var result = controller.RecoverAndSelectExistingCampaign(
        ActiveSaveRecoveryActionKind.SyncActiveToCampaign,
        "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
        MultiplayerGameMode.Standard);

    AssertEx.True(result.Success);
    AssertEx.Equal(ActiveSaveRecoveryActionKind.SyncActiveToCampaign, recovery.LastRecoveredAction);
    AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", activator.ActivatedCampaignId);
    AssertEx.Equal(1, continuation.LoadExistingCount);
}

private static void ControllerStopsWhenRecoveryFails()
{
    var recovery = new FakeActiveSaveRecovery { Failure = "sync failed" };
    var continuation = new FakeHostFlowContinuation();
    var controller = CreateController(new FakeHostFlowSaveBank(), continuation: continuation, recovery: recovery);

    var result = controller.RecoverAndSelectStartNewRun(
        ActiveSaveRecoveryActionKind.SyncActiveToCampaign,
        MultiplayerGameMode.Daily);

    AssertEx.False(result.Success);
    AssertEx.Equal("sync failed", result.ErrorMessage);
    AssertEx.Equal(0, continuation.StartNewRunCount);
}
```

Update `CreateController(...)` to accept `FakeActiveSaveRecovery? recovery = null` and pass it to the `HostFlowController` constructor.

Add this fake:

```csharp
private sealed class FakeActiveSaveRecovery : IActiveSaveRecovery
{
    public string? Failure { get; init; }
    public MultiplayerGameMode? LastBuildGameMode { get; private set; }
    public ActiveSaveRecoveryActionKind? LastRecoveredAction { get; private set; }
    public MultiplayerGameMode? LastRecoveredGameMode { get; private set; }

    public ActiveSaveRecoveryModel BuildRecoveryModel(MultiplayerGameMode gameMode)
    {
        LastBuildGameMode = gameMode;
        return new ActiveSaveRecoveryModel(
            "Active multiplayer save needs attention",
            "Choose how to protect the current active multiplayer save before continuing.",
            [
                new ActiveSaveRecoveryOption(
                    ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign,
                    "Duplicate Active Save",
                    "Copy the current active save into the Multiplayer Save Slots bank.")
            ]);
    }

    public OperationResult Recover(ActiveSaveRecoveryActionKind action, MultiplayerGameMode gameMode, DateTimeOffset nowUtc)
    {
        if (Failure is not null)
            return OperationResult.Fail(Failure);

        LastRecoveredAction = action;
        LastRecoveredGameMode = gameMode;
        return OperationResult.Ok();
    }
}
```

- [ ] **Step 2: Run tests to verify failure**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile fails because recovery types and constructor overloads do not exist.

- [ ] **Step 3: Implement runtime recovery model and controller methods**

Create `src/Runtime/ActiveSaveRecovery.cs` with:

```csharp
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public enum ActiveSaveRecoveryActionKind
{
    SyncActiveToCampaign,
    DuplicateActiveIntoCampaign
}

public sealed record ActiveSaveRecoveryOption(
    ActiveSaveRecoveryActionKind Kind,
    string Label,
    string Description);

public sealed record ActiveSaveRecoveryModel(
    string Title,
    string Message,
    IReadOnlyList<ActiveSaveRecoveryOption> Options)
{
    public bool HasOptions => Options.Count > 0;

    public static ActiveSaveRecoveryModel None() =>
        new("Multiplayer Save Slots", "No recovery action is available.", []);
}

public interface IActiveSaveRecovery
{
    ActiveSaveRecoveryModel BuildRecoveryModel(MultiplayerGameMode gameMode);
    OperationResult Recover(ActiveSaveRecoveryActionKind action, MultiplayerGameMode gameMode, DateTimeOffset nowUtc);
}

public sealed class NoActiveSaveRecovery : IActiveSaveRecovery
{
    public ActiveSaveRecoveryModel BuildRecoveryModel(MultiplayerGameMode gameMode) => ActiveSaveRecoveryModel.None();

    public OperationResult Recover(ActiveSaveRecoveryActionKind action, MultiplayerGameMode gameMode, DateTimeOffset nowUtc) =>
        OperationResult.Fail("No recovery action is available.");
}
```

Modify `HostFlowController`:

```csharp
private readonly IActiveSaveRecovery _recovery;
```

Add the `IActiveSaveRecovery recovery` constructor parameter between continuation and session, assign `_recovery = recovery`, and add:

```csharp
public ActiveSaveRecoveryModel BuildRecoveryModel(MultiplayerGameMode gameMode) =>
    _recovery.BuildRecoveryModel(gameMode);

public OperationResult RecoverAndSelectStartNewRun(
    ActiveSaveRecoveryActionKind action,
    MultiplayerGameMode gameMode)
{
    var recovery = _recovery.Recover(action, gameMode, _clock.UtcNow);
    if (!recovery.Success)
        return recovery;

    return SelectStartNewRun(gameMode);
}

public OperationResult RecoverAndSelectExistingCampaign(
    ActiveSaveRecoveryActionKind action,
    string campaignId,
    MultiplayerGameMode gameMode)
{
    var recovery = _recovery.Recover(action, gameMode, _clock.UtcNow);
    if (!recovery.Success)
        return recovery;

    return SelectExistingCampaign(campaignId, gameMode);
}
```

- [ ] **Step 4: Run tests to verify pass**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 5: Commit**

```bash
git add src/Runtime/ActiveSaveRecovery.cs src/Runtime/HostFlowController.cs tests/HostFlowControllerTests.cs
git commit -m "feat: add active save recovery flow"
```

## Task 2: Storage-Backed Recovery Actions

**Files:**
- Modify: `src/Runtime/ActiveSaveRecovery.cs`
- Modify: `src/Runtime/Sts2HostFlowRuntime.cs`
- Test: `tests/ActiveSaveSwitcherTests.cs`

- [ ] **Step 1: Write failing storage-backed recovery tests**

Register these tests in `ActiveSaveSwitcherTests.Register(...)`:

```csharp
tests.Add(new TestCase("recovery offers duplicate for unmanaged active save", RecoveryOffersDuplicateForUnmanagedActiveSave));
tests.Add(new TestCase("recovery offers sync for unsynced managed active save", RecoveryOffersSyncForUnsyncedManagedActiveSave));
tests.Add(new TestCase("recovery duplicates active save into bank", RecoveryDuplicatesActiveSaveIntoBank));
tests.Add(new TestCase("recovery syncs active save to selected campaign", RecoverySyncsActiveSaveToSelectedCampaign));
```

Add tests that create a `MultiplayerSaveBank`, `ActiveSaveSwitcher`, and `ActiveSaveRecoveryService` with temp active/state paths. The duplicate test should assert a new campaign appears in `bank.ListCampaigns(MultiplayerGameMode.Standard)` and that `active-state.json` points at it. The sync test should activate a campaign, mutate the active save, recover with `SyncActiveToCampaign`, and assert the bank payload changed.

- [ ] **Step 2: Run tests to verify failure**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile fails because `ActiveSaveRecoveryService` does not exist.

- [ ] **Step 3: Implement storage-backed recovery service**

Append this class to `src/Runtime/ActiveSaveRecovery.cs`:

```csharp
using System.Text.Json;
using MultiplayerSaveSlots.Storage;
```

```csharp
public sealed class ActiveSaveRecoveryService : IActiveSaveRecovery
{
    private readonly MultiplayerSaveBank _bank;
    private readonly ActiveSaveSwitcher _switcher;
    private readonly string _activeSavePath;
    private readonly string _statePath;

    public ActiveSaveRecoveryService(
        MultiplayerSaveBank bank,
        ActiveSaveSwitcher switcher,
        string activeSavePath,
        string statePath)
    {
        _bank = bank;
        _switcher = switcher;
        _activeSavePath = activeSavePath;
        _statePath = statePath;
    }

    public ActiveSaveRecoveryModel BuildRecoveryModel(MultiplayerGameMode gameMode)
    {
        if (!File.Exists(_activeSavePath))
            return ActiveSaveRecoveryModel.None();

        if (!File.Exists(_statePath))
            return DuplicateModel("Current multiplayer save is not managed by Multiplayer Save Slots yet.");

        try
        {
            var state = JsonFile.Read<ActiveSaveState>(_statePath);
            var activeChecksum = FileChecksum.Sha256(_activeSavePath);
            if (activeChecksum == state.ActiveChecksumAfterActivation)
                return ActiveSaveRecoveryModel.None();

            return new ActiveSaveRecoveryModel(
                "Active multiplayer save has unsynced changes",
                "Sync the active save back to its selected campaign before switching slots.",
                [
                    new ActiveSaveRecoveryOption(
                        ActiveSaveRecoveryActionKind.SyncActiveToCampaign,
                        "Sync Active Save",
                        "Back up the campaign payload and copy the active save into the selected campaign.")
                ]);
        }
        catch (Exception ex) when (ex is IOException or JsonException or InvalidOperationException)
        {
            return DuplicateModel($"Active save state cannot be verified: {ex.Message}");
        }
    }

    public OperationResult Recover(ActiveSaveRecoveryActionKind action, MultiplayerGameMode gameMode, DateTimeOffset nowUtc)
    {
        try
        {
            if (action == ActiveSaveRecoveryActionKind.SyncActiveToCampaign)
            {
                _switcher.SyncBack(nowUtc);
                return OperationResult.Ok();
            }

            if (!File.Exists(_activeSavePath))
                return OperationResult.Fail("Active multiplayer save is missing");

            var metadata = _bank.CreateCampaign(new CampaignCreateRequest(gameMode, [], _activeSavePath, nowUtc));
            _switcher.ClaimActiveSave(metadata.CampaignId, nowUtc);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    private static ActiveSaveRecoveryModel DuplicateModel(string message) =>
        new(
            "Active multiplayer save needs attention",
            message,
            [
                new ActiveSaveRecoveryOption(
                    ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign,
                    "Duplicate Active Save",
                    "Copy the current active save into the Multiplayer Save Slots bank before continuing.")
            ]);
}
```

Update `Sts2HostFlowRuntime.CreateController(...)` so `HostFlowController` receives:

```csharp
new ActiveSaveRecoveryService(bank, switcher, paths.ActiveSavePath, paths.ActiveStatePath),
```

between `new Sts2HostFlowContinuation(hostSubmenu)` and `Session`.

- [ ] **Step 4: Run tests to verify pass**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 5: Commit**

```bash
git add src/Runtime/ActiveSaveRecovery.cs src/Runtime/Sts2HostFlowRuntime.cs tests/ActiveSaveSwitcherTests.cs
git commit -m "feat: add storage-backed save recovery"
```

## Task 3: Recovery Modal UI

**Files:**
- Create: `src/UI/MultiplayerSaveRecoveryModal.cs`
- Modify: `src/UI/MultiplayerSavePickerModal.cs`
- Modify: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing UI-adjacent controller test**

Add a test to `HostFlowControllerTests.All()`:

```csharp
yield return new TestCase("recovery model reports unavailable when no options exist", RecoveryModelReportsUnavailableWhenNoOptionsExist);
```

Add:

```csharp
private static void RecoveryModelReportsUnavailableWhenNoOptionsExist()
{
    var model = ActiveSaveRecoveryModel.None();

    AssertEx.False(model.HasOptions);
    AssertEx.Equal("No recovery action is available.", model.Message);
}
```

- [ ] **Step 2: Run tests to verify failure if model shape changed incorrectly**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: tests pass if Task 1 implemented the model correctly. If they fail, fix the model before UI wiring.

- [ ] **Step 3: Implement recovery modal and picker handoff**

Create `src/UI/MultiplayerSaveRecoveryModal.cs` with a `Control, IScreenContext` modal that receives `HostFlowController`, `MultiplayerGameMode`, `MultiplayerSavePickerRow`, and `ActiveSaveRecoveryModel`. It should build a panel with model title/message, one button per recovery option, and a cancel button. On option press, call either `RecoverAndSelectStartNewRun(...)` or `RecoverAndSelectExistingCampaign(...)`; show `NErrorPopup` if recovery or retry fails.

In `MultiplayerSavePickerModal.SelectRow(...)`, replace direct `ShowError(...)` with:

```csharp
var recovery = _controller.BuildRecoveryModel(_model.GameMode);
if (recovery.HasOptions)
{
    MultiplayerSaveRecoveryModal.Show(_controller, _model.GameMode, row, recovery);
    return;
}

ShowError(result.ErrorMessage ?? "Unable to continue multiplayer host flow.");
```

- [ ] **Step 4: Run tests to verify pass**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 5: Commit**

```bash
git add src/UI/MultiplayerSaveRecoveryModal.cs src/UI/MultiplayerSavePickerModal.cs tests/HostFlowControllerTests.cs
git commit -m "feat: show active save recovery choices"
```

## Task 4: Documentation And Verification

**Files:**
- Modify: `README.md`
- Modify: `docs/implementation/hook-discovery.md`

- [ ] **Step 1: Update docs**

Change `README.md` status to say Phase 4 adds recovery choices for unmanaged or unsynced active saves. Add smoke-test steps for preserving an existing vanilla multiplayer save through the duplicate action and syncing unsynced changes through the sync action.

Append a short Phase 4 note to `docs/implementation/hook-discovery.md`:

```markdown
## Phase 4 Safety Recovery

Phase 4 does not add new STS2 hooks. It reuses the host picker flow and save-sync services to offer conservative recovery actions before the mod replaces `current_run_mp.save`.
```

- [ ] **Step 2: Run full verification**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: build succeeds and all tests pass.

- [ ] **Step 3: Commit**

```bash
git add README.md docs/implementation/hook-discovery.md
git commit -m "docs: document active save recovery"
```

## Self-Review

- Spec coverage: implements the approved safety/recovery slice using duplicate-active and sync-active actions. Backup browsing and restore UI remain intentionally outside this phase.
- Placeholder scan: no unfinished markers remain.
- Type consistency: `ActiveSaveRecoveryActionKind`, `ActiveSaveRecoveryModel`, `IActiveSaveRecovery`, and `ActiveSaveRecoveryService` are introduced before later tasks consume them.
