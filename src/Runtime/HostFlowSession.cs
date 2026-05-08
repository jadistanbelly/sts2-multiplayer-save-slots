using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public sealed class HostFlowSession
{
    public string? SelectedCampaignId { get; private set; }
    public MultiplayerGameMode? SelectedGameMode { get; private set; }
    public bool IsPendingNewRun { get; private set; }

    public void SelectExistingCampaign(string campaignId, MultiplayerGameMode gameMode)
    {
        SelectedCampaignId = campaignId;
        SelectedGameMode = gameMode;
        IsPendingNewRun = false;
    }

    public void SelectNewRun(MultiplayerGameMode gameMode)
    {
        SelectedCampaignId = null;
        SelectedGameMode = gameMode;
        IsPendingNewRun = true;
    }

    public void Clear()
    {
        SelectedCampaignId = null;
        SelectedGameMode = null;
        IsPendingNewRun = false;
    }
}
