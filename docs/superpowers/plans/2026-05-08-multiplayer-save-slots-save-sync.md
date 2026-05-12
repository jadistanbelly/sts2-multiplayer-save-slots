# Multiplayer Save Slots Save Sync Implementation Plan

> **Archived historical plan:** This document is preserved for context only. It is not an active implementation plan; unchecked checklist items below do not indicate pending work. See `docs/superpowers/README.md` for archive policy.

> **Archived worker note:** This was the original execution guidance. Do not execute this document as current work unless a new issue or branch explicitly reactivates it.

**Goal:** Sync vanilla multiplayer save writes back into the active Multiplayer Save Slots campaign and finalize pending new runs into the save bank.

**Architecture:** Phase 3 adds a save-sync runtime layer around `SaveManager.SaveRun(...)` instead of mutating STS2 save internals. A Harmony postfix wraps the returned save `Task`, waits for vanilla persistence to finish, then asks a pure controller to either sync the selected campaign or claim the newly written active save as a new bank campaign. Existing checksum and backup protections remain in `ActiveSaveSwitcher`.

**Tech Stack:** C# `net9.0`, Harmony, STS2 `SaveManager`, existing storage services, `System.Text.Json`, Godot/STS2 logging, and the no-NuGet console test harness.

---

## Scope Check

This plan implements:

- `SaveManager.SaveRun(AbstractRoom?, bool)` task wrapping so sync happens after vanilla writes finish
- sync-back for an existing selected campaign from `HostFlowSession`
- pending new-run finalization into `MultiplayerSaveBank`
- active-state claiming for the already-written vanilla multiplayer save
- controller and patch tests
- README and hook-discovery updates

This plan does not implement:

- roster extraction from live multiplayer lobbies
- progress parsing from save payloads
- recovery choice UI for sync failures
- cloud conflict resolution beyond the existing checksum guards
- clearing session state on run end or lobby cleanup

## File Structure

- Create: `src/Runtime/SaveSyncController.cs` - pure save-sync decision logic for selected campaigns and pending new runs.
- Create: `src/Patches/SaveManagerPatch.cs` - Harmony postfix helper for `SaveManager.SaveRun(...)`.
- Modify: `src/Runtime/HostFlowContinuations.cs` - add small sync interfaces used by the controller.
- Modify: `src/Runtime/Sts2HostFlowRuntime.cs` - construct the save-sync controller and storage adapter.
- Modify: `src/Storage/ActiveSaveSwitcher.cs` - add active-save claim support for pending new runs.
- Modify: `tests/HostFlowControllerTests.cs` - add save-sync controller tests and fakes.
- Modify: `tests/HostFlowPatchTests.cs` - add task-wrapper tests for the save patch.
- Modify: `README.md` - document Phase 3 status and smoke test expectations.
- Modify: `docs/implementation/hook-discovery.md` - record the confirmed save-sync hook.

## Confirmed STS2 Hook Facts

- `RunSaveManager.SaveRun(AbstractRoom? preFinishedRoom)` serializes the current run and writes `current_run_mp.save` when `RunManager.Instance.NetService.Type.IsMultiplayer()` is true.
- `RunSaveManager.SaveRun(...)` raises its `Saved` event after the write is complete.
- `SaveManager.SaveRun(AbstractRoom? preFinishedRoom, bool saveProgress = true)` awaits `_runSaveManager.SaveRun(...)` and catches/logs save exceptions.
- A Harmony postfix on `SaveManager.SaveRun(...)` can replace `ref Task __result` with an async continuation so mod sync runs after vanilla save completion.

## Task 1: Save Sync Controller

**Files:**
- Create: `src/Runtime/SaveSyncController.cs`
- Modify: `src/Runtime/HostFlowContinuations.cs`
- Test: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing save-sync controller tests**

Append these cases to `HostFlowControllerTests.All()`:

