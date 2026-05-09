using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.Patches;

[HarmonyPatch(
    typeof(StartRunLobby),
    "BeginRunForAllPlayers",
    new[] { typeof(string), typeof(List<ModifierModel>) })]
public static class StartRunLobbyMetadataPatch
{
    [HarmonyPrefix]
    public static void Prefix(StartRunLobby __instance)
    {
        try
        {
            if (!Sts2HostFlowRuntime.Session.IsPendingNewRun)
                return;

            var snapshot = Sts2CampaignMetadataExtractor.FromStartRunLobby(__instance);
            Sts2HostFlowRuntime.Session.CapturePendingNewRunMetadata(snapshot);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to capture lobby metadata: {ex.Message}");
        }
    }
}
