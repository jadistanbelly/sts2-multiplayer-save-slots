using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public sealed class HostFlowSession
{
    public string? SelectedCampaignId { get; private set; }
    public MultiplayerGameMode? SelectedGameMode { get; private set; }
    public bool IsPendingNewRun { get; private set; }
    public CampaignMetadataSnapshot PendingNewRunMetadata { get; private set; } = CampaignMetadataSnapshot.Empty;

    public void SelectExistingCampaign(string campaignId, MultiplayerGameMode gameMode)
    {
        SelectedCampaignId = campaignId;
        SelectedGameMode = gameMode;
        IsPendingNewRun = false;
        PendingNewRunMetadata = CampaignMetadataSnapshot.Empty;
    }

    public void SelectNewRun(MultiplayerGameMode gameMode)
    {
        SelectedCampaignId = null;
        SelectedGameMode = gameMode;
        IsPendingNewRun = true;
        PendingNewRunMetadata = CampaignMetadataSnapshot.Empty;
    }

    public void CapturePendingNewRunMetadata(CampaignMetadataSnapshot metadata)
    {
        if (IsPendingNewRun)
            PendingNewRunMetadata = metadata;
    }

    public void Clear()
    {
        SelectedCampaignId = null;
        SelectedGameMode = null;
        IsPendingNewRun = false;
        PendingNewRunMetadata = CampaignMetadataSnapshot.Empty;
    }
}
