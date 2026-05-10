using System.Globalization;
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.UI;

public enum PickerRowKind
{
    StartNewRun,
    Campaign
}

public sealed record MultiplayerSavePickerModel(
    MultiplayerGameMode GameMode,
    IReadOnlyList<MultiplayerSavePickerRow> Rows)
{
    public IReadOnlyList<MultiplayerSavePickerRow> CampaignRows =>
        Rows.Where(row => row.Kind == PickerRowKind.Campaign).ToList();

    public MultiplayerSavePickerRow? DefaultSelectedCampaign => CampaignRows.FirstOrDefault();

    public static string EmptyPreviewTitle => "No saved runs";

    public static string EmptyPreviewBody => "Start a new multiplayer run to create the first save slot.";

    public static string CharacterBadgeText(string? selectedCharacterId)
    {
        if (string.IsNullOrWhiteSpace(selectedCharacterId))
            return "??";

        return selectedCharacterId.Trim().ToUpperInvariant() switch
        {
            "CHARACTER.IRONCLAD" => "IC",
            "CHARACTER.SILENT" => "SI",
            "CHARACTER.DEFECT" => "DE",
            "CHARACTER.NECROBINDER" => "NE",
            "CHARACTER.REGENT" => "RG",
            _ => "??"
        };
    }
}

public sealed record MultiplayerSavePickerDetails(
    string Title,
    string Subtitle,
    IReadOnlyList<string> SummaryLines,
    IReadOnlyList<string> RosterLines)
{
    public IReadOnlyList<MultiplayerSavePickerRosterEntry> RosterEntries { get; init; } = [];
}

public sealed record MultiplayerSavePickerRosterEntry(
    string Text,
    string? SelectedCharacterId,
    string BadgeText,
    bool HasKnownPlayer);

public sealed record MultiplayerSavePickerRow(
    PickerRowKind Kind,
    string Title,
    string Subtitle,
    string? CampaignId,
    MultiplayerSavePickerDetails? Details)
{
    public static MultiplayerSavePickerRow StartNew() =>
        new(PickerRowKind.StartNewRun, "Start New Run", "Create a separate multiplayer run", null, null);

    public static IReadOnlyList<MultiplayerSavePickerRow> Campaigns(IReadOnlyList<CampaignMetadata> campaigns)
    {
        var rows = campaigns.Select(Campaign).ToList();
        var duplicateKeys = rows
            .Where(row => row.Kind == PickerRowKind.Campaign)
            .GroupBy(row => $"{row.Title}\u001f{row.Subtitle}", StringComparer.Ordinal)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToHashSet(StringComparer.Ordinal);

        return rows
            .Select(row => duplicateKeys.Contains($"{row.Title}\u001f{row.Subtitle}")
                ? row.WithSubtitle($"{row.Subtitle} - ID {ShortValue(row.CampaignId)}")
                : row)
            .ToList();
    }

    public static MultiplayerSavePickerRow Campaign(CampaignMetadata metadata)
    {
        var playerLabel = metadata.Roster.Count == 1 ? "1 player" : $"{metadata.Roster.Count} players";
        var subtitle = string.IsNullOrWhiteSpace(metadata.ActOrFloor)
            ? playerLabel
            : $"{metadata.ActOrFloor} - {playerLabel}";

        return new MultiplayerSavePickerRow(
            PickerRowKind.Campaign,
            metadata.Label,
            subtitle,
            metadata.CampaignId,
            BuildDetails(metadata, subtitle));
    }

    private MultiplayerSavePickerRow WithSubtitle(string subtitle) =>
        this with
        {
            Subtitle = subtitle,
            Details = Details is null ? null : Details with { Subtitle = subtitle }
        };

    private static MultiplayerSavePickerDetails BuildDetails(CampaignMetadata metadata, string subtitle)
    {
        var progress = string.IsNullOrWhiteSpace(metadata.ActOrFloor) ? "Unknown" : metadata.ActOrFloor.Trim();
        var summaryLines = new[]
        {
            $"Progress: {progress}",
            $"Players: {metadata.Roster.Count}",
            $"Created: {FormatTimestamp(metadata.CreatedAtUtc)}",
            $"Last played: {FormatTimestamp(metadata.LastPlayedAtUtc)}",
            $"Campaign id: {ShortValue(metadata.CampaignId)}",
            $"Save fingerprint: {ShortValue(metadata.PayloadChecksum ?? metadata.ActiveChecksum)}"
        };

        var rosterEntries = metadata.Roster.Count == 0
            ? [new MultiplayerSavePickerRosterEntry("Unknown party", null, "?", false)]
            : metadata.Roster
                .Select((player, index) => new MultiplayerSavePickerRosterEntry(
                    $"{index + 1}. {DisplayName(player)}",
                    player.SelectedCharacterId,
                    BadgeText(player),
                    true))
                .ToArray();
        var rosterLines = rosterEntries.Select(entry => entry.Text).ToArray();

        return new MultiplayerSavePickerDetails(metadata.Label, subtitle, summaryLines, rosterLines)
        {
            RosterEntries = rosterEntries
        };
    }

    private static string DisplayName(PlayerIdentity player) =>
        FormatPlayerName(player, CharacterDisplayName(player.SelectedCharacterId));

    private static string FormatPlayerName(PlayerIdentity player, string? characterName)
    {
        var displayName = string.IsNullOrWhiteSpace(player.DisplayName) ? "Unknown" : player.DisplayName.Trim();
        return string.IsNullOrWhiteSpace(characterName) ? displayName : $"{displayName} - {characterName}";
    }

    private static string? CharacterDisplayName(string? selectedCharacterId)
    {
        if (string.IsNullOrWhiteSpace(selectedCharacterId))
            return null;

        var normalized = selectedCharacterId.Trim();
        var knownName = normalized.ToUpperInvariant() switch
        {
            "CHARACTER.IRONCLAD" => "The Ironclad",
            "CHARACTER.SILENT" => "The Silent",
            "CHARACTER.DEFECT" => "The Defect",
            "CHARACTER.NECROBINDER" => "The Necrobinder",
            "CHARACTER.REGENT" => "The Regent",
            _ => null
        };
        if (knownName is not null)
            return knownName;

        var token = normalized.Contains('.', StringComparison.Ordinal)
            ? normalized[(normalized.LastIndexOf('.') + 1)..]
            : normalized;
        token = token.Replace('_', ' ').Trim();
        return string.IsNullOrWhiteSpace(token)
            ? null
            : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(token.ToLowerInvariant());
    }

    private static string ShortValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "Unknown";

        var trimmed = value.Trim();
        return trimmed.Length <= 8 ? trimmed : trimmed[..8];
    }

    private static string FormatTimestamp(DateTimeOffset timestamp) =>
        timestamp.ToUniversalTime().ToString("yyyy-MM-dd HH:mm 'UTC'", CultureInfo.InvariantCulture);

    private static string BadgeText(PlayerIdentity player)
    {
        var characterBadge = MultiplayerSavePickerModel.CharacterBadgeText(player.SelectedCharacterId);
        return characterBadge == "??" ? InitialBadge(player.DisplayName) : characterBadge;
    }

    private static string InitialBadge(string? displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            return "?";

        var trimmed = displayName.Trim();
        foreach (var character in trimmed)
        {
            if (char.IsLetterOrDigit(character))
                return char.ToUpperInvariant(character).ToString();
        }

        return "?";
    }
}
