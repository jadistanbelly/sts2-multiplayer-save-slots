using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Storage;
using System.Text.Json;

namespace MultiplayerSaveSlots.Tests;

public static class ActiveSaveSwitcherTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("switcher activates campaign into active save slot", ActivatesCampaign));
        tests.Add(new TestCase("switcher activation backs up existing active save", ActivationBacksUpExistingActiveSave));
        tests.Add(new TestCase("switcher records previous active checksum and backs up active file", BacksUpPreviousActive));
        tests.Add(new TestCase("switcher activation updates metadata", ActivationUpdatesMetadata));
        tests.Add(new TestCase("switcher activation rejects missing bank payload", ActivationRejectsMissingBankPayload));
        tests.Add(new TestCase("switcher activation supports bare active and state paths", ActivationSupportsBareActiveAndStatePaths));
        tests.Add(new TestCase("switcher activation fails before mutating active save when metadata is missing", ActivationMissingMetadataFailsBeforeMutatingActiveSave));
        tests.Add(new TestCase("switcher activation fails before mutating active save when metadata is malformed", ActivationMalformedMetadataFailsBeforeMutatingActiveSave));
        tests.Add(new TestCase("switcher activation fails before mutating active save when campaign is unindexed", ActivationUnindexedCampaignFailsBeforeMutatingActiveSave));
        tests.Add(new TestCase("switcher syncs active save back to selected campaign", SyncsBack));
        tests.Add(new TestCase("switcher sync-back backs up bank payload", SyncBackBacksUpBankPayload));
        tests.Add(new TestCase("switcher sync-back updates metadata", SyncBackUpdatesMetadata));
        tests.Add(new TestCase("switcher sync-back rejects missing active save", SyncBackRejectsMissingActiveSave));
        tests.Add(new TestCase("switcher rejects sync without active state", RejectsSyncWithoutActiveState));
        tests.Add(new TestCase("switcher sync-back fails before mutating payload when metadata is missing", SyncBackMissingMetadataFailsBeforeMutatingPayload));
        tests.Add(new TestCase("switcher sync-back fails before mutating payload when metadata is malformed", SyncBackMalformedMetadataFailsBeforeMutatingPayload));
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

    private static void ActivationBacksUpExistingActiveSave()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");
        File.WriteAllText(active, "existing-active");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, new DateTimeOffset(2026, 5, 8, 12, 0, 0, TimeSpan.Zero));

        var backup = SingleFile(bank.GetBackupDirectory(metadata.CampaignId));
        AssertEx.Equal("existing-active", File.ReadAllText(backup));
    }

    private static void BacksUpPreviousActive()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(source, "campaign");
        File.WriteAllText(active, "previous-active");
        var checksumBeforeActivation = FileChecksum.Sha256(active);

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, state);

        switcher.Activate(metadata.CampaignId, new DateTimeOffset(2026, 5, 8, 13, 0, 0, TimeSpan.Zero));

        var backups = Directory.GetFiles(bank.GetBackupDirectory(metadata.CampaignId));
        AssertEx.Equal(1, backups.Length);
        AssertEx.Equal("previous-active", File.ReadAllText(backups[0]));

        using var stateJson = JsonDocument.Parse(File.ReadAllText(state));
        AssertEx.Equal(checksumBeforeActivation, stateJson.RootElement.GetProperty("activeChecksumBeforeActivation").GetString());
    }

    private static void ActivationUpdatesMetadata()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var now = new DateTimeOffset(2026, 5, 8, 13, 0, 0, TimeSpan.Zero);
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, now);

        var updated = bank.GetCampaign(metadata.CampaignId);
        AssertEx.Equal(FileChecksum.Sha256(active), updated.ActiveChecksum);
        AssertEx.Equal(FileChecksum.Sha256(bank.GetPayloadPath(metadata.CampaignId)), updated.PayloadChecksum);
        AssertEx.Equal(now, updated.LastPlayedAtUtc);
    }

    private static void ActivationRejectsMissingBankPayload()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        File.Delete(bank.GetPayloadPath(metadata.CampaignId));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        AssertEx.Throws<FileNotFoundException>(() => switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow));
    }

    private static void ActivationSupportsBareActiveAndStatePaths()
    {
        using var temp = new TempDirectory();
        var originalDirectory = Directory.GetCurrentDirectory();
        try
        {
            Directory.SetCurrentDirectory(temp.Path);
            File.WriteAllText("source.save", "campaign");

            var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
            var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], "source.save", DateTimeOffset.UtcNow));
            var switcher = new ActiveSaveSwitcher(bank, "active.save", "active-state.json");

            switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);

            AssertEx.Equal("campaign", File.ReadAllText("active.save"));
            AssertEx.True(File.Exists("active-state.json"));
        }
        finally
        {
            Directory.SetCurrentDirectory(originalDirectory);
        }
    }

    private static void ActivationMissingMetadataFailsBeforeMutatingActiveSave()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(source, "campaign");
        File.WriteAllText(active, "previous-active");

        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        File.Delete(paths.MetadataPath(metadata.CampaignId));
        var switcher = new ActiveSaveSwitcher(bank, active, state);

        AssertEx.Throws<FileNotFoundException>(() => switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow));
        AssertEx.Equal("previous-active", File.ReadAllText(active));
        AssertEx.False(File.Exists(state));
    }

    private static void ActivationMalformedMetadataFailsBeforeMutatingActiveSave()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(source, "campaign");
        File.WriteAllText(active, "previous-active");

        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        File.WriteAllText(paths.MetadataPath(metadata.CampaignId), "{ not valid json");
        var switcher = new ActiveSaveSwitcher(bank, active, state);

        AssertEx.Throws<JsonException>(() => switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow));
        AssertEx.Equal("previous-active", File.ReadAllText(active));
        AssertEx.False(File.Exists(state));
    }

    private static void ActivationUnindexedCampaignFailsBeforeMutatingActiveSave()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(source, "campaign");
        File.WriteAllText(active, "previous-active");

        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        JsonFile.Write(paths.IndexPath, CampaignIndex.Empty);
        var switcher = new ActiveSaveSwitcher(bank, active, state);

        AssertEx.Throws<InvalidOperationException>(() => switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow));
        AssertEx.Equal("previous-active", File.ReadAllText(active));
        AssertEx.False(File.Exists(state));
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

    private static void SyncBackBacksUpBankPayload()
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
        switcher.SyncBack(new DateTimeOffset(2026, 5, 8, 14, 0, 0, TimeSpan.Zero));

        var backup = SingleFile(bank.GetBackupDirectory(metadata.CampaignId));
        AssertEx.Equal("campaign", File.ReadAllText(backup));
    }

    private static void SyncBackUpdatesMetadata()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var now = new DateTimeOffset(2026, 5, 8, 15, 0, 0, TimeSpan.Zero);
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "campaign-progress");
        switcher.SyncBack(now);

        var updated = bank.GetCampaign(metadata.CampaignId);
        AssertEx.Equal(FileChecksum.Sha256(active), updated.ActiveChecksum);
        AssertEx.Equal(FileChecksum.Sha256(bank.GetPayloadPath(metadata.CampaignId)), updated.PayloadChecksum);
        AssertEx.Equal(now, updated.LastPlayedAtUtc);
    }

    private static void SyncBackRejectsMissingActiveSave()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.Delete(active);

        AssertEx.Throws<FileNotFoundException>(() => switcher.SyncBack(DateTimeOffset.UtcNow));
    }

    private static void RejectsSyncWithoutActiveState()
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

    private static void SyncBackMissingMetadataFailsBeforeMutatingPayload()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "campaign-progress");
        File.Delete(paths.MetadataPath(metadata.CampaignId));

        AssertEx.Throws<FileNotFoundException>(() => switcher.SyncBack(DateTimeOffset.UtcNow));
        AssertEx.Equal("campaign", File.ReadAllText(bank.GetPayloadPath(metadata.CampaignId)));
    }

    private static void SyncBackMalformedMetadataFailsBeforeMutatingPayload()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "campaign-progress");
        File.WriteAllText(paths.MetadataPath(metadata.CampaignId), "{ not valid json");

        AssertEx.Throws<JsonException>(() => switcher.SyncBack(DateTimeOffset.UtcNow));
        AssertEx.Equal("campaign", File.ReadAllText(bank.GetPayloadPath(metadata.CampaignId)));
    }

    private static string SingleFile(string directory)
    {
        var files = Directory.GetFiles(directory);
        AssertEx.Equal(1, files.Length);
        return files[0];
    }
}
