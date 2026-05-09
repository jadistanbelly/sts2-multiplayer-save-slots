using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;
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
        tests.Add(new TestCase("sync-back rejects stale bank payload before mutation", SyncBackRejectsStaleBankPayloadBeforeMutation));
        tests.Add(new TestCase("sync-back rejects active save matching previous active checksum", SyncBackRejectsActiveSaveMatchingPreviousActiveChecksum));
        tests.Add(new TestCase("sync-back updates active state checksum after successful sync", SyncBackUpdatesActiveStateChecksumAfterSuccessfulSync));
        tests.Add(new TestCase("activation rejects unsynced active state before mutation", ActivationRejectsUnsyncedActiveStateBeforeMutation));
        tests.Add(new TestCase("activation allows switching when active state is clean", ActivationAllowsSwitchingWhenActiveStateIsClean));
        tests.Add(new TestCase("switcher restores previous active save after activation", RestoresPreviousActiveAfterActivation));
        tests.Add(new TestCase("switcher restore rejects changed activated save", RestoreRejectsChangedActivatedSave));
        tests.Add(new TestCase("switcher restore fails before mutating when previous state backup is missing", RestoreFailsBeforeMutatingWhenPreviousStateBackupMissing));
        tests.Add(new TestCase("switcher sync-back fails before mutating payload when metadata is missing", SyncBackMissingMetadataFailsBeforeMutatingPayload));
        tests.Add(new TestCase("switcher sync-back fails before mutating payload when metadata is malformed", SyncBackMalformedMetadataFailsBeforeMutatingPayload));
        tests.Add(new TestCase("switcher claims active save for pending new campaign", ClaimsActiveSaveForPendingNewCampaign));
        tests.Add(new TestCase("switcher claim rejects missing active save", ClaimRejectsMissingActiveSave));
        tests.Add(new TestCase("switcher claim rejects mismatched campaign payload", ClaimRejectsMismatchedCampaignPayload));
        tests.Add(new TestCase("recovery offers duplicate for unmanaged active save", RecoveryOffersDuplicateForUnmanagedActiveSave));
        tests.Add(new TestCase("recovery offers sync for unsynced managed active save", RecoveryOffersSyncForUnsyncedManagedActiveSave));
        tests.Add(new TestCase("recovery duplicates active save into bank", RecoveryDuplicatesActiveSaveIntoBank));
        tests.Add(new TestCase("recovery duplicates active save with captured metadata", RecoveryDuplicatesActiveSaveWithCapturedMetadata));
        tests.Add(new TestCase("recovery duplicates active save when metadata extractor fails", RecoveryDuplicatesActiveSaveWhenMetadataExtractorFails));
        tests.Add(new TestCase("recovery syncs active save to selected campaign", RecoverySyncsActiveSaveToSelectedCampaign));
        tests.Add(new TestCase("sync-back refreshes progress metadata", SyncBackRefreshesProgressMetadata));
        tests.Add(new TestCase("active save sync refreshes roster character metadata", ActiveSaveSyncRefreshesRosterCharacterMetadata));
        tests.Add(new TestCase("active save sync finalizes pending new run when metadata extractor fails", ActiveSaveSyncFinalizesPendingNewRunWhenMetadataExtractorFails));
        tests.Add(new TestCase("metadata repair updates empty roster and progress", MetadataRepairUpdatesEmptyRosterAndProgress));
        tests.Add(new TestCase("metadata repair preserves existing roster", MetadataRepairPreservesExistingRoster));
        tests.Add(new TestCase("metadata repair ignores extractor failure", MetadataRepairIgnoresExtractorFailure));
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

    private static void SyncBackRefreshesProgressMetadata()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var now = new DateTimeOffset(2026, 5, 8, 15, 30, 0, TimeSpan.Zero);
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [new PlayerIdentity("steam:1", "buddy1")],
            source,
            DateTimeOffset.UtcNow,
            "Floor 4"));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "campaign-progress");
        switcher.SyncBack(now, "Floor 5");

        var updated = bank.GetCampaign(metadata.CampaignId);
        AssertEx.Equal("buddy1", updated.Roster[0].DisplayName);
        AssertEx.Equal("Floor 5", updated.ActOrFloor);
    }

    private static void ActiveSaveSyncRefreshesRosterCharacterMetadata()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var now = new DateTimeOffset(2026, 5, 8, 16, 0, 0, TimeSpan.Zero);
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [new PlayerIdentity("steam:1", "buddy1")],
            source,
            DateTimeOffset.UtcNow,
            "Floor 4"));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));
        var sync = new Sts2ActiveSaveSync(
            bank,
            switcher,
            active,
            new FakeCampaignMetadataExtractor(new CampaignMetadataSnapshot(
                [new PlayerIdentity("steam:1", "buddy1", "CHARACTER.IRONCLAD")],
                "Floor 5")));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "campaign-progress");
        var result = sync.SyncBack(now);

        AssertEx.True(result.Success);
        var updated = bank.GetCampaign(metadata.CampaignId);
        AssertEx.Equal("CHARACTER.IRONCLAD", updated.Roster[0].SelectedCharacterId);
        AssertEx.Equal("Floor 5", updated.ActOrFloor);
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

    private static void SyncBackRejectsStaleBankPayloadBeforeMutation()
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
        File.WriteAllText(bank.GetPayloadPath(metadata.CampaignId), "external-bank-change");

        AssertEx.Throws<InvalidOperationException>(() => switcher.SyncBack(DateTimeOffset.UtcNow));
        AssertEx.Equal("external-bank-change", File.ReadAllText(bank.GetPayloadPath(metadata.CampaignId)));
        AssertNoBackups(bank.GetBackupDirectory(metadata.CampaignId));
    }

    private static void SyncBackRejectsActiveSaveMatchingPreviousActiveChecksum()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");
        File.WriteAllText(active, "previous-active");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "previous-active");

        AssertEx.Throws<InvalidOperationException>(() => switcher.SyncBack(DateTimeOffset.UtcNow));
        AssertEx.Equal("campaign", File.ReadAllText(bank.GetPayloadPath(metadata.CampaignId)));
    }

    private static void SyncBackUpdatesActiveStateChecksumAfterSuccessfulSync()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(source, "campaign");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, state);

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "campaign-progress-1");
        switcher.SyncBack(DateTimeOffset.UtcNow);

        using (var stateJson = JsonDocument.Parse(File.ReadAllText(state)))
        {
            AssertEx.Equal(FileChecksum.Sha256(active), stateJson.RootElement.GetProperty("activeChecksumAfterActivation").GetString());
        }

        File.WriteAllText(active, "campaign-progress-2");
        switcher.SyncBack(DateTimeOffset.UtcNow);

        AssertEx.Equal("campaign-progress-2", File.ReadAllText(bank.GetPayloadPath(metadata.CampaignId)));
    }

    private static void ActivationRejectsUnsyncedActiveStateBeforeMutation()
    {
        using var temp = new TempDirectory();
        var sourceA = Path.Combine(temp.Path, "source-a.save");
        var sourceB = Path.Combine(temp.Path, "source-b.save");
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(sourceA, "campaign-a");
        File.WriteAllText(sourceB, "campaign-b");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadataA = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], sourceA, DateTimeOffset.UtcNow));
        var metadataB = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], sourceB, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, state);

        switcher.Activate(metadataA.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "campaign-a-progress");

        AssertEx.Throws<InvalidOperationException>(() => switcher.Activate(metadataB.CampaignId, DateTimeOffset.UtcNow));
        AssertEx.Equal("campaign-a-progress", File.ReadAllText(active));

        var activeState = JsonFile.Read<ActiveSaveState>(state);
        AssertEx.Equal(metadataA.CampaignId, activeState.CampaignId);
        AssertNoBackups(bank.GetBackupDirectory(metadataB.CampaignId));
    }

    private static void ActivationAllowsSwitchingWhenActiveStateIsClean()
    {
        using var temp = new TempDirectory();
        var sourceA = Path.Combine(temp.Path, "source-a.save");
        var sourceB = Path.Combine(temp.Path, "source-b.save");
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(sourceA, "campaign-a");
        File.WriteAllText(sourceB, "campaign-b");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadataA = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], sourceA, DateTimeOffset.UtcNow));
        var metadataB = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], sourceB, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, state);

        switcher.Activate(metadataA.CampaignId, DateTimeOffset.UtcNow);
        switcher.Activate(metadataB.CampaignId, DateTimeOffset.UtcNow);

        AssertEx.Equal("campaign-b", File.ReadAllText(active));

        var activeState = JsonFile.Read<ActiveSaveState>(state);
        AssertEx.Equal(metadataB.CampaignId, activeState.CampaignId);
    }

    private static void RestoresPreviousActiveAfterActivation()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(source, "campaign");
        File.WriteAllText(active, "previous-active");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, state);

        switcher.Activate(metadata.CampaignId, new DateTimeOffset(2026, 5, 8, 12, 0, 0, TimeSpan.Zero));
        switcher.RestorePreviousActive(new DateTimeOffset(2026, 5, 8, 12, 1, 0, TimeSpan.Zero));

        AssertEx.Equal("previous-active", File.ReadAllText(active));
        AssertEx.False(File.Exists(state));
    }

    private static void RestoreRejectsChangedActivatedSave()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(source, "campaign");
        File.WriteAllText(active, "previous-active");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, state);

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "changed-after-activation");

        AssertEx.Throws<InvalidOperationException>(() => switcher.RestorePreviousActive(DateTimeOffset.UtcNow));
        AssertEx.Equal("changed-after-activation", File.ReadAllText(active));
    }

    private static void RestoreFailsBeforeMutatingWhenPreviousStateBackupMissing()
    {
        using var temp = new TempDirectory();
        var sourceA = Path.Combine(temp.Path, "source-a.save");
        var sourceB = Path.Combine(temp.Path, "source-b.save");
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(sourceA, "campaign-a");
        File.WriteAllText(sourceB, "campaign-b");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadataA = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], sourceA, DateTimeOffset.UtcNow));
        var metadataB = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], sourceB, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, state);
        switcher.Activate(metadataA.CampaignId, new DateTimeOffset(2026, 5, 8, 12, 0, 0, TimeSpan.Zero));
        switcher.Activate(metadataB.CampaignId, new DateTimeOffset(2026, 5, 8, 12, 1, 0, TimeSpan.Zero));
        var currentState = JsonFile.Read<ActiveSaveState>(state);
        File.Delete(currentState.PreviousStateBackupPath!);

        AssertEx.Throws<FileNotFoundException>(() => switcher.RestorePreviousActive(DateTimeOffset.UtcNow));
        AssertEx.Equal("campaign-b", File.ReadAllText(active));
        AssertEx.Equal(metadataB.CampaignId, JsonFile.Read<ActiveSaveState>(state).CampaignId);
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

    private static void RecoveryOffersDuplicateForUnmanagedActiveSave()
    {
        using var temp = new TempDirectory();
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(active, "unmanaged-active");
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var switcher = new ActiveSaveSwitcher(bank, active, state);
        var recovery = new ActiveSaveRecoveryService(bank, switcher, active, state);

        var model = recovery.BuildRecoveryModel(MultiplayerGameMode.Standard);

        AssertEx.True(model.HasOptions);
        AssertEx.Equal("Current multiplayer save is not managed by Multiplayer Save Slots yet.", model.Message);
        AssertEx.Equal(ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign, model.Options[0].Kind);
    }

    private static void RecoveryOffersSyncForUnsyncedManagedActiveSave()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(source, "campaign");
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, state);
        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "campaign-progress");
        var recovery = new ActiveSaveRecoveryService(bank, switcher, active, state);

        var model = recovery.BuildRecoveryModel(MultiplayerGameMode.Standard);

        AssertEx.True(model.HasOptions);
        AssertEx.Equal("Active multiplayer save has unsynced changes", model.Title);
        AssertEx.Equal(ActiveSaveRecoveryActionKind.SyncActiveToCampaign, model.Options[0].Kind);
    }

    private static void RecoveryDuplicatesActiveSaveIntoBank()
    {
        using var temp = new TempDirectory();
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(active, "unmanaged-active");
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var switcher = new ActiveSaveSwitcher(bank, active, state);
        var recovery = new ActiveSaveRecoveryService(bank, switcher, active, state);

        var result = recovery.Recover(
            ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign,
            MultiplayerGameMode.Standard,
            new DateTimeOffset(2026, 5, 8, 17, 0, 0, TimeSpan.Zero));

        AssertEx.True(result.Success);
        var campaigns = bank.ListCampaigns(MultiplayerGameMode.Standard);
        AssertEx.Equal(1, campaigns.Count);
        AssertEx.Equal("unmanaged-active", File.ReadAllText(bank.GetPayloadPath(campaigns[0].CampaignId)));
        AssertEx.Equal(campaigns[0].CampaignId, JsonFile.Read<ActiveSaveState>(state).CampaignId);
    }

    private static void RecoveryDuplicatesActiveSaveWithCapturedMetadata()
    {
        using var temp = new TempDirectory();
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(active, "unmanaged-active");
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var switcher = new ActiveSaveSwitcher(bank, active, state);
        var recovery = new ActiveSaveRecoveryService(
            bank,
            switcher,
            active,
            state,
            new FakeCampaignMetadataExtractor(new CampaignMetadataSnapshot(
                [new PlayerIdentity("steam:1", "buddy1")],
                "Floor 18")));

        var result = recovery.Recover(
            ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign,
            MultiplayerGameMode.Standard,
            new DateTimeOffset(2026, 5, 8, 17, 0, 0, TimeSpan.Zero));

        AssertEx.True(result.Success);
        var campaigns = bank.ListCampaigns(MultiplayerGameMode.Standard);
        AssertEx.Equal(1, campaigns.Count);
        AssertEx.Equal("buddy1", campaigns[0].Label);
        AssertEx.Equal("Floor 18", campaigns[0].ActOrFloor);
    }

    private static void RecoveryDuplicatesActiveSaveWhenMetadataExtractorFails()
    {
        using var temp = new TempDirectory();
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(active, "unmanaged-active");
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var switcher = new ActiveSaveSwitcher(bank, active, state);
        var recovery = new ActiveSaveRecoveryService(
            bank,
            switcher,
            active,
            state,
            new ThrowingCampaignMetadataExtractor());

        var result = recovery.Recover(
            ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign,
            MultiplayerGameMode.Standard,
            new DateTimeOffset(2026, 5, 8, 17, 0, 0, TimeSpan.Zero));

        AssertEx.True(result.Success);
        var campaigns = bank.ListCampaigns(MultiplayerGameMode.Standard);
        AssertEx.Equal(1, campaigns.Count);
        AssertEx.Equal("Unknown party", campaigns[0].Label);
        AssertEx.Equal(null, campaigns[0].ActOrFloor);
    }

    private static void RecoverySyncsActiveSaveToSelectedCampaign()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(source, "campaign");
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, state);
        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "campaign-progress");
        var recovery = new ActiveSaveRecoveryService(bank, switcher, active, state);

        var result = recovery.Recover(
            ActiveSaveRecoveryActionKind.SyncActiveToCampaign,
            MultiplayerGameMode.Standard,
            new DateTimeOffset(2026, 5, 8, 17, 30, 0, TimeSpan.Zero));

        AssertEx.True(result.Success);
        AssertEx.Equal("campaign-progress", File.ReadAllText(bank.GetPayloadPath(metadata.CampaignId)));
    }

    private static void ActiveSaveSyncFinalizesPendingNewRunWhenMetadataExtractorFails()
    {
        using var temp = new TempDirectory();
        var active = Path.Combine(temp.Path, "active.save");
        var state = Path.Combine(temp.Path, "active-state.json");
        File.WriteAllText(active, "new-campaign");
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var switcher = new ActiveSaveSwitcher(bank, active, state);
        var sync = new Sts2ActiveSaveSync(bank, switcher, active, new ThrowingCampaignMetadataExtractor());

        var result = sync.FinalizePendingNewRun(
            MultiplayerGameMode.Standard,
            new CampaignMetadataSnapshot([new PlayerIdentity("steam:1", "buddy1")], "Floor 3"),
            new DateTimeOffset(2026, 5, 8, 18, 0, 0, TimeSpan.Zero));

        AssertEx.True(result.Success);
        var campaigns = bank.ListCampaigns(MultiplayerGameMode.Standard);
        AssertEx.Equal(1, campaigns.Count);
        AssertEx.Equal("buddy1", campaigns[0].Label);
        AssertEx.Equal("Floor 3", campaigns[0].ActOrFloor);
        AssertEx.Equal(campaigns[0].CampaignId, JsonFile.Read<ActiveSaveState>(state).CampaignId);
    }

    private static void MetadataRepairUpdatesEmptyRosterAndProgress()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        File.WriteAllText(source, "campaign");
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            source,
            DateTimeOffset.Parse("2026-05-08T00:00:00Z")));
        var repair = new ActivatedCampaignMetadataRepair(
            bank,
            new FakeCampaignMetadataExtractor(new CampaignMetadataSnapshot(
                [new PlayerIdentity("steam:1", "buddy1"), new PlayerIdentity("steam:2", "buddy2")],
                "Floor 12")));

        repair.RepairActivatedCampaign(metadata.CampaignId, DateTimeOffset.Parse("2026-05-08T12:00:00Z"));

        var updated = bank.GetCampaign(metadata.CampaignId);
        AssertEx.Equal("buddy1, buddy2", updated.Label);
        AssertEx.Equal(2, updated.Roster.Count);
        AssertEx.Equal("buddy1", updated.Roster[0].DisplayName);
        AssertEx.Equal("buddy2", updated.Roster[1].DisplayName);
        AssertEx.Equal("Floor 12", updated.ActOrFloor);
        AssertEx.Equal("campaign", File.ReadAllText(bank.GetPayloadPath(metadata.CampaignId)));
    }

    private static void MetadataRepairPreservesExistingRoster()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        File.WriteAllText(source, "campaign");
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [new PlayerIdentity("steam:existing", "existingBuddy")],
            source,
            DateTimeOffset.Parse("2026-05-08T00:00:00Z")));
        var repair = new ActivatedCampaignMetadataRepair(
            bank,
            new FakeCampaignMetadataExtractor(new CampaignMetadataSnapshot(
                [new PlayerIdentity("steam:new", "newBuddy")],
                "Floor 9")));

        repair.RepairActivatedCampaign(metadata.CampaignId, DateTimeOffset.Parse("2026-05-08T12:00:00Z"));

        var updated = bank.GetCampaign(metadata.CampaignId);
        AssertEx.Equal("existingBuddy", updated.Label);
        AssertEx.Equal(1, updated.Roster.Count);
        AssertEx.Equal("steam:existing", updated.Roster[0].StableId);
        AssertEx.Equal("Floor 9", updated.ActOrFloor);
    }

    private static void MetadataRepairIgnoresExtractorFailure()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        File.WriteAllText(source, "campaign");
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            source,
            DateTimeOffset.Parse("2026-05-08T00:00:00Z")));
        var repair = new ActivatedCampaignMetadataRepair(bank, new ThrowingCampaignMetadataExtractor());

        repair.RepairActivatedCampaign(metadata.CampaignId, DateTimeOffset.Parse("2026-05-08T12:00:00Z"));

        var updated = bank.GetCampaign(metadata.CampaignId);
        AssertEx.Equal("Unknown party", updated.Label);
        AssertEx.Equal(0, updated.Roster.Count);
        AssertEx.Equal(null, updated.ActOrFloor);
        AssertEx.Equal("campaign", File.ReadAllText(bank.GetPayloadPath(metadata.CampaignId)));
    }

    private static string SingleFile(string directory)
    {
        var files = Directory.GetFiles(directory);
        AssertEx.Equal(1, files.Length);
        return files[0];
    }

    private static void AssertNoBackups(string directory)
    {
        if (!Directory.Exists(directory))
            return;

        AssertEx.Equal(0, Directory.GetFiles(directory).Length);
    }

    private sealed class FakeCampaignMetadataExtractor(CampaignMetadataSnapshot snapshot) : ICampaignMetadataExtractor
    {
        public CampaignMetadataSnapshot CaptureActiveSaveMetadata() => snapshot;
    }

    private sealed class ThrowingCampaignMetadataExtractor : ICampaignMetadataExtractor
    {
        public CampaignMetadataSnapshot CaptureActiveSaveMetadata() =>
            throw new InvalidOperationException("metadata unavailable");
    }
}
