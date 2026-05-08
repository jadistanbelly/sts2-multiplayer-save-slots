namespace MultiplayerSaveSlots.Storage;

public static class BackupManager
{
    public static string CreateBackup(string sourcePath, string backupDirectory, string reason, DateTimeOffset timestampUtc)
    {
        if (!File.Exists(sourcePath))
            throw new FileNotFoundException("Cannot back up missing file", sourcePath);

        Directory.CreateDirectory(backupDirectory);
        var safeReason = string.Concat(reason.Select(ch => char.IsLetterOrDigit(ch) ? ch : '-'));
        var destination = NextBackupPath(backupDirectory, timestampUtc, safeReason, Path.GetFileName(sourcePath));
        File.Copy(sourcePath, destination, overwrite: false);
        return destination;
    }

    private static string NextBackupPath(string backupDirectory, DateTimeOffset timestampUtc, string safeReason, string sourceFileName)
    {
        var prefix = $"{timestampUtc:yyyyMMdd-HHmmss}-{safeReason}";

        for (var attempt = 0; ; attempt++)
        {
            var fileName = attempt == 0
                ? $"{prefix}-{sourceFileName}"
                : $"{prefix}-{attempt}-{sourceFileName}";
            var destination = Path.Combine(backupDirectory, fileName);

            if (!File.Exists(destination))
                return destination;
        }
    }
}