```csharp
yield return new TestCase("save sync no-ops without selected campaign", SaveSyncNoOpsWithoutSelection);
yield return new TestCase("save sync syncs existing selected campaign", SaveSyncSyncsExistingSelection);
yield return new TestCase("save sync finalizes pending new run", SaveSyncFinalizesPendingNewRun);
yield return new TestCase("save sync keeps pending new run selected when finalization fails", SaveSyncKeepsPendingNewRunWhenFinalizationFails);
yield return new TestCase("save sync maps sync exceptions to failed result", SaveSyncMapsExceptions);
```

Add these tests:

```csharp
private static void SaveSyncNoOpsWithoutSelection()
{
    var sync = new FakeActiveSaveSync();
    var session = new HostFlowSession();
    var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

    var result = controller.SyncAfterVanillaSave();

    AssertEx.True(result.Success);
    AssertEx.Equal(0, sync.SyncBackCount);
    AssertEx.Equal(0, sync.FinalizeCount);
}

private static void SaveSyncSyncsExistingSelection()
{
    var sync = new FakeActiveSaveSync();
    var session = new HostFlowSession();
    session.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);
    var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

    var result = controller.SyncAfterVanillaSave();

    AssertEx.True(result.Success);
    AssertEx.Equal(1, sync.SyncBackCount);
    AssertEx.Equal(0, sync.FinalizeCount);
    AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", session.SelectedCampaignId);
    AssertEx.False(session.IsPendingNewRun);
}

private static void SaveSyncFinalizesPendingNewRun()
{
    var sync = new FakeActiveSaveSync { FinalizedCampaignId = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb" };
    var session = new HostFlowSession();
    session.SelectNewRun(MultiplayerGameMode.Custom);
    var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

    var result = controller.SyncAfterVanillaSave();

    AssertEx.True(result.Success);
    AssertEx.Equal(0, sync.SyncBackCount);
    AssertEx.Equal(1, sync.FinalizeCount);
    AssertEx.Equal(MultiplayerGameMode.Custom, sync.FinalizedGameMode);
    AssertEx.Equal("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", session.SelectedCampaignId);
    AssertEx.Equal(MultiplayerGameMode.Custom, session.SelectedGameMode);
    AssertEx.False(session.IsPendingNewRun);
}

private static void SaveSyncKeepsPendingNewRunWhenFinalizationFails()
{
    var sync = new FakeActiveSaveSync { FinalizeFailure = "active save is missing" };
    var session = new HostFlowSession();
    session.SelectNewRun(MultiplayerGameMode.Daily);
    var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

    var result = controller.SyncAfterVanillaSave();

    AssertEx.False(result.Success);
    AssertEx.Equal("active save is missing", result.ErrorMessage);
    AssertEx.Equal(null, session.SelectedCampaignId);
    AssertEx.Equal(MultiplayerGameMode.Daily, session.SelectedGameMode);
    AssertEx.True(session.IsPendingNewRun);
}

private static void SaveSyncMapsExceptions()
{
    var sync = new ThrowingActiveSaveSync();
    var session = new HostFlowSession();
    session.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);
    var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

    var result = controller.SyncAfterVanillaSave();

    AssertEx.False(result.Success);
    AssertEx.Equal("sync exploded", result.ErrorMessage);
}
```

Add these fakes in `HostFlowControllerTests.cs`:

```csharp
private sealed class FakeActiveSaveSync : IActiveSaveSync
{
    public int SyncBackCount { get; private set; }
    public int FinalizeCount { get; private set; }
    public string FinalizedCampaignId { get; init; } = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
    public string? FinalizeFailure { get; init; }
    public MultiplayerGameMode? FinalizedGameMode { get; private set; }

    public OperationResult SyncBack(DateTimeOffset nowUtc)
    {
        SyncBackCount++;
        return OperationResult.Ok();
    }

    public OperationResult<string> FinalizePendingNewRun(MultiplayerGameMode gameMode, DateTimeOffset nowUtc)
    {
        FinalizeCount++;
        FinalizedGameMode = gameMode;
        return FinalizeFailure is null
            ? OperationResult<string>.Ok(FinalizedCampaignId)
            : OperationResult<string>.Fail(FinalizeFailure);
    }
}

private sealed class ThrowingActiveSaveSync : IActiveSaveSync
{
    public OperationResult SyncBack(DateTimeOffset nowUtc) => throw new InvalidOperationException("sync exploded");

    public OperationResult<string> FinalizePendingNewRun(MultiplayerGameMode gameMode, DateTimeOffset nowUtc) =>
        throw new InvalidOperationException("finalize exploded");
}
```

