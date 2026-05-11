using HarmonyLib;
using Godot;
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
using System.Globalization;
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
        _bank.ListCampaigns(gameMode)
            .Select(TryRepairPayloadMetadata)
            .ToList();

    public IReadOnlyList<ArchivedCampaign> ListArchivedCampaigns(MultiplayerGameMode gameMode) =>
        _bank.ListArchivedCampaigns(gameMode);

    public bool HasDeletedCampaigns() => _bank.HasDeletedCampaigns();

    public void ArchiveCampaign(string campaignId, DateTimeOffset deletedAtUtc) =>
        _bank.ArchiveCampaign(campaignId, deletedAtUtc);

    public CampaignMetadata RestoreArchivedCampaign(string archiveKey) =>
        _bank.RestoreArchivedCampaign(archiveKey);

    public CampaignMetadata RenameCampaign(string campaignId, string? customName) =>
        _bank.RenameCampaign(campaignId, customName);

    public void DeleteCampaign(string campaignId) =>
        _bank.DeleteCampaign(campaignId);

    public void DeleteArchivedCampaign(string archiveKey) =>
        _bank.DeleteArchivedCampaign(archiveKey);

    public void ClearDeletedCampaigns() =>
        _bank.ClearDeletedCampaigns();

    private CampaignMetadata TryRepairPayloadMetadata(CampaignMetadata metadata)
    {
        try
        {
            var payloadMetadata = ReadPayloadMetadata(metadata);
            var repairedRoster = metadata.Roster;
            var rosterChanged = false;
            if (metadata.Roster.Any(player => string.IsNullOrWhiteSpace(player.SelectedCharacterId)))
                repairedRoster = CampaignRosterMerger.MergeByStableId(metadata.Roster, payloadMetadata.Roster, out rosterChanged);

            var repairedProgress = string.IsNullOrWhiteSpace(payloadMetadata.ActOrFloor)
                ? metadata.ActOrFloor
                : payloadMetadata.ActOrFloor.Trim();
            var progressChanged = repairedProgress != metadata.ActOrFloor;
            if (!rosterChanged && !progressChanged)
                return metadata;

            var repaired = metadata with
            {
                Label = CampaignLabeler.Build(repairedRoster),
                Roster = repairedRoster,
                ActOrFloor = repairedProgress
            };
            _bank.UpdateMetadata(repaired);
            return repaired;
        }
        catch (Exception ex) when (ex is IOException or JsonException or InvalidOperationException or ArgumentException)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to repair picker metadata for {metadata.CampaignId}: {ex.Message}");
            return metadata;
        }
    }

    private PayloadMetadata ReadPayloadMetadata(CampaignMetadata metadata)
    {
        var payloadPath = _bank.GetPayloadPath(metadata.CampaignId);
        if (!File.Exists(payloadPath))
            return new PayloadMetadata([], null);

        using var document = JsonDocument.Parse(File.ReadAllText(payloadPath));
        var root = document.RootElement;
        var actOrFloor = ReadPayloadProgress(root);
        if (!root.TryGetProperty("players", out var players) || players.ValueKind != JsonValueKind.Array)
            return new PayloadMetadata([], actOrFloor);

        var platformPrefix = PlatformPrefix(root, metadata.Roster);
        if (platformPrefix is null)
            return new PayloadMetadata([], actOrFloor);

        var roster = new List<PlayerIdentity>();
        foreach (var player in players.EnumerateArray())
        {
            if (!TryReadNetId(player, out var netId) ||
                !TryReadString(player, "character_id", out var characterId))
            {
                continue;
            }

            roster.Add(new PlayerIdentity($"{platformPrefix}:{netId}", string.Empty, characterId));
        }

        return new PayloadMetadata(roster, actOrFloor);
    }

    private static string? ReadPayloadProgress(JsonElement root)
    {
        var currentActIndex = TryReadInt(root, "current_act_index", out var snakeAct)
            ? snakeAct
            : TryReadInt(root, "CurrentActIndex", out var pascalAct)
                ? pascalAct
                : -1;
        var completedFloorCount = TryReadMapPointHistoryCount(root, "map_point_history", out var snakeFloors)
            ? snakeFloors
            : TryReadMapPointHistoryCount(root, "MapPointHistory", out var pascalFloors)
                ? pascalFloors
                : 0;

        return RunProgressLabeler.Build(currentActIndex, completedFloorCount);
    }

    private static string? PlatformPrefix(JsonElement root, IReadOnlyList<PlayerIdentity> existingRoster)
    {
        if (TryReadString(root, "platform_type", out var platformType))
        {
            return platformType.Trim().ToLowerInvariant() switch
            {
                "steam" => "Steam",
                "none" => "None",
                _ => null
            };
        }

        foreach (var stableId in existingRoster.Select(player => player.StableId))
        {
            if (string.IsNullOrWhiteSpace(stableId))
                continue;

            var separator = stableId.IndexOf(':');
            if (separator > 0)
                return stableId[..separator];
        }

        return null;
    }

    private static bool TryReadNetId(JsonElement player, out string netId)
    {
        netId = string.Empty;
        if (!player.TryGetProperty("net_id", out var value))
            return false;

        switch (value.ValueKind)
        {
            case JsonValueKind.Number when value.TryGetUInt64(out var numeric):
                netId = numeric.ToString(CultureInfo.InvariantCulture);
                return true;
            case JsonValueKind.String:
                var text = value.GetString();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    netId = text.Trim();
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    private static bool TryReadString(JsonElement element, string propertyName, out string value)
    {
        value = string.Empty;
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.String)
            return false;

        var text = property.GetString();
        if (string.IsNullOrWhiteSpace(text))
            return false;

        value = text.Trim();
        return true;
    }

    private static bool TryReadInt(JsonElement element, string propertyName, out int value)
    {
        value = 0;
        return element.TryGetProperty(propertyName, out var property) &&
            property.ValueKind == JsonValueKind.Number &&
            property.TryGetInt32(out value);
    }

    private static bool TryReadMapPointHistoryCount(JsonElement element, string propertyName, out int count)
    {
        count = 0;
        if (!element.TryGetProperty(propertyName, out var history) || history.ValueKind != JsonValueKind.Array)
            return false;

        foreach (var act in history.EnumerateArray())
        {
            if (act.ValueKind == JsonValueKind.Array)
                count += act.GetArrayLength();
        }

        return true;
    }

    private sealed record PayloadMetadata(IReadOnlyList<PlayerIdentity> Roster, string? ActOrFloor);
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
            _switcher.SyncBack(nowUtc, metadata.ActOrFloor, metadata.Roster);
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
            StoragePathGuard.EnsureSafeFilePath(_activeSavePath, "active save path");
            _switcher.EnsureCanUseRuntimePaths();
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
            new ActiveSaveReplacementGuard(
                paths.ActiveSavePath,
                paths.ActiveStatePath,
                campaignId => HasStoredCampaignPayload(bank, campaignId)),
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
        return MultiplayerSaveRuntimePaths.FromSts2ActiveSavePath(
            activePath,
            GetFullSaveStorePath,
            ProjectSettings.GlobalizePath);
    }

    private static string? GetFullSaveStorePath(string path)
    {
        var saveStore = Traverse.Create(SaveManager.Instance).Field("_saveStore").GetValue<ISaveStore>();
        return saveStore?.GetFullPath(path);
    }

    public static void ResumeVanillaStart(NMultiplayerHostSubmenu hostSubmenu, GameMode gameMode) =>
        VanillaStartContinuation(hostSubmenu, gameMode);

    public static void ShowPicker(HostFlowController controller, MultiplayerGameMode gameMode) =>
        MultiplayerSavePickerModal.Show(controller, gameMode);

    private static bool HasStoredCampaignPayload(MultiplayerSaveBank bank, string campaignId) =>
        File.Exists(bank.GetPayloadPath(campaignId));
}

