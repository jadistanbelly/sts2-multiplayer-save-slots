using System.Reflection;
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Patches;
using MultiplayerSaveSlots.Runtime;
using MultiplayerSaveSlots.UI;

namespace MultiplayerSaveSlots.Tests;

public static class HostFlowPatchTests
{
    public static IEnumerable<TestCase> All()
    {
        yield return new TestCase("game mode map handles standard daily and custom", GameModeMapHandlesAllModes);
        yield return new TestCase("host submenu prefix allows vanilla while resuming", HostSubmenuPrefixAllowsVanillaWhileResuming);
        yield return new TestCase("host submenu prefix blocks vanilla when picker setup fails", HostSubmenuPrefixBlocksVanillaWhenPickerSetupFails);
        yield return new TestCase("save manager patch runs sync after vanilla task completes", SaveManagerPatchRunsSyncAfterVanillaTask);
        yield return new TestCase("save manager patch logs sync failure without failing vanilla save", SaveManagerPatchLogsSyncFailure);
        yield return new TestCase("save manager patch propagates vanilla task failure", SaveManagerPatchPropagatesVanillaFailure);
        yield return new TestCase("recovery modal exposes show method", RecoveryModalExposesShowMethod);
        yield return new TestCase("picker modal builds details body text", PickerModalBuildsDetailsBodyText);
        yield return new TestCase("load lobby compatibility patch preserves vanilla false", LoadLobbyCompatibilityPatchPreservesVanillaFalse);
        yield return new TestCase("load lobby compatibility patch applies guard after vanilla true", LoadLobbyCompatibilityPatchAppliesGuardAfterVanillaTrue);
        yield return new TestCase("load lobby compatibility patch exposes postfix", LoadLobbyCompatibilityPatchExposesPostfix);
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

    private static void SaveManagerPatchRunsSyncAfterVanillaTask()
    {
        var vanilla = Task.CompletedTask;
        var syncCount = 0;
        var logs = new List<string>();

        var wrapped = SaveManagerPatch.AppendSync(
            vanilla,
            () => new FakeSaveSyncController(() =>
            {
                syncCount++;
                return OperationResult.Ok();
            }),
            logs.Add);

        wrapped.GetAwaiter().GetResult();

        AssertEx.Equal(1, syncCount);
        AssertEx.Equal(0, logs.Count);
    }

    private static void SaveManagerPatchLogsSyncFailure()
    {
        var logs = new List<string>();

        var wrapped = SaveManagerPatch.AppendSync(
            Task.CompletedTask,
            () => new FakeSaveSyncController(() => OperationResult.Fail("sync failed")),
            logs.Add);

        wrapped.GetAwaiter().GetResult();

        AssertEx.Equal(1, logs.Count);
        AssertEx.Equal("[MultiplayerSaveSlots] Save sync failed: sync failed", logs[0]);
    }

    private static void SaveManagerPatchPropagatesVanillaFailure()
    {
        var logs = new List<string>();
        var vanilla = Task.FromException(new InvalidOperationException("vanilla failed"));

        var wrapped = SaveManagerPatch.AppendSync(
            vanilla,
            () => new FakeSaveSyncController(() => OperationResult.Ok()),
            logs.Add);

        AssertEx.Throws<InvalidOperationException>(() => wrapped.GetAwaiter().GetResult());
        AssertEx.Equal(0, logs.Count);
    }

    private static void RecoveryModalExposesShowMethod()
    {
        var modalType = typeof(MultiplayerSaveGameModeMap).Assembly.GetType("MultiplayerSaveSlots.UI.MultiplayerSaveRecoveryModal");
        AssertEx.True(modalType is not null);
        var show = modalType!.GetMethod("Show", BindingFlags.Static | BindingFlags.Public);
        AssertEx.True(show is not null);
    }

    private static void PickerModalBuildsDetailsBodyText()
    {
        var modalType = typeof(MultiplayerSaveGameModeMap).Assembly.GetType("MultiplayerSaveSlots.UI.MultiplayerSavePickerModal");
        AssertEx.True(modalType is not null);
        var buildDetailsBody = modalType!.GetMethod("BuildDetailsBody", BindingFlags.Static | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Details body helper was not found");
        var details = new MultiplayerSavePickerDetails(
            "buddy1, buddy2",
            "Floor 18 - 2 players",
            ["Progress: Floor 18", "Players: 2"],
            ["1. buddy1", "2. buddy2"]);

        var body = buildDetailsBody.Invoke(null, [details]);

        AssertEx.Equal("Progress: Floor 18\nPlayers: 2\n\nRoster\n1. buddy1\n2. buddy2", body);
    }

    private static void LoadLobbyCompatibilityPatchPreservesVanillaFalse()
    {
        var guardCalls = 0;

        var result = LoadLobbyCompatibilityPatch.AppendCompatibilityWarning(
            Task.FromResult(false),
            () =>
            {
                guardCalls++;
                return true;
            }).GetAwaiter().GetResult();

        AssertEx.False(result);
        AssertEx.Equal(0, guardCalls);
    }

    private static void LoadLobbyCompatibilityPatchAppliesGuardAfterVanillaTrue()
    {
        var guardCalls = 0;

        var result = LoadLobbyCompatibilityPatch.AppendCompatibilityWarning(
            Task.FromResult(true),
            () =>
            {
                guardCalls++;
                return false;
            }).GetAwaiter().GetResult();

        AssertEx.False(result);
        AssertEx.Equal(1, guardCalls);
    }

    private static void LoadLobbyCompatibilityPatchExposesPostfix()
    {
        var patchType = typeof(MultiplayerSaveGameModeMap).Assembly.GetType("MultiplayerSaveSlots.Patches.LoadLobbyCompatibilityPatch");
        AssertEx.True(patchType is not null);
        var postfix = patchType!.GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public);
        AssertEx.True(postfix is not null);
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

    private sealed class FakeSaveSyncController : ISaveSyncRunner
    {
        private readonly Func<OperationResult> _sync;

        public FakeSaveSyncController(Func<OperationResult> sync)
        {
            _sync = sync;
        }

        public OperationResult SyncAfterVanillaSave() => _sync();
    }
}
