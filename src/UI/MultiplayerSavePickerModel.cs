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

public sealed record MultiplayerSavePickerRow(
    PickerRowKind Kind,
    string Title,
    string Subtitle,
    string? CampaignId)
{
    public static MultiplayerSavePickerRow StartNew() =>
        new(PickerRowKind.StartNewRun, "Start New Run", "Create a separate multiplayer run", null);

    public static MultiplayerSavePickerRow Campaign(CampaignMetadata metadata)
    {
        var progress = string.IsNullOrWhiteSpace(metadata.ActOrFloor) ? "Unknown progress" : metadata.ActOrFloor;
        var playerLabel = metadata.Roster.Count == 1 ? "1 player" : $"{metadata.Roster.Count} players";

        return new MultiplayerSavePickerRow(
            PickerRowKind.Campaign,
            metadata.Label,
            $"{progress} - {playerLabel}",
            metadata.CampaignId);
    }
}
