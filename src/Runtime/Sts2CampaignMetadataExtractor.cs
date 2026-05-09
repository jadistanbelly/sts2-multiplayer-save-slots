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

    public static CampaignMetadataSnapshot FromSerializableRun(SerializableRun run)
    {
        var roster = run.Players
            .Select(player => ToIdentity(run.PlatformType, player.NetId, null))
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
            .Select(player => ToIdentity(platform, player.id, player.slotId))
            .ToList();

    private static PlayerIdentity ToIdentity(PlatformType platform, ulong playerId, int? slotId)
    {
        var stableId = $"{platform}:{playerId}";
        try
        {
            var name = PlatformUtil.GetPlayerName(platform, playerId);
            return new PlayerIdentity(stableId, string.IsNullOrWhiteSpace(name) ? FallbackName(playerId, slotId) : name);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to resolve player name for {stableId}: {ex.Message}");
            return new PlayerIdentity(stableId, FallbackName(playerId, slotId));
        }
    }

    private static PlatformType GetVanillaMultiplayerPlatform() =>
        SteamInitializer.Initialized && !CommandLineHelper.HasArg("fastmp")
            ? PlatformType.Steam
            : PlatformType.None;

    private static string FallbackName(ulong playerId, int? slotId) =>
        slotId is null ? playerId.ToString() : $"Player {slotId.Value + 1}";
}
