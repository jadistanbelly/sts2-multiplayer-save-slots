using MultiplayerSaveSlots.Core;

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
