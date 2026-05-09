using System.Text.Json;
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
            ActOrFloor: request.ActOrFloor);

        JsonFile.Write(_paths.MetadataPath(campaignId), metadata);
        var index = ReadIndex();
        WriteIndex(index.CampaignIds.Concat([campaignId]));

        return metadata;
    }

    public IReadOnlyList<CampaignMetadata> ListCampaigns(MultiplayerGameMode gameMode)
    {
        EnsureCreated();
        return ReadIndex().CampaignIds
            .Select(TryReadMetadata)
            .Where(metadata => metadata is not null)
            .Cast<CampaignMetadata>()
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
        EnsureCampaignIndexed(metadata.CampaignId);

        JsonFile.Write(_paths.MetadataPath(metadata.CampaignId), metadata);
    }

    internal void EnsureCampaignIndexed(string campaignId)
    {
        EnsureCreated();
        if (!ReadIndex().CampaignIds.Contains(campaignId))
            throw new InvalidOperationException($"Campaign {campaignId} is not indexed.");
    }

    private void EnsureCreated()
    {
        Directory.CreateDirectory(_paths.RootDirectory);
        Directory.CreateDirectory(_paths.SavesDirectory);
        if (!File.Exists(_paths.IndexPath))
            JsonFile.Write(_paths.IndexPath, CampaignIndex.Empty);
    }

    private CampaignIndex ReadIndex()
    {
        if (!File.Exists(_paths.IndexPath))
            return CampaignIndex.Empty;

        var index = NormalizeIndex(JsonFile.Read<CampaignIndex>(_paths.IndexPath));
        WriteIndex(index.CampaignIds);
        return index;
    }

    private static CampaignIndex NormalizeIndex(CampaignIndex index) =>
        new(index.CampaignIds.Distinct(StringComparer.Ordinal).ToList());

    private void WriteIndex(IEnumerable<string> campaignIds) =>
        JsonFile.Write(_paths.IndexPath, new CampaignIndex(campaignIds.Distinct(StringComparer.Ordinal).ToList()));

    private CampaignMetadata? TryReadMetadata(string campaignId)
    {
        try
        {
            return JsonFile.Read<CampaignMetadata>(_paths.MetadataPath(campaignId));
        }
        catch (Exception ex) when (ex is IOException or JsonException or InvalidOperationException)
        {
            return null;
        }
    }
}
