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

    public string RootDirectory => _paths.RootDirectory;

    public CampaignMetadata CreateCampaign(CampaignCreateRequest request)
    {
        StoragePathGuard.EnsureSafeFilePath(request.SavePayloadPath, "save payload");
        if (!File.Exists(request.SavePayloadPath))
            throw new FileNotFoundException("Save payload does not exist", request.SavePayloadPath);

        EnsureCreated();

        var campaignId = Guid.NewGuid().ToString("N");
        var campaignDir = _paths.CampaignDirectory(campaignId);
        StoragePathGuard.EnsurePathInsideDirectory(campaignDir, _paths.SavesDirectory, "campaign directory");
        StoragePathGuard.EnsureSafeDirectoryPath(campaignDir, "campaign directory");
        Directory.CreateDirectory(campaignDir);

        var payloadPath = _paths.PayloadPath(campaignId);
        StoragePathGuard.EnsureSafeFilePath(payloadPath, "campaign payload");
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

        StoragePathGuard.EnsureSafeFilePath(_paths.MetadataPath(campaignId), "campaign metadata");
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
        var metadataPath = _paths.MetadataPath(campaignId);
        StoragePathGuard.EnsureSafeFilePath(metadataPath, "campaign metadata");
        var metadata = JsonFile.Read<CampaignMetadata>(metadataPath);
        if (!string.Equals(metadata.CampaignId, campaignId, StringComparison.Ordinal))
            throw new InvalidOperationException("Campaign metadata id does not match requested campaign.");

        return metadata;
    }

    public string GetPayloadPath(string campaignId) => _paths.PayloadPath(campaignId);
    public string GetBackupDirectory(string campaignId) => _paths.BackupDirectory(campaignId);

    public void UpdateMetadata(CampaignMetadata metadata)
    {
        EnsureCampaignIndexed(metadata.CampaignId);

        StoragePathGuard.EnsureSafeFilePath(_paths.MetadataPath(metadata.CampaignId), "campaign metadata");
        JsonFile.Write(_paths.MetadataPath(metadata.CampaignId), metadata);
    }

    internal void EnsureCampaignIndexed(string campaignId)
    {
        SaveBankPaths.ValidateCampaignId(campaignId);
        EnsureCreated();
        if (!ReadIndex().CampaignIds.Contains(campaignId))
            throw new InvalidOperationException($"Campaign {campaignId} is not indexed.");
    }

    public void EnsureStorageSafe() =>
        StoragePathGuard.EnsureSafeTree(_paths.RootDirectory, "save bank");

    private void EnsureCreated()
    {
        EnsureStorageSafe();
        Directory.CreateDirectory(_paths.RootDirectory);
        StoragePathGuard.EnsureSafeDirectoryPath(_paths.SavesDirectory, "save bank saves directory");
        Directory.CreateDirectory(_paths.SavesDirectory);
        StoragePathGuard.EnsureSafeFilePath(_paths.IndexPath, "save bank index");
        if (!File.Exists(_paths.IndexPath))
            JsonFile.Write(_paths.IndexPath, CampaignIndex.Empty);
    }

    private CampaignIndex ReadIndex()
    {
        StoragePathGuard.EnsureSafeFilePath(_paths.IndexPath, "save bank index");
        if (!File.Exists(_paths.IndexPath))
            return CampaignIndex.Empty;

        var index = NormalizeIndex(JsonFile.Read<CampaignIndex>(_paths.IndexPath));
        WriteIndex(index.CampaignIds);
        return index;
    }

    private static CampaignIndex NormalizeIndex(CampaignIndex index) =>
        new(index.CampaignIds.Distinct(StringComparer.Ordinal).ToList());

    private void WriteIndex(IEnumerable<string> campaignIds) =>
        WriteIndexFile(new CampaignIndex(campaignIds.Distinct(StringComparer.Ordinal).ToList()));

    private void WriteIndexFile(CampaignIndex index)
    {
        StoragePathGuard.EnsureSafeFilePath(_paths.IndexPath, "save bank index");
        JsonFile.Write(_paths.IndexPath, index);
    }

    private CampaignMetadata? TryReadMetadata(string campaignId)
    {
        if (!SaveBankPaths.IsValidCampaignId(campaignId))
            return null;

        try
        {
            var metadataPath = _paths.MetadataPath(campaignId);
            StoragePathGuard.EnsureSafeFilePath(metadataPath, "campaign metadata");
            var metadata = JsonFile.Read<CampaignMetadata>(metadataPath);
            return string.Equals(metadata.CampaignId, campaignId, StringComparison.Ordinal) &&
                SaveBankPaths.IsValidCampaignId(metadata.CampaignId)
                    ? metadata
                    : null;
        }
        catch (Exception ex) when (ex is IOException or JsonException or InvalidOperationException or ArgumentException)
        {
            return null;
        }
    }
}
