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

    public ActiveSaveRecoveryService(
        MultiplayerSaveBank bank,
        ActiveSaveSwitcher switcher,
        string activeSavePath,
        string statePath)
    {
        _bank = bank;
        _switcher = switcher;
        _activeSavePath = activeSavePath;
        _statePath = statePath;
    }

    public ActiveSaveRecoveryModel BuildRecoveryModel(MultiplayerGameMode gameMode)
    {
        if (!File.Exists(_activeSavePath))
            return ActiveSaveRecoveryModel.None();

        if (!File.Exists(_statePath))
            return DuplicateModel("Current multiplayer save is not managed by Multiplayer Save Slots yet.");

        try
        {
            var state = JsonFile.Read<ActiveSaveState>(_statePath);
            var activeChecksum = FileChecksum.Sha256(_activeSavePath);
            if (activeChecksum == state.ActiveChecksumAfterActivation)
                return ActiveSaveRecoveryModel.None();

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
        catch (Exception ex) when (ex is IOException or JsonException or InvalidOperationException)
        {
            return DuplicateModel($"Active save state cannot be verified: {ex.Message}");
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

            if (!File.Exists(_activeSavePath))
                return OperationResult.Fail("Active multiplayer save is missing");

            var metadata = _bank.CreateCampaign(new CampaignCreateRequest(gameMode, [], _activeSavePath, nowUtc));
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
}
