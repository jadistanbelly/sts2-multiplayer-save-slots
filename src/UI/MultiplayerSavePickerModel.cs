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
    IReadOnlyList<MultiplayerSavePickerRow> Rows);

public sealed record MultiplayerSavePickerDetails(
    string Title,
    string Subtitle,
    IReadOnlyList<string> SummaryLines,
    IReadOnlyList<string> RosterLines);

public sealed record MultiplayerSavePickerRow(
    PickerRowKind Kind,
    string Title,
    string Subtitle,
    string? CampaignId,
    MultiplayerSavePickerDetails? Details)
{
    public static MultiplayerSavePickerRow StartNew() =>
        new(PickerRowKind.StartNewRun, "Start New Run", "Create a separate multiplayer run", null, null);

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

    private static MultiplayerSavePickerDetails BuildDetails(CampaignMetadata metadata, string subtitle)
    {
        var progress = string.IsNullOrWhiteSpace(metadata.ActOrFloor) ? "Unknown" : metadata.ActOrFloor.Trim();
        var summaryLines = new[]
        {
            $"Progress: {progress}",
            $"Players: {metadata.Roster.Count}",
            $"Created: {FormatTimestamp(metadata.CreatedAtUtc)}",
            $"Last played: {FormatTimestamp(metadata.LastPlayedAtUtc)}",
            $"Campaign id: {metadata.CampaignId}"
        };

        var rosterLines = metadata.Roster.Count == 0
            ? ["Unknown party"]
            : metadata.Roster.Select((player, index) => $"{index + 1}. {DisplayName(player)}").ToArray();

        return new MultiplayerSavePickerDetails(metadata.Label, subtitle, summaryLines, rosterLines);
    }

    private static string DisplayName(PlayerIdentity player) =>
        string.IsNullOrWhiteSpace(player.DisplayName) ? "Unknown" : player.DisplayName.Trim();

    private static string FormatTimestamp(DateTimeOffset timestamp) =>
        timestamp.ToUniversalTime().ToString("yyyy-MM-dd HH:mm 'UTC'", CultureInfo.InvariantCulture);
}
