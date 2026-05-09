using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.UI;

namespace MultiplayerSaveSlots.Runtime;

public sealed class HostFlowController
{
    private readonly IHostFlowSaveBank _bank;
    private readonly IActiveSavePreflight _preflight;
    private readonly IActiveSaveActivator _activator;
    private readonly IHostFlowContinuation _continuation;
    private readonly IActiveSaveRecovery _recovery;
    private readonly HostFlowSession _session;
    private readonly IClock _clock;
    private readonly IActivatedCampaignMetadataRepair _metadataRepair;

    public HostFlowController(
        IHostFlowSaveBank bank,
        IActiveSavePreflight preflight,
        IActiveSaveActivator activator,
        IHostFlowContinuation continuation,
        IActiveSaveRecovery recovery,
        HostFlowSession session,
        IClock clock,
        IActivatedCampaignMetadataRepair? metadataRepair = null)
    {
        _bank = bank;
        _preflight = preflight;
        _activator = activator;
        _continuation = continuation;
        _recovery = recovery;
        _session = session;
        _clock = clock;
        _metadataRepair = metadataRepair ?? new NoOpActivatedCampaignMetadataRepair();
    }

    public MultiplayerSavePickerModel BuildPickerModel(MultiplayerGameMode gameMode)
    {
        var rows = new List<MultiplayerSavePickerRow> { MultiplayerSavePickerRow.StartNew() };
        rows.AddRange(_bank.ListCampaigns(gameMode).Select(MultiplayerSavePickerRow.Campaign));

        return new MultiplayerSavePickerModel(gameMode, rows);
    }

    public ActiveSaveRecoveryModel BuildRecoveryModel(MultiplayerGameMode gameMode) =>
        _recovery.BuildRecoveryModel(gameMode);

    public OperationResult SelectStartNewRun(MultiplayerGameMode gameMode)
    {
        var preflight = _preflight.EnsureActiveSaveCanBeReplaced();
        if (!preflight.Success)
            return preflight;

        var continuation = _continuation.StartNewRun(gameMode);
        if (!continuation.Success)
            return continuation;

        _session.SelectNewRun(gameMode);
        return continuation;
    }

    public OperationResult RecoverAndSelectStartNewRun(
        ActiveSaveRecoveryActionKind action,
        MultiplayerGameMode gameMode)
    {
        var recovery = _recovery.Recover(action, gameMode, _clock.UtcNow);
        if (!recovery.Success)
            return recovery;

        if (action == ActiveSaveRecoveryActionKind.DuplicateActiveIntoCampaign)
            return recovery;

        return SelectStartNewRun(gameMode);
    }

    public OperationResult SelectExistingCampaign(string campaignId, MultiplayerGameMode gameMode)
    {
        var activePreflight = _preflight.EnsureActiveSaveCanBeReplaced();
        if (!activePreflight.Success)
            return activePreflight;

        var loadPreflight = _continuation.PrepareLoadExistingRun();
        if (!loadPreflight.Success)
            return loadPreflight;

        var activation = _activator.Activate(campaignId, _clock.UtcNow);
        if (!activation.Success)
            return activation;

        RepairActivatedCampaignMetadata(campaignId);

        var continuation = _continuation.LoadExistingRun();
        if (!continuation.Success)
        {
            var restore = _activator.RestorePreviousActive(_clock.UtcNow);
            if (!restore.Success)
                return OperationResult.Fail($"{continuation.ErrorMessage}; rollback failed: {restore.ErrorMessage}");

            return continuation;
        }

        _session.SelectExistingCampaign(campaignId, gameMode);
        return continuation;
    }

    private void RepairActivatedCampaignMetadata(string campaignId)
    {
        try
        {
            _metadataRepair.RepairActivatedCampaign(campaignId, _clock.UtcNow);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to repair activated campaign metadata: {ex.Message}");
        }
    }

    public OperationResult RecoverAndSelectExistingCampaign(
        ActiveSaveRecoveryActionKind action,
        string campaignId,
        MultiplayerGameMode gameMode)
    {
        var recovery = _recovery.Recover(action, gameMode, _clock.UtcNow);
        if (!recovery.Success)
            return recovery;

        return SelectExistingCampaign(campaignId, gameMode);
    }
}
