using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Storage;

namespace MultiplayerSaveSlots.Tests;

public static class MultiplayerSaveBankTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("save bank creates campaign with payload and metadata", CreatesCampaign));
        tests.Add(new TestCase("save bank lists campaigns by gamemode", ListsByGameMode));
    }

    private static void CreatesCampaign()
    {
        using var temp = new TempDirectory();
        var payload = Path.Combine(temp.Path, "vanilla.save");
        File.WriteAllText(payload, "run-payload");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [new PlayerIdentity("steam:1", "buddy1")],
            payload,
            new DateTimeOffset(2026, 5, 8, 12, 0, 0, TimeSpan.Zero)));

        var campaignDir = Path.Combine(temp.Path, "MultiSaves", "saves", metadata.CampaignId);
        AssertEx.True(File.Exists(Path.Combine(campaignDir, "metadata.json")));
        AssertEx.True(File.Exists(Path.Combine(campaignDir, "multiplayer_run.save")));
        AssertEx.Equal("buddy1", metadata.Label);
        AssertEx.Equal("run-payload", File.ReadAllText(Path.Combine(campaignDir, "multiplayer_run.save")));
    }

    private static void ListsByGameMode()
    {
        using var temp = new TempDirectory();
        var standardPayload = Path.Combine(temp.Path, "standard.save");
        var dailyPayload = Path.Combine(temp.Path, "daily.save");
        File.WriteAllText(standardPayload, "standard");
        File.WriteAllText(dailyPayload, "daily");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], standardPayload, DateTimeOffset.UtcNow));
        bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Daily, [], dailyPayload, DateTimeOffset.UtcNow));

        var standard = bank.ListCampaigns(MultiplayerGameMode.Standard);
        var daily = bank.ListCampaigns(MultiplayerGameMode.Daily);

        AssertEx.Equal(1, standard.Count);
        AssertEx.Equal(1, daily.Count);
        AssertEx.Equal(MultiplayerGameMode.Standard, standard[0].GameMode);
        AssertEx.Equal(MultiplayerGameMode.Daily, daily[0].GameMode);
    }
}
