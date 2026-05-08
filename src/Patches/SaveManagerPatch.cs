using HarmonyLib;
using MegaCrit.Sts2.Core.Saves;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.Patches;

[HarmonyPatch(typeof(SaveManager), nameof(SaveManager.SaveRun))]
public static class SaveManagerPatch
{
    [HarmonyPostfix]
    public static void Postfix(ref Task __result)
    {
        __result = AppendSync(
            __result,
            Sts2HostFlowRuntime.CreateSaveSyncController,
            message => Console.Error.WriteLine(message));
    }

    public static async Task AppendSync(
        Task vanillaSaveTask,
        Func<ISaveSyncRunner> createController,
        Action<string> logError)
    {
        await vanillaSaveTask.ConfigureAwait(false);

        var result = createController().SyncAfterVanillaSave();
        if (!result.Success)
            logError($"[MultiplayerSaveSlots] Save sync failed: {result.ErrorMessage}");
    }
}
