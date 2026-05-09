using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public interface ICampaignMetadataExtractor
{
    CampaignMetadataSnapshot CaptureActiveSaveMetadata();
}

public sealed class EmptyCampaignMetadataExtractor : ICampaignMetadataExtractor
{
    public CampaignMetadataSnapshot CaptureActiveSaveMetadata() => CampaignMetadataSnapshot.Empty;
}
