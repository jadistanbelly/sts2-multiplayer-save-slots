namespace MultiplayerSaveSlots.Storage;

public sealed record ActiveSaveState(
    string CampaignId,
    string? ActiveChecksumBeforeActivation,
    string ActiveChecksumAfterActivation,
    DateTimeOffset ActivatedAtUtc);
