namespace MultiplayerSaveSlots.Storage;

public sealed class ActiveSaveSwitcher
{
    private readonly MultiplayerSaveBank _bank;
    private readonly string _activeSavePath;
    private readonly string _statePath;

    public ActiveSaveSwitcher(MultiplayerSaveBank bank, string activeSavePath, string statePath)
    {
        _bank = bank;
        _activeSavePath = activeSavePath;
        _statePath = statePath;
    }

    public void Activate(string campaignId, DateTimeOffset nowUtc)
    {
        var payloadPath = _bank.GetPayloadPath(campaignId);
        if (!File.Exists(payloadPath))
            throw new FileNotFoundException("Campaign payload is missing", payloadPath);

        var metadata = _bank.GetCampaign(campaignId);
        _bank.EnsureCampaignIndexed(campaignId);

        var activeSaveDirectory = Path.GetDirectoryName(_activeSavePath);
        if (!string.IsNullOrEmpty(activeSaveDirectory))
            Directory.CreateDirectory(activeSaveDirectory);

        string? checksumBeforeActivation = null;
        if (File.Exists(_activeSavePath))
        {
            checksumBeforeActivation = FileChecksum.Sha256(_activeSavePath);
            BackupManager.CreateBackup(_activeSavePath, _bank.GetBackupDirectory(campaignId), "before-activate-active", nowUtc);
        }

        File.Copy(payloadPath, _activeSavePath, overwrite: true);
        var checksum = FileChecksum.Sha256(_activeSavePath);
        JsonFile.Write(_statePath, new ActiveSaveState(campaignId, checksumBeforeActivation, checksum, nowUtc));

        _bank.UpdateMetadata(metadata with
        {
            ActiveChecksum = checksum,
            PayloadChecksum = FileChecksum.Sha256(payloadPath),
            LastPlayedAtUtc = nowUtc
        });
    }

    public void SyncBack(DateTimeOffset nowUtc)
    {
        if (!File.Exists(_statePath))
            throw new InvalidOperationException("Cannot sync active save without active campaign state");

        if (!File.Exists(_activeSavePath))
            throw new FileNotFoundException("Active multiplayer save is missing", _activeSavePath);

        var state = JsonFile.Read<ActiveSaveState>(_statePath);
        var payloadPath = _bank.GetPayloadPath(state.CampaignId);
        if (!File.Exists(payloadPath))
            throw new FileNotFoundException("Campaign payload is missing", payloadPath);

        var metadata = _bank.GetCampaign(state.CampaignId);
        _bank.EnsureCampaignIndexed(state.CampaignId);

        BackupManager.CreateBackup(payloadPath, _bank.GetBackupDirectory(state.CampaignId), "before-sync-bank", nowUtc);
        File.Copy(_activeSavePath, payloadPath, overwrite: true);

        _bank.UpdateMetadata(metadata with
        {
            ActiveChecksum = FileChecksum.Sha256(_activeSavePath),
            PayloadChecksum = FileChecksum.Sha256(payloadPath),
            LastPlayedAtUtc = nowUtc
        });
    }
}
