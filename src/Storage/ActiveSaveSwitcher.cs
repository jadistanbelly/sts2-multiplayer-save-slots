using MultiplayerSaveSlots.Core;

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

        if (File.Exists(_statePath) && File.Exists(_activeSavePath))
        {
            var existingState = JsonFile.Read<ActiveSaveState>(_statePath);
            var currentActiveChecksum = FileChecksum.Sha256(_activeSavePath);
            if (currentActiveChecksum != existingState.ActiveChecksumAfterActivation)
                throw new InvalidOperationException("Active save has unsynced changes");
        }

        var activeSaveDirectory = Path.GetDirectoryName(_activeSavePath);
        if (!string.IsNullOrEmpty(activeSaveDirectory))
            Directory.CreateDirectory(activeSaveDirectory);

        string? checksumBeforeActivation = null;
        string? previousActiveBackupPath = null;
        if (File.Exists(_activeSavePath))
        {
            checksumBeforeActivation = FileChecksum.Sha256(_activeSavePath);
            previousActiveBackupPath = BackupManager.CreateBackup(_activeSavePath, _bank.GetBackupDirectory(campaignId), "before-activate-active", nowUtc);
        }

        string? previousStateBackupPath = null;
        if (File.Exists(_statePath))
            previousStateBackupPath = BackupManager.CreateBackup(_statePath, _bank.GetBackupDirectory(campaignId), "before-activate-state", nowUtc);

        File.Copy(payloadPath, _activeSavePath, overwrite: true);
        var checksum = FileChecksum.Sha256(_activeSavePath);
        JsonFile.Write(_statePath, new ActiveSaveState(
            campaignId,
            checksumBeforeActivation,
            checksum,
            nowUtc,
            previousActiveBackupPath,
            previousStateBackupPath));

        _bank.UpdateMetadata(metadata with
        {
            ActiveChecksum = checksum,
            PayloadChecksum = FileChecksum.Sha256(payloadPath),
            LastPlayedAtUtc = nowUtc
        });
    }

    public void RestorePreviousActive(DateTimeOffset nowUtc)
    {
        if (!File.Exists(_statePath))
            throw new InvalidOperationException("Cannot restore active save without active campaign state");

        var state = JsonFile.Read<ActiveSaveState>(_statePath);
        if (state.PreviousActiveBackupPath is not null && !File.Exists(state.PreviousActiveBackupPath))
            throw new FileNotFoundException("Previous active save backup is missing", state.PreviousActiveBackupPath);

        if (state.PreviousStateBackupPath is not null && !File.Exists(state.PreviousStateBackupPath))
            throw new FileNotFoundException("Previous active state backup is missing", state.PreviousStateBackupPath);

        if (File.Exists(_activeSavePath))
        {
            var currentActiveChecksum = FileChecksum.Sha256(_activeSavePath);
            if (currentActiveChecksum != state.ActiveChecksumAfterActivation)
                throw new InvalidOperationException("Active save has changed since activation and cannot be restored automatically");

            BackupManager.CreateBackup(_activeSavePath, _bank.GetBackupDirectory(state.CampaignId), "before-restore-active", nowUtc);
        }

        if (state.PreviousActiveBackupPath is null)
        {
            if (File.Exists(_activeSavePath))
                File.Delete(_activeSavePath);
        }
        else
        {
            var activeSaveDirectory = Path.GetDirectoryName(_activeSavePath);
            if (!string.IsNullOrEmpty(activeSaveDirectory))
                Directory.CreateDirectory(activeSaveDirectory);
            File.Copy(state.PreviousActiveBackupPath, _activeSavePath, overwrite: true);
        }

        if (state.PreviousStateBackupPath is null)
        {
            if (File.Exists(_statePath))
                File.Delete(_statePath);
        }
        else
        {
            var stateDirectory = Path.GetDirectoryName(_statePath);
            if (!string.IsNullOrEmpty(stateDirectory))
                Directory.CreateDirectory(stateDirectory);
            File.Copy(state.PreviousStateBackupPath, _statePath, overwrite: true);
        }
    }

    public void ClaimActiveSave(string campaignId, DateTimeOffset nowUtc)
    {
        if (!File.Exists(_activeSavePath))
            throw new FileNotFoundException("Active multiplayer save is missing", _activeSavePath);

        var payloadPath = _bank.GetPayloadPath(campaignId);
        if (!File.Exists(payloadPath))
            throw new FileNotFoundException("Campaign payload is missing", payloadPath);

        var metadata = _bank.GetCampaign(campaignId);
        _bank.EnsureCampaignIndexed(campaignId);

        var activeChecksum = FileChecksum.Sha256(_activeSavePath);
        var payloadChecksum = FileChecksum.Sha256(payloadPath);
        if (activeChecksum != payloadChecksum)
            throw new InvalidOperationException("Campaign payload does not match active save");

        string? previousStateBackupPath = null;
        if (File.Exists(_statePath))
            previousStateBackupPath = BackupManager.CreateBackup(_statePath, _bank.GetBackupDirectory(campaignId), "before-claim-state", nowUtc);

        JsonFile.Write(_statePath, new ActiveSaveState(
            campaignId,
            ActiveChecksumBeforeActivation: null,
            activeChecksum,
            nowUtc,
            PreviousActiveBackupPath: null,
            previousStateBackupPath));

        _bank.UpdateMetadata(metadata with
        {
            ActiveChecksum = activeChecksum,
            PayloadChecksum = payloadChecksum,
            LastPlayedAtUtc = nowUtc
        });
    }

    public void SyncBack(
        DateTimeOffset nowUtc,
        string? actOrFloor = null,
        IReadOnlyList<PlayerIdentity>? capturedRoster = null)
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

        var currentPayloadChecksum = FileChecksum.Sha256(payloadPath);
        if (currentPayloadChecksum != state.ActiveChecksumAfterActivation)
            throw new InvalidOperationException("Bank payload has changed since active save activation");

        var currentActiveChecksum = FileChecksum.Sha256(_activeSavePath);
        if (state.ActiveChecksumBeforeActivation is not null &&
            currentActiveChecksum == state.ActiveChecksumBeforeActivation)
        {
            throw new InvalidOperationException("Active save matches the pre-activation checksum");
        }

        BackupManager.CreateBackup(payloadPath, _bank.GetBackupDirectory(state.CampaignId), "before-sync-bank", nowUtc);
        File.Copy(_activeSavePath, payloadPath, overwrite: true);
        var syncedPayloadChecksum = FileChecksum.Sha256(payloadPath);
        JsonFile.Write(_statePath, state with { ActiveChecksumAfterActivation = syncedPayloadChecksum });
        var refreshedRoster = RefreshRoster(metadata.Roster, capturedRoster);

        _bank.UpdateMetadata(metadata with
        {
            ActiveChecksum = FileChecksum.Sha256(_activeSavePath),
            PayloadChecksum = syncedPayloadChecksum,
            LastPlayedAtUtc = nowUtc,
            ActOrFloor = actOrFloor ?? metadata.ActOrFloor,
            Roster = refreshedRoster,
            Label = CampaignLabeler.Build(refreshedRoster)
        });
    }

    private static IReadOnlyList<PlayerIdentity> RefreshRoster(
        IReadOnlyList<PlayerIdentity> existingRoster,
        IReadOnlyList<PlayerIdentity>? capturedRoster)
    {
        if (capturedRoster is null || capturedRoster.Count == 0)
            return existingRoster;

        if (existingRoster.Count == 0)
            return capturedRoster;

        if (!AllStableIds(existingRoster) || !AllStableIds(capturedRoster))
            return existingRoster;

        var capturedGroups = capturedRoster
            .GroupBy(player => player.StableId!, StringComparer.Ordinal)
            .ToList();
        if (capturedGroups.Any(group => group.Count() > 1))
            return existingRoster;

        var capturedById = capturedGroups.ToDictionary(group => group.Key, group => group.First(), StringComparer.Ordinal);
        if (capturedById.Count != existingRoster.Count ||
            !existingRoster.Select(player => player.StableId!).All(capturedById.ContainsKey))
        {
            return existingRoster;
        }

        return existingRoster
            .Select(existing => MergePlayer(existing, capturedById[existing.StableId!]))
            .ToList();
    }

    private static PlayerIdentity MergePlayer(PlayerIdentity existing, PlayerIdentity captured) =>
        existing with
        {
            DisplayName = string.IsNullOrWhiteSpace(captured.DisplayName)
                ? existing.DisplayName
                : captured.DisplayName,
            SelectedCharacterId = string.IsNullOrWhiteSpace(captured.SelectedCharacterId)
                ? existing.SelectedCharacterId
                : captured.SelectedCharacterId
        };

    private static bool AllStableIds(IReadOnlyList<PlayerIdentity> roster) =>
        roster.All(player => !string.IsNullOrWhiteSpace(player.StableId));
}
