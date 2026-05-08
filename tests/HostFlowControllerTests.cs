using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;
using MultiplayerSaveSlots.UI;

namespace MultiplayerSaveSlots.Tests;

public static class HostFlowControllerTests
{
    public static IEnumerable<TestCase> All()
    {
        yield return new TestCase("runtime paths place bank beside active multiplayer save", RuntimePathsPlaceBankBesideActiveSave);
        yield return new TestCase("host flow session tracks existing campaign selection", SessionTracksExistingCampaignSelection);
        yield return new TestCase("host flow session tracks pending new run", SessionTracksPendingNewRun);
        yield return new TestCase("host flow session clears selected and pending state", SessionClearsSelectedAndPendingState);
        yield return new TestCase("controller builds picker model with start new and campaign rows", ControllerBuildsPickerModel);
        yield return new TestCase("controller starts new run through continuation", ControllerStartsNewRunThroughContinuation);
        yield return new TestCase("controller does not start new run when active preflight fails", ControllerStopsStartNewWhenPreflightFails);
        yield return new TestCase("controller does not select session when start new continuation fails", ControllerStopsSessionSelectionWhenStartNewFails);
        yield return new TestCase("controller activates existing campaign before load continuation", ControllerActivatesExistingCampaign);
        yield return new TestCase("controller does not activate existing campaign when active preflight fails", ControllerStopsExistingCampaignWhenActivePreflightFails);
        yield return new TestCase("controller does not activate existing campaign when load preflight fails", ControllerStopsExistingCampaignWhenLoadPreflightFails);
        yield return new TestCase("controller does not continue when activation fails", ControllerStopsWhenActivationFails);
        yield return new TestCase("controller restores previous active save when existing load continuation fails", ControllerRestoresPreviousActiveWhenLoadFails);
        yield return new TestCase("controller reports rollback failure after existing load continuation fails", ControllerReportsRollbackFailureWhenLoadFails);
        yield return new TestCase("controller does not select session when existing load continuation fails", ControllerStopsSessionSelectionWhenLoadFails);
        yield return new TestCase("active save activator maps exceptions to failed result", ActiveSaveActivatorMapsExceptions);
        yield return new TestCase("save sync no-ops without selected campaign", SaveSyncNoOpsWithoutSelection);
        yield return new TestCase("save sync syncs existing selected campaign", SaveSyncSyncsExistingSelection);
        yield return new TestCase("save sync finalizes pending new run", SaveSyncFinalizesPendingNewRun);
        yield return new TestCase("save sync keeps pending new run selected when finalization fails", SaveSyncKeepsPendingNewRunWhenFinalizationFails);
        yield return new TestCase("save sync maps sync exceptions to failed result", SaveSyncMapsExceptions);
    }

    private static void RuntimePathsPlaceBankBesideActiveSave()
    {
        var saveDirectory = Path.Combine(Path.GetTempPath(), "sts2", "profile1", "saves");
        var activeSavePath = Path.Combine(saveDirectory, "current_run_mp.save");

        var paths = MultiplayerSaveRuntimePaths.FromActiveSavePath(activeSavePath);

        AssertEx.Equal(activeSavePath, paths.ActiveSavePath);
        AssertEx.Equal(Path.Combine(saveDirectory, "MultiplayerSaveSlots"), paths.BankRootDirectory);
        AssertEx.Equal(Path.Combine(saveDirectory, "MultiplayerSaveSlots", "active-state.json"), paths.ActiveStatePath);
    }

    private static void SessionTracksExistingCampaignSelection()
    {
        var session = new HostFlowSession();

        session.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", session.SelectedCampaignId);
        AssertEx.Equal(MultiplayerGameMode.Standard, session.SelectedGameMode);
        AssertEx.False(session.IsPendingNewRun);
    }

    private static void SessionTracksPendingNewRun()
    {
        var session = new HostFlowSession();

        session.SelectNewRun(MultiplayerGameMode.Custom);

        AssertEx.Equal(null, session.SelectedCampaignId);
        AssertEx.Equal(MultiplayerGameMode.Custom, session.SelectedGameMode);
        AssertEx.True(session.IsPendingNewRun);
    }

    private static void SessionClearsSelectedAndPendingState()
    {
        var session = new HostFlowSession();

        session.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);
        session.Clear();

        AssertEx.Equal(null, session.SelectedCampaignId);
        AssertEx.Equal(null, session.SelectedGameMode);
        AssertEx.False(session.IsPendingNewRun);

        session.SelectNewRun(MultiplayerGameMode.Custom);
        session.Clear();