- [ ] **Step 2: Run test to verify it fails**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile fails because `SaveSyncController` and `IActiveSaveSync` do not exist.

- [ ] **Step 3: Implement sync interfaces and controller**

Append to `src/Runtime/HostFlowContinuations.cs`:

```csharp
public interface IActiveSaveSync
{
    OperationResult SyncBack(DateTimeOffset nowUtc);
    OperationResult<string> FinalizePendingNewRun(MultiplayerGameMode gameMode, DateTimeOffset nowUtc);
}
```

Create `src/Runtime/SaveSyncController.cs`:

```csharp
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public sealed class SaveSyncController
{
    private readonly IActiveSaveSync _activeSaveSync;
    private readonly HostFlowSession _session;
    private readonly IClock _clock;

    public SaveSyncController(IActiveSaveSync activeSaveSync, HostFlowSession session, IClock clock)
    {
        _activeSaveSync = activeSaveSync;
        _session = session;
        _clock = clock;
    }

    public OperationResult SyncAfterVanillaSave()
    {
        try
        {
            if (_session.SelectedGameMode is null)
                return OperationResult.Ok();

            if (_session.IsPendingNewRun)
                return FinalizePendingNewRun(_session.SelectedGameMode.Value);

            if (_session.SelectedCampaignId is null)
                return OperationResult.Ok();

            return _activeSaveSync.SyncBack(_clock.UtcNow);
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    private OperationResult FinalizePendingNewRun(MultiplayerGameMode gameMode)
    {
        var finalized = _activeSaveSync.FinalizePendingNewRun(gameMode, _clock.UtcNow);
        if (!finalized.Success || finalized.Value is null)
            return OperationResult.Fail(finalized.ErrorMessage ?? "Unable to finalize pending multiplayer save.");

        _session.SelectExistingCampaign(finalized.Value, gameMode);
        return OperationResult.Ok();
    }
}
```

- [ ] **Step 4: Run test to verify it passes**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 5: Commit**

```bash
git add src/Runtime/HostFlowContinuations.cs src/Runtime/SaveSyncController.cs tests/HostFlowControllerTests.cs
git commit -m "feat: add save sync controller"
```

## Task 2: Active Save Claim And STS2 Sync Adapter

**Files:**
- Modify: `src/Storage/ActiveSaveSwitcher.cs`
- Modify: `src/Runtime/Sts2HostFlowRuntime.cs`
- Test: `tests/ActiveSaveSwitcherTests.cs`

- [ ] **Step 1: Write failing active claim tests**

Register these tests in `ActiveSaveSwitcherTests.Register(...)`:

```csharp
tests.Add(new TestCase("switcher claims active save for pending new campaign", ClaimsActiveSaveForPendingNewCampaign));
tests.Add(new TestCase("switcher claim rejects missing active save", ClaimRejectsMissingActiveSave));
tests.Add(new TestCase("switcher claim rejects mismatched campaign payload", ClaimRejectsMismatchedCampaignPayload));
```

Add these test methods:

