using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Storage;

namespace MultiplayerSaveSlots.Runtime;

public sealed class ActivatedCampaignMetadataRepair : IActivatedCampaignMetadataRepair
{
    private readonly MultiplayerSaveBank _bank;
    private readonly ICampaignMetadataExtractor _metadataExtractor;

    public ActivatedCampaignMetadataRepair(
        MultiplayerSaveBank bank,
        ICampaignMetadataExtractor metadataExtractor)
    {
        _bank = bank;
        _metadataExtractor = metadataExtractor;
    }

    public void RepairActivatedCampaign(string campaignId, DateTimeOffset nowUtc)
    {
        try
        {
            Repair(campaignId);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to repair activated campaign metadata: {ex.Message}");
        }
    }

    private void Repair(string campaignId)
    {
        var metadata = _bank.GetCampaign(campaignId);
        var snapshot = _metadataExtractor.CaptureActiveSaveMetadata();

        var repairedRoster = metadata.Roster;
        var rosterChanged = false;
        if (metadata.Roster.Count == 0 && snapshot.Roster.Count > 0)
        {
            repairedRoster = snapshot.Roster;
            rosterChanged = true;
        }

        var repairedProgress = string.IsNullOrWhiteSpace(snapshot.ActOrFloor)
            ? metadata.ActOrFloor
            : snapshot.ActOrFloor.Trim();
        var progressChanged = repairedProgress != metadata.ActOrFloor;

        if (!rosterChanged && !progressChanged)
            return;

        _bank.UpdateMetadata(metadata with
        {
            Label = CampaignLabeler.Build(repairedRoster),
            Roster = repairedRoster,
            ActOrFloor = repairedProgress
        });
    }
}
