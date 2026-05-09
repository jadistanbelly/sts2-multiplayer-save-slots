using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Storage;
using MultiplayerSaveSlots.UI;
using System.Text.Json;

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

public sealed class Sts2ActiveSaveSync : IActiveSaveSync
{
    private readonly MultiplayerSaveBank _bank;
    private readonly ActiveSaveSwitcher _switcher;
    private readonly string _activeSavePath;
    private readonly ICampaignMetadataExtractor _metadataExtractor;

    public Sts2ActiveSaveSync(
        MultiplayerSaveBank bank,
        ActiveSaveSwitcher switcher,
        string activeSavePath,
        ICampaignMetadataExtractor? metadataExtractor = null)
    {
        _bank = bank;
        _switcher = switcher;
        _activeSavePath = activeSavePath;
        _metadataExtractor = metadataExtractor ?? new EmptyCampaignMetadataExtractor();
    }

    public OperationResult SyncBack(DateTimeOffset nowUtc)
    {
        try
        {
            var metadata = CaptureMetadataOrEmpty();
            _switcher.SyncBack(nowUtc, metadata.ActOrFloor);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }

    public OperationResult<string> FinalizePendingNewRun(
        MultiplayerGameMode gameMode,
        CampaignMetadataSnapshot metadata,
        DateTimeOffset nowUtc)
    {
        try
        {
            if (!File.Exists(_activeSavePath))
                return OperationResult<string>.Fail("Active multiplayer save is missing");

            var activeMetadata = CaptureMetadataOrEmpty();
            var roster = metadata.Roster.Count > 0 ? metadata.Roster : activeMetadata.Roster;
            var actOrFloor = activeMetadata.ActOrFloor ?? metadata.ActOrFloor;
            var created = _bank.CreateCampaign(new CampaignCreateRequest(gameMode, roster, _activeSavePath, nowUtc, actOrFloor));
            _switcher.ClaimActiveSave(created.CampaignId, nowUtc);
            return OperationResult<string>.Ok(created.CampaignId);
        }
        catch (Exception ex)
        {
            return OperationResult<string>.Fail(ex.Message);
        }
    }

    private CampaignMetadataSnapshot CaptureMetadataOrEmpty()
    {
        try
        {
            return _metadataExtractor.CaptureActiveSaveMetadata();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to capture active save metadata: {ex.Message}");
            return CampaignMetadataSnapshot.Empty;
        }
    }
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
        var metadataExtractor = new Sts2CampaignMetadataExtractor();
        return new HostFlowController(
            new Sts2SaveBankAdapter(bank),
            new ActiveSaveReplacementGuard(paths.ActiveSavePath, paths.ActiveStatePath),
            new DelegateActiveSaveActivator(switcher.Activate, switcher.RestorePreviousActive),
            new Sts2HostFlowContinuation(hostSubmenu),
            new ActiveSaveRecoveryService(
                bank,
                switcher,
                paths.ActiveSavePath,
                paths.ActiveStatePath,
                metadataExtractor),
            Session,
            new SystemClock(),
            new ActivatedCampaignMetadataRepair(bank, metadataExtractor));
    }

    public static SaveSyncController CreateSaveSyncController()
    {
        var paths = CreatePaths();
        var bank = new MultiplayerSaveBank(new SaveBankPaths(paths.BankRootDirectory));
        var switcher = new ActiveSaveSwitcher(bank, paths.ActiveSavePath, paths.ActiveStatePath);
        return new SaveSyncController(
            new Sts2ActiveSaveSync(bank, switcher, paths.ActiveSavePath, new Sts2CampaignMetadataExtractor()),
            Session,
            new SystemClock());
    }

    public static LoadLobbyCompatibilityGuard CreateLoadLobbyCompatibilityGuard(NMultiplayerLoadGameScreen loadGameScreen)
    {
        var paths = CreatePaths();
        var bank = new MultiplayerSaveBank(new SaveBankPaths(paths.BankRootDirectory));
        return new LoadLobbyCompatibilityGuard(
            Session,
            bank.GetCampaign,
            () => CaptureLoadLobbyRoster(loadGameScreen),
            ShowCompatibilityWarning);
    }

    private static IReadOnlyList<PlayerIdentity> CaptureLoadLobbyRoster(NMultiplayerLoadGameScreen loadGameScreen)
    {
        var field = AccessTools.Field(typeof(NMultiplayerLoadGameScreen), "_runLobby")
            ?? throw new InvalidOperationException("NMultiplayerLoadGameScreen._runLobby field was not found.");
        var lobby = field.GetValue(loadGameScreen) as LoadRunLobby
            ?? throw new InvalidOperationException("Load-run lobby is not available.");

        return Sts2CampaignMetadataExtractor.FromLoadRunLobby(lobby).Roster;
    }

    private static void ShowCompatibilityWarning(CampaignCompatibilityWarning warning)
    {
        var popup = NErrorPopup.Create(warning.Title, warning.Message, showReportBugButton: false);
        var container = NModalContainer.Instance;
        if (popup is null || container is null)
            throw new InvalidOperationException("Compatibility warning popup is not available.");

        container.Clear();
        container.Add(popup);
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

public sealed class ActiveSaveReplacementGuard : IActiveSavePreflight
{
    private readonly string _activeSavePath;
    private readonly string _statePath;

    public ActiveSaveReplacementGuard(string activeSavePath, string statePath)
    {
        _activeSavePath = activeSavePath;
        _statePath = statePath;
    }

    public OperationResult EnsureActiveSaveCanBeReplaced()
    {
        if (!File.Exists(_activeSavePath))
            return OperationResult.Ok();

        if (!File.Exists(_statePath))
            return OperationResult.Fail("Current multiplayer save is not managed by Multiplayer Save Slots yet.");

        try
        {
            var state = JsonFile.Read<ActiveSaveState>(_statePath);
            var currentActiveChecksum = FileChecksum.Sha256(_activeSavePath);
            return currentActiveChecksum == state.ActiveChecksumAfterActivation
                ? OperationResult.Ok()
                : OperationResult.Fail("Active save has unsynced changes");
        }
        catch (Exception ex) when (ex is IOException or JsonException or InvalidOperationException)
        {
            return OperationResult.Fail($"Active save state cannot be verified: {ex.Message}");
        }
    }
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
    private NMultiplayerSubmenu? _preparedMultiplayerSubmenu;

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

    public OperationResult PrepareLoadExistingRun()
    {
        try
        {
            _preparedMultiplayerSubmenu = GetMultiplayerSubmenu();
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
            var multiplayerSubmenu = _preparedMultiplayerSubmenu ?? GetMultiplayerSubmenu();
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

    private NMultiplayerSubmenu GetMultiplayerSubmenu()
    {
        var stack = Traverse.Create(_hostSubmenu).Field("_stack").GetValue<NSubmenuStack>();
        return stack.GetSubmenuType<NMultiplayerSubmenu>();
    }

    private static PlatformType GetVanillaMultiplayerPlatform() =>
        SteamInitializer.Initialized && !CommandLineHelper.HasArg("fastmp")
            ? PlatformType.Steam
            : PlatformType.None;
}
