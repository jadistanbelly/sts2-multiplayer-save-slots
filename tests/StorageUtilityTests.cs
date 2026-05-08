using MultiplayerSaveSlots.Storage;

namespace MultiplayerSaveSlots.Tests;

public static class StorageUtilityTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("checksum changes with file content", ChecksumChanges));
        tests.Add(new TestCase("backup copies source file", BackupCopiesSource));
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
}
