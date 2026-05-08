# STS2 Hook Discovery

## Purpose

This note captures the focused reflection output used to plan UI and Harmony hooks for multiplayer save slots. It is intentionally curated from the inspector output instead of preserving the full raw dump, which includes thousands of unrelated generated serializer and Godot bridge members.

## Generation

Command used to generate the raw source output:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tools/Sts2Inspector/Sts2Inspector.csproj -- "$HOME/.local/share/Steam/steamapps/common/Slay the Spire 2/data_sts2_linuxbsd_x86_64/sts2.dll" > /tmp/sts2-hook-discovery-check.md
```

STS2 dll inspected:

```text
$HOME/.local/share/Steam/steamapps/common/Slay the Spire 2/data_sts2_linuxbsd_x86_64/sts2.dll
```

Inspector filter terms:

```text
Multiplayer, Save, RunSave, Lobby, HostSubmenu, LoadGame
```

## Prioritized Candidates

1. `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerSubmenu`
   - Main multiplayer menu surface.
   - Contains `StartLoad`, `StartHost`, `StartHostAsync`, `TryAbandonMultiplayerRun`, and load/host/join/abandon button fields.
   - Likely UI entry point for adding save slot selection before loading or hosting a saved multiplayer run.

2. `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerHostSubmenu`
   - Host submenu for standard/custom/daily multiplayer run creation.
   - Contains `StartHost` and `StartHostAsync`.
   - Candidate hook for assigning a campaign slot before new multiplayer host flow begins.

3. `MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NMultiplayerLoadGameScreen`
   - Loaded multiplayer run lobby screen.
   - Contains `InitializeAsHost`, `InitializeAsClient`, `BeginRun`, `StartRun`, and `CleanUpLobby`.
   - Candidate for confirming selected campaign metadata and synchronizing loaded-run state.

4. `MegaCrit.Sts2.Core.Saves.Managers.RunSaveManager`
   - Lower-level current run save manager.
   - Contains direct multiplayer save operations: `LoadMultiplayerRunSave`, `LoadAndCanonicalizeMultiplayerRunSave`, `DeleteCurrentMultiplayerRun`, and `CurrentMultiplayerRunSavePath`.
   - Candidate for redirecting STS2's single current multiplayer run save file to a selected campaign slot.

5. `MegaCrit.Sts2.Core.Saves.SaveManager`
   - Facade over save managers and singleton access point.
   - Exposes `HasMultiplayerRunSave`, `DeleteCurrentMultiplayerRun`, and load/save orchestration.
   - Candidate hook point if UI code uses the facade instead of `RunSaveManager` directly.

6. Lobby flow types:
   - `MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby`
   - `MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby`
   - `MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.RunLobby`
   - These represent active start/load lobby state after menu selection. They are useful for observing state but are probably later-stage hooks than save path and menu entry points.

## Selected Detailed Output

### `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerSubmenu`

```text
## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerSubmenu
- method `AbandonRun(MegaCrit.Sts2.Core.Nodes.GodotExtensions.NButton _)` -> `System.Void`
- method `Create()` -> `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerSubmenu`
- method `FastHost(MegaCrit.Sts2.Core.Runs.GameMode gameMode)` -> `System.Void`
- method `OnHostPressed(MegaCrit.Sts2.Core.Nodes.GodotExtensions.NButton _)` -> `System.Void`
- method `OnJoinFriendsPressed()` -> `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NJoinFriendScreen`
- method `OpenJoinFriendsScreen(MegaCrit.Sts2.Core.Nodes.GodotExtensions.NButton _)` -> `System.Void`
- method `StartHost(MegaCrit.Sts2.Core.Saves.SerializableRun run)` -> `System.Void`
- method `StartHostAsync(MegaCrit.Sts2.Core.Saves.SerializableRun run)` -> `System.Threading.Tasks.Task`
- method `StartLoad(MegaCrit.Sts2.Core.Nodes.GodotExtensions.NButton _)` -> `System.Void`
- method `TryAbandonMultiplayerRun()` -> `System.Threading.Tasks.Task`
- method `UpdateButtons()` -> `System.Void`
- field `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSubmenuButton _abandonButton`
- field `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSubmenuButton _hostButton`
- field `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSubmenuButton _joinButton`
- field `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSubmenuButton _loadButton`
```

### `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerHostSubmenu`

```text
## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerHostSubmenu
- method `Create()` -> `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerHostSubmenu`
- method `OnCustomPressed(MegaCrit.Sts2.Core.Nodes.GodotExtensions.NButton _)` -> `System.Void`
- method `OnDailyPressed(MegaCrit.Sts2.Core.Nodes.GodotExtensions.NButton _)` -> `System.Void`
- method `OnStandardPressed(MegaCrit.Sts2.Core.Nodes.GodotExtensions.NButton _)` -> `System.Void`
- method `OnSubmenuOpened()` -> `System.Void`
- method `RefreshButtons()` -> `System.Void`
- method `StartHost(MegaCrit.Sts2.Core.Runs.GameMode gameMode)` -> `System.Void`
- method `StartHostAsync(MegaCrit.Sts2.Core.Runs.GameMode gameMode, Godot.Control loadingOverlay, MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSubmenuStack stack)` -> `System.Threading.Tasks.Task`
- field `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSubmenuButton _customButton`
- field `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSubmenuButton _dailyButton`
- field `Godot.Control _loadingOverlay`
- field `MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSubmenuButton _standardButton`
```

### `MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NMultiplayerLoadGameScreen`

```text
## MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NMultiplayerLoadGameScreen
- method `AfterMultiplayerStarted()` -> `System.Void`
- method `BeginRun()` -> `System.Void`
- method `CleanUpLobby(System.Boolean disconnectSession)` -> `System.Void`
- method `Create()` -> `MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NMultiplayerLoadGameScreen`
- method `InitializeAsClient(MegaCrit.Sts2.Core.Multiplayer.Game.INetGameService gameService, MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientLoadJoinResponseMessage message)` -> `System.Void`
- method `InitializeAsHost(MegaCrit.Sts2.Core.Multiplayer.Game.INetGameService gameService, MegaCrit.Sts2.Core.Saves.SerializableRun run)` -> `System.Void`
- method `LocalPlayerDisconnected(MegaCrit.Sts2.Core.Entities.Multiplayer.NetErrorInfo info)` -> `System.Void`
- method `OnEmbarkPressed(MegaCrit.Sts2.Core.Nodes.GodotExtensions.NButton _)` -> `System.Void`
- method `OnUnreadyPressed(MegaCrit.Sts2.Core.Nodes.GodotExtensions.NButton _)` -> `System.Void`
- method `PlayerConnected(System.UInt64 playerId)` -> `System.Void`
- method `PlayerReadyChanged(System.UInt64 playerId)` -> `System.Void`
- method `RemotePlayerDisconnected(System.UInt64 playerId)` -> `System.Void`
- method `ShouldAllowRunToBegin()` -> `System.Threading.Tasks.Task<System.Boolean>`
- method `StartRun()` -> `System.Threading.Tasks.Task`
- method `UpdateRichPresence()` -> `System.Void`
```

### `MegaCrit.Sts2.Core.Saves.Managers.RunSaveManager`

```text
## MegaCrit.Sts2.Core.Saves.Managers.RunSaveManager
- method `DeleteCurrentMultiplayerRun()` -> `System.Void`
- method `DeleteCurrentRun()` -> `System.Void`
- method `get_CurrentMultiplayerRunSavePath()` -> `System.String`
- method `get_CurrentRunSavePath()` -> `System.String`
- method `get_HasMultiplayerRunSave()` -> `System.Boolean`
- method `get_HasRunSave()` -> `System.Boolean`
- method `GetRunSavePath(System.Int32 profileId, System.String fileName)` -> `System.String`
- method `LoadAndCanonicalizeMultiplayerRunSave(System.UInt64 localPlayerId)` -> `MegaCrit.Sts2.Core.Saves.ReadSaveResult<MegaCrit.Sts2.Core.Saves.SerializableRun>`
- method `LoadMultiplayerRunSave()` -> `MegaCrit.Sts2.Core.Saves.ReadSaveResult<MegaCrit.Sts2.Core.Saves.SerializableRun>`
- method `LoadRunSave()` -> `MegaCrit.Sts2.Core.Saves.ReadSaveResult<MegaCrit.Sts2.Core.Saves.SerializableRun>`
- method `RenameBrokenMultiplayerRunSave(MegaCrit.Sts2.Core.Saves.ReadSaveStatus status)` -> `System.Void`
- method `SaveRun(MegaCrit.Sts2.Core.Rooms.AbstractRoom preFinishedRoom)` -> `System.Threading.Tasks.Task`
- field `System.String multiplayerRunSaveFileName`
- field `System.String runSaveFileName`
```

### `MegaCrit.Sts2.Core.Saves.SaveManager`

```text
## MegaCrit.Sts2.Core.Saves.SaveManager
- method `BeginSaveBatch()` -> `MegaCrit.Sts2.Core.Saves.SaveBatchScope`
- method `CleanupStaleCurrentRunSaveForProfile(System.Int32 profileId, System.String runSaveFileName)` -> `System.Void`
- method `CleanupStaleCurrentRunSaves()` -> `System.Void`
- method `ConstructDefault()` -> `MegaCrit.Sts2.Core.Saves.SaveManager`
- method `DeleteCurrentMultiplayerRun()` -> `System.Void`
- method `DeleteCurrentRun()` -> `System.Void`
- method `ExtractStartTimeFromRunSave(System.String json)` -> `System.Nullable<System.Int64>`
- method `FromJson(System.String json)` -> `MegaCrit.Sts2.Core.Saves.ReadSaveResult<T>`
- method `get_CurrentProfileId()` -> `System.Int32`
- method `get_CurrentRunSaveTask()` -> `System.Threading.Tasks.Task`
- method `get_HasMultiplayerRunSave()` -> `System.Boolean`
- method `get_HasRunSave()` -> `System.Boolean`
- method `get_Instance()` -> `MegaCrit.Sts2.Core.Saves.SaveManager`
- method `LoadAndCanonicalizeMultiplayerRunSave(System.UInt64 localPlayerId)` -> `MegaCrit.Sts2.Core.Saves.ReadSaveResult<MegaCrit.Sts2.Core.Saves.SerializableRun>`
- method `LoadRunSave()` -> `MegaCrit.Sts2.Core.Saves.ReadSaveResult<MegaCrit.Sts2.Core.Saves.SerializableRun>`
- method `SaveRun(MegaCrit.Sts2.Core.Rooms.AbstractRoom preFinishedRoom)` -> `System.Threading.Tasks.Task`
- field `MegaCrit.Sts2.Core.Saves.Managers.RunSaveManager _runSaveManager`
- field `System.Threading.Tasks.Task <CurrentRunSaveTask>k__BackingField`
```

### `MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby`

```text
## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby
- method `AddLocalHostPlayer(MegaCrit.Sts2.Core.Unlocks.UnlockState unlocks, System.Int32 maxMultiplayerAscension)` -> `System.Nullable<MegaCrit.Sts2.Core.Entities.Multiplayer.LobbyPlayer>`
- method `BeginRunForAllPlayers(System.String seed, System.Collections.Generic.List<MegaCrit.Sts2.Core.Models.ModifierModel> modifiers)` -> `System.Void`
- method `BeginRunIfAllPlayersReady()` -> `System.Void`
- method `BeginRunLocally(System.String seed, System.Collections.Generic.List<MegaCrit.Sts2.Core.Models.ModifierModel> modifiers)` -> `System.Void`
- method `CleanUp(System.Boolean disconnectSession)` -> `System.Void`
- method `get_LobbyListener()` -> `MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.IStartRunLobbyListener`
- method `get_NetService()` -> `MegaCrit.Sts2.Core.Multiplayer.Game.INetGameService`
- method `get_Players()` -> `System.Collections.Generic.List<MegaCrit.Sts2.Core.Entities.Multiplayer.LobbyPlayer>`
- method `HandleClientLoadJoinRequestMessage(MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientLoadJoinRequestMessage _, System.UInt64 senderId)` -> `System.Void`
- method `HandleClientLobbyJoinRequestMessage(MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientLobbyJoinRequestMessage message, System.UInt64 senderId)` -> `System.Void`
- method `HandleLobbyBeginRunMessage(MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyBeginRunMessage message, System.UInt64 senderId)` -> `System.Void`
- method `InitializeFromMessage(MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientLobbyJoinResponseMessage message)` -> `System.Void`
```

### `MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby`

```text
## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby
- method `AddLocalHostPlayer()` -> `System.Void`
- method `BeginRunIfAllPlayersReady()` -> `System.Void`
- method `CleanUp(System.Boolean disconnectSession)` -> `System.Void`
- method `get_ConnectedPlayerIds()` -> `System.Collections.Generic.HashSet<System.UInt64>`
- method `get_LobbyListener()` -> `MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.ILoadRunLobbyListener`
- method `get_NetService()` -> `MegaCrit.Sts2.Core.Multiplayer.Game.INetGameService`
- method `get_Run()` -> `MegaCrit.Sts2.Core.Saves.SerializableRun`
- method `HandleClientLoadJoinRequestMessage(MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientLoadJoinRequestMessage message, System.UInt64 senderId)` -> `System.Void`
- method `HandleLobbyBeginRunMessage(MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyBeginLoadedRunMessage message, System.UInt64 senderId)` -> `System.Void`
- method `HandlePlayerReadyMessage(MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyPlayerSetReadyMessage message, System.UInt64 senderId)` -> `System.Void`
- method `IsAboutToBeginGame()` -> `System.Boolean`
- method `IsPlayerReady(System.UInt64 playerId)` -> `System.Boolean`
- method `SetReady(System.Boolean ready)` -> `System.Void`
- method `TryBeginRun()` -> `System.Threading.Tasks.Task`
```

## Phase 2 Hook Selection

Phase 2 uses a postfix on `NMultiplayerSubmenu.UpdateButtons()` to keep the Host button visible even when vanilla detects a current multiplayer save.

Phase 2 uses a prefix on `NMultiplayerHostSubmenu.StartHost(GameMode)` as the picker insertion point. This method runs after Standard/Daily/Custom has been selected and before `StartHostAsync` starts the vanilla networking flow.

Existing campaign selections activate the campaign payload into `current_run_mp.save`, then call the vanilla loaded-run continuation through `NMultiplayerSubmenu.StartHost(SerializableRun)` where possible.
