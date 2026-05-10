namespace MultiplayerSaveSlots.Core;

public sealed record PlayerIdentity(string? StableId, string DisplayName, string? SelectedCharacterId = null);
