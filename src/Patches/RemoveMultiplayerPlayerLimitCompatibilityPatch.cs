using System.Reflection;
using System.Runtime.ExceptionServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Runs;
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.Patches;

public static class RemoveMultiplayerPlayerLimitCompatibilityPatch
{
    private const string RmpHostBootstrapNodeTypeName =
        "RemoveMultiplayerPlayerLimit.Network.HostBootstrapModule+HostBootstrapNode";
    private const string RmpHostSubmenuMethodName = "OnHostSubmenuPressed";

    private static bool _patchApplied;
    private static bool _resumingRmpHost;
    private static MethodInfo? _rmpHostSubmenuMethod;

    public static bool Apply(Harmony harmony)
    {
        if (_patchApplied)
            return true;

        var method = FindRmpHostSubmenuMethod();
        if (method is null)
            return false;

        var prefix = typeof(RemoveMultiplayerPlayerLimitCompatibilityPatch).GetMethod(
            nameof(Prefix),
            BindingFlags.Static | BindingFlags.Public)
            ?? throw new MissingMethodException(nameof(RemoveMultiplayerPlayerLimitCompatibilityPatch), nameof(Prefix));

        harmony.Patch(method, prefix: new HarmonyMethod(prefix));
        _rmpHostSubmenuMethod = method;
        _patchApplied = true;
        Console.Error.WriteLine("[MultiplayerSaveSlots] RemoveMultiplayerPlayerLimit host compatibility patch enabled.");
        return true;
    }

    public static MethodInfo? FindRmpHostSubmenuMethod()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetType(RmpHostBootstrapNodeTypeName, throwOnError: false);
            var method = type?.GetMethod(RmpHostSubmenuMethodName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (method is null || !HasExpectedSignature(method))
                continue;

            return method;
        }

        return null;
    }

    public static bool Prefix(object __instance, NMultiplayerHostSubmenu submenu, GameMode gameMode)
    {
        if (_resumingRmpHost)
            return true;

        var method = _rmpHostSubmenuMethod ?? FindRmpHostSubmenuMethod();
        if (method is null)
        {
            Console.Error.WriteLine(
                "[MultiplayerSaveSlots] Failed to locate RemoveMultiplayerPlayerLimit host continuation; blocking direct host flow.");
            return false;
        }

        return TryOpenPickerBeforeRmpHost(
            __instance,
            method,
            submenu,
            gameMode,
            () => Sts2HostFlowRuntime.CreateController(submenu),
            Sts2HostFlowRuntime.ShowPicker);
    }

    public static bool TryOpenPickerBeforeRmpHost(
        object rmpHostBootstrap,
        MethodInfo rmpHostSubmenuMethod,
        NMultiplayerHostSubmenu submenu,
        GameMode gameMode,
        Func<HostFlowController> createController,
        Action<HostFlowController, MultiplayerGameMode> showPicker)
    {
        Sts2HostFlowRuntime.VanillaStartContinuation = (hostSubmenu, resumedGameMode) =>
            ResumeRmpHost(rmpHostBootstrap, rmpHostSubmenuMethod, hostSubmenu, resumedGameMode);

        return MultiplayerHostSubmenuPatch.TryOpenPicker(gameMode, createController, showPicker);
    }

    private static void ResumeRmpHost(
        object rmpHostBootstrap,
        MethodInfo rmpHostSubmenuMethod,
        NMultiplayerHostSubmenu hostSubmenu,
        GameMode gameMode)
    {
        _resumingRmpHost = true;
        try
        {
            rmpHostSubmenuMethod.Invoke(rmpHostBootstrap, [hostSubmenu, gameMode]);
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
        }
        finally
        {
            _resumingRmpHost = false;
        }
    }

    private static bool HasExpectedSignature(MethodInfo method)
    {
        var parameters = method.GetParameters();
        return parameters.Length == 2
            && parameters[0].ParameterType == typeof(NMultiplayerHostSubmenu)
            && parameters[1].ParameterType == typeof(GameMode);
    }
}
