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
