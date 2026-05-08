namespace MultiplayerSaveSlots.Storage;

public sealed record ActiveSaveState(
    string CampaignId,
    string ActiveChecksumAfterActivation,
    DateTimeOffset ActivatedAtUtc);
