using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Tests;

public static class CampaignLabelerTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("labeler uses one player name", OnePlayer));
        tests.Add(new TestCase("labeler uses two player names", TwoPlayers));
        tests.Add(new TestCase("labeler compacts large roster", ManyPlayers));
        tests.Add(new TestCase("labeler handles empty roster", EmptyRoster));
        tests.Add(new TestCase("labeler trims player names", TrimmedPlayerName));
        tests.Add(new TestCase("labeler uses unknown for whitespace player names", WhitespacePlayerName));
    }

    private static void OnePlayer()
    {
        var label = CampaignLabeler.Build([
            new PlayerIdentity("steam:1", "buddy1")
        ]);

        AssertEx.Equal("buddy1", label);
    }

    private static void TwoPlayers()
    {
        var label = CampaignLabeler.Build([
            new PlayerIdentity("steam:1", "buddy1"),
            new PlayerIdentity("steam:2", "buddy2")
        ]);

        AssertEx.Equal("buddy1 + buddy2", label);
    }

    private static void ManyPlayers()
    {
        var label = CampaignLabeler.Build([
            new PlayerIdentity("steam:1", "buddy1"),
            new PlayerIdentity("steam:2", "buddy2"),
            new PlayerIdentity("steam:3", "buddy3"),
            new PlayerIdentity("steam:4", "buddy4"),
            new PlayerIdentity("steam:5", "buddy5")
        ]);

        AssertEx.Equal("buddy1 + buddy2 + 3 more", label);
    }

    private static void EmptyRoster()
    {
        AssertEx.Equal("Unknown party", CampaignLabeler.Build([]));
    }

    private static void TrimmedPlayerName()
    {
        var label = CampaignLabeler.Build([
            new PlayerIdentity("steam:1", "  buddy1  ")
        ]);

        AssertEx.Equal("buddy1", label);
    }

    private static void WhitespacePlayerName()
    {
        var label = CampaignLabeler.Build([
            new PlayerIdentity("steam:1", "   ")
        ]);

        AssertEx.Equal("Unknown", label);
    }
}