```csharp
private static void ClaimsActiveSaveForPendingNewCampaign()
{
    using var temp = new TempDirectory();
    var active = Path.Combine(temp.Path, "active.save");
    var state = Path.Combine(temp.Path, "active-state.json");
    File.WriteAllText(active, "new-campaign");

    var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
    var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], active, DateTimeOffset.UtcNow));
    var switcher = new ActiveSaveSwitcher(bank, active, state);
    var now = new DateTimeOffset(2026, 5, 8, 16, 0, 0, TimeSpan.Zero);

    switcher.ClaimActiveSave(metadata.CampaignId, now);

    var activeState = JsonFile.Read<ActiveSaveState>(state);
    AssertEx.Equal(metadata.CampaignId, activeState.CampaignId);
    AssertEx.Equal(null, activeState.ActiveChecksumBeforeActivation);
    AssertEx.Equal(FileChecksum.Sha256(active), activeState.ActiveChecksumAfterActivation);

    var updated = bank.GetCampaign(metadata.CampaignId);
    AssertEx.Equal(FileChecksum.Sha256(active), updated.ActiveChecksum);
    AssertEx.Equal(FileChecksum.Sha256(bank.GetPayloadPath(metadata.CampaignId)), updated.PayloadChecksum);
    AssertEx.Equal(now, updated.LastPlayedAtUtc);
}

private static void ClaimRejectsMissingActiveSave()
{
    using var temp = new TempDirectory();
    var source = Path.Combine(temp.Path, "source.save");
    var active = Path.Combine(temp.Path, "active.save");
    File.WriteAllText(source, "new-campaign");

    var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
    var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
    var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

    AssertEx.Throws<FileNotFoundException>(() => switcher.ClaimActiveSave(metadata.CampaignId, DateTimeOffset.UtcNow));
}

private static void ClaimRejectsMismatchedCampaignPayload()
{
    using var temp = new TempDirectory();
    var source = Path.Combine(temp.Path, "source.save");
    var active = Path.Combine(temp.Path, "active.save");
    File.WriteAllText(source, "bank-payload");
    File.WriteAllText(active, "active-payload");

    var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
    var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
    var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

    AssertEx.Throws<InvalidOperationException>(() => switcher.ClaimActiveSave(metadata.CampaignId, DateTimeOffset.UtcNow));
}
```

- [ ] **Step 2: Run test to verify it fails**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile fails because `ClaimActiveSave` does not exist.

- [ ] **Step 3: Implement active claim and runtime adapter**

Add this method to `src/Storage/ActiveSaveSwitcher.cs`:

```csharp
public void ClaimActiveSave(string campaignId, DateTimeOffset nowUtc)
{
    if (!File.Exists(_activeSavePath))
        throw new FileNotFoundException("Active multiplayer save is missing", _activeSavePath);

    var payloadPath = _bank.GetPayloadPath(campaignId);
    if (!File.Exists(payloadPath))
        throw new FileNotFoundException("Campaign payload is missing", payloadPath);

    var metadata = _bank.GetCampaign(campaignId);
    _bank.EnsureCampaignIndexed(campaignId);

    var activeChecksum = FileChecksum.Sha256(_activeSavePath);
    var payloadChecksum = FileChecksum.Sha256(payloadPath);
    if (activeChecksum != payloadChecksum)
        throw new InvalidOperationException("Campaign payload does not match active save");

    string? previousStateBackupPath = null;
    if (File.Exists(_statePath))
        previousStateBackupPath = BackupManager.CreateBackup(_statePath, _bank.GetBackupDirectory(campaignId), "before-claim-state", nowUtc);

    JsonFile.Write(_statePath, new ActiveSaveState(
        campaignId,
        ActiveChecksumBeforeActivation: null,
        activeChecksum,
        nowUtc,
        PreviousActiveBackupPath: null,
        previousStateBackupPath));

    _bank.UpdateMetadata(metadata with
    {
        ActiveChecksum = activeChecksum,
        PayloadChecksum = payloadChecksum,
        LastPlayedAtUtc = nowUtc
    });
}
```

Add `Sts2ActiveSaveSync` to `src/Runtime/Sts2HostFlowRuntime.cs`:

```csharp
public sealed class Sts2ActiveSaveSync : IActiveSaveSync
{
    private readonly MultiplayerSaveBank _bank;
    private readonly ActiveSaveSwitcher _switcher;
    private readonly string _activeSavePath;

    public Sts2ActiveSaveSync(MultiplayerSaveBank bank, ActiveSaveSwitcher switcher, string activeSavePath)
    {
        _bank = bank;
        _switcher = switcher;
        _activeSavePath = activeSavePath;
    }

    public OperationResult SyncBack(DateTimeOffset nowUtc)
    {
        try
        {
            _switcher.SyncBack(nowUtc);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    public OperationResult<string> FinalizePendingNewRun(MultiplayerGameMode gameMode, DateTimeOffset nowUtc)
    {
        try
        {
            if (!File.Exists(_activeSavePath))
                return OperationResult<string>.Fail("Active multiplayer save is missing");

            var metadata = _bank.CreateCampaign(new CampaignCreateRequest(gameMode, [], _activeSavePath, nowUtc));
            _switcher.ClaimActiveSave(metadata.CampaignId, nowUtc);
            return OperationResult<string>.Ok(metadata.CampaignId);
        }
        catch (Exception ex)
        {
            return OperationResult<string>.Fail(ex.Message);
        }
    }
}
```

