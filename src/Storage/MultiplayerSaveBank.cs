using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Storage;

public sealed class MultiplayerSaveBank
{
    private readonly SaveBankPaths _paths;

    public MultiplayerSaveBank(SaveBankPaths paths)
    {
        _paths = paths;
    }

    public CampaignMetadata CreateCampaign(CampaignCreateRequest request)
    {
        if (!File.Exists(request.SavePayloadPath))
            throw new FileNotFoundException("Save payload does not exist", request.SavePayloadPath);

        EnsureCreated();

        var campaignId = Guid.NewGuid().ToString("N");
        var campaignDir = _paths.CampaignDirectory(campaignId);
        Directory.CreateDirectory(campaignDir);

        var payloadPath = _paths.PayloadPath(campaignId);
        File.Copy(request.SavePayloadPath, payloadPath, overwrite: false);
        var payloadChecksum = FileChecksum.Sha256(payloadPath);

        var metadata = new CampaignMetadata(
            campaignId,
            request.GameMode,
            CampaignLabeler.Build(request.Roster),
            request.Roster,
            request.CreatedAtUtc,
            request.CreatedAtUtc,
            ActiveChecksum: null,
            PayloadChecksum: payloadChecksum,
            ActOrFloor: null);

        JsonFile.Write(_paths.MetadataPath(campaignId), metadata);
        var index = ReadIndex();
        JsonFile.Write(_paths.IndexPath, new CampaignIndex(index.CampaignIds.Concat([campaignId]).ToList()));

        return metadata;
    }

    public IReadOnlyList<CampaignMetadata> ListCampaigns(MultiplayerGameMode gameMode)
    {
        EnsureCreated();
        return ReadIndex().CampaignIds
            .Select(id => JsonFile.Read<CampaignMetadata>(_paths.MetadataPath(id)))
            .Where(metadata => metadata.GameMode == gameMode)
            .OrderByDescending(metadata => metadata.LastPlayedAtUtc)
            .ToList();
    }

    public CampaignMetadata GetCampaign(string campaignId)
    {
        EnsureCreated();
        return JsonFile.Read<CampaignMetadata>(_paths.MetadataPath(campaignId));
    }

    public string GetPayloadPath(string campaignId) => _paths.PayloadPath(campaignId);
    public string GetBackupDirectory(string campaignId) => _paths.BackupDirectory(campaignId);

    public void UpdateMetadata(CampaignMetadata metadata)
    {
        EnsureCreated();
        JsonFile.Write(_paths.MetadataPath(metadata.CampaignId), metadata);
    }

    private void EnsureCreated()
    {
        Directory.CreateDirectory(_paths.RootDirectory);
        Directory.CreateDirectory(_paths.SavesDirectory);
        if (!File.Exists(_paths.IndexPath))
            JsonFile.Write(_paths.IndexPath, CampaignIndex.Empty);
    }

    private CampaignIndex ReadIndex() => File.Exists(_paths.IndexPath)
        ? JsonFile.Read<CampaignIndex>(_paths.IndexPath)
        : CampaignIndex.Empty;
}
