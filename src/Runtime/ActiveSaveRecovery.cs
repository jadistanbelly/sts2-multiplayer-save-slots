using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Storage;
using System.Text.Json;

namespace MultiplayerSaveSlots.Runtime;

public enum ActiveSaveRecoveryActionKind
{
    SyncActiveToCampaign,
    DuplicateActiveIntoCampaign
}

public sealed record ActiveSaveRecoveryOption(
    ActiveSaveRecoveryActionKind Kind,
    string Label,
    string Description);

public sealed record ActiveSaveRecoveryModel(
    string Title,
    string Message,
    IReadOnlyList<ActiveSaveRecoveryOption> Options)
{
    public bool HasOptions => Options.Count > 0;

    public static ActiveSaveRecoveryModel None() =>
        new("Multiplayer Save Slots", "No recovery action is available.", []);
}

public interface IActiveSaveRecovery
{
    ActiveSaveRecoveryModel BuildRecoveryModel(MultiplayerGameMode gameMode);
    OperationResult Recover(ActiveSaveRecoveryActionKind action, MultiplayerGameMode gameMode, DateTimeOffset nowUtc);
}

public sealed class NoActiveSaveRecovery : IActiveSaveRecovery
{
    public ActiveSaveRecoveryModel BuildRecoveryModel(MultiplayerGameMode gameMode) => ActiveSaveRecoveryModel.None();

    public OperationResult Recover(ActiveSaveRecoveryActionKind action, MultiplayerGameMode gameMode, DateTimeOffset nowUtc) =>
        OperationResult.Fail("No recovery action is available.");
}

public sealed class ActiveSaveRecoveryService : IActiveSaveRecovery
{
    private readonly MultiplayerSaveBank _bank;
    private readonly ActiveSaveSwitcher _switcher;
    private readonly string _activeSavePath;
    private readonly string _statePath;
    private readonly ICampaignMetadataExtractor _metadataExtractor;

    public ActiveSaveRecoveryService(
        MultiplayerSaveBank bank,
        ActiveSaveSwitcher switcher,
        string activeSavePath,
        string statePath,
        ICampaignMetadataExtractor? metadataExtractor = null)
    {
        _bank = bank;
        _switcher = switcher;
        _activeSavePath = activeSavePath;
        _statePath = statePath;
        _metadataExtractor = metadataExtractor ?? new EmptyCampaignMetadataExtractor();
    }

    public ActiveSaveRecoveryModel BuildRecoveryModel(MultiplayerGameMode gameMode)
    {
        try
        {
            EnsureInspectionPathsSafe();
            if (!File.Exists(_activeSavePath))
                return ActiveSaveRecoveryModel.None();

            if (!File.Exists(_statePath))
                return DuplicateModel("Current multiplayer save is not managed by Multiplayer Save Slots yet.");

            var state = JsonFile.Read<ActiveSaveState>(_statePath);
            var activeChecksum = FileChecksum.Sha256(_activeSavePath);
            if (activeChecksum == state.ActiveChecksumAfterActivation)
                return ActiveSaveRecoveryModel.None();

            if (!CanSyncToStoredCampaign(state))
                return DuplicateModel("The campaign for the active save no longer exists. Duplicate the active save before switching slots.");

            return new ActiveSaveRecoveryModel(
                "Active multiplayer save has unsynced changes",
                "Sync the active save back to its selected campaign before switching slots.",
                [
                    new ActiveSaveRecoveryOption(
                        ActiveSaveRecoveryActionKind.SyncActiveToCampaign,
                        "Sync Active Save",
                        "Back up the campaign payload and copy the active save into the selected campaign.")
                ]);
        }
        catch (StoragePathSafetyException ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Active save path safety check failed: {ex.Message}");
            return ActiveSaveRecoveryModel.None();
        }
        catch (Exception ex) when (ex is IOException or JsonException or InvalidOperationException)
        {
            return DuplicateModel($"Active save state cannot be verified: {ex.Message}");
        }
    }

    private bool CanSyncToStoredCampaign(ActiveSaveState state)
    {
        try
        {
            var payloadPath = _bank.GetPayloadPath(state.CampaignId);
            if (!File.Exists(payloadPath))
                return false;

            _bank.GetCampaign(state.CampaignId);
            _bank.EnsureCampaignIndexed(state.CampaignId);
            return true;
        }
        catch (Exception ex) when (ex is IOException or JsonException or InvalidOperationException or ArgumentException)
        {
            return false;
        }
    }

    public OperationResult Recover(ActiveSaveRecoveryActionKind action, MultiplayerGameMode gameMode, DateTimeOffset nowUtc)
    {
        try
        {
            if (action == ActiveSaveRecoveryActionKind.SyncActiveToCampaign)
            {
                _switcher.SyncBack(nowUtc);
                return OperationResult.Ok();
            }

            StoragePathGuard.EnsureSafeFilePath(_activeSavePath, "active save path");
            _switcher.EnsureCanUseRuntimePaths();
            if (!File.Exists(_activeSavePath))
                return OperationResult.Fail("Active multiplayer save is missing");

            var snapshot = CaptureMetadataOrEmpty();
            var metadata = _bank.CreateCampaign(new CampaignCreateRequest(
                gameMode,
                snapshot.Roster,
                _activeSavePath,
                nowUtc,
                snapshot.ActOrFloor));
            _switcher.ClaimActiveSave(metadata.CampaignId, nowUtc);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    private static ActiveSaveRecoveryModel DuplicateModel(string message) =>
        new(
            "Active multiplayer save needs attention",
            message,
            [
                new ActiveSaveRecoveryOption(
                    ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign,
                    "Duplicate Active Save",
                    "Copy the current active save into the Multiplayer Save Slots bank before continuing.")
            ]);

    private void EnsureInspectionPathsSafe()
    {
        StoragePathGuard.EnsureSafeFilePath(_activeSavePath, "active save path");
        StoragePathGuard.EnsureSafeFilePath(_statePath, "active save state path");
        _bank.EnsureStorageSafe();
    }

    private CampaignMetadataSnapshot CaptureMetadataOrEmpty()
    {
        try
        {
            return _metadataExtractor.CaptureActiveSaveMetadata();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to capture active save metadata: {ex.Message}");
            return CampaignMetadataSnapshot.Empty;
        }
    }
}
