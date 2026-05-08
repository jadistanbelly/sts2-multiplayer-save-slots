using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Storage;
using MultiplayerSaveSlots.UI;

namespace MultiplayerSaveSlots.Runtime;

public sealed class Sts2SaveBankAdapter : IHostFlowSaveBank
{
    private readonly MultiplayerSaveBank _bank;

    public Sts2SaveBankAdapter(MultiplayerSaveBank bank)
    {
        _bank = bank;
    }

    public IReadOnlyList<CampaignMetadata> ListCampaigns(MultiplayerGameMode gameMode) =>
        _bank.ListCampaigns(gameMode);
}

public static class Sts2HostFlowRuntime
{
    public static HostFlowSession Session { get; } = new();

    public static MultiplayerSaveGameModeMap ModeMap { get; } = new();

    public static Action<NMultiplayerHostSubmenu, GameMode> VanillaStartContinuation { get; set; } =
        (_, _) => throw new InvalidOperationException("Vanilla multiplayer host continuation is not configured.");

    public static HostFlowController CreateController(NMultiplayerHostSubmenu hostSubmenu)
    {
        var paths = CreatePaths();
        var bank = new MultiplayerSaveBank(new SaveBankPaths(paths.BankRootDirectory));
        var switcher = new ActiveSaveSwitcher(bank, paths.ActiveSavePath, paths.ActiveStatePath);
        return new HostFlowController(
            new Sts2SaveBankAdapter(bank),
            new DelegateActiveSaveActivator(switcher.Activate),
            new Sts2HostFlowContinuation(hostSubmenu),
            Session,
            new SystemClock());
    }

    public static MultiplayerSaveRuntimePaths CreatePaths()
    {
        var activePath = RunSaveManager.GetRunSavePath(
            SaveManager.Instance.CurrentProfileId,
            RunSaveManager.multiplayerRunSaveFileName);
        return MultiplayerSaveRuntimePaths.FromActiveSavePath(activePath);
    }

    public static void ResumeVanillaStart(NMultiplayerHostSubmenu hostSubmenu, GameMode gameMode) =>
        VanillaStartContinuation(hostSubmenu, gameMode);

    public static void ShowPicker(HostFlowController controller, MultiplayerGameMode gameMode) =>
        MultiplayerSavePickerModal.Show(controller, gameMode);
}

public sealed class MultiplayerSaveGameModeMap
{
    public MultiplayerGameMode ToMultiplayerGameMode(GameMode gameMode) =>
        gameMode switch
        {
            GameMode.Daily => MultiplayerGameMode.Daily,
            GameMode.Custom => MultiplayerGameMode.Custom,
            _ => MultiplayerGameMode.Standard
        };

    public GameMode ToSts2GameMode(MultiplayerGameMode gameMode) =>
        gameMode switch
        {
            MultiplayerGameMode.Daily => GameMode.Daily,
            MultiplayerGameMode.Custom => GameMode.Custom,
            _ => GameMode.Standard
        };
}

public sealed class Sts2HostFlowContinuation : IHostFlowContinuation
{
    private readonly NMultiplayerHostSubmenu _hostSubmenu;

    public Sts2HostFlowContinuation(NMultiplayerHostSubmenu hostSubmenu)
    {
        _hostSubmenu = hostSubmenu;
    }

    public OperationResult StartNewRun(MultiplayerGameMode gameMode)
    {
        try
        {
            Sts2HostFlowRuntime.ResumeVanillaStart(_hostSubmenu, Sts2HostFlowRuntime.ModeMap.ToSts2GameMode(gameMode));
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    public OperationResult LoadExistingRun()
    {
        try
        {
            var stack = Traverse.Create(_hostSubmenu).Field("_stack").GetValue<NSubmenuStack>();
            var multiplayerSubmenu = stack.GetSubmenuType<NMultiplayerSubmenu>();
            var platformType = GetVanillaMultiplayerPlatform();
            var readSave = SaveManager.Instance.LoadAndCanonicalizeMultiplayerRunSave(PlatformUtil.GetLocalPlayerId(platformType));
            if (!readSave.Success || readSave.SaveData == null)
                return OperationResult.Fail($"Failed to load activated multiplayer save: {readSave.Status}");

            multiplayerSubmenu.StartHost(readSave.SaveData);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    private static PlatformType GetVanillaMultiplayerPlatform() =>
        SteamInitializer.Initialized && !CommandLineHelper.HasArg("fastmp")
            ? PlatformType.Steam
            : PlatformType.None;
}
