using System.Globalization;
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.UI;

public enum PickerRowKind
{
    StartNewRun,
    Campaign,
    ArchivedCampaign
}

public sealed record MultiplayerSavePickerModel(
    MultiplayerGameMode GameMode,
    IReadOnlyList<MultiplayerSavePickerRow> Rows,
    bool HasDeletedCampaigns = false)
{
    public IReadOnlyList<MultiplayerSavePickerRow> CampaignRows =>
        Rows.Where(row => row.Kind == PickerRowKind.Campaign).ToList();

    public IReadOnlyList<MultiplayerSavePickerRow> ArchivedRows =>
        Rows.Where(row => row.Kind == PickerRowKind.ArchivedCampaign).ToList();

    public MultiplayerSavePickerRow? DefaultSelectedCampaign => CampaignRows.FirstOrDefault();

    public MultiplayerSavePickerRow? DefaultSelectedArchive => ArchivedRows.FirstOrDefault();

    public static string EmptyPreviewTitle => "No saved runs";

    public static string EmptyPreviewBody => "Start a new multiplayer run to create the first save slot.";

    public static string EmptyArchiveTitle => "No archived runs";

    public static string EmptyArchiveBody => "Archived multiplayer runs will appear here.";

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
    IReadOnlyList<string> RosterLines,
    string? AutoLabel = null)
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
    MultiplayerSavePickerDetails? Details,
    string? ArchiveKey = null)
{
    public static MultiplayerSavePickerRow StartNew() =>
        new(PickerRowKind.StartNewRun, "Start New Run", "Create a separate multiplayer run", null, null);

    public static IReadOnlyList<MultiplayerSavePickerRow> Campaigns(IReadOnlyList<CampaignMetadata> campaigns)
    {
        var rows = campaigns.Select(Campaign).ToList();
        return DisambiguateRows(rows);
    }

    public static IReadOnlyList<MultiplayerSavePickerRow> ArchivedCampaigns(
        IReadOnlyList<MultiplayerSaveSlots.Core.ArchivedCampaign> archivedCampaigns)
    {
        var rows = archivedCampaigns.Select(ArchivedCampaign).ToList();
        return DisambiguateRows(rows);
    }

    public static MultiplayerSavePickerRow Campaign(CampaignMetadata metadata)
    {
        var subtitle = BuildSubtitle(metadata);
        var title = DisplayTitle(metadata);

        return new MultiplayerSavePickerRow(
            PickerRowKind.Campaign,
            title,
            subtitle,
            metadata.CampaignId,
            BuildDetails(metadata, title, subtitle));
    }

    public static MultiplayerSavePickerRow ArchivedCampaign(MultiplayerSaveSlots.Core.ArchivedCampaign archived)
    {
        var metadata = archived.Metadata;
        var subtitle = BuildSubtitle(metadata);
        var title = DisplayTitle(metadata);

        return new MultiplayerSavePickerRow(
            PickerRowKind.ArchivedCampaign,
            title,
            subtitle,
            metadata.CampaignId,
            BuildDetails(metadata, title, subtitle),
            archived.ArchiveKey);
    }

    private static IReadOnlyList<MultiplayerSavePickerRow> DisambiguateRows(
        IReadOnlyList<MultiplayerSavePickerRow> rows)
    {
        var duplicateKeys = rows
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

    private static string BuildSubtitle(CampaignMetadata metadata)
    {
        var playerLabel = metadata.Roster.Count == 1 ? "1 player" : $"{metadata.Roster.Count} players";

        return string.IsNullOrWhiteSpace(metadata.ActOrFloor)
            ? playerLabel
            : $"{metadata.ActOrFloor} - {playerLabel}";
    }

    private MultiplayerSavePickerRow WithSubtitle(string subtitle) =>
        this with
        {
            Subtitle = subtitle,
            Details = Details is null ? null : Details with { Subtitle = subtitle }
        };

    private static MultiplayerSavePickerDetails BuildDetails(CampaignMetadata metadata, string title, string subtitle)
    {
        var summaryLines = new[]
        {
            $"Last played: {FormatTimestamp(metadata.LastPlayedAtUtc)}",
            $"Save id: {ShortValue(metadata.CampaignId)}"
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
        var autoLabel = string.Equals(title, metadata.Label, StringComparison.Ordinal)
            ? null
            : metadata.Label;

        return new MultiplayerSavePickerDetails(title, subtitle, summaryLines, rosterLines, autoLabel)
        {
            RosterEntries = rosterEntries
        };
    }

    private static string DisplayTitle(CampaignMetadata metadata) =>
        string.IsNullOrWhiteSpace(metadata.CustomName) ? metadata.Label : metadata.CustomName.Trim();

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
