using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.Tests;

public static class HostFlowControllerTests
{
    public static IEnumerable<TestCase> All()
    {
        yield return new TestCase("runtime paths place bank beside active multiplayer save", RuntimePathsPlaceBankBesideActiveSave);
        yield return new TestCase("host flow session tracks existing campaign selection", SessionTracksExistingCampaignSelection);
        yield return new TestCase("host flow session tracks pending new run", SessionTracksPendingNewRun);
        yield return new TestCase("host flow session clears selected and pending state", SessionClearsSelectedAndPendingState);
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
}
