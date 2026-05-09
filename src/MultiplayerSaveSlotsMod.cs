using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MultiplayerSaveSlots.Patches;

namespace MultiplayerSaveSlots;

[ModInitializer(nameof(Initialize))]
public static class MultiplayerSaveSlotsMod
{
    internal static Harmony? Harmony { get; private set; }

    public static void Initialize()
    {
        Harmony = new Harmony("com.jadistanbelly.multiplayersaveslots");
        Harmony.PatchAll(typeof(MultiplayerSaveSlotsMod).Assembly);
        RemoveMultiplayerPlayerLimitCompatibilityPatch.Apply(Harmony);
        GD.Print("[MultiplayerSaveSlots] Initialized");
    }
}
