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
        yield return new TestCase("picker subtitle omits unknown progress", PickerSubtitleOmitsUnknownProgress);
        yield return new TestCase("picker campaign row includes full details", PickerCampaignRowIncludesFullDetails);
        yield return new TestCase("picker details handle missing progress and roster", PickerDetailsHandleMissingProgressAndRoster);
        yield return new TestCase("picker start new row has no details", PickerStartNewRowHasNoDetails);
        yield return new TestCase("controller starts new run through continuation", ControllerStartsNewRunThroughContinuation);
        yield return new TestCase("controller does not start new run when active preflight fails", ControllerStopsStartNewWhenPreflightFails);
        yield return new TestCase("controller does not select session when start new continuation fails", ControllerStopsSessionSelectionWhenStartNewFails);
        yield return new TestCase("controller activates existing campaign before load continuation", ControllerActivatesExistingCampaign);
        yield return new TestCase("controller repairs metadata after existing campaign activation", ControllerRepairsMetadataAfterExistingCampaignActivation);
        yield return new TestCase("controller continues existing campaign when metadata repair fails", ControllerContinuesExistingCampaignWhenMetadataRepairFails);
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
        yield return new TestCase("save sync finalizes pending new run with metadata", SaveSyncFinalizesPendingNewRunWithMetadata);
        yield return new TestCase("save sync keeps pending new run selected when finalization fails", SaveSyncKeepsPendingNewRunWhenFinalizationFails);
        yield return new TestCase("save sync maps sync exceptions to failed result", SaveSyncMapsExceptions);
        yield return new TestCase("controller exposes recovery model", ControllerExposesRecoveryModel);
        yield return new TestCase("controller recovers then starts new run", ControllerRecoversThenStartsNewRun);
        yield return new TestCase("controller recovers then selects existing campaign", ControllerRecoversThenSelectsExistingCampaign);
        yield return new TestCase("controller stops when recovery fails", ControllerStopsWhenRecoveryFails);
        yield return new TestCase("recovery model reports unavailable when no options exist", RecoveryModelReportsUnavailableWhenNoOptionsExist);
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

    private static void PickerSubtitleOmitsUnknownProgress()
    {
        var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            MultiplayerGameMode.Standard,
            "buddy1, buddy2 +2",
            [
                new PlayerIdentity("1", "buddy1"),
                new PlayerIdentity("2", "buddy2"),
                new PlayerIdentity("3", "buddy3"),
                new PlayerIdentity("4", "buddy4")
            ],
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            DateTimeOffset.Parse("2026-05-08T01:00:00Z"),
            null,
            "checksum",
            null));

        AssertEx.Equal("buddy1, buddy2 +2", row.Title);
        AssertEx.Equal("4 players", row.Subtitle);
    }

    private static void PickerCampaignRowIncludesFullDetails()
    {
        var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            MultiplayerGameMode.Standard,
            "buddy1, buddy2 +2",
            [
                new PlayerIdentity("1", "buddy1"),
                new PlayerIdentity("2", "buddy2"),
                new PlayerIdentity("3", "buddy3"),
                new PlayerIdentity("4", "buddy4")
            ],
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            DateTimeOffset.Parse("2026-05-08T01:30:00Z"),
            null,
            "checksum",
            "Floor 18"));

        var details = row.Details ?? throw new InvalidOperationException("Expected campaign details");

        AssertEx.Equal("buddy1, buddy2 +2", details.Title);
        AssertEx.Equal("Floor 18 - 4 players", details.Subtitle);
        AssertEx.Equal(5, details.SummaryLines.Count);
        AssertEx.Equal("Progress: Floor 18", details.SummaryLines[0]);
        AssertEx.Equal("Players: 4", details.SummaryLines[1]);
        AssertEx.Equal("Created: 2026-05-08 00:00 UTC", details.SummaryLines[2]);
        AssertEx.Equal("Last played: 2026-05-08 01:30 UTC", details.SummaryLines[3]);
        AssertEx.Equal("Campaign id: aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", details.SummaryLines[4]);
        AssertEx.Equal(4, details.RosterLines.Count);
        AssertEx.Equal("1. buddy1", details.RosterLines[0]);
        AssertEx.Equal("2. buddy2", details.RosterLines[1]);
        AssertEx.Equal("3. buddy3", details.RosterLines[2]);
        AssertEx.Equal("4. buddy4", details.RosterLines[3]);
    }

    private static void PickerDetailsHandleMissingProgressAndRoster()
    {
        var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
            "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb",
            MultiplayerGameMode.Standard,
            "Unknown party",
            [],
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
            null,
            "checksum",
            null));

        var details = row.Details ?? throw new InvalidOperationException("Expected campaign details");

        AssertEx.Equal("0 players", row.Subtitle);
        AssertEx.Equal("Progress: Unknown", details.SummaryLines[0]);
        AssertEx.Equal("Players: 0", details.SummaryLines[1]);
        AssertEx.Equal(1, details.RosterLines.Count);
        AssertEx.Equal("Unknown party", details.RosterLines[0]);
    }

    private static void PickerStartNewRowHasNoDetails()
    {
        var row = MultiplayerSavePickerRow.StartNew();

        AssertEx.Equal(null, row.Details);
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

    private static void ControllerRepairsMetadataAfterExistingCampaignActivation()
    {
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation();
        var repair = new FakeActivatedCampaignMetadataRepair();
        var controller = CreateController(
            new FakeHostFlowSaveBank(),
            activator,
            continuation,
            metadataRepair: repair);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.True(result.Success);
        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", repair.RepairedCampaignId);
        AssertEx.Equal(DateTimeOffset.Parse("2026-05-08T12:00:00Z"), repair.RepairedAtUtc);
        AssertEx.Equal(1, continuation.LoadExistingCount);
    }

    private static void ControllerContinuesExistingCampaignWhenMetadataRepairFails()
    {
        var activator = new FakeActiveSaveActivator();
        var continuation = new FakeHostFlowContinuation();
        var repair = new FakeActivatedCampaignMetadataRepair { ThrowOnRepair = true };
        var controller = CreateController(
            new FakeHostFlowSaveBank(),
            activator,
            continuation,
            metadataRepair: repair);

        var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.True(result.Success);
        AssertEx.Equal(1, repair.RepairCount);
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

    private static void SaveSyncFinalizesPendingNewRunWithMetadata()
    {
        var sync = new FakeActiveSaveSync { FinalizedCampaignId = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb" };
        var session = new HostFlowSession();
        session.SelectNewRun(MultiplayerGameMode.Custom);
        session.CapturePendingNewRunMetadata(new CampaignMetadataSnapshot(
            [new PlayerIdentity("steam:1", "buddy1")],
            "Floor 18"));
        var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

        var result = controller.SyncAfterVanillaSave();

        AssertEx.True(result.Success);
        AssertEx.Equal(1, sync.FinalizeCount);
        AssertEx.Equal("buddy1", sync.FinalizedMetadata?.Roster[0].DisplayName);
        AssertEx.Equal("Floor 18", sync.FinalizedMetadata?.ActOrFloor);
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
        var controller = CreateController(
            new FakeHostFlowSaveBank(),
            continuation: continuation,
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
        var controller = CreateController(
            new FakeHostFlowSaveBank(),
            activator,
            continuation,
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

    private static void RecoveryModelReportsUnavailableWhenNoOptionsExist()
    {
        var model = ActiveSaveRecoveryModel.None();

        AssertEx.False(model.HasOptions);
        AssertEx.Equal("No recovery action is available.", model.Message);
    }

    private static HostFlowController CreateController(
        FakeHostFlowSaveBank? bank = null,
        FakeActiveSaveActivator? activator = null,
        FakeHostFlowContinuation? continuation = null,
        HostFlowSession? session = null,
        FakeActiveSavePreflight? preflight = null,
        FakeActiveSaveRecovery? recovery = null,
        FakeActivatedCampaignMetadataRepair? metadataRepair = null)
    {
        return new HostFlowController(
            bank ?? new FakeHostFlowSaveBank(),
            preflight ?? new FakeActiveSavePreflight(),
            activator ?? new FakeActiveSaveActivator(),
            continuation ?? new FakeHostFlowContinuation(),
            recovery ?? new FakeActiveSaveRecovery(),
            session ?? new HostFlowSession(),
            new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")),
            metadataRepair ?? new FakeActivatedCampaignMetadataRepair());
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
        public int FailuresBeforeSuccess { get; init; } = int.MaxValue;
        private int _calls;

        public OperationResult EnsureActiveSaveCanBeReplaced()
        {
            _calls++;
            return Failure is not null && _calls <= FailuresBeforeSuccess
                ? OperationResult.Fail(Failure)
                : OperationResult.Ok();
        }
    }

    private sealed class FakeActivatedCampaignMetadataRepair : IActivatedCampaignMetadataRepair
    {
        public string? RepairedCampaignId { get; private set; }
        public DateTimeOffset? RepairedAtUtc { get; private set; }
        public bool ThrowOnRepair { get; init; }
        public int RepairCount { get; private set; }

        public void RepairActivatedCampaign(string campaignId, DateTimeOffset nowUtc)
        {
            RepairCount++;
            RepairedCampaignId = campaignId;
            RepairedAtUtc = nowUtc;
            if (ThrowOnRepair)
                throw new InvalidOperationException("repair failed");
        }
    }

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

    private sealed class FakeActiveSaveSync : IActiveSaveSync
    {
        public int SyncBackCount { get; private set; }
        public int FinalizeCount { get; private set; }
        public string FinalizedCampaignId { get; init; } = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
        public string? FinalizeFailure { get; init; }
        public MultiplayerGameMode? FinalizedGameMode { get; private set; }
        public CampaignMetadataSnapshot? FinalizedMetadata { get; private set; }

        public OperationResult SyncBack(DateTimeOffset nowUtc)
        {
            SyncBackCount++;
            return OperationResult.Ok();
        }

        public OperationResult<string> FinalizePendingNewRun(
            MultiplayerGameMode gameMode,
            CampaignMetadataSnapshot metadata,
            DateTimeOffset nowUtc)
        {
            FinalizeCount++;
            FinalizedGameMode = gameMode;
            FinalizedMetadata = metadata;
            return FinalizeFailure is null
                ? OperationResult<string>.Ok(FinalizedCampaignId)
                : OperationResult<string>.Fail(FinalizeFailure);
        }
    }

    private sealed class ThrowingActiveSaveSync : IActiveSaveSync
    {
        public OperationResult SyncBack(DateTimeOffset nowUtc) => throw new InvalidOperationException("sync exploded");

        public OperationResult<string> FinalizePendingNewRun(
            MultiplayerGameMode gameMode,
            CampaignMetadataSnapshot metadata,
            DateTimeOffset nowUtc) =>
            throw new InvalidOperationException("finalize exploded");
    }
}
