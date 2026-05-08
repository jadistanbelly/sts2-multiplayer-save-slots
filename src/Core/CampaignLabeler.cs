namespace MultiplayerSaveSlots.Core;

public static class CampaignLabeler
{
    public static string Build(IReadOnlyList<PlayerIdentity> roster)
    {
        var names = roster
            .Select(player => string.IsNullOrWhiteSpace(player.DisplayName)
                ? "Unknown"
                : player.DisplayName.Trim())
            .ToList();

        return names.Count switch
        {
            0 => "Unknown party",
            1 => names[0],
            2 => $"{names[0]} + {names[1]}",
            _ => $"{names[0]} + {names[1]} + {names.Count - 2} more"
        };
    }
}
