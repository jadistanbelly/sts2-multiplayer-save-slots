using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Runs;
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.Patches;

[HarmonyPatch(typeof(NMultiplayerHostSubmenu), nameof(NMultiplayerHostSubmenu.StartHost))]
public static class MultiplayerHostSubmenuPatch
{
    private static bool _resumingVanilla;

    [HarmonyPrefix]
    public static bool Prefix(NMultiplayerHostSubmenu __instance, GameMode gameMode)
    {
        Sts2HostFlowRuntime.VanillaStartContinuation = ResumeVanillaStart;

        if (_resumingVanilla)
            return true;

        return TryOpenPicker(
            gameMode,
            () => Sts2HostFlowRuntime.CreateController(__instance),
            Sts2HostFlowRuntime.ShowPicker);
    }

    public static bool TryOpenPicker(
        GameMode gameMode,
        Func<HostFlowController> createController,
        Action<HostFlowController, MultiplayerGameMode> showPicker)
    {
        try
        {
            var controller = createController();
            var multiplayerGameMode = Sts2HostFlowRuntime.ModeMap.ToMultiplayerGameMode(gameMode);
            showPicker(controller, multiplayerGameMode);
            return false;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to open save picker; blocking vanilla host flow: {ex.Message}");
            return false;
        }
    }

    public static void ResumeVanillaStart(NMultiplayerHostSubmenu hostSubmenu, GameMode gameMode)
    {
        _resumingVanilla = true;
        try
        {
            hostSubmenu.StartHost(gameMode);
        }
        finally
        {
            _resumingVanilla = false;
        }
    }
}
