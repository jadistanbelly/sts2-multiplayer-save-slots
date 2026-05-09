using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.Patches;

[HarmonyPatch(typeof(NMultiplayerLoadGameScreen), nameof(NMultiplayerLoadGameScreen.ShouldAllowRunToBegin))]
public static class LoadLobbyCompatibilityPatch
{
    [HarmonyPostfix]
    public static void Postfix(NMultiplayerLoadGameScreen __instance, ref Task<bool> __result)
    {
        __result = AppendCompatibilityWarning(
            __result,
            () => Sts2HostFlowRuntime.CreateLoadLobbyCompatibilityGuard(__instance).ShouldAllowRunToBegin());
    }

    public static async Task<bool> AppendCompatibilityWarning(
        Task<bool> vanillaResult,
        Func<bool> shouldAllowRunToBegin)
    {
        var vanillaAllowed = await vanillaResult.ConfigureAwait(false);
        if (!vanillaAllowed)
            return false;

        try
        {
            return shouldAllowRunToBegin();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to run load-lobby compatibility warning: {ex.Message}");
            return true;
        }
    }
}
