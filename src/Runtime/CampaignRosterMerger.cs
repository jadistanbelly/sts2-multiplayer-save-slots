using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

internal static class CampaignRosterMerger
{
    public static IReadOnlyList<PlayerIdentity> MergeByStableId(
        IReadOnlyList<PlayerIdentity> existingRoster,
        IReadOnlyList<PlayerIdentity>? capturedRoster,
        out bool changed)
    {
        changed = false;
        if (capturedRoster is null || capturedRoster.Count == 0)
            return existingRoster;

        if (existingRoster.Count == 0)
        {
            changed = true;
            return capturedRoster;
        }

        if (!AllStableIds(existingRoster) || !AllStableIds(capturedRoster))
            return existingRoster;

        var capturedGroups = capturedRoster
            .GroupBy(player => player.StableId!, StringComparer.Ordinal)
            .ToList();
        if (capturedGroups.Any(group => group.Count() > 1))
            return existingRoster;

        var capturedById = capturedGroups.ToDictionary(group => group.Key, group => group.First(), StringComparer.Ordinal);
        if (capturedById.Count != existingRoster.Count ||
            !existingRoster.Select(player => player.StableId!).All(capturedById.ContainsKey))
        {
            return existingRoster;
        }

        var merged = existingRoster
            .Select(existing => MergePlayer(existing, capturedById[existing.StableId!]))
            .ToList();
        changed = !existingRoster.SequenceEqual(merged);
        return merged;
    }

    private static PlayerIdentity MergePlayer(PlayerIdentity existing, PlayerIdentity captured) =>
        existing with
        {
            DisplayName = string.IsNullOrWhiteSpace(captured.DisplayName)
                ? existing.DisplayName
                : captured.DisplayName,
            SelectedCharacterId = string.IsNullOrWhiteSpace(captured.SelectedCharacterId)
                ? existing.SelectedCharacterId
                : captured.SelectedCharacterId
        };

    private static bool AllStableIds(IReadOnlyList<PlayerIdentity> roster) =>
        roster.All(player => !string.IsNullOrWhiteSpace(player.StableId));
}
