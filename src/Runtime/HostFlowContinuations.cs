using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public interface IHostFlowSaveBank
{
    IReadOnlyList<CampaignMetadata> ListCampaigns(MultiplayerGameMode gameMode);
    bool HasDeletedCampaigns();
    void ArchiveCampaign(string campaignId, DateTimeOffset deletedAtUtc);
    void ClearDeletedCampaigns();
}

public interface IActiveSaveActivator
{
    OperationResult Activate(string campaignId, DateTimeOffset nowUtc);
    OperationResult RestorePreviousActive(DateTimeOffset nowUtc);
}

public interface IActivatedCampaignMetadataRepair
{
    void RepairActivatedCampaign(string campaignId, DateTimeOffset nowUtc);
}

public interface IActiveSavePreflight
{
    OperationResult EnsureActiveSaveCanBeReplaced();
}

public interface IHostFlowContinuation
{
    OperationResult StartNewRun(MultiplayerGameMode gameMode);
    OperationResult PrepareLoadExistingRun();
    OperationResult LoadExistingRun();
}

public interface IActiveSaveSync
{
    OperationResult SyncBack(DateTimeOffset nowUtc);
    OperationResult<string> FinalizePendingNewRun(
        MultiplayerGameMode gameMode,
        CampaignMetadataSnapshot metadata,
        DateTimeOffset nowUtc);
}

public sealed class DelegateActiveSaveActivator : IActiveSaveActivator
{
    private readonly Action<string, DateTimeOffset> _activate;
    private readonly Action<DateTimeOffset> _restorePreviousActive;

    public DelegateActiveSaveActivator(
        Action<string, DateTimeOffset> activate,
        Action<DateTimeOffset>? restorePreviousActive = null)
    {
        _activate = activate;
        _restorePreviousActive = restorePreviousActive ?? (_ => { });
    }

    public OperationResult Activate(string campaignId, DateTimeOffset nowUtc)
    {
        try
        {
            _activate(campaignId, nowUtc);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    public OperationResult RestorePreviousActive(DateTimeOffset nowUtc)
    {
        try
        {
            _restorePreviousActive(nowUtc);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }
}

public sealed class NoOpActivatedCampaignMetadataRepair : IActivatedCampaignMetadataRepair
{
    public void RepairActivatedCampaign(string campaignId, DateTimeOffset nowUtc)
    {
    }
}