Add this factory to `Sts2HostFlowRuntime`:

```csharp
public static SaveSyncController CreateSaveSyncController()
{
    var paths = CreatePaths();
    var bank = new MultiplayerSaveBank(new SaveBankPaths(paths.BankRootDirectory));
    var switcher = new ActiveSaveSwitcher(bank, paths.ActiveSavePath, paths.ActiveStatePath);
    return new SaveSyncController(
        new Sts2ActiveSaveSync(bank, switcher, paths.ActiveSavePath),
        Session,
        new SystemClock());
}
```

- [ ] **Step 4: Run test to verify it passes**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 5: Commit**

```bash
git add src/Storage/ActiveSaveSwitcher.cs src/Runtime/Sts2HostFlowRuntime.cs tests/ActiveSaveSwitcherTests.cs
git commit -m "feat: claim pending multiplayer saves"
```

## Task 3: SaveManager Save Task Patch

**Files:**
- Create: `src/Patches/SaveManagerPatch.cs`
- Modify: `tests/HostFlowPatchTests.cs`

- [ ] **Step 1: Write failing patch tests**

Add these cases to `HostFlowPatchTests.All()`:

```csharp
yield return new TestCase("save manager patch runs sync after vanilla task completes", SaveManagerPatchRunsSyncAfterVanillaTask);
yield return new TestCase("save manager patch logs sync failure without failing vanilla save", SaveManagerPatchLogsSyncFailure);
yield return new TestCase("save manager patch propagates vanilla task failure", SaveManagerPatchPropagatesVanillaFailure);
```

Add these tests:

```csharp
private static void SaveManagerPatchRunsSyncAfterVanillaTask()
{
    var vanilla = Task.CompletedTask;
    var syncCount = 0;
    var logs = new List<string>();

    var wrapped = SaveManagerPatch.AppendSync(
        vanilla,
        () => new FakeSaveSyncController(() =>
        {
            syncCount++;
            return OperationResult.Ok();
        }),
        logs.Add);

    wrapped.GetAwaiter().GetResult();

    AssertEx.Equal(1, syncCount);
    AssertEx.Equal(0, logs.Count);
}

private static void SaveManagerPatchLogsSyncFailure()
{
    var logs = new List<string>();

    var wrapped = SaveManagerPatch.AppendSync(
        Task.CompletedTask,
        () => new FakeSaveSyncController(() => OperationResult.Fail("sync failed")),
        logs.Add);

    wrapped.GetAwaiter().GetResult();

    AssertEx.Equal(1, logs.Count);
    AssertEx.Equal("[MultiplayerSaveSlots] Save sync failed: sync failed", logs[0]);
}

private static void SaveManagerPatchPropagatesVanillaFailure()
{
    var logs = new List<string>();
    var vanilla = Task.FromException(new InvalidOperationException("vanilla failed"));

    var wrapped = SaveManagerPatch.AppendSync(
        vanilla,
        () => new FakeSaveSyncController(() => OperationResult.Ok()),
        logs.Add);

    AssertEx.Throws<InvalidOperationException>(() => wrapped.GetAwaiter().GetResult());
    AssertEx.Equal(0, logs.Count);
}
```

Add this fake:

```csharp
private sealed class FakeSaveSyncController : ISaveSyncRunner
{
    private readonly Func<OperationResult> _sync;

    public FakeSaveSyncController(Func<OperationResult> sync)
    {
        _sync = sync;
    }

    public OperationResult SyncAfterVanillaSave() => _sync();
}
```

