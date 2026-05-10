using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Storage;

namespace MultiplayerSaveSlots.Tests;

public static class MultiplayerSaveBankTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("save bank creates campaign with payload and metadata", CreatesCampaign));
        tests.Add(new TestCase("save bank lists campaigns by gamemode", ListsByGameMode));
        tests.Add(new TestCase("save bank persists metadata contents and json", PersistsMetadataContentsAndJson));
        tests.Add(new TestCase("save bank renames campaign with normalized custom name", RenamesCampaignWithNormalizedCustomName));
        tests.Add(new TestCase("save bank reads old metadata without custom name", ReadsOldMetadataWithoutCustomName));
        tests.Add(new TestCase("save bank writes created id once in index", WritesCreatedIdOnceInIndex));
        tests.Add(new TestCase("save bank payload checksum matches copied payload", PayloadChecksumMatchesCopiedPayload));
        tests.Add(new TestCase("save bank lists same mode by last played descending", ListsSameModeByLastPlayedDescending));
        tests.Add(new TestCase("save bank ignores duplicate ids in index", IgnoresDuplicateIdsInIndex));
        tests.Add(new TestCase("save bank skips missing metadata while listing", SkipsMissingMetadataWhileListing));
        tests.Add(new TestCase("save bank skips malformed metadata while listing", SkipsMalformedMetadataWhileListing));
        tests.Add(new TestCase("save bank skips metadata with mismatched campaign id while listing", SkipsMismatchedMetadataWhileListing));
        tests.Add(new TestCase("save bank update rejects unindexed campaign", UpdateRejectsUnindexedCampaign));
        tests.Add(new TestCase("save bank update rejects symlinked metadata path before mutation", UpdateRejectsSymlinkedMetadataPathBeforeMutation));
        tests.Add(new TestCase("save bank archives campaign into deleted area", ArchivesCampaignIntoDeletedArea));
        tests.Add(new TestCase("save bank archives campaign with suffix on collision", ArchivesCampaignWithSuffixOnCollision));
        tests.Add(new TestCase("save bank archive rejects symlinked campaign tree before mutation", ArchiveRejectsSymlinkedCampaignTreeBeforeMutation));
        tests.Add(new TestCase("save bank lists archived campaigns by gamemode", ListsArchivedCampaignsByGameMode));
        tests.Add(new TestCase("save bank restores archived campaign into active index", RestoresArchivedCampaignIntoActiveIndex));
        tests.Add(new TestCase("save bank restore rejects existing active target before mutation", RestoreRejectsExistingActiveTargetBeforeMutation));
        tests.Add(new TestCase("save bank permanently deletes active campaign", PermanentlyDeletesActiveCampaign));
        tests.Add(new TestCase("save bank permanently deletes archived campaign", PermanentlyDeletesArchivedCampaign));
        tests.Add(new TestCase("save bank archive keys reject path traversal", ArchiveKeysRejectPathTraversal));
        tests.Add(new TestCase("save bank clears deleted campaigns", ClearsDeletedCampaigns));
        tests.Add(new TestCase("save bank clear deleted rejects symlinked archive tree before mutation", ClearDeletedRejectsSymlinkedArchiveTreeBeforeMutation));
        tests.Add(new TestCase("save bank paths reject invalid campaign ids", PathsRejectInvalidCampaignIds));
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

    private static void PersistsMetadataContentsAndJson()
    {
        using var temp = new TempDirectory();
        var payload = CreatePayload(temp.Path, "vanilla.save", "run-payload");
        var createdAt = new DateTimeOffset(2026, 5, 8, 12, 0, 0, TimeSpan.Zero);
        var roster = new[] { new PlayerIdentity("steam:1", "buddy1") };

        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            roster,
            payload,
            createdAt,
            "Floor 18"));

        var json = File.ReadAllText(paths.MetadataPath(metadata.CampaignId));
        var roundTrip = JsonFile.Read<CampaignMetadata>(paths.MetadataPath(metadata.CampaignId));

        AssertEx.True(json.Contains("\"campaignId\""));
        AssertEx.True(json.Contains("\"gameMode\": \"Standard\""));
        AssertEx.Equal("buddy1", roundTrip.Label);
        AssertEx.Equal(1, roundTrip.Roster.Count);
        AssertEx.Equal("steam:1", roundTrip.Roster[0].StableId);
        AssertEx.Equal("buddy1", roundTrip.Roster[0].DisplayName);
        AssertEx.Equal(createdAt, roundTrip.CreatedAtUtc);
        AssertEx.Equal(createdAt, roundTrip.LastPlayedAtUtc);
        AssertEx.Equal(MultiplayerGameMode.Standard, roundTrip.GameMode);
        AssertEx.Equal(FileChecksum.Sha256(paths.PayloadPath(metadata.CampaignId)), roundTrip.PayloadChecksum);
        AssertEx.Equal(null, roundTrip.ActiveChecksum);
        AssertEx.Equal("Floor 18", roundTrip.ActOrFloor);
    }

    private static void RenamesCampaignWithNormalizedCustomName()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [new PlayerIdentity("steam:1", "phatstatss"), new PlayerIdentity("steam:2", "Magical Crocs")],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.Parse("2026-05-10T12:00:00Z"),
            "Floor 5"));

        var renamed = bank.RenameCampaign(metadata.CampaignId, "  Friday Poison Run  ");

        AssertEx.Equal("Friday Poison Run", renamed.CustomName);
        AssertEx.Equal("phatstatss, Magical Crocs", renamed.Label);
        var roundTrip = bank.GetCampaign(metadata.CampaignId);
        AssertEx.Equal("Friday Poison Run", roundTrip.CustomName);
        AssertEx.Equal("phatstatss, Magical Crocs", roundTrip.Label);

        var cleared = bank.RenameCampaign(metadata.CampaignId, "   ");
        AssertEx.Equal(null, cleared.CustomName);
        AssertEx.Equal("phatstatss, Magical Crocs", cleared.Label);
    }

    private static void ReadsOldMetadataWithoutCustomName()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [new PlayerIdentity("steam:1", "phatstatss")],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.Parse("2026-05-10T12:00:00Z")));

        var jsonWithoutCustomName = """
            {
              "campaignId": "__ID__",
              "gameMode": "Standard",
              "label": "phatstatss",
              "roster": [
                {
                  "stableId": "steam:1",
                  "displayName": "phatstatss",
                  "selectedCharacterId": null
                }
              ],
              "createdAtUtc": "2026-05-10T12:00:00+00:00",
              "lastPlayedAtUtc": "2026-05-10T12:00:00+00:00",
              "activeChecksum": null,
              "payloadChecksum": "payload",
              "actOrFloor": null
            }
            """.Replace("__ID__", metadata.CampaignId, StringComparison.Ordinal);
        File.WriteAllText(paths.MetadataPath(metadata.CampaignId), jsonWithoutCustomName);

        var roundTrip = bank.GetCampaign(metadata.CampaignId);

        AssertEx.Equal(null, roundTrip.CustomName);
        AssertEx.Equal("phatstatss", roundTrip.Label);
    }

    private static void WritesCreatedIdOnceInIndex()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);

        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));

        var index = JsonFile.Read<CampaignIndex>(paths.IndexPath);

        AssertEx.Equal(1, index.CampaignIds.Count);
        AssertEx.Equal(metadata.CampaignId, index.CampaignIds[0]);
    }

    private static void PayloadChecksumMatchesCopiedPayload()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);

        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));

        AssertEx.Equal(FileChecksum.Sha256(paths.PayloadPath(metadata.CampaignId)), metadata.PayloadChecksum);
    }

    private static void ListsSameModeByLastPlayedDescending()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var older = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "older.save", "older"),
            new DateTimeOffset(2026, 5, 8, 10, 0, 0, TimeSpan.Zero)));
        var newer = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "newer.save", "newer"),
            new DateTimeOffset(2026, 5, 8, 12, 0, 0, TimeSpan.Zero)));

        var campaigns = bank.ListCampaigns(MultiplayerGameMode.Standard);

        AssertEx.Equal(2, campaigns.Count);
        AssertEx.Equal(newer.CampaignId, campaigns[0].CampaignId);
        AssertEx.Equal(older.CampaignId, campaigns[1].CampaignId);
    }

    private static void IgnoresDuplicateIdsInIndex()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));
        JsonFile.Write(paths.IndexPath, new CampaignIndex([metadata.CampaignId, metadata.CampaignId]));

        var campaigns = bank.ListCampaigns(MultiplayerGameMode.Standard);
        var index = JsonFile.Read<CampaignIndex>(paths.IndexPath);

        AssertEx.Equal(1, campaigns.Count);
        AssertEx.Equal(metadata.CampaignId, campaigns[0].CampaignId);
        AssertEx.Equal(1, index.CampaignIds.Count);
    }

    private static void SkipsMissingMetadataWhileListing()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var valid = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "valid.save", "valid"),
            DateTimeOffset.UtcNow));
        var missingId = Guid.NewGuid().ToString("N");
        Directory.CreateDirectory(paths.CampaignDirectory(missingId));
        JsonFile.Write(paths.IndexPath, new CampaignIndex([valid.CampaignId, missingId]));

        var campaigns = bank.ListCampaigns(MultiplayerGameMode.Standard);

        AssertEx.Equal(1, campaigns.Count);
        AssertEx.Equal(valid.CampaignId, campaigns[0].CampaignId);
    }

    private static void SkipsMalformedMetadataWhileListing()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var valid = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "valid.save", "valid"),
            DateTimeOffset.UtcNow));
        var malformedId = Guid.NewGuid().ToString("N");
        Directory.CreateDirectory(paths.CampaignDirectory(malformedId));
        File.WriteAllText(paths.MetadataPath(malformedId), "{ not valid json");
        JsonFile.Write(paths.IndexPath, new CampaignIndex([valid.CampaignId, malformedId]));

        var campaigns = bank.ListCampaigns(MultiplayerGameMode.Standard);

        AssertEx.Equal(1, campaigns.Count);
        AssertEx.Equal(valid.CampaignId, campaigns[0].CampaignId);
    }

    private static void SkipsMismatchedMetadataWhileListing()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var valid = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "valid.save", "valid"),
            DateTimeOffset.UtcNow));
        var mismatchedId = Guid.NewGuid().ToString("N");
        Directory.CreateDirectory(paths.CampaignDirectory(mismatchedId));
        JsonFile.Write(paths.MetadataPath(mismatchedId), valid with { CampaignId = "not-a-guid" });
        JsonFile.Write(paths.IndexPath, new CampaignIndex([valid.CampaignId, mismatchedId]));

        var campaigns = bank.ListCampaigns(MultiplayerGameMode.Standard);

        AssertEx.Equal(1, campaigns.Count);
        AssertEx.Equal(valid.CampaignId, campaigns[0].CampaignId);
    }

    private static void UpdateRejectsUnindexedCampaign()
    {
        using var temp = new TempDirectory();
        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = new CampaignMetadata(
            Guid.NewGuid().ToString("N"),
            MultiplayerGameMode.Standard,
            "orphan",
            [],
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow,
            ActiveChecksum: null,
            PayloadChecksum: null,
            ActOrFloor: null);

        AssertEx.Throws<InvalidOperationException>(() => bank.UpdateMetadata(metadata));
    }

    private static void UpdateRejectsSymlinkedMetadataPathBeforeMutation()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));
        var externalMetadata = Path.Combine(temp.Path, "external-metadata.json");
        File.WriteAllText(externalMetadata, "external metadata");
        File.Delete(paths.MetadataPath(metadata.CampaignId));
        File.CreateSymbolicLink(paths.MetadataPath(metadata.CampaignId), externalMetadata);

        AssertEx.Throws<InvalidOperationException>(() => bank.UpdateMetadata(metadata with { Label = "changed" }));
        AssertEx.True((File.GetAttributes(paths.MetadataPath(metadata.CampaignId)) & FileAttributes.ReparsePoint) != 0);
        AssertEx.Equal("external metadata", File.ReadAllText(externalMetadata));
    }

    private static void ArchivesCampaignIntoDeletedArea()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));

        var archivedPath = bank.ArchiveCampaign(metadata.CampaignId, DateTimeOffset.Parse("2026-05-10T12:34:56Z"));
        var expectedArchive = Path.Combine(paths.DeletedDirectory, $"20260510123456-{metadata.CampaignId}");

        AssertEx.Equal(expectedArchive, archivedPath);
        AssertEx.Equal(0, bank.ListCampaigns(MultiplayerGameMode.Standard).Count);
        AssertEx.True(!Directory.Exists(paths.CampaignDirectory(metadata.CampaignId)));
        AssertEx.True(File.Exists(Path.Combine(archivedPath, "metadata.json")));
        AssertEx.True(File.Exists(Path.Combine(archivedPath, "multiplayer_run.save")));
        AssertEx.True(bank.HasDeletedCampaigns());
    }

    private static void ArchivesCampaignWithSuffixOnCollision()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));
        var firstArchive = Path.Combine(paths.DeletedDirectory, $"20260510123456-{metadata.CampaignId}");
        Directory.CreateDirectory(firstArchive);

        var archivedPath = bank.ArchiveCampaign(metadata.CampaignId, DateTimeOffset.Parse("2026-05-10T12:34:56Z"));

        AssertEx.Equal($"{firstArchive}-01", archivedPath);
        AssertEx.True(Directory.Exists(archivedPath));
        AssertEx.True(Directory.Exists(firstArchive));
    }

    private static void ArchiveRejectsSymlinkedCampaignTreeBeforeMutation()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));
        var externalPayload = Path.Combine(temp.Path, "external.save");
        File.WriteAllText(externalPayload, "external payload");
        File.Delete(paths.PayloadPath(metadata.CampaignId));
        File.CreateSymbolicLink(paths.PayloadPath(metadata.CampaignId), externalPayload);

        AssertEx.Throws<StoragePathSafetyException>(() =>
            bank.ArchiveCampaign(metadata.CampaignId, DateTimeOffset.Parse("2026-05-10T12:34:56Z")));
        AssertEx.Equal(metadata.CampaignId, JsonFile.Read<CampaignIndex>(paths.IndexPath).CampaignIds.Single());
        AssertEx.True(Directory.Exists(paths.CampaignDirectory(metadata.CampaignId)));
        AssertEx.True(!Directory.Exists(paths.DeletedDirectory));
        AssertEx.Equal("external payload", File.ReadAllText(externalPayload));
    }

    private static void ListsArchivedCampaignsByGameMode()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var standard = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "standard.save", "standard"),
            new DateTimeOffset(2026, 5, 8, 12, 0, 0, TimeSpan.Zero)));
        var daily = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Daily,
            [],
            CreatePayload(temp.Path, "daily.save", "daily"),
            new DateTimeOffset(2026, 5, 8, 13, 0, 0, TimeSpan.Zero)));
        var standardArchive = bank.ArchiveCampaign(standard.CampaignId, DateTimeOffset.Parse("2026-05-10T12:34:56Z"));
        bank.ArchiveCampaign(daily.CampaignId, DateTimeOffset.Parse("2026-05-10T12:34:57Z"));

        var archived = bank.ListArchivedCampaigns(MultiplayerGameMode.Standard);

        AssertEx.Equal(1, archived.Count);
        AssertEx.Equal(Path.GetFileName(standardArchive), archived[0].ArchiveKey);
        AssertEx.Equal(standard.CampaignId, archived[0].Metadata.CampaignId);
        AssertEx.Equal(MultiplayerGameMode.Standard, archived[0].Metadata.GameMode);
    }

    private static void RestoresArchivedCampaignIntoActiveIndex()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));
        var archivedPath = bank.ArchiveCampaign(metadata.CampaignId, DateTimeOffset.Parse("2026-05-10T12:34:56Z"));
        var archiveKey = Path.GetFileName(archivedPath);

        var restored = bank.RestoreArchivedCampaign(archiveKey);
        var index = JsonFile.Read<CampaignIndex>(paths.IndexPath);

        AssertEx.Equal(metadata.CampaignId, restored.CampaignId);
        AssertEx.True(Directory.Exists(paths.CampaignDirectory(metadata.CampaignId)));
        AssertEx.True(!Directory.Exists(archivedPath));
        AssertEx.Equal(metadata.CampaignId, index.CampaignIds.Single());
        AssertEx.Equal(1, bank.ListCampaigns(MultiplayerGameMode.Standard).Count);
    }

    private static void RestoreRejectsExistingActiveTargetBeforeMutation()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));
        var archivedPath = bank.ArchiveCampaign(metadata.CampaignId, DateTimeOffset.Parse("2026-05-10T12:34:56Z"));
        var archiveKey = Path.GetFileName(archivedPath);
        Directory.CreateDirectory(paths.CampaignDirectory(metadata.CampaignId));

        AssertEx.Throws<InvalidOperationException>(() => bank.RestoreArchivedCampaign(archiveKey));
        AssertEx.True(Directory.Exists(archivedPath));
        AssertEx.True(Directory.Exists(paths.CampaignDirectory(metadata.CampaignId)));
        AssertEx.Equal(0, JsonFile.Read<CampaignIndex>(paths.IndexPath).CampaignIds.Count);
    }

    private static void PermanentlyDeletesActiveCampaign()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));

        bank.DeleteCampaign(metadata.CampaignId);

        AssertEx.True(!Directory.Exists(paths.CampaignDirectory(metadata.CampaignId)));
        AssertEx.Equal(0, JsonFile.Read<CampaignIndex>(paths.IndexPath).CampaignIds.Count);
        AssertEx.Equal(0, bank.ListCampaigns(MultiplayerGameMode.Standard).Count);
    }

    private static void PermanentlyDeletesArchivedCampaign()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));
        var archivedPath = bank.ArchiveCampaign(metadata.CampaignId, DateTimeOffset.Parse("2026-05-10T12:34:56Z"));
        var archiveKey = Path.GetFileName(archivedPath);

        bank.DeleteArchivedCampaign(archiveKey);

        AssertEx.True(!Directory.Exists(archivedPath));
        AssertEx.True(Directory.Exists(paths.DeletedDirectory));
        AssertEx.False(bank.HasDeletedCampaigns());
    }

    private static void ArchiveKeysRejectPathTraversal()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);

        AssertEx.Throws<ArgumentException>(() => paths.ArchivedCampaignDirectory("../outside"));
        AssertEx.Throws<ArgumentException>(() => bank.RestoreArchivedCampaign("../outside"));
        AssertEx.Throws<ArgumentException>(() => bank.DeleteArchivedCampaign("nested/outside"));
    }

    private static void ClearsDeletedCampaigns()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));
        bank.ArchiveCampaign(metadata.CampaignId, DateTimeOffset.Parse("2026-05-10T12:34:56Z"));

        bank.ClearDeletedCampaigns();

        AssertEx.True(!Directory.Exists(paths.DeletedDirectory));
        AssertEx.True(!bank.HasDeletedCampaigns());
    }

    private static void ClearDeletedRejectsSymlinkedArchiveTreeBeforeMutation()
    {
        using var temp = new TempDirectory();
        var paths = new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves"));
        var bank = new MultiplayerSaveBank(paths);
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [],
            CreatePayload(temp.Path, "vanilla.save", "run-payload"),
            DateTimeOffset.UtcNow));
        var archivedPath = bank.ArchiveCampaign(metadata.CampaignId, DateTimeOffset.Parse("2026-05-10T12:34:56Z"));
        var externalPayload = Path.Combine(temp.Path, "external.save");
        File.WriteAllText(externalPayload, "external payload");
        File.CreateSymbolicLink(Path.Combine(archivedPath, "linked.save"), externalPayload);

        AssertEx.Throws<StoragePathSafetyException>(bank.ClearDeletedCampaigns);
        AssertEx.True(Directory.Exists(paths.DeletedDirectory));
        AssertEx.True(File.Exists(Path.Combine(archivedPath, "linked.save")));
        AssertEx.Equal("external payload", File.ReadAllText(externalPayload));
    }

    private static void PathsRejectInvalidCampaignIds()
    {
        var paths = new SaveBankPaths("MultiSaves");

        AssertEx.Throws<ArgumentException>(() => paths.CampaignDirectory("not-a-guid"));
        AssertEx.Throws<ArgumentException>(() => paths.MetadataPath("not-a-guid"));
        AssertEx.Throws<ArgumentException>(() => paths.PayloadPath("not-a-guid"));
        AssertEx.Throws<ArgumentException>(() => paths.BackupDirectory("not-a-guid"));
        AssertEx.Throws<ArgumentException>(() => paths.DeletedCampaignDirectory("not-a-guid", DateTimeOffset.UnixEpoch));
    }

    private static string CreatePayload(string directory, string fileName, string contents)
    {
        var path = Path.Combine(directory, fileName);
        File.WriteAllText(path, contents);
        return path;
    }
}
