using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public sealed class SaveSyncController
{
    private readonly IActiveSaveSync _activeSaveSync;
    private readonly HostFlowSession _session;
    private readonly IClock _clock;

    public SaveSyncController(IActiveSaveSync activeSaveSync, HostFlowSession session, IClock clock)
    {
        _activeSaveSync = activeSaveSync;
        _session = session;
        _clock = clock;
    }

    public OperationResult SyncAfterVanillaSave()
    {
        try
        {
            if (_session.SelectedGameMode is null)
                return OperationResult.Ok();

            if (_session.IsPendingNewRun)
                return FinalizePendingNewRun(_session.SelectedGameMode.Value);

            if (_session.SelectedCampaignId is null)
                return OperationResult.Ok();

            return _activeSaveSync.SyncBack(_clock.UtcNow);
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    private OperationResult FinalizePendingNewRun(MultiplayerGameMode gameMode)
    {
        var finalized = _activeSaveSync.FinalizePendingNewRun(gameMode, _clock.UtcNow);
        if (!finalized.Success || finalized.Value is null)
            return OperationResult.Fail(finalized.ErrorMessage ?? "Unable to finalize pending multiplayer save.");

        _session.SelectExistingCampaign(finalized.Value, gameMode);
        return OperationResult.Ok();
    }
}
