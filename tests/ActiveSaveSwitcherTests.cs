using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Storage;

namespace MultiplayerSaveSlots.Tests;

public static class ActiveSaveSwitcherTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("switcher activates campaign into active save slot", ActivatesCampaign));
        tests.Add(new TestCase("switcher syncs active save back to selected campaign", SyncsBack));
        tests.Add(new TestCase("switcher rejects sync when active checksum changed before selection", RejectsUnexpectedActiveChange));
    }

    private static void ActivatesCampaign()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);

        AssertEx.Equal("campaign", File.ReadAllText(active));
        AssertEx.True(File.Exists(Path.Combine(temp.Path, "active-state.json")));
    }

    private static void SyncsBack()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "campaign-progress");
        switcher.SyncBack(DateTimeOffset.UtcNow);

        AssertEx.Equal("campaign-progress", File.ReadAllText(bank.GetPayloadPath(metadata.CampaignId)));
    }

    private static void RejectsUnexpectedActiveChange()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.Delete(Path.Combine(temp.Path, "active-state.json"));

        AssertEx.Throws<InvalidOperationException>(() => switcher.SyncBack(DateTimeOffset.UtcNow));
    }
}
