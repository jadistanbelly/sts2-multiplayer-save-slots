using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public sealed record CampaignCompatibilityWarning(
    string WarningKey,
    string Title,
    string Message);

public static class CampaignCompatibilityChecker
{
    public static CampaignCompatibilityWarning? BuildWarning(
        CampaignMetadata expectedCampaign,
        IReadOnlyList<PlayerIdentity> currentRoster)
    {
        var expectedById = StablePlayers(expectedCampaign.Roster);
        var currentById = StablePlayers(currentRoster);
        if (expectedById.Count == 0 || currentById.Count == 0)
            return null;

        var expectedIds = expectedById.Keys.ToHashSet(StringComparer.Ordinal);
        var currentIds = currentById.Keys.ToHashSet(StringComparer.Ordinal);
        var missingIds = expectedIds.Except(currentIds, StringComparer.Ordinal).Order(StringComparer.Ordinal).ToList();
        var extraIds = currentIds.Except(expectedIds, StringComparer.Ordinal).Order(StringComparer.Ordinal).ToList();
        if (missingIds.Count == 0 && extraIds.Count == 0)
            return null;

        var lines = new List<string>
        {
            "This lobby may not match the selected campaign's original party."
        };

        if (missingIds.Count > 0)
            lines.Add($"Missing original players: {FormatPlayers(missingIds.Select(id => expectedById[id]))}");

        if (extraIds.Count > 0)
            lines.Add($"Extra current players: {FormatPlayers(extraIds.Select(id => currentById[id]))}");

        lines.Add("Press Embark again to continue anyway, or adjust the lobby before starting.");

        return new CampaignCompatibilityWarning(
            BuildWarningKey(expectedCampaign.CampaignId, expectedIds, currentIds),
            "Multiplayer Save Slots",
            string.Join("\n\n", lines));
    }

    private static Dictionary<string, PlayerIdentity> StablePlayers(IReadOnlyList<PlayerIdentity> roster) =>
        roster
            .Where(player => !string.IsNullOrWhiteSpace(player.StableId))
            .GroupBy(player => player.StableId!.Trim(), StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.Ordinal);

    private static string FormatPlayers(IEnumerable<PlayerIdentity> players)
    {
        var names = players.Select(DisplayName).ToList();
        return names.Count <= 3
            ? string.Join(", ", names)
            : $"{string.Join(", ", names.Take(3))} +{names.Count - 3}";
    }

    private static string DisplayName(PlayerIdentity player) =>
        string.IsNullOrWhiteSpace(player.DisplayName) ? "Unknown" : player.DisplayName.Trim();

    private static string BuildWarningKey(
        string campaignId,
        IEnumerable<string> expectedIds,
        IEnumerable<string> currentIds) =>
        $"{campaignId}|expected:{string.Join(",", expectedIds.Order(StringComparer.Ordinal))}|current:{string.Join(",", currentIds.Order(StringComparer.Ordinal))}";
}