public sealed class ActiveSaveReplacementGuard : IActiveSavePreflight
{
    private readonly string _activeSavePath;
    private readonly string _statePath;
    private readonly Func<string, bool>? _hasStoredCampaignPayload;

    public ActiveSaveReplacementGuard(
        string activeSavePath,
        string statePath,
        Func<string, bool>? hasStoredCampaignPayload = null)
    {
        _activeSavePath = activeSavePath;
        _statePath = statePath;
        _hasStoredCampaignPayload = hasStoredCampaignPayload;
    }

    public OperationResult EnsureActiveSaveCanBeReplaced()
    {
        try
        {
            StoragePathGuard.EnsureSafeFilePath(_activeSavePath, "active save path");
            StoragePathGuard.EnsureSafeFilePath(_statePath, "active save state path");
            if (!File.Exists(_activeSavePath))
                return OperationResult.Ok();

            if (!File.Exists(_statePath))
                return OperationResult.Fail("Current multiplayer save is not managed by Multiplayer Save Slots yet.");

            var state = JsonFile.Read<ActiveSaveState>(_statePath);
            var currentActiveChecksum = FileChecksum.Sha256(_activeSavePath);
            if (currentActiveChecksum == state.ActiveChecksumAfterActivation)
                return OperationResult.Ok();

            if (_hasStoredCampaignPayload is not null &&
                !_hasStoredCampaignPayload(state.CampaignId))
            {
                return OperationResult.Ok();
            }

            return OperationResult.Fail("Active save has unsynced changes");
        }
        catch (Exception ex) when (ex is IOException or JsonException or InvalidOperationException or ArgumentException)
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