- [ ] **Step 2: Run test to verify it fails**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile fails because `SaveManagerPatch` and `ISaveSyncRunner` do not exist.

- [ ] **Step 3: Implement patch**

Add this interface to `src/Runtime/SaveSyncController.cs` and implement it on the controller:

```csharp
public interface ISaveSyncRunner
{
    OperationResult SyncAfterVanillaSave();
}
```

Change the class declaration:

```csharp
public sealed class SaveSyncController : ISaveSyncRunner
```

Create `src/Patches/SaveManagerPatch.cs`:

```csharp
using HarmonyLib;
using MegaCrit.Sts2.Core.Saves;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.Patches;

[HarmonyPatch(typeof(SaveManager), nameof(SaveManager.SaveRun))]
public static class SaveManagerPatch
{
    [HarmonyPostfix]
    public static void Postfix(ref Task __result)
    {
        __result = AppendSync(
            __result,
            Sts2HostFlowRuntime.CreateSaveSyncController,
            message => Console.Error.WriteLine(message));
    }

    public static async Task AppendSync(
        Task vanillaSaveTask,
        Func<ISaveSyncRunner> createController,
        Action<string> logError)
    {
        await vanillaSaveTask.ConfigureAwait(false);

        var result = createController().SyncAfterVanillaSave();
        if (!result.Success)
            logError($"[MultiplayerSaveSlots] Save sync failed: {result.ErrorMessage}");
    }
}
```

- [ ] **Step 4: Run test to verify it passes**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 5: Commit**

```bash
git add src/Runtime/SaveSyncController.cs src/Patches/SaveManagerPatch.cs tests/HostFlowPatchTests.cs
git commit -m "feat: sync multiplayer saves after vanilla writes"
```

## Task 4: Documentation And Verification

**Files:**
- Modify: `README.md`
- Modify: `docs/implementation/hook-discovery.md`

- [ ] **Step 1: Update README status**

Replace the status paragraph in `README.md` with:

```markdown
Private development. Phase 3 adds save sync after vanilla multiplayer save writes.

The picker can start a new vanilla run or activate an existing campaign payload from the save bank before resuming the vanilla host flow. After vanilla writes `current_run_mp.save`, the mod syncs existing selected campaigns back to the bank and finalizes newly started runs as separate bank campaigns. Roster labels for newly finalized runs are still placeholder metadata until live lobby roster extraction is added.
```

Add to the smoke test:

```markdown
10. Progress far enough for STS2 to write the multiplayer run save.
11. Confirm `MultiplayerSaveSlots/index.json` gains a new campaign id.
12. Re-open `Host -> Standard` and confirm the new campaign appears in the picker.
```

- [ ] **Step 2: Update hook discovery**

Append to `docs/implementation/hook-discovery.md`:

```markdown
## Phase 3 Hook Selection

Phase 3 uses a postfix on `SaveManager.SaveRun(AbstractRoom?, bool)` and wraps the returned `Task`. This keeps vanilla error handling intact and runs Multiplayer Save Slots sync only after STS2 has completed its save batch.

The lower-level `RunSaveManager.SaveRun(AbstractRoom?)` is the method that writes `current_run_mp.save` and raises `Saved`, but patching the facade gives the mod one point after progress save batching and current-save task serialization.
```

- [ ] **Step 3: Run full verification**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: build succeeds and all tests pass.

- [ ] **Step 4: Commit**

```bash
git add README.md docs/implementation/hook-discovery.md
git commit -m "docs: document save sync phase"
```

## Self-Review

- Spec coverage: this plan covers the save-sync patch, existing-campaign sync-back, pending new-run bank finalization, backups/checksums through `ActiveSaveSwitcher`, and manual smoke testing. Roster extraction and recovery UI remain intentional follow-up work from the design.
- Placeholder scan: no unfinished markers or undefined implementation steps remain.
- Type consistency: `SaveSyncController`, `IActiveSaveSync`, `ISaveSyncRunner`, `ClaimActiveSave`, and `CreateSaveSyncController` are introduced before use in later tasks.
