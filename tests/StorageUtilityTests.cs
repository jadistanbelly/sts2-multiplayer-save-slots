using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Storage;

namespace MultiplayerSaveSlots.Tests;

public static class StorageUtilityTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("checksum changes with file content", ChecksumChanges));
        tests.Add(new TestCase("checksum matches known sha256 vector", ChecksumKnownVector));
        tests.Add(new TestCase("backup copies source file", BackupCopiesSource));
        tests.Add(new TestCase("backup retries deterministic suffix on collision", BackupRetriesSuffixOnCollision));
        tests.Add(new TestCase("json file round trips campaign metadata", JsonFileRoundTripsCampaignMetadata));
        tests.Add(new TestCase("json file overwrites without temp file remaining", JsonFileOverwritesWithoutTempFile));
        tests.Add(new TestCase("json file writes bare relative filename", JsonFileWritesBareRelativeFilename));
    }

    private static void ChecksumChanges()
    {
        using var temp = new TempDirectory();
        var file = Path.Combine(temp.Path, "save.dat");

        File.WriteAllText(file, "first");
        var first = FileChecksum.Sha256(file);

        File.WriteAllText(file, "second");
        var second = FileChecksum.Sha256(file);

        AssertEx.False(first == second);
    }

    private static void ChecksumKnownVector()
    {
        using var temp = new TempDirectory();
        var file = Path.Combine(temp.Path, "abc.dat");
        File.WriteAllText(file, "abc");

        AssertEx.Equal("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", FileChecksum.Sha256(file));
    }

    private static void BackupCopiesSource()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "active.save");
        var backupDir = Path.Combine(temp.Path, "backup");
        File.WriteAllText(source, "payload");

        var backup = BackupManager.CreateBackup(source, backupDir, "activate", new DateTimeOffset(2026, 5, 8, 12, 30, 0, TimeSpan.Zero));

        AssertEx.True(File.Exists(backup));
        AssertEx.Equal("payload", File.ReadAllText(backup));
        AssertEx.True(Path.GetFileName(backup).Contains("activate"));
    }

    private static void BackupRetriesSuffixOnCollision()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "active.save");
        var backupDir = Path.Combine(temp.Path, "backup");
        var timestamp = new DateTimeOffset(2026, 5, 8, 12, 30, 0, TimeSpan.Zero);
        File.WriteAllText(source, "payload");

        var first = BackupManager.CreateBackup(source, backupDir, "activate", timestamp);
        var second = BackupManager.CreateBackup(source, backupDir, "activate", timestamp);

        AssertEx.True(File.Exists(first));
        AssertEx.True(File.Exists(second));
        AssertEx.False(first == second);
        AssertEx.Equal("20260508-123000-activate-active.save", Path.GetFileName(first));
        AssertEx.Equal("20260508-123000-activate-1-active.save", Path.GetFileName(second));
    }

    private static void JsonFileRoundTripsCampaignMetadata()
    {
        using var temp = new TempDirectory();
        var path = Path.Combine(temp.Path, "campaign.json");
        var metadata = new CampaignMetadata(
            "campaign-1",
            MultiplayerGameMode.Custom,
            "Run Label",
            [new PlayerIdentity("steam-1", "Ayla")],
            new DateTimeOffset(2026, 5, 8, 12, 30, 0, TimeSpan.Zero),
            new DateTimeOffset(2026, 5, 9, 13, 45, 0, TimeSpan.Zero),
            null,
            "payload-checksum",
            "Act 2");

        JsonFile.Write(path, metadata);
        var json = File.ReadAllText(path);
        var roundTrip = JsonFile.Read<CampaignMetadata>(path);

        AssertEx.True(json.Contains("\"campaignId\""));
        AssertEx.True(json.Contains("\"gameMode\": \"Custom\""));
        AssertEx.Equal(metadata.CampaignId, roundTrip.CampaignId);
        AssertEx.Equal(metadata.GameMode, roundTrip.GameMode);
        AssertEx.Equal(metadata.Label, roundTrip.Label);
        AssertEx.Equal(metadata.Roster.Count, roundTrip.Roster.Count);
        AssertEx.Equal(metadata.Roster[0].StableId, roundTrip.Roster[0].StableId);
        AssertEx.Equal(metadata.Roster[0].DisplayName, roundTrip.Roster[0].DisplayName);
        AssertEx.Equal(metadata.CreatedAtUtc, roundTrip.CreatedAtUtc);
        AssertEx.Equal(metadata.LastPlayedAtUtc, roundTrip.LastPlayedAtUtc);
        AssertEx.Equal(metadata.ActiveChecksum, roundTrip.ActiveChecksum);
        AssertEx.Equal(metadata.PayloadChecksum, roundTrip.PayloadChecksum);
        AssertEx.Equal(metadata.ActOrFloor, roundTrip.ActOrFloor);
    }

    private static void JsonFileOverwritesWithoutTempFile()
    {
        using var temp = new TempDirectory();
        var path = Path.Combine(temp.Path, "campaign.json");

        JsonFile.Write(path, new CampaignMetadata("first", MultiplayerGameMode.Standard, "First", [], DateTimeOffset.UnixEpoch, DateTimeOffset.UnixEpoch, "active", null, null));
        JsonFile.Write(path, new CampaignMetadata("second", MultiplayerGameMode.Daily, "Second", [], DateTimeOffset.UnixEpoch, DateTimeOffset.UnixEpoch, null, "payload", "Floor 3"));

        var roundTrip = JsonFile.Read<CampaignMetadata>(path);
        AssertEx.Equal("second", roundTrip.CampaignId);
        AssertEx.False(File.Exists(path + ".tmp"));
    }

    private static void JsonFileWritesBareRelativeFilename()
    {
        using var temp = new TempDirectory();
        var previousDirectory = Directory.GetCurrentDirectory();

        try
        {
            Directory.SetCurrentDirectory(temp.Path);

            JsonFile.Write("campaign.json", new CampaignMetadata("bare", MultiplayerGameMode.Custom, "Bare", [], DateTimeOffset.UnixEpoch, DateTimeOffset.UnixEpoch, null, "payload", "Act 1"));

            AssertEx.True(File.Exists(Path.Combine(temp.Path, "campaign.json")));
            AssertEx.False(File.Exists(Path.Combine(temp.Path, "campaign.json.tmp")));
            AssertEx.Equal("bare", JsonFile.Read<CampaignMetadata>("campaign.json").CampaignId);
        }
        finally
        {
            Directory.SetCurrentDirectory(previousDirectory);
        }
    }
}
