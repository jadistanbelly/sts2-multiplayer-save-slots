namespace MultiplayerSaveSlots.Storage;

public static class BackupManager
{
    public static string CreateBackup(string sourcePath, string backupDirectory, string reason, DateTimeOffset timestampUtc)
    {
        if (!File.Exists(sourcePath))
            throw new FileNotFoundException("Cannot back up missing file", sourcePath);

        Directory.CreateDirectory(backupDirectory);
        var safeReason = string.Concat(reason.Select(ch => char.IsLetterOrDigit(ch) ? ch : '-'));
        var fileName = $"{timestampUtc:yyyyMMdd-HHmmss}-{safeReason}-{Path.GetFileName(sourcePath)}";
        var destination = Path.Combine(backupDirectory, fileName);
        File.Copy(sourcePath, destination, overwrite: false);
        return destination;
    }
}
