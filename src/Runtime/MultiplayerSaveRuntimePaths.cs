namespace MultiplayerSaveSlots.Runtime;

public sealed record MultiplayerSaveRuntimePaths(
    string ActiveSavePath,
    string BankRootDirectory,
    string ActiveStatePath)
{
    public static MultiplayerSaveRuntimePaths FromSts2ActiveSavePath(
        string activeSavePath,
        Func<string, string?> getStoreFullPath,
        Func<string, string> globalizePath)
    {
        var storePath = getStoreFullPath(activeSavePath);
        var fileSystemPath = ToFileSystemPath(storePath ?? activeSavePath, globalizePath);
        return FromActiveSavePath(fileSystemPath);
    }

    public static MultiplayerSaveRuntimePaths FromActiveSavePath(string activeSavePath)
    {
        var saveDirectory = Path.GetDirectoryName(activeSavePath);
        if (string.IsNullOrWhiteSpace(saveDirectory))
            saveDirectory = Directory.GetCurrentDirectory();

        var bankRoot = Path.Combine(saveDirectory, "MultiplayerSaveSlots");
        return new MultiplayerSaveRuntimePaths(
            activeSavePath,
            bankRoot,
            Path.Combine(bankRoot, "active-state.json"));
    }

    private static string ToFileSystemPath(string path, Func<string, string> globalizePath)
    {
        if (path.Contains("://", StringComparison.Ordinal))
            return globalizePath(path);

        return path;
    }
}
