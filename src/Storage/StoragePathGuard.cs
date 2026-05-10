namespace MultiplayerSaveSlots.Storage;

public sealed class StoragePathSafetyException : InvalidOperationException
{
    public StoragePathSafetyException(string message) : base(message)
    {
    }
}

public static class StoragePathGuard
{
    public static void EnsureSafeFilePath(string path, string description) =>
        EnsureNoReparsePointInExistingPath(path, description);

    public static void EnsureSafeDirectoryPath(string path, string description) =>
        EnsureNoReparsePointInExistingPath(path, description);

    public static void EnsureSafeTree(string rootDirectory, string description)
    {
        EnsureSafeDirectoryPath(rootDirectory, description);
        if (!Directory.Exists(rootDirectory))
            return;

        RejectReparsePointsInTree(Path.GetFullPath(rootDirectory), description);
    }

    public static void EnsurePathInsideDirectory(string path, string rootDirectory, string description)
    {
        var fullPath = Path.GetFullPath(path);
        var fullRoot = Path.GetFullPath(rootDirectory);
        var comparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        if (string.Equals(
            Path.TrimEndingDirectorySeparator(fullPath),
            Path.TrimEndingDirectorySeparator(fullRoot),
            comparison))
        {
            return;
        }

        var rootWithSeparator = EnsureTrailingDirectorySeparator(fullRoot);
        if (!fullPath.StartsWith(rootWithSeparator, comparison))
            throw new StoragePathSafetyException($"{description} must stay inside {fullRoot}: {fullPath}");
    }

    private static void EnsureNoReparsePointInExistingPath(string path, string description)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException($"{description} path is required.", nameof(path));

        var fullPath = Path.GetFullPath(path);
        var root = Path.GetPathRoot(fullPath);
        if (string.IsNullOrEmpty(root))
            return;

        var current = root;
        var relative = Path.GetRelativePath(root, fullPath);
        if (relative == ".")
            return;

        foreach (var part in relative.Split(
            [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
            StringSplitOptions.RemoveEmptyEntries))
        {
            current = Path.Combine(current, part);
            if (!TryGetAttributes(current, out var attributes))
                break;

            if ((attributes & FileAttributes.ReparsePoint) != 0)
                throw new StoragePathSafetyException($"{description} must not contain symlinks or reparse points: {current}");
        }
    }

    private static void RejectReparsePointsInTree(string directory, string description)
    {
        foreach (var entry in Directory.EnumerateFileSystemEntries(directory))
        {
            if (!TryGetAttributes(entry, out var attributes))
                continue;

            if ((attributes & FileAttributes.ReparsePoint) != 0)
                throw new StoragePathSafetyException($"{description} must not contain symlinks or reparse points: {entry}");

            if ((attributes & FileAttributes.Directory) != 0)
                RejectReparsePointsInTree(entry, description);
        }
    }

    private static bool TryGetAttributes(string path, out FileAttributes attributes)
    {
        try
        {
            attributes = File.GetAttributes(path);
            return true;
        }
        catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
        {
            attributes = default;
            return false;
        }
    }

    private static string EnsureTrailingDirectorySeparator(string path)
    {
        var trimmed = Path.TrimEndingDirectorySeparator(path);
        return trimmed.EndsWith(Path.DirectorySeparatorChar)
            ? trimmed
            : $"{trimmed}{Path.DirectorySeparatorChar}";
    }
}
