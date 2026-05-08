namespace MultiplayerSaveSlots.Storage;

public sealed class SaveBankPaths
{
    public SaveBankPaths(string rootDirectory)
    {
        RootDirectory = rootDirectory;
    }

    public string RootDirectory { get; }
    public string IndexPath => Path.Combine(RootDirectory, "index.json");
    public string SavesDirectory => Path.Combine(RootDirectory, "saves");

    public string CampaignDirectory(string campaignId) => Path.Combine(SavesDirectory, campaignId);
    public string MetadataPath(string campaignId) => Path.Combine(CampaignDirectory(campaignId), "metadata.json");
    public string PayloadPath(string campaignId) => Path.Combine(CampaignDirectory(campaignId), "multiplayer_run.save");
    public string BackupDirectory(string campaignId) => Path.Combine(CampaignDirectory(campaignId), "backup");
}
