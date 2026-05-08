namespace MultiplayerSaveSlots.Core;

public sealed record CampaignMetadata(
    string CampaignId,
    MultiplayerGameMode GameMode,
    string Label,
    IReadOnlyList<PlayerIdentity> Roster,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset LastPlayedAtUtc,
    string? ActiveChecksum,
    string? PayloadChecksum,
    string? ActOrFloor);
