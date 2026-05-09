using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public sealed class HostFlowSession
{
    public string? SelectedCampaignId { get; private set; }
    public MultiplayerGameMode? SelectedGameMode { get; private set; }
    public bool IsPendingNewRun { get; private set; }
    public CampaignMetadataSnapshot PendingNewRunMetadata { get; private set; } = CampaignMetadataSnapshot.Empty;
    private string? _acknowledgedCompatibilityWarningKey;

    public void SelectExistingCampaign(string campaignId, MultiplayerGameMode gameMode)
    {
        SelectedCampaignId = campaignId;
        SelectedGameMode = gameMode;
        IsPendingNewRun = false;
        PendingNewRunMetadata = CampaignMetadataSnapshot.Empty;
        _acknowledgedCompatibilityWarningKey = null;
    }

    public void SelectNewRun(MultiplayerGameMode gameMode)
    {
        SelectedCampaignId = null;
        SelectedGameMode = gameMode;
        IsPendingNewRun = true;
        PendingNewRunMetadata = CampaignMetadataSnapshot.Empty;
        _acknowledgedCompatibilityWarningKey = null;
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
        _acknowledgedCompatibilityWarningKey = null;
    }

    public bool HasAcknowledgedCompatibilityWarning(string warningKey) =>
        _acknowledgedCompatibilityWarningKey == warningKey;

    public void AcknowledgeCompatibilityWarning(string warningKey)
    {
        _acknowledgedCompatibilityWarningKey = warningKey;
    }
}
