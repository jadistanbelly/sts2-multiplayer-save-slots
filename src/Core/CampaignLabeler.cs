namespace MultiplayerSaveSlots.Core;

public static class CampaignLabeler
{
    public static string Build(IReadOnlyList<PlayerIdentity> roster)
    {
        return roster.Count switch
        {
            0 => "Unknown party",
            1 => NormalizeName(roster[0]),
            2 => $"{NormalizeName(roster[0])}, {NormalizeName(roster[1])}",
            _ => $"{NormalizeName(roster[0])}, {NormalizeName(roster[1])} +{roster.Count - 2}"
        };
    }

    private static string NormalizeName(PlayerIdentity player)
    {
        return string.IsNullOrWhiteSpace(player.DisplayName)
            ? "Unknown"
            : player.DisplayName.Trim();
    }
}
