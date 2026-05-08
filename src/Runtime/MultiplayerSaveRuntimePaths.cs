namespace MultiplayerSaveSlots.Runtime;

public sealed record MultiplayerSaveRuntimePaths(
    string ActiveSavePath,
    string BankRootDirectory,
    string ActiveStatePath)
{
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
}
