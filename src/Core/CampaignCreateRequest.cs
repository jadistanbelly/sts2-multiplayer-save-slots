namespace MultiplayerSaveSlots.Core;

public sealed record CampaignCreateRequest(
    MultiplayerGameMode GameMode,
    IReadOnlyList<PlayerIdentity> Roster,
    string SavePayloadPath,
    DateTimeOffset CreatedAtUtc,
    string? ActOrFloor = null);
