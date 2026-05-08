using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public interface IHostFlowSaveBank
{
    IReadOnlyList<CampaignMetadata> ListCampaigns(MultiplayerGameMode gameMode);
}

public interface IActiveSaveActivator
{
    OperationResult Activate(string campaignId, DateTimeOffset nowUtc);
}

public interface IHostFlowContinuation
{
    OperationResult StartNewRun(MultiplayerGameMode gameMode);
    OperationResult LoadExistingRun();
}

public sealed class DelegateActiveSaveActivator : IActiveSaveActivator
{
    private readonly Action<string, DateTimeOffset> _activate;

    public DelegateActiveSaveActivator(Action<string, DateTimeOffset> activate)
    {
        _activate = activate;
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
}
