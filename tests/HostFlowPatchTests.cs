using System.Reflection;
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.Tests;

public static class HostFlowPatchTests
{
    public static IEnumerable<TestCase> All()
    {
        yield return new TestCase("game mode map handles standard daily and custom", GameModeMapHandlesAllModes);
        yield return new TestCase("host submenu prefix allows vanilla while resuming", HostSubmenuPrefixAllowsVanillaWhileResuming);
        yield return new TestCase("host submenu prefix blocks vanilla when picker setup fails", HostSubmenuPrefixBlocksVanillaWhenPickerSetupFails);
    }

    private static void GameModeMapHandlesAllModes()
    {
        var map = new MultiplayerSaveGameModeMap();
        var toMultiplayer = typeof(MultiplayerSaveGameModeMap).GetMethod(
            nameof(MultiplayerSaveGameModeMap.ToMultiplayerGameMode),
            BindingFlags.Instance | BindingFlags.Public)
            ?? throw new InvalidOperationException("ToMultiplayerGameMode method was not found");
        var toSts2 = typeof(MultiplayerSaveGameModeMap).GetMethod(
            nameof(MultiplayerSaveGameModeMap.ToSts2GameMode),
            BindingFlags.Instance | BindingFlags.Public)
            ?? throw new InvalidOperationException("ToSts2GameMode method was not found");
        var gameModeType = toMultiplayer.GetParameters().Single().ParameterType;

        AssertEx.Equal(MultiplayerGameMode.Standard, InvokeToMultiplayer(map, toMultiplayer, gameModeType, "Standard"));
        AssertEx.Equal(MultiplayerGameMode.Daily, InvokeToMultiplayer(map, toMultiplayer, gameModeType, "Daily"));
        AssertEx.Equal(MultiplayerGameMode.Custom, InvokeToMultiplayer(map, toMultiplayer, gameModeType, "Custom"));
        AssertEx.Equal(Enum.Parse(gameModeType, "Standard"), InvokeToSts2(map, toSts2, MultiplayerGameMode.Standard));
        AssertEx.Equal(Enum.Parse(gameModeType, "Daily"), InvokeToSts2(map, toSts2, MultiplayerGameMode.Daily));
        AssertEx.Equal(Enum.Parse(gameModeType, "Custom"), InvokeToSts2(map, toSts2, MultiplayerGameMode.Custom));
    }

    private static MultiplayerGameMode InvokeToMultiplayer(
        MultiplayerSaveGameModeMap map,
        MethodInfo method,
        Type gameModeType,
        string value) =>
        (MultiplayerGameMode)(method.Invoke(map, [Enum.Parse(gameModeType, value)])
            ?? throw new InvalidOperationException($"Failed to map STS2 {value} game mode"));

    private static object InvokeToSts2(
        MultiplayerSaveGameModeMap map,
        MethodInfo method,
        MultiplayerGameMode value) =>
        method.Invoke(map, [value])
            ?? throw new InvalidOperationException($"Failed to map {value} game mode");

    private static void HostSubmenuPrefixAllowsVanillaWhileResuming()
    {
        var (resumingField, prefix, _) = GetHostSubmenuPatchMembers();
        var gameModeType = prefix.GetParameters()[1].ParameterType;

        resumingField.SetValue(null, true);
        try
        {
            var result = prefix.Invoke(null, [null, Enum.Parse(gameModeType, "Standard")]);

            AssertEx.Equal(true, result);
        }
        finally
        {
            resumingField.SetValue(null, false);
        }
    }

    private static void HostSubmenuPrefixBlocksVanillaWhenPickerSetupFails()
    {
        var (_, prefix, _) = GetHostSubmenuPatchMembers();
        var gameModeType = prefix.GetParameters()[1].ParameterType;

        var tryOpenPicker = typeof(MultiplayerSaveGameModeMap).Assembly
            .GetType("MultiplayerSaveSlots.Patches.MultiplayerHostSubmenuPatch")
            ?.GetMethod("TryOpenPicker", BindingFlags.Static | BindingFlags.Public)
            ?? throw new InvalidOperationException("Host submenu TryOpenPicker helper was not found");

        var result = tryOpenPicker.Invoke(null, new object[]
        {
            Enum.Parse(gameModeType, "Standard"),
            (Func<HostFlowController>)(() => throw new InvalidOperationException("picker setup failed")),
            (Action<HostFlowController, MultiplayerGameMode>)((_, _) => { })
        });

        AssertEx.Equal(false, result);
    }

    private static (FieldInfo ResumingField, MethodInfo Prefix, Type PatchType) GetHostSubmenuPatchMembers()
    {
        var patchType = typeof(MultiplayerSaveGameModeMap).Assembly.GetType("MultiplayerSaveSlots.Patches.MultiplayerHostSubmenuPatch")
            ?? throw new InvalidOperationException("Host submenu patch type was not found");
        var resumingField = patchType.GetField("_resumingVanilla", BindingFlags.Static | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Host submenu patch resume guard was not found");
        var prefix = patchType.GetMethod("Prefix", BindingFlags.Static | BindingFlags.Public)
            ?? throw new InvalidOperationException("Host submenu prefix was not found");

        return (resumingField, prefix, patchType);
    }
}
