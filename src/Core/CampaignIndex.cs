namespace MultiplayerSaveSlots.Core;

public sealed record CampaignIndex(IReadOnlyList<string> CampaignIds)
{
    public static CampaignIndex Empty { get; } = new([]);
}
