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
        yield return new TestCase("run save manager patch runs sync after vanilla task completes", RunSaveManagerPatchRunsSyncAfterVanillaTask);
        yield return new TestCase("run save manager patch logs sync failure without failing vanilla save", RunSaveManagerPatchLogsSyncFailure);
        yield return new TestCase("run save manager patch propagates vanilla task failure", RunSaveManagerPatchPropagatesVanillaFailure);
        yield return new TestCase("run save manager patch exposes postfix", RunSaveManagerPatchExposesPostfix);
        yield return new TestCase("recovery modal exposes show method", RecoveryModalExposesShowMethod);
        yield return new TestCase("picker modal builds details body text", PickerModalBuildsDetailsBodyText);
        yield return new TestCase("picker modal exposes explicit UI builder", PickerModalExposesExplicitUiBuilder);
        yield return new TestCase("recovery modal exposes explicit UI builder", RecoveryModalExposesExplicitUiBuilder);
        yield return new TestCase("RMP host compatibility opens picker before direct host", RmpHostCompatibilityOpensPickerBeforeDirectHost);
        yield return new TestCase("RMP host compatibility resumes original host handler", RmpHostCompatibilityResumesOriginalHostHandler);
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

    private static void RunSaveManagerPatchRunsSyncAfterVanillaTask()
    {
        var vanilla = Task.CompletedTask;
        var syncCount = 0;
        var logs = new List<string>();

        var wrapped = RunSaveManagerPatch.AppendSync(
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

    private static void RunSaveManagerPatchLogsSyncFailure()
    {
        var logs = new List<string>();

        var wrapped = RunSaveManagerPatch.AppendSync(
            Task.CompletedTask,
            () => new FakeSaveSyncController(() => OperationResult.Fail("sync failed")),
            logs.Add);

        wrapped.GetAwaiter().GetResult();

        AssertEx.Equal(1, logs.Count);
        AssertEx.Equal("[MultiplayerSaveSlots] Save sync failed: sync failed", logs[0]);
    }

    private static void RunSaveManagerPatchPropagatesVanillaFailure()
    {
        var logs = new List<string>();
        var vanilla = Task.FromException(new InvalidOperationException("vanilla failed"));

        var wrapped = RunSaveManagerPatch.AppendSync(
            vanilla,
            () => new FakeSaveSyncController(() => OperationResult.Ok()),
            logs.Add);

        AssertEx.Throws<InvalidOperationException>(() => wrapped.GetAwaiter().GetResult());
        AssertEx.Equal(0, logs.Count);
    }

    private static void RunSaveManagerPatchExposesPostfix()
    {
        var patchType = typeof(MultiplayerSaveGameModeMap).Assembly.GetType("MultiplayerSaveSlots.Patches.RunSaveManagerPatch");
        AssertEx.True(patchType is not null);
        var postfix = patchType!.GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public);
        AssertEx.True(postfix is not null);
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

    private static void PickerModalExposesExplicitUiBuilder()
    {
        var modalType = typeof(MultiplayerSaveGameModeMap).Assembly.GetType("MultiplayerSaveSlots.UI.MultiplayerSavePickerModal");
        AssertEx.True(modalType is not null);
        var buildUi = modalType!.GetMethod("BuildUi", BindingFlags.Instance | BindingFlags.NonPublic);
        AssertEx.True(buildUi is not null);
    }

    private static void RecoveryModalExposesExplicitUiBuilder()
    {
        var modalType = typeof(MultiplayerSaveGameModeMap).Assembly.GetType("MultiplayerSaveSlots.UI.MultiplayerSaveRecoveryModal");
        AssertEx.True(modalType is not null);
        var buildUi = modalType!.GetMethod("BuildUi", BindingFlags.Instance | BindingFlags.NonPublic);
        AssertEx.True(buildUi is not null);
    }

    private static void RmpHostCompatibilityOpensPickerBeforeDirectHost()
    {
        var (tryOpenPicker, _, _) = GetRmpCompatibilityPatchMembers();
        var rmpHost = new FakeRmpHostBootstrap();
        var gameModeType = tryOpenPicker.GetParameters()[3].ParameterType;
        var shownGameModes = new List<MultiplayerGameMode>();

        var result = tryOpenPicker.Invoke(null, new object[]
        {
            rmpHost,
            rmpHost.Handler,
            null!,
            Enum.Parse(gameModeType, "Standard"),
            (Func<HostFlowController>)(() => null!),
            (Action<HostFlowController, MultiplayerGameMode>)((_, mode) => shownGameModes.Add(mode))
        });

        AssertEx.Equal(false, result);
        AssertEx.Equal(1, shownGameModes.Count);
        AssertEx.Equal(MultiplayerGameMode.Standard, shownGameModes[0]);
        AssertEx.Equal(0, rmpHost.Calls);
    }

    private static void RmpHostCompatibilityResumesOriginalHostHandler()
    {
        var (tryOpenPicker, resumingField, _) = GetRmpCompatibilityPatchMembers();
        var rmpHost = new FakeRmpHostBootstrap();
        var gameModeType = tryOpenPicker.GetParameters()[3].ParameterType;
        var gameMode = Enum.Parse(gameModeType, "Standard");
        var continuationProperty = typeof(Sts2HostFlowRuntime).GetProperty("VanillaStartContinuation")
            ?? throw new InvalidOperationException("VanillaStartContinuation property was not found");
        var previousContinuation = continuationProperty.GetValue(null);

        try
        {
            tryOpenPicker.Invoke(null, new object[]
            {
                rmpHost,
                rmpHost.Handler,
                null!,
                gameMode,
                (Func<HostFlowController>)(() => null!),
                (Action<HostFlowController, MultiplayerGameMode>)((_, _) => { })
            });

            var resume = typeof(Sts2HostFlowRuntime).GetMethod("ResumeVanillaStart")
                ?? throw new InvalidOperationException("ResumeVanillaStart method was not found");
            resume.Invoke(null, [null, gameMode]);

            AssertEx.Equal(1, rmpHost.Calls);
            AssertEx.Equal(gameMode, rmpHost.LastGameMode);
            AssertEx.Equal(false, resumingField.GetValue(null));
        }
        finally
        {
            continuationProperty.SetValue(null, previousContinuation);
        }
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

    private static (MethodInfo TryOpenPicker, FieldInfo ResumingField, Type PatchType) GetRmpCompatibilityPatchMembers()
    {
        var patchType = typeof(MultiplayerSaveGameModeMap).Assembly.GetType("MultiplayerSaveSlots.Patches.RemoveMultiplayerPlayerLimitCompatibilityPatch")
            ?? throw new InvalidOperationException("RMP compatibility patch type was not found");
        var tryOpenPicker = patchType.GetMethod("TryOpenPickerBeforeRmpHost", BindingFlags.Static | BindingFlags.Public)
            ?? throw new InvalidOperationException("RMP compatibility picker helper was not found");
        var resumingField = patchType.GetField("_resumingRmpHost", BindingFlags.Static | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("RMP compatibility resume guard was not found");

        return (tryOpenPicker, resumingField, patchType);
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

    private sealed class FakeRmpHostBootstrap
    {
        public int Calls { get; private set; }

        public object? LastGameMode { get; private set; }

        public MethodInfo Handler =>
            GetType().GetMethod(nameof(OnHostSubmenuPressed), BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Fake RMP handler was not found");

        private void OnHostSubmenuPressed(object? submenu, object gameMode)
        {
            Calls++;
            LastGameMode = gameMode;
        }
    }
}