        AssertEx.Equal(null, session.SelectedCampaignId);
        AssertEx.Equal(null, session.SelectedGameMode);
        AssertEx.False(session.IsPendingNewRun);
    }

    private static void ControllerBuildsPickerModel()
    {
        var bank = new FakeHostFlowSaveBank
        {
            Campaigns =
            [
                new CampaignMetadata(
                    "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                    MultiplayerGameMode.Standard,
                    "buddy1 + buddy2",
                    [new PlayerIdentity("1", "buddy1"), new PlayerIdentity("2", "buddy2")],
                    DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
                    DateTimeOffset.Parse("2026-05-08T01:00:00Z"),
                    null,
                    "checksum",
                    "Floor 7")
            ]
        };
        var controller = CreateController(bank);

        var model = controller.BuildPickerModel(MultiplayerGameMode.Standard);

        AssertEx.Equal(MultiplayerGameMode.Standard, model.GameMode);
        AssertEx.Equal(2, model.Rows.Count);
        AssertEx.Equal(PickerRowKind.StartNewRun, model.Rows[0].Kind);
        AssertEx.Equal("Start New Run", model.Rows[0].Title);
        AssertEx.Equal(PickerRowKind.Campaign, model.Rows[1].Kind);
        AssertEx.Equal("buddy1 + buddy2", model.Rows[1].Title);
        AssertEx.Equal("Floor 7 - 2 players", model.Rows[1].Subtitle);
    }

    private static void ControllerStartsNewRunThroughContinuation()
    {
        var continuation = new FakeHostFlowContinuation();
        var session = new HostFlowSession();
        var controller = CreateController(new FakeHostFlowSaveBank(), continuation: continuation, session: session);

        var result = controller.SelectStartNewRun(MultiplayerGameMode.Daily);

        AssertEx.True(result.Success);
        AssertEx.Equal(MultiplayerGameMode.Daily, session.SelectedGameMode);
        AssertEx.True(session.IsPendingNewRun);
        AssertEx.Equal(1, continuation.StartNewRunCount);
        AssertEx.Equal(0, continuation.LoadExistingCount);
    }

    private static void ControllerActivatesExistingCampaign()
    {
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation();
        var session = new HostFlowSession();
        var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation, session);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.True(result.Success);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", activator.ActivatedCampaignId);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", session.SelectedCampaignId);
        AssertEx.Equal(0, continuation.StartNewRunCount);
        AssertEx.Equal(1, continuation.LoadExistingCount);
    }

    private static void ControllerStopsStartNewWhenPreflightFails()
    {
        var preflight = new FakeActiveSavePreflight { Failure = "Active save has unsynced changes" };
        var continuation = new FakeHostFlowContinuation();
        var session = new HostFlowSession();
        var controller = CreateController(
            new FakeHostFlowSaveBank(),
            continuation: continuation,
            session: session,
            preflight: preflight);

        var result = controller.SelectStartNewRun(MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("Active save has unsynced changes", result.ErrorMessage);
        AssertEx.Equal(0, continuation.StartNewRunCount);
        AssertEx.Equal(null, session.SelectedGameMode);
    }

    private static void ControllerStopsSessionSelectionWhenStartNewFails()
    {
        var continuation = new FakeHostFlowContinuation { StartNewRunFailure = "start failed" };
        var session = new HostFlowSession();
        var controller = CreateController(new FakeHostFlowSaveBank(), continuation: continuation, session: session);

        var result = controller.SelectStartNewRun(MultiplayerGameMode.Daily);

        AssertEx.False(result.Success);
        AssertEx.Equal("start failed", result.ErrorMessage);
        AssertEx.Equal(null, session.SelectedCampaignId);
        AssertEx.Equal(null, session.SelectedGameMode);
        AssertEx.False(session.IsPendingNewRun);
    }

    private static void ControllerStopsWhenActivationFails()
    {
        var activator = new FakeActiveSaveActivator { Failure = "Active save has unsynced changes" };
        var continuation = new FakeHostFlowContinuation();
        var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("Active save has unsynced changes", result.ErrorMessage);
        AssertEx.Equal(0, continuation.StartNewRunCount);
        AssertEx.Equal(0, continuation.LoadExistingCount);
    }

    private static void ControllerStopsExistingCampaignWhenLoadPreflightFails()
    {
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation { PrepareLoadExistingFailure = "menu stack unavailable" };
        var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("menu stack unavailable", result.ErrorMessage);
        AssertEx.Equal(null, activator.ActivatedCampaignId);
        AssertEx.Equal(0, continuation.LoadExistingCount);
    }

    private static void ControllerStopsExistingCampaignWhenActivePreflightFails()
    {
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation();
        var preflight = new FakeActiveSavePreflight { Failure = "Current multiplayer save is not managed" };
        var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation, preflight: preflight);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("Current multiplayer save is not managed", result.ErrorMessage);
        AssertEx.Equal(null, activator.ActivatedCampaignId);
        AssertEx.Equal(0, continuation.LoadExistingCount);
    }

    private static void ActiveSaveActivatorMapsExceptions()
    {
        var activator = new DelegateActiveSaveActivator((_, _) => throw new InvalidOperationException("bad active save"));

        var result = activator.Activate("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", DateTimeOffset.Parse("2026-05-08T12:00:00Z"));

        AssertEx.False(result.Success);
        AssertEx.Equal("bad active save", result.ErrorMessage);
    }

    private static void ControllerRestoresPreviousActiveWhenLoadFails()
    {
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation { LoadExistingFailure = "load failed" };
        var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("load failed", result.ErrorMessage);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", activator.ActivatedCampaignId);
        AssertEx.Equal(1, activator.RestoreCount);
    }

    private static void ControllerReportsRollbackFailureWhenLoadFails()
    {
        var activator = new FakeActiveSaveActivator { RestoreFailure = "restore failed" };
        var continuation = new FakeHostFlowContinuation { LoadExistingFailure = "load failed" };
        var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("load failed; rollback failed: restore failed", result.ErrorMessage);
        AssertEx.Equal(1, activator.RestoreCount);
    }

    private static void ControllerStopsSessionSelectionWhenLoadFails()
    {
        var continuation = new FakeHostFlowContinuation { LoadExistingFailure = "load failed" };
        var session = new HostFlowSession();
        var controller = CreateController(new FakeHostFlowSaveBank(), continuation: continuation, session: session);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.False(result.Success);
        AssertEx.Equal("load failed", result.ErrorMessage);
        AssertEx.Equal(null, session.SelectedCampaignId);
        AssertEx.Equal(null, session.SelectedGameMode);
        AssertEx.False(session.IsPendingNewRun);
    }

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

    private static HostFlowController CreateController(
        FakeHostFlowSaveBank? bank = null,
        FakeActiveSaveActivator? activator = null,
        FakeHostFlowContinuation? continuation = null,
        HostFlowSession? session = null,
        FakeActiveSavePreflight? preflight = null)
    {
        return new HostFlowController(
            bank ?? new FakeHostFlowSaveBank(),
            preflight ?? new FakeActiveSavePreflight(),
            activator ?? new FakeActiveSaveActivator(),
            continuation ?? new FakeHostFlowContinuation(),
            session ?? new HostFlowSession(),
            new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));
    }

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }

    private sealed class FakeHostFlowSaveBank : IHostFlowSaveBank
    {
        public IReadOnlyList<CampaignMetadata> Campaigns { get; init; } = [];

        public IReadOnlyList<CampaignMetadata> ListCampaigns(MultiplayerGameMode gameMode) =>
            Campaigns.Where(campaign => campaign.GameMode == gameMode).ToList();
    }

    private sealed class FakeActiveSaveActivator : IActiveSaveActivator
    {
        public string? ActivatedCampaignId { get; private set; }
        public string? Failure { get; init; }
        public string? RestoreFailure { get; init; }
        public int RestoreCount { get; private set; }

        public OperationResult Activate(string campaignId, DateTimeOffset nowUtc)
        {
            if (Failure is not null)
                return OperationResult.Fail(Failure);

            ActivatedCampaignId = campaignId;
            return OperationResult.Ok();
        }

        public OperationResult RestorePreviousActive(DateTimeOffset nowUtc)
        {
            RestoreCount++;
            if (RestoreFailure is not null)
                return OperationResult.Fail(RestoreFailure);

            return OperationResult.Ok();
        }
    }

    private sealed class FakeHostFlowContinuation : IHostFlowContinuation
    {
        public int StartNewRunCount { get; private set; }
        public int PrepareLoadExistingCount { get; private set; }
        public int LoadExistingCount { get; private set; }
        public string? StartNewRunFailure { get; init; }
        public string? PrepareLoadExistingFailure { get; init; }
        public string? LoadExistingFailure { get; init; }

        public OperationResult StartNewRun(MultiplayerGameMode gameMode)
        {
            StartNewRunCount++;
            if (StartNewRunFailure is not null)
                return OperationResult.Fail(StartNewRunFailure);

            return OperationResult.Ok();
        }

        public OperationResult PrepareLoadExistingRun()
        {
            PrepareLoadExistingCount++;
            if (PrepareLoadExistingFailure is not null)
                return OperationResult.Fail(PrepareLoadExistingFailure);

            return OperationResult.Ok();
        }

        public OperationResult LoadExistingRun()
        {
            LoadExistingCount++;
            if (LoadExistingFailure is not null)
                return OperationResult.Fail(LoadExistingFailure);

            return OperationResult.Ok();
        }
    }

    private sealed class FakeActiveSavePreflight : IActiveSavePreflight
    {
        public string? Failure { get; init; }

        public OperationResult EnsureActiveSaveCanBeReplaced() =>
            Failure is null ? OperationResult.Ok() : OperationResult.Fail(Failure);
    }

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
}
