namespace MultiplayerSaveSlots.Storage;

public sealed class SaveBankPaths
{
    public SaveBankPaths(string rootDirectory)
    {
        RootDirectory = Path.GetFullPath(rootDirectory);
    }

    public string RootDirectory { get; }
    public string IndexPath => Path.Combine(RootDirectory, "index.json");
    public string SavesDirectory => Path.Combine(RootDirectory, "saves");
    public string DeletedDirectory => Path.Combine(RootDirectory, "deleted");

    public string CampaignDirectory(string campaignId)
    {
        ValidateCampaignId(campaignId);
        return Path.Combine(SavesDirectory, campaignId);
    }

    public string MetadataPath(string campaignId) => Path.Combine(CampaignDirectory(campaignId), "metadata.json");
    public string PayloadPath(string campaignId) => Path.Combine(CampaignDirectory(campaignId), "multiplayer_run.save");
    public string BackupDirectory(string campaignId) => Path.Combine(CampaignDirectory(campaignId), "backup");

    public string DeletedCampaignDirectory(string campaignId, DateTimeOffset deletedAtUtc)
    {
        ValidateCampaignId(campaignId);
        return Path.Combine(DeletedDirectory, $"{deletedAtUtc.ToUniversalTime():yyyyMMddHHmmss}-{campaignId}");
    }

    internal static bool IsValidCampaignId(string campaignId) =>
        Guid.TryParseExact(campaignId, "N", out _);

    internal static void ValidateCampaignId(string campaignId)
    {
        if (!IsValidCampaignId(campaignId))
            throw new ArgumentException("Campaign id must be a 32-digit GUID string.", nameof(campaignId));
    }
}
