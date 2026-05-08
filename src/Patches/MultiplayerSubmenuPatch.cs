using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

namespace MultiplayerSaveSlots.Patches;

[HarmonyPatch(typeof(NMultiplayerSubmenu), "UpdateButtons")]
public static class MultiplayerSubmenuPatch
{
    [HarmonyPostfix]
    public static void Postfix(NMultiplayerSubmenu __instance)
    {
        var hostButton = Traverse.Create(__instance).Field("_hostButton").GetValue<NSubmenuButton>();
        if (hostButton is not null)
            hostButton.Visible = true;
    }
}
