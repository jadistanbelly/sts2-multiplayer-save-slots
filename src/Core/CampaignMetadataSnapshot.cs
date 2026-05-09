namespace MultiplayerSaveSlots.Core;

public sealed record CampaignMetadataSnapshot(
    IReadOnlyList<PlayerIdentity> Roster,
    string? ActOrFloor)
{
    public static CampaignMetadataSnapshot Empty { get; } = new([], null);
}
