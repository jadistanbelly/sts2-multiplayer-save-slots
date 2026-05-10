using System.Reflection;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Saves;
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public sealed class Sts2CampaignMetadataExtractor : ICampaignMetadataExtractor
{
    public CampaignMetadataSnapshot CaptureActiveSaveMetadata()
    {
        var platform = GetVanillaMultiplayerPlatform();
        var localPlayerId = PlatformUtil.GetLocalPlayerId(platform);
        var save = SaveManager.Instance.LoadAndCanonicalizeMultiplayerRunSave(localPlayerId);
        return save.Success && save.SaveData is not null
            ? FromSerializableRun(save.SaveData)
            : CampaignMetadataSnapshot.Empty;
    }

    public static CampaignMetadataSnapshot FromStartRunLobby(StartRunLobby lobby) =>
        new(CapturePlayers(lobby.Players, lobby.NetService.Platform), null);

    public static CampaignMetadataSnapshot FromLoadRunLobby(LoadRunLobby lobby)
    {
        var platform = lobby.NetService.Platform;
        var playerIds = lobby.ConnectedPlayerIds.ToHashSet();
        playerIds.Add(PlatformUtil.GetLocalPlayerId(platform));

        var roster = playerIds
            .Order()
            .Select(playerId => ToIdentity(platform, playerId, null))
            .ToList();

        return new CampaignMetadataSnapshot(roster, null);
    }

    public static CampaignMetadataSnapshot FromSerializableRun(SerializableRun run)
    {
        var roster = run.Players
            .Select(player => ToIdentity(run.PlatformType, player.NetId, null, TryReadMemberText(player, "CharacterId")))
            .ToList();
        var completedFloors = run.MapPointHistory?.Sum(act => act.Count) ?? 0;

        return new CampaignMetadataSnapshot(
            roster,
            RunProgressLabeler.Build(run.CurrentActIndex, completedFloors));
    }

    private static IReadOnlyList<PlayerIdentity> CapturePlayers(
        IReadOnlyList<LobbyPlayer> players,
        PlatformType platform) =>
        players
            .OrderBy(player => player.slotId)
            .Select(player => ToIdentity(platform, player.id, player.slotId, TryReadMemberText(player, "character")))
            .ToList();

    private static PlayerIdentity ToIdentity(
        PlatformType platform,
        ulong playerId,
        int? slotId,
        string? selectedCharacterId = null)
    {
        var stableId = $"{platform}:{playerId}";
        try
        {
            var name = PlatformUtil.GetPlayerName(platform, playerId);
            return new PlayerIdentity(
                stableId,
                string.IsNullOrWhiteSpace(name) ? FallbackName(playerId, slotId) : name,
                selectedCharacterId);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to resolve player name for {stableId}: {ex.Message}");
            return new PlayerIdentity(stableId, FallbackName(playerId, slotId), selectedCharacterId);
        }
    }

    private static string? TryReadMemberText(object? target, string memberName)
    {
        if (target is null)
            return null;

        var type = target.GetType();
        var property = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (property is not null)
            return ModelIdTextUnlessSelf(property.GetValue(target), target);

        var field = type.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        return field is null ? null : ModelIdTextUnlessSelf(field.GetValue(target), target);
    }

    private static string? ModelIdTextUnlessSelf(object? value, object source) =>
        ReferenceEquals(value, source) ? null : ModelIdText(value);

    private static string? ModelIdText(object? value)
    {
        if (value is null)
            return null;

        if (value is string stringValue)
            return string.IsNullOrWhiteSpace(stringValue) ? null : stringValue.Trim();

        var text = value.ToString();
        var typeName = value.GetType().FullName ?? value.GetType().Name;
        if (!string.IsNullOrWhiteSpace(text) && !string.Equals(text, typeName, StringComparison.Ordinal))
            return text;

        foreach (var memberName in new[] { "Id", "ModelId", "id", "modelId" })
        {
            var memberText = TryReadMemberText(value, memberName);
            if (!string.IsNullOrWhiteSpace(memberText))
                return memberText;
        }

        return null;
    }

    private static PlatformType GetVanillaMultiplayerPlatform() =>
        SteamInitializer.Initialized && !CommandLineHelper.HasArg("fastmp")
            ? PlatformType.Steam
            : PlatformType.None;

    private static string FallbackName(ulong playerId, int? slotId) =>
        slotId is null ? playerId.ToString() : $"Player {slotId.Value + 1}";
}
