## MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.GetLogsConsoleCmd+<GetAllSaveFiles>d__20
- method `<>m__Finally1()` -> `Void`
- method `MoveNext()` -> `Boolean`
- method `System.Collections.Generic.IEnumerable<System.String>.GetEnumerator()` -> `IEnumerator`1`
- method `System.Collections.Generic.IEnumerator<System.String>.get_Current()` -> `String`
- method `System.Collections.IEnumerable.GetEnumerator()` -> `IEnumerator`
- method `System.Collections.IEnumerator.get_Current()` -> `Object`
- method `System.Collections.IEnumerator.Reset()` -> `Void`
- method `System.IDisposable.Dispose()` -> `Void`
- field `Int32 <>1__state`
- field `String <>2__current`
- field `String <>3__accountBasePath`
- field `IEnumerator`1 <>7__wrap1`
- field `Int32 <>l__initialThreadId`
- field `String accountBasePath`

## MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.MultiplayerConsoleCmd
- method `get_Args()` -> `String`
- method `get_CmdName()` -> `String`
- method `get_Description()` -> `String`
- method `get_IsNetworked()` -> `Boolean`
- method `Process(Player issuingPlayer, String[] args)` -> `CmdResult`

## MegaCrit.Sts2.Core.Entities.Cards.CardMultiplayerConstraint
- field `CardMultiplayerConstraint MultiplayerOnly`
- field `CardMultiplayerConstraint None`
- field `CardMultiplayerConstraint SingleplayerOnly`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Entities.Multiplayer.ActionSynchronizerCombatState
- field `ActionSynchronizerCombatState EndTurnPhaseOne`
- field `ActionSynchronizerCombatState NotInCombat`
- field `ActionSynchronizerCombatState NotPlayPhase`
- field `ActionSynchronizerCombatState PlayPhase`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Entities.Multiplayer.ConnectionFailureExtraInfo
- method `<Clone>$()` -> `ConnectionFailureExtraInfo`
- method `Equals(Object obj)` -> `Boolean`
- method `Equals(ConnectionFailureExtraInfo other)` -> `Boolean`
- method `get_EqualityContract()` -> `Type`
- method `GetHashCode()` -> `Int32`
- method `op_Equality(ConnectionFailureExtraInfo left, ConnectionFailureExtraInfo right)` -> `Boolean`
- method `op_Inequality(ConnectionFailureExtraInfo left, ConnectionFailureExtraInfo right)` -> `Boolean`
- method `PrintMembers(StringBuilder builder)` -> `Boolean`
- method `ToString()` -> `String`
- field `List`1 missingModsOnHost`
- field `List`1 missingModsOnLocal`

## MegaCrit.Sts2.Core.Entities.Multiplayer.ConnectionFailureReason
- field `ConnectionFailureReason LobbyFull`
- field `ConnectionFailureReason ModMismatch`
- field `ConnectionFailureReason None`
- field `ConnectionFailureReason NotInSaveGame`
- field `ConnectionFailureReason RunInProgress`
- field `Int32 value__`
- field `ConnectionFailureReason VersionMismatch`

## MegaCrit.Sts2.Core.Entities.Multiplayer.GameActionType
- field `GameActionType Any`
- field `GameActionType Combat`
- field `GameActionType CombatPlayPhaseOnly`
- field `GameActionType NonCombat`
- field `GameActionType None`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Entities.Multiplayer.LobbyPlayer
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `CharacterModel character`
- field `UInt64 id`
- field `Boolean isReady`
- field `Int32 maxMultiplayerAscensionUnlocked`
- field `Int32 slotId`
- field `SerializableUnlockState unlockState`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetChecksumData
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `UInt32 checksum`
- field `UInt32 id`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetClientData
- field `UInt64 peerId`
- field `Boolean readyForBroadcasting`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetCombatCard
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Equals(NetCombatCard other)` -> `Boolean`
- method `Equals(Object obj)` -> `Boolean`
- method `ForTesting(UInt32 index)` -> `NetCombatCard`
- method `FromModel(CardModel card)` -> `NetCombatCard`
- method `get_CombatCardIndex()` -> `UInt32`
- method `GetHashCode()` -> `Int32`
- method `op_Equality(NetCombatCard c1, NetCombatCard c2)` -> `Boolean`
- method `op_Inequality(NetCombatCard c1, NetCombatCard c2)` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_CombatCardIndex(UInt32 value)` -> `Void`
- method `ToCardModel()` -> `CardModel`
- method `ToCardModelOrNull()` -> `CardModel`
- method `ToString()` -> `String`
- field `UInt32 <CombatCardIndex>k__BackingField`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetDeckCard
- method `Deserialize(PacketReader reader)` -> `Void`
- method `FromModel(CardModel card)` -> `NetDeckCard`
- method `get_DeckIndex()` -> `UInt32`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_DeckIndex(UInt32 value)` -> `Void`
- method `ToCardModel(Player player)` -> `CardModel`
- method `ToString()` -> `String`
- field `UInt32 <DeckIndex>k__BackingField`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetError
- field `NetError CancelledJoin`
- field `NetError FailedToHost`
- field `NetError HandshakeTimeout`
- field `NetError HostAbandoned`
- field `NetError InternalError`
- field `NetError InvalidJoin`
- field `NetError JoinBlockedByUser`
- field `NetError Kicked`
- field `NetError LobbyFull`
- field `NetError ModMismatch`
- field `NetError NoInternet`
- field `NetError None`
- field `NetError NotInSaveGame`
- field `NetError Quit`
- field `NetError QuitGameOver`
- field `NetError RateLimited`
- field `NetError RunInProgress`
- field `NetError StateDivergence`
- field `NetError Timeout`
- field `NetError TryAgainLater`
- field `NetError UnknownNetworkError`
- field `Int32 value__`
- field `NetError VersionMismatch`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetErrorInfo
- method `get_SelfInitiated()` -> `Boolean`
- method `GetErrorString()` -> `String`
- method `GetReason()` -> `NetError`
- method `ToString()` -> `String`
- field `ConnectionFailureExtraInfo _connectionExtraInfo`
- field `Nullable`1 _connectionReason`
- field `String _debugReason`
- field `Nullable`1 _godotError`
- field `Nullable`1 _lobbyCreationResult`
- field `Nullable`1 _lobbyEnterResponse`
- field `Nullable`1 _reason`
- field `Nullable`1 _steamReason`
- field `Boolean <SelfInitiated>k__BackingField`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState
- method `Anonymized()` -> `NetFullCombatState`
- method `Deserialize(PacketReader reader)` -> `Void`
- method `FromRun(IRunState runState, GameAction justFinishedAction)` -> `NetFullCombatState`
- method `get_Creatures()` -> `List`1`
- method `get_Players()` -> `List`1`
- method `get_Rng()` -> `SerializableRunRngSet`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Creatures(List`1 value)` -> `Void`
- method `set_Players(List`1 value)` -> `Void`
- method `set_Rng(SerializableRunRngSet value)` -> `Void`
- method `ToString()` -> `String`
- field `List`1 <Creatures>k__BackingField`
- field `List`1 <Players>k__BackingField`
- field `SerializableRunRngSet <Rng>k__BackingField`
- field `Nullable`1 lastExecutedActionId`
- field `Nullable`1 lastExecutedHookId`
- field `List`1 nextChoiceIds`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState+<>c
- method `<Anonymized>b__26_0(CreatureState c)` -> `CreatureState`
- method `<Anonymized>b__26_1(PlayerState p)` -> `PlayerState`
- method `<ToString>b__27_0(ModelId m)` -> `String`
- field `<>c <>9`
- field `Func`2 <>9__26_0`
- field `Func`2 <>9__26_1`
- field `Func`2 <>9__27_0`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState+<>O
- field `Func`2 <0>__From`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState+CardState
- method `Deserialize(PacketReader reader)` -> `Void`
- method `From(CardModel card)` -> `CardState`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `ModelId affliction`
- field `Int32 afflictionCount`
- field `SerializableCard card`
- field `Nullable`1 energyCost`
- field `List`1 keywords`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState+CombatPileState
- method `Deserialize(PacketReader reader)` -> `Void`
- method `From(CardPile pile)` -> `CombatPileState`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `List`1 cards`
- field `PileType pileType`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState+CreatureState
- method `Anonymized()` -> `CreatureState`
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `Int32 block`
- field `Int32 currentHp`
- field `Int32 maxHp`
- field `ModelId monsterId`
- field `Nullable`1 playerId`
- field `List`1 powers`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState+OrbState
- method `Deserialize(PacketReader reader)` -> `Void`
- method `From(OrbModel orb)` -> `OrbState`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `Int32 evoke`
- field `ModelId id`
- field `Int32 passive`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState+PlayerState
- method `Anonymized()` -> `PlayerState`
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `ModelId characterId`
- field `Int32 energy`
- field `Int32 gold`
- field `Int32 maxPotionCount`
- field `Int32 maxStars`
- field `List`1 orbs`
- field `List`1 piles`
- field `UInt64 playerId`
- field `List`1 potions`
- field `SerializableRelicGrabBag relicGrabBag`
- field `List`1 relics`
- field `SerializablePlayerRngSet rngSet`
- field `Int32 stars`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState+PotionState
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `ModelId id`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState+PowerState
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `Int32 amount`
- field `ModelId id`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetFullCombatState+RelicState
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `SerializableRelic relic`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetPlayerChoiceResult
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `List`1 canonicalCards`
- field `List`1 combatCards`
- field `List`1 deckCards`
- field `List`1 indexes`
- field `Nullable`1 mutableCardOwner`
- field `List`1 mutableCards`
- field `Nullable`1 playerId`
- field `PlayerChoiceType type`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetScreenType
- field `NetScreenType CardPile`
- field `NetScreenType CardSelection`
- field `NetScreenType Compendium`
- field `NetScreenType DeckView`
- field `NetScreenType Feedback`
- field `NetScreenType GameOver`
- field `NetScreenType Map`
- field `NetScreenType None`
- field `NetScreenType PauseMenu`
- field `NetScreenType RemotePlayerExpandedState`
- field `NetScreenType Rewards`
- field `NetScreenType Room`
- field `NetScreenType Settings`
- field `NetScreenType SharedRelicPicking`
- field `NetScreenType SimpleCardsView`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Entities.Multiplayer.NetScreenTypeExtensions
- method `GetLocationIcon(NetScreenType screenType)` -> `Texture2D`

## MegaCrit.Sts2.Core.Entities.Multiplayer.PlayerChoiceOptions
- field `PlayerChoiceOptions CancelPlayCardActions`
- field `PlayerChoiceOptions None`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Entities.Multiplayer.ReactionType
- field `ReactionType Exclamation`
- field `ReactionType HappyCultist`
- field `ReactionType Heart`
- field `ReactionType None`
- field `ReactionType QuestionMark`
- field `ReactionType SadSlime`
- field `ReactionType Skull`
- field `ReactionType ThumbDown`
- field `ReactionType ThumbUp`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Entities.Multiplayer.RunSessionState
- field `RunSessionState InLoadedLobby`
- field `RunSessionState InLobby`
- field `RunSessionState None`
- field `RunSessionState Running`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Entities.UI.MultiplayerUiMode
- field `MultiplayerUiMode Client`
- field `MultiplayerUiMode Host`
- field `MultiplayerUiMode Load`
- field `MultiplayerUiMode None`
- field `MultiplayerUiMode Singleplayer`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Exceptions.SaveException

## MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionQueueSet
- method `ActionQueueIsPaused(UInt64 playerId)` -> `Boolean`
- method `add_ActionEnqueued(Action`1 value)` -> `Void`
- method `add_ActionQueueChanged(Action value)` -> `Void`
- method `add_ActionResumed(Action`1 value)` -> `Void`
- method `BecameEmpty()` -> `Task`
- method `CancelNonExecutingActionsForPlayer(UInt64 playerId)` -> `Void`
- method `CancelNonExecutingActionsOfType(UInt64 ownerId, Nullable`1 maxActionId)` -> `Void`
- method `CheckIfQueuesEmpty()` -> `Void`
- method `CombatEnded()` -> `Void`
- method `CombatStarted()` -> `Void`
- method `EnqueueWithoutSynchronizing(GameAction gameAction)` -> `Void`
- method `FastForwardNextActionId(UInt32 nextId)` -> `Void`
- method `get_IsEmpty()` -> `Boolean`
- method `get_NextActionId()` -> `UInt32`
- method `GetAndIncrementActionId()` -> `UInt32`
- method `GetQueue(UInt64 playerId)` -> `ActionQueue`
- method `GetReadyAction()` -> `GameAction`
- method `IsGameActionPlayerDriven(GameAction gameAction)` -> `Boolean`
- method `PauseActionForPlayerChoice(GameAction action, PlayerChoiceOptions options)` -> `Void`
- method `PauseAllPlayerQueues()` -> `Void`
- method `PopAction(GameAction action)` -> `Void`
- method `remove_ActionEnqueued(Action`1 value)` -> `Void`
- method `remove_ActionQueueChanged(Action value)` -> `Void`
- method `remove_ActionResumed(Action`1 value)` -> `Void`
- method `Reset()` -> `Void`
- method `ResumeActionWithoutSynchronizing(UInt32 id)` -> `Void`
- method `StartCancellingAllPlayerDrivenCombatActions()` -> `Void`
- method `TryGetAction(UInt32 id, GameAction& gameAction, ActionQueue& queue)` -> `Boolean`
- method `UnpauseAllPlayerQueues()` -> `Void`
- field `List`1 _actionQueues`
- field `List`1 _actionsWaitingForResumption`
- field `Boolean _isInCombat`
- field `Logger _logger`
- field `UInt32 _nextId`
- field `TaskCompletionSource _queuesEmptyCompletionSource`
- field `Action`1 ActionEnqueued`
- field `Action ActionQueueChanged`
- field `Action`1 ActionResumed`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionQueueSet+<>c
- method `<CheckIfQueuesEmpty>b__41_0(ActionQueue q)` -> `Boolean`
- field `<>c <>9`
- field `Func`2 <>9__41_0`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionQueueSet+<>c__DisplayClass38_0
- method `<GetQueue>b__0(ActionQueue q)` -> `Boolean`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionQueueSet+ActionQueue
- field `List`1 actions`
- field `Boolean isCancellingCombatActions`
- field `Boolean isCancellingPlayCardActions`
- field `Boolean isCancellingPlayerDrivenCombatActions`
- field `Boolean isPaused`
- field `UInt64 ownerId`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionQueueSet+ActionWaitingForResumption
- field `UInt32 newId`
- field `UInt32 oldId`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionQueueSynchronizer
- method `Dispose()` -> `Void`
- method `EnqueueAction(GameAction action, UInt64 actionOwnerId)` -> `Void`
- method `EnqueueHookAction(GenericHookGameAction gameAction)` -> `Void`
- method `FastForwardHookId(UInt32 hookId)` -> `Void`
- method `GenerateHookAction(UInt64 ownerId, GameActionType gameActionType)` -> `GenericHookGameAction`
- method `get_CombatState()` -> `ActionSynchronizerCombatState`
- method `get_NextHookId()` -> `UInt32`
- method `GetHookActionForId(UInt32 id, UInt64 ownerId, GameActionType gameActionType)` -> `GenericHookGameAction`
- method `HandleActionEnqueuedMessage(ActionEnqueuedMessage message, UInt64 _)` -> `Void`
- method `HandleHookActionEnqueuedMessage(HookActionEnqueuedMessage message, UInt64 _)` -> `Void`
- method `HandleRequestEnqueueActionMessage(RequestEnqueueActionMessage message, UInt64 senderId)` -> `Void`
- method `HandleRequestEnqueueHookActionMessage(RequestEnqueueHookActionMessage message, UInt64 senderId)` -> `Void`
- method `HandleRequestResumeActionAfterPlayerChoiceMessage(RequestResumeActionAfterPlayerChoiceMessage afterPlayerChoiceMessage, UInt64 senderId)` -> `Void`
- method `HandleResumeActionAfterPlayerChoiceMessage(ResumeActionAfterPlayerChoiceMessage afterPlayerChoiceMessage, UInt64 _)` -> `Void`
- method `HookActionStarted(GenericHookGameAction action)` -> `Void`
- method `NetActionToGameAction(INetAction action, UInt64 actionOwnerId)` -> `GameAction`
- method `RequestEnqueue(GameAction action)` -> `Void`
- method `RequestEnqueueHookAction(GenericHookGameAction action)` -> `Void`
- method `RequestResumeActionAfterPlayerChoice(GameAction action)` -> `Void`
- method `ResumeActionAfterPlayerChoice(UInt32 id)` -> `Void`
- method `set_CombatState(ActionSynchronizerCombatState value)` -> `Void`
- method `SetCombatState(ActionSynchronizerCombatState combatState)` -> `Void`
- field `ActionQueueSet _actionQueueSet`
- field `List`1 _hookActions`
- field `Logger _logger`
- field `RunLocationTargetedMessageBuffer _messageBuffer`
- field `INetGameService _netService`
- field `UInt32 _nextHookId`
- field `IPlayerCollection _playerCollection`
- field `List`1 _requestedActionsWaitingForPlayerTurn`
- field `ActionSynchronizerCombatState <CombatState>k__BackingField`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionQueueSynchronizer+<>c__DisplayClass31_0
- method `<GetHookActionForId>b__0(GenericHookGameAction a)` -> `Boolean`
- method `<GetHookActionForId>b__1(Task _)` -> `Void`
- field `ActionQueueSynchronizer <>4__this`
- field `GenericHookGameAction action`
- field `UInt32 id`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionTypes
- method `ToId(INetAction message)` -> `Int32`
- method `TryGetActionType(Int32 id, Type& type)` -> `Boolean`
- method `TypeToId()` -> `Int32`
- method `TypeToId(Type type)` -> `Int32`
- field `NetTypeCache`1 _cache`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.BlockingPlayerChoiceContext
- method `SignalPlayerChoiceBegun(PlayerChoiceOptions options)` -> `Task`
- method `SignalPlayerChoiceEnded()` -> `Task`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.GameActionPlayerChoiceContext
- method `get_Action()` -> `GameAction`
- method `SignalPlayerChoiceBegun(PlayerChoiceOptions options)` -> `Task`
- method `SignalPlayerChoiceEnded()` -> `Task`
- field `GameAction <Action>k__BackingField`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.GameActionPlayerChoiceContext+<SignalPlayerChoiceEnded>d__5
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `GameActionPlayerChoiceContext <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext
- method `AssignTaskAndWaitForPauseOrCompletion(Task task)` -> `Task`1`
- method `ExecuteTaskThenInvokeExecutionFinished(Task task)` -> `Task`
- method `get_ActionExecutor()` -> `ActionExecutor`
- method `get_ActionQueueSet()` -> `ActionQueueSet`
- method `get_ActionQueueSynchronizer()` -> `ActionQueueSynchronizer`
- method `get_GameAction()` -> `GenericHookGameAction`
- method `get_Owner()` -> `Player`
- method `get_Source()` -> `AbstractModel`
- method `get_Task()` -> `Task`
- method `MockDependenciesForTest(ActionQueueSynchronizer actionQueueSynchronizer, ActionQueueSet actionQueueSet, ActionExecutor actionExecutor)` -> `Void`
- method `set_Task(Task value)` -> `Void`
- method `SignalPlayerChoiceBegun(PlayerChoiceOptions options)` -> `Task`
- method `SignalPlayerChoiceEnded()` -> `Task`
- method `WaitForPauseOrCompletionWithoutAssigningTask(Task task)` -> `Task`1`
- field `ActionExecutor _actionExecutor`
- field `ActionQueueSet _actionQueueSet`
- field `ActionQueueSynchronizer _actionQueueSynchronizer`
- field `GenericHookGameAction _gameAction`
- field `GameActionType _gameActionType`
- field `UInt64 _localPlayerId`
- field `TaskCompletionSource _pausedCompletionSource`
- field `TaskCompletionSource _taskAssignedCompletionSource`
- field `Player <Owner>k__BackingField`
- field `AbstractModel <Source>k__BackingField`
- field `Task <Task>k__BackingField`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext+<AssignTaskAndWaitForPauseOrCompletion>d__29
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `HookPlayerChoiceContext <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter <>u__1`
- field `Task task`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext+<ExecuteTaskThenInvokeExecutionFinished>d__31
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `HookPlayerChoiceContext <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `Task task`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext+<SignalPlayerChoiceBegun>d__32
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `HookPlayerChoiceContext <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `PlayerChoiceOptions options`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext+<SignalPlayerChoiceEnded>d__33
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `HookPlayerChoiceContext <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext+<WaitForPauseOrCompletionWithoutAssigningTask>d__30
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `HookPlayerChoiceContext <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter <>u__1`
- field `Task task`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.INetAction
- method `ToGameAction(Player player)` -> `GameAction`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.INetActionSubtypes
- method `Get(Int32 i)` -> `Type`
- method `get_All()` -> `IReadOnlyList`1`
- method `get_Count()` -> `Int32`
- field `Type[] _subtypes`
- field `Type _t0`
- field `Type _t1`
- field `Type _t10`
- field `Type _t2`
- field `Type _t3`
- field `Type _t4`
- field `Type _t5`
- field `Type _t6`
- field `Type _t7`
- field `Type _t8`
- field `Type _t9`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.NetCombatCardDb
- method `ClearCardsForTesting()` -> `Void`
- method `get_Instance()` -> `NetCombatCardDb`
- method `GetCard(UInt32 id)` -> `CardModel`
- method `GetCardId(CardModel card)` -> `UInt32`
- method `IdCardForTesting(CardModel card)` -> `UInt32`
- method `IdCardIfNecessary(CardModel card)` -> `Void`
- method `OnCombatEnded(CombatRoom _)` -> `Void`
- method `OnPileContentsChanged(CardPile pile)` -> `Void`
- method `StartCombat(IReadOnlyList`1 players)` -> `Void`
- method `TryGetCard(UInt32 id, CardModel& card)` -> `Boolean`
- method `TryGetCardId(CardModel card, UInt32& id)` -> `Boolean`
- field `Dictionary`2 _cardToId`
- field `Dictionary`2 _idToCard`
- field `UInt32 _nextId`
- field `List`1 _subscriptions`
- field `NetCombatCardDb <Instance>k__BackingField`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.NetCombatCardDb+<>c
- method `<StartCombat>b__8_0(CardPile p)` -> `IEnumerable`1`
- field `<>c <>9`
- field `Func`2 <>9__8_0`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.NetCombatCardDb+<>c__DisplayClass8_0
- method `<StartCombat>b__1()` -> `Void`
- field `NetCombatCardDb <>4__this`
- field `Subscription subscription`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.NetCombatCardDb+Subscription
- field `Action action`
- field `CardPile pile`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext
- method `get_LastInvolvedModel()` -> `AbstractModel`
- method `PopModel(AbstractModel model)` -> `Void`
- method `PushModel(AbstractModel model)` -> `Void`
- method `SignalPlayerChoiceBegun(PlayerChoiceOptions options)` -> `Task`
- method `SignalPlayerChoiceEnded()` -> `Task`
- field `Stack`1 _modelStack`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContinuation
- method `BeginInvoke(PlayerChoiceResult result, AsyncCallback callback, Object object)` -> `IAsyncResult`
- method `EndInvoke(IAsyncResult result)` -> `Task`
- method `Invoke(PlayerChoiceResult result)` -> `Task`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceDelegate
- method `BeginInvoke(AsyncCallback callback, Object object)` -> `IAsyncResult`
- method `EndInvoke(IAsyncResult result)` -> `Task`1`
- method `Invoke()` -> `Task`1`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceSynchronizer
- method `add_PlayerChoiceReceived(Action`3 value)` -> `Void`
- method `Dispose()` -> `Void`
- method `FastForwardChoiceIds(List`1 choiceIds)` -> `Void`
- method `get_ChoiceIds()` -> `IReadOnlyList`1`
- method `GetChoiceId(Player player)` -> `UInt32`
- method `OnPlayerChoiceMessageReceived(PlayerChoiceMessage message, UInt64 senderId)` -> `Void`
- method `OnReceivePlayerChoice(Player player, UInt32 choiceId, NetPlayerChoiceResult result)` -> `Void`
- method `ReceiveReplayChoice(Player player, UInt32 choiceId, NetPlayerChoiceResult result)` -> `Void`
- method `remove_PlayerChoiceReceived(Action`3 value)` -> `Void`
- method `ReserveChoiceId(Player player)` -> `UInt32`
- method `SyncLocalChoice(Player player, UInt32 choiceId, PlayerChoiceResult result)` -> `Void`
- method `ValidateChoiceId(Player player, UInt32 choiceId)` -> `Boolean`
- method `WaitForRemoteChoice(Player player, UInt32 choiceId)` -> `Task`1`
- field `List`1 _choiceIds`
- field `Logger _logger`
- field `INetGameService _netService`
- field `IPlayerCollection _players`
- field `List`1 _receivedChoices`
- field `Action`3 PlayerChoiceReceived`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceSynchronizer+<>c__DisplayClass15_0
- method `<WaitForRemoteChoice>b__0(ReceivedChoice c)` -> `Boolean`
- field `UInt32 choiceId`
- field `Player player`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceSynchronizer+<>c__DisplayClass19_0
- method `<OnReceivePlayerChoice>b__0(ReceivedChoice c)` -> `Boolean`
- field `UInt32 choiceId`
- field `Player player`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceSynchronizer+<WaitForRemoteChoice>d__15
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `PlayerChoiceSynchronizer <>4__this`
- field `<>c__DisplayClass15_0 <>8__1`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `UInt32 choiceId`
- field `Player player`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceSynchronizer+ReceivedChoice
- field `UInt32 choiceId`
- field `TaskCompletionSource`1 completionSource`
- field `UInt64 senderId`

## MegaCrit.Sts2.Core.GameActions.Multiplayer.ThrowingPlayerChoiceContext
- method `SignalPlayerChoiceBegun(PlayerChoiceOptions options)` -> `Task`
- method `SignalPlayerChoiceEnded()` -> `Task`

## MegaCrit.Sts2.Core.Map.SavedActMap
- method `CreatePoint(SerializableMapPoint saved)` -> `MapPoint`
- method `get_BossMapPoint()` -> `MapPoint`
- method `get_Grid()` -> `MapPoint[,]`
- method `get_SecondBossMapPoint()` -> `MapPoint`
- method `get_StartingMapPoint()` -> `MapPoint`
- method `WireChildren(IEnumerable`1 points, Dictionary`2 lookup)` -> `Void`
- method `WireChildren(SerializableMapPoint savedPoint, Dictionary`2 lookup)` -> `Void`
- field `MapPoint <BossMapPoint>k__BackingField`
- field `MapPoint[,] <Grid>k__BackingField`
- field `MapPoint <SecondBossMapPoint>k__BackingField`
- field `MapPoint <StartingMapPoint>k__BackingField`

## MegaCrit.Sts2.Core.Modding.SettingsSaveMod
- method `get_Id()` -> `String`
- method `get_IsEnabled()` -> `Boolean`
- method `get_Source()` -> `ModSource`
- method `set_Id(String value)` -> `Void`
- method `set_IsEnabled(Boolean value)` -> `Void`
- method `set_Source(ModSource value)` -> `Void`
- field `String <Id>k__BackingField`
- field `Boolean <IsEnabled>k__BackingField`
- field `ModSource <Source>k__BackingField`

## MegaCrit.Sts2.Core.Models.Singleton.MultiplayerScalingModel
- method `get_ShouldReceiveCombatHooks()` -> `Boolean`
- method `GetMultiplayerScaling(EncounterModel encounter, Int32 actIndex)` -> `Decimal`
- method `Initialize(RunState state)` -> `Void`
- method `ModifyBlockMultiplicative(Creature target, Decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)` -> `Decimal`
- method `ModifyPowerAmountGiven(PowerModel power, Creature giver, Decimal amount, Creature target, CardModel cardSource)` -> `Decimal`
- method `OnCombatEntered(CombatState combatState)` -> `Void`
- method `OnCombatFinished()` -> `Void`
- field `CombatState _combatState`
- field `RunState _runState`

## MegaCrit.Sts2.Core.Multiplayer.CombatStateSynchronizer
- method `CheckSyncCompleted()` -> `Void`
- method `Dispose()` -> `Void`
- method `get_IsDisabled()` -> `Boolean`
- method `OnPeerDisconnected(UInt64 peerId)` -> `Void`
- method `OnSyncPlayerMessageReceived(SyncPlayerDataMessage syncMessage, UInt64 senderId)` -> `Void`
- method `OnSyncRngMessageReceived(SyncRngMessage syncMessage, UInt64 senderId)` -> `Void`
- method `set_IsDisabled(Boolean value)` -> `Void`
- method `StartSync()` -> `Void`
- method `WaitForSync()` -> `Task`
- field `Logger _logger`
- field `INetGameService _netService`
- field `SerializableRunRngSet _rngSet`
- field `RunLobby _runLobby`
- field `RunState _runState`
- field `SerializableRelicGrabBag _sharedRelicGrabBag`
- field `TaskCompletionSource _syncCompletionSource`
- field `Dictionary`2 _syncData`
- field `Boolean <IsDisabled>k__BackingField`

## MegaCrit.Sts2.Core.Multiplayer.CombatStateSynchronizer+<WaitForSync>d__18
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `CombatStateSynchronizer <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`

## MegaCrit.Sts2.Core.Multiplayer.Connection.ClientConnectionFailedException
- field `NetErrorInfo info`

## MegaCrit.Sts2.Core.Multiplayer.Connection.ENetClientConnectionInitializer
- method `Connect(NetClientGameService gameService, CancellationToken cancelToken)` -> `Task`1`
- field `String _ip`
- field `UInt64 _netId`
- field `UInt16 _port`

## MegaCrit.Sts2.Core.Multiplayer.Connection.ENetClientConnectionInitializer+<Connect>d__4
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `ENetClientConnectionInitializer <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `CancellationToken cancelToken`
- field `NetClientGameService gameService`

## MegaCrit.Sts2.Core.Multiplayer.Connection.IClientConnectionInitializer
- method `Connect(NetClientGameService gameService, CancellationToken cancelToken)` -> `Task`1`

## MegaCrit.Sts2.Core.Multiplayer.Connection.SteamClientConnectionInitializer
- method `Connect(NetClientGameService gameService, CancellationToken cancelToken)` -> `Task`1`
- method `FromLobby(UInt64 lobbySteamId)` -> `SteamClientConnectionInitializer`
- method `FromPlayer(UInt64 playerSteamId)` -> `SteamClientConnectionInitializer`
- method `ToString()` -> `String`
- field `Nullable`1 _lobbySteamId`
- field `Nullable`1 _playerSteamId`

## MegaCrit.Sts2.Core.Multiplayer.Connection.SteamClientConnectionInitializer+<Connect>d__4
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `SteamClientConnectionInitializer <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `CancellationToken cancelToken`
- field `NetClientGameService gameService`

## MegaCrit.Sts2.Core.Multiplayer.Game.ActChangeSynchronizer
- method `IsWaitingForOtherPlayers()` -> `Boolean`
- method `MoveToNextAct()` -> `Void`
- method `OnPlayerReady(Player player)` -> `Void`
- method `SetLocalPlayerReady()` -> `Void`
- field `Logger _logger`
- field `List`1 _readyPlayers`
- field `RunState _runState`

## MegaCrit.Sts2.Core.Multiplayer.Game.ActChangeSynchronizer+<>c
- method `<OnPlayerReady>b__6_0(Boolean x)` -> `Boolean`
- field `<>c <>9`
- field `Func`2 <>9__6_0`

## MegaCrit.Sts2.Core.Multiplayer.Game.ChecksumTracker
- method `add_ChecksumGenerated(Action`3 value)` -> `Void`
- method `add_StateDiverged(Action`1 value)` -> `Void`
- method `CheckAgainstReplayChecksum(NetChecksumData localData, String context)` -> `Void`
- method `CompareChecksums(TrackedChecksum localChecksum, NetChecksumData remoteChecksum, UInt64 remoteId)` -> `Void`
- method `Dispose()` -> `Void`
- method `GenerateChecksum(String context, GameAction action)` -> `NetChecksumData`
- method `GenerateChecksum(NetFullCombatState state)` -> `UInt32`
- method `get_IsEnabled()` -> `Boolean`
- method `get_NextId()` -> `UInt32`
- method `LoadReplayChecksums(List`1 replayChecksums, UInt32 nextId)` -> `Void`
- method `LogStateDivergence(TrackedChecksum localChecksum, StateDivergenceMessage message, UInt64 remoteId, Int32 checksumIndex)` -> `Void`
- method `ObtainAndTrackChecksum(String context, GameAction action)` -> `NetChecksumData`
- method `OnReceivedChecksumDataMessage(ChecksumDataMessage message, UInt64 senderId)` -> `Void`
- method `OnReceivedStateDivergenceMessage(StateDivergenceMessage message, UInt64 senderId)` -> `Void`
- method `remove_ChecksumGenerated(Action`3 value)` -> `Void`
- method `remove_StateDiverged(Action`1 value)` -> `Void`
- method `ReportDivergenceToSentry(TrackedChecksum localChecksum, StateDivergenceMessage message, UInt64 remoteId, Int32 checksumIndex)` -> `Void`
- method `set_IsEnabled(Boolean value)` -> `Void`
- method `set_NextId(UInt32 value)` -> `Void`
- field `List`1 _checksums`
- field `Int32 _checksumsToSave`
- field `Logger _logger`
- field `INetGameService _netService`
- field `PacketWriter _packetWriter`
- field `List`1 _queuedRemoteChecksums`
- field `List`1 _replayChecksums`
- field `IRunState _runState`
- field `Boolean <IsEnabled>k__BackingField`
- field `UInt32 <NextId>k__BackingField`
- field `Action`3 ChecksumGenerated`
- field `Action`1 StateDiverged`

## MegaCrit.Sts2.Core.Multiplayer.Game.ChecksumTracker+<>c__DisplayClass27_0
- method `<OnReceivedChecksumDataMessage>b__0(TrackedChecksum c)` -> `Boolean`
- field `NetChecksumData remoteChecksumData`

## MegaCrit.Sts2.Core.Multiplayer.Game.ChecksumTracker+<>c__DisplayClass28_0
- method `<OnReceivedStateDivergenceMessage>b__0(TrackedChecksum c)` -> `Boolean`
- field `NetChecksumData remoteChecksumData`

## MegaCrit.Sts2.Core.Multiplayer.Game.ChecksumTracker+<>c__DisplayClass32_0
- method `<ReportDivergenceToSentry>b__0(Scope scope)` -> `Void`
- field `ChecksumTracker <>4__this`
- field `Int32 checksumIndex`
- field `TrackedChecksum localChecksum`
- field `String localState`
- field `StateDivergenceMessage message`
- field `UInt64 remoteId`
- field `String remoteState`
- field `String role`

## MegaCrit.Sts2.Core.Multiplayer.Game.ChecksumTracker+<>c__DisplayClass34_0
- method `<CheckAgainstReplayChecksum>b__0(ReplayChecksumData c)` -> `Boolean`
- method `<CheckAgainstReplayChecksum>b__1(TrackedChecksum c)` -> `Boolean`
- field `NetChecksumData localData`

## MegaCrit.Sts2.Core.Multiplayer.Game.ChecksumTracker+QueuedRemoteChecksum
- field `NetChecksumData data`
- field `UInt64 senderId`

## MegaCrit.Sts2.Core.Multiplayer.Game.ChecksumTracker+TrackedChecksum
- field `String context`
- field `NetChecksumData data`
- field `String fingerprintContext`
- field `NetFullCombatState fullState`

## MegaCrit.Sts2.Core.Multiplayer.Game.EventSynchronizer
- method `add_PlayerVoteChanged(Action`1 value)` -> `Void`
- method `BeginEvent(EventModel canonicalEvent, Boolean isPrefinished, Action`1 debugOnStart)` -> `Void`
- method `ChooseLocalOption(Int32 index)` -> `Void`
- method `ChooseOptionForEvent(Player player, Int32 optionIndex)` -> `Void`
- method `ChooseOptionForSharedEvent(UInt32 optionIndex)` -> `Void`
- method `ChooseSharedEventOption()` -> `Void`
- method `ClearPlayerVotes()` -> `Void`
- method `Dispose()` -> `Void`
- method `get_Events()` -> `IReadOnlyList`1`
- method `get_IsShared()` -> `Boolean`
- method `get_LocalPlayer()` -> `Player`
- method `GetEventForPlayer(Player player)` -> `EventModel`
- method `GetLocalEvent()` -> `EventModel`
- method `GetPlayerVote(Player player)` -> `Nullable`1`
- method `HandleEventOptionChosenMessage(OptionIndexChosenMessage message, UInt64 senderId)` -> `Void`
- method `HandleSharedEventOptionChosenMessage(SharedEventOptionChosenMessage message, UInt64 senderId)` -> `Void`
- method `HandleVotedForSharedEventOptionMessage(VotedForSharedEventOptionMessage message, UInt64 senderId)` -> `Void`
- method `PlayerVotedForSharedOptionIndex(Player player, UInt32 optionIndex, UInt32 pageIndex)` -> `Void`
- method `remove_PlayerVoteChanged(Action`1 value)` -> `Void`
- method `ResumeEvents(AbstractRoom exitedRoom)` -> `Void`
- method `SaveEventOptionToHistory(Player player, EventOption option)` -> `Void`
- field `EventModel _canonicalEvent`
- field `List`1 _events`
- field `UInt64 _localPlayerId`
- field `Logger _logger`
- field `RunLocationTargetedMessageBuffer _messageBuffer`
- field `Rng _multiplayerOptionSelectionRng`
- field `INetGameService _netService`
- field `UInt32 _pageIndex`
- field `IPlayerCollection _playerCollection`
- field `List`1 _playerVotes`
- field `Action`1 PlayerVoteChanged`

## MegaCrit.Sts2.Core.Multiplayer.Game.EventSynchronizer+<>c
- method `<PlayerVotedForSharedOptionIndex>b__23_0(Nullable`1 p)` -> `Boolean`
- field `<>c <>9`
- field `Func`2 <>9__23_0`

## MegaCrit.Sts2.Core.Multiplayer.Game.FlavorSynchronizer
- method `add_OnEndTurnPingReceived(Action`1 value)` -> `Void`
- method `CreateEndTurnPingDialogueIfNecessary(Player player)` -> `Void`
- method `CreateMapPing(MapCoord coord, Player player)` -> `Void`
- method `Dispose()` -> `Void`
- method `get_LocalPlayer()` -> `Player`
- method `HandleEndTurnPingMessage(EndTurnPingMessage message, UInt64 senderId)` -> `Void`
- method `HandleMapPingMessage(MapPingMessage message, UInt64 senderId)` -> `Void`
- method `remove_OnEndTurnPingReceived(Action`1 value)` -> `Void`
- method `SendEndTurnPing()` -> `Void`
- method `SendMapPing(MapCoord coord)` -> `Void`
- field `Dictionary`2 _endTurnPingDialogues`
- field `INetGameService _gameService`
- field `UInt64 _localPlayerId`
- field `UInt64 _mapPingDebounceMsec`
- field `UInt64 _nextAllowedPingTime`
- field `UInt64 _pingDebounceMsec`
- field `IPlayerCollection _playerCollection`
- field `Action`1 OnEndTurnPingReceived`

## MegaCrit.Sts2.Core.Multiplayer.Game.INetClientGameService
- method `get_NetClient()` -> `NetClient`

## MegaCrit.Sts2.Core.Multiplayer.Game.INetGameService
- method `add_Disconnected(Action`1 value)` -> `Void`
- method `Disconnect(NetError reason, Boolean now)` -> `Void`
- method `get_IsConnected()` -> `Boolean`
- method `get_IsGameLoading()` -> `Boolean`
- method `get_NetId()` -> `UInt64`
- method `get_Platform()` -> `PlatformType`
- method `get_Type()` -> `NetGameType`
- method `GetRawLobbyIdentifier()` -> `String`
- method `GetStatsForPeer(UInt64 peerId)` -> `ConnectionStats`
- method `RegisterMessageHandler(MessageHandlerDelegate`1 messageHandlerDelegate)` -> `Void`
- method `remove_Disconnected(Action`1 value)` -> `Void`
- method `SendMessage(T message, UInt64 playerId)` -> `Void`
- method `SendMessage(T message)` -> `Void`
- method `SetGameLoading(Boolean isLoading)` -> `Void`
- method `UnregisterMessageHandler(MessageHandlerDelegate`1 messageHandlerDelegate)` -> `Void`
- method `Update()` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Game.INetHostGameService
- method `add_ClientConnected(Action`1 value)` -> `Void`
- method `add_ClientDisconnected(Action`2 value)` -> `Void`
- method `DisconnectClient(UInt64 peerId, NetError reason, Boolean now)` -> `Void`
- method `get_ConnectedPeers()` -> `IReadOnlyList`1`
- method `get_NetHost()` -> `NetHost`
- method `remove_ClientConnected(Action`1 value)` -> `Void`
- method `remove_ClientDisconnected(Action`2 value)` -> `Void`
- method `SetPeerReadyForBroadcasting(UInt64 peerId)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Game.JoinFlow
- method `AttemptJoin(NetClientGameService gameService)` -> `Task`1`
- method `AttemptLoadJoin(NetClientGameService gameService)` -> `Task`1`
- method `AttemptRejoin(NetClientGameService gameService)` -> `Task`1`
- method `Begin(IClientConnectionInitializer initializer, SceneTree sceneTree)` -> `Task`1`
- method `Cancel()` -> `Void`
- method `get_CancelToken()` -> `CancellationTokenSource`
- method `get_NetService()` -> `NetClientGameService`
- method `HandleInitialGameInfoMessage(InitialGameInfoMessage message, UInt64 _)` -> `Void`
- method `HandleJoinResponseMessage(ClientLobbyJoinResponseMessage message, UInt64 senderId)` -> `Void`
- method `HandleLoadJoinResponseMessage(ClientLoadJoinResponseMessage message, UInt64 senderId)` -> `Void`
- method `HandleRejoinResponseMessage(ClientRejoinResponseMessage message, UInt64 senderId)` -> `Void`
- method `NetServiceUpdateLoop(CancellationTokenSource token, SceneTree sceneTree)` -> `Task`
- method `OnDisconnected(NetErrorInfo info)` -> `Void`
- method `set_CancelToken(CancellationTokenSource value)` -> `Void`
- method `set_NetService(NetClientGameService value)` -> `Void`
- field `TaskCompletionSource`1 _connectCompletion`
- field `TaskCompletionSource`1 _joinCompletion`
- field `TaskCompletionSource`1 _loadJoinCompletion`
- field `Logger _logger`
- field `TaskCompletionSource`1 _rejoinCompletion`
- field `CancellationTokenSource <CancelToken>k__BackingField`
- field `NetClientGameService <NetService>k__BackingField`

## MegaCrit.Sts2.Core.Multiplayer.Game.JoinFlow+<AttemptJoin>d__15
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `JoinFlow <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `NetClientGameService gameService`

## MegaCrit.Sts2.Core.Multiplayer.Game.JoinFlow+<AttemptLoadJoin>d__16
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `JoinFlow <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `NetClientGameService gameService`

## MegaCrit.Sts2.Core.Multiplayer.Game.JoinFlow+<AttemptRejoin>d__17
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `JoinFlow <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `NetClientGameService gameService`

## MegaCrit.Sts2.Core.Multiplayer.Game.JoinFlow+<Begin>d__13
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `JoinFlow <>4__this`
- field `Object <>7__wrap2`
- field `Int32 <>7__wrap3`
- field `JoinResult <>7__wrap4`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `TaskAwaiter`1 <>u__2`
- field `TaskAwaiter`1 <>u__3`
- field `TaskAwaiter`1 <>u__4`
- field `TaskAwaiter`1 <>u__5`
- field `TaskAwaiter <>u__6`
- field `InitialGameInfoMessage <initialMessage>5__6`
- field `RunSessionState <state>5__7`
- field `CancellationTokenSource <updateLoopCancelSource>5__2`
- field `IClientConnectionInitializer initializer`
- field `SceneTree sceneTree`

## MegaCrit.Sts2.Core.Multiplayer.Game.JoinFlow+<NetServiceUpdateLoop>d__14
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `JoinFlow <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `Object <>u__1`
- field `SceneTree sceneTree`
- field `CancellationTokenSource token`

## MegaCrit.Sts2.Core.Multiplayer.Game.JoinResult
- field `GameMode gameMode`
- field `Nullable`1 joinResponse`
- field `Nullable`1 loadJoinResponse`
- field `Nullable`1 rejoinResponse`
- field `Nullable`1 sessionState`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.ILoadRunLobbyListener
- method `BeginRun()` -> `Void`
- method `LocalPlayerDisconnected(NetErrorInfo info)` -> `Void`
- method `PlayerConnected(UInt64 playerId)` -> `Void`
- method `PlayerReadyChanged(UInt64 playerId)` -> `Void`
- method `RemotePlayerDisconnected(UInt64 playerId)` -> `Void`
- method `ShouldAllowRunToBegin()` -> `Task`1`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.IRunLobbyListener
- method `GetRejoinMessage()` -> `ClientRejoinResponseMessage`
- method `LocalPlayerDisconnected(NetErrorInfo info)` -> `Void`
- method `RunAbandoned()` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.IStartRunLobbyListener
- method `AscensionChanged()` -> `Void`
- method `BeginRun(String seed, List`1 acts, IReadOnlyList`1 modifiers)` -> `Void`
- method `LocalPlayerDisconnected(NetErrorInfo info)` -> `Void`
- method `MaxAscensionChanged()` -> `Void`
- method `ModifiersChanged()` -> `Void`
- method `PlayerChanged(LobbyPlayer player, Boolean isRandomCharacterResolution)` -> `Void`
- method `PlayerConnected(LobbyPlayer player)` -> `Void`
- method `RemotePlayerDisconnected(LobbyPlayer player)` -> `Void`
- method `SeedChanged()` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby
- method `AddLocalHostPlayer()` -> `Void`
- method `BeginHandshakeTimeout(ConnectingPlayer connectingPlayer)` -> `Task`
- method `BeginRunIfAllPlayersReady()` -> `Void`
- method `CleanUp(Boolean disconnectSession)` -> `Void`
- method `get_ConnectedPlayerIds()` -> `HashSet`1`
- method `get_GameMode()` -> `GameMode`
- method `get_HandshakeTimeout()` -> `Int32`
- method `get_InputSynchronizer()` -> `PeerInputSynchronizer`
- method `get_LobbyListener()` -> `ILoadRunLobbyListener`
- method `get_NetService()` -> `INetGameService`
- method `get_Run()` -> `SerializableRun`
- method `HandleClientLoadJoinRequestMessage(ClientLoadJoinRequestMessage message, UInt64 senderId)` -> `Void`
- method `HandleClientLobbyJoinRequestMessage(ClientLobbyJoinRequestMessage _, UInt64 senderId)` -> `Void`
- method `HandleClientRejoinRequestMessage(ClientRejoinRequestMessage _, UInt64 senderId)` -> `Void`
- method `HandleLobbyBeginRunMessage(LobbyBeginLoadedRunMessage message, UInt64 senderId)` -> `Void`
- method `HandlePlayerLeftMessage(PlayerLeftMessage message, UInt64 senderId)` -> `Void`
- method `HandlePlayerReadyMessage(LobbyPlayerSetReadyMessage message, UInt64 senderId)` -> `Void`
- method `HandlePlayerReconnectedMessage(PlayerReconnectedMessage message, UInt64 _)` -> `Void`
- method `IsAboutToBeginGame()` -> `Boolean`
- method `IsPlayerReady(UInt64 playerId)` -> `Boolean`
- method `OnConnectedToClientAsHost(UInt64 playerId)` -> `Void`
- method `OnDisconnected(NetErrorInfo info)` -> `Void`
- method `OnDisconnectedFromClientAsHost(UInt64 playerId, NetErrorInfo info)` -> `Void`
- method `RemoveConnectingPlayer(UInt64 playerId)` -> `Void`
- method `set_HandshakeTimeout(Int32 value)` -> `Void`
- method `SetReady(Boolean ready)` -> `Void`
- method `TryBeginRun()` -> `Task`
- field `List`1 _connectingPlayers`
- field `Boolean _isBeginningRun`
- field `Logger _logger`
- field `HashSet`1 _readyPlayers`
- field `HashSet`1 <ConnectedPlayerIds>k__BackingField`
- field `Int32 <HandshakeTimeout>k__BackingField`
- field `PeerInputSynchronizer <InputSynchronizer>k__BackingField`
- field `ILoadRunLobbyListener <LobbyListener>k__BackingField`
- field `INetGameService <NetService>k__BackingField`
- field `SerializableRun <Run>k__BackingField`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby+<>c__DisplayClass30_0
- method `<HandleClientLoadJoinRequestMessage>b__0(SerializablePlayer p)` -> `Boolean`
- field `UInt64 senderId`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby+<>c__DisplayClass42_0
- method `<OnConnectedToClientAsHost>b__0(SerializablePlayer p)` -> `Boolean`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby+<BeginHandshakeTimeout>d__43
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `LoadRunLobby <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `ConnectingPlayer connectingPlayer`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby+<TryBeginRun>d__37
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `LoadRunLobby <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter`1 <>u__1`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby+ConnectingPlayer
- method `Equals(ConnectingPlayer other)` -> `Boolean`
- method `Equals(Object obj)` -> `Boolean`
- method `GetHashCode()` -> `Int32`
- field `UInt64 id`
- field `CancellationTokenSource timeoutCancelToken`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.RunLobby
- method `AbandonRun()` -> `Void`
- method `add_LocalPlayerDisconnected(Action value)` -> `Void`
- method `add_PlayerRejoined(Action`1 value)` -> `Void`
- method `add_RemotePlayerDisconnected(Action`1 value)` -> `Void`
- method `Dispose()` -> `Void`
- method `get_ConnectedPlayerIds()` -> `IReadOnlyCollection`1`
- method `get_GameMode()` -> `GameMode`
- method `HandleClientLoadJoinRequestMessage(ClientLoadJoinRequestMessage _, UInt64 senderId)` -> `Void`
- method `HandleClientLobbyJoinRequestMessage(ClientLobbyJoinRequestMessage _, UInt64 senderId)` -> `Void`
- method `HandleClientRejoinRequestMessage(ClientRejoinRequestMessage message, UInt64 senderId)` -> `Void`
- method `HandlePlayerLeftMessage(PlayerLeftMessage message, UInt64 _)` -> `Void`
- method `HandlePlayerRejoinedMessage(PlayerRejoinedMessage message, UInt64 _)` -> `Void`
- method `HandleRunAbandonedMessage(RunAbandonedMessage message, UInt64 _)` -> `Void`
- method `OnConnectedToClientAsHost(UInt64 playerId)` -> `Void`
- method `OnDisconnected(NetErrorInfo info)` -> `Void`
- method `OnDisconnectedFromClientAsHost(UInt64 playerId, NetErrorInfo info)` -> `Void`
- method `remove_LocalPlayerDisconnected(Action value)` -> `Void`
- method `remove_PlayerRejoined(Action`1 value)` -> `Void`
- method `remove_RemotePlayerDisconnected(Action`1 value)` -> `Void`
- field `HashSet`1 _connectedPlayerIds`
- field `IRunLobbyListener _lobbyListener`
- field `Logger _logger`
- field `INetGameService _netService`
- field `IPlayerCollection _playerCollection`
- field `GameMode <GameMode>k__BackingField`
- field `Action LocalPlayerDisconnected`
- field `Action`1 PlayerRejoined`
- field `Action`1 RemotePlayerDisconnected`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby
- method `<get_LocalPlayer>b__51_0(LobbyPlayer p)` -> `Boolean`
- method `<SetReady>b__88_0(LobbyPlayer p)` -> `Boolean`
- method `add_PlayerConnected(Action`1 value)` -> `Void`
- method `add_PlayerDisconnected(Action`1 value)` -> `Void`
- method `AddLocalHostPlayer(UnlockState unlocks, Int32 maxMultiplayerAscension)` -> `Nullable`1`
- method `AddLocalHostPlayerInternal(SerializableUnlockState unlockState, Int32 maxMultiplayerAscension)` -> `Nullable`1`
- method `BeginHandshakeTimeout(ConnectingPlayer connectingPlayer)` -> `Task`
- method `BeginRunForAllPlayers(String seed, List`1 modifiers)` -> `Void`
- method `BeginRunIfAllPlayersReady()` -> `Void`
- method `BeginRunLocally(String seed, List`1 modifiers)` -> `Void`
- method `ChangeCharacter(UInt64 playerId, CharacterModel character, Boolean isRandomCharacterResolution)` -> `Void`
- method `CleanUp(Boolean disconnectSession)` -> `Void`
- method `get_Act1()` -> `String`
- method `get_Ascension()` -> `Int32`
- method `get_DailyTime()` -> `Nullable`1`
- method `get_GameMode()` -> `GameMode`
- method `get_HandshakeTimeout()` -> `Int32`
- method `get_InputSynchronizer()` -> `PeerInputSynchronizer`
- method `get_LobbyListener()` -> `IStartRunLobbyListener`
- method `get_LocalPlayer()` -> `LobbyPlayer`
- method `get_MaxAscension()` -> `Int32`
- method `get_MaxPlayers()` -> `Int32`
- method `get_Modifiers()` -> `IReadOnlyList`1`
- method `get_NetService()` -> `INetGameService`
- method `get_Players()` -> `List`1`
- method `get_Seed()` -> `String`
- method `GetAct(String act1Key)` -> `ActModel`
- method `GetMaxAscensionAcrossAllCharacters()` -> `Int32`
- method `GetUnlockState()` -> `UnlockState`
- method `HandleAscensionChangedMessage(LobbyAscensionChangedMessage message, UInt64 _)` -> `Void`
- method `HandleClientLoadJoinRequestMessage(ClientLoadJoinRequestMessage _, UInt64 senderId)` -> `Void`
- method `HandleClientLobbyJoinRequestMessage(ClientLobbyJoinRequestMessage message, UInt64 senderId)` -> `Void`
- method `HandleClientRejoinRequestMessage(ClientRejoinRequestMessage _, UInt64 senderId)` -> `Void`
- method `HandleLobbyBeginRunMessage(LobbyBeginRunMessage message, UInt64 senderId)` -> `Void`
- method `HandleLobbyPlayerChangedCharacterMessage(LobbyPlayerChangedCharacterMessage message, UInt64 senderId)` -> `Void`
- method `HandleModifiersChangedMessage(LobbyModifiersChangedMessage message, UInt64 _)` -> `Void`
- method `HandlePlayerJoinedMessage(PlayerJoinedMessage message, UInt64 senderId)` -> `Void`
- method `HandlePlayerLeftMessage(PlayerLeftMessage message, UInt64 senderId)` -> `Void`
- method `HandlePlayerReadyMessage(LobbyPlayerSetReadyMessage message, UInt64 senderId)` -> `Void`
- method `HandleSeedChangedMessage(LobbySeedChangedMessage message, UInt64 _)` -> `Void`
- method `InitializeFromMessage(ClientLobbyJoinResponseMessage message)` -> `Void`
- method `IsAboutToBeginGame()` -> `Boolean`
- method `IsAscensionEpochRevealed(ModelId characterId)` -> `Boolean`
- method `OnConnectedToClientAsHost(UInt64 playerId)` -> `Void`
- method `OnDisconnected(NetErrorInfo info)` -> `Void`
- method `OnDisconnectedFromClientAsHost(UInt64 playerId, NetErrorInfo info)` -> `Void`
- method `remove_PlayerConnected(Action`1 value)` -> `Void`
- method `remove_PlayerDisconnected(Action`1 value)` -> `Void`
- method `RemoveConnectingPlayer(UInt64 playerId)` -> `Void`
- method `set_Act1(String value)` -> `Void`
- method `set_Ascension(Int32 value)` -> `Void`
- method `set_DailyTime(Nullable`1 value)` -> `Void`
- method `set_GameMode(GameMode value)` -> `Void`
- method `set_HandshakeTimeout(Int32 value)` -> `Void`
- method `set_MaxAscension(Int32 value)` -> `Void`
- method `set_MaxPlayers(Int32 value)` -> `Void`
- method `set_Seed(String value)` -> `Void`
- method `SetLocalCharacter(CharacterModel character)` -> `Void`
- method `SetModifiers(List`1 modifiers)` -> `Void`
- method `SetReady(Boolean ready)` -> `Void`
- method `SetSeed(String seed)` -> `Void`
- method `SetSingleplayerAscensionAfterCharacterChanged(ModelId characterId)` -> `Void`
- method `SyncAscensionChange(Int32 ascension)` -> `Void`
- method `TryAddPlayerInFirstAvailableSlot(SerializableUnlockState unlockState, Int32 maxAscensionUnlocked, UInt64 playerId)` -> `Nullable`1`
- method `UpdateMaxMultiplayerAscension()` -> `Void`
- method `UpdatePreferredAscension()` -> `Void`
- field `List`1 _connectingPlayers`
- field `Boolean _isBeginningRun`
- field `Logger _logger`
- field `List`1 _modifiers`
- field `String <Act1>k__BackingField`
- field `Int32 <Ascension>k__BackingField`
- field `Nullable`1 <DailyTime>k__BackingField`
- field `GameMode <GameMode>k__BackingField`
- field `Int32 <HandshakeTimeout>k__BackingField`
- field `PeerInputSynchronizer <InputSynchronizer>k__BackingField`
- field `IStartRunLobbyListener <LobbyListener>k__BackingField`
- field `Int32 <MaxAscension>k__BackingField`
- field `Int32 <MaxPlayers>k__BackingField`
- field `INetGameService <NetService>k__BackingField`
- field `List`1 <Players>k__BackingField`
- field `String <Seed>k__BackingField`
- field `Action`1 PlayerConnected`
- field `Action`1 PlayerDisconnected`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby+<>c
- method `<BeginRunForAllPlayers>b__78_0(ModifierModel m)` -> `SerializableModifier`
- method `<GetUnlockState>b__96_0(LobbyPlayer p)` -> `UnlockState`
- method `<HandleClientLobbyJoinRequestMessage>b__65_0(ModifierModel m)` -> `SerializableModifier`
- method `<HandleModifiersChangedMessage>b__74_0(SerializableModifier m)` -> `ModelId`
- method `<IsAboutToBeginGame>b__90_0(LobbyPlayer p)` -> `Boolean`
- method `<SetModifiers>b__87_0(ModifierModel m)` -> `SerializableModifier`
- method `<UpdateMaxMultiplayerAscension>b__66_0(LobbyPlayer p)` -> `Int32`
- field `<>c <>9`
- field `Func`2 <>9__65_0`
- field `Func`2 <>9__66_0`
- field `Func`2 <>9__74_0`
- field `Func`2 <>9__78_0`
- field `Func`2 <>9__87_0`
- field `Func`2 <>9__90_0`
- field `Func`2 <>9__96_0`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby+<>c__DisplayClass70_0
- method `<HandlePlayerLeftMessage>b__0(LobbyPlayer p)` -> `Boolean`
- field `PlayerLeftMessage message`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby+<>c__DisplayClass75_0
- method `<HandlePlayerReadyMessage>b__0(LobbyPlayer p)` -> `Boolean`
- field `UInt64 senderId`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby+<>c__DisplayClass77_0
- method `<ChangeCharacter>b__0(LobbyPlayer p)` -> `Boolean`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby+<>c__DisplayClass92_0
- method `<TryAddPlayerInFirstAvailableSlot>b__0(LobbyPlayer p)` -> `Boolean`
- field `Int32 i`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby+<>c__DisplayClass95_0
- method `<OnDisconnectedFromClientAsHost>b__0(LobbyPlayer p)` -> `Boolean`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby+<>O
- field `Func`2 <0>__FromSerializable`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby+<BeginHandshakeTimeout>d__94
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `StartRunLobby <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `ConnectingPlayer connectingPlayer`

## MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby+ConnectingPlayer
- method `Equals(ConnectingPlayer other)` -> `Boolean`
- method `Equals(Object obj)` -> `Boolean`
- method `GetHashCode()` -> `Int32`
- field `UInt64 id`
- field `CancellationTokenSource timeoutCancelToken`

## MegaCrit.Sts2.Core.Multiplayer.Game.MapSelectionSynchronizer
- method `<PlayerVotedForMapCoord>b__21_0(Nullable`1 p)` -> `Boolean`
- method `add_PlayerVoteCancelled(Action`1 value)` -> `Void`
- method `add_PlayerVoteChanged(Action`3 value)` -> `Void`
- method `add_PlayerVotesCleared(Action value)` -> `Void`
- method `BeforeMapGenerated()` -> `Void`
- method `get_MapGenerationCount()` -> `Int32`
- method `GetVote(Player player)` -> `Nullable`1`
- method `MoveToMapCoord()` -> `Void`
- method `OnLocationChanged(MapLocation location)` -> `Void`
- method `PlayerVotedForMapCoord(Player player, MapLocation source, Nullable`1 destination)` -> `Void`
- method `remove_PlayerVoteCancelled(Action`1 value)` -> `Void`
- method `remove_PlayerVoteChanged(Action`3 value)` -> `Void`
- method `remove_PlayerVotesCleared(Action value)` -> `Void`
- method `set_MapGenerationCount(Int32 value)` -> `Void`
- field `MapLocation _acceptingVotesFromSource`
- field `ActionQueueSynchronizer _actionQueueSynchronizer`
- field `Logger _logger`
- field `Rng _multiplayerMapPointSelection`
- field `INetGameService _netService`
- field `RunState _runState`
- field `List`1 _votes`
- field `Int32 <MapGenerationCount>k__BackingField`
- field `Action`1 PlayerVoteCancelled`
- field `Action`3 PlayerVoteChanged`
- field `Action PlayerVotesCleared`

## MegaCrit.Sts2.Core.Multiplayer.Game.MapVote
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `MapCoord coord`
- field `Int32 mapGenerationCount`

## MegaCrit.Sts2.Core.Multiplayer.Game.MessageHandlerDelegate`1
- method `BeginInvoke(T message, UInt64 senderId, AsyncCallback callback, Object object)` -> `IAsyncResult`
- method `EndInvoke(IAsyncResult result)` -> `Void`
- method `Invoke(T message, UInt64 senderId)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Game.NetGameType
- field `NetGameType Client`
- field `NetGameType Host`
- field `NetGameType None`
- field `NetGameType Replay`
- field `NetGameType Singleplayer`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Multiplayer.Game.NetGameTypeExtensions
- method `IsMultiplayer(NetGameType type)` -> `Boolean`

## MegaCrit.Sts2.Core.Multiplayer.Game.NetLoadingHandle
- method `Dispose()` -> `Void`
- method `Release(INetGameService netService)` -> `Void`
- field `Dictionary`2 _loadCounts`
- field `INetGameService _netService`

## MegaCrit.Sts2.Core.Multiplayer.Game.OneOffSynchronizer
- method `Dispose()` -> `Void`
- method `DoLocalMerchantCardRemoval(Int32 goldCost, Boolean cancelable)` -> `Task`1`
- method `DoLocalTreasureRoomRewards()` -> `Task`1`
- method `DoMerchantCardRemoval(Player player, Int32 goldCost, Boolean cancelable)` -> `Task`1`
- method `DoTreasureRoomRewards(Player player)` -> `Task`1`
- method `get_LocalPlayer()` -> `Player`
- method `HandleMerchantCardRemoval(MerchantCardRemovalMessage message, UInt64 senderId)` -> `Void`
- method `HandleTreasureChestOpenedMessage(TreasureChestOpenedMessage message, UInt64 senderId)` -> `Void`
- method `TryHandleSpoilsMap(Player player)` -> `Task`1`
- field `INetGameService _gameService`
- field `UInt64 _localPlayerId`
- field `RunLocationTargetedMessageBuffer _messageBuffer`
- field `IPlayerCollection _playerCollection`

## MegaCrit.Sts2.Core.Multiplayer.Game.OneOffSynchronizer+<>c
- method `<TryHandleSpoilsMap>b__14_0(AbstractModel q)` -> `Boolean`
- field `<>c <>9`
- field `Func`2 <>9__14_0`

## MegaCrit.Sts2.Core.Multiplayer.Game.OneOffSynchronizer+<DoMerchantCardRemoval>d__10
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `TaskAwaiter <>u__2`
- field `CardModel <card>5__2`
- field `Boolean cancelable`
- field `Int32 goldCost`
- field `Player player`

## MegaCrit.Sts2.Core.Multiplayer.Game.OneOffSynchronizer+<DoTreasureRoomRewards>d__13
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `OneOffSynchronizer <>4__this`
- field `Double <>7__wrap2`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter <>u__1`
- field `TaskAwaiter`1 <>u__2`
- field `Double <gold>5__2`
- field `Player player`

## MegaCrit.Sts2.Core.Multiplayer.Game.OneOffSynchronizer+<TryHandleSpoilsMap>d__14
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `Player player`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.HoveredModelData
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Equals(HoveredModelData other)` -> `Boolean`
- method `FromModel(AbstractModel model)` -> `HoveredModelData`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `Nullable`1 hoveredCombatCard`
- field `ModelId hoveredModelId`
- field `Nullable`1 hoveredPotionIndex`
- field `Nullable`1 hoveredRelicIndex`
- field `HoveredModelType type`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.HoveredModelTracker
- method `add_HoverChanged(Action`1 value)` -> `Void`
- method `GetHoveredModel(UInt64 playerId)` -> `AbstractModel`
- method `OnLocalCardDeselected()` -> `Void`
- method `OnLocalCardHovered(CardModel cardModel)` -> `Void`
- method `OnLocalCardSelected(CardModel cardModel)` -> `Void`
- method `OnLocalCardUnhovered()` -> `Void`
- method `OnLocalPotionDeselected()` -> `Void`
- method `OnLocalPotionHovered(PotionModel potionModel)` -> `Void`
- method `OnLocalPotionSelected(PotionModel potionModel)` -> `Void`
- method `OnLocalPotionUnhovered()` -> `Void`
- method `OnLocalRelicHovered(RelicModel relicModel)` -> `Void`
- method `OnLocalRelicUnhovered()` -> `Void`
- method `OnPlayerStateChanged(UInt64 playerId)` -> `Void`
- method `OnPlayerStateRemoved(UInt64 playerId)` -> `Void`
- method `remove_HoverChanged(Action`1 value)` -> `Void`
- method `SynchronizeLocalHoveredModel()` -> `Void`
- field `List`1 _hoveredModels`
- field `PeerInputSynchronizer _inputSynchronizer`
- field `CardModel _localHoveredCard`
- field `PotionModel _localHoveredPotion`
- field `RelicModel _localHoveredRelic`
- field `CardModel _localSelectedCard`
- field `PotionModel _localSelectedPotion`
- field `IPlayerCollection _playerCollection`
- field `Action`1 HoverChanged`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.HoveredModelType
- field `HoveredModelType Card`
- field `HoveredModelType None`
- field `HoveredModelType Potion`
- field `HoveredModelType Relic`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.INetCursorPositionTranslator
- method `GetNetPositionFromScreenPosition(Vector2 screenPosition)` -> `Vector2`
- method `GetScreenPositionFromNetPosition(Vector2 netPosition)` -> `Vector2`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.MapDrawingEventType
- field `MapDrawingEventType BeginLine`
- field `MapDrawingEventType ContinueLine`
- field `MapDrawingEventType EndLine`
- field `MapDrawingEventType None`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.NetCursorHelper
- method `GetControlSpacePosition(Vector2 normalizedCursorPosition, Control rootNode)` -> `Vector2`
- method `GetNormalizedPosition(Vector2 mouseScreenPos, Control rootNode)` -> `Vector2`
- field `QuantizeParams quantizeParams`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.NetMapDrawingEvent
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `QuantizeParams _quantizeParamsX`
- field `QuantizeParams _quantizeParamsY`
- field `Nullable`1 overrideDrawingMode`
- field `Vector2 position`
- field `MapDrawingEventType type`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.PeerInputSynchronizer
- method `add_ScreenChanged(Action`2 value)` -> `Void`
- method `add_StateAdded(Action`1 value)` -> `Void`
- method `add_StateChanged(Action`1 value)` -> `Void`
- method `add_StateRemoved(Action`1 value)` -> `Void`
- method `Dispose()` -> `Void`
- method `ForceGetStateForPlayer(UInt64 playerId)` -> `PeerInputState`
- method `get_NetService()` -> `INetGameService`
- method `GetControlSpaceFocusPosition(UInt64 playerId, Control rootControl)` -> `Vector2`
- method `GetHoveredModelData(UInt64 playerId)` -> `HoveredModelData`
- method `GetIsTargeting(UInt64 playerId)` -> `Boolean`
- method `GetMouseDown(UInt64 playerId)` -> `Boolean`
- method `GetOrCreateStateForPlayer(UInt64 playerId)` -> `PeerInputState`
- method `GetScreenType(UInt64 playerId)` -> `NetScreenType`
- method `GetStateForPlayer(UInt64 playerId)` -> `PeerInputState`
- method `HandlePeerInputMessage(PeerInputMessage message, UInt64 senderId)` -> `Void`
- method `OnPlayerDisconnected(UInt64 playerId)` -> `Void`
- method `QueueSyncMessage(Int32 delayMsec)` -> `Task`
- method `remove_ScreenChanged(Action`2 value)` -> `Void`
- method `remove_StateAdded(Action`1 value)` -> `Void`
- method `remove_StateChanged(Action`1 value)` -> `Void`
- method `remove_StateRemoved(Action`1 value)` -> `Void`
- method `SendSyncMessage()` -> `Void`
- method `SendSyncMessageAfterSmallDelay()` -> `Task`
- method `StartOverridingCursorPositioning(INetCursorPositionTranslator positionTranslator)` -> `Void`
- method `StopOverridingCursorPositioning()` -> `Void`
- method `SyncLocalControllerFocus(Vector2 focusPosition, Control rootControl)` -> `Void`
- method `SyncLocalHoveredModel(AbstractModel model)` -> `Void`
- method `SyncLocalIsTargeting(Boolean isTargeting)` -> `Void`
- method `SyncLocalIsUsingController(Boolean isUsingController)` -> `Void`
- method `SyncLocalMouseDown(Boolean mouseDown)` -> `Void`
- method `SyncLocalMousePos(Vector2 mouseScreenPos, Control rootControl)` -> `Void`
- method `SyncLocalScreen(NetScreenType netScreenType)` -> `Void`
- method `TrySendSyncMessage()` -> `Void`
- field `INetCursorPositionTranslator _cursorTranslator`
- field `List`1 _inputStates`
- field `UInt64 _lastSyncMsec`
- field `Logger _logger`
- field `INetGameService _netService`
- field `Task _syncMessageTask`
- field `PeerInputMessage _syncMessageToSend`
- field `Int32 minUpdateMsec`
- field `Action`2 ScreenChanged`
- field `Action`1 StateAdded`
- field `Action`1 StateChanged`
- field `Action`1 StateRemoved`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.PeerInputSynchronizer+<>c__DisplayClass36_0
- method `<GetStateForPlayer>b__0(PeerInputState s)` -> `Boolean`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.PeerInputSynchronizer+<>c__DisplayClass47_0
- method `<OnPlayerDisconnected>b__0(PeerInputState p)` -> `Boolean`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.PeerInputSynchronizer+<QueueSyncMessage>d__33
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `PeerInputSynchronizer <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `Int32 delayMsec`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.PeerInputSynchronizer+<SendSyncMessageAfterSmallDelay>d__34
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `PeerInputSynchronizer <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `YieldAwaiter <>u__1`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.PeerInputSynchronizer+PeerInputState
- field `Vector2 controllerFocusPosition`
- field `HoveredModelData hoveredModelData`
- field `Boolean isMouseDown`
- field `Boolean isTargeting`
- field `Boolean isUsingController`
- field `Vector2 netMousePosition`
- field `NetScreenType netScreenType`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.ScreenStateTracker
- method `GetCurrentScreen()` -> `NetScreenType`
- method `OnCapstoneScreenChanged()` -> `Void`
- method `OnMapScreenVisibilityChanged()` -> `Void`
- method `OnOverlayStackChanged()` -> `Void`
- method `SetIsInSharedRelicPickingScreen(Boolean isInSharedRelicPicking)` -> `Void`
- method `SyncLocalScreen()` -> `Void`
- field `NetScreenType _capstoneScreen`
- field `Boolean _isInSharedRelicPicking`
- field `Boolean _mapScreenVisible`
- field `Callable _onRewardsScreenCompleted`
- field `NetScreenType _overlayScreen`

## MegaCrit.Sts2.Core.Multiplayer.Game.ReactionSynchronizer
- method `Dispose()` -> `Void`
- method `get_NetService()` -> `INetGameService`
- method `HandleReactionMessage(ReactionMessage message, UInt64 senderId)` -> `Void`
- method `SendLocalReaction(ReactionType type, Vector2 mouseScreenPos)` -> `Void`
- field `NReactionContainer _container`
- field `INetGameService <NetService>k__BackingField`

## MegaCrit.Sts2.Core.Multiplayer.Game.RestSiteSynchronizer
- method `add_AfterPlayerOptionChosen(Action`3 value)` -> `Void`
- method `add_BeforePlayerOptionChosen(Action`2 value)` -> `Void`
- method `add_PlayerHoverChanged(Action`1 value)` -> `Void`
- method `BeginRestSite()` -> `Void`
- method `ChooseLocalOption(Int32 index)` -> `Task`1`
- method `ChooseOption(Player player, Int32 optionIndex)` -> `Task`1`
- method `Dispose()` -> `Void`
- method `get_LocalPlayer()` -> `Player`
- method `GetChosenOptionIndex(UInt64 playerId)` -> `Nullable`1`
- method `GetHoveredOptionIndex(UInt64 playerId)` -> `Nullable`1`
- method `GetLocalOptions()` -> `IReadOnlyList`1`
- method `GetOptionsForPlayer(UInt64 playerId)` -> `IReadOnlyList`1`
- method `GetOptionsForPlayer(Player player)` -> `IReadOnlyList`1`
- method `HandleRestSiteOptionChosenMessage(OptionIndexChosenMessage message, UInt64 senderId)` -> `Void`
- method `HandleRestSiteOptionHoveredMessage(RestSiteOptionHoveredMessage message, UInt64 senderId)` -> `Void`
- method `LocalOptionHovered(RestSiteOption option)` -> `Void`
- method `QueueHoverMessage(Int32 delayMsec)` -> `Task`
- method `remove_AfterPlayerOptionChosen(Action`3 value)` -> `Void`
- method `remove_BeforePlayerOptionChosen(Action`2 value)` -> `Void`
- method `remove_PlayerHoverChanged(Action`1 value)` -> `Void`
- method `SendHoverMessage()` -> `Void`
- method `SendHoverMessageAfterSmallDelay()` -> `Task`
- method `TrySendHoverMessage()` -> `Void`
- field `Nullable`1 _hoveredMessage`
- field `Task _hoverMessageTask`
- field `UInt64 _lastHoverMessageMsec`
- field `UInt64 _localPlayerId`
- field `Logger _logger`
- field `RunLocationTargetedMessageBuffer _messageBuffer`
- field `INetGameService _netService`
- field `IPlayerCollection _playerCollection`
- field `List`1 _restSites`
- field `Action`3 AfterPlayerOptionChosen`
- field `Action`2 BeforePlayerOptionChosen`
- field `Int32 minHoverMessageMsec`
- field `Action`1 PlayerHoverChanged`

## MegaCrit.Sts2.Core.Multiplayer.Game.RestSiteSynchronizer+<ChooseOption>d__28
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `RestSiteSynchronizer <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `RestSiteOption <option>5__3`
- field `PlayerRestSite <restSite>5__2`
- field `Int32 optionIndex`
- field `Player player`

## MegaCrit.Sts2.Core.Multiplayer.Game.RestSiteSynchronizer+<QueueHoverMessage>d__36
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `RestSiteSynchronizer <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `Int32 delayMsec`

## MegaCrit.Sts2.Core.Multiplayer.Game.RestSiteSynchronizer+<SendHoverMessageAfterSmallDelay>d__37
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `RestSiteSynchronizer <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `YieldAwaiter <>u__1`

## MegaCrit.Sts2.Core.Multiplayer.Game.RestSiteSynchronizer+PlayerRestSite
- field `Nullable`1 hoveredOptionIndex`
- field `Nullable`1 lastChosenOptionIndex`
- field `List`1 options`

## MegaCrit.Sts2.Core.Multiplayer.Game.RewardSynchronizer
- method `Dispose()` -> `Void`
- method `DoCardRemoval(Player player)` -> `Task`1`
- method `DoLocalCardRemoval()` -> `Task`1`
- method `get_LocalPlayer()` -> `Player`
- method `HandleCardRemovedMessage(CardRemovedMessage message, UInt64 senderId)` -> `Void`
- method `HandleGoldLostMessage(GoldLostMessage message, UInt64 senderId)` -> `Void`
- method `HandlePaelsWingSacrifice(PaelsWingSacrificeMessage message, UInt64 senderId)` -> `Void`
- method `HandleRewardObtainedMessage(RewardObtainedMessage message, UInt64 senderId)` -> `Void`
- method `OnCombatEnded(CombatRoom _)` -> `Void`
- method `SyncLocalCardEvent(CardModel card, Boolean skipped)` -> `Void`
- method `SyncLocalGoldLost(Int32 goldLost)` -> `Void`
- method `SyncLocalObtainedCard(CardModel card)` -> `Void`
- method `SyncLocalObtainedGold(Int32 goldAmount)` -> `Void`
- method `SyncLocalObtainedPotion(PotionModel potion)` -> `Void`
- method `SyncLocalObtainedRelic(RelicModel relic)` -> `Void`
- method `SyncLocalPaelsWingSacrifice(PaelsWing paelsWing)` -> `Void`
- method `SyncLocalPotionEvent(PotionModel potion, Boolean skipped)` -> `Void`
- method `SyncLocalRelicEvent(RelicModel relic, Boolean skipped)` -> `Void`
- method `SyncLocalSkippedCard(CardModel card)` -> `Void`
- method `SyncLocalSkippedPotion(PotionModel potion)` -> `Void`
- method `SyncLocalSkippedRelic(RelicModel relic)` -> `Void`
- field `List`1 _bufferedMessages`
- field `INetGameService _gameService`
- field `UInt64 _localPlayerId`
- field `RunLocationTargetedMessageBuffer _messageBuffer`
- field `IPlayerCollection _playerCollection`

## MegaCrit.Sts2.Core.Multiplayer.Game.RewardSynchronizer+<DoCardRemoval>d__28
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `TaskAwaiter <>u__2`
- field `CardModel <card>5__2`
- field `Player player`

## MegaCrit.Sts2.Core.Multiplayer.Game.RewardSynchronizer+<DoLocalCardRemoval>d__21
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `RewardSynchronizer <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`

## MegaCrit.Sts2.Core.Multiplayer.Game.RewardSynchronizer+BufferedMessage
- field `CardRemovedMessage cardRemovedMessage`
- field `Nullable`1 goldLostMessage`
- field `PaelsWingSacrificeMessage paelsWingSacrificeMessage`
- field `Nullable`1 rewardMessage`
- field `UInt64 senderId`

## MegaCrit.Sts2.Core.Multiplayer.Game.RunLocationTargetedMessageBuffer
- method `CallHandlersOfType(Type type, INetMessage message, UInt64 senderId)` -> `Void`
- method `get_CurrentLocation()` -> `RunLocation`
- method `HandleMessage(T message, UInt64 senderId)` -> `Void`
- method `OnLocationChanged(RunLocation location)` -> `Void`
- method `RegisterMessageHandler(MessageHandlerDelegate`1 handler)` -> `Void`
- method `set_CurrentLocation(RunLocation value)` -> `Void`
- method `UnregisterMessageHandler(MessageHandlerDelegate`1 handler)` -> `Void`
- field `INetGameService _gameService`
- field `Logger _logger`
- field `List`1 _messageHandlers`
- field `List`1 _messagesWaitingOnLocationChange`
- field `HashSet`1 _visitedLocations`
- field `RunLocation <CurrentLocation>k__BackingField`

## MegaCrit.Sts2.Core.Multiplayer.Game.RunLocationTargetedMessageBuffer+<>c__DisplayClass16_0`1
- method `<RegisterMessageHandler>g__AnonymousDelegate|0(INetMessage message, UInt64 senderId)` -> `Void`
- field `MessageHandlerDelegate`1 handler`

## MegaCrit.Sts2.Core.Multiplayer.Game.RunLocationTargetedMessageBuffer+AnonymizedMessageHandlerDelegate
- method `BeginInvoke(INetMessage message, UInt64 senderId, AsyncCallback callback, Object object)` -> `IAsyncResult`
- method `EndInvoke(IAsyncResult result)` -> `Void`
- method `Invoke(INetMessage message, UInt64 senderId)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Game.RunLocationTargetedMessageBuffer+BlockedMessage
- field `RunLocation location`
- field `INetMessage message`
- field `Type messageType`
- field `UInt64 senderId`

## MegaCrit.Sts2.Core.Multiplayer.Game.RunLocationTargetedMessageBuffer+MessageHandler
- field `AnonymizedMessageHandlerDelegate anonymizedHandler`
- field `Object originalHandler`

## MegaCrit.Sts2.Core.Multiplayer.Game.RunLocationTargetedMessageBuffer+TypeAndMessageHandlers
- field `List`1 handlers`
- field `Type messageType`
- field `Object netServiceHandler`

## MegaCrit.Sts2.Core.Multiplayer.Game.StateDivergenceException

## MegaCrit.Sts2.Core.Multiplayer.Game.TreasureRoomRelicSynchronizer
- method `add_RelicsAwarded(Action`1 value)` -> `Void`
- method `add_VotesChanged(Action value)` -> `Void`
- method `AwardRelics()` -> `Void`
- method `BeginRelicPicking()` -> `Void`
- method `CompleteWithNoRelics()` -> `Void`
- method `EndRelicVoting()` -> `Void`
- method `get_CurrentRelics()` -> `IReadOnlyList`1`
- method `get_LocalPlayer()` -> `Player`
- method `GetPlayerVote(Player player)` -> `PlayerVote`
- method `OnPicked(Player player, Nullable`1 index)` -> `Void`
- method `OnRoomExited()` -> `Void`
- method `PickRelicLocally(Nullable`1 index)` -> `Void`
- method `remove_RelicsAwarded(Action`1 value)` -> `Void`
- method `remove_VotesChanged(Action value)` -> `Void`
- method `SkipRelicLocally()` -> `Void`
- method `TryGetRelicForTutorial(UnlockState unlockState)` -> `RelicModel`
- field `ActionQueueSynchronizer _actionQueueSynchronizer`
- field `List`1 _currentRelics`
- field `UInt64 _localPlayerId`
- field `Logger _logger`
- field `IPlayerCollection _playerCollection`
- field `PlayerVote _predictedVote`
- field `Rng _rng`
- field `RelicGrabBag _sharedGrabBag`
- field `Boolean _singlePlayerSkipped`
- field `List`1 _votes`
- field `Action`1 RelicsAwarded`
- field `Action VotesChanged`

## MegaCrit.Sts2.Core.Multiplayer.Game.TreasureRoomRelicSynchronizer+<>c
- method `<OnPicked>b__25_0(PlayerVote v)` -> `Boolean`
- method `<TryGetRelicForTutorial>b__31_0(IReadOnlyList`1 l)` -> `IEnumerable`1`
- method `<TryGetRelicForTutorial>b__31_1(MapPointHistoryEntry p)` -> `Boolean`
- field `<>c <>9`
- field `Func`2 <>9__25_0`
- field `Func`2 <>9__31_0`
- field `Func`2 <>9__31_1`

## MegaCrit.Sts2.Core.Multiplayer.Game.TreasureRoomRelicSynchronizer+<>c__DisplayClass26_0
- method `<AwardRelics>b__0(Player p)` -> `Boolean`
- field `TreasureRoomRelicSynchronizer <>4__this`
- field `List`1 results`

## MegaCrit.Sts2.Core.Multiplayer.Game.TreasureRoomRelicSynchronizer+<>c__DisplayClass26_1
- method `<AwardRelics>b__1()` -> `RelicPickingFightMove`
- field `<>c__DisplayClass26_0 CS$<>8__locals1`
- field `RelicPickingFightMove[] possibleMoves`

## MegaCrit.Sts2.Core.Multiplayer.Game.TreasureRoomRelicSynchronizer+<>c__DisplayClass26_2
- method `<AwardRelics>b__2(RelicPickingResult r)` -> `Boolean`
- field `Player p`

## MegaCrit.Sts2.Core.Multiplayer.Game.TreasureRoomRelicSynchronizer+PlayerVote
- field `Nullable`1 index`
- field `Boolean voteReceived`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.ActionEnqueuedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `INetAction action`
- field `RunLocation location`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.CardRemovedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Location(RunLocation value)` -> `Void`
- field `RunLocation <Location>k__BackingField`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Checksums.ChecksumDataMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `NetChecksumData checksumData`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Checksums.StateDivergenceMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `NetChecksumData senderChecksum`
- field `NetFullCombatState senderCombatState`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor.ClearMapDrawingsMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor.EndTurnPingMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor.MapDrawingMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Events()` -> `IReadOnlyList`1`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `TryAddEvent(NetMapDrawingEvent ev)` -> `Boolean`
- field `List`1 _events`
- field `Int32 _listBits`
- field `Nullable`1 drawingMode`
- field `Int32 maxEventCount`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor.MapDrawingModeChangedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `DrawingMode drawingMode`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor.MapPingMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `MapCoord coord`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor.ReactionMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `QuantizeParams _quantizeParams`
- field `Vector2 normalizedPosition`
- field `ReactionType type`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor.RestSiteOptionHoveredMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Location(RunLocation value)` -> `Void`
- field `RunLocation <Location>k__BackingField`
- field `Nullable`1 optionIndex`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.HookActionEnqueuedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `GameActionType gameActionType`
- field `UInt32 hookActionId`
- field `RunLocation location`
- field `UInt64 ownerId`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.IRunLocationTargetedMessage
- method `get_Location()` -> `RunLocation`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.MerchantCardRemovalMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Location(RunLocation value)` -> `Void`
- field `RunLocation <Location>k__BackingField`
- field `Int32 goldCost`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.PaelsWingSacrificeMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Location(RunLocation value)` -> `Void`
- field `RunLocation <Location>k__BackingField`
- field `UInt32 relicIndex`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.PlayerChoiceMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `UInt32 choiceId`
- field `NetPlayerChoiceResult result`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.RequestEnqueueActionMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `INetAction action`
- field `RunLocation location`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.RequestEnqueueHookActionMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `GameActionType gameActionType`
- field `UInt32 hookActionId`
- field `RunLocation location`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.RequestResumeActionAfterPlayerChoiceMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `UInt32 actionId`
- field `RunLocation location`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.ResumeActionAfterPlayerChoiceMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `UInt32 actionId`
- field `RunLocation location`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.RunAbandonedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync.GoldLostMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `Int32 goldLost`
- field `RunLocation location`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync.OptionIndexChosenMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `RunLocation location`
- field `UInt32 optionIndex`
- field `OptionIndexType type`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync.OptionIndexType
- field `OptionIndexType Event`
- field `OptionIndexType RestSite`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync.PeerInputMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `QuantizeParams _quantizeParams`
- field `Nullable`1 controllerFocusPosition`
- field `HoveredModelData hoveredModelData`
- field `Boolean isTargeting`
- field `Boolean isUsingController`
- field `Boolean mouseDown`
- field `Nullable`1 netMousePos`
- field `NetScreenType screenType`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync.RewardObtainedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `CardModel cardModel`
- field `Nullable`1 goldAmount`
- field `RunLocation location`
- field `PotionModel potionModel`
- field `RelicModel relicModel`
- field `RewardType rewardType`
- field `Boolean wasSkipped`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync.SharedEventOptionChosenMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `RunLocation location`
- field `UInt32 optionIndex`
- field `UInt32 pageIndex`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync.VotedForSharedEventOptionMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `RunLocation location`
- field `UInt32 optionIndex`
- field `UInt32 pageIndex`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.SyncPlayerDataMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `SerializablePlayer player`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.SyncRngMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `SerializableRunRngSet rng`
- field `SerializableRelicGrabBag sharedRelicGrabBag`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Game.TreasureChestOpenedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Location()` -> `RunLocation`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Location(RunLocation value)` -> `Void`
- field `RunLocation <Location>k__BackingField`

## MegaCrit.Sts2.Core.Multiplayer.Messages.HeartbeatRequestMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Equals(Object obj)` -> `Boolean`
- method `Equals(HeartbeatRequestMessage other)` -> `Boolean`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `GetHashCode()` -> `Int32`
- method `op_Equality(HeartbeatRequestMessage left, HeartbeatRequestMessage right)` -> `Boolean`
- method `op_Inequality(HeartbeatRequestMessage left, HeartbeatRequestMessage right)` -> `Boolean`
- method `PrintMembers(StringBuilder builder)` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `Int32 counter`

## MegaCrit.Sts2.Core.Multiplayer.Messages.HeartbeatResponseMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Equals(Object obj)` -> `Boolean`
- method `Equals(HeartbeatResponseMessage other)` -> `Boolean`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `GetHashCode()` -> `Int32`
- method `op_Equality(HeartbeatResponseMessage left, HeartbeatResponseMessage right)` -> `Boolean`
- method `op_Inequality(HeartbeatResponseMessage left, HeartbeatResponseMessage right)` -> `Boolean`
- method `PrintMembers(StringBuilder builder)` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `Int32 counter`
- field `Boolean isLoading`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientLoadJoinRequestMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientLoadJoinResponseMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `List`1 playersAlreadyConnected`
- field `SerializableRun serializableRun`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientLobbyJoinRequestMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `Int32 maxAscensionUnlocked`
- field `SerializableUnlockState unlockState`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientLobbyJoinResponseMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- field `Int32 ascension`
- field `Nullable`1 dailyTime`
- field `List`1 modifiers`
- field `List`1 playersInLobby`
- field `String seed`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientRejoinRequestMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientRejoinResponseMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `NetFullCombatState combatState`
- field `SerializableRun serializableRun`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.InitialGameInfoMessage
- method `Basic()` -> `InitialGameInfoMessage`
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `Nullable`1 connectionFailureReason`
- field `GameMode gameMode`
- field `UInt32 idDatabaseHash`
- field `List`1 mods`
- field `RunSessionState sessionState`
- field `String version`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyAscensionChangedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `Int32 ascension`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyBeginLoadedRunMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyBeginRunMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `String act1`
- field `List`1 modifiers`
- field `List`1 playersInLobby`
- field `String seed`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyModifiersChangedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `List`1 modifiers`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyPlayerChangedCharacterMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `CharacterModel character`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyPlayerSetReadyMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `Boolean ready`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbySeedChangedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `String seed`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.PlayerJoinedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `LobbyPlayer lobbyPlayer`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.PlayerLeftMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.PlayerReconnectedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.PlayerRejoinedMessage
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Multiplayer.MultiplayerDebugUtil
- method `Chunk(IReadOnlyList`1 source, Int32 size)` -> `IEnumerable`1`
- method `FormatAsHex(ReadOnlySpan`1 data, Int32 lineWidth, Int32 byteWidth)` -> `String`
- method `GetHexVal(Char hex)` -> `Int32`
- method `ReplaceControlCharactersWithDots(Byte[] characters)` -> `Byte[]`
- method `ReplaceControlCharacterWithDot(Byte character)` -> `Byte`
- method `StringToByteArray(String hex)` -> `Byte[]`

## MegaCrit.Sts2.Core.Multiplayer.MultiplayerDebugUtil+<>c
- method `<Chunk>b__0_1(Byte[] bucket)` -> `Boolean`
- method `<Chunk>b__0_2(Byte[] e)` -> `BigInteger`
- field `<>c <>9`
- field `Func`2 <>9__0_1`
- field `Func`2 <>9__0_2`

## MegaCrit.Sts2.Core.Multiplayer.MultiplayerDebugUtil+<>c__DisplayClass0_0
- method `<Chunk>b__0(Byte _, Int32 index)` -> `Byte[]`
- field `Int32 size`
- field `IReadOnlyList`1 source`

## MegaCrit.Sts2.Core.Multiplayer.MultiplayerDebugUtil+<>c__DisplayClass3_0
- method `<FormatAsHex>b__0(BigInteger v)` -> `String`
- field `Func`2 <>9__0`
- field `Int32 byteWidth`

## MegaCrit.Sts2.Core.Multiplayer.MultiplayerDebugUtil+<>O
- field `Func`2 <0>__ReplaceControlCharacterWithDot`

## MegaCrit.Sts2.Core.Multiplayer.NetClientGameService
- method `add_ConnectedToHost(Action value)` -> `Void`
- method `add_Disconnected(Action`1 value)` -> `Void`
- method `Disconnect(NetError reason, Boolean now)` -> `Void`
- method `get_HostNetId()` -> `UInt64`
- method `get_IsConnected()` -> `Boolean`
- method `get_IsGameLoading()` -> `Boolean`
- method `get_NetClient()` -> `NetClient`
- method `get_NetId()` -> `UInt64`
- method `get_Platform()` -> `PlatformType`
- method `get_Type()` -> `NetGameType`
- method `GetRawLobbyIdentifier()` -> `String`
- method `GetStatsForPeer(UInt64 peerId)` -> `ConnectionStats`
- method `Initialize(NetClient client, PlatformType platform)` -> `Void`
- method `OnConnectedToHost()` -> `Void`
- method `OnDisconnectedFromHost(UInt64 hostNetId, NetErrorInfo info)` -> `Void`
- method `OnPacketReceived(UInt64 senderId, Byte[] packetBytes, NetTransferMode mode, Int32 channel)` -> `Void`
- method `RegisterMessageHandler(MessageHandlerDelegate`1 handler)` -> `Void`
- method `remove_ConnectedToHost(Action value)` -> `Void`
- method `remove_Disconnected(Action`1 value)` -> `Void`
- method `SendMessage(T message, UInt64 playerId)` -> `Void`
- method `SendMessage(T message)` -> `Void`
- method `set_NetClient(NetClient value)` -> `Void`
- method `set_Platform(PlatformType value)` -> `Void`
- method `SetGameLoading(Boolean isLoading)` -> `Void`
- method `UnregisterMessageHandler(MessageHandlerDelegate`1 handler)` -> `Void`
- method `Update()` -> `Void`
- field `NetMessageBus _messageBus`
- field `NetQualityTracker _qualityTracker`
- field `NetClient <NetClient>k__BackingField`
- field `PlatformType <Platform>k__BackingField`
- field `Action ConnectedToHost`
- field `Action`1 Disconnected`

## MegaCrit.Sts2.Core.Multiplayer.NetConst
- field `Int32 timeoutMsec`

## MegaCrit.Sts2.Core.Multiplayer.NetHostGameService
- method `add_ClientConnected(Action`1 value)` -> `Void`
- method `add_ClientDisconnected(Action`2 value)` -> `Void`
- method `add_Disconnected(Action`1 value)` -> `Void`
- method `BroadcastMessage(T message, UInt64 excludePeerId, Int32 channel, UInt64 overrideSenderId)` -> `Void`
- method `Disconnect(NetError reason, Boolean now)` -> `Void`
- method `DisconnectClient(UInt64 peerId, NetError reason, Boolean now)` -> `Void`
- method `get_ConnectedPeers()` -> `IReadOnlyList`1`
- method `get_IsConnected()` -> `Boolean`
- method `get_IsGameLoading()` -> `Boolean`
- method `get_NetHost()` -> `NetHost`
- method `get_NetId()` -> `UInt64`
- method `get_Platform()` -> `PlatformType`
- method `get_Type()` -> `NetGameType`
- method `GetRawLobbyIdentifier()` -> `String`
- method `GetStatsForPeer(UInt64 peerId)` -> `ConnectionStats`
- method `OnDisconnected(NetErrorInfo info)` -> `Void`
- method `OnPacketReceived(UInt64 senderId, Byte[] packetBytes, NetTransferMode mode, Int32 channel)` -> `Void`
- method `OnPeerConnected(UInt64 peerId)` -> `Void`
- method `OnPeerDisconnected(UInt64 peerId, NetErrorInfo info)` -> `Void`
- method `RegisterMessageHandler(MessageHandlerDelegate`1 handler)` -> `Void`
- method `remove_ClientConnected(Action`1 value)` -> `Void`
- method `remove_ClientDisconnected(Action`2 value)` -> `Void`
- method `remove_Disconnected(Action`1 value)` -> `Void`
- method `SendMessage(T message, UInt64 peerId)` -> `Void`
- method `SendMessage(T message)` -> `Void`
- method `SendMessageToClientInternal(T message, UInt64 peerId, Int32 channel, Nullable`1 overrideSenderId)` -> `Void`
- method `set_Platform(PlatformType value)` -> `Void`
- method `SetGameLoading(Boolean isLoading)` -> `Void`
- method `SetPeerReadyForBroadcasting(UInt64 peerId)` -> `Void`
- method `StartENetHost(UInt16 port, Int32 maxClients)` -> `Nullable`1`
- method `StartSteamHost(Int32 maxClients)` -> `Task`1`
- method `UnregisterMessageHandler(MessageHandlerDelegate`1 handler)` -> `Void`
- method `Update()` -> `Void`
- field `List`1 _connectedPeers`
- field `NetMessageBus _messageBus`
- field `NetHost _netHost`
- field `NetQualityTracker _qualityTracker`
- field `PlatformType <Platform>k__BackingField`
- field `Action`1 ClientConnected`
- field `Action`2 ClientDisconnected`
- field `Action`1 Disconnected`

## MegaCrit.Sts2.Core.Multiplayer.NetHostGameService+<>c__DisplayClass45_0
- method `<OnPeerDisconnected>b__0(NetClientData p)` -> `Boolean`
- field `UInt64 peerId`

## MegaCrit.Sts2.Core.Multiplayer.NetMessageBus
- method `RegisterMessageHandler(MessageHandlerDelegate`1 handler)` -> `Void`
- method `SendMessageToAllHandlers(INetMessage message, UInt64 senderId)` -> `Void`
- method `SerializeMessage(UInt64 senderId, T message, Int32& length)` -> `Byte[]`
- method `TryDeserializeMessage(Byte[] packetBytes, INetMessage& message, Nullable`1& overrideSenderId)` -> `Boolean`
- method `UnregisterMessageHandler(MessageHandlerDelegate`1 handler)` -> `Void`
- field `List`1 _cachedPairList`
- field `Logger _logger`
- field `Dictionary`2 _messageHandlers`
- field `PacketReader _reader`
- field `PacketWriter _writer`

## MegaCrit.Sts2.Core.Multiplayer.NetMessageBus+<>c__DisplayClass10_0`1
- method `<RegisterMessageHandler>b__0(INetMessage message, UInt64 senderId)` -> `Void`
- field `MessageHandlerDelegate`1 handler`

## MegaCrit.Sts2.Core.Multiplayer.NetMessageBus+<>c__DisplayClass11_0`1
- method `<UnregisterMessageHandler>b__0(CallbackPair p)` -> `Boolean`
- field `MessageHandlerDelegate`1 handler`

## MegaCrit.Sts2.Core.Multiplayer.NetMessageBus+AnonymizedMessageHandlerDelegate
- method `BeginInvoke(INetMessage message, UInt64 senderId, AsyncCallback callback, Object object)` -> `IAsyncResult`
- method `EndInvoke(IAsyncResult result)` -> `Void`
- method `Invoke(INetMessage message, UInt64 senderId)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.NetMessageBus+CallbackPair
- field `AnonymizedMessageHandlerDelegate handler`
- field `Object originalHandler`

## MegaCrit.Sts2.Core.Multiplayer.NetReplayGameService
- method `add_Disconnected(Action`1 value)` -> `Void`
- method `Disconnect(NetError reason, Boolean now)` -> `Void`
- method `get_IsConnected()` -> `Boolean`
- method `get_IsGameLoading()` -> `Boolean`
- method `get_NetId()` -> `UInt64`
- method `get_Platform()` -> `PlatformType`
- method `get_Type()` -> `NetGameType`
- method `GetRawLobbyIdentifier()` -> `String`
- method `GetStatsForPeer(UInt64 peerId)` -> `ConnectionStats`
- method `RegisterMessageHandler(MessageHandlerDelegate`1 handler)` -> `Void`
- method `remove_Disconnected(Action`1 value)` -> `Void`
- method `SendMessage(T message, UInt64 playerId)` -> `Void`
- method `SendMessage(T message)` -> `Void`
- method `set_NetId(UInt64 value)` -> `Void`
- method `SetGameLoading(Boolean isLoading)` -> `Void`
- method `UnregisterMessageHandler(MessageHandlerDelegate`1 handler)` -> `Void`
- method `Update()` -> `Void`
- field `Boolean _isLoading`
- field `UInt64 <NetId>k__BackingField`
- field `Action`1 Disconnected`

## MegaCrit.Sts2.Core.Multiplayer.NetSingleplayerGameService
- method `add_Disconnected(Action`1 value)` -> `Void`
- method `Disconnect(NetError reason, Boolean now)` -> `Void`
- method `get_IsConnected()` -> `Boolean`
- method `get_IsGameLoading()` -> `Boolean`
- method `get_NetId()` -> `UInt64`
- method `get_Platform()` -> `PlatformType`
- method `get_Type()` -> `NetGameType`
- method `GetRawLobbyIdentifier()` -> `String`
- method `GetStatsForPeer(UInt64 peerId)` -> `ConnectionStats`
- method `RegisterMessageHandler(MessageHandlerDelegate`1 handler)` -> `Void`
- method `remove_Disconnected(Action`1 value)` -> `Void`
- method `SendMessage(T message, UInt64 playerId)` -> `Void`
- method `SendMessage(T message)` -> `Void`
- method `SetGameLoading(Boolean isLoading)` -> `Void`
- method `UnregisterMessageHandler(MessageHandlerDelegate`1 handler)` -> `Void`
- method `Update()` -> `Void`
- field `Boolean _isLoading`
- field `Int32 defaultNetId`
- field `Action`1 Disconnected`

## MegaCrit.Sts2.Core.Multiplayer.Quality.ConnectionStats
- method `GenerateHeartbeat(UInt64 timeMsec)` -> `HeartbeatRequestMessage`
- method `get_LastReceivedTime()` -> `Nullable`1`
- method `get_PacketLoss()` -> `Single`
- method `get_PeerId()` -> `UInt64`
- method `get_PingMsec()` -> `Single`
- method `get_RemoteIsLoading()` -> `Boolean`
- method `OnHeartbeatReceived(HeartbeatResponseMessage message, UInt64 timeMsec)` -> `Void`
- method `OnPacketLost()` -> `Void`
- method `set_LastReceivedTime(Nullable`1 value)` -> `Void`
- method `set_PacketLoss(Single value)` -> `Void`
- method `set_PeerId(UInt64 value)` -> `Void`
- method `set_PingMsec(Single value)` -> `Void`
- method `set_RemoteIsLoading(Boolean value)` -> `Void`
- field `Logger _logger`
- field `Int32 _nextIndex`
- field `Nullable`1[] _statuses`
- field `Single _weightedAverageFactor`
- field `Nullable`1 <LastReceivedTime>k__BackingField`
- field `Single <PacketLoss>k__BackingField`
- field `UInt64 <PeerId>k__BackingField`
- field `Single <PingMsec>k__BackingField`
- field `Boolean <RemoteIsLoading>k__BackingField`
- field `Int32 ringBufferSize`

## MegaCrit.Sts2.Core.Multiplayer.Quality.HeartbeatStatus
- field `Int32 counter`
- field `Nullable`1 receivedMsec`
- field `UInt64 sentMsec`

## MegaCrit.Sts2.Core.Multiplayer.Quality.NetQualityTracker
- method `Dispose()` -> `Void`
- method `get_IsGameLoading()` -> `Boolean`
- method `GetCurrentTime()` -> `UInt64`
- method `GetStatsForPeer(UInt64 peerId)` -> `ConnectionStats`
- method `HandleHeartbeatRequestMessage(HeartbeatRequestMessage message, UInt64 senderId)` -> `Void`
- method `HandleHeartbeatResponseMessage(HeartbeatResponseMessage message, UInt64 senderId)` -> `Void`
- method `OnPeerConnected(UInt64 peerId)` -> `Void`
- method `OnPeerDisconnected(UInt64 peerId)` -> `Void`
- method `SetIsLoading(Boolean isLoading)` -> `Void`
- method `Update()` -> `Void`
- field `Boolean _isLoading`
- field `Nullable`1 _lastLogMsec`
- field `Nullable`1 _lastUpdateMsec`
- field `Logger _logger`
- field `INetGameService _netService`
- field `List`1 _stats`
- field `Func`1 getTimeMsec`
- field `Int32 logRateMsec`
- field `Int32 sendRateMsec`

## MegaCrit.Sts2.Core.Multiplayer.Quality.NetQualityTracker+<>c__DisplayClass15_0
- method `<OnPeerDisconnected>b__0(ConnectionStats s)` -> `Boolean`
- field `UInt64 peerId`

## MegaCrit.Sts2.Core.Multiplayer.Replay.CombatReplay
- method `Anonymized()` -> `CombatReplay`
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `List`1 checksumData`
- field `List`1 choiceIds`
- field `List`1 events`
- field `String gitCommit`
- field `UInt32 modelIdHash`
- field `UInt32 nextActionId`
- field `UInt32 nextChecksumId`
- field `UInt32 nextHookId`
- field `SerializableRun serializableRun`
- field `String version`

## MegaCrit.Sts2.Core.Multiplayer.Replay.CombatReplay+<>c
- method `<Anonymized>b__12_0(CombatReplayEvent e)` -> `CombatReplayEvent`
- method `<Anonymized>b__12_1(ReplayChecksumData c)` -> `ReplayChecksumData`
- field `<>c <>9`
- field `Func`2 <>9__12_0`
- field `Func`2 <>9__12_1`

## MegaCrit.Sts2.Core.Multiplayer.Replay.CombatReplayEvent
- method `Anonymized()` -> `CombatReplayEvent`
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `INetAction action`
- field `Nullable`1 actionId`
- field `Nullable`1 choiceId`
- field `CombatReplayEventType eventType`
- field `Nullable`1 gameActionType`
- field `Nullable`1 hookId`
- field `Nullable`1 playerChoiceResult`
- field `Nullable`1 playerId`

## MegaCrit.Sts2.Core.Multiplayer.Replay.CombatReplayEventType
- field `CombatReplayEventType GameAction`
- field `CombatReplayEventType HookAction`
- field `CombatReplayEventType None`
- field `CombatReplayEventType PlayerChoice`
- field `CombatReplayEventType ResumeAction`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Multiplayer.Replay.CombatReplayWriter
- method `Dispose()` -> `Void`
- method `get_IsEnabled()` -> `Boolean`
- method `get_IsRecordingReplay()` -> `Boolean`
- method `RecordActionResume(UInt32 actionId)` -> `Void`
- method `RecordChecksum(NetChecksumData checksum, String context, NetFullCombatState fullCombatState)` -> `Void`
- method `RecordGameAction(GameAction gameAction)` -> `Void`
- method `RecordInitialState(SerializableRun serializableRun)` -> `Void`
- method `RecordPlayerChoice(Player player, UInt32 choiceId, NetPlayerChoiceResult result)` -> `Void`
- method `set_IsEnabled(Boolean value)` -> `Void`
- method `StopRecording()` -> `Void`
- method `WriteReplay(String filePath, Boolean stopRecording)` -> `Void`
- field `ActionQueueSet _actionQueueSet`
- field `ActionQueueSynchronizer _actionQueueSynchronizer`
- field `ChecksumTracker _checksumTracker`
- field `PlayerChoiceSynchronizer _playerChoiceSynchronizer`
- field `CombatReplay _replay`
- field `PacketWriter _writer`
- field `Boolean <IsEnabled>k__BackingField`

## MegaCrit.Sts2.Core.Multiplayer.Replay.ReplayChecksumData
- method `Anonymized()` -> `ReplayChecksumData`
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `NetChecksumData checksumData`
- field `String context`
- field `NetFullCombatState fullState`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.BitSerializationUtil
- method `GetBitsAtPosition(Byte[] bytes, Int32 bitPosition, Int32 bitsToObtain)` -> `Byte`
- method `GetByteMask(Int32 bits, Int32 startBit)` -> `Byte`
- method `ReadBits(Byte[] originBuffer, Int32 originBitPosition, Byte[] destinationBuffer, Int32 totalBitsToRead)` -> `Void`
- method `WriteBytes(Byte[] originBuffer, Byte[] destinationBuffer, Int32 destinationBitPosition, Int32 totalBitsToWrite)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.INetMessage
- method `get_LogLevel()` -> `LogLevel`
- method `get_Mode()` -> `NetTransferMode`
- method `get_ShouldBroadcast()` -> `Boolean`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.INetMessageSubtypes
- method `Get(Int32 i)` -> `Type`
- method `get_All()` -> `IReadOnlyList`1`
- method `get_Count()` -> `Int32`
- field `Type[] _subtypes`
- field `Type _t0`
- field `Type _t1`
- field `Type _t10`
- field `Type _t11`
- field `Type _t12`
- field `Type _t13`
- field `Type _t14`
- field `Type _t15`
- field `Type _t16`
- field `Type _t17`
- field `Type _t18`
- field `Type _t19`
- field `Type _t2`
- field `Type _t20`
- field `Type _t21`
- field `Type _t22`
- field `Type _t23`
- field `Type _t24`
- field `Type _t25`
- field `Type _t26`
- field `Type _t27`
- field `Type _t28`
- field `Type _t29`
- field `Type _t3`
- field `Type _t30`
- field `Type _t31`
- field `Type _t32`
- field `Type _t33`
- field `Type _t34`
- field `Type _t35`
- field `Type _t36`
- field `Type _t37`
- field `Type _t38`
- field `Type _t39`
- field `Type _t4`
- field `Type _t40`
- field `Type _t41`
- field `Type _t42`
- field `Type _t43`
- field `Type _t44`
- field `Type _t45`
- field `Type _t46`
- field `Type _t47`
- field `Type _t48`
- field `Type _t5`
- field `Type _t6`
- field `Type _t7`
- field `Type _t8`
- field `Type _t9`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.IPacketSerializable
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.MaxEnumValueCache
- method `Get()` -> `Int32`
- field `Dictionary`2 _maxEnumValues`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.MaxEnumValueCache+<>c__1`1
- method `<Get>b__1_0(T v)` -> `Int32`
- field `<>c__1`1 <>9`
- field `Func`2 <>9__1_0`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.MessageTypes
- method `ToId(INetMessage message)` -> `Int32`
- method `TryGetMessageType(Int32 id, Type& type)` -> `Boolean`
- method `TypeToId()` -> `Int32`
- method `TypeToId(Type type)` -> `Int32`
- field `NetTypeCache`1 _cache`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.ModelIdSerializationCache
- method `Dump()` -> `String`
- method `get_CategoryIdBitSize()` -> `Int32`
- method `get_EntryIdBitSize()` -> `Int32`
- method `get_EpochIdBitSize()` -> `Int32`
- method `get_Hash()` -> `UInt32`
- method `get_MaxCategoryId()` -> `Int32`
- method `get_MaxEntryId()` -> `Int32`
- method `get_MaxEpochId()` -> `Int32`
- method `GetCategoryForNetId(Int32 netId)` -> `String`
- method `GetEntryForNetId(Int32 netId)` -> `String`
- method `GetEpochIdForNetId(Int32 netId)` -> `String`
- method `GetNetIdForCategory(String category)` -> `Int32`
- method `GetNetIdForEntry(String entry)` -> `Int32`
- method `GetNetIdForEpochId(String epochId)` -> `Int32`
- method `Init()` -> `Void`
- method `set_CategoryIdBitSize(Int32 value)` -> `Void`
- method `set_EntryIdBitSize(Int32 value)` -> `Void`
- method `set_EpochIdBitSize(Int32 value)` -> `Void`
- method `set_Hash(UInt32 value)` -> `Void`
- method `TryGetNetIdForCategory(String category, Int32& netId)` -> `Boolean`
- method `TryGetNetIdForEntry(String entry, Int32& netId)` -> `Boolean`
- field `Dictionary`2 _categoryNameToNetIdMap`
- field `Dictionary`2 _entryNameToNetIdMap`
- field `Dictionary`2 _epochNameToNetIdMap`
- field `List`1 _netIdToCategoryNameMap`
- field `List`1 _netIdToEntryNameMap`
- field `List`1 _netIdToEpochNameMap`
- field `Int32 <CategoryIdBitSize>k__BackingField`
- field `Int32 <EntryIdBitSize>k__BackingField`
- field `Int32 <EpochIdBitSize>k__BackingField`
- field `UInt32 <Hash>k__BackingField`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.ModelIdSerializationCache+<>c
- method `<Init>b__28_0(ValueTuple`2 p1, ValueTuple`2 p2)` -> `Int32`
- method `<Init>b__28_1(ValueTuple`2 p)` -> `Type`
- field `<>c <>9`
- field `Comparison`1 <>9__28_0`
- field `Func`2 <>9__28_1`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.NetTypeCache`1
- method `TryGetTypeFromId(Int32 id, Type& type)` -> `Boolean`
- method `TypeToId()` -> `Int32`
- method `TypeToId(Type type)` -> `Int32`
- field `List`1 _idToType`
- field `Dictionary`2 _typeToId`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.NetTypeCache`1+<>c
- method `<.ctor>b__2_0(Type t1, Type t2)` -> `Int32`
- field `<>c <>9`
- field `Comparison`1 <>9__2_0`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.PacketReader
- method `get_BitPosition()` -> `Int32`
- method `get_Buffer()` -> `Byte[]`
- method `Read()` -> `T`
- method `ReadBool()` -> `Boolean`
- method `ReadByte(Int32 bits)` -> `Byte`
- method `ReadBytes(Byte[] destinationBuffer, Int32 byteCount)` -> `Void`
- method `ReadDouble()` -> `Double`
- method `ReadEnum()` -> `T`
- method `ReadFloat(Nullable`1 quantizeParams)` -> `Single`
- method `ReadInt(Int32 bits)` -> `Int32`
- method `ReadList(Int32 lengthBits)` -> `List`1`
- method `ReadLong(Int32 bits)` -> `Int64`
- method `ReadShort(Int32 bits)` -> `Int16`
- method `ReadString()` -> `String`
- method `ReadUInt(Int32 bits)` -> `UInt32`
- method `ReadULong(Int32 bits)` -> `UInt64`
- method `ReadUShort(Int32 bits)` -> `UInt16`
- method `ReadVector2(Nullable`1 quantizeParamsX, Nullable`1 quantizeParamsY)` -> `Vector2`
- method `Reset(Byte[] buffer)` -> `Void`
- method `set_BitPosition(Int32 value)` -> `Void`
- method `set_Buffer(Byte[] value)` -> `Void`
- method `Unquantize(UInt32 value, Single min, Single max, Int32 bitLength)` -> `Single`
- field `Byte[] _stringBuffer`
- field `Byte[] _tempBuffer`
- field `Int32 <BitPosition>k__BackingField`
- field `Byte[] <Buffer>k__BackingField`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.PacketReaderExtensions
- method `ReadEpoch(PacketReader reader)` -> `EpochModel`
- method `ReadEpochId(PacketReader reader)` -> `String`
- method `ReadFullModelId(PacketReader reader)` -> `ModelId`
- method `ReadFullModelIdList(PacketReader reader)` -> `List`1`
- method `ReadModel(PacketReader reader)` -> `T`
- method `ReadModelIdAssumingType(PacketReader reader)` -> `ModelId`
- method `ReadModelIdListAssumingType(PacketReader reader)` -> `List`1`
- method `ReadModelList(PacketReader reader)` -> `List`1`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.PacketWriter
- method `get_BitPosition()` -> `Int32`
- method `get_Buffer()` -> `Byte[]`
- method `get_BytePosition()` -> `Int32`
- method `get_WarnOnGrow()` -> `Boolean`
- method `Quantize(Single value, Single min, Single max, Int32 bitLength)` -> `UInt32`
- method `Reset()` -> `Void`
- method `ResizeBufferIfNecessary(Int32 bitsBeingWritten)` -> `Void`
- method `set_BitPosition(Int32 value)` -> `Void`
- method `set_Buffer(Byte[] value)` -> `Void`
- method `set_WarnOnGrow(Boolean value)` -> `Void`
- method `Write(T val)` -> `Void`
- method `WriteBool(Boolean val)` -> `Void`
- method `WriteByte(Byte val, Int32 bits)` -> `Void`
- method `WriteBytes(Byte[] bytes, Int32 byteCount)` -> `Void`
- method `WriteDouble(Double val)` -> `Void`
- method `WriteEnum(T val)` -> `Void`
- method `WriteFloat(Single val, Nullable`1 quantizeParams)` -> `Void`
- method `WriteInt(Int32 val, Int32 bits)` -> `Void`
- method `WriteList(IReadOnlyList`1 list, Int32 lengthBits)` -> `Void`
- method `WriteLong(Int64 val, Int32 bits)` -> `Void`
- method `WriteShort(Int16 val, Int32 bits)` -> `Void`
- method `WriteString(String str)` -> `Void`
- method `WriteUInt(UInt32 val, Int32 bits)` -> `Void`
- method `WriteULong(UInt64 val, Int32 bits)` -> `Void`
- method `WriteUShort(UInt16 val, Int32 bits)` -> `Void`
- method `WriteVector2(Vector2 val, Nullable`1 quantizeParamsX, Nullable`1 quantizeParamsY)` -> `Void`
- method `ZeroByteRemainder()` -> `Void`
- field `Byte[] _stringBuffer`
- field `Byte[] _tempBuffer`
- field `Int32 <BitPosition>k__BackingField`
- field `Byte[] <Buffer>k__BackingField`
- field `Boolean <WarnOnGrow>k__BackingField`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.PacketWriterExtensions
- method `WriteEpoch(PacketWriter writer)` -> `Void`
- method `WriteEpoch(PacketWriter writer, EpochModel epochModel)` -> `Void`
- method `WriteEpochId(PacketWriter writer, String epochId)` -> `Void`
- method `WriteFullModelId(PacketWriter writer, ModelId id)` -> `Void`
- method `WriteFullModelIdList(PacketWriter writer, IReadOnlyCollection`1 models)` -> `Void`
- method `WriteModel(PacketWriter writer, T model)` -> `Void`
- method `WriteModelEntriesInList(PacketWriter writer, IReadOnlyCollection`1 modelIds)` -> `Void`
- method `WriteModelEntry(PacketWriter writer, ModelId id)` -> `Void`
- method `WriteModelList(PacketWriter writer, IReadOnlyCollection`1 models)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Serialization.QuantizeParams
- field `Int32 bits`
- field `Single max`
- field `Single min`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetClient
- method `AssertClientStarted()` -> `Void`
- method `ConnectToHost(UInt64 netId, String ip, UInt16 port, CancellationToken cancelToken)` -> `Task`1`
- method `DisconnectFromHost(NetError reason, Boolean now)` -> `Void`
- method `get_HostNetId()` -> `UInt64`
- method `get_IsConnected()` -> `Boolean`
- method `get_NetId()` -> `UInt64`
- method `GetRawLobbyIdentifier()` -> `String`
- method `HandleMessageReceived(ENetServiceData data)` -> `Void`
- method `SendAndWaitForNetIdAck(UInt64 netId, List`1 bufferedPackets, CancellationToken cancelToken)` -> `Task`1`
- method `SendMessageToHost(Byte[] bytes, Int32 length, NetTransferMode mode, Int32 channel)` -> `Void`
- method `Update()` -> `Void`
- field `ENetConnection _connection`
- field `Int32 _handshakeTimeoutMsec`
- field `Int32 _handshakeUpdateRateMsec`
- field `Boolean _isConnected`
- field `Logger _logger`
- field `UInt64 _netId`
- field `ENetPacketPeer _peer`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetClient+<ConnectToHost>d__14
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `ENetClient <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter <>u__1`
- field `TaskAwaiter`1 <>u__2`
- field `List`1 <bufferedPackets>5__3`
- field `Int32 <timeoutTimer>5__2`
- field `CancellationToken cancelToken`
- field `String ip`
- field `UInt64 netId`
- field `UInt16 port`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetClient+<SendAndWaitForNetIdAck>d__15
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `ENetClient <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter <>u__1`
- field `Boolean <receivedAck>5__2`
- field `Int32 <timeoutTimer>5__3`
- field `List`1 bufferedPackets`
- field `CancellationToken cancelToken`
- field `UInt64 netId`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetConnectionExtension
- method `TryService(ENetConnection connection, Nullable`1& output)` -> `Boolean`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetDisconnection
- field `NetError reason`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetHandshakeRequest
- field `UInt64 netId`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetHandshakeResponse
- field `UInt64 netId`
- field `ENetHandshakeStatus status`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetHandshakeStatus
- field `ENetHandshakeStatus IdCollision`
- field `ENetHandshakeStatus Success`
- field `Byte value__`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetHost
- method `AssertHostStarted()` -> `Void`
- method `DisconnectClient(UInt64 peerId, NetError reason, Boolean now)` -> `Void`
- method `DisconnectClientInternal(UInt64 peerId, NetError reason, Boolean now, Boolean notifyHandler)` -> `Void`
- method `DoClientHandshake(ENetPacketPeer peer)` -> `Task`
- method `get_ConnectedPeerIds()` -> `IEnumerable`1`
- method `get_IsConnected()` -> `Boolean`
- method `get_NetId()` -> `UInt64`
- method `GetConnectionById(UInt64 id)` -> `Nullable`1`
- method `GetConnectionByPeer(ENetPacketPeer peer)` -> `Nullable`1`
- method `GetRawLobbyIdentifier()` -> `String`
- method `HandleClientDisconnection(ClientConnection conn, NetError reason, Boolean notifyHandler)` -> `Void`
- method `HandlePacketReceived(ENetServiceData data)` -> `Void`
- method `SendMessageToAll(Byte[] bytes, Int32 length, NetTransferMode mode, Int32 channel)` -> `Void`
- method `SendMessageToClient(UInt64 peerId, Byte[] bytes, Int32 length, NetTransferMode mode, Int32 channel)` -> `Void`
- method `SetHostIsClosed(Boolean isClosed)` -> `Void`
- method `StartHost(UInt16 port, Int32 maxClients)` -> `Nullable`1`
- method `StopHost(NetError reason, Boolean now)` -> `Void`
- method `Update()` -> `Void`
- field `List`1 _connectedPeers`
- field `ENetConnection _connection`
- field `Int32 _handshakeTimeoutMsec`
- field `Int32 _handshakeUpdateRateMsec`
- field `Boolean _isConnected`
- field `Logger _logger`
- field `List`1 _receivedHandshakes`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetHost+<>c
- method `<get_ConnectedPeerIds>b__8_0(ClientConnection c)` -> `UInt64`
- field `<>c <>9`
- field `Func`2 <>9__8_0`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetHost+<DoClientHandshake>d__17
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `ENetHost <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `Nullable`1 <handshake>5__3`
- field `Int32 <timeoutTimer>5__2`
- field `ENetPacketPeer peer`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetHost+ClientConnection
- method `Equals(Object obj)` -> `Boolean`
- method `Equals(ClientConnection other)` -> `Boolean`
- method `GetHashCode()` -> `Int32`
- method `op_Equality(ClientConnection left, ClientConnection right)` -> `Boolean`
- method `op_Inequality(ClientConnection left, ClientConnection right)` -> `Boolean`
- method `PrintMembers(StringBuilder builder)` -> `Boolean`
- method `ToString()` -> `String`
- field `UInt64 netId`
- field `ENetPacketPeer peer`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetHost+HandshakeAwaitingResponse
- field `ClientConnection conn`
- field `UInt64 receivedMsec`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetPacket
- method `AsAppMessage()` -> `Byte[]`
- method `AsDisconnection()` -> `ENetDisconnection`
- method `AsHandshakeRequest()` -> `ENetHandshakeRequest`
- method `AsHandshakeResponse()` -> `ENetHandshakeResponse`
- method `FromAppMessage(Byte[] messageBytes, Int32 count)` -> `ENetPacket`
- method `FromDisconnection(ENetDisconnection disconnection)` -> `ENetPacket`
- method `FromHandshakeRequest(ENetHandshakeRequest request)` -> `ENetPacket`
- method `FromHandshakeResponse(ENetHandshakeResponse response)` -> `ENetPacket`
- method `get_AllBytes()` -> `Byte[]`
- method `get_PacketType()` -> `ENetPacketType`
- field `Byte[] _packetBytes`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetPacketType
- field `ENetPacketType ApplicationMessage`
- field `ENetPacketType Disconnection`
- field `ENetPacketType HandshakeRequest`
- field `ENetPacketType HandshakeResponse`
- field `Byte value__`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetServiceData
- field `Int32 channel`
- field `Error error`
- field `NetTransferMode mode`
- field `Array originalData`
- field `Byte[] packetData`
- field `ENetPacketPeer peer`
- field `EventType type`

## MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetUtil
- method `FlagsFromMode(NetTransferMode mode)` -> `Int32`
- method `ModeFromFlags(Int32 flags)` -> `NetTransferMode`

## MegaCrit.Sts2.Core.Multiplayer.Transport.INetClientHandler
- method `OnConnectedToHost()` -> `Void`
- method `OnDisconnectedFromHost(UInt64 hostNetId, NetErrorInfo info)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Transport.INetHandler
- method `OnPacketReceived(UInt64 senderId, Byte[] packetBytes, NetTransferMode mode, Int32 channel)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Transport.INetHostHandler
- method `OnDisconnected(NetErrorInfo info)` -> `Void`
- method `OnPeerConnected(UInt64 peerId)` -> `Void`
- method `OnPeerDisconnected(UInt64 peerId, NetErrorInfo info)` -> `Void`

## MegaCrit.Sts2.Core.Multiplayer.Transport.NetClient
- method `DisconnectFromHost(NetError reason, Boolean now)` -> `Void`
- method `get_HostNetId()` -> `UInt64`
- method `get_IsConnected()` -> `Boolean`
- method `get_NetId()` -> `UInt64`
- method `GetRawLobbyIdentifier()` -> `String`
- method `SendMessageToHost(Byte[] bytes, Int32 length, NetTransferMode mode, Int32 channel)` -> `Void`
- method `Update()` -> `Void`
- field `INetClientHandler _handler`

## MegaCrit.Sts2.Core.Multiplayer.Transport.NetHost
- method `DisconnectClient(UInt64 peerId, NetError reason, Boolean now)` -> `Void`
- method `get_ConnectedPeerIds()` -> `IEnumerable`1`
- method `get_IsConnected()` -> `Boolean`
- method `get_NetId()` -> `UInt64`
- method `GetRawLobbyIdentifier()` -> `String`
- method `SendMessageToAll(Byte[] bytes, Int32 length, NetTransferMode mode, Int32 channel)` -> `Void`
- method `SendMessageToClient(UInt64 peerId, Byte[] bytes, Int32 length, NetTransferMode mode, Int32 channel)` -> `Void`
- method `SetHostIsClosed(Boolean isClosed)` -> `Void`
- method `StopHost(NetError reason, Boolean now)` -> `Void`
- method `Update()` -> `Void`
- field `INetHostHandler _handler`

## MegaCrit.Sts2.Core.Multiplayer.Transport.NetTransferMode
- field `NetTransferMode None`
- field `NetTransferMode Reliable`
- field `NetTransferMode Unreliable`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Multiplayer.Transport.NetTransferModeExtensions
- method `ToChannelId(NetTransferMode mode)` -> `Int32`

## MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamCallResult`1
- method `Cancel()` -> `Void`
- method `Dispose()` -> `Void`
- method `get_Task()` -> `Task`1`
- method `OnCallResult(T result, Boolean ioError)` -> `Void`
- field `CallResult`1 _callResult`
- field `Nullable`1 _cancelTokenRegistration`
- field `TaskCompletionSource`1 _completionSource`

## MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamClient
- method `CancelConnection()` -> `Void`
- method `CleanupConnection(Int32 closeReason, String debugReason)` -> `Void`
- method `CleanupLobby()` -> `Void`
- method `ConnectToLobby(UInt64 lobbyId, CancellationToken cancelToken)` -> `Task`1`
- method `ConnectToLobbyOwnedByFriend(UInt64 steamPlayerId, CancellationToken cancelToken)` -> `Task`1`
- method `DisconnectFromHost(NetError reason, Boolean now)` -> `Void`
- method `DisconnectFromHostInternal(SteamDisconnectionReason reason, String debugReason, Boolean now, Boolean selfInitiated)` -> `Void`
- method `get_HostNetId()` -> `UInt64`
- method `get_IsConnected()` -> `Boolean`
- method `get_LobbyId()` -> `Nullable`1`
- method `get_NetId()` -> `UInt64`
- method `GetRawLobbyIdentifier()` -> `String`
- method `HandleDisconnection(SteamDisconnectionReason reason, String debugReason)` -> `Void`
- method `OnNetStatusChanged(SteamNetConnectionStatusChangedCallback_t data)` -> `Void`
- method `SendMessageToHost(Byte[] bytes, Int32 length, NetTransferMode mode, Int32 channel)` -> `Void`
- method `Update()` -> `Void`
- field `Nullable`1 _conn`
- field `TaskCompletionSource`1 _connectingTaskCompletionSource`
- field `CSteamID _hostNetId`
- field `Boolean _isConnected`
- field `Nullable`1 _lobbyId`
- field `Logger _logger`
- field `Callback`1 _netStatusChangedCallback`

## MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamClient+<ConnectToLobby>d__18
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `SteamClient <>4__this`
- field `Object <>7__wrap4`
- field `Int32 <>7__wrap5`
- field `Nullable`1 <>7__wrap6`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `TaskAwaiter`1 <>u__2`
- field `ValueTaskAwaiter <>u__3`
- field `SteamCallResult`1 <callResult>5__3`
- field `CancellationTokenRegistration <cancelRegister>5__2`
- field `CSteamID <lobbyOwnerId>5__4`
- field `CancellationToken cancelToken`
- field `UInt64 lobbyId`

## MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamClient+ConnectionResult
- field `Nullable`1 connection`
- field `String debugReason`
- field `Nullable`1 disconnectionReason`

## MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamHost
- method `CloseConnectionAndRemove(HSteamNetConnection conn, SteamDisconnectionReason reason, String debugReason, Boolean now, Boolean selfInitiated)` -> `Void`
- method `CloseSocketAfterDelay(HSteamListenSocket socket)` -> `Task`
- method `DisconnectClient(UInt64 peerId, NetError reason, Boolean now)` -> `Void`
- method `get_ConnectedPeerIds()` -> `IEnumerable`1`
- method `get_IsConnected()` -> `Boolean`
- method `get_LobbyId()` -> `Nullable`1`
- method `get_NetId()` -> `UInt64`
- method `GetConnectionForNetId(UInt64 peerId)` -> `Nullable`1`
- method `GetRawLobbyIdentifier()` -> `String`
- method `IsInLobby(SteamNetworkingIdentity id)` -> `Boolean`
- method `OnNetStatusChanged(SteamNetConnectionStatusChangedCallback_t data)` -> `Void`
- method `SendMessageToAll(Byte[] bytes, Int32 length, NetTransferMode mode, Int32 channel)` -> `Void`
- method `SendMessageToClient(UInt64 peerId, Byte[] bytes, Int32 length, NetTransferMode mode, Int32 channel)` -> `Void`
- method `SetHostIsClosed(Boolean isClosed)` -> `Void`
- method `StartHost(Int32 maxPlayers)` -> `Task`1`
- method `StopHost(NetError reason, Boolean now)` -> `Void`
- method `Update()` -> `Void`
- field `List`1 _connections`
- field `List`1 _connectionsCache`
- field `Boolean _isConnected`
- field `Nullable`1 _lobbyId`
- field `Logger _logger`
- field `Callback`1 _netStatusChangedCallback`
- field `HSteamListenSocket _socket`

## MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamHost+<>c
- method `<get_ConnectedPeerIds>b__6_0(ClientConnection c)` -> `UInt64`
- field `<>c <>9`
- field `Func`2 <>9__6_0`

## MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamHost+<>c__DisplayClass25_0
- method `<CloseConnectionAndRemove>b__0(ClientConnection c)` -> `Boolean`
- field `HSteamNetConnection conn`

## MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamHost+<CloseSocketAfterDelay>d__27
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `SteamHost <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `HSteamListenSocket socket`

## MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamHost+<StartHost>d__17
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `SteamHost <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `SteamCallResult`1 <callResult>5__2`
- field `Int32 maxPlayers`

## MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamHost+ClientConnection
- field `HSteamNetConnection conn`
- field `SteamNetworkingIdentity netId`

## MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamNetworkingSend
- field `SteamNetworkingSend NoDelay`
- field `SteamNetworkingSend NoNagle`
- field `SteamNetworkingSend Reliable`
- field `SteamNetworkingSend Unreliable`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamUtil
- method `FlagsFromMode(NetTransferMode mode)` -> `Int32`
- method `ModeFromFlags(Int32 flags)` -> `NetTransferMode`
- method `ProcessMessages(HSteamNetConnection conn, INetHandler handler, Logger logger)` -> `Void`
- method `ToNetId(CSteamID id)` -> `SteamNetworkingIdentity`
- method `ToNetId64(UInt64 id)` -> `SteamNetworkingIdentity`
- field `IntPtr[] _messageBuffer`
- field `UInt32 handshakeMagicBytes`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer
- method `AnimVoteIn(VoteIcon vote, Boolean animate)` -> `Void`
- method `AnimVoteOut(VoteIcon vote, Boolean animate)` -> `Void`
- method `BouncePlayers()` -> `Void`
- method `get_AssetPaths()` -> `IEnumerable`1`
- method `get_Players()` -> `IEnumerable`1`
- method `GetGodotMethodList()` -> `List`1`
- method `GetVoteIndex(Player player)` -> `Int32`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `Initialize(PlayerVotedDelegate del, IReadOnlyList`1 players)` -> `Void`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `RefreshPlayerVotes(Boolean animate)` -> `Void`
- method `RemoveVoteAfterAnimation(VoteIcon vote)` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetPlayerHighlighted(Player player, Boolean isHighlighted)` -> `Void`
- field `List`1 _allPlayers`
- field `List`1 _iconsAnimatingOut`
- field `PlayerVotedDelegate _playerVotedDelegate`
- field `String _voteIconPath`
- field `List`1 _votes`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer+<>c
- method `<get_Players>b__9_0(VoteIcon v)` -> `Player`
- field `<>c <>9`
- field `Func`2 <>9__9_0`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer+<>c__DisplayClass12_0
- method `<RefreshPlayerVotes>b__0(VoteIcon p)` -> `Boolean`
- field `Player player`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer+<>c__DisplayClass13_0
- method `<GetVoteIndex>b__0(VoteIcon v)` -> `Boolean`
- field `Player player`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer+<>c__DisplayClass14_0
- method `<SetPlayerHighlighted>b__0(VoteIcon v)` -> `Boolean`
- field `Player player`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer+<>c__DisplayClass16_0
- method `<AnimVoteIn>b__0(VoteIcon i)` -> `Boolean`
- field `VoteIcon vote`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer+<>c__DisplayClass17_0
- method `<AnimVoteOut>b__0()` -> `Void`
- field `NMultiplayerVoteContainer <>4__this`
- field `VoteIcon vote`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer+MethodName
- field `StringName BouncePlayers`
- field `StringName RefreshPlayerVotes`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer+PlayerVotedDelegate
- method `BeginInvoke(Player player, AsyncCallback callback, Object object)` -> `IAsyncResult`
- method `EndInvoke(IAsyncResult result)` -> `Boolean`
- method `Invoke(Player player)` -> `Boolean`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer+PropertyName

## MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer+SignalName

## MegaCrit.Sts2.Core.Nodes.CommonUi.NMultiplayerVoteContainer+VoteIcon
- method `<Clone>$()` -> `VoteIcon`
- method `Equals(Object obj)` -> `Boolean`
- method `Equals(VoteIcon other)` -> `Boolean`
- method `get_EqualityContract()` -> `Type`
- method `GetHashCode()` -> `Int32`
- method `op_Equality(VoteIcon left, VoteIcon right)` -> `Boolean`
- method `op_Inequality(VoteIcon left, VoteIcon right)` -> `Boolean`
- method `PrintMembers(StringBuilder builder)` -> `Boolean`
- method `ToString()` -> `String`
- field `TextureRect node`
- field `Player player`
- field `Tween tween`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NSaveIndicator
- method `_EnterTree()` -> `Void`
- method `_ExitTree()` -> `Void`
- method `_Ready()` -> `Void`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SavedGame()` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- field `Tween _tween`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NSaveIndicator+MethodName
- field `StringName _EnterTree`
- field `StringName _ExitTree`
- field `StringName _Ready`
- field `StringName SavedGame`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NSaveIndicator+PropertyName
- field `StringName _tween`

## MegaCrit.Sts2.Core.Nodes.CommonUi.NSaveIndicator+SignalName

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest
- method `_ExitTree()` -> `Void`
- method `_Process(Double delta)` -> `Void`
- method `_Ready()` -> `Void`
- method `AddGame()` -> `Void`
- method `AfterMultiplayerStarted()` -> `Void`
- method `AscensionChanged()` -> `Void`
- method `BeginRun(String seed, List`1 acts, IReadOnlyList`1 __)` -> `Void`
- method `BeginRunAsync(String seed, List`1 acts)` -> `Task`
- method `BeginRunAsyncWrapper(String seed, List`1 acts)` -> `Task`
- method `ChooseReplayToLoad()` -> `Void`
- method `Disconnect(NetError reason)` -> `Void`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `HostButtonPressed()` -> `Void`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `JoinButtonPressed()` -> `Void`
- method `JoinToHost(IClientConnectionInitializer initializer)` -> `Task`
- method `LoadReplay(String path)` -> `Void`
- method `LocalPlayerDisconnected(NetErrorInfo info)` -> `Void`
- method `MaxAscensionChanged()` -> `Void`
- method `ModifiersChanged()` -> `Void`
- method `OnCharacterChanged(CharacterModel model)` -> `Void`
- method `PlayerChanged(LobbyPlayer player, Boolean isRandomCharacterResolution)` -> `Void`
- method `PlayerConnected(LobbyPlayer player)` -> `Void`
- method `ReadyButtonPressed()` -> `Void`
- method `RemotePlayerDisconnected(LobbyPlayer player)` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `RunReplay(CombatReplay replay)` -> `Task`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SeedChanged()` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `StartHost(Boolean steam)` -> `Task`1`
- method `SteamHostButtonPressed()` -> `Void`
- field `Boolean _beginningRun`
- field `List`1 _characterContainers`
- field `NMultiplayerTestCharacterPaginator _characterPaginator`
- field `NGame _game`
- field `TextEdit _idField`
- field `Boolean _ignoreReplayModelIdHash`
- field `TextEdit _ipField`
- field `Control _loadingPanel`
- field `StartRunLobby _lobby`
- field `SerializablePlayer _localPlayerData`
- field `UInt16 _port`
- field `Button _readyButton`
- field `Control _readyIndicator`
- field `IBootstrapSettings _settings`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest+<>c
- method `<AfterMultiplayerStarted>b__28_0(LobbyPlayer p)` -> `UInt64`
- method `<BeginRunAsync>b__24_0(LobbyPlayer p)` -> `Player`
- method `<BeginRunAsync>b__24_1(ActModel a)` -> `ActModel`
- method `<BeginRunAsync>b__24_2(Player p)` -> `CharacterModel`
- method `<ReadyButtonPressed>b__21_0(CardModel c)` -> `SerializableCard`
- method `<ReadyButtonPressed>b__21_1(RelicModel r)` -> `SerializableRelic`
- method `<RunReplay>b__31_0(Player p)` -> `CharacterModel`
- field `<>c <>9`
- field `Func`2 <>9__21_0`
- field `Func`2 <>9__21_1`
- field `Func`2 <>9__24_0`
- field `Func`2 <>9__24_1`
- field `Func`2 <>9__24_2`
- field `Func`2 <>9__28_0`
- field `Func`2 <>9__31_0`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest+<>O
- field `Action <0>__DeleteCloudSaves`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest+<BeginRunAsync>d__24
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerTest <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `TaskAwaiter`1 <>u__2`
- field `TaskAwaiter`1 <>u__3`
- field `NetLoadingHandle <loadHandle>5__2`
- field `RunState <runState>5__3`
- field `List`1 acts`
- field `String seed`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest+<BeginRunAsyncWrapper>d__23
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerTest <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `List`1 acts`
- field `String seed`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest+<JoinToHost>d__27
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerTest <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `JoinFlow <joinFlow>5__2`
- field `IClientConnectionInitializer initializer`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest+<RunReplay>d__31
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerTest <>4__this`
- field `Enumerator <>7__wrap2`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `Object <>u__2`
- field `GameAction <action>5__5`
- field `CombatReplayEvent <replayEvent>5__4`
- field `RunState <runState>5__2`
- field `CombatReplay replay`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest+<StartHost>d__26
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerTest <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `NetHostGameService <netService>5__2`
- field `Boolean steam`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest+CharacterContainer
- field `TextureRect characterImage`
- field `Label playerName`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest+MethodName
- field `StringName _ExitTree`
- field `StringName _Process`
- field `StringName _Ready`
- field `StringName AddGame`
- field `StringName AfterMultiplayerStarted`
- field `StringName AscensionChanged`
- field `StringName ChooseReplayToLoad`
- field `StringName Disconnect`
- field `StringName HostButtonPressed`
- field `StringName JoinButtonPressed`
- field `StringName LoadReplay`
- field `StringName MaxAscensionChanged`
- field `StringName ModifiersChanged`
- field `StringName ReadyButtonPressed`
- field `StringName SeedChanged`
- field `StringName SteamHostButtonPressed`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest+PropertyName
- field `StringName _beginningRun`
- field `StringName _characterPaginator`
- field `StringName _game`
- field `StringName _idField`
- field `StringName _ignoreReplayModelIdHash`
- field `StringName _ipField`
- field `StringName _loadingPanel`
- field `StringName _readyButton`
- field `StringName _readyIndicator`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest+SignalName

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTestCharacterPaginator
- method `_Ready()` -> `Void`
- method `add_CharacterChanged(Action`1 value)` -> `Void`
- method `get_Character()` -> `CharacterModel`
- method `GetGodotMethodList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `OnIndexChanged(Int32 index)` -> `Void`
- method `remove_CharacterChanged(Action`1 value)` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- field `CharacterModel[] _characters`
- field `Action`1 CharacterChanged`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTestCharacterPaginator+MethodName
- field `StringName _Ready`
- field `StringName OnIndexChanged`

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTestCharacterPaginator+PropertyName

## MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTestCharacterPaginator+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NGenericPopup
- method `Create()` -> `NGenericPopup`
- method `get_AssetPaths()` -> `IEnumerable`1`
- method `get_DefaultFocusedControl()` -> `Control`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `InvokeGodotClassStaticMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `OnNoButtonPressed(NButton _)` -> `Void`
- method `OnYesButtonPressed(NButton _)` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `WaitForConfirmation(LocString body, LocString header, LocString noButton, LocString yesButton)` -> `Task`1`
- field `TaskCompletionSource`1 _confirmationCompletionSource`
- field `String _scenePath`
- field `UInt64 _steamId`
- field `NVerticalPopup _verticalPopup`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NGenericPopup+MethodName
- field `StringName Create`
- field `StringName OnNoButtonPressed`
- field `StringName OnYesButtonPressed`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NGenericPopup+PropertyName
- field `StringName _steamId`
- field `StringName _verticalPopup`
- field `StringName DefaultFocusedControl`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NGenericPopup+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NInvitePlayersButton
- method `_ExitTree()` -> `Void`
- method `_Ready()` -> `Void`
- method `get_Hotkeys()` -> `String[]`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `Initialize(StartRunLobby lobby)` -> `Void`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `OnFocus()` -> `Void`
- method `OnPlayerConnected(LobbyPlayer player)` -> `Void`
- method `OnPlayerDisconnected(LobbyPlayer player)` -> `Void`
- method `OnRelease()` -> `Void`
- method `OnUnfocus()` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `UpdateVisibility()` -> `Void`
- field `Control _container`
- field `StringName _s`
- field `ShaderMaterial _shaderMaterial`
- field `StartRunLobby _startRunLobby`
- field `StringName _v`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NInvitePlayersButton+MethodName
- field `StringName _ExitTree`
- field `StringName _Ready`
- field `StringName OnFocus`
- field `StringName OnRelease`
- field `StringName OnUnfocus`
- field `StringName UpdateVisibility`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NInvitePlayersButton+PropertyName
- field `StringName _container`
- field `StringName _shaderMaterial`
- field `StringName Hotkeys`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NInvitePlayersButton+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerCardIntent
- method `_Ready()` -> `Void`
- method `get_Card()` -> `CardModel`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `set_Card(CardModel value)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- field `CardModel _card`
- field `NCard _cardNode`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerCardIntent+MethodName
- field `StringName _Ready`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerCardIntent+PropertyName
- field `StringName _cardNode`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerCardIntent+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerNetworkProblemIndicator
- method `get_IsShown()` -> `Boolean`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `Initialize(UInt64 peerId)` -> `Void`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `set_IsShown(Boolean value)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `UpdateLoop()` -> `Task`
- field `UInt64 _peerId`
- field `Single _qualityScoreToShowAt`
- field `Tween _tween`
- field `Boolean <IsShown>k__BackingField`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerNetworkProblemIndicator+<UpdateLoop>d__8
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerNetworkProblemIndicator <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerNetworkProblemIndicator+MethodName
- field `StringName Initialize`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerNetworkProblemIndicator+PropertyName
- field `StringName _peerId`
- field `StringName _tween`
- field `StringName IsShown`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerNetworkProblemIndicator+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerExpandedState
- method `_Input(InputEvent inputEvent)` -> `Void`
- method `_Ready()` -> `Void`
- method `AfterCapstoneClosed()` -> `Void`
- method `AfterCapstoneOpened()` -> `Void`
- method `BackButtonPressed(NButton _)` -> `Void`
- method `Create(Player player)` -> `NMultiplayerPlayerExpandedState`
- method `get_DefaultFocusedControl()` -> `Control`
- method `get_ScreenType()` -> `NetScreenType`
- method `get_UseSharedBackstop()` -> `Boolean`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `OnRelicClicked(NRelic node)` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `ShowEntry(NDeckHistoryEntry entry)` -> `Void`
- method `UpdateNavigation()` -> `Void`
- field `NBackButton _backButton`
- field `Control _cardContainer`
- field `List`1 _cards`
- field `MegaRichTextLabel _cardsHeader`
- field `Player _player`
- field `MegaRichTextLabel _playerNameLabel`
- field `Control _potionContainer`
- field `MegaRichTextLabel _potionsHeader`
- field `Control _relicContainer`
- field `MegaRichTextLabel _relicsHeader`
- field `String _scenePath`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerExpandedState+<>c
- method `<_Ready>b__19_1(CardModel x)` -> `CardGroupKey`
- field `<>c <>9`
- field `Func`2 <>9__19_1`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerExpandedState+<>c__DisplayClass19_0
- method `<_Ready>b__0(NButton _)` -> `Void`
- field `NMultiplayerPlayerExpandedState <>4__this`
- field `NRelicBasicHolder holder`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerExpandedState+CardGroupKey
- method `Equals(Object obj)` -> `Boolean`
- method `GetHashCode()` -> `Int32`
- field `CardModel _card`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerExpandedState+MethodName
- field `StringName _Input`
- field `StringName _Ready`
- field `StringName AfterCapstoneClosed`
- field `StringName AfterCapstoneOpened`
- field `StringName BackButtonPressed`
- field `StringName OnRelicClicked`
- field `StringName ShowEntry`
- field `StringName UpdateNavigation`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerExpandedState+PropertyName
- field `StringName _backButton`
- field `StringName _cardContainer`
- field `StringName _cardsHeader`
- field `StringName _playerNameLabel`
- field `StringName _potionContainer`
- field `StringName _potionsHeader`
- field `StringName _relicContainer`
- field `StringName _relicsHeader`
- field `StringName DefaultFocusedControl`
- field `StringName ScreenType`
- field `StringName UseSharedBackstop`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerExpandedState+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerIntentHandler
- method `_ExitTree()` -> `Void`
- method `_Process(Double delta)` -> `Void`
- method `_Ready()` -> `Void`
- method `BeforeActionExecuted(GameAction action)` -> `Void`
- method `BeforeActionPausedForPlayerChoice(GameAction action)` -> `Void`
- method `BeforeActionReadyToResumeAfterPlayerChoice(GameAction action)` -> `Void`
- method `Create(Player player)` -> `NMultiplayerPlayerIntentHandler`
- method `get_CardIntent()` -> `NMultiplayerCardIntent`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `HideThinkyDots()` -> `Void`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `OnActionEnqueued(GameAction action)` -> `Void`
- method `OnHitboxEntered()` -> `Void`
- method `OnHitboxExited()` -> `Void`
- method `OnHoverChanged(UInt64 playerId)` -> `Void`
- method `OnPeerInputStateChanged(UInt64 playerId)` -> `Void`
- method `OnPeerInputStateRemoved(UInt64 playerId)` -> `Void`
- method `RefreshHoverDisplay()` -> `Void`
- method `RefreshHoverTips()` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `UnsubscribeFromAction(GameAction action)` -> `Void`
- field `NCard _cardInPlayAwaitingPlayerChoice`
- field `NMultiplayerCardIntent _cardIntent`
- field `MegaRichTextLabel _cardThinkyDots`
- field `AbstractModel _displayedModel`
- field `Control _hitbox`
- field `NHoverTipSet _hoverTips`
- field `Boolean _isInPlayerChoice`
- field `Player _player`
- field `NPotion _potionIntent`
- field `MegaRichTextLabel _potionThinkyDots`
- field `NPower _powerIntent`
- field `MegaRichTextLabel _powerThinkyDots`
- field `NRelic _relicIntent`
- field `MegaRichTextLabel _relicThinkyDots`
- field `String _scenePath`
- field `Boolean _shouldShowHoverTip`
- field `NRemoteTargetingIndicator _targetingIndicator`
- field `Tween _tween`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerIntentHandler+MethodName
- field `StringName _ExitTree`
- field `StringName _Process`
- field `StringName _Ready`
- field `StringName HideThinkyDots`
- field `StringName OnHitboxEntered`
- field `StringName OnHitboxExited`
- field `StringName OnHoverChanged`
- field `StringName OnPeerInputStateChanged`
- field `StringName OnPeerInputStateRemoved`
- field `StringName RefreshHoverDisplay`
- field `StringName RefreshHoverTips`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerIntentHandler+PropertyName
- field `StringName _cardInPlayAwaitingPlayerChoice`
- field `StringName _cardIntent`
- field `StringName _cardThinkyDots`
- field `StringName _hitbox`
- field `StringName _hoverTips`
- field `StringName _isInPlayerChoice`
- field `StringName _potionIntent`
- field `StringName _potionThinkyDots`
- field `StringName _powerIntent`
- field `StringName _powerThinkyDots`
- field `StringName _relicIntent`
- field `StringName _relicThinkyDots`
- field `StringName _shouldShowHoverTip`
- field `StringName _targetingIndicator`
- field `StringName _tween`
- field `StringName CardIntent`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerIntentHandler+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState
- method `_ExitTree()` -> `Void`
- method `_Ready()` -> `Void`
- method `<TweenLocationIconAway>b__91_0()` -> `Boolean`
- method `AnimateCardObtained(CardModel card)` -> `Task`
- method `AnimateCardRemovedFromDeck(CardModel card)` -> `Task`
- method `AnimatePotionDiscarded(PotionModel potion)` -> `Task`
- method `AnimatePotionObtained(PotionModel potion)` -> `Task`
- method `AnimateRelicObtained(RelicModel relic)` -> `Task`
- method `AnimateRelicRemoved(RelicModel relic)` -> `Task`
- method `BlockChanged(Int32 oldBlock, Int32 blockGain)` -> `Void`
- method `Create(Player player)` -> `NMultiplayerPlayerState`
- method `FlashEndTurn()` -> `Void`
- method `FlashPlayerReady()` -> `Void`
- method `get_AssetPaths()` -> `IEnumerable`1`
- method `get_Hitbox()` -> `NButton`
- method `get_Player()` -> `Player`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `ObtainedAnimation(Control node)` -> `Task`
- method `OnCardAdded(CardModel _)` -> `Void`
- method `OnCardObtained(CardModel card)` -> `Void`
- method `OnCardRemoved(CardModel _)` -> `Void`
- method `OnCardRemovedFromDeck(CardModel card)` -> `Void`
- method `OnCombatEnded(CombatRoom _)` -> `Void`
- method `OnCombatSetUp(CombatState _)` -> `Void`
- method `OnCreatureChanged(Creature _)` -> `Void`
- method `OnCreatureHovered()` -> `Void`
- method `OnCreatureUnhovered()` -> `Void`
- method `OnCreatureValueChanged(Int32 _, Int32 __)` -> `Void`
- method `OnEnergyChanged(Int32 _, Int32 __)` -> `Void`
- method `OnFocus(NButton _)` -> `Void`
- method `OnPlayerEndTurnPing(UInt64 playerId)` -> `Void`
- method `OnPlayerScreenChanged(UInt64 playerId, NetScreenType _)` -> `Void`
- method `OnPlayerVoteChanged(Player player, Nullable`1 _, Nullable`1 __)` -> `Void`
- method `OnPlayerVotesCleared()` -> `Void`
- method `OnPotionDiscarded(PotionModel potion)` -> `Void`
- method `OnPotionProcured(PotionModel potion)` -> `Void`
- method `OnPowerAppliedOrRemoved(PowerModel _)` -> `Void`
- method `OnPowerDecreased(PowerModel _, Boolean __)` -> `Void`
- method `OnPowerIncreased(PowerModel _, Int32 __, Boolean ___)` -> `Void`
- method `OnRelease(NButton _)` -> `Void`
- method `OnRelicObtained(RelicModel relic)` -> `Void`
- method `OnRelicRemoved(RelicModel relic)` -> `Void`
- method `OnStarsChanged(Int32 _, Int32 __)` -> `Void`
- method `OnTurnStarted(CombatState _)` -> `Void`
- method `OnUnfocus(NButton _)` -> `Void`
- method `RefreshCombatValues()` -> `Void`
- method `RefreshConnectedState(UInt64 _)` -> `Void`
- method `RefreshConnectedState()` -> `Void`
- method `RefreshPlayerReadyIndicator(Player player, Boolean _)` -> `Void`
- method `RefreshPlayerReadyIndicator(Player player)` -> `Void`
- method `RefreshValues()` -> `Void`
- method `RemovedAnimation(Control node)` -> `Task`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `set_Hitbox(NButton value)` -> `Void`
- method `set_Player(Player value)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `SetNextTweenTime()` -> `Void`
- method `TweenLocationIconAway()` -> `Void`
- method `TweenLocationIconIn(Texture2D texture)` -> `Void`
- method `UpdateHealthBarWidth()` -> `Void`
- method `UpdateHighlightedState()` -> `Void`
- method `UpdateSelectionReticleWidth()` -> `Void`
- method `WaitUntilNextTweenTime()` -> `Task`
- field `Control _cardContainer`
- field `MegaLabel _cardCount`
- field `NTinyCard _cardImage`
- field `String _cardScenePath`
- field `TextureRect _characterIcon`
- field `Texture2D _currentLocationIcon`
- field `String _darkenedEnergyMatPath`
- field `UInt64 _delayBetweenTweensMsec`
- field `TextureRect _disconnectedIndicator`
- field `Control _energyContainer`
- field `MegaLabel _energyCount`
- field `TextureRect _energyImage`
- field `Boolean _focusedWhileTargeting`
- field `NHealthBar _healthBar`
- field `Boolean _isCreatureHovered`
- field `Boolean _isHighlighted`
- field `Boolean _isMouseOver`
- field `Control _locationContainer`
- field `TextureRect _locationIcon`
- field `Tween _locationIconTween`
- field `MegaLabel _nameplateLabel`
- field `NMultiplayerNetworkProblemIndicator _networkProblemIndicator`
- field `UInt64 _nextTweenTime`
- field `Single _refHpBarMaxHp`
- field `Single _refHpBarWidth`
- field `String _scenePath`
- field `NSelectionReticle _selectionReticle`
- field `Single _selectionReticlePadding`
- field `Control _starContainer`
- field `MegaLabel _starCount`
- field `HBoxContainer _topContainer`
- field `TextureRect _turnEndIndicator`
- field `NButton <Hitbox>k__BackingField`
- field `Player <Player>k__BackingField`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+<>c__DisplayClass92_0
- method `<TweenLocationIconIn>b__0()` -> `Texture2D`
- field `NMultiplayerPlayerState <>4__this`
- field `Texture2D texture`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+<AnimateCardObtained>d__72
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerPlayerState <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `NDeckHistoryEntry <cardNode>5__2`
- field `CardModel card`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+<AnimateCardRemovedFromDeck>d__74
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerPlayerState <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `NDeckHistoryEntry <cardNode>5__2`
- field `CardModel card`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+<AnimatePotionDiscarded>d__78
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerPlayerState <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `NPotion <node>5__2`
- field `PotionModel potion`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+<AnimatePotionObtained>d__76
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerPlayerState <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `NPotion <node>5__2`
- field `PotionModel potion`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+<AnimateRelicObtained>d__68
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerPlayerState <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `NRelic <relicImage>5__2`
- field `RelicModel relic`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+<AnimateRelicRemoved>d__70
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerPlayerState <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `NRelic <relicImage>5__2`
- field `RelicModel relic`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+<ObtainedAnimation>d__88
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerPlayerState <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `Object <>u__1`
- field `Control node`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+<RemovedAnimation>d__89
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerPlayerState <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `Object <>u__1`
- field `Control node`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+<WaitUntilNextTweenTime>d__87
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerPlayerState <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `Object <>u__1`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+MethodName
- field `StringName _ExitTree`
- field `StringName _Ready`
- field `StringName BlockChanged`
- field `StringName FlashEndTurn`
- field `StringName FlashPlayerReady`
- field `StringName OnCreatureHovered`
- field `StringName OnCreatureUnhovered`
- field `StringName OnCreatureValueChanged`
- field `StringName OnEnergyChanged`
- field `StringName OnFocus`
- field `StringName OnPlayerEndTurnPing`
- field `StringName OnPlayerScreenChanged`
- field `StringName OnPlayerVotesCleared`
- field `StringName OnRelease`
- field `StringName OnStarsChanged`
- field `StringName OnUnfocus`
- field `StringName RefreshCombatValues`
- field `StringName RefreshConnectedState`
- field `StringName RefreshValues`
- field `StringName SetNextTweenTime`
- field `StringName TweenLocationIconAway`
- field `StringName TweenLocationIconIn`
- field `StringName UpdateHealthBarWidth`
- field `StringName UpdateHighlightedState`
- field `StringName UpdateSelectionReticleWidth`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+PropertyName
- field `StringName _cardContainer`
- field `StringName _cardCount`
- field `StringName _cardImage`
- field `StringName _characterIcon`
- field `StringName _currentLocationIcon`
- field `StringName _disconnectedIndicator`
- field `StringName _energyContainer`
- field `StringName _energyCount`
- field `StringName _energyImage`
- field `StringName _focusedWhileTargeting`
- field `StringName _healthBar`
- field `StringName _isCreatureHovered`
- field `StringName _isHighlighted`
- field `StringName _isMouseOver`
- field `StringName _locationContainer`
- field `StringName _locationIcon`
- field `StringName _locationIconTween`
- field `StringName _nameplateLabel`
- field `StringName _networkProblemIndicator`
- field `StringName _nextTweenTime`
- field `StringName _selectionReticle`
- field `StringName _starContainer`
- field `StringName _starCount`
- field `StringName _topContainer`
- field `StringName _turnEndIndicator`
- field `StringName Hitbox`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerState+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerStateContainer
- method `_EnterTree()` -> `Void`
- method `_ExitTree()` -> `Void`
- method `_Input(InputEvent inputEvent)` -> `Void`
- method `_Ready()` -> `Void`
- method `AnimHide()` -> `Void`
- method `AnimShow()` -> `Void`
- method `FlashPlayerReady(Player player)` -> `Void`
- method `get_FirstPlayerState()` -> `NMultiplayerPlayerState`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `HideImmediately()` -> `Void`
- method `HighlightPlayer(Player player)` -> `Void`
- method `Initialize(RunState runState)` -> `Void`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `LockNavigation()` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `ShowImmediately()` -> `Void`
- method `UnhighlightPlayer(Player player)` -> `Void`
- method `UnlockNavigation()` -> `Void`
- method `UpdateNavigation()` -> `Void`
- method `UpdatePosition()` -> `Void`
- method `UpdatePositionAfterOneFrame()` -> `Void`
- method `UpdatePositionAfterOneFrameAsync()` -> `Task`
- field `List`1 _nodes`
- field `Vector2 _originalPosition`
- field `IRunState _runState`
- field `Tween _tween`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerStateContainer+<>c__DisplayClass17_0
- method `<HighlightPlayer>b__0(NMultiplayerPlayerState n)` -> `Boolean`
- field `Player player`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerStateContainer+<>c__DisplayClass18_0
- method `<UnhighlightPlayer>b__0(NMultiplayerPlayerState n)` -> `Boolean`
- field `Player player`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerStateContainer+<>c__DisplayClass19_0
- method `<FlashPlayerReady>b__0(NMultiplayerPlayerState n)` -> `Boolean`
- field `Player player`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerStateContainer+<UpdatePositionAfterOneFrameAsync>d__15
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerPlayerStateContainer <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `Object <>u__1`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerStateContainer+MethodName
- field `StringName _EnterTree`
- field `StringName _ExitTree`
- field `StringName _Input`
- field `StringName _Ready`
- field `StringName AnimHide`
- field `StringName AnimShow`
- field `StringName HideImmediately`
- field `StringName LockNavigation`
- field `StringName ShowImmediately`
- field `StringName UnlockNavigation`
- field `StringName UpdateNavigation`
- field `StringName UpdatePosition`
- field `StringName UpdatePositionAfterOneFrame`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerStateContainer+PropertyName
- field `StringName _originalPosition`
- field `StringName _tween`
- field `StringName FirstPlayerState`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerPlayerStateContainer+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerTimeoutOverlay
- method `_Ready()` -> `Void`
- method `get_IsShown()` -> `Boolean`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `Initialize(INetGameService netService, Boolean isGameLevel)` -> `Void`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `Relocalize()` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `set_IsShown(Boolean value)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `UpdateLoop()` -> `Task`
- field `Boolean _gameLevel`
- field `TextureRect _icon`
- field `Int32 _loadingNoResponseMsec`
- field `NetClientGameService _netService`
- field `Int32 _noResponseMsec`
- field `Boolean <IsShown>k__BackingField`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerTimeoutOverlay+<UpdateLoop>d__12
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerTimeoutOverlay <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerTimeoutOverlay+MethodName
- field `StringName _Ready`
- field `StringName Relocalize`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerTimeoutOverlay+PropertyName
- field `StringName _gameLevel`
- field `StringName _icon`
- field `StringName IsShown`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerTimeoutOverlay+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerWarningPopup
- method `_Ready()` -> `Void`
- method `Create()` -> `NMultiplayerWarningPopup`
- method `get_AssetPaths()` -> `IEnumerable`1`
- method `get_DefaultFocusedControl()` -> `Control`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `InvokeGodotClassStaticMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `OnBackButtonPressed(NButton _)` -> `Void`
- method `OnIgnoreButtonPressed(NButton _)` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- field `String _scenePath`
- field `NVerticalPopup _verticalPopup`
- field `String ftueId`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerWarningPopup+MethodName
- field `StringName _Ready`
- field `StringName Create`
- field `StringName OnBackButtonPressed`
- field `StringName OnIgnoreButtonPressed`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerWarningPopup+PropertyName
- field `StringName _verticalPopup`
- field `StringName DefaultFocusedControl`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerWarningPopup+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLoadLobbyPlayerContainer
- method `_Ready()` -> `Void`
- method `Cleanup()` -> `Void`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `Initialize(LoadRunLobby runLobby, Boolean displayLocalPlayer)` -> `Void`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `OnPlayerChanged(UInt64 playerId)` -> `Void`
- method `OnPlayerConnected(UInt64 playerId)` -> `Void`
- method `OnPlayerDisconnected(UInt64 playerId)` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- field `Control _container`
- field `LoadRunLobby _lobby`
- field `List`1 _nodes`
- field `MegaLabel _othersLabel`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLoadLobbyPlayerContainer+<>c__DisplayClass8_0
- method `<OnPlayerChanged>b__0(NRemoteLobbyPlayer p)` -> `Boolean`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLoadLobbyPlayerContainer+MethodName
- field `StringName _Ready`
- field `StringName Cleanup`
- field `StringName OnPlayerChanged`
- field `StringName OnPlayerConnected`
- field `StringName OnPlayerDisconnected`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLoadLobbyPlayerContainer+PropertyName
- field `StringName _container`
- field `StringName _othersLabel`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLoadLobbyPlayerContainer+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayer
- method `_Process(Double delta)` -> `Void`
- method `_Ready()` -> `Void`
- method `CancelShake()` -> `Void`
- method `Create(LobbyPlayer player, PlatformType platform, Boolean isSingleplayer)` -> `NRemoteLobbyPlayer`
- method `Create(LoadRunLobby runLobby, UInt64 playerId, PlatformType platform, Boolean isSingleplayer)` -> `NRemoteLobbyPlayer`
- method `get_AssetPaths()` -> `IEnumerable`1`
- method `get_PlayerId()` -> `UInt64`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `OnPlayerChanged(LobbyPlayer lobbyPlayer)` -> `Void`
- method `OnPlayerChanged(LoadRunLobby runLobby, UInt64 playerId)` -> `Void`
- method `RefreshVisuals()` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetCharacter(CharacterModel character)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- field `CharacterModel _character`
- field `TextureRect _characterIcon`
- field `MegaLabel _characterLabel`
- field `Control _disconnectedIndicator`
- field `Boolean _isConnected`
- field `Boolean _isReady`
- field `Boolean _isSingleplayer`
- field `MegaLabel _nameplateLabel`
- field `Nullable`1 _originalPosition`
- field `PlatformType _platform`
- field `UInt64 _playerId`
- field `Control _readyIndicator`
- field `String _scenePath`
- field `ScreenPunchInstance _shake`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayer+<>c__DisplayClass19_0
- method `<Create>b__0(SerializablePlayer p)` -> `Boolean`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayer+<>c__DisplayClass22_0
- method `<OnPlayerChanged>b__0(SerializablePlayer p)` -> `Boolean`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayer+MethodName
- field `StringName _Process`
- field `StringName _Ready`
- field `StringName CancelShake`
- field `StringName RefreshVisuals`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayer+PropertyName
- field `StringName _characterIcon`
- field `StringName _characterLabel`
- field `StringName _disconnectedIndicator`
- field `StringName _isConnected`
- field `StringName _isReady`
- field `StringName _isSingleplayer`
- field `StringName _nameplateLabel`
- field `StringName _platform`
- field `StringName _playerId`
- field `StringName _readyIndicator`
- field `StringName PlayerId`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayer+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayerContainer
- method `_Ready()` -> `Void`
- method `Cleanup()` -> `Void`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `Initialize(StartRunLobby lobby, Boolean displayLocalPlayer)` -> `Void`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `OnPlayerChanged(LobbyPlayer player)` -> `Void`
- method `OnPlayerConnected(LobbyPlayer player)` -> `Void`
- method `OnPlayerDisconnected(LobbyPlayer player)` -> `Void`
- method `RefreshSoloLabelVisibility()` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- field `Container _container`
- field `Boolean _displayLocalPlayer`
- field `NInvitePlayersButton _inviteButton`
- field `StartRunLobby _lobby`
- field `List`1 _nodes`
- field `MegaLabel _soloLabel`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayerContainer+<>c__DisplayClass10_0
- method `<OnPlayerChanged>b__0(NRemoteLobbyPlayer p)` -> `Boolean`
- field `LobbyPlayer player`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayerContainer+<>c__DisplayClass9_0
- method `<OnPlayerDisconnected>b__0(NRemoteLobbyPlayer p)` -> `Boolean`
- field `LobbyPlayer player`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayerContainer+MethodName
- field `StringName _Ready`
- field `StringName Cleanup`
- field `StringName RefreshSoloLabelVisibility`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayerContainer+PropertyName
- field `StringName _container`
- field `StringName _displayLocalPlayer`
- field `StringName _inviteButton`
- field `StringName _soloLabel`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayerContainer+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteMouseCursor
- method `_Process(Double delta)` -> `Void`
- method `_Ready()` -> `Void`
- method `Create(UInt64 playerId)` -> `NRemoteMouseCursor`
- method `get_PlayerId()` -> `UInt64`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `GetHotspot(DrawingMode drawingMode)` -> `Vector2`
- method `GetTexture(Boolean isDown, DrawingMode drawingMode)` -> `Texture2D`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `InvokeGodotClassStaticMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `RefreshSize()` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `set_PlayerId(UInt64 value)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `SetNextPosition(Vector2 position)` -> `Void`
- method `UpdateImage(Boolean isDown, DrawingMode drawingMode)` -> `Void`
- field `Image _defaultCursorImage`
- field `ImageTexture _defaultCursorTexture`
- field `Image _defaultDrawingImage`
- field `ImageTexture _defaultDrawingTexture`
- field `Image _defaultErasingImage`
- field `ImageTexture _defaultErasingTexture`
- field `Vector2 _defaultHotspot`
- field `Vector2 _drawingHotspot`
- field `DrawingMode _drawingMode`
- field `Vector2 _erasingHotspot`
- field `UInt64 _lastPositionUpdateMsec`
- field `Nullable`1 _nextPosition`
- field `Nullable`1 _previousPosition`
- field `String _scenePath`
- field `TextureRect _textureRect`
- field `Image _tiltedCursorImage`
- field `ImageTexture _tiltedCursorTexture`
- field `Image _tiltedDrawingImage`
- field `ImageTexture _tiltedDrawingTexture`
- field `Image _tiltedErasingImage`
- field `ImageTexture _tiltedErasingTexture`
- field `UInt64 <PlayerId>k__BackingField`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteMouseCursor+MethodName
- field `StringName _Process`
- field `StringName _Ready`
- field `StringName Create`
- field `StringName GetHotspot`
- field `StringName GetTexture`
- field `StringName RefreshSize`
- field `StringName SetNextPosition`
- field `StringName UpdateImage`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteMouseCursor+PropertyName
- field `StringName _defaultCursorImage`
- field `StringName _defaultCursorTexture`
- field `StringName _defaultDrawingImage`
- field `StringName _defaultDrawingTexture`
- field `StringName _defaultErasingImage`
- field `StringName _defaultErasingTexture`
- field `StringName _defaultHotspot`
- field `StringName _drawingHotspot`
- field `StringName _drawingMode`
- field `StringName _erasingHotspot`
- field `StringName _lastPositionUpdateMsec`
- field `StringName _textureRect`
- field `StringName _tiltedCursorImage`
- field `StringName _tiltedCursorTexture`
- field `StringName _tiltedDrawingImage`
- field `StringName _tiltedDrawingTexture`
- field `StringName _tiltedErasingImage`
- field `StringName _tiltedErasingTexture`
- field `StringName PlayerId`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteMouseCursor+SignalName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteMouseCursorContainer
- method `_ExitTree()` -> `Void`
- method `_Input(InputEvent inputEvent)` -> `Void`
- method `_Ready()` -> `Void`
- method `AddCursor(UInt64 playerId)` -> `Void`
- method `ApplyDebugUiVisibility()` -> `Void`
- method `Deinitialize()` -> `Void`
- method `DrawingCursorStateChanged(UInt64 playerId)` -> `Void`
- method `ForceUpdateAllCursors()` -> `Void`
- method `GetCursor(UInt64 playerId)` -> `NRemoteMouseCursor`
- method `GetCursorPosition(UInt64 playerId)` -> `Vector2`
- method `GetDrawingMode(UInt64 playerId)` -> `DrawingMode`
- method `GetGodotMethodList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `Initialize(PeerInputSynchronizer synchronizer, IEnumerable`1 connectedPlayerIds)` -> `Void`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `InvokeGodotClassStaticMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `NetServiceDisconnected(NetErrorInfo _)` -> `Void`
- method `OnGuiFocusChanged(Control focused)` -> `Void`
- method `OnInputStateAdded(UInt64 playerId)` -> `Void`
- method `OnInputStateChanged(UInt64 playerId)` -> `Void`
- method `OnInputStateRemoved(UInt64 playerId)` -> `Void`
- method `RemoveCursor(UInt64 playerId)` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `UpdateCursorVisibility()` -> `Void`
- field `List`1 _cursors`
- field `Boolean _isDebugUiVisible`
- field `PeerInputSynchronizer _synchronizer`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteMouseCursorContainer+<>c__DisplayClass12_0
- method `<AddCursor>b__0(NRemoteMouseCursor c)` -> `Boolean`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteMouseCursorContainer+<>c__DisplayClass16_0
- method `<GetCursor>b__0(NRemoteMouseCursor c)` -> `Boolean`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteMouseCursorContainer+MethodName
- field `StringName _ExitTree`
- field `StringName _Input`
- field `StringName _Ready`
- field `StringName AddCursor`
- field `StringName ApplyDebugUiVisibility`
- field `StringName Deinitialize`
- field `StringName DrawingCursorStateChanged`
- field `StringName ForceUpdateAllCursors`
- field `StringName GetCursor`
- field `StringName GetCursorPosition`
- field `StringName GetDrawingMode`
- field `StringName OnGuiFocusChanged`
- field `StringName OnInputStateAdded`
- field `StringName OnInputStateChanged`
- field `StringName OnInputStateRemoved`
- field `StringName RemoveCursor`
- field `StringName UpdateCursorVisibility`

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteMouseCursorContainer+PropertyName

## MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteMouseCursorContainer+SignalName

## MegaCrit.Sts2.Core.Nodes.NGame+<StartNewMultiplayerRun>d__138
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NGame <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter <>u__1`
- field `RunState <runState>5__2`
- field `IReadOnlyList`1 acts`
- field `Int32 ascensionLevel`
- field `Nullable`1 dailyTime`
- field `StartRunLobby lobby`
- field `IReadOnlyList`1 modifiers`
- field `String seed`
- field `Boolean shouldSave`

## MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NCharacterSelectScreen+<StartNewMultiplayerRun>d__55
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NCharacterSelectScreen <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `TaskAwaiter`1 <>u__2`
- field `TaskAwaiter`1 <>u__3`
- field `NetLoadingHandle <loadHandle>5__2`
- field `RunState <runState>5__3`
- field `List`1 acts`
- field `String seed`

## MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NMultiplayerLoadGameScreen
- method `_Process(Double delta)` -> `Void`
- method `_Ready()` -> `Void`
- method `<AfterMultiplayerStarted>b__42_0(SerializablePlayer p)` -> `Boolean`
- method `<StartRun>b__36_0(SerializablePlayer p)` -> `Boolean`
- method `AfterMultiplayerStarted()` -> `Void`
- method `BeginRun()` -> `Void`
- method `CleanUpLobby(Boolean disconnectSession)` -> `Void`
- method `Create()` -> `NMultiplayerLoadGameScreen`
- method `get_AssetPaths()` -> `IEnumerable`1`
- method `get_InitialFocusedControl()` -> `Control`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InitializeAsClient(INetGameService gameService, ClientLoadJoinResponseMessage message)` -> `Void`
- method `InitializeAsHost(INetGameService gameService, SerializableRun run)` -> `Void`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `InvokeGodotClassStaticMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `LocalPlayerDisconnected(NetErrorInfo info)` -> `Void`
- method `OnEmbarkPressed(NButton _)` -> `Void`
- method `OnSubmenuClosed()` -> `Void`
- method `OnSubmenuHidden()` -> `Void`
- method `OnSubmenuOpened()` -> `Void`
- method `OnSubmenuShown()` -> `Void`
- method `OnUnreadyPressed(NButton _)` -> `Void`
- method `PlayerConnected(UInt64 playerId)` -> `Void`
- method `PlayerReadyChanged(UInt64 playerId)` -> `Void`
- method `RemotePlayerDisconnected(UInt64 playerId)` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `ShouldAllowRunToBegin()` -> `Task`1`
- method `StartRun()` -> `Task`
- method `UpdateRichPresence()` -> `Void`
- field `MegaRichTextLabel _actLabel`
- field `NAscensionPanel _ascensionPanel`
- field `NBackButton _backButton`
- field `Control _bgContainer`
- field `NConfirmButton _confirmButton`
- field `MegaRichTextLabel _floorLabel`
- field `MegaLabel _gold`
- field `MegaLabel _hp`
- field `Control _infoPanel`
- field `Vector2 _infoPanelPosFinalVal`
- field `Tween _infoPanelTween`
- field `MegaLabel _name`
- field `NRemoteLoadLobbyPlayerContainer _remotePlayerContainer`
- field `LoadRunLobby _runLobby`
- field `String _sceneCharSelectButtonPath`
- field `String _scenePath`
- field `NCharacterSelectButton _selectedButton`
- field `NBackButton _unreadyButton`

## MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NMultiplayerLoadGameScreen+<ShouldAllowRunToBegin>d__35
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerLoadGameScreen <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`

## MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NMultiplayerLoadGameScreen+<StartRun>d__36
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerLoadGameScreen <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`

## MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NMultiplayerLoadGameScreen+MethodName
- field `StringName _Process`
- field `StringName _Ready`
- field `StringName AfterMultiplayerStarted`
- field `StringName BeginRun`
- field `StringName CleanUpLobby`
- field `StringName Create`
- field `StringName OnEmbarkPressed`
- field `StringName OnSubmenuClosed`
- field `StringName OnSubmenuHidden`
- field `StringName OnSubmenuOpened`
- field `StringName OnSubmenuShown`
- field `StringName OnUnreadyPressed`
- field `StringName PlayerConnected`
- field `StringName PlayerReadyChanged`
- field `StringName RemotePlayerDisconnected`
- field `StringName UpdateRichPresence`

## MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NMultiplayerLoadGameScreen+PropertyName
- field `StringName _actLabel`
- field `StringName _ascensionPanel`
- field `StringName _backButton`
- field `StringName _bgContainer`
- field `StringName _confirmButton`
- field `StringName _floorLabel`
- field `StringName _gold`
- field `StringName _hp`
- field `StringName _infoPanel`
- field `StringName _infoPanelPosFinalVal`
- field `StringName _infoPanelTween`
- field `StringName _name`
- field `StringName _remotePlayerContainer`
- field `StringName _selectedButton`
- field `StringName _unreadyButton`
- field `StringName InitialFocusedControl`

## MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NMultiplayerLoadGameScreen+SignalName

## MegaCrit.Sts2.Core.Nodes.Screens.CustomRun.NCustomRunScreen+<StartNewMultiplayerRun>d__41
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NCustomRunScreen <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `TaskAwaiter`1 <>u__2`
- field `List`1 acts`
- field `IReadOnlyList`1 modifiers`
- field `String seed`

## MegaCrit.Sts2.Core.Nodes.Screens.DailyRun.NDailyRunScreen+<SetupLobbyForHostOrSingleplayer>d__34
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NDailyRunScreen <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter`1 <>u__1`

## MegaCrit.Sts2.Core.Nodes.Screens.DailyRun.NDailyRunScreen+<StartNewMultiplayerRun>d__55
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NDailyRunScreen <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `TaskAwaiter`1 <>u__2`
- field `List`1 acts`
- field `IReadOnlyList`1 modifiers`
- field `String seed`

## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerHostSubmenu
- method `_Ready()` -> `Void`
- method `Create()` -> `NMultiplayerHostSubmenu`
- method `get_InitialFocusedControl()` -> `Control`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `InvokeGodotClassStaticMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `OnCustomPressed(NButton _)` -> `Void`
- method `OnDailyPressed(NButton _)` -> `Void`
- method `OnStandardPressed(NButton _)` -> `Void`
- method `OnSubmenuOpened()` -> `Void`
- method `RefreshButtons()` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `StartHost(GameMode gameMode)` -> `Void`
- method `StartHostAsync(GameMode gameMode, Control loadingOverlay, NSubmenuStack stack)` -> `Task`
- field `NSubmenuButton _customButton`
- field `NSubmenuButton _dailyButton`
- field `String _keyCustom`
- field `String _keyDaily`
- field `String _keyStandard`
- field `Control _loadingOverlay`
- field `String _scenePath`
- field `NSubmenuButton _standardButton`

## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerHostSubmenu+<StartHostAsync>d__18
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `NetHostGameService <netService>5__2`
- field `GameMode gameMode`
- field `Control loadingOverlay`
- field `NSubmenuStack stack`

## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerHostSubmenu+MethodName
- field `StringName _Ready`
- field `StringName Create`
- field `StringName OnCustomPressed`
- field `StringName OnDailyPressed`
- field `StringName OnStandardPressed`
- field `StringName OnSubmenuOpened`
- field `StringName RefreshButtons`
- field `StringName StartHost`

## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerHostSubmenu+PropertyName
- field `StringName _customButton`
- field `StringName _dailyButton`
- field `StringName _loadingOverlay`
- field `StringName _standardButton`
- field `StringName InitialFocusedControl`

## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerHostSubmenu+SignalName

## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerSubmenu
- method `_Ready()` -> `Void`
- method `AbandonRun(NButton _)` -> `Void`
- method `Create()` -> `NMultiplayerSubmenu`
- method `FastHost(GameMode gameMode)` -> `Void`
- method `get_InitialFocusedControl()` -> `Control`
- method `GetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `GetGodotMethodList()` -> `List`1`
- method `GetGodotPropertyList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `InvokeGodotClassStaticMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `OnHostPressed(NButton _)` -> `Void`
- method `OnJoinFriendsPressed()` -> `NJoinFriendScreen`
- method `OnSubmenuShown()` -> `Void`
- method `OpenJoinFriendsScreen(NButton _)` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SetGodotClassPropertyValue(godot_string_name& name, godot_variant& value)` -> `Boolean`
- method `StartHost(SerializableRun run)` -> `Void`
- method `StartHostAsync(SerializableRun run)` -> `Task`
- method `StartLoad(NButton _)` -> `Void`
- method `TryAbandonMultiplayerRun()` -> `Task`
- method `UpdateButtons()` -> `Void`
- field `NSubmenuButton _abandonButton`
- field `NSubmenuButton _hostButton`
- field `NSubmenuButton _joinButton`
- field `String _keyAbandon`
- field `String _keyHost`
- field `String _keyJoin`
- field `String _keyLoad`
- field `NSubmenuButton _loadButton`
- field `Control _loadingOverlay`
- field `String _scenePath`

## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerSubmenu+<StartHostAsync>d__21
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerSubmenu <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `NetHostGameService <netService>5__2`
- field `SerializableRun run`

## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerSubmenu+<TryAbandonMultiplayerRun>d__16
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NMultiplayerSubmenu <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter`1 <>u__1`

## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerSubmenu+MethodName
- field `StringName _Ready`
- field `StringName AbandonRun`
- field `StringName Create`
- field `StringName FastHost`
- field `StringName OnHostPressed`
- field `StringName OnJoinFriendsPressed`
- field `StringName OnSubmenuShown`
- field `StringName OpenJoinFriendsScreen`
- field `StringName StartLoad`
- field `StringName UpdateButtons`

## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerSubmenu+PropertyName
- field `StringName _abandonButton`
- field `StringName _hostButton`
- field `StringName _joinButton`
- field `StringName _loadButton`
- field `StringName _loadingOverlay`
- field `StringName InitialFocusedControl`

## MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerSubmenu+SignalName

## MegaCrit.Sts2.Core.Nodes.Screens.Settings.NUploadGameplayDataHoverTip
- method `_Ready()` -> `Void`
- method `GetGodotMethodList()` -> `List`1`
- method `HasGodotClassMethod(godot_string_name& method)` -> `Boolean`
- method `InvokeGodotClassMethod(godot_string_name& method, NativeVariantPtrArgs args, godot_variant& ret)` -> `Boolean`
- method `OnHovered()` -> `Void`
- method `OnUnhovered()` -> `Void`
- method `RestoreGodotObjectData(GodotSerializationInfo info)` -> `Void`
- method `SaveGodotObjectData(GodotSerializationInfo info)` -> `Void`
- field `IHoverTip _hoverTip`

## MegaCrit.Sts2.Core.Nodes.Screens.Settings.NUploadGameplayDataHoverTip+MethodName
- field `StringName _Ready`
- field `StringName OnHovered`
- field `StringName OnUnhovered`

## MegaCrit.Sts2.Core.Nodes.Screens.Settings.NUploadGameplayDataHoverTip+PropertyName

## MegaCrit.Sts2.Core.Nodes.Screens.Settings.NUploadGameplayDataHoverTip+SignalName

## MegaCrit.Sts2.Core.Nodes.Screens.Timeline.NEraColumn+<SaveBeforeAnimationPosition>d__22
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `NEraColumn <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `Object <>u__1`

## MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName
- method `<Clone>$()` -> `NullMultiplayerName`
- method `Equals(Object obj)` -> `Boolean`
- method `Equals(NullMultiplayerName other)` -> `Boolean`
- method `get_EqualityContract()` -> `Type`
- method `GetHashCode()` -> `Int32`
- method `op_Equality(NullMultiplayerName left, NullMultiplayerName right)` -> `Boolean`
- method `op_Inequality(NullMultiplayerName left, NullMultiplayerName right)` -> `Boolean`
- method `PrintMembers(StringBuilder builder)` -> `Boolean`
- method `ToString()` -> `String`
- field `String name`
- field `UInt64 netId`

## MegaCrit.Sts2.Core.Platform.Steam.SteamRemoteSaveStore
- method `BeginSaveBatch()` -> `Void`
- method `CanonicalizePath(String path)` -> `String`
- method `CreateDirectory(String directoryPath)` -> `Void`
- method `DeleteDirectory(String directoryPath)` -> `Void`
- method `DeleteFile(String path)` -> `Void`
- method `DeleteTemporaryFiles(String directoryPath)` -> `Void`
- method `DirectoryExists(String path)` -> `Boolean`
- method `EndSaveBatch()` -> `Void`
- method `FileExists(String path)` -> `Boolean`
- method `ForgetFile(String path)` -> `Void`
- method `GetDirectoriesInDirectory(String directoryPath)` -> `String[]`
- method `GetFilesInDirectory(String directoryPath)` -> `String[]`
- method `GetFileSize(String path)` -> `Int32`
- method `GetFullPath(String filename)` -> `String`
- method `GetLastModifiedTime(String path)` -> `DateTimeOffset`
- method `HasCloudFiles()` -> `Boolean`
- method `IsFilePersisted(String path)` -> `Boolean`
- method `ReadFile(String path)` -> `String`
- method `ReadFileAsync(String path)` -> `Task`1`
- method `RenameFile(String sourcePath, String destinationPath)` -> `Void`
- method `SetLastModifiedTime(String path, DateTimeOffset time)` -> `Void`
- method `WriteFile(String path, String content)` -> `Void`
- method `WriteFile(String path, Byte[] bytes)` -> `Void`
- method `WriteFileAsync(String path, String content)` -> `Task`
- method `WriteFileAsync(String path, Byte[] bytes)` -> `Task`

## MegaCrit.Sts2.Core.Platform.Steam.SteamRemoteSaveStore+<ReadFileAsync>d__1
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `SteamRemoteSaveStore <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `Int32 <byteCount>5__2`
- field `SteamCallResult`1 <callResult>5__3`
- field `String path`

## MegaCrit.Sts2.Core.Platform.Steam.SteamRemoteSaveStore+<WriteFileAsync>d__5
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `SteamRemoteSaveStore <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `SteamCallResult`1 <callResult>5__2`
- field `Byte[] bytes`
- field `String path`

## MegaCrit.Sts2.Core.Platform.Steam.SteamRemoteSaveStoreException
- method `get_Result()` -> `EResult`
- method `set_Result(EResult value)` -> `Void`
- field `EResult <Result>k__BackingField`

## MegaCrit.Sts2.Core.Saves.AccountScopeUserDataMigrator
- method `ArchiveLegacyData()` -> `Void`
- method `HasLegacyData()` -> `Boolean`
- method `MigrateToUserScopedDirectories()` -> `Void`
- field `Boolean _migrationPerformed`

## MegaCrit.Sts2.Core.Saves.AncientCharacterStats
- method `get_Character()` -> `ModelId`
- method `get_Losses()` -> `Int32`
- method `get_Visits()` -> `Int32`
- method `get_Wins()` -> `Int32`
- method `set_Character(ModelId value)` -> `Void`
- method `set_Losses(Int32 value)` -> `Void`
- method `set_Wins(Int32 value)` -> `Void`
- field `ModelId <Character>k__BackingField`
- field `Int32 <Losses>k__BackingField`
- field `Int32 <Wins>k__BackingField`

## MegaCrit.Sts2.Core.Saves.AncientStats
- method `get_CharStats()` -> `List`1`
- method `get_Id()` -> `ModelId`
- method `get_TotalLosses()` -> `Int32`
- method `get_TotalVisits()` -> `Int32`
- method `get_TotalWins()` -> `Int32`
- method `GetStats(ModelId characterId)` -> `AncientCharacterStats`
- method `GetVisitsAs(ModelId characterId)` -> `Int32`
- method `IncrementLoss(ModelId characterId)` -> `Void`
- method `IncrementWin(ModelId characterId)` -> `Void`
- method `set_CharStats(List`1 value)` -> `Void`
- method `set_Id(ModelId value)` -> `Void`
- field `List`1 <CharStats>k__BackingField`
- field `ModelId <Id>k__BackingField`

## MegaCrit.Sts2.Core.Saves.AncientStats+<>c
- method `<get_TotalLosses>b__13_0(AncientCharacterStats fight)` -> `Int32`
- method `<get_TotalVisits>b__9_0(AncientCharacterStats c)` -> `Int32`
- method `<get_TotalWins>b__11_0(AncientCharacterStats fight)` -> `Int32`
- field `<>c <>9`
- field `Func`2 <>9__11_0`
- field `Func`2 <>9__13_0`
- field `Func`2 <>9__9_0`

## MegaCrit.Sts2.Core.Saves.AncientStats+<>c__DisplayClass17_0
- method `<GetStats>b__0(AncientCharacterStats c)` -> `Boolean`
- field `ModelId characterId`

## MegaCrit.Sts2.Core.Saves.BadgeStats
- method `get_Count()` -> `Int32`
- method `get_Id()` -> `String`
- method `get_Rarity()` -> `BadgeRarity`
- method `set_Count(Int32 value)` -> `Void`
- method `set_Id(String value)` -> `Void`
- method `set_Rarity(BadgeRarity value)` -> `Void`
- field `Int32 <Count>k__BackingField`
- field `String <Id>k__BackingField`
- field `BadgeRarity <Rarity>k__BackingField`

## MegaCrit.Sts2.Core.Saves.CardStats
- method `get_Id()` -> `ModelId`
- method `get_TimesLost()` -> `Int64`
- method `get_TimesPicked()` -> `Int64`
- method `get_TimesSkipped()` -> `Int64`
- method `get_TimesWon()` -> `Int64`
- method `set_Id(ModelId value)` -> `Void`
- method `set_TimesLost(Int64 value)` -> `Void`
- method `set_TimesPicked(Int64 value)` -> `Void`
- method `set_TimesSkipped(Int64 value)` -> `Void`
- method `set_TimesWon(Int64 value)` -> `Void`
- field `ModelId <Id>k__BackingField`
- field `Int64 <TimesLost>k__BackingField`
- field `Int64 <TimesPicked>k__BackingField`
- field `Int64 <TimesSkipped>k__BackingField`
- field `Int64 <TimesWon>k__BackingField`

## MegaCrit.Sts2.Core.Saves.CharacterStats
- method `get_Badges()` -> `List`1`
- method `get_BestWinStreak()` -> `Int64`
- method `get_CurrentWinStreak()` -> `Int64`
- method `get_FastestWinTime()` -> `Int64`
- method `get_Id()` -> `ModelId`
- method `get_MaxAscension()` -> `Int32`
- method `get_Playtime()` -> `Int64`
- method `get_PreferredAscension()` -> `Int32`
- method `get_TotalLosses()` -> `Int32`
- method `get_TotalWins()` -> `Int32`
- method `set_Badges(List`1 value)` -> `Void`
- method `set_BestWinStreak(Int64 value)` -> `Void`
- method `set_CurrentWinStreak(Int64 value)` -> `Void`
- method `set_FastestWinTime(Int64 value)` -> `Void`
- method `set_Id(ModelId value)` -> `Void`
- method `set_MaxAscension(Int32 value)` -> `Void`
- method `set_Playtime(Int64 value)` -> `Void`
- method `set_PreferredAscension(Int32 value)` -> `Void`
- method `set_TotalLosses(Int32 value)` -> `Void`
- method `set_TotalWins(Int32 value)` -> `Void`
- field `List`1 <Badges>k__BackingField`
- field `Int64 <BestWinStreak>k__BackingField`
- field `Int64 <CurrentWinStreak>k__BackingField`
- field `Int64 <FastestWinTime>k__BackingField`
- field `ModelId <Id>k__BackingField`
- field `Int32 <MaxAscension>k__BackingField`
- field `Int64 <Playtime>k__BackingField`
- field `Int32 <PreferredAscension>k__BackingField`
- field `Int32 <TotalLosses>k__BackingField`
- field `Int32 <TotalWins>k__BackingField`

## MegaCrit.Sts2.Core.Saves.CloudSaveStore
- method `<ForgetFilesInDirectoryBeforeWritingIfNecessaryInternal>b__37_0(String p1, String p2)` -> `Int32`
- method `BeginSaveBatch()` -> `Void`
- method `CreateDirectory(String directoryPath)` -> `Void`
- method `DeleteDirectory(String directoryPath)` -> `Void`
- method `DeleteFile(String path)` -> `Void`
- method `DeleteTemporaryFiles(String directoryPath)` -> `Void`
- method `DirectoryExists(String path)` -> `Boolean`
- method `EndSaveBatch()` -> `Void`
- method `FileExists(String path)` -> `Boolean`
- method `ForgetFile(String path)` -> `Void`
- method `ForgetFilesInDirectoryBeforeWritingIfNecessary(String directoryPath, Int32 bytesToBeWritten, Int32 byteLimit, Int32 fileLimit)` -> `Void`
- method `ForgetFilesInDirectoryBeforeWritingIfNecessaryInternal(String directoryPath, Int32 bytesToBeWritten, Int32 byteLimit, Int32 fileLimit)` -> `Void`
- method `get_CloudStore()` -> `ICloudSaveStore`
- method `get_LocalStore()` -> `ISaveStore`
- method `GetDirectoriesInDirectory(String directoryPath)` -> `String[]`
- method `GetFilesInDirectory(String directoryPath)` -> `String[]`
- method `GetFileSize(String path)` -> `Int32`
- method `GetFullPath(String filename)` -> `String`
- method `GetLastModifiedTime(String path)` -> `DateTimeOffset`
- method `HasCloudFiles()` -> `Boolean`
- method `IsFilePersisted(String path)` -> `Boolean`
- method `OverwriteCloudWithLocal(String path, Boolean forgetImmediately)` -> `Task`
- method `OverwriteCloudWithLocalDirectory(String directoryPath, Nullable`1 byteLimit, Nullable`1 fileLimit)` -> `IEnumerable`1`
- method `ReadFile(String path)` -> `String`
- method `ReadFileAsync(String path)` -> `Task`1`
- method `RenameFile(String sourcePath, String destinationPath)` -> `Void`
- method `SetLastModifiedTime(String path, DateTimeOffset time)` -> `Void`
- method `SyncCloudToLocal(String path)` -> `Task`
- method `SyncCloudToLocalDirectory(String directoryPath)` -> `IEnumerable`1`
- method `SyncCloudToLocalInternal(String path)` -> `Task`
- method `SyncLocalTimestamp(String path)` -> `Void`
- method `WriteFile(String path, String content)` -> `Void`
- method `WriteFile(String path, Byte[] bytes)` -> `Void`
- method `WriteFileAsync(String path, String content)` -> `Task`
- method `WriteFileAsync(String path, Byte[] bytes)` -> `Task`
- field `ICloudSaveStore <CloudStore>k__BackingField`
- field `ISaveStore <LocalStore>k__BackingField`

## MegaCrit.Sts2.Core.Saves.CloudSaveStore+<>c__DisplayClass35_0
- method `<OverwriteCloudWithLocalDirectory>b__0(String p1, String p2)` -> `Int32`
- field `CloudSaveStore <>4__this`
- field `String directoryPath`

## MegaCrit.Sts2.Core.Saves.CloudSaveStore+<OverwriteCloudWithLocal>d__34
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `CloudSaveStore <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `TaskAwaiter <>u__2`
- field `Boolean forgetImmediately`
- field `String path`

## MegaCrit.Sts2.Core.Saves.CloudSaveStore+<OverwriteCloudWithLocalDirectory>d__35
- method `<>m__Finally1()` -> `Void`
- method `MoveNext()` -> `Boolean`
- method `System.Collections.Generic.IEnumerable<System.Threading.Tasks.Task>.GetEnumerator()` -> `IEnumerator`1`
- method `System.Collections.Generic.IEnumerator<System.Threading.Tasks.Task>.get_Current()` -> `Task`
- method `System.Collections.IEnumerable.GetEnumerator()` -> `IEnumerator`
- method `System.Collections.IEnumerator.get_Current()` -> `Object`
- method `System.Collections.IEnumerator.Reset()` -> `Void`
- method `System.IDisposable.Dispose()` -> `Void`
- field `Int32 <>1__state`
- field `Task <>2__current`
- field `Nullable`1 <>3__byteLimit`
- field `String <>3__directoryPath`
- field `Nullable`1 <>3__fileLimit`
- field `CloudSaveStore <>4__this`
- field `String[] <>7__wrap2`
- field `Int32 <>7__wrap3`
- field `Enumerator <>7__wrap5`
- field `<>c__DisplayClass35_0 <>8__1`
- field `Int32 <>l__initialThreadId`
- field `Int32 <bytesToWrite>5__7`
- field `HashSet`1 <filePathsRead>5__2`
- field `Int32 <totalFilesWritten>5__5`
- field `Nullable`1 byteLimit`
- field `String directoryPath`
- field `Nullable`1 fileLimit`

## MegaCrit.Sts2.Core.Saves.CloudSaveStore+<SyncCloudToLocal>d__31
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `CloudSaveStore <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `String path`

## MegaCrit.Sts2.Core.Saves.CloudSaveStore+<SyncCloudToLocalDirectory>d__33
- method `MoveNext()` -> `Boolean`
- method `System.Collections.Generic.IEnumerable<System.Threading.Tasks.Task>.GetEnumerator()` -> `IEnumerator`1`
- method `System.Collections.Generic.IEnumerator<System.Threading.Tasks.Task>.get_Current()` -> `Task`
- method `System.Collections.IEnumerable.GetEnumerator()` -> `IEnumerator`
- method `System.Collections.IEnumerator.get_Current()` -> `Object`
- method `System.Collections.IEnumerator.Reset()` -> `Void`
- method `System.IDisposable.Dispose()` -> `Void`
- field `Int32 <>1__state`
- field `Task <>2__current`
- field `String <>3__directoryPath`
- field `CloudSaveStore <>4__this`
- field `String[] <>7__wrap2`
- field `Int32 <>7__wrap3`
- field `Int32 <>l__initialThreadId`
- field `HashSet`1 <filePathsRead>5__2`
- field `String directoryPath`

## MegaCrit.Sts2.Core.Saves.CloudSaveStore+<SyncCloudToLocalInternal>d__32
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `CloudSaveStore <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter`1 <>u__1`
- field `TaskAwaiter <>u__2`
- field `String path`

## MegaCrit.Sts2.Core.Saves.CloudSaveStore+<WriteFileAsync>d__13
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `CloudSaveStore <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `String content`
- field `String path`

## MegaCrit.Sts2.Core.Saves.CloudSaveStore+<WriteFileAsync>d__14
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `CloudSaveStore <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `Byte[] bytes`
- field `String path`

## MegaCrit.Sts2.Core.Saves.CorruptFileHandler
- method `GenerateCorruptFilePath(String originalPath, ReadSaveStatus status)` -> `String`
- method `GetReasonChar(ReadSaveStatus status)` -> `String`

## MegaCrit.Sts2.Core.Saves.EncounterStats
- method `get_FightStats()` -> `List`1`
- method `get_Id()` -> `ModelId`
- method `get_TotalLosses()` -> `Int32`
- method `get_TotalWins()` -> `Int32`
- method `IncrementLoss(ModelId characterId)` -> `Void`
- method `IncrementWin(ModelId characterId)` -> `Void`
- method `set_FightStats(List`1 value)` -> `Void`
- method `set_Id(ModelId value)` -> `Void`
- field `List`1 <FightStats>k__BackingField`
- field `ModelId <Id>k__BackingField`

## MegaCrit.Sts2.Core.Saves.EncounterStats+<>c
- method `<get_TotalLosses>b__11_0(FightStats fight)` -> `Int32`
- method `<get_TotalWins>b__9_0(FightStats fight)` -> `Int32`
- field `<>c <>9`
- field `Func`2 <>9__11_0`
- field `Func`2 <>9__9_0`

## MegaCrit.Sts2.Core.Saves.EncounterStats+<>c__DisplayClass12_0
- method `<IncrementWin>b__0(FightStats fight)` -> `Boolean`
- field `ModelId characterId`

## MegaCrit.Sts2.Core.Saves.EncounterStats+<>c__DisplayClass13_0
- method `<IncrementLoss>b__0(FightStats fight)` -> `Boolean`
- field `ModelId characterId`

## MegaCrit.Sts2.Core.Saves.EnemyStats
- method `get_FightStats()` -> `List`1`
- method `get_Id()` -> `ModelId`
- method `get_TotalLosses()` -> `Int32`
- method `get_TotalWins()` -> `Int32`
- method `IncrementLoss(ModelId characterId)` -> `Void`
- method `IncrementWin(ModelId characterId)` -> `Void`
- method `set_FightStats(List`1 value)` -> `Void`
- method `set_Id(ModelId value)` -> `Void`
- field `List`1 <FightStats>k__BackingField`
- field `ModelId <Id>k__BackingField`

## MegaCrit.Sts2.Core.Saves.EnemyStats+<>c
- method `<get_TotalLosses>b__11_0(FightStats f)` -> `Int32`
- method `<get_TotalWins>b__9_0(FightStats f)` -> `Int32`
- field `<>c <>9`
- field `Func`2 <>9__11_0`
- field `Func`2 <>9__9_0`

## MegaCrit.Sts2.Core.Saves.EnemyStats+<>c__DisplayClass12_0
- method `<IncrementWin>b__0(FightStats fight)` -> `Boolean`
- field `ModelId characterId`

## MegaCrit.Sts2.Core.Saves.EnemyStats+<>c__DisplayClass13_0
- method `<IncrementLoss>b__0(FightStats fight)` -> `Boolean`
- field `ModelId characterId`

## MegaCrit.Sts2.Core.Saves.EpochState
- field `EpochState None`
- field `EpochState NoSlot`
- field `EpochState NotObtained`
- field `EpochState Obtained`
- field `EpochState ObtainedNoSlot`
- field `EpochState Revealed`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Saves.FightStats
- method `get_Character()` -> `ModelId`
- method `get_Losses()` -> `Int32`
- method `get_Wins()` -> `Int32`
- method `set_Character(ModelId value)` -> `Void`
- method `set_Losses(Int32 value)` -> `Void`
- method `set_Wins(Int32 value)` -> `Void`
- field `ModelId <Character>k__BackingField`
- field `Int32 <Losses>k__BackingField`
- field `Int32 <Wins>k__BackingField`

## MegaCrit.Sts2.Core.Saves.FileAccessStream
- method `Dispose(Boolean disposing)` -> `Void`
- method `Flush()` -> `Void`
- method `get_CanRead()` -> `Boolean`
- method `get_CanSeek()` -> `Boolean`
- method `get_CanWrite()` -> `Boolean`
- method `get_Length()` -> `Int64`
- method `get_Position()` -> `Int64`
- method `Read(Byte[] buffer, Int32 offset, Int32 count)` -> `Int32`
- method `Seek(Int64 offset, SeekOrigin origin)` -> `Int64`
- method `set_Position(Int64 value)` -> `Void`
- method `SetLength(Int64 value)` -> `Void`
- method `Write(Byte[] buffer, Int32 offset, Int32 count)` -> `Void`
- field `FileAccess _file`
- field `String _filePath`
- field `ModeFlags _flags`

## MegaCrit.Sts2.Core.Saves.GodotFileIo
- method `CopyBackup(String fullPath)` -> `Void`
- method `CreateDirectory(String directoryPath)` -> `Void`
- method `DeleteDirectory(String directoryPath)` -> `Void`
- method `DeleteFile(String path)` -> `Void`
- method `DeleteTemporaryFiles(String directoryPath)` -> `Void`
- method `DirectoryExists(String path)` -> `Boolean`
- method `FileExists(String path)` -> `Boolean`
- method `get_SaveDir()` -> `String`
- method `GetDirectoriesInDirectory(String directoryPath)` -> `String[]`
- method `GetFilesInDirectory(String directoryPath)` -> `String[]`
- method `GetFileSize(String path)` -> `Int32`
- method `GetFullPath(String filename)` -> `String`
- method `GetLastModifiedTime(String path)` -> `DateTimeOffset`
- method `ReadFile(String path)` -> `String`
- method `ReadFileAsync(String path)` -> `Task`1`
- method `RenameFile(String sourcePath, String destinationPath)` -> `Void`
- method `set_SaveDir(String value)` -> `Void`
- method `SetLastModifiedTime(String path, DateTimeOffset time)` -> `Void`
- method `ValidateGodotFilePath(String godotFilePath)` -> `Void`
- method `WriteFile(String path, String content)` -> `Void`
- method `WriteFile(String path, Byte[] bytes)` -> `Void`
- method `WriteFileAsync(String path, String content)` -> `Task`
- method `WriteFileAsync(String path, Byte[] bytes)` -> `Task`
- field `String <SaveDir>k__BackingField`

## MegaCrit.Sts2.Core.Saves.GodotFileIo+<ReadFileAsync>d__7
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `GodotFileIo <>4__this`
- field `Object <>7__wrap3`
- field `Int32 <>7__wrap4`
- field `String <>7__wrap5`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter <>u__1`
- field `ValueTaskAwaiter <>u__2`
- field `MemoryStream <memoryStream>5__3`
- field `FileAccessStream <stream>5__2`
- field `String path`

## MegaCrit.Sts2.Core.Saves.GodotFileIo+<WriteFileAsync>d__14
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `GodotFileIo <>4__this`
- field `Object <>7__wrap3`
- field `Int32 <>7__wrap4`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `ValueTaskAwaiter <>u__1`
- field `FileAccessStream <stream>5__3`
- field `String <tempPath>5__2`
- field `Byte[] bytes`
- field `String path`

## MegaCrit.Sts2.Core.Saves.ICloudSaveStore
- method `BeginSaveBatch()` -> `Void`
- method `EndSaveBatch()` -> `Void`
- method `ForgetFile(String path)` -> `Void`
- method `HasCloudFiles()` -> `Boolean`
- method `IsFilePersisted(String path)` -> `Boolean`

## MegaCrit.Sts2.Core.Saves.IProfileIdProvider
- method `get_CurrentProfileId()` -> `Int32`

## MegaCrit.Sts2.Core.Saves.ISaveSchema
- method `get_SchemaVersion()` -> `Int32`
- method `set_SchemaVersion(Int32 value)` -> `Void`

## MegaCrit.Sts2.Core.Saves.ISaveStore
- method `CreateDirectory(String directoryPath)` -> `Void`
- method `DeleteDirectory(String directoryPath)` -> `Void`
- method `DeleteFile(String path)` -> `Void`
- method `DeleteTemporaryFiles(String directoryPath)` -> `Void`
- method `DirectoryExists(String path)` -> `Boolean`
- method `FileExists(String path)` -> `Boolean`
- method `GetDirectoriesInDirectory(String directoryPath)` -> `String[]`
- method `GetFilesInDirectory(String directoryPath)` -> `String[]`
- method `GetFileSize(String path)` -> `Int32`
- method `GetFullPath(String filename)` -> `String`
- method `GetLastModifiedTime(String path)` -> `DateTimeOffset`
- method `ReadFile(String path)` -> `String`
- method `ReadFileAsync(String path)` -> `Task`1`
- method `RenameFile(String sourcePath, String destinationPath)` -> `Void`
- method `SetLastModifiedTime(String path, DateTimeOffset time)` -> `Void`
- method `WriteFile(String path, String content)` -> `Void`
- method `WriteFile(String path, Byte[] content)` -> `Void`
- method `WriteFileAsync(String path, String content)` -> `Task`
- method `WriteFileAsync(String path, Byte[] content)` -> `Task`

## MegaCrit.Sts2.Core.Saves.JsonSerializationUtility
- method `AddTypeInfoResolver(IJsonTypeInfoResolver resolver)` -> `Void`
- method `AlphabetizeProperties(JsonTypeInfo info)` -> `Void`
- method `FromJson(String json)` -> `ReadSaveResult`1`
- method `get_DefaultResolver()` -> `IJsonTypeInfoResolver`
- method `get_Options()` -> `JsonSerializerOptions`
- method `GetTypeInfo(T value)` -> `JsonTypeInfo`1`
- method `GetTypeInfo()` -> `JsonTypeInfo`1`
- method `SerializeAsync(T data)` -> `Task`1`
- method `ToJson(T obj)` -> `String`
- field `IJsonTypeInfoResolver <DefaultResolver>k__BackingField`
- field `JsonSerializerOptions <Options>k__BackingField`

## MegaCrit.Sts2.Core.Saves.JsonSerializationUtility+<>c
- method `<AlphabetizeProperties>b__10_0(JsonPropertyInfo p1, JsonPropertyInfo p2)` -> `Int32`
- field `<>c <>9`
- field `Comparison`1 <>9__10_0`

## MegaCrit.Sts2.Core.Saves.JsonSerializationUtility+<SerializeAsync>d__7`1
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter <>u__1`
- field `TaskAwaiter`1 <>u__2`
- field `StreamReader <reader>5__3`
- field `MemoryStream <stream>5__2`
- field `T data`

## MegaCrit.Sts2.Core.Saves.Managers.PrefsSaveManager
- method `get_Prefs()` -> `PrefsSave`
- method `GetPrefsPath(Int32 profileId)` -> `String`
- method `LoadPrefs()` -> `ReadSaveResult`1`
- method `SavePrefs()` -> `Void`
- method `set_Prefs(PrefsSave value)` -> `Void`
- field `MigrationManager _migrationManager`
- field `IProfileIdProvider _profileIdProvider`
- field `ISaveStore _saveStore`
- field `PrefsSave <Prefs>k__BackingField`
- field `String fileName`

## MegaCrit.Sts2.Core.Saves.Managers.ProfileSaveManager
- method `get_Profile()` -> `ProfileSave`
- method `get_ProfilePath()` -> `String`
- method `LoadProfile()` -> `ReadSaveResult`1`
- method `SaveProfile()` -> `Void`
- method `set_Profile(ProfileSave value)` -> `Void`
- field `MigrationManager _migrationManager`
- field `ISaveStore _saveStore`
- field `ProfileSave <Profile>k__BackingField`
- field `Int32 maxProfileCount`
- field `String profileSaveFileName`

## MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager
- method `<UpdateEpochsPostRun>b__20_0(AncientEventModel a)` -> `Boolean`
- method `CheckAscensionOneCompleted(SerializablePlayer serializablePlayer, SerializableRun serializableRun)` -> `Void`
- method `CheckFifteenBossesDefeatedEpoch(Player localPlayer)` -> `Void`
- method `CheckFifteenElitesDefeatedEpoch(Player localPlayer)` -> `Void`
- method `GenerateUnlockState()` -> `UnlockState`
- method `get_Progress()` -> `ProgressState`
- method `GetEliteEncounters()` -> `HashSet`1`
- method `GetProgressPathForProfile(Int32 profileId)` -> `String`
- method `GetRevealableEpochs()` -> `IEnumerable`1`
- method `IncrementEncounterLoss(ModelId characterId, ModelId encounterId)` -> `Void`
- method `IncrementEnemyFightLoss(ModelId characterId, ModelId monster)` -> `Void`
- method `IncrementMultiplayerAscension(SerializableRun run)` -> `Void`
- method `IncrementSingleplayerAscension(SerializableRun run, CharacterStats charStats)` -> `Void`
- method `LoadProgress()` -> `ReadSaveResult`1`
- method `MarkCardAsSeen(CardModel card)` -> `Void`
- method `MarkFtueAsComplete(String ftueId)` -> `Void`
- method `MarkPotionAsSeen(PotionModel potion)` -> `Void`
- method `MarkRelicAsSeen(RelicModel relic)` -> `Void`
- method `ObtainCharUnlockEpoch(Player localPlayer, Int32 act)` -> `Void`
- method `PostRunCharacterEpochChecks(SerializablePlayer serializablePlayer, SerializableRun serializableRun, Boolean victory)` -> `Void`
- method `PostRunUnlockCharacterEpochCheck(SerializablePlayer serializablePlayer, SerializableRun serializableRun)` -> `Void`
- method `ResetFtues()` -> `Void`
- method `SaveProgress()` -> `Void`
- method `SeenFtue(String ftueKey)` -> `Boolean`
- method `set_Progress(ProgressState value)` -> `Void`
- method `SetFtuesEnabled(Boolean enabled)` -> `Void`
- method `TryObtainEpochInternal(EpochModel epoch)` -> `Boolean`
- method `TryObtainEpochMidRun(EpochModel epoch, Player localPlayer)` -> `Boolean`
- method `TryObtainEpochPostRun(EpochModel epoch, SerializablePlayer serializablePlayer, SerializableRun serializableRun)` -> `Boolean`
- method `UpdateAfterCombatWon(Player localPlayer, CombatRoom room)` -> `Void`
- method `UpdateEpochsPostRun(SerializablePlayer serializablePlayer, SerializableRun serializableRun, Boolean victory)` -> `Void`
- method `UpdateWithRunData(SerializableRun serializableRun, Boolean victory)` -> `Void`
- field `HashSet`1 _eliteEncounters`
- field `MigrationManager _migrationManager`
- field `IProfileIdProvider _profileIdProvider`
- field `ISaveStore _saveStore`
- field `ProgressState <Progress>k__BackingField`
- field `String fileName`

## MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager+<>c
- method `<CheckFifteenBossesDefeatedEpoch>b__25_0(ActModel a)` -> `IEnumerable`1`
- method `<CheckFifteenBossesDefeatedEpoch>b__25_1(EncounterModel e)` -> `ModelId`
- method `<GetEliteEncounters>b__39_0(EncounterModel e)` -> `Boolean`
- method `<GetEliteEncounters>b__39_1(EncounterModel e)` -> `ModelId`
- method `<GetRevealableEpochs>b__30_0(SerializableEpoch e)` -> `Boolean`
- method `<GetRevealableEpochs>b__30_1(SerializableEpoch e)` -> `String`
- method `<UpdateWithRunData>b__19_0(List`1 act)` -> `IEnumerable`1`
- method `<UpdateWithRunData>b__19_2(SerializableCard c)` -> `ModelId`
- field `<>c <>9`
- field `Func`2 <>9__19_0`
- field `Func`2 <>9__19_2`
- field `Func`2 <>9__25_0`
- field `Func`2 <>9__25_1`
- field `Func`2 <>9__30_0`
- field `Func`2 <>9__30_1`
- field `Func`2 <>9__39_0`
- field `Func`2 <>9__39_1`

## MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager+<>c__DisplayClass14_0
- method `<IncrementEncounterLoss>b__0(FightStats f)` -> `Boolean`
- field `ModelId characterId`

## MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager+<>c__DisplayClass15_0
- method `<IncrementEnemyFightLoss>b__0(FightStats f)` -> `Boolean`
- field `ModelId characterId`

## MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager+<>c__DisplayClass19_0
- method `<UpdateWithRunData>b__1(SerializablePlayer p)` -> `Boolean`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager+<>c__DisplayClass29_0
- method `<TryObtainEpochInternal>b__0(SerializableEpoch e)` -> `Boolean`
- method `<TryObtainEpochInternal>b__1(Scope scope)` -> `Void`
- field `ProgressSaveManager <>4__this`
- field `EpochModel epoch`

## MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager+<>c__DisplayClass30_0
- method `<GetRevealableEpochs>b__2(SerializableEpoch e)` -> `Boolean`
- field `HashSet`1 reachableSet`
- field `HashSet`1 satisfiedEpochIds`

## MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager+<>c__DisplayClass31_0
- method `<UpdateAfterCombatWon>b__0(FightStats f)` -> `Boolean`
- method `<UpdateAfterCombatWon>b__1(FightStats f)` -> `Boolean`
- field `Func`2 <>9__1`
- field `CharacterModel character`

## MegaCrit.Sts2.Core.Saves.Managers.RunHistorySaveManager
- method `CreateRunHistoryDirectory()` -> `Void`
- method `get_HistoryPath()` -> `String`
- method `GetHistoryCount()` -> `Int32`
- method `GetHistoryPath(Int32 profileId)` -> `String`
- method `LoadAllRunHistoryNames()` -> `List`1`
- method `LoadHistory(String fileName)` -> `ReadSaveResult`1`
- method `SaveHistory(RunHistory history)` -> `Void`
- method `SaveHistoryInternal(String path, String content)` -> `Void`
- field `String _historyDirName`
- field `MigrationManager _migrationManager`
- field `IProfileIdProvider _profileIdProvider`
- field `ISaveStore _saveStore`
- field `Int32 maxCloudBytes`
- field `Int32 maxCloudFileCount`

## MegaCrit.Sts2.Core.Saves.Managers.RunSaveManager
- method `add_Saved(Action value)` -> `Void`
- method `DeleteCurrentMultiplayerRun()` -> `Void`
- method `DeleteCurrentRun()` -> `Void`
- method `get_CurrentMultiplayerRunSavePath()` -> `String`
- method `get_CurrentRunSavePath()` -> `String`
- method `get_HasMultiplayerRunSave()` -> `Boolean`
- method `get_HasRunSave()` -> `Boolean`
- method `get_SchemaVersion()` -> `Int32`
- method `GetRunSavePath(Int32 profileId, String fileName)` -> `String`
- method `LoadAndCanonicalizeMultiplayerRunSave(UInt64 localPlayerId)` -> `ReadSaveResult`1`
- method `LoadMultiplayerRunSave()` -> `ReadSaveResult`1`
- method `LoadRunSave()` -> `ReadSaveResult`1`
- method `remove_Saved(Action value)` -> `Void`
- method `RenameBrokenMultiplayerRunSave(ReadSaveStatus status)` -> `Void`
- method `SaveRun(AbstractRoom preFinishedRoom)` -> `Task`
- field `Boolean _forceSynchronous`
- field `MigrationManager _migrationManager`
- field `IProfileIdProvider _profileIdProvider`
- field `ISaveStore _saveStore`
- field `String multiplayerRunSaveFileName`
- field `String runSaveFileName`
- field `Action Saved`

## MegaCrit.Sts2.Core.Saves.Managers.RunSaveManager+<SaveRun>d__21
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `RunSaveManager <>4__this`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `String <savePath>5__2`
- field `MemoryStream <stream>5__3`
- field `AbstractRoom preFinishedRoom`

## MegaCrit.Sts2.Core.Saves.Managers.SettingsSaveManager
- method `ApplyPlatformDefaults(SettingsSave settings)` -> `Void`
- method `get_Settings()` -> `SettingsSave`
- method `get_SettingsPath()` -> `String`
- method `LoadSettings()` -> `ReadSaveResult`1`
- method `SaveSettings()` -> `Void`
- method `set_Settings(SettingsSave value)` -> `Void`
- field `MigrationManager _migrationManager`
- field `ISaveStore _saveStore`
- field `SettingsSave <Settings>k__BackingField`
- field `String settingsSaveFileName`

## MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `QuantizeParams _quantizeParamsX`
- field `QuantizeParams _quantizeParamsY`
- field `Boolean isEraser`
- field `List`1 mapPoints`

## MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings
- method `Anonymized()` -> `SerializableMapDrawings`
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `List`1 drawings`

## MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings+<>c
- method `<Anonymized>b__3_0(SerializablePlayerMapDrawings d)` -> `SerializablePlayerMapDrawings`
- field `<>c <>9`
- field `Func`2 <>9__3_0`

## MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingsJsonConverter
- method `Read(Utf8JsonReader& reader, Type typeToConvert, JsonSerializerOptions options)` -> `SerializableMapDrawings`
- method `Write(Utf8JsonWriter writer, SerializableMapDrawings mapDrawings, JsonSerializerOptions options)` -> `Void`
- field `PacketWriter _packetWriter`

## MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings
- method `Anonymized()` -> `SerializablePlayerMapDrawings`
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Serialize(PacketWriter writer)` -> `Void`
- field `List`1 lines`
- field `UInt64 playerId`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext
- method `AncientCharacterStatsCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `AncientCharacterStatsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `AncientChoiceHistoryEntryPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `AncientStatsCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `AncientStatsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `BadgeStatsCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `BadgeStatsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `CardChoiceHistoryEntryPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `CardEnchantmentHistoryEntryPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `CardStatsCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `CardStatsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `CardTransformationHistoryEntryPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `CharacterStatsCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `CharacterStatsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `Create_AncientCharacterStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_AncientChoiceHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_AncientStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_AspectRatioSetting(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_BadgeRarity(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_BadgeStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_Boolean(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_CardChoiceHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_CardCreationSource(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_CardEnchantmentHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_CardRarityOddsType(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_CardStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_CardTransformationHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_CharacterStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ControllerMappingType(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_DateTimeOffset(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_DictionaryPlayerRngTypeInt32(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_DictionaryRelicRarityListModelId(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_DictionaryRunRngTypeInt32(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_DictionaryStringObject(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_DictionaryStringString(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_DictionaryUInt64ListSerializableReward(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_Double(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_EncounterStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_EnemyStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_EpochState(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_EventOptionHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_FastModeType(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_FeedbackData(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_FightStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_GameMode(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_IEnumerableSerializableBadge(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_IEnumerableSerializableCard(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_IEnumerableSerializablePotion(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_IEnumerableSerializableRelic(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_Int32(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_Int32Array(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_Int64(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_JsonNode(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_JsonObject(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListAncientCharacterStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListAncientChoiceHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListAncientStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListBadgeStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListCardChoiceHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListCardEnchantmentHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListCardStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListCardTransformationHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListCharacterStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListDictionaryStringObject(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListEncounterStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListEnemyStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListEventOptionHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListFightStats(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListJsonNode(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListListMapPointHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListListPlayerMapPointHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListMapCoord(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListMapPointHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListMapPointRoomHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListMigratingData(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListModelChoiceHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListModelId(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListNullLeaderboard(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListNullLeaderboardFileEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListNullMultiplayerName(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListPlayerMapPointHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListRunHistoryPlayer(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSavedPropertyBoolean(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSavedPropertyInt32(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSavedPropertyInt32Array(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSavedPropertyModelId(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSavedPropertySerializableCard(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSavedPropertySerializableCardArray(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSavedPropertyString(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSerializableActModel(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSerializableCard(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSerializableEpoch(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSerializableMapDrawingLine(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSerializableMapPoint(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSerializableModifier(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSerializablePlayer(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSerializablePlayerMapDrawings(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSerializablePotion(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSerializableRelic(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSerializableReward(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSerializableUnlockedAchievement(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListSettingsSaveMod(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListString(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListUInt64(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ListVector2(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_LocString(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_MapCoord(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_MapPointHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_MapPointRoomHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_MapPointType(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_MigratingData(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ModelChoiceHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ModelId(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ModManifest(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ModSettings(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ModSource(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_NullableDateTimeOffset(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_NullableInt32(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_NullLeaderboard(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_NullLeaderboardFile(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_NullLeaderboardFileEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_NullMultiplayerName(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_Object(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_PlatformType(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_PlayerMapPointHistoryEntry(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_PlayerRngType(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_PrefsSave(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_ProfileSave(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_RelicRarity(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_RewardType(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_RoomType(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_RunHistory(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_RunHistoryPlayer(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_RunRngType(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SavedProperties(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SavedPropertyBoolean(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SavedPropertyInt32(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SavedPropertyInt32Array(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SavedPropertyModelId(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SavedPropertySerializableCard(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SavedPropertySerializableCardArray(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SavedPropertyString(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableActMap(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableActModel(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableBadge(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableCard(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableCardArray(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableEnchantment(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableEpoch(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableExtraPlayerFields(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableExtraRunFields(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableMapDrawingLine(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableMapDrawings(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableMapPoint(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableModifier(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializablePlayer(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializablePlayerMapDrawings(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializablePlayerOddsSet(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializablePlayerRngSet(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializablePotion(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableProgress(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableRelic(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableRelicGrabBag(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableReward(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableRoom(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableRoomSet(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableRun(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableRunOddsSet(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableRunRngSet(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableUnlockedAchievement(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SerializableUnlockState(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SettingsSave(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_SettingsSaveMod(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_Single(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_String(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_UInt32(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_UInt64(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_Vector2(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_Vector2I(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `Create_VSyncType(JsonSerializerOptions options)` -> `JsonTypeInfo`1`
- method `EncounterStatsCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `EncounterStatsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `EnemyStatsCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `EnemyStatsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `EventOptionHistoryEntryPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `ExpandConverter(Type type, JsonConverter converter, JsonSerializerOptions options, Boolean validateCanConvert)` -> `JsonConverter`
- method `FeedbackDataPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `FightStatsCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `FightStatsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `get_AncientCharacterStats()` -> `JsonTypeInfo`1`
- method `get_AncientChoiceHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_AncientStats()` -> `JsonTypeInfo`1`
- method `get_AspectRatioSetting()` -> `JsonTypeInfo`1`
- method `get_BadgeRarity()` -> `JsonTypeInfo`1`
- method `get_BadgeStats()` -> `JsonTypeInfo`1`
- method `get_Boolean()` -> `JsonTypeInfo`1`
- method `get_CardChoiceHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_CardCreationSource()` -> `JsonTypeInfo`1`
- method `get_CardEnchantmentHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_CardRarityOddsType()` -> `JsonTypeInfo`1`
- method `get_CardStats()` -> `JsonTypeInfo`1`
- method `get_CardTransformationHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_CharacterStats()` -> `JsonTypeInfo`1`
- method `get_ControllerMappingType()` -> `JsonTypeInfo`1`
- method `get_DateTimeOffset()` -> `JsonTypeInfo`1`
- method `get_Default()` -> `MegaCritSerializerContext`
- method `get_DefaultGeneratedSerializerOptions()` -> `JsonSerializerOptions`
- method `get_DictionaryPlayerRngTypeInt32()` -> `JsonTypeInfo`1`
- method `get_DictionaryRelicRarityListModelId()` -> `JsonTypeInfo`1`
- method `get_DictionaryRunRngTypeInt32()` -> `JsonTypeInfo`1`
- method `get_DictionaryStringObject()` -> `JsonTypeInfo`1`
- method `get_DictionaryStringString()` -> `JsonTypeInfo`1`
- method `get_DictionaryUInt64ListSerializableReward()` -> `JsonTypeInfo`1`
- method `get_Double()` -> `JsonTypeInfo`1`
- method `get_EncounterStats()` -> `JsonTypeInfo`1`
- method `get_EnemyStats()` -> `JsonTypeInfo`1`
- method `get_EpochState()` -> `JsonTypeInfo`1`
- method `get_EventOptionHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_FastModeType()` -> `JsonTypeInfo`1`
- method `get_FeedbackData()` -> `JsonTypeInfo`1`
- method `get_FightStats()` -> `JsonTypeInfo`1`
- method `get_GameMode()` -> `JsonTypeInfo`1`
- method `get_GeneratedSerializerOptions()` -> `JsonSerializerOptions`
- method `get_IEnumerableSerializableBadge()` -> `JsonTypeInfo`1`
- method `get_IEnumerableSerializableCard()` -> `JsonTypeInfo`1`
- method `get_IEnumerableSerializablePotion()` -> `JsonTypeInfo`1`
- method `get_IEnumerableSerializableRelic()` -> `JsonTypeInfo`1`
- method `get_Int32()` -> `JsonTypeInfo`1`
- method `get_Int32Array()` -> `JsonTypeInfo`1`
- method `get_Int64()` -> `JsonTypeInfo`1`
- method `get_JsonNode()` -> `JsonTypeInfo`1`
- method `get_JsonObject()` -> `JsonTypeInfo`1`
- method `get_ListAncientCharacterStats()` -> `JsonTypeInfo`1`
- method `get_ListAncientChoiceHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_ListAncientStats()` -> `JsonTypeInfo`1`
- method `get_ListBadgeStats()` -> `JsonTypeInfo`1`
- method `get_ListCardChoiceHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_ListCardEnchantmentHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_ListCardStats()` -> `JsonTypeInfo`1`
- method `get_ListCardTransformationHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_ListCharacterStats()` -> `JsonTypeInfo`1`
- method `get_ListDictionaryStringObject()` -> `JsonTypeInfo`1`
- method `get_ListEncounterStats()` -> `JsonTypeInfo`1`
- method `get_ListEnemyStats()` -> `JsonTypeInfo`1`
- method `get_ListEventOptionHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_ListFightStats()` -> `JsonTypeInfo`1`
- method `get_ListJsonNode()` -> `JsonTypeInfo`1`
- method `get_ListListMapPointHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_ListListPlayerMapPointHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_ListMapCoord()` -> `JsonTypeInfo`1`
- method `get_ListMapPointHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_ListMapPointRoomHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_ListMigratingData()` -> `JsonTypeInfo`1`
- method `get_ListModelChoiceHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_ListModelId()` -> `JsonTypeInfo`1`
- method `get_ListNullLeaderboard()` -> `JsonTypeInfo`1`
- method `get_ListNullLeaderboardFileEntry()` -> `JsonTypeInfo`1`
- method `get_ListNullMultiplayerName()` -> `JsonTypeInfo`1`
- method `get_ListPlayerMapPointHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_ListRunHistoryPlayer()` -> `JsonTypeInfo`1`
- method `get_ListSavedPropertyBoolean()` -> `JsonTypeInfo`1`
- method `get_ListSavedPropertyInt32()` -> `JsonTypeInfo`1`
- method `get_ListSavedPropertyInt32Array()` -> `JsonTypeInfo`1`
- method `get_ListSavedPropertyModelId()` -> `JsonTypeInfo`1`
- method `get_ListSavedPropertySerializableCard()` -> `JsonTypeInfo`1`
- method `get_ListSavedPropertySerializableCardArray()` -> `JsonTypeInfo`1`
- method `get_ListSavedPropertyString()` -> `JsonTypeInfo`1`
- method `get_ListSerializableActModel()` -> `JsonTypeInfo`1`
- method `get_ListSerializableCard()` -> `JsonTypeInfo`1`
- method `get_ListSerializableEpoch()` -> `JsonTypeInfo`1`
- method `get_ListSerializableMapDrawingLine()` -> `JsonTypeInfo`1`
- method `get_ListSerializableMapPoint()` -> `JsonTypeInfo`1`
- method `get_ListSerializableModifier()` -> `JsonTypeInfo`1`
- method `get_ListSerializablePlayer()` -> `JsonTypeInfo`1`
- method `get_ListSerializablePlayerMapDrawings()` -> `JsonTypeInfo`1`
- method `get_ListSerializablePotion()` -> `JsonTypeInfo`1`
- method `get_ListSerializableRelic()` -> `JsonTypeInfo`1`
- method `get_ListSerializableReward()` -> `JsonTypeInfo`1`
- method `get_ListSerializableUnlockedAchievement()` -> `JsonTypeInfo`1`
- method `get_ListSettingsSaveMod()` -> `JsonTypeInfo`1`
- method `get_ListString()` -> `JsonTypeInfo`1`
- method `get_ListUInt64()` -> `JsonTypeInfo`1`
- method `get_ListVector2()` -> `JsonTypeInfo`1`
- method `get_LocString()` -> `JsonTypeInfo`1`
- method `get_MapCoord()` -> `JsonTypeInfo`1`
- method `get_MapPointHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_MapPointRoomHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_MapPointType()` -> `JsonTypeInfo`1`
- method `get_MigratingData()` -> `JsonTypeInfo`1`
- method `get_ModelChoiceHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_ModelId()` -> `JsonTypeInfo`1`
- method `get_ModManifest()` -> `JsonTypeInfo`1`
- method `get_ModSettings()` -> `JsonTypeInfo`1`
- method `get_ModSource()` -> `JsonTypeInfo`1`
- method `get_NullableDateTimeOffset()` -> `JsonTypeInfo`1`
- method `get_NullableInt32()` -> `JsonTypeInfo`1`
- method `get_NullLeaderboard()` -> `JsonTypeInfo`1`
- method `get_NullLeaderboardFile()` -> `JsonTypeInfo`1`
- method `get_NullLeaderboardFileEntry()` -> `JsonTypeInfo`1`
- method `get_NullMultiplayerName()` -> `JsonTypeInfo`1`
- method `get_Object()` -> `JsonTypeInfo`1`
- method `get_PlatformType()` -> `JsonTypeInfo`1`
- method `get_PlayerMapPointHistoryEntry()` -> `JsonTypeInfo`1`
- method `get_PlayerRngType()` -> `JsonTypeInfo`1`
- method `get_PrefsSave()` -> `JsonTypeInfo`1`
- method `get_ProfileSave()` -> `JsonTypeInfo`1`
- method `get_RelicRarity()` -> `JsonTypeInfo`1`
- method `get_RewardType()` -> `JsonTypeInfo`1`
- method `get_RoomType()` -> `JsonTypeInfo`1`
- method `get_RunHistory()` -> `JsonTypeInfo`1`
- method `get_RunHistoryPlayer()` -> `JsonTypeInfo`1`
- method `get_RunRngType()` -> `JsonTypeInfo`1`
- method `get_SavedProperties()` -> `JsonTypeInfo`1`
- method `get_SavedPropertyBoolean()` -> `JsonTypeInfo`1`
- method `get_SavedPropertyInt32()` -> `JsonTypeInfo`1`
- method `get_SavedPropertyInt32Array()` -> `JsonTypeInfo`1`
- method `get_SavedPropertyModelId()` -> `JsonTypeInfo`1`
- method `get_SavedPropertySerializableCard()` -> `JsonTypeInfo`1`
- method `get_SavedPropertySerializableCardArray()` -> `JsonTypeInfo`1`
- method `get_SavedPropertyString()` -> `JsonTypeInfo`1`
- method `get_SerializableActMap()` -> `JsonTypeInfo`1`
- method `get_SerializableActModel()` -> `JsonTypeInfo`1`
- method `get_SerializableBadge()` -> `JsonTypeInfo`1`
- method `get_SerializableCard()` -> `JsonTypeInfo`1`
- method `get_SerializableCardArray()` -> `JsonTypeInfo`1`
- method `get_SerializableEnchantment()` -> `JsonTypeInfo`1`
- method `get_SerializableEpoch()` -> `JsonTypeInfo`1`
- method `get_SerializableExtraPlayerFields()` -> `JsonTypeInfo`1`
- method `get_SerializableExtraRunFields()` -> `JsonTypeInfo`1`
- method `get_SerializableMapDrawingLine()` -> `JsonTypeInfo`1`
- method `get_SerializableMapDrawings()` -> `JsonTypeInfo`1`
- method `get_SerializableMapPoint()` -> `JsonTypeInfo`1`
- method `get_SerializableModifier()` -> `JsonTypeInfo`1`
- method `get_SerializablePlayer()` -> `JsonTypeInfo`1`
- method `get_SerializablePlayerMapDrawings()` -> `JsonTypeInfo`1`
- method `get_SerializablePlayerOddsSet()` -> `JsonTypeInfo`1`
- method `get_SerializablePlayerRngSet()` -> `JsonTypeInfo`1`
- method `get_SerializablePotion()` -> `JsonTypeInfo`1`
- method `get_SerializableProgress()` -> `JsonTypeInfo`1`
- method `get_SerializableRelic()` -> `JsonTypeInfo`1`
- method `get_SerializableRelicGrabBag()` -> `JsonTypeInfo`1`
- method `get_SerializableReward()` -> `JsonTypeInfo`1`
- method `get_SerializableRoom()` -> `JsonTypeInfo`1`
- method `get_SerializableRoomSet()` -> `JsonTypeInfo`1`
- method `get_SerializableRun()` -> `JsonTypeInfo`1`
- method `get_SerializableRunOddsSet()` -> `JsonTypeInfo`1`
- method `get_SerializableRunRngSet()` -> `JsonTypeInfo`1`
- method `get_SerializableUnlockedAchievement()` -> `JsonTypeInfo`1`
- method `get_SerializableUnlockState()` -> `JsonTypeInfo`1`
- method `get_SettingsSave()` -> `JsonTypeInfo`1`
- method `get_SettingsSaveMod()` -> `JsonTypeInfo`1`
- method `get_Single()` -> `JsonTypeInfo`1`
- method `get_String()` -> `JsonTypeInfo`1`
- method `get_UInt32()` -> `JsonTypeInfo`1`
- method `get_UInt64()` -> `JsonTypeInfo`1`
- method `get_Vector2()` -> `JsonTypeInfo`1`
- method `get_Vector2I()` -> `JsonTypeInfo`1`
- method `get_VSyncType()` -> `JsonTypeInfo`1`
- method `GetRuntimeConverterForType(Type type, JsonSerializerOptions options)` -> `JsonConverter`
- method `GetTypeInfo(Type type)` -> `JsonTypeInfo`
- method `global::System.Text.Json.Serialization.Metadata.IJsonTypeInfoResolver.GetTypeInfo(Type type, JsonSerializerOptions options)` -> `JsonTypeInfo`
- method `LocStringCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `LocStringPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `MapCoordPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `MapPointHistoryEntryPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `MapPointRoomHistoryEntryPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `MigratingDataPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `ModelChoiceHistoryEntryPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `ModelIdCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `ModelIdPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `ModManifestPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `ModSettingsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `NullLeaderboardFileEntryCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `NullLeaderboardFileEntryPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `NullLeaderboardFilePropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `NullLeaderboardPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `NullMultiplayerNamePropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `PlayerMapPointHistoryEntryPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `PrefsSavePropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `ProfileSavePropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `RunHistoryCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `RunHistoryPlayerCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `RunHistoryPlayerPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `RunHistoryPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SavedPropertiesPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SavedPropertyBooleanPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SavedPropertyInt32ArrayPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SavedPropertyInt32PropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SavedPropertyModelIdPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SavedPropertySerializableCardArrayPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SavedPropertySerializableCardPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SavedPropertyStringPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableActMapPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableActModelPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableBadgeCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `SerializableBadgePropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableCardPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableEnchantmentPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableEpochCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `SerializableEpochPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableExtraPlayerFieldsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableExtraRunFieldsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableMapDrawingLinePropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableMapDrawingsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableMapPointPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableModifierPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializablePlayerMapDrawingsPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializablePlayerOddsSetPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializablePlayerPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializablePlayerRngSetPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializablePotionPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableProgressCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `SerializableProgressPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableRelicGrabBagPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableRelicPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableRewardPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableRoomPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableRoomSetPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableRunOddsSetPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableRunPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableRunRngSetPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableUnlockedAchievementCtorParamInit()` -> `JsonParameterInfoValues[]`
- method `SerializableUnlockedAchievementPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SerializableUnlockStatePropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SettingsSaveModPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `SettingsSavePropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `TryGetTypeInfoForRuntimeCustomConverter(JsonSerializerOptions options, JsonTypeInfo`1& jsonTypeInfo)` -> `Boolean`
- method `Vector2IPropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- method `Vector2PropInit(JsonSerializerOptions options)` -> `JsonPropertyInfo[]`
- field `JsonTypeInfo`1 _AncientCharacterStats`
- field `JsonTypeInfo`1 _AncientChoiceHistoryEntry`
- field `JsonTypeInfo`1 _AncientStats`
- field `JsonTypeInfo`1 _AspectRatioSetting`
- field `JsonTypeInfo`1 _BadgeRarity`
- field `JsonTypeInfo`1 _BadgeStats`
- field `JsonTypeInfo`1 _Boolean`
- field `JsonTypeInfo`1 _CardChoiceHistoryEntry`
- field `JsonTypeInfo`1 _CardCreationSource`
- field `JsonTypeInfo`1 _CardEnchantmentHistoryEntry`
- field `JsonTypeInfo`1 _CardRarityOddsType`
- field `JsonTypeInfo`1 _CardStats`
- field `JsonTypeInfo`1 _CardTransformationHistoryEntry`
- field `JsonTypeInfo`1 _CharacterStats`
- field `JsonTypeInfo`1 _ControllerMappingType`
- field `JsonTypeInfo`1 _DateTimeOffset`
- field `JsonTypeInfo`1 _DictionaryPlayerRngTypeInt32`
- field `JsonTypeInfo`1 _DictionaryRelicRarityListModelId`
- field `JsonTypeInfo`1 _DictionaryRunRngTypeInt32`
- field `JsonTypeInfo`1 _DictionaryStringObject`
- field `JsonTypeInfo`1 _DictionaryStringString`
- field `JsonTypeInfo`1 _DictionaryUInt64ListSerializableReward`
- field `JsonTypeInfo`1 _Double`
- field `JsonTypeInfo`1 _EncounterStats`
- field `JsonTypeInfo`1 _EnemyStats`
- field `JsonTypeInfo`1 _EpochState`
- field `JsonTypeInfo`1 _EventOptionHistoryEntry`
- field `JsonTypeInfo`1 _FastModeType`
- field `JsonTypeInfo`1 _FeedbackData`
- field `JsonTypeInfo`1 _FightStats`
- field `JsonTypeInfo`1 _GameMode`
- field `JsonTypeInfo`1 _IEnumerableSerializableBadge`
- field `JsonTypeInfo`1 _IEnumerableSerializableCard`
- field `JsonTypeInfo`1 _IEnumerableSerializablePotion`
- field `JsonTypeInfo`1 _IEnumerableSerializableRelic`
- field `JsonTypeInfo`1 _Int32`
- field `JsonTypeInfo`1 _Int32Array`
- field `JsonTypeInfo`1 _Int64`
- field `JsonTypeInfo`1 _JsonNode`
- field `JsonTypeInfo`1 _JsonObject`
- field `JsonTypeInfo`1 _ListAncientCharacterStats`
- field `JsonTypeInfo`1 _ListAncientChoiceHistoryEntry`
- field `JsonTypeInfo`1 _ListAncientStats`
- field `JsonTypeInfo`1 _ListBadgeStats`
- field `JsonTypeInfo`1 _ListCardChoiceHistoryEntry`
- field `JsonTypeInfo`1 _ListCardEnchantmentHistoryEntry`
- field `JsonTypeInfo`1 _ListCardStats`
- field `JsonTypeInfo`1 _ListCardTransformationHistoryEntry`
- field `JsonTypeInfo`1 _ListCharacterStats`
- field `JsonTypeInfo`1 _ListDictionaryStringObject`
- field `JsonTypeInfo`1 _ListEncounterStats`
- field `JsonTypeInfo`1 _ListEnemyStats`
- field `JsonTypeInfo`1 _ListEventOptionHistoryEntry`
- field `JsonTypeInfo`1 _ListFightStats`
- field `JsonTypeInfo`1 _ListJsonNode`
- field `JsonTypeInfo`1 _ListListMapPointHistoryEntry`
- field `JsonTypeInfo`1 _ListListPlayerMapPointHistoryEntry`
- field `JsonTypeInfo`1 _ListMapCoord`
- field `JsonTypeInfo`1 _ListMapPointHistoryEntry`
- field `JsonTypeInfo`1 _ListMapPointRoomHistoryEntry`
- field `JsonTypeInfo`1 _ListMigratingData`
- field `JsonTypeInfo`1 _ListModelChoiceHistoryEntry`
- field `JsonTypeInfo`1 _ListModelId`
- field `JsonTypeInfo`1 _ListNullLeaderboard`
- field `JsonTypeInfo`1 _ListNullLeaderboardFileEntry`
- field `JsonTypeInfo`1 _ListNullMultiplayerName`
- field `JsonTypeInfo`1 _ListPlayerMapPointHistoryEntry`
- field `JsonTypeInfo`1 _ListRunHistoryPlayer`
- field `JsonTypeInfo`1 _ListSavedPropertyBoolean`
- field `JsonTypeInfo`1 _ListSavedPropertyInt32`
- field `JsonTypeInfo`1 _ListSavedPropertyInt32Array`
- field `JsonTypeInfo`1 _ListSavedPropertyModelId`
- field `JsonTypeInfo`1 _ListSavedPropertySerializableCard`
- field `JsonTypeInfo`1 _ListSavedPropertySerializableCardArray`
- field `JsonTypeInfo`1 _ListSavedPropertyString`
- field `JsonTypeInfo`1 _ListSerializableActModel`
- field `JsonTypeInfo`1 _ListSerializableCard`
- field `JsonTypeInfo`1 _ListSerializableEpoch`
- field `JsonTypeInfo`1 _ListSerializableMapDrawingLine`
- field `JsonTypeInfo`1 _ListSerializableMapPoint`
- field `JsonTypeInfo`1 _ListSerializableModifier`
- field `JsonTypeInfo`1 _ListSerializablePlayer`
- field `JsonTypeInfo`1 _ListSerializablePlayerMapDrawings`
- field `JsonTypeInfo`1 _ListSerializablePotion`
- field `JsonTypeInfo`1 _ListSerializableRelic`
- field `JsonTypeInfo`1 _ListSerializableReward`
- field `JsonTypeInfo`1 _ListSerializableUnlockedAchievement`
- field `JsonTypeInfo`1 _ListSettingsSaveMod`
- field `JsonTypeInfo`1 _ListString`
- field `JsonTypeInfo`1 _ListUInt64`
- field `JsonTypeInfo`1 _ListVector2`
- field `JsonTypeInfo`1 _LocString`
- field `JsonTypeInfo`1 _MapCoord`
- field `JsonTypeInfo`1 _MapPointHistoryEntry`
- field `JsonTypeInfo`1 _MapPointRoomHistoryEntry`
- field `JsonTypeInfo`1 _MapPointType`
- field `JsonTypeInfo`1 _MigratingData`
- field `JsonTypeInfo`1 _ModelChoiceHistoryEntry`
- field `JsonTypeInfo`1 _ModelId`
- field `JsonTypeInfo`1 _ModManifest`
- field `JsonTypeInfo`1 _ModSettings`
- field `JsonTypeInfo`1 _ModSource`
- field `JsonTypeInfo`1 _NullableDateTimeOffset`
- field `JsonTypeInfo`1 _NullableInt32`
- field `JsonTypeInfo`1 _NullLeaderboard`
- field `JsonTypeInfo`1 _NullLeaderboardFile`
- field `JsonTypeInfo`1 _NullLeaderboardFileEntry`
- field `JsonTypeInfo`1 _NullMultiplayerName`
- field `JsonTypeInfo`1 _Object`
- field `JsonTypeInfo`1 _PlatformType`
- field `JsonTypeInfo`1 _PlayerMapPointHistoryEntry`
- field `JsonTypeInfo`1 _PlayerRngType`
- field `JsonTypeInfo`1 _PrefsSave`
- field `JsonTypeInfo`1 _ProfileSave`
- field `JsonTypeInfo`1 _RelicRarity`
- field `JsonTypeInfo`1 _RewardType`
- field `JsonTypeInfo`1 _RoomType`
- field `JsonTypeInfo`1 _RunHistory`
- field `JsonTypeInfo`1 _RunHistoryPlayer`
- field `JsonTypeInfo`1 _RunRngType`
- field `JsonTypeInfo`1 _SavedProperties`
- field `JsonTypeInfo`1 _SavedPropertyBoolean`
- field `JsonTypeInfo`1 _SavedPropertyInt32`
- field `JsonTypeInfo`1 _SavedPropertyInt32Array`
- field `JsonTypeInfo`1 _SavedPropertyModelId`
- field `JsonTypeInfo`1 _SavedPropertySerializableCard`
- field `JsonTypeInfo`1 _SavedPropertySerializableCardArray`
- field `JsonTypeInfo`1 _SavedPropertyString`
- field `JsonTypeInfo`1 _SerializableActMap`
- field `JsonTypeInfo`1 _SerializableActModel`
- field `JsonTypeInfo`1 _SerializableBadge`
- field `JsonTypeInfo`1 _SerializableCard`
- field `JsonTypeInfo`1 _SerializableCardArray`
- field `JsonTypeInfo`1 _SerializableEnchantment`
- field `JsonTypeInfo`1 _SerializableEpoch`
- field `JsonTypeInfo`1 _SerializableExtraPlayerFields`
- field `JsonTypeInfo`1 _SerializableExtraRunFields`
- field `JsonTypeInfo`1 _SerializableMapDrawingLine`
- field `JsonTypeInfo`1 _SerializableMapDrawings`
- field `JsonTypeInfo`1 _SerializableMapPoint`
- field `JsonTypeInfo`1 _SerializableModifier`
- field `JsonTypeInfo`1 _SerializablePlayer`
- field `JsonTypeInfo`1 _SerializablePlayerMapDrawings`
- field `JsonTypeInfo`1 _SerializablePlayerOddsSet`
- field `JsonTypeInfo`1 _SerializablePlayerRngSet`
- field `JsonTypeInfo`1 _SerializablePotion`
- field `JsonTypeInfo`1 _SerializableProgress`
- field `JsonTypeInfo`1 _SerializableRelic`
- field `JsonTypeInfo`1 _SerializableRelicGrabBag`
- field `JsonTypeInfo`1 _SerializableReward`
- field `JsonTypeInfo`1 _SerializableRoom`
- field `JsonTypeInfo`1 _SerializableRoomSet`
- field `JsonTypeInfo`1 _SerializableRun`
- field `JsonTypeInfo`1 _SerializableRunOddsSet`
- field `JsonTypeInfo`1 _SerializableRunRngSet`
- field `JsonTypeInfo`1 _SerializableUnlockedAchievement`
- field `JsonTypeInfo`1 _SerializableUnlockState`
- field `JsonTypeInfo`1 _SettingsSave`
- field `JsonTypeInfo`1 _SettingsSaveMod`
- field `JsonTypeInfo`1 _Single`
- field `JsonTypeInfo`1 _String`
- field `JsonTypeInfo`1 _UInt32`
- field `JsonTypeInfo`1 _UInt64`
- field `JsonTypeInfo`1 _Vector2`
- field `JsonTypeInfo`1 _Vector2I`
- field `JsonTypeInfo`1 _VSyncType`
- field `MegaCritSerializerContext <Default>k__BackingField`
- field `JsonSerializerOptions <GeneratedSerializerOptions>k__BackingField`
- field `BindingFlags InstanceMemberBindingFlags`
- field `JsonSerializerOptions s_defaultOptions`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c
- method `<AncientCharacterStatsPropInit>b__195_0(Object obj)` -> `ModelId`
- method `<AncientCharacterStatsPropInit>b__195_1(Object obj, ModelId value)` -> `Void`
- method `<AncientCharacterStatsPropInit>b__195_2()` -> `ICustomAttributeProvider`
- method `<AncientCharacterStatsPropInit>b__195_3(Object obj)` -> `Int32`
- method `<AncientCharacterStatsPropInit>b__195_4(Object obj, Int32 value)` -> `Void`
- method `<AncientCharacterStatsPropInit>b__195_5()` -> `ICustomAttributeProvider`
- method `<AncientCharacterStatsPropInit>b__195_6(Object obj)` -> `Int32`
- method `<AncientCharacterStatsPropInit>b__195_7(Object obj, Int32 value)` -> `Void`
- method `<AncientCharacterStatsPropInit>b__195_8()` -> `ICustomAttributeProvider`
- method `<AncientCharacterStatsPropInit>b__195_9()` -> `ICustomAttributeProvider`
- method `<AncientChoiceHistoryEntryPropInit>b__138_0(Object obj)` -> `LocString`
- method `<AncientChoiceHistoryEntryPropInit>b__138_1(Object obj, LocString value)` -> `Void`
- method `<AncientChoiceHistoryEntryPropInit>b__138_2()` -> `ICustomAttributeProvider`
- method `<AncientChoiceHistoryEntryPropInit>b__138_3(Object obj)` -> `Boolean`
- method `<AncientChoiceHistoryEntryPropInit>b__138_4(Object obj, Boolean value)` -> `Void`
- method `<AncientChoiceHistoryEntryPropInit>b__138_5()` -> `ICustomAttributeProvider`
- method `<AncientChoiceHistoryEntryPropInit>b__138_6(Object obj)` -> `String`
- method `<AncientChoiceHistoryEntryPropInit>b__138_7()` -> `ICustomAttributeProvider`
- method `<AncientStatsPropInit>b__201_0(Object obj)` -> `ModelId`
- method `<AncientStatsPropInit>b__201_1(Object obj, ModelId value)` -> `Void`
- method `<AncientStatsPropInit>b__201_2()` -> `ICustomAttributeProvider`
- method `<AncientStatsPropInit>b__201_3(Object obj)` -> `List`1`
- method `<AncientStatsPropInit>b__201_4(Object obj, List`1 value)` -> `Void`
- method `<AncientStatsPropInit>b__201_5()` -> `ICustomAttributeProvider`
- method `<AncientStatsPropInit>b__201_6()` -> `ICustomAttributeProvider`
- method `<AncientStatsPropInit>b__201_7()` -> `ICustomAttributeProvider`
- method `<AncientStatsPropInit>b__201_8()` -> `ICustomAttributeProvider`
- method `<BadgeStatsPropInit>b__207_0(Object obj)` -> `String`
- method `<BadgeStatsPropInit>b__207_1(Object obj, String value)` -> `Void`
- method `<BadgeStatsPropInit>b__207_2()` -> `ICustomAttributeProvider`
- method `<BadgeStatsPropInit>b__207_3(Object obj)` -> `Int32`
- method `<BadgeStatsPropInit>b__207_4(Object obj, Int32 value)` -> `Void`
- method `<BadgeStatsPropInit>b__207_5()` -> `ICustomAttributeProvider`
- method `<BadgeStatsPropInit>b__207_6(Object obj)` -> `BadgeRarity`
- method `<BadgeStatsPropInit>b__207_7(Object obj, BadgeRarity value)` -> `Void`
- method `<BadgeStatsPropInit>b__207_8()` -> `ICustomAttributeProvider`
- method `<CardChoiceHistoryEntryPropInit>b__143_0(Object obj)` -> `SerializableCard`
- method `<CardChoiceHistoryEntryPropInit>b__143_1(Object obj, SerializableCard value)` -> `Void`
- method `<CardChoiceHistoryEntryPropInit>b__143_2()` -> `ICustomAttributeProvider`
- method `<CardChoiceHistoryEntryPropInit>b__143_3(Object obj)` -> `Boolean`
- method `<CardChoiceHistoryEntryPropInit>b__143_4(Object obj, Boolean value)` -> `Void`
- method `<CardChoiceHistoryEntryPropInit>b__143_5()` -> `ICustomAttributeProvider`
- method `<CardEnchantmentHistoryEntryPropInit>b__148_0(Object obj)` -> `SerializableCard`
- method `<CardEnchantmentHistoryEntryPropInit>b__148_1(Object obj, SerializableCard value)` -> `Void`
- method `<CardEnchantmentHistoryEntryPropInit>b__148_2()` -> `ICustomAttributeProvider`
- method `<CardEnchantmentHistoryEntryPropInit>b__148_3(Object obj)` -> `ModelId`
- method `<CardEnchantmentHistoryEntryPropInit>b__148_4(Object obj, ModelId value)` -> `Void`
- method `<CardEnchantmentHistoryEntryPropInit>b__148_5()` -> `ICustomAttributeProvider`
- method `<CardStatsPropInit>b__213_0(Object obj)` -> `ModelId`
- method `<CardStatsPropInit>b__213_1(Object obj, ModelId value)` -> `Void`
- method `<CardStatsPropInit>b__213_10(Object obj, Int64 value)` -> `Void`
- method `<CardStatsPropInit>b__213_11()` -> `ICustomAttributeProvider`
- method `<CardStatsPropInit>b__213_12(Object obj)` -> `Int64`
- method `<CardStatsPropInit>b__213_13(Object obj, Int64 value)` -> `Void`
- method `<CardStatsPropInit>b__213_14()` -> `ICustomAttributeProvider`
- method `<CardStatsPropInit>b__213_2()` -> `ICustomAttributeProvider`
- method `<CardStatsPropInit>b__213_3(Object obj)` -> `Int64`
- method `<CardStatsPropInit>b__213_4(Object obj, Int64 value)` -> `Void`
- method `<CardStatsPropInit>b__213_5()` -> `ICustomAttributeProvider`
- method `<CardStatsPropInit>b__213_6(Object obj)` -> `Int64`
- method `<CardStatsPropInit>b__213_7(Object obj, Int64 value)` -> `Void`
- method `<CardStatsPropInit>b__213_8()` -> `ICustomAttributeProvider`
- method `<CardStatsPropInit>b__213_9(Object obj)` -> `Int64`
- method `<CardTransformationHistoryEntryPropInit>b__153_0(Object obj)` -> `SerializableCard`
- method `<CardTransformationHistoryEntryPropInit>b__153_1(Object obj, SerializableCard value)` -> `Void`
- method `<CardTransformationHistoryEntryPropInit>b__153_2()` -> `ICustomAttributeProvider`
- method `<CardTransformationHistoryEntryPropInit>b__153_3(Object obj)` -> `SerializableCard`
- method `<CardTransformationHistoryEntryPropInit>b__153_4(Object obj, SerializableCard value)` -> `Void`
- method `<CardTransformationHistoryEntryPropInit>b__153_5()` -> `ICustomAttributeProvider`
- method `<CharacterStatsPropInit>b__219_0(Object obj)` -> `ModelId`
- method `<CharacterStatsPropInit>b__219_1(Object obj, ModelId value)` -> `Void`
- method `<CharacterStatsPropInit>b__219_10(Object obj, Int32 value)` -> `Void`
- method `<CharacterStatsPropInit>b__219_11()` -> `ICustomAttributeProvider`
- method `<CharacterStatsPropInit>b__219_12(Object obj)` -> `Int32`
- method `<CharacterStatsPropInit>b__219_13(Object obj, Int32 value)` -> `Void`
- method `<CharacterStatsPropInit>b__219_14()` -> `ICustomAttributeProvider`
- method `<CharacterStatsPropInit>b__219_15(Object obj)` -> `Int64`
- method `<CharacterStatsPropInit>b__219_16(Object obj, Int64 value)` -> `Void`
- method `<CharacterStatsPropInit>b__219_17()` -> `ICustomAttributeProvider`
- method `<CharacterStatsPropInit>b__219_18(Object obj)` -> `Int64`
- method `<CharacterStatsPropInit>b__219_19(Object obj, Int64 value)` -> `Void`
- method `<CharacterStatsPropInit>b__219_2()` -> `ICustomAttributeProvider`
- method `<CharacterStatsPropInit>b__219_20()` -> `ICustomAttributeProvider`
- method `<CharacterStatsPropInit>b__219_21(Object obj)` -> `Int64`
- method `<CharacterStatsPropInit>b__219_22(Object obj, Int64 value)` -> `Void`
- method `<CharacterStatsPropInit>b__219_23()` -> `ICustomAttributeProvider`
- method `<CharacterStatsPropInit>b__219_24(Object obj)` -> `Int64`
- method `<CharacterStatsPropInit>b__219_25(Object obj, Int64 value)` -> `Void`
- method `<CharacterStatsPropInit>b__219_26()` -> `ICustomAttributeProvider`
- method `<CharacterStatsPropInit>b__219_27(Object obj)` -> `List`1`
- method `<CharacterStatsPropInit>b__219_28(Object obj, List`1 value)` -> `Void`
- method `<CharacterStatsPropInit>b__219_29()` -> `ICustomAttributeProvider`
- method `<CharacterStatsPropInit>b__219_3(Object obj)` -> `Int32`
- method `<CharacterStatsPropInit>b__219_4(Object obj, Int32 value)` -> `Void`
- method `<CharacterStatsPropInit>b__219_5()` -> `ICustomAttributeProvider`
- method `<CharacterStatsPropInit>b__219_6(Object obj)` -> `Int32`
- method `<CharacterStatsPropInit>b__219_7(Object obj, Int32 value)` -> `Void`
- method `<CharacterStatsPropInit>b__219_8()` -> `ICustomAttributeProvider`
- method `<CharacterStatsPropInit>b__219_9(Object obj)` -> `Int32`
- method `<Create_AncientCharacterStats>b__194_0(Object[] args)` -> `AncientCharacterStats`
- method `<Create_AncientCharacterStats>b__194_2()` -> `ICustomAttributeProvider`
- method `<Create_AncientChoiceHistoryEntry>b__137_0()` -> `AncientChoiceHistoryEntry`
- method `<Create_AncientChoiceHistoryEntry>b__137_2()` -> `ICustomAttributeProvider`
- method `<Create_AncientStats>b__200_0(Object[] args)` -> `AncientStats`
- method `<Create_AncientStats>b__200_2()` -> `ICustomAttributeProvider`
- method `<Create_BadgeStats>b__206_0(Object[] args)` -> `BadgeStats`
- method `<Create_BadgeStats>b__206_2()` -> `ICustomAttributeProvider`
- method `<Create_CardChoiceHistoryEntry>b__142_0()` -> `CardChoiceHistoryEntry`
- method `<Create_CardChoiceHistoryEntry>b__142_2()` -> `ICustomAttributeProvider`
- method `<Create_CardEnchantmentHistoryEntry>b__147_0()` -> `CardEnchantmentHistoryEntry`
- method `<Create_CardEnchantmentHistoryEntry>b__147_2()` -> `ICustomAttributeProvider`
- method `<Create_CardStats>b__212_0(Object[] args)` -> `CardStats`
- method `<Create_CardStats>b__212_2()` -> `ICustomAttributeProvider`
- method `<Create_CardTransformationHistoryEntry>b__152_0()` -> `CardTransformationHistoryEntry`
- method `<Create_CardTransformationHistoryEntry>b__152_2()` -> `ICustomAttributeProvider`
- method `<Create_CharacterStats>b__218_0(Object[] args)` -> `CharacterStats`
- method `<Create_CharacterStats>b__218_2()` -> `ICustomAttributeProvider`
- method `<Create_DictionaryPlayerRngTypeInt32>b__470_0()` -> `Dictionary`2`
- method `<Create_DictionaryRelicRarityListModelId>b__466_0()` -> `Dictionary`2`
- method `<Create_DictionaryRunRngTypeInt32>b__474_0()` -> `Dictionary`2`
- method `<Create_DictionaryStringObject>b__478_0()` -> `Dictionary`2`
- method `<Create_DictionaryStringString>b__482_0()` -> `Dictionary`2`
- method `<Create_DictionaryUInt64ListSerializableReward>b__486_0()` -> `Dictionary`2`
- method `<Create_EncounterStats>b__224_0(Object[] args)` -> `EncounterStats`
- method `<Create_EncounterStats>b__224_2()` -> `ICustomAttributeProvider`
- method `<Create_EnemyStats>b__230_0(Object[] args)` -> `EnemyStats`
- method `<Create_EnemyStats>b__230_2()` -> `ICustomAttributeProvider`
- method `<Create_EventOptionHistoryEntry>b__157_0()` -> `EventOptionHistoryEntry`
- method `<Create_EventOptionHistoryEntry>b__157_2()` -> `ICustomAttributeProvider`
- method `<Create_FeedbackData>b__87_0()` -> `FeedbackData`
- method `<Create_FeedbackData>b__87_2()` -> `ICustomAttributeProvider`
- method `<Create_FightStats>b__240_0(Object[] args)` -> `FightStats`
- method `<Create_FightStats>b__240_2()` -> `ICustomAttributeProvider`
- method `<Create_ListAncientCharacterStats>b__574_0()` -> `List`1`
- method `<Create_ListAncientChoiceHistoryEntry>b__534_0()` -> `List`1`
- method `<Create_ListAncientStats>b__578_0()` -> `List`1`
- method `<Create_ListBadgeStats>b__582_0()` -> `List`1`
- method `<Create_ListCardChoiceHistoryEntry>b__538_0()` -> `List`1`
- method `<Create_ListCardEnchantmentHistoryEntry>b__542_0()` -> `List`1`
- method `<Create_ListCardStats>b__586_0()` -> `List`1`
- method `<Create_ListCardTransformationHistoryEntry>b__546_0()` -> `List`1`
- method `<Create_ListCharacterStats>b__590_0()` -> `List`1`
- method `<Create_ListDictionaryStringObject>b__686_0()` -> `List`1`
- method `<Create_ListEncounterStats>b__594_0()` -> `List`1`
- method `<Create_ListEnemyStats>b__598_0()` -> `List`1`
- method `<Create_ListEventOptionHistoryEntry>b__550_0()` -> `List`1`
- method `<Create_ListFightStats>b__602_0()` -> `List`1`
- method `<Create_ListJsonNode>b__698_0()` -> `List`1`
- method `<Create_ListListMapPointHistoryEntry>b__690_0()` -> `List`1`
- method `<Create_ListListPlayerMapPointHistoryEntry>b__694_0()` -> `List`1`
- method `<Create_ListMapCoord>b__510_0()` -> `List`1`
- method `<Create_ListMapPointHistoryEntry>b__554_0()` -> `List`1`
- method `<Create_ListMapPointRoomHistoryEntry>b__558_0()` -> `List`1`
- method `<Create_ListMigratingData>b__614_0()` -> `List`1`
- method `<Create_ListModelChoiceHistoryEntry>b__562_0()` -> `List`1`
- method `<Create_ListModelId>b__518_0()` -> `List`1`
- method `<Create_ListNullLeaderboard>b__522_0()` -> `List`1`
- method `<Create_ListNullLeaderboardFileEntry>b__526_0()` -> `List`1`
- method `<Create_ListNullMultiplayerName>b__530_0()` -> `List`1`
- method `<Create_ListPlayerMapPointHistoryEntry>b__566_0()` -> `List`1`
- method `<Create_ListRunHistoryPlayer>b__570_0()` -> `List`1`
- method `<Create_ListSavedPropertyBoolean>b__618_0()` -> `List`1`
- method `<Create_ListSavedPropertyInt32>b__638_0()` -> `List`1`
- method `<Create_ListSavedPropertyInt32Array>b__634_0()` -> `List`1`
- method `<Create_ListSavedPropertyModelId>b__622_0()` -> `List`1`
- method `<Create_ListSavedPropertySerializableCard>b__630_0()` -> `List`1`
- method `<Create_ListSavedPropertySerializableCardArray>b__626_0()` -> `List`1`
- method `<Create_ListSavedPropertyString>b__642_0()` -> `List`1`
- method `<Create_ListSerializableActModel>b__646_0()` -> `List`1`
- method `<Create_ListSerializableCard>b__650_0()` -> `List`1`
- method `<Create_ListSerializableEpoch>b__678_0()` -> `List`1`
- method `<Create_ListSerializableMapDrawingLine>b__606_0()` -> `List`1`
- method `<Create_ListSerializableMapPoint>b__654_0()` -> `List`1`
- method `<Create_ListSerializableModifier>b__658_0()` -> `List`1`
- method `<Create_ListSerializablePlayer>b__662_0()` -> `List`1`
- method `<Create_ListSerializablePlayerMapDrawings>b__610_0()` -> `List`1`
- method `<Create_ListSerializablePotion>b__666_0()` -> `List`1`
- method `<Create_ListSerializableRelic>b__670_0()` -> `List`1`
- method `<Create_ListSerializableReward>b__674_0()` -> `List`1`
- method `<Create_ListSerializableUnlockedAchievement>b__682_0()` -> `List`1`
- method `<Create_ListSettingsSaveMod>b__514_0()` -> `List`1`
- method `<Create_ListString>b__702_0()` -> `List`1`
- method `<Create_ListUInt64>b__706_0()` -> `List`1`
- method `<Create_ListVector2>b__506_0()` -> `List`1`
- method `<Create_LocString>b__43_0(Object[] args)` -> `LocString`
- method `<Create_LocString>b__43_2()` -> `ICustomAttributeProvider`
- method `<Create_MapCoord>b__49_0()` -> `MapCoord`
- method `<Create_MapCoord>b__49_2()` -> `ICustomAttributeProvider`
- method `<Create_MapPointHistoryEntry>b__162_0()` -> `MapPointHistoryEntry`
- method `<Create_MapPointHistoryEntry>b__162_2()` -> `ICustomAttributeProvider`
- method `<Create_MapPointRoomHistoryEntry>b__167_0()` -> `MapPointRoomHistoryEntry`
- method `<Create_MapPointRoomHistoryEntry>b__167_2()` -> `ICustomAttributeProvider`
- method `<Create_ModelChoiceHistoryEntry>b__172_0()` -> `ModelChoiceHistoryEntry`
- method `<Create_ModelChoiceHistoryEntry>b__172_2()` -> `ICustomAttributeProvider`
- method `<Create_ModelId>b__81_0(Object[] args)` -> `ModelId`
- method `<Create_ModelId>b__81_2()` -> `ICustomAttributeProvider`
- method `<Create_ModManifest>b__58_0()` -> `ModManifest`
- method `<Create_ModManifest>b__58_2()` -> `ICustomAttributeProvider`
- method `<Create_ModSettings>b__63_0()` -> `ModSettings`
- method `<Create_ModSettings>b__63_2()` -> `ICustomAttributeProvider`
- method `<Create_NullLeaderboard>b__92_0()` -> `NullLeaderboard`
- method `<Create_NullLeaderboard>b__92_2()` -> `ICustomAttributeProvider`
- method `<Create_NullLeaderboardFile>b__97_0()` -> `NullLeaderboardFile`
- method `<Create_NullLeaderboardFile>b__97_2()` -> `ICustomAttributeProvider`
- method `<Create_NullLeaderboardFileEntry>b__102_0(Object[] args)` -> `NullLeaderboardFileEntry`
- method `<Create_NullLeaderboardFileEntry>b__102_2()` -> `ICustomAttributeProvider`
- method `<Create_NullMultiplayerName>b__108_0()` -> `NullMultiplayerName`
- method `<Create_NullMultiplayerName>b__108_2()` -> `ICustomAttributeProvider`
- method `<Create_PlayerMapPointHistoryEntry>b__177_0()` -> `PlayerMapPointHistoryEntry`
- method `<Create_PlayerMapPointHistoryEntry>b__177_2()` -> `ICustomAttributeProvider`
- method `<Create_PrefsSave>b__266_0()` -> `PrefsSave`
- method `<Create_PrefsSave>b__266_2()` -> `ICustomAttributeProvider`
- method `<Create_ProfileSave>b__271_0()` -> `ProfileSave`
- method `<Create_ProfileSave>b__271_2()` -> `ICustomAttributeProvider`
- method `<Create_RunHistory>b__182_0(Object[] args)` -> `RunHistory`
- method `<Create_RunHistory>b__182_2()` -> `ICustomAttributeProvider`
- method `<Create_RunHistoryPlayer>b__188_0(Object[] args)` -> `RunHistoryPlayer`
- method `<Create_RunHistoryPlayer>b__188_2()` -> `ICustomAttributeProvider`
- method `<Create_SavedProperties>b__276_0()` -> `SavedProperties`
- method `<Create_SavedProperties>b__276_2()` -> `ICustomAttributeProvider`
- method `<Create_SavedPropertyBoolean>b__281_0()` -> `SavedProperty`1`
- method `<Create_SavedPropertyBoolean>b__281_2()` -> `ICustomAttributeProvider`
- method `<Create_SavedPropertyInt32>b__306_0()` -> `SavedProperty`1`
- method `<Create_SavedPropertyInt32>b__306_2()` -> `ICustomAttributeProvider`
- method `<Create_SavedPropertyInt32Array>b__301_0()` -> `SavedProperty`1`
- method `<Create_SavedPropertyInt32Array>b__301_2()` -> `ICustomAttributeProvider`
- method `<Create_SavedPropertyModelId>b__286_0()` -> `SavedProperty`1`
- method `<Create_SavedPropertyModelId>b__286_2()` -> `ICustomAttributeProvider`
- method `<Create_SavedPropertySerializableCard>b__296_0()` -> `SavedProperty`1`
- method `<Create_SavedPropertySerializableCard>b__296_2()` -> `ICustomAttributeProvider`
- method `<Create_SavedPropertySerializableCardArray>b__291_0()` -> `SavedProperty`1`
- method `<Create_SavedPropertySerializableCardArray>b__291_2()` -> `ICustomAttributeProvider`
- method `<Create_SavedPropertyString>b__311_0()` -> `SavedProperty`1`
- method `<Create_SavedPropertyString>b__311_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableActMap>b__316_0()` -> `SerializableActMap`
- method `<Create_SerializableActMap>b__316_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableActModel>b__321_0()` -> `SerializableActModel`
- method `<Create_SerializableActModel>b__321_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableBadge>b__326_0(Object[] args)` -> `SerializableBadge`
- method `<Create_SerializableBadge>b__326_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableCard>b__332_0()` -> `SerializableCard`
- method `<Create_SerializableCard>b__332_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableEnchantment>b__341_0()` -> `SerializableEnchantment`
- method `<Create_SerializableEnchantment>b__341_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableEpoch>b__411_0(Object[] args)` -> `SerializableEpoch`
- method `<Create_SerializableEpoch>b__411_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableExtraPlayerFields>b__417_0()` -> `SerializableExtraPlayerFields`
- method `<Create_SerializableExtraPlayerFields>b__417_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableExtraRunFields>b__346_0()` -> `SerializableExtraRunFields`
- method `<Create_SerializableExtraRunFields>b__346_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableMapDrawingLine>b__246_0()` -> `SerializableMapDrawingLine`
- method `<Create_SerializableMapDrawingLine>b__246_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableMapDrawings>b__251_0()` -> `SerializableMapDrawings`
- method `<Create_SerializableMapDrawings>b__251_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableMapPoint>b__351_0()` -> `SerializableMapPoint`
- method `<Create_SerializableMapPoint>b__351_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableModifier>b__356_0()` -> `SerializableModifier`
- method `<Create_SerializableModifier>b__356_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializablePlayer>b__361_0()` -> `SerializablePlayer`
- method `<Create_SerializablePlayer>b__361_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializablePlayerMapDrawings>b__256_0()` -> `SerializablePlayerMapDrawings`
- method `<Create_SerializablePlayerMapDrawings>b__256_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializablePlayerOddsSet>b__366_0()` -> `SerializablePlayerOddsSet`
- method `<Create_SerializablePlayerOddsSet>b__366_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializablePlayerRngSet>b__422_0()` -> `SerializablePlayerRngSet`
- method `<Create_SerializablePlayerRngSet>b__422_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializablePotion>b__371_0()` -> `SerializablePotion`
- method `<Create_SerializablePotion>b__371_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableProgress>b__427_0(Object[] args)` -> `SerializableProgress`
- method `<Create_SerializableProgress>b__427_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableRelic>b__376_0()` -> `SerializableRelic`
- method `<Create_SerializableRelic>b__376_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableRelicGrabBag>b__381_0()` -> `SerializableRelicGrabBag`
- method `<Create_SerializableRelicGrabBag>b__381_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableReward>b__386_0()` -> `SerializableReward`
- method `<Create_SerializableReward>b__386_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableRoom>b__391_0()` -> `SerializableRoom`
- method `<Create_SerializableRoom>b__391_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableRoomSet>b__396_0()` -> `SerializableRoomSet`
- method `<Create_SerializableRoomSet>b__396_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableRun>b__433_0()` -> `SerializableRun`
- method `<Create_SerializableRun>b__433_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableRunOddsSet>b__401_0()` -> `SerializableRunOddsSet`
- method `<Create_SerializableRunOddsSet>b__401_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableRunRngSet>b__406_0()` -> `SerializableRunRngSet`
- method `<Create_SerializableRunRngSet>b__406_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableUnlockedAchievement>b__438_0(Object[] args)` -> `SerializableUnlockedAchievement`
- method `<Create_SerializableUnlockedAchievement>b__438_2()` -> `ICustomAttributeProvider`
- method `<Create_SerializableUnlockState>b__461_0()` -> `SerializableUnlockState`
- method `<Create_SerializableUnlockState>b__461_2()` -> `ICustomAttributeProvider`
- method `<Create_SettingsSave>b__444_0()` -> `SettingsSave`
- method `<Create_SettingsSave>b__444_2()` -> `ICustomAttributeProvider`
- method `<Create_SettingsSaveMod>b__72_0()` -> `SettingsSaveMod`
- method `<Create_SettingsSaveMod>b__72_2()` -> `ICustomAttributeProvider`
- method `<Create_Vector2>b__17_0()` -> `Vector2`
- method `<Create_Vector2>b__17_2()` -> `ICustomAttributeProvider`
- method `<Create_Vector2I>b__22_0()` -> `Vector2I`
- method `<Create_Vector2I>b__22_2()` -> `ICustomAttributeProvider`
- method `<EncounterStatsPropInit>b__225_0(Object obj)` -> `ModelId`
- method `<EncounterStatsPropInit>b__225_1(Object obj, ModelId value)` -> `Void`
- method `<EncounterStatsPropInit>b__225_2()` -> `ICustomAttributeProvider`
- method `<EncounterStatsPropInit>b__225_3(Object obj)` -> `List`1`
- method `<EncounterStatsPropInit>b__225_4(Object obj, List`1 value)` -> `Void`
- method `<EncounterStatsPropInit>b__225_5()` -> `ICustomAttributeProvider`
- method `<EncounterStatsPropInit>b__225_6()` -> `ICustomAttributeProvider`
- method `<EncounterStatsPropInit>b__225_7()` -> `ICustomAttributeProvider`
- method `<EnemyStatsPropInit>b__231_0(Object obj)` -> `ModelId`
- method `<EnemyStatsPropInit>b__231_1(Object obj, ModelId value)` -> `Void`
- method `<EnemyStatsPropInit>b__231_2()` -> `ICustomAttributeProvider`
- method `<EnemyStatsPropInit>b__231_3(Object obj)` -> `List`1`
- method `<EnemyStatsPropInit>b__231_4(Object obj, List`1 value)` -> `Void`
- method `<EnemyStatsPropInit>b__231_5()` -> `ICustomAttributeProvider`
- method `<EnemyStatsPropInit>b__231_6()` -> `ICustomAttributeProvider`
- method `<EnemyStatsPropInit>b__231_7()` -> `ICustomAttributeProvider`
- method `<EventOptionHistoryEntryPropInit>b__158_0(Object obj)` -> `LocString`
- method `<EventOptionHistoryEntryPropInit>b__158_1(Object obj, LocString value)` -> `Void`
- method `<EventOptionHistoryEntryPropInit>b__158_2()` -> `ICustomAttributeProvider`
- method `<EventOptionHistoryEntryPropInit>b__158_3(Object obj)` -> `Dictionary`2`
- method `<EventOptionHistoryEntryPropInit>b__158_4(Object obj, Dictionary`2 value)` -> `Void`
- method `<EventOptionHistoryEntryPropInit>b__158_5()` -> `ICustomAttributeProvider`
- method `<FeedbackDataPropInit>b__88_0(Object obj)` -> `String`
- method `<FeedbackDataPropInit>b__88_1(Object obj, String value)` -> `Void`
- method `<FeedbackDataPropInit>b__88_10(Object obj, String value)` -> `Void`
- method `<FeedbackDataPropInit>b__88_11()` -> `ICustomAttributeProvider`
- method `<FeedbackDataPropInit>b__88_12(Object obj)` -> `String`
- method `<FeedbackDataPropInit>b__88_13(Object obj, String value)` -> `Void`
- method `<FeedbackDataPropInit>b__88_14()` -> `ICustomAttributeProvider`
- method `<FeedbackDataPropInit>b__88_15(Object obj)` -> `String`
- method `<FeedbackDataPropInit>b__88_16(Object obj, String value)` -> `Void`
- method `<FeedbackDataPropInit>b__88_17()` -> `ICustomAttributeProvider`
- method `<FeedbackDataPropInit>b__88_18(Object obj)` -> `String`
- method `<FeedbackDataPropInit>b__88_19(Object obj, String value)` -> `Void`
- method `<FeedbackDataPropInit>b__88_2()` -> `ICustomAttributeProvider`
- method `<FeedbackDataPropInit>b__88_20()` -> `ICustomAttributeProvider`
- method `<FeedbackDataPropInit>b__88_21(Object obj)` -> `Boolean`
- method `<FeedbackDataPropInit>b__88_22(Object obj, Boolean value)` -> `Void`
- method `<FeedbackDataPropInit>b__88_23()` -> `ICustomAttributeProvider`
- method `<FeedbackDataPropInit>b__88_24(Object obj)` -> `Boolean`
- method `<FeedbackDataPropInit>b__88_25(Object obj, Boolean value)` -> `Void`
- method `<FeedbackDataPropInit>b__88_26()` -> `ICustomAttributeProvider`
- method `<FeedbackDataPropInit>b__88_3(Object obj)` -> `String`
- method `<FeedbackDataPropInit>b__88_4(Object obj, String value)` -> `Void`
- method `<FeedbackDataPropInit>b__88_5()` -> `ICustomAttributeProvider`
- method `<FeedbackDataPropInit>b__88_6(Object obj)` -> `String`
- method `<FeedbackDataPropInit>b__88_7(Object obj, String value)` -> `Void`
- method `<FeedbackDataPropInit>b__88_8()` -> `ICustomAttributeProvider`
- method `<FeedbackDataPropInit>b__88_9(Object obj)` -> `String`
- method `<FightStatsPropInit>b__241_0(Object obj)` -> `ModelId`
- method `<FightStatsPropInit>b__241_1(Object obj, ModelId value)` -> `Void`
- method `<FightStatsPropInit>b__241_2()` -> `ICustomAttributeProvider`
- method `<FightStatsPropInit>b__241_3(Object obj)` -> `Int32`
- method `<FightStatsPropInit>b__241_4(Object obj, Int32 value)` -> `Void`
- method `<FightStatsPropInit>b__241_5()` -> `ICustomAttributeProvider`
- method `<FightStatsPropInit>b__241_6(Object obj)` -> `Int32`
- method `<FightStatsPropInit>b__241_7(Object obj, Int32 value)` -> `Void`
- method `<FightStatsPropInit>b__241_8()` -> `ICustomAttributeProvider`
- method `<LocStringPropInit>b__44_0(Object obj)` -> `String`
- method `<LocStringPropInit>b__44_1()` -> `ICustomAttributeProvider`
- method `<LocStringPropInit>b__44_2(Object obj)` -> `String`
- method `<LocStringPropInit>b__44_3()` -> `ICustomAttributeProvider`
- method `<LocStringPropInit>b__44_4()` -> `ICustomAttributeProvider`
- method `<LocStringPropInit>b__44_5()` -> `ICustomAttributeProvider`
- method `<MapCoordPropInit>b__50_0(Object obj)` -> `Int32`
- method `<MapCoordPropInit>b__50_1(Object obj, Int32 value)` -> `Void`
- method `<MapCoordPropInit>b__50_2()` -> `ICustomAttributeProvider`
- method `<MapCoordPropInit>b__50_3(Object obj)` -> `Int32`
- method `<MapCoordPropInit>b__50_4(Object obj, Int32 value)` -> `Void`
- method `<MapCoordPropInit>b__50_5()` -> `ICustomAttributeProvider`
- method `<MapPointHistoryEntryPropInit>b__163_0(Object obj)` -> `MapPointType`
- method `<MapPointHistoryEntryPropInit>b__163_1(Object obj, MapPointType value)` -> `Void`
- method `<MapPointHistoryEntryPropInit>b__163_2()` -> `ICustomAttributeProvider`
- method `<MapPointHistoryEntryPropInit>b__163_3(Object obj)` -> `List`1`
- method `<MapPointHistoryEntryPropInit>b__163_4(Object obj, List`1 value)` -> `Void`
- method `<MapPointHistoryEntryPropInit>b__163_5()` -> `ICustomAttributeProvider`
- method `<MapPointHistoryEntryPropInit>b__163_6(Object obj)` -> `List`1`
- method `<MapPointHistoryEntryPropInit>b__163_7(Object obj, List`1 value)` -> `Void`
- method `<MapPointHistoryEntryPropInit>b__163_8()` -> `ICustomAttributeProvider`
- method `<MapPointRoomHistoryEntryPropInit>b__168_0(Object obj)` -> `RoomType`
- method `<MapPointRoomHistoryEntryPropInit>b__168_1(Object obj, RoomType value)` -> `Void`
- method `<MapPointRoomHistoryEntryPropInit>b__168_10(Object obj, Int32 value)` -> `Void`
- method `<MapPointRoomHistoryEntryPropInit>b__168_11()` -> `ICustomAttributeProvider`
- method `<MapPointRoomHistoryEntryPropInit>b__168_2()` -> `ICustomAttributeProvider`
- method `<MapPointRoomHistoryEntryPropInit>b__168_3(Object obj)` -> `ModelId`
- method `<MapPointRoomHistoryEntryPropInit>b__168_4(Object obj, ModelId value)` -> `Void`
- method `<MapPointRoomHistoryEntryPropInit>b__168_5()` -> `ICustomAttributeProvider`
- method `<MapPointRoomHistoryEntryPropInit>b__168_6(Object obj)` -> `List`1`
- method `<MapPointRoomHistoryEntryPropInit>b__168_7(Object obj, List`1 value)` -> `Void`
- method `<MapPointRoomHistoryEntryPropInit>b__168_8()` -> `ICustomAttributeProvider`
- method `<MapPointRoomHistoryEntryPropInit>b__168_9(Object obj)` -> `Int32`
- method `<ModelChoiceHistoryEntryPropInit>b__173_0(Object obj)` -> `ModelId`
- method `<ModelChoiceHistoryEntryPropInit>b__173_1(Object obj, ModelId value)` -> `Void`
- method `<ModelChoiceHistoryEntryPropInit>b__173_2()` -> `ICustomAttributeProvider`
- method `<ModelChoiceHistoryEntryPropInit>b__173_3(Object obj)` -> `Boolean`
- method `<ModelChoiceHistoryEntryPropInit>b__173_4(Object obj, Boolean value)` -> `Void`
- method `<ModelChoiceHistoryEntryPropInit>b__173_5()` -> `ICustomAttributeProvider`
- method `<ModelIdPropInit>b__82_0(Object obj)` -> `String`
- method `<ModelIdPropInit>b__82_1()` -> `ICustomAttributeProvider`
- method `<ModelIdPropInit>b__82_2(Object obj)` -> `String`
- method `<ModelIdPropInit>b__82_3()` -> `ICustomAttributeProvider`
- method `<ModManifestPropInit>b__59_0(Object obj)` -> `String`
- method `<ModManifestPropInit>b__59_1(Object obj, String value)` -> `Void`
- method `<ModManifestPropInit>b__59_10(Object obj, String value)` -> `Void`
- method `<ModManifestPropInit>b__59_11()` -> `ICustomAttributeProvider`
- method `<ModManifestPropInit>b__59_12(Object obj)` -> `String`
- method `<ModManifestPropInit>b__59_13(Object obj, String value)` -> `Void`
- method `<ModManifestPropInit>b__59_14()` -> `ICustomAttributeProvider`
- method `<ModManifestPropInit>b__59_15(Object obj)` -> `Boolean`
- method `<ModManifestPropInit>b__59_16(Object obj, Boolean value)` -> `Void`
- method `<ModManifestPropInit>b__59_17()` -> `ICustomAttributeProvider`
- method `<ModManifestPropInit>b__59_18(Object obj)` -> `Boolean`
- method `<ModManifestPropInit>b__59_19(Object obj, Boolean value)` -> `Void`
- method `<ModManifestPropInit>b__59_2()` -> `ICustomAttributeProvider`
- method `<ModManifestPropInit>b__59_20()` -> `ICustomAttributeProvider`
- method `<ModManifestPropInit>b__59_21(Object obj)` -> `List`1`
- method `<ModManifestPropInit>b__59_22(Object obj, List`1 value)` -> `Void`
- method `<ModManifestPropInit>b__59_23()` -> `ICustomAttributeProvider`
- method `<ModManifestPropInit>b__59_24(Object obj)` -> `Boolean`
- method `<ModManifestPropInit>b__59_25(Object obj, Boolean value)` -> `Void`
- method `<ModManifestPropInit>b__59_26()` -> `ICustomAttributeProvider`
- method `<ModManifestPropInit>b__59_3(Object obj)` -> `String`
- method `<ModManifestPropInit>b__59_4(Object obj, String value)` -> `Void`
- method `<ModManifestPropInit>b__59_5()` -> `ICustomAttributeProvider`
- method `<ModManifestPropInit>b__59_6(Object obj)` -> `String`
- method `<ModManifestPropInit>b__59_7(Object obj, String value)` -> `Void`
- method `<ModManifestPropInit>b__59_8()` -> `ICustomAttributeProvider`
- method `<ModManifestPropInit>b__59_9(Object obj)` -> `String`
- method `<ModSettingsPropInit>b__64_0(Object obj)` -> `Boolean`
- method `<ModSettingsPropInit>b__64_1(Object obj, Boolean value)` -> `Void`
- method `<ModSettingsPropInit>b__64_2()` -> `ICustomAttributeProvider`
- method `<ModSettingsPropInit>b__64_3(Object obj)` -> `List`1`
- method `<ModSettingsPropInit>b__64_4(Object obj, List`1 value)` -> `Void`
- method `<ModSettingsPropInit>b__64_5()` -> `ICustomAttributeProvider`
- method `<NullLeaderboardFileEntryPropInit>b__103_0(Object obj)` -> `String`
- method `<NullLeaderboardFileEntryPropInit>b__103_1(Object obj, String value)` -> `Void`
- method `<NullLeaderboardFileEntryPropInit>b__103_10(Object obj, List`1 value)` -> `Void`
- method `<NullLeaderboardFileEntryPropInit>b__103_11()` -> `ICustomAttributeProvider`
- method `<NullLeaderboardFileEntryPropInit>b__103_2()` -> `ICustomAttributeProvider`
- method `<NullLeaderboardFileEntryPropInit>b__103_3(Object obj)` -> `Int32`
- method `<NullLeaderboardFileEntryPropInit>b__103_4(Object obj, Int32 value)` -> `Void`
- method `<NullLeaderboardFileEntryPropInit>b__103_5()` -> `ICustomAttributeProvider`
- method `<NullLeaderboardFileEntryPropInit>b__103_6(Object obj)` -> `UInt64`
- method `<NullLeaderboardFileEntryPropInit>b__103_7(Object obj, UInt64 value)` -> `Void`
- method `<NullLeaderboardFileEntryPropInit>b__103_8()` -> `ICustomAttributeProvider`
- method `<NullLeaderboardFileEntryPropInit>b__103_9(Object obj)` -> `List`1`
- method `<NullLeaderboardFilePropInit>b__98_0(Object obj)` -> `Int32`
- method `<NullLeaderboardFilePropInit>b__98_1(Object obj, Int32 value)` -> `Void`
- method `<NullLeaderboardFilePropInit>b__98_2()` -> `ICustomAttributeProvider`
- method `<NullLeaderboardFilePropInit>b__98_3(Object obj)` -> `List`1`
- method `<NullLeaderboardFilePropInit>b__98_4(Object obj, List`1 value)` -> `Void`
- method `<NullLeaderboardFilePropInit>b__98_5()` -> `ICustomAttributeProvider`
- method `<NullLeaderboardPropInit>b__93_0(Object obj)` -> `String`
- method `<NullLeaderboardPropInit>b__93_1(Object obj, String value)` -> `Void`
- method `<NullLeaderboardPropInit>b__93_2()` -> `ICustomAttributeProvider`
- method `<NullLeaderboardPropInit>b__93_3(Object obj)` -> `List`1`
- method `<NullLeaderboardPropInit>b__93_4(Object obj, List`1 value)` -> `Void`
- method `<NullLeaderboardPropInit>b__93_5()` -> `ICustomAttributeProvider`
- method `<NullMultiplayerNamePropInit>b__109_0(Object obj)` -> `UInt64`
- method `<NullMultiplayerNamePropInit>b__109_1(Object obj, UInt64 value)` -> `Void`
- method `<NullMultiplayerNamePropInit>b__109_2()` -> `ICustomAttributeProvider`
- method `<NullMultiplayerNamePropInit>b__109_3(Object obj)` -> `String`
- method `<NullMultiplayerNamePropInit>b__109_4(Object obj, String value)` -> `Void`
- method `<NullMultiplayerNamePropInit>b__109_5()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_0(Object obj)` -> `UInt64`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_1(Object obj, UInt64 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_10(Object obj, Int32 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_11()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_12(Object obj)` -> `Int32`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_13(Object obj, Int32 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_14()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_15(Object obj)` -> `Int32`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_16(Object obj, Int32 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_17()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_18(Object obj)` -> `Int32`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_19(Object obj, Int32 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_2()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_20()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_21(Object obj)` -> `Int32`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_22(Object obj, Int32 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_23()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_24(Object obj)` -> `Int32`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_25(Object obj, Int32 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_26()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_27(Object obj)` -> `Int32`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_28(Object obj, Int32 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_29()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_3(Object obj)` -> `Int32`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_30(Object obj)` -> `Int32`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_31(Object obj, Int32 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_32()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_33(Object obj)` -> `Int32`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_34(Object obj, Int32 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_35()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_36(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_37(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_38()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_39(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_4(Object obj, Int32 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_40(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_41()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_42(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_43(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_44()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_45(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_46(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_47()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_48(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_49(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_5()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_50()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_51(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_52(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_53()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_54(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_55(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_56()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_57(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_58(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_59()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_6(Object obj)` -> `Int32`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_60(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_61(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_62()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_63(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_64(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_65()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_66(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_67(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_68()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_69(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_7(Object obj, Int32 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_70(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_71()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_72(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_73(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_74()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_75(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_76(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_77()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_78(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_79(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_8()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_80()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_81(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_82(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_83()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_84(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_85(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_86()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_87(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_88(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_89()` -> `ICustomAttributeProvider`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_9(Object obj)` -> `Int32`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_90(Object obj)` -> `List`1`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_91(Object obj, List`1 value)` -> `Void`
- method `<PlayerMapPointHistoryEntryPropInit>b__178_92()` -> `ICustomAttributeProvider`
- method `<PrefsSavePropInit>b__267_0(Object obj)` -> `Int32`
- method `<PrefsSavePropInit>b__267_1(Object obj, Int32 value)` -> `Void`
- method `<PrefsSavePropInit>b__267_10(Object obj, Int32 value)` -> `Void`
- method `<PrefsSavePropInit>b__267_11()` -> `ICustomAttributeProvider`
- method `<PrefsSavePropInit>b__267_12(Object obj)` -> `Boolean`
- method `<PrefsSavePropInit>b__267_13(Object obj, Boolean value)` -> `Void`
- method `<PrefsSavePropInit>b__267_14()` -> `ICustomAttributeProvider`
- method `<PrefsSavePropInit>b__267_15(Object obj)` -> `Boolean`
- method `<PrefsSavePropInit>b__267_16(Object obj, Boolean value)` -> `Void`
- method `<PrefsSavePropInit>b__267_17()` -> `ICustomAttributeProvider`
- method `<PrefsSavePropInit>b__267_18(Object obj)` -> `Boolean`
- method `<PrefsSavePropInit>b__267_19(Object obj, Boolean value)` -> `Void`
- method `<PrefsSavePropInit>b__267_2()` -> `ICustomAttributeProvider`
- method `<PrefsSavePropInit>b__267_20()` -> `ICustomAttributeProvider`
- method `<PrefsSavePropInit>b__267_21(Object obj)` -> `Boolean`
- method `<PrefsSavePropInit>b__267_22(Object obj, Boolean value)` -> `Void`
- method `<PrefsSavePropInit>b__267_23()` -> `ICustomAttributeProvider`
- method `<PrefsSavePropInit>b__267_24(Object obj)` -> `Boolean`
- method `<PrefsSavePropInit>b__267_25(Object obj, Boolean value)` -> `Void`
- method `<PrefsSavePropInit>b__267_26()` -> `ICustomAttributeProvider`
- method `<PrefsSavePropInit>b__267_27(Object obj)` -> `Boolean`
- method `<PrefsSavePropInit>b__267_28(Object obj, Boolean value)` -> `Void`
- method `<PrefsSavePropInit>b__267_29()` -> `ICustomAttributeProvider`
- method `<PrefsSavePropInit>b__267_3(Object obj)` -> `FastModeType`
- method `<PrefsSavePropInit>b__267_30(Object obj)` -> `Boolean`
- method `<PrefsSavePropInit>b__267_31(Object obj, Boolean value)` -> `Void`
- method `<PrefsSavePropInit>b__267_32()` -> `ICustomAttributeProvider`
- method `<PrefsSavePropInit>b__267_4(Object obj, FastModeType value)` -> `Void`
- method `<PrefsSavePropInit>b__267_5()` -> `ICustomAttributeProvider`
- method `<PrefsSavePropInit>b__267_6(Object obj)` -> `Boolean`
- method `<PrefsSavePropInit>b__267_7(Object obj, Boolean value)` -> `Void`
- method `<PrefsSavePropInit>b__267_8()` -> `ICustomAttributeProvider`
- method `<PrefsSavePropInit>b__267_9(Object obj)` -> `Int32`
- method `<ProfileSavePropInit>b__272_0(Object obj)` -> `Int32`
- method `<ProfileSavePropInit>b__272_1(Object obj, Int32 value)` -> `Void`
- method `<ProfileSavePropInit>b__272_2()` -> `ICustomAttributeProvider`
- method `<ProfileSavePropInit>b__272_3(Object obj)` -> `Int32`
- method `<ProfileSavePropInit>b__272_4(Object obj, Int32 value)` -> `Void`
- method `<ProfileSavePropInit>b__272_5()` -> `ICustomAttributeProvider`
- method `<RunHistoryPlayerPropInit>b__189_0(Object obj)` -> `UInt64`
- method `<RunHistoryPlayerPropInit>b__189_1(Object obj, UInt64 value)` -> `Void`
- method `<RunHistoryPlayerPropInit>b__189_10(Object obj, IEnumerable`1 value)` -> `Void`
- method `<RunHistoryPlayerPropInit>b__189_11()` -> `ICustomAttributeProvider`
- method `<RunHistoryPlayerPropInit>b__189_12(Object obj)` -> `IEnumerable`1`
- method `<RunHistoryPlayerPropInit>b__189_13(Object obj, IEnumerable`1 value)` -> `Void`
- method `<RunHistoryPlayerPropInit>b__189_14()` -> `ICustomAttributeProvider`
- method `<RunHistoryPlayerPropInit>b__189_15(Object obj)` -> `IEnumerable`1`
- method `<RunHistoryPlayerPropInit>b__189_16(Object obj, IEnumerable`1 value)` -> `Void`
- method `<RunHistoryPlayerPropInit>b__189_17()` -> `ICustomAttributeProvider`
- method `<RunHistoryPlayerPropInit>b__189_18(Object obj)` -> `Int32`
- method `<RunHistoryPlayerPropInit>b__189_19(Object obj, Int32 value)` -> `Void`
- method `<RunHistoryPlayerPropInit>b__189_2()` -> `ICustomAttributeProvider`
- method `<RunHistoryPlayerPropInit>b__189_20()` -> `ICustomAttributeProvider`
- method `<RunHistoryPlayerPropInit>b__189_3(Object obj)` -> `ModelId`
- method `<RunHistoryPlayerPropInit>b__189_4(Object obj, ModelId value)` -> `Void`
- method `<RunHistoryPlayerPropInit>b__189_5()` -> `ICustomAttributeProvider`
- method `<RunHistoryPlayerPropInit>b__189_6(Object obj)` -> `IEnumerable`1`
- method `<RunHistoryPlayerPropInit>b__189_7(Object obj, IEnumerable`1 value)` -> `Void`
- method `<RunHistoryPlayerPropInit>b__189_8()` -> `ICustomAttributeProvider`
- method `<RunHistoryPlayerPropInit>b__189_9(Object obj)` -> `IEnumerable`1`
- method `<RunHistoryPropInit>b__183_0(Object obj)` -> `Int32`
- method `<RunHistoryPropInit>b__183_1(Object obj, Int32 value)` -> `Void`
- method `<RunHistoryPropInit>b__183_10(Object obj, Boolean value)` -> `Void`
- method `<RunHistoryPropInit>b__183_11()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_12(Object obj)` -> `String`
- method `<RunHistoryPropInit>b__183_13(Object obj, String value)` -> `Void`
- method `<RunHistoryPropInit>b__183_14()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_15(Object obj)` -> `Int64`
- method `<RunHistoryPropInit>b__183_16(Object obj, Int64 value)` -> `Void`
- method `<RunHistoryPropInit>b__183_17()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_18(Object obj)` -> `Single`
- method `<RunHistoryPropInit>b__183_19(Object obj, Single value)` -> `Void`
- method `<RunHistoryPropInit>b__183_2()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_20()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_21(Object obj)` -> `Int32`
- method `<RunHistoryPropInit>b__183_22(Object obj, Int32 value)` -> `Void`
- method `<RunHistoryPropInit>b__183_23()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_24(Object obj)` -> `String`
- method `<RunHistoryPropInit>b__183_25(Object obj, String value)` -> `Void`
- method `<RunHistoryPropInit>b__183_26()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_27(Object obj)` -> `Boolean`
- method `<RunHistoryPropInit>b__183_28(Object obj, Boolean value)` -> `Void`
- method `<RunHistoryPropInit>b__183_29()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_3(Object obj)` -> `PlatformType`
- method `<RunHistoryPropInit>b__183_30(Object obj)` -> `ModelId`
- method `<RunHistoryPropInit>b__183_31(Object obj, ModelId value)` -> `Void`
- method `<RunHistoryPropInit>b__183_32()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_33(Object obj)` -> `ModelId`
- method `<RunHistoryPropInit>b__183_34(Object obj, ModelId value)` -> `Void`
- method `<RunHistoryPropInit>b__183_35()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_36(Object obj)` -> `List`1`
- method `<RunHistoryPropInit>b__183_37(Object obj, List`1 value)` -> `Void`
- method `<RunHistoryPropInit>b__183_38()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_39(Object obj)` -> `List`1`
- method `<RunHistoryPropInit>b__183_4(Object obj, PlatformType value)` -> `Void`
- method `<RunHistoryPropInit>b__183_40(Object obj, List`1 value)` -> `Void`
- method `<RunHistoryPropInit>b__183_41()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_42(Object obj)` -> `List`1`
- method `<RunHistoryPropInit>b__183_43(Object obj, List`1 value)` -> `Void`
- method `<RunHistoryPropInit>b__183_44()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_45(Object obj)` -> `List`1`
- method `<RunHistoryPropInit>b__183_46(Object obj, List`1 value)` -> `Void`
- method `<RunHistoryPropInit>b__183_47()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_5()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_6(Object obj)` -> `GameMode`
- method `<RunHistoryPropInit>b__183_7(Object obj, GameMode value)` -> `Void`
- method `<RunHistoryPropInit>b__183_8()` -> `ICustomAttributeProvider`
- method `<RunHistoryPropInit>b__183_9(Object obj)` -> `Boolean`
- method `<SavedPropertiesPropInit>b__277_0(Object obj)` -> `List`1`
- method `<SavedPropertiesPropInit>b__277_1(Object obj, List`1 value)` -> `Void`
- method `<SavedPropertiesPropInit>b__277_10(Object obj, List`1 value)` -> `Void`
- method `<SavedPropertiesPropInit>b__277_11()` -> `ICustomAttributeProvider`
- method `<SavedPropertiesPropInit>b__277_12(Object obj)` -> `List`1`
- method `<SavedPropertiesPropInit>b__277_13(Object obj, List`1 value)` -> `Void`
- method `<SavedPropertiesPropInit>b__277_14()` -> `ICustomAttributeProvider`
- method `<SavedPropertiesPropInit>b__277_15(Object obj)` -> `List`1`
- method `<SavedPropertiesPropInit>b__277_16(Object obj, List`1 value)` -> `Void`
- method `<SavedPropertiesPropInit>b__277_17()` -> `ICustomAttributeProvider`
- method `<SavedPropertiesPropInit>b__277_18(Object obj)` -> `List`1`
- method `<SavedPropertiesPropInit>b__277_19(Object obj, List`1 value)` -> `Void`
- method `<SavedPropertiesPropInit>b__277_2()` -> `ICustomAttributeProvider`
- method `<SavedPropertiesPropInit>b__277_20()` -> `ICustomAttributeProvider`
- method `<SavedPropertiesPropInit>b__277_3(Object obj)` -> `List`1`
- method `<SavedPropertiesPropInit>b__277_4(Object obj, List`1 value)` -> `Void`
- method `<SavedPropertiesPropInit>b__277_5()` -> `ICustomAttributeProvider`
- method `<SavedPropertiesPropInit>b__277_6(Object obj)` -> `List`1`
- method `<SavedPropertiesPropInit>b__277_7(Object obj, List`1 value)` -> `Void`
- method `<SavedPropertiesPropInit>b__277_8()` -> `ICustomAttributeProvider`
- method `<SavedPropertiesPropInit>b__277_9(Object obj)` -> `List`1`
- method `<SavedPropertyBooleanPropInit>b__282_0(Object obj)` -> `String`
- method `<SavedPropertyBooleanPropInit>b__282_1(Object obj, String value)` -> `Void`
- method `<SavedPropertyBooleanPropInit>b__282_2()` -> `ICustomAttributeProvider`
- method `<SavedPropertyBooleanPropInit>b__282_3(Object obj)` -> `Boolean`
- method `<SavedPropertyBooleanPropInit>b__282_4(Object obj, Boolean value)` -> `Void`
- method `<SavedPropertyBooleanPropInit>b__282_5()` -> `ICustomAttributeProvider`
- method `<SavedPropertyInt32ArrayPropInit>b__302_0(Object obj)` -> `String`
- method `<SavedPropertyInt32ArrayPropInit>b__302_1(Object obj, String value)` -> `Void`
- method `<SavedPropertyInt32ArrayPropInit>b__302_2()` -> `ICustomAttributeProvider`
- method `<SavedPropertyInt32ArrayPropInit>b__302_3(Object obj)` -> `Int32[]`
- method `<SavedPropertyInt32ArrayPropInit>b__302_4(Object obj, Int32[] value)` -> `Void`
- method `<SavedPropertyInt32ArrayPropInit>b__302_5()` -> `ICustomAttributeProvider`
- method `<SavedPropertyInt32PropInit>b__307_0(Object obj)` -> `String`
- method `<SavedPropertyInt32PropInit>b__307_1(Object obj, String value)` -> `Void`
- method `<SavedPropertyInt32PropInit>b__307_2()` -> `ICustomAttributeProvider`
- method `<SavedPropertyInt32PropInit>b__307_3(Object obj)` -> `Int32`
- method `<SavedPropertyInt32PropInit>b__307_4(Object obj, Int32 value)` -> `Void`
- method `<SavedPropertyInt32PropInit>b__307_5()` -> `ICustomAttributeProvider`
- method `<SavedPropertyModelIdPropInit>b__287_0(Object obj)` -> `String`
- method `<SavedPropertyModelIdPropInit>b__287_1(Object obj, String value)` -> `Void`
- method `<SavedPropertyModelIdPropInit>b__287_2()` -> `ICustomAttributeProvider`
- method `<SavedPropertyModelIdPropInit>b__287_3(Object obj)` -> `ModelId`
- method `<SavedPropertyModelIdPropInit>b__287_4(Object obj, ModelId value)` -> `Void`
- method `<SavedPropertyModelIdPropInit>b__287_5()` -> `ICustomAttributeProvider`
- method `<SavedPropertySerializableCardArrayPropInit>b__292_0(Object obj)` -> `String`
- method `<SavedPropertySerializableCardArrayPropInit>b__292_1(Object obj, String value)` -> `Void`
- method `<SavedPropertySerializableCardArrayPropInit>b__292_2()` -> `ICustomAttributeProvider`
- method `<SavedPropertySerializableCardArrayPropInit>b__292_3(Object obj)` -> `SerializableCard[]`
- method `<SavedPropertySerializableCardArrayPropInit>b__292_4(Object obj, SerializableCard[] value)` -> `Void`
- method `<SavedPropertySerializableCardArrayPropInit>b__292_5()` -> `ICustomAttributeProvider`
- method `<SavedPropertySerializableCardPropInit>b__297_0(Object obj)` -> `String`
- method `<SavedPropertySerializableCardPropInit>b__297_1(Object obj, String value)` -> `Void`
- method `<SavedPropertySerializableCardPropInit>b__297_2()` -> `ICustomAttributeProvider`
- method `<SavedPropertySerializableCardPropInit>b__297_3(Object obj)` -> `SerializableCard`
- method `<SavedPropertySerializableCardPropInit>b__297_4(Object obj, SerializableCard value)` -> `Void`
- method `<SavedPropertySerializableCardPropInit>b__297_5()` -> `ICustomAttributeProvider`
- method `<SavedPropertyStringPropInit>b__312_0(Object obj)` -> `String`
- method `<SavedPropertyStringPropInit>b__312_1(Object obj, String value)` -> `Void`
- method `<SavedPropertyStringPropInit>b__312_2()` -> `ICustomAttributeProvider`
- method `<SavedPropertyStringPropInit>b__312_3(Object obj)` -> `String`
- method `<SavedPropertyStringPropInit>b__312_4(Object obj, String value)` -> `Void`
- method `<SavedPropertyStringPropInit>b__312_5()` -> `ICustomAttributeProvider`
- method `<SerializableActMapPropInit>b__317_0(Object obj)` -> `List`1`
- method `<SerializableActMapPropInit>b__317_1(Object obj, List`1 value)` -> `Void`
- method `<SerializableActMapPropInit>b__317_10(Object obj, SerializableMapPoint value)` -> `Void`
- method `<SerializableActMapPropInit>b__317_11()` -> `ICustomAttributeProvider`
- method `<SerializableActMapPropInit>b__317_12(Object obj)` -> `List`1`
- method `<SerializableActMapPropInit>b__317_13(Object obj, List`1 value)` -> `Void`
- method `<SerializableActMapPropInit>b__317_14()` -> `ICustomAttributeProvider`
- method `<SerializableActMapPropInit>b__317_15(Object obj)` -> `Int32`
- method `<SerializableActMapPropInit>b__317_16(Object obj, Int32 value)` -> `Void`
- method `<SerializableActMapPropInit>b__317_17()` -> `ICustomAttributeProvider`
- method `<SerializableActMapPropInit>b__317_18(Object obj)` -> `Int32`
- method `<SerializableActMapPropInit>b__317_19(Object obj, Int32 value)` -> `Void`
- method `<SerializableActMapPropInit>b__317_2()` -> `ICustomAttributeProvider`
- method `<SerializableActMapPropInit>b__317_20()` -> `ICustomAttributeProvider`
- method `<SerializableActMapPropInit>b__317_3(Object obj)` -> `SerializableMapPoint`
- method `<SerializableActMapPropInit>b__317_4(Object obj, SerializableMapPoint value)` -> `Void`
- method `<SerializableActMapPropInit>b__317_5()` -> `ICustomAttributeProvider`
- method `<SerializableActMapPropInit>b__317_6(Object obj)` -> `SerializableMapPoint`
- method `<SerializableActMapPropInit>b__317_7(Object obj, SerializableMapPoint value)` -> `Void`
- method `<SerializableActMapPropInit>b__317_8()` -> `ICustomAttributeProvider`
- method `<SerializableActMapPropInit>b__317_9(Object obj)` -> `SerializableMapPoint`
- method `<SerializableActModelPropInit>b__322_0(Object obj)` -> `ModelId`
- method `<SerializableActModelPropInit>b__322_1(Object obj, ModelId value)` -> `Void`
- method `<SerializableActModelPropInit>b__322_2()` -> `ICustomAttributeProvider`
- method `<SerializableActModelPropInit>b__322_3(Object obj)` -> `SerializableRoomSet`
- method `<SerializableActModelPropInit>b__322_4(Object obj, SerializableRoomSet value)` -> `Void`
- method `<SerializableActModelPropInit>b__322_5()` -> `ICustomAttributeProvider`
- method `<SerializableActModelPropInit>b__322_6(Object obj)` -> `SerializableActMap`
- method `<SerializableActModelPropInit>b__322_7(Object obj, SerializableActMap value)` -> `Void`
- method `<SerializableActModelPropInit>b__322_8()` -> `ICustomAttributeProvider`
- method `<SerializableBadgePropInit>b__327_0(Object obj)` -> `String`
- method `<SerializableBadgePropInit>b__327_1(Object obj, String value)` -> `Void`
- method `<SerializableBadgePropInit>b__327_2()` -> `ICustomAttributeProvider`
- method `<SerializableBadgePropInit>b__327_3(Object obj)` -> `BadgeRarity`
- method `<SerializableBadgePropInit>b__327_4(Object obj, BadgeRarity value)` -> `Void`
- method `<SerializableBadgePropInit>b__327_5()` -> `ICustomAttributeProvider`
- method `<SerializableCardPropInit>b__333_0(Object obj)` -> `ModelId`
- method `<SerializableCardPropInit>b__333_1(Object obj, ModelId value)` -> `Void`
- method `<SerializableCardPropInit>b__333_10(Object obj, SavedProperties value)` -> `Void`
- method `<SerializableCardPropInit>b__333_11()` -> `ICustomAttributeProvider`
- method `<SerializableCardPropInit>b__333_12(Object obj)` -> `Nullable`1`
- method `<SerializableCardPropInit>b__333_13(Object obj, Nullable`1 value)` -> `Void`
- method `<SerializableCardPropInit>b__333_14()` -> `ICustomAttributeProvider`
- method `<SerializableCardPropInit>b__333_2()` -> `ICustomAttributeProvider`
- method `<SerializableCardPropInit>b__333_3(Object obj)` -> `Int32`
- method `<SerializableCardPropInit>b__333_4(Object obj, Int32 value)` -> `Void`
- method `<SerializableCardPropInit>b__333_5()` -> `ICustomAttributeProvider`
- method `<SerializableCardPropInit>b__333_6(Object obj)` -> `SerializableEnchantment`
- method `<SerializableCardPropInit>b__333_7(Object obj, SerializableEnchantment value)` -> `Void`
- method `<SerializableCardPropInit>b__333_8()` -> `ICustomAttributeProvider`
- method `<SerializableCardPropInit>b__333_9(Object obj)` -> `SavedProperties`
- method `<SerializableEnchantmentPropInit>b__342_0(Object obj)` -> `ModelId`
- method `<SerializableEnchantmentPropInit>b__342_1(Object obj, ModelId value)` -> `Void`
- method `<SerializableEnchantmentPropInit>b__342_2()` -> `ICustomAttributeProvider`
- method `<SerializableEnchantmentPropInit>b__342_3(Object obj)` -> `Int32`
- method `<SerializableEnchantmentPropInit>b__342_4(Object obj, Int32 value)` -> `Void`
- method `<SerializableEnchantmentPropInit>b__342_5()` -> `ICustomAttributeProvider`
- method `<SerializableEnchantmentPropInit>b__342_6(Object obj)` -> `SavedProperties`
- method `<SerializableEnchantmentPropInit>b__342_7(Object obj, SavedProperties value)` -> `Void`
- method `<SerializableEnchantmentPropInit>b__342_8()` -> `ICustomAttributeProvider`
- method `<SerializableEpochPropInit>b__412_0(Object obj)` -> `String`
- method `<SerializableEpochPropInit>b__412_1()` -> `ICustomAttributeProvider`
- method `<SerializableEpochPropInit>b__412_2(Object obj)` -> `EpochState`
- method `<SerializableEpochPropInit>b__412_3(Object obj, EpochState value)` -> `Void`
- method `<SerializableEpochPropInit>b__412_4()` -> `ICustomAttributeProvider`
- method `<SerializableEpochPropInit>b__412_5(Object obj)` -> `Int64`
- method `<SerializableEpochPropInit>b__412_6(Object obj, Int64 value)` -> `Void`
- method `<SerializableEpochPropInit>b__412_7()` -> `ICustomAttributeProvider`
- method `<SerializableExtraPlayerFieldsPropInit>b__418_0(Object obj)` -> `Int32`
- method `<SerializableExtraPlayerFieldsPropInit>b__418_1(Object obj, Int32 value)` -> `Void`
- method `<SerializableExtraPlayerFieldsPropInit>b__418_2()` -> `ICustomAttributeProvider`
- method `<SerializableExtraPlayerFieldsPropInit>b__418_3(Object obj)` -> `Int32`
- method `<SerializableExtraPlayerFieldsPropInit>b__418_4(Object obj, Int32 value)` -> `Void`
- method `<SerializableExtraPlayerFieldsPropInit>b__418_5()` -> `ICustomAttributeProvider`
- method `<SerializableExtraRunFieldsPropInit>b__347_0(Object obj)` -> `Boolean`
- method `<SerializableExtraRunFieldsPropInit>b__347_1(Object obj, Boolean value)` -> `Void`
- method `<SerializableExtraRunFieldsPropInit>b__347_2()` -> `ICustomAttributeProvider`
- method `<SerializableExtraRunFieldsPropInit>b__347_3(Object obj)` -> `Int32`
- method `<SerializableExtraRunFieldsPropInit>b__347_4(Object obj, Int32 value)` -> `Void`
- method `<SerializableExtraRunFieldsPropInit>b__347_5()` -> `ICustomAttributeProvider`
- method `<SerializableExtraRunFieldsPropInit>b__347_6(Object obj)` -> `Boolean`
- method `<SerializableExtraRunFieldsPropInit>b__347_7(Object obj, Boolean value)` -> `Void`
- method `<SerializableExtraRunFieldsPropInit>b__347_8()` -> `ICustomAttributeProvider`
- method `<SerializableMapDrawingLinePropInit>b__247_0(Object obj)` -> `Boolean`
- method `<SerializableMapDrawingLinePropInit>b__247_1(Object obj, Boolean value)` -> `Void`
- method `<SerializableMapDrawingLinePropInit>b__247_2()` -> `ICustomAttributeProvider`
- method `<SerializableMapDrawingLinePropInit>b__247_3(Object obj)` -> `List`1`
- method `<SerializableMapDrawingLinePropInit>b__247_4(Object obj, List`1 value)` -> `Void`
- method `<SerializableMapDrawingLinePropInit>b__247_5()` -> `ICustomAttributeProvider`
- method `<SerializableMapDrawingsPropInit>b__252_0(Object obj)` -> `List`1`
- method `<SerializableMapDrawingsPropInit>b__252_1(Object obj, List`1 value)` -> `Void`
- method `<SerializableMapDrawingsPropInit>b__252_2()` -> `ICustomAttributeProvider`
- method `<SerializableMapPointPropInit>b__352_0(Object obj)` -> `MapCoord`
- method `<SerializableMapPointPropInit>b__352_1(Object obj, MapCoord value)` -> `Void`
- method `<SerializableMapPointPropInit>b__352_10(Object obj, List`1 value)` -> `Void`
- method `<SerializableMapPointPropInit>b__352_11()` -> `ICustomAttributeProvider`
- method `<SerializableMapPointPropInit>b__352_2()` -> `ICustomAttributeProvider`
- method `<SerializableMapPointPropInit>b__352_3(Object obj)` -> `MapPointType`
- method `<SerializableMapPointPropInit>b__352_4(Object obj, MapPointType value)` -> `Void`
- method `<SerializableMapPointPropInit>b__352_5()` -> `ICustomAttributeProvider`
- method `<SerializableMapPointPropInit>b__352_6(Object obj)` -> `Boolean`
- method `<SerializableMapPointPropInit>b__352_7(Object obj, Boolean value)` -> `Void`
- method `<SerializableMapPointPropInit>b__352_8()` -> `ICustomAttributeProvider`
- method `<SerializableMapPointPropInit>b__352_9(Object obj)` -> `List`1`
- method `<SerializableModifierPropInit>b__357_0(Object obj)` -> `ModelId`
- method `<SerializableModifierPropInit>b__357_1(Object obj, ModelId value)` -> `Void`
- method `<SerializableModifierPropInit>b__357_2()` -> `ICustomAttributeProvider`
- method `<SerializableModifierPropInit>b__357_3(Object obj)` -> `SavedProperties`
- method `<SerializableModifierPropInit>b__357_4(Object obj, SavedProperties value)` -> `Void`
- method `<SerializableModifierPropInit>b__357_5()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerMapDrawingsPropInit>b__257_0(Object obj)` -> `UInt64`
- method `<SerializablePlayerMapDrawingsPropInit>b__257_1(Object obj, UInt64 value)` -> `Void`
- method `<SerializablePlayerMapDrawingsPropInit>b__257_2()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerMapDrawingsPropInit>b__257_3(Object obj)` -> `List`1`
- method `<SerializablePlayerMapDrawingsPropInit>b__257_4(Object obj, List`1 value)` -> `Void`
- method `<SerializablePlayerMapDrawingsPropInit>b__257_5()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerOddsSetPropInit>b__367_0(Object obj)` -> `Single`
- method `<SerializablePlayerOddsSetPropInit>b__367_1(Object obj, Single value)` -> `Void`
- method `<SerializablePlayerOddsSetPropInit>b__367_2()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerOddsSetPropInit>b__367_3(Object obj)` -> `Single`
- method `<SerializablePlayerOddsSetPropInit>b__367_4(Object obj, Single value)` -> `Void`
- method `<SerializablePlayerOddsSetPropInit>b__367_5()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_0(Object obj)` -> `ModelId`
- method `<SerializablePlayerPropInit>b__362_1(Object obj, ModelId value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_10(Object obj, Int32 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_11()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_12(Object obj)` -> `Int32`
- method `<SerializablePlayerPropInit>b__362_13(Object obj, Int32 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_14()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_15(Object obj)` -> `Int32`
- method `<SerializablePlayerPropInit>b__362_16(Object obj, Int32 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_17()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_18(Object obj)` -> `Int32`
- method `<SerializablePlayerPropInit>b__362_19(Object obj, Int32 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_2()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_20()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_21(Object obj)` -> `UInt64`
- method `<SerializablePlayerPropInit>b__362_22(Object obj, UInt64 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_23()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_24(Object obj)` -> `List`1`
- method `<SerializablePlayerPropInit>b__362_25(Object obj, List`1 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_26()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_27(Object obj)` -> `List`1`
- method `<SerializablePlayerPropInit>b__362_28(Object obj, List`1 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_29()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_3(Object obj)` -> `Int32`
- method `<SerializablePlayerPropInit>b__362_30(Object obj)` -> `List`1`
- method `<SerializablePlayerPropInit>b__362_31(Object obj, List`1 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_32()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_33(Object obj)` -> `SerializablePlayerRngSet`
- method `<SerializablePlayerPropInit>b__362_34(Object obj, SerializablePlayerRngSet value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_35()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_36(Object obj)` -> `SerializablePlayerOddsSet`
- method `<SerializablePlayerPropInit>b__362_37(Object obj, SerializablePlayerOddsSet value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_38()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_39(Object obj)` -> `SerializableRelicGrabBag`
- method `<SerializablePlayerPropInit>b__362_4(Object obj, Int32 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_40(Object obj, SerializableRelicGrabBag value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_41()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_42(Object obj)` -> `SerializableExtraPlayerFields`
- method `<SerializablePlayerPropInit>b__362_43(Object obj, SerializableExtraPlayerFields value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_44()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_45(Object obj)` -> `SerializableUnlockState`
- method `<SerializablePlayerPropInit>b__362_46(Object obj, SerializableUnlockState value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_47()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_48(Object obj)` -> `List`1`
- method `<SerializablePlayerPropInit>b__362_49(Object obj, List`1 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_5()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_50()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_51(Object obj)` -> `List`1`
- method `<SerializablePlayerPropInit>b__362_52(Object obj, List`1 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_53()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_54(Object obj)` -> `List`1`
- method `<SerializablePlayerPropInit>b__362_55(Object obj, List`1 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_56()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_57(Object obj)` -> `List`1`
- method `<SerializablePlayerPropInit>b__362_58(Object obj, List`1 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_59()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_6(Object obj)` -> `Int32`
- method `<SerializablePlayerPropInit>b__362_60(Object obj)` -> `List`1`
- method `<SerializablePlayerPropInit>b__362_61(Object obj, List`1 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_62()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_7(Object obj, Int32 value)` -> `Void`
- method `<SerializablePlayerPropInit>b__362_8()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerPropInit>b__362_9(Object obj)` -> `Int32`
- method `<SerializablePlayerRngSetPropInit>b__423_0(Object obj)` -> `UInt32`
- method `<SerializablePlayerRngSetPropInit>b__423_1(Object obj, UInt32 value)` -> `Void`
- method `<SerializablePlayerRngSetPropInit>b__423_2()` -> `ICustomAttributeProvider`
- method `<SerializablePlayerRngSetPropInit>b__423_3(Object obj)` -> `Dictionary`2`
- method `<SerializablePlayerRngSetPropInit>b__423_4(Object obj, Dictionary`2 value)` -> `Void`
- method `<SerializablePlayerRngSetPropInit>b__423_5()` -> `ICustomAttributeProvider`
- method `<SerializablePotionPropInit>b__372_0(Object obj)` -> `ModelId`
- method `<SerializablePotionPropInit>b__372_1(Object obj, ModelId value)` -> `Void`
- method `<SerializablePotionPropInit>b__372_2()` -> `ICustomAttributeProvider`
- method `<SerializablePotionPropInit>b__372_3(Object obj)` -> `Int32`
- method `<SerializablePotionPropInit>b__372_4(Object obj, Int32 value)` -> `Void`
- method `<SerializablePotionPropInit>b__372_5()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_0(Object obj)` -> `Int32`
- method `<SerializableProgressPropInit>b__428_1(Object obj, Int32 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_10(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_11()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_12(Object obj)` -> `List`1`
- method `<SerializableProgressPropInit>b__428_13(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_14()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_15(Object obj)` -> `List`1`
- method `<SerializableProgressPropInit>b__428_16(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_17()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_18(Object obj)` -> `List`1`
- method `<SerializableProgressPropInit>b__428_19(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_2()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_20()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_21(Object obj)` -> `Boolean`
- method `<SerializableProgressPropInit>b__428_22(Object obj, Boolean value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_23()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_24(Object obj)` -> `List`1`
- method `<SerializableProgressPropInit>b__428_25(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_26()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_27(Object obj)` -> `List`1`
- method `<SerializableProgressPropInit>b__428_28(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_29()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_3(Object obj)` -> `String`
- method `<SerializableProgressPropInit>b__428_30(Object obj)` -> `List`1`
- method `<SerializableProgressPropInit>b__428_31(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_32()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_33(Object obj)` -> `List`1`
- method `<SerializableProgressPropInit>b__428_34(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_35()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_36(Object obj)` -> `List`1`
- method `<SerializableProgressPropInit>b__428_37(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_38()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_39(Object obj)` -> `List`1`
- method `<SerializableProgressPropInit>b__428_4(Object obj, String value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_40(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_41()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_42(Object obj)` -> `List`1`
- method `<SerializableProgressPropInit>b__428_43(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_44()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_45(Object obj)` -> `List`1`
- method `<SerializableProgressPropInit>b__428_46(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_47()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_48(Object obj)` -> `Int64`
- method `<SerializableProgressPropInit>b__428_49(Object obj, Int64 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_5()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_50()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_51(Object obj)` -> `Int32`
- method `<SerializableProgressPropInit>b__428_52(Object obj, Int32 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_53()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_54(Object obj)` -> `Int32`
- method `<SerializableProgressPropInit>b__428_55(Object obj, Int32 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_56()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_57(Object obj)` -> `Int64`
- method `<SerializableProgressPropInit>b__428_58(Object obj, Int64 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_59()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_6(Object obj)` -> `List`1`
- method `<SerializableProgressPropInit>b__428_60(Object obj)` -> `Int64`
- method `<SerializableProgressPropInit>b__428_61(Object obj, Int64 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_62()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_63(Object obj)` -> `Int32`
- method `<SerializableProgressPropInit>b__428_64(Object obj, Int32 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_65()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_66(Object obj)` -> `Int32`
- method `<SerializableProgressPropInit>b__428_67(Object obj, Int32 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_68()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_69(Object obj)` -> `Int32`
- method `<SerializableProgressPropInit>b__428_7(Object obj, List`1 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_70(Object obj, Int32 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_71()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_72(Object obj)` -> `Int32`
- method `<SerializableProgressPropInit>b__428_73(Object obj, Int32 value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_74()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_75(Object obj)` -> `ModelId`
- method `<SerializableProgressPropInit>b__428_76(Object obj, ModelId value)` -> `Void`
- method `<SerializableProgressPropInit>b__428_77()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_78()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_79()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_8()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_80()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_81()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_82()` -> `ICustomAttributeProvider`
- method `<SerializableProgressPropInit>b__428_9(Object obj)` -> `List`1`
- method `<SerializableRelicGrabBagPropInit>b__382_0(Object obj)` -> `Dictionary`2`
- method `<SerializableRelicGrabBagPropInit>b__382_1(Object obj, Dictionary`2 value)` -> `Void`
- method `<SerializableRelicGrabBagPropInit>b__382_2()` -> `ICustomAttributeProvider`
- method `<SerializableRelicPropInit>b__377_0(Object obj)` -> `ModelId`
- method `<SerializableRelicPropInit>b__377_1(Object obj, ModelId value)` -> `Void`
- method `<SerializableRelicPropInit>b__377_2()` -> `ICustomAttributeProvider`
- method `<SerializableRelicPropInit>b__377_3(Object obj)` -> `SavedProperties`
- method `<SerializableRelicPropInit>b__377_4(Object obj, SavedProperties value)` -> `Void`
- method `<SerializableRelicPropInit>b__377_5()` -> `ICustomAttributeProvider`
- method `<SerializableRelicPropInit>b__377_6(Object obj)` -> `Nullable`1`
- method `<SerializableRelicPropInit>b__377_7(Object obj, Nullable`1 value)` -> `Void`
- method `<SerializableRelicPropInit>b__377_8()` -> `ICustomAttributeProvider`
- method `<SerializableRewardPropInit>b__387_0(Object obj)` -> `RewardType`
- method `<SerializableRewardPropInit>b__387_1(Object obj, RewardType value)` -> `Void`
- method `<SerializableRewardPropInit>b__387_10(Object obj, Int32 value)` -> `Void`
- method `<SerializableRewardPropInit>b__387_11()` -> `ICustomAttributeProvider`
- method `<SerializableRewardPropInit>b__387_12(Object obj)` -> `Boolean`
- method `<SerializableRewardPropInit>b__387_13(Object obj, Boolean value)` -> `Void`
- method `<SerializableRewardPropInit>b__387_14()` -> `ICustomAttributeProvider`
- method `<SerializableRewardPropInit>b__387_15(Object obj)` -> `CardCreationSource`
- method `<SerializableRewardPropInit>b__387_16(Object obj, CardCreationSource value)` -> `Void`
- method `<SerializableRewardPropInit>b__387_17()` -> `ICustomAttributeProvider`
- method `<SerializableRewardPropInit>b__387_18(Object obj)` -> `CardRarityOddsType`
- method `<SerializableRewardPropInit>b__387_19(Object obj, CardRarityOddsType value)` -> `Void`
- method `<SerializableRewardPropInit>b__387_2()` -> `ICustomAttributeProvider`
- method `<SerializableRewardPropInit>b__387_20()` -> `ICustomAttributeProvider`
- method `<SerializableRewardPropInit>b__387_21(Object obj)` -> `List`1`
- method `<SerializableRewardPropInit>b__387_22(Object obj, List`1 value)` -> `Void`
- method `<SerializableRewardPropInit>b__387_23()` -> `ICustomAttributeProvider`
- method `<SerializableRewardPropInit>b__387_24(Object obj)` -> `Int32`
- method `<SerializableRewardPropInit>b__387_25(Object obj, Int32 value)` -> `Void`
- method `<SerializableRewardPropInit>b__387_26()` -> `ICustomAttributeProvider`
- method `<SerializableRewardPropInit>b__387_27(Object obj)` -> `ModelId`
- method `<SerializableRewardPropInit>b__387_28(Object obj, ModelId value)` -> `Void`
- method `<SerializableRewardPropInit>b__387_29()` -> `ICustomAttributeProvider`
- method `<SerializableRewardPropInit>b__387_3(Object obj)` -> `ModelId`
- method `<SerializableRewardPropInit>b__387_4(Object obj, ModelId value)` -> `Void`
- method `<SerializableRewardPropInit>b__387_5()` -> `ICustomAttributeProvider`
- method `<SerializableRewardPropInit>b__387_6(Object obj)` -> `SerializableCard`
- method `<SerializableRewardPropInit>b__387_7(Object obj, SerializableCard value)` -> `Void`
- method `<SerializableRewardPropInit>b__387_8()` -> `ICustomAttributeProvider`
- method `<SerializableRewardPropInit>b__387_9(Object obj)` -> `Int32`
- method `<SerializableRoomPropInit>b__392_0(Object obj)` -> `RoomType`
- method `<SerializableRoomPropInit>b__392_1(Object obj, RoomType value)` -> `Void`
- method `<SerializableRoomPropInit>b__392_10(Object obj, Boolean value)` -> `Void`
- method `<SerializableRoomPropInit>b__392_11()` -> `ICustomAttributeProvider`
- method `<SerializableRoomPropInit>b__392_12(Object obj)` -> `Single`
- method `<SerializableRoomPropInit>b__392_13(Object obj, Single value)` -> `Void`
- method `<SerializableRoomPropInit>b__392_14()` -> `ICustomAttributeProvider`
- method `<SerializableRoomPropInit>b__392_15(Object obj)` -> `Dictionary`2`
- method `<SerializableRoomPropInit>b__392_16(Object obj, Dictionary`2 value)` -> `Void`
- method `<SerializableRoomPropInit>b__392_17()` -> `ICustomAttributeProvider`
- method `<SerializableRoomPropInit>b__392_18(Object obj)` -> `ModelId`
- method `<SerializableRoomPropInit>b__392_19(Object obj, ModelId value)` -> `Void`
- method `<SerializableRoomPropInit>b__392_2()` -> `ICustomAttributeProvider`
- method `<SerializableRoomPropInit>b__392_20()` -> `ICustomAttributeProvider`
- method `<SerializableRoomPropInit>b__392_21(Object obj)` -> `Boolean`
- method `<SerializableRoomPropInit>b__392_22(Object obj, Boolean value)` -> `Void`
- method `<SerializableRoomPropInit>b__392_23()` -> `ICustomAttributeProvider`
- method `<SerializableRoomPropInit>b__392_24(Object obj)` -> `Dictionary`2`
- method `<SerializableRoomPropInit>b__392_25(Object obj, Dictionary`2 value)` -> `Void`
- method `<SerializableRoomPropInit>b__392_26()` -> `ICustomAttributeProvider`
- method `<SerializableRoomPropInit>b__392_3(Object obj)` -> `ModelId`
- method `<SerializableRoomPropInit>b__392_4(Object obj, ModelId value)` -> `Void`
- method `<SerializableRoomPropInit>b__392_5()` -> `ICustomAttributeProvider`
- method `<SerializableRoomPropInit>b__392_6(Object obj)` -> `ModelId`
- method `<SerializableRoomPropInit>b__392_7(Object obj, ModelId value)` -> `Void`
- method `<SerializableRoomPropInit>b__392_8()` -> `ICustomAttributeProvider`
- method `<SerializableRoomPropInit>b__392_9(Object obj)` -> `Boolean`
- method `<SerializableRoomSetPropInit>b__397_0(Object obj)` -> `List`1`
- method `<SerializableRoomSetPropInit>b__397_1(Object obj, List`1 value)` -> `Void`
- method `<SerializableRoomSetPropInit>b__397_10(Object obj, Int32 value)` -> `Void`
- method `<SerializableRoomSetPropInit>b__397_11()` -> `ICustomAttributeProvider`
- method `<SerializableRoomSetPropInit>b__397_12(Object obj)` -> `List`1`
- method `<SerializableRoomSetPropInit>b__397_13(Object obj, List`1 value)` -> `Void`
- method `<SerializableRoomSetPropInit>b__397_14()` -> `ICustomAttributeProvider`
- method `<SerializableRoomSetPropInit>b__397_15(Object obj)` -> `Int32`
- method `<SerializableRoomSetPropInit>b__397_16(Object obj, Int32 value)` -> `Void`
- method `<SerializableRoomSetPropInit>b__397_17()` -> `ICustomAttributeProvider`
- method `<SerializableRoomSetPropInit>b__397_18(Object obj)` -> `Int32`
- method `<SerializableRoomSetPropInit>b__397_19(Object obj, Int32 value)` -> `Void`
- method `<SerializableRoomSetPropInit>b__397_2()` -> `ICustomAttributeProvider`
- method `<SerializableRoomSetPropInit>b__397_20()` -> `ICustomAttributeProvider`
- method `<SerializableRoomSetPropInit>b__397_21(Object obj)` -> `ModelId`
- method `<SerializableRoomSetPropInit>b__397_22(Object obj, ModelId value)` -> `Void`
- method `<SerializableRoomSetPropInit>b__397_23()` -> `ICustomAttributeProvider`
- method `<SerializableRoomSetPropInit>b__397_24(Object obj)` -> `ModelId`
- method `<SerializableRoomSetPropInit>b__397_25(Object obj, ModelId value)` -> `Void`
- method `<SerializableRoomSetPropInit>b__397_26()` -> `ICustomAttributeProvider`
- method `<SerializableRoomSetPropInit>b__397_27(Object obj)` -> `ModelId`
- method `<SerializableRoomSetPropInit>b__397_28(Object obj, ModelId value)` -> `Void`
- method `<SerializableRoomSetPropInit>b__397_29()` -> `ICustomAttributeProvider`
- method `<SerializableRoomSetPropInit>b__397_3(Object obj)` -> `Int32`
- method `<SerializableRoomSetPropInit>b__397_4(Object obj, Int32 value)` -> `Void`
- method `<SerializableRoomSetPropInit>b__397_5()` -> `ICustomAttributeProvider`
- method `<SerializableRoomSetPropInit>b__397_6(Object obj)` -> `List`1`
- method `<SerializableRoomSetPropInit>b__397_7(Object obj, List`1 value)` -> `Void`
- method `<SerializableRoomSetPropInit>b__397_8()` -> `ICustomAttributeProvider`
- method `<SerializableRoomSetPropInit>b__397_9(Object obj)` -> `Int32`
- method `<SerializableRunOddsSetPropInit>b__402_0(Object obj)` -> `Single`
- method `<SerializableRunOddsSetPropInit>b__402_1(Object obj, Single value)` -> `Void`
- method `<SerializableRunOddsSetPropInit>b__402_10(Object obj, Single value)` -> `Void`
- method `<SerializableRunOddsSetPropInit>b__402_11()` -> `ICustomAttributeProvider`
- method `<SerializableRunOddsSetPropInit>b__402_2()` -> `ICustomAttributeProvider`
- method `<SerializableRunOddsSetPropInit>b__402_3(Object obj)` -> `Single`
- method `<SerializableRunOddsSetPropInit>b__402_4(Object obj, Single value)` -> `Void`
- method `<SerializableRunOddsSetPropInit>b__402_5()` -> `ICustomAttributeProvider`
- method `<SerializableRunOddsSetPropInit>b__402_6(Object obj)` -> `Single`
- method `<SerializableRunOddsSetPropInit>b__402_7(Object obj, Single value)` -> `Void`
- method `<SerializableRunOddsSetPropInit>b__402_8()` -> `ICustomAttributeProvider`
- method `<SerializableRunOddsSetPropInit>b__402_9(Object obj)` -> `Single`
- method `<SerializableRunPropInit>b__434_0(Object obj)` -> `Int32`
- method `<SerializableRunPropInit>b__434_1(Object obj, Int32 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_10(Object obj, Nullable`1 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_11()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_12(Object obj)` -> `Int32`
- method `<SerializableRunPropInit>b__434_13(Object obj, Int32 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_14()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_15(Object obj)` -> `List`1`
- method `<SerializableRunPropInit>b__434_16(Object obj, List`1 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_17()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_18(Object obj)` -> `SerializableRoom`
- method `<SerializableRunPropInit>b__434_19(Object obj, SerializableRoom value)` -> `Void`
- method `<SerializableRunPropInit>b__434_2()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_20()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_21(Object obj)` -> `SerializableRunOddsSet`
- method `<SerializableRunPropInit>b__434_22(Object obj, SerializableRunOddsSet value)` -> `Void`
- method `<SerializableRunPropInit>b__434_23()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_24(Object obj)` -> `SerializableRelicGrabBag`
- method `<SerializableRunPropInit>b__434_25(Object obj, SerializableRelicGrabBag value)` -> `Void`
- method `<SerializableRunPropInit>b__434_26()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_27(Object obj)` -> `List`1`
- method `<SerializableRunPropInit>b__434_28(Object obj, List`1 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_29()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_3(Object obj)` -> `List`1`
- method `<SerializableRunPropInit>b__434_30(Object obj)` -> `SerializableRunRngSet`
- method `<SerializableRunPropInit>b__434_31(Object obj, SerializableRunRngSet value)` -> `Void`
- method `<SerializableRunPropInit>b__434_32()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_33(Object obj)` -> `List`1`
- method `<SerializableRunPropInit>b__434_34(Object obj, List`1 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_35()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_36(Object obj)` -> `List`1`
- method `<SerializableRunPropInit>b__434_37(Object obj, List`1 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_38()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_39(Object obj)` -> `Int64`
- method `<SerializableRunPropInit>b__434_4(Object obj, List`1 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_40(Object obj, Int64 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_41()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_42(Object obj)` -> `Int64`
- method `<SerializableRunPropInit>b__434_43(Object obj, Int64 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_44()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_45(Object obj)` -> `Int64`
- method `<SerializableRunPropInit>b__434_46(Object obj, Int64 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_47()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_48(Object obj)` -> `Int64`
- method `<SerializableRunPropInit>b__434_49(Object obj, Int64 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_5()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_50()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_51(Object obj)` -> `Int32`
- method `<SerializableRunPropInit>b__434_52(Object obj, Int32 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_53()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_54(Object obj)` -> `PlatformType`
- method `<SerializableRunPropInit>b__434_55(Object obj, PlatformType value)` -> `Void`
- method `<SerializableRunPropInit>b__434_56()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_57(Object obj)` -> `SerializableMapDrawings`
- method `<SerializableRunPropInit>b__434_58(Object obj, SerializableMapDrawings value)` -> `Void`
- method `<SerializableRunPropInit>b__434_59()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_6(Object obj)` -> `List`1`
- method `<SerializableRunPropInit>b__434_60(Object obj)` -> `SerializableExtraRunFields`
- method `<SerializableRunPropInit>b__434_61(Object obj, SerializableExtraRunFields value)` -> `Void`
- method `<SerializableRunPropInit>b__434_62()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_63(Object obj)` -> `GameMode`
- method `<SerializableRunPropInit>b__434_64(Object obj, GameMode value)` -> `Void`
- method `<SerializableRunPropInit>b__434_65()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_7(Object obj, List`1 value)` -> `Void`
- method `<SerializableRunPropInit>b__434_8()` -> `ICustomAttributeProvider`
- method `<SerializableRunPropInit>b__434_9(Object obj)` -> `Nullable`1`
- method `<SerializableRunRngSetPropInit>b__407_0(Object obj)` -> `String`
- method `<SerializableRunRngSetPropInit>b__407_1(Object obj, String value)` -> `Void`
- method `<SerializableRunRngSetPropInit>b__407_2()` -> `ICustomAttributeProvider`
- method `<SerializableRunRngSetPropInit>b__407_3(Object obj)` -> `Dictionary`2`
- method `<SerializableRunRngSetPropInit>b__407_4(Object obj, Dictionary`2 value)` -> `Void`
- method `<SerializableRunRngSetPropInit>b__407_5()` -> `ICustomAttributeProvider`
- method `<SerializableUnlockedAchievementPropInit>b__439_0(Object obj)` -> `String`
- method `<SerializableUnlockedAchievementPropInit>b__439_1(Object obj, String value)` -> `Void`
- method `<SerializableUnlockedAchievementPropInit>b__439_2()` -> `ICustomAttributeProvider`
- method `<SerializableUnlockedAchievementPropInit>b__439_3(Object obj)` -> `Int64`
- method `<SerializableUnlockedAchievementPropInit>b__439_4(Object obj, Int64 value)` -> `Void`
- method `<SerializableUnlockedAchievementPropInit>b__439_5()` -> `ICustomAttributeProvider`
- method `<SerializableUnlockStatePropInit>b__462_0(Object obj)` -> `List`1`
- method `<SerializableUnlockStatePropInit>b__462_1(Object obj, List`1 value)` -> `Void`
- method `<SerializableUnlockStatePropInit>b__462_2()` -> `ICustomAttributeProvider`
- method `<SerializableUnlockStatePropInit>b__462_3(Object obj)` -> `List`1`
- method `<SerializableUnlockStatePropInit>b__462_4(Object obj, List`1 value)` -> `Void`
- method `<SerializableUnlockStatePropInit>b__462_5()` -> `ICustomAttributeProvider`
- method `<SerializableUnlockStatePropInit>b__462_6(Object obj)` -> `Int32`
- method `<SerializableUnlockStatePropInit>b__462_7(Object obj, Int32 value)` -> `Void`
- method `<SerializableUnlockStatePropInit>b__462_8()` -> `ICustomAttributeProvider`
- method `<SettingsSaveModPropInit>b__73_0(Object obj)` -> `String`
- method `<SettingsSaveModPropInit>b__73_1(Object obj, String value)` -> `Void`
- method `<SettingsSaveModPropInit>b__73_2()` -> `ICustomAttributeProvider`
- method `<SettingsSaveModPropInit>b__73_3(Object obj)` -> `ModSource`
- method `<SettingsSaveModPropInit>b__73_4(Object obj, ModSource value)` -> `Void`
- method `<SettingsSaveModPropInit>b__73_5()` -> `ICustomAttributeProvider`
- method `<SettingsSaveModPropInit>b__73_6(Object obj)` -> `Boolean`
- method `<SettingsSaveModPropInit>b__73_7(Object obj, Boolean value)` -> `Void`
- method `<SettingsSaveModPropInit>b__73_8()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_0(Object obj)` -> `Int32`
- method `<SettingsSavePropInit>b__445_1(Object obj, Int32 value)` -> `Void`
- method `<SettingsSavePropInit>b__445_10(Object obj, Vector2I value)` -> `Void`
- method `<SettingsSavePropInit>b__445_11()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_12(Object obj)` -> `Vector2I`
- method `<SettingsSavePropInit>b__445_13(Object obj, Vector2I value)` -> `Void`
- method `<SettingsSavePropInit>b__445_14()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_15(Object obj)` -> `Boolean`
- method `<SettingsSavePropInit>b__445_16(Object obj, Boolean value)` -> `Void`
- method `<SettingsSavePropInit>b__445_17()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_18(Object obj)` -> `AspectRatioSetting`
- method `<SettingsSavePropInit>b__445_19(Object obj, AspectRatioSetting value)` -> `Void`
- method `<SettingsSavePropInit>b__445_2()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_20()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_21(Object obj)` -> `Int32`
- method `<SettingsSavePropInit>b__445_22(Object obj, Int32 value)` -> `Void`
- method `<SettingsSavePropInit>b__445_23()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_24(Object obj)` -> `Boolean`
- method `<SettingsSavePropInit>b__445_25(Object obj, Boolean value)` -> `Void`
- method `<SettingsSavePropInit>b__445_26()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_27(Object obj)` -> `VSyncType`
- method `<SettingsSavePropInit>b__445_28(Object obj, VSyncType value)` -> `Void`
- method `<SettingsSavePropInit>b__445_29()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_3(Object obj)` -> `Int32`
- method `<SettingsSavePropInit>b__445_30(Object obj)` -> `Int32`
- method `<SettingsSavePropInit>b__445_31(Object obj, Int32 value)` -> `Void`
- method `<SettingsSavePropInit>b__445_32()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_33(Object obj)` -> `Single`
- method `<SettingsSavePropInit>b__445_34(Object obj, Single value)` -> `Void`
- method `<SettingsSavePropInit>b__445_35()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_36(Object obj)` -> `Single`
- method `<SettingsSavePropInit>b__445_37(Object obj, Single value)` -> `Void`
- method `<SettingsSavePropInit>b__445_38()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_39(Object obj)` -> `Single`
- method `<SettingsSavePropInit>b__445_4(Object obj, Int32 value)` -> `Void`
- method `<SettingsSavePropInit>b__445_40(Object obj, Single value)` -> `Void`
- method `<SettingsSavePropInit>b__445_41()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_42(Object obj)` -> `Single`
- method `<SettingsSavePropInit>b__445_43(Object obj, Single value)` -> `Void`
- method `<SettingsSavePropInit>b__445_44()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_45(Object obj)` -> `ModSettings`
- method `<SettingsSavePropInit>b__445_46(Object obj, ModSettings value)` -> `Void`
- method `<SettingsSavePropInit>b__445_47()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_48(Object obj)` -> `Boolean`
- method `<SettingsSavePropInit>b__445_49(Object obj, Boolean value)` -> `Void`
- method `<SettingsSavePropInit>b__445_5()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_50()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_51(Object obj)` -> `Dictionary`2`
- method `<SettingsSavePropInit>b__445_52(Object obj, Dictionary`2 value)` -> `Void`
- method `<SettingsSavePropInit>b__445_53()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_54(Object obj)` -> `ControllerMappingType`
- method `<SettingsSavePropInit>b__445_55(Object obj, ControllerMappingType value)` -> `Void`
- method `<SettingsSavePropInit>b__445_56()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_57(Object obj)` -> `Dictionary`2`
- method `<SettingsSavePropInit>b__445_58(Object obj, Dictionary`2 value)` -> `Void`
- method `<SettingsSavePropInit>b__445_59()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_6(Object obj)` -> `String`
- method `<SettingsSavePropInit>b__445_60(Object obj)` -> `Boolean`
- method `<SettingsSavePropInit>b__445_61(Object obj, Boolean value)` -> `Void`
- method `<SettingsSavePropInit>b__445_62()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_63(Object obj)` -> `Boolean`
- method `<SettingsSavePropInit>b__445_64(Object obj, Boolean value)` -> `Void`
- method `<SettingsSavePropInit>b__445_65()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_66(Object obj)` -> `Boolean`
- method `<SettingsSavePropInit>b__445_67(Object obj, Boolean value)` -> `Void`
- method `<SettingsSavePropInit>b__445_68()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_7(Object obj, String value)` -> `Void`
- method `<SettingsSavePropInit>b__445_8()` -> `ICustomAttributeProvider`
- method `<SettingsSavePropInit>b__445_9(Object obj)` -> `Vector2I`
- method `<Vector2IPropInit>b__23_0(Object obj)` -> `Int32`
- method `<Vector2IPropInit>b__23_1(Object obj, Int32 value)` -> `Void`
- method `<Vector2IPropInit>b__23_2()` -> `ICustomAttributeProvider`
- method `<Vector2IPropInit>b__23_3(Object obj)` -> `Int32`
- method `<Vector2IPropInit>b__23_4(Object obj, Int32 value)` -> `Void`
- method `<Vector2IPropInit>b__23_5()` -> `ICustomAttributeProvider`
- method `<Vector2PropInit>b__18_0(Object obj)` -> `Single`
- method `<Vector2PropInit>b__18_1(Object obj, Single value)` -> `Void`
- method `<Vector2PropInit>b__18_2()` -> `ICustomAttributeProvider`
- method `<Vector2PropInit>b__18_3(Object obj)` -> `Single`
- method `<Vector2PropInit>b__18_4(Object obj, Single value)` -> `Void`
- method `<Vector2PropInit>b__18_5()` -> `ICustomAttributeProvider`
- field `<>c <>9`
- field `Func`2 <>9__102_0`
- field `Func`1 <>9__102_2`
- field `Func`2 <>9__103_0`
- field `Action`2 <>9__103_1`
- field `Action`2 <>9__103_10`
- field `Func`1 <>9__103_11`
- field `Func`1 <>9__103_2`
- field `Func`2 <>9__103_3`
- field `Action`2 <>9__103_4`
- field `Func`1 <>9__103_5`
- field `Func`2 <>9__103_6`
- field `Action`2 <>9__103_7`
- field `Func`1 <>9__103_8`
- field `Func`2 <>9__103_9`
- field `Func`1 <>9__108_0`
- field `Func`1 <>9__108_2`
- field `Func`2 <>9__109_0`
- field `Action`2 <>9__109_1`
- field `Func`1 <>9__109_2`
- field `Func`2 <>9__109_3`
- field `Action`2 <>9__109_4`
- field `Func`1 <>9__109_5`
- field `Func`1 <>9__137_0`
- field `Func`1 <>9__137_2`
- field `Func`2 <>9__138_0`
- field `Action`2 <>9__138_1`
- field `Func`1 <>9__138_2`
- field `Func`2 <>9__138_3`
- field `Action`2 <>9__138_4`
- field `Func`1 <>9__138_5`
- field `Func`2 <>9__138_6`
- field `Func`1 <>9__138_7`
- field `Func`1 <>9__142_0`
- field `Func`1 <>9__142_2`
- field `Func`2 <>9__143_0`
- field `Action`2 <>9__143_1`
- field `Func`1 <>9__143_2`
- field `Func`2 <>9__143_3`
- field `Action`2 <>9__143_4`
- field `Func`1 <>9__143_5`
- field `Func`1 <>9__147_0`
- field `Func`1 <>9__147_2`
- field `Func`2 <>9__148_0`
- field `Action`2 <>9__148_1`
- field `Func`1 <>9__148_2`
- field `Func`2 <>9__148_3`
- field `Action`2 <>9__148_4`
- field `Func`1 <>9__148_5`
- field `Func`1 <>9__152_0`
- field `Func`1 <>9__152_2`
- field `Func`2 <>9__153_0`
- field `Action`2 <>9__153_1`
- field `Func`1 <>9__153_2`
- field `Func`2 <>9__153_3`
- field `Action`2 <>9__153_4`
- field `Func`1 <>9__153_5`
- field `Func`1 <>9__157_0`
- field `Func`1 <>9__157_2`
- field `Func`2 <>9__158_0`
- field `Action`2 <>9__158_1`
- field `Func`1 <>9__158_2`
- field `Func`2 <>9__158_3`
- field `Action`2 <>9__158_4`
- field `Func`1 <>9__158_5`
- field `Func`1 <>9__162_0`
- field `Func`1 <>9__162_2`
- field `Func`2 <>9__163_0`
- field `Action`2 <>9__163_1`
- field `Func`1 <>9__163_2`
- field `Func`2 <>9__163_3`
- field `Action`2 <>9__163_4`
- field `Func`1 <>9__163_5`
- field `Func`2 <>9__163_6`
- field `Action`2 <>9__163_7`
- field `Func`1 <>9__163_8`
- field `Func`1 <>9__167_0`
- field `Func`1 <>9__167_2`
- field `Func`2 <>9__168_0`
- field `Action`2 <>9__168_1`
- field `Action`2 <>9__168_10`
- field `Func`1 <>9__168_11`
- field `Func`1 <>9__168_2`
- field `Func`2 <>9__168_3`
- field `Action`2 <>9__168_4`
- field `Func`1 <>9__168_5`
- field `Func`2 <>9__168_6`
- field `Action`2 <>9__168_7`
- field `Func`1 <>9__168_8`
- field `Func`2 <>9__168_9`
- field `Func`1 <>9__17_0`
- field `Func`1 <>9__17_2`
- field `Func`1 <>9__172_0`
- field `Func`1 <>9__172_2`
- field `Func`2 <>9__173_0`
- field `Action`2 <>9__173_1`
- field `Func`1 <>9__173_2`
- field `Func`2 <>9__173_3`
- field `Action`2 <>9__173_4`
- field `Func`1 <>9__173_5`
- field `Func`1 <>9__177_0`
- field `Func`1 <>9__177_2`
- field `Func`2 <>9__178_0`
- field `Action`2 <>9__178_1`
- field `Action`2 <>9__178_10`
- field `Func`1 <>9__178_11`
- field `Func`2 <>9__178_12`
- field `Action`2 <>9__178_13`
- field `Func`1 <>9__178_14`
- field `Func`2 <>9__178_15`
- field `Action`2 <>9__178_16`
- field `Func`1 <>9__178_17`
- field `Func`2 <>9__178_18`
- field `Action`2 <>9__178_19`
- field `Func`1 <>9__178_2`
- field `Func`1 <>9__178_20`
- field `Func`2 <>9__178_21`
- field `Action`2 <>9__178_22`
- field `Func`1 <>9__178_23`
- field `Func`2 <>9__178_24`
- field `Action`2 <>9__178_25`
- field `Func`1 <>9__178_26`
- field `Func`2 <>9__178_27`
- field `Action`2 <>9__178_28`
- field `Func`1 <>9__178_29`
- field `Func`2 <>9__178_3`
- field `Func`2 <>9__178_30`
- field `Action`2 <>9__178_31`
- field `Func`1 <>9__178_32`
- field `Func`2 <>9__178_33`
- field `Action`2 <>9__178_34`
- field `Func`1 <>9__178_35`
- field `Func`2 <>9__178_36`
- field `Action`2 <>9__178_37`
- field `Func`1 <>9__178_38`
- field `Func`2 <>9__178_39`
- field `Action`2 <>9__178_4`
- field `Action`2 <>9__178_40`
- field `Func`1 <>9__178_41`
- field `Func`2 <>9__178_42`
- field `Action`2 <>9__178_43`
- field `Func`1 <>9__178_44`
- field `Func`2 <>9__178_45`
- field `Action`2 <>9__178_46`
- field `Func`1 <>9__178_47`
- field `Func`2 <>9__178_48`
- field `Action`2 <>9__178_49`
- field `Func`1 <>9__178_5`
- field `Func`1 <>9__178_50`
- field `Func`2 <>9__178_51`
- field `Action`2 <>9__178_52`
- field `Func`1 <>9__178_53`
- field `Func`2 <>9__178_54`
- field `Action`2 <>9__178_55`
- field `Func`1 <>9__178_56`
- field `Func`2 <>9__178_57`
- field `Action`2 <>9__178_58`
- field `Func`1 <>9__178_59`
- field `Func`2 <>9__178_6`
- field `Func`2 <>9__178_60`
- field `Action`2 <>9__178_61`
- field `Func`1 <>9__178_62`
- field `Func`2 <>9__178_63`
- field `Action`2 <>9__178_64`
- field `Func`1 <>9__178_65`
- field `Func`2 <>9__178_66`
- field `Action`2 <>9__178_67`
- field `Func`1 <>9__178_68`
- field `Func`2 <>9__178_69`
- field `Action`2 <>9__178_7`
- field `Action`2 <>9__178_70`
- field `Func`1 <>9__178_71`
- field `Func`2 <>9__178_72`
- field `Action`2 <>9__178_73`
- field `Func`1 <>9__178_74`
- field `Func`2 <>9__178_75`
- field `Action`2 <>9__178_76`
- field `Func`1 <>9__178_77`
- field `Func`2 <>9__178_78`
- field `Action`2 <>9__178_79`
- field `Func`1 <>9__178_8`
- field `Func`1 <>9__178_80`
- field `Func`2 <>9__178_81`
- field `Action`2 <>9__178_82`
- field `Func`1 <>9__178_83`
- field `Func`2 <>9__178_84`
- field `Action`2 <>9__178_85`
- field `Func`1 <>9__178_86`
- field `Func`2 <>9__178_87`
- field `Action`2 <>9__178_88`
- field `Func`1 <>9__178_89`
- field `Func`2 <>9__178_9`
- field `Func`2 <>9__178_90`
- field `Action`2 <>9__178_91`
- field `Func`1 <>9__178_92`
- field `Func`2 <>9__18_0`
- field `Action`2 <>9__18_1`
- field `Func`1 <>9__18_2`
- field `Func`2 <>9__18_3`
- field `Action`2 <>9__18_4`
- field `Func`1 <>9__18_5`
- field `Func`2 <>9__182_0`
- field `Func`1 <>9__182_2`
- field `Func`2 <>9__183_0`
- field `Action`2 <>9__183_1`
- field `Action`2 <>9__183_10`
- field `Func`1 <>9__183_11`
- field `Func`2 <>9__183_12`
- field `Action`2 <>9__183_13`
- field `Func`1 <>9__183_14`
- field `Func`2 <>9__183_15`
- field `Action`2 <>9__183_16`
- field `Func`1 <>9__183_17`
- field `Func`2 <>9__183_18`
- field `Action`2 <>9__183_19`
- field `Func`1 <>9__183_2`
- field `Func`1 <>9__183_20`
- field `Func`2 <>9__183_21`
- field `Action`2 <>9__183_22`
- field `Func`1 <>9__183_23`
- field `Func`2 <>9__183_24`
- field `Action`2 <>9__183_25`
- field `Func`1 <>9__183_26`
- field `Func`2 <>9__183_27`
- field `Action`2 <>9__183_28`
- field `Func`1 <>9__183_29`
- field `Func`2 <>9__183_3`
- field `Func`2 <>9__183_30`
- field `Action`2 <>9__183_31`
- field `Func`1 <>9__183_32`
- field `Func`2 <>9__183_33`
- field `Action`2 <>9__183_34`
- field `Func`1 <>9__183_35`
- field `Func`2 <>9__183_36`
- field `Action`2 <>9__183_37`
- field `Func`1 <>9__183_38`
- field `Func`2 <>9__183_39`
- field `Action`2 <>9__183_4`
- field `Action`2 <>9__183_40`
- field `Func`1 <>9__183_41`
- field `Func`2 <>9__183_42`
- field `Action`2 <>9__183_43`
- field `Func`1 <>9__183_44`
- field `Func`2 <>9__183_45`
- field `Action`2 <>9__183_46`
- field `Func`1 <>9__183_47`
- field `Func`1 <>9__183_5`
- field `Func`2 <>9__183_6`
- field `Action`2 <>9__183_7`
- field `Func`1 <>9__183_8`
- field `Func`2 <>9__183_9`
- field `Func`2 <>9__188_0`
- field `Func`1 <>9__188_2`
- field `Func`2 <>9__189_0`
- field `Action`2 <>9__189_1`
- field `Action`2 <>9__189_10`
- field `Func`1 <>9__189_11`
- field `Func`2 <>9__189_12`
- field `Action`2 <>9__189_13`
- field `Func`1 <>9__189_14`
- field `Func`2 <>9__189_15`
- field `Action`2 <>9__189_16`
- field `Func`1 <>9__189_17`
- field `Func`2 <>9__189_18`
- field `Action`2 <>9__189_19`
- field `Func`1 <>9__189_2`
- field `Func`1 <>9__189_20`
- field `Func`2 <>9__189_3`
- field `Action`2 <>9__189_4`
- field `Func`1 <>9__189_5`
- field `Func`2 <>9__189_6`
- field `Action`2 <>9__189_7`
- field `Func`1 <>9__189_8`
- field `Func`2 <>9__189_9`
- field `Func`2 <>9__194_0`
- field `Func`1 <>9__194_2`
- field `Func`2 <>9__195_0`
- field `Action`2 <>9__195_1`
- field `Func`1 <>9__195_2`
- field `Func`2 <>9__195_3`
- field `Action`2 <>9__195_4`
- field `Func`1 <>9__195_5`
- field `Func`2 <>9__195_6`
- field `Action`2 <>9__195_7`
- field `Func`1 <>9__195_8`
- field `Func`1 <>9__195_9`
- field `Func`2 <>9__200_0`
- field `Func`1 <>9__200_2`
- field `Func`2 <>9__201_0`
- field `Action`2 <>9__201_1`
- field `Func`1 <>9__201_2`
- field `Func`2 <>9__201_3`
- field `Action`2 <>9__201_4`
- field `Func`1 <>9__201_5`
- field `Func`1 <>9__201_6`
- field `Func`1 <>9__201_7`
- field `Func`1 <>9__201_8`
- field `Func`2 <>9__206_0`
- field `Func`1 <>9__206_2`
- field `Func`2 <>9__207_0`
- field `Action`2 <>9__207_1`
- field `Func`1 <>9__207_2`
- field `Func`2 <>9__207_3`
- field `Action`2 <>9__207_4`
- field `Func`1 <>9__207_5`
- field `Func`2 <>9__207_6`
- field `Action`2 <>9__207_7`
- field `Func`1 <>9__207_8`
- field `Func`2 <>9__212_0`
- field `Func`1 <>9__212_2`
- field `Func`2 <>9__213_0`
- field `Action`2 <>9__213_1`
- field `Action`2 <>9__213_10`
- field `Func`1 <>9__213_11`
- field `Func`2 <>9__213_12`
- field `Action`2 <>9__213_13`
- field `Func`1 <>9__213_14`
- field `Func`1 <>9__213_2`
- field `Func`2 <>9__213_3`
- field `Action`2 <>9__213_4`
- field `Func`1 <>9__213_5`
- field `Func`2 <>9__213_6`
- field `Action`2 <>9__213_7`
- field `Func`1 <>9__213_8`
- field `Func`2 <>9__213_9`
- field `Func`2 <>9__218_0`
- field `Func`1 <>9__218_2`
- field `Func`2 <>9__219_0`
- field `Action`2 <>9__219_1`
- field `Action`2 <>9__219_10`
- field `Func`1 <>9__219_11`
- field `Func`2 <>9__219_12`
- field `Action`2 <>9__219_13`
- field `Func`1 <>9__219_14`
- field `Func`2 <>9__219_15`
- field `Action`2 <>9__219_16`
- field `Func`1 <>9__219_17`
- field `Func`2 <>9__219_18`
- field `Action`2 <>9__219_19`
- field `Func`1 <>9__219_2`
- field `Func`1 <>9__219_20`
- field `Func`2 <>9__219_21`
- field `Action`2 <>9__219_22`
- field `Func`1 <>9__219_23`
- field `Func`2 <>9__219_24`
- field `Action`2 <>9__219_25`
- field `Func`1 <>9__219_26`
- field `Func`2 <>9__219_27`
- field `Action`2 <>9__219_28`
- field `Func`1 <>9__219_29`
- field `Func`2 <>9__219_3`
- field `Action`2 <>9__219_4`
- field `Func`1 <>9__219_5`
- field `Func`2 <>9__219_6`
- field `Action`2 <>9__219_7`
- field `Func`1 <>9__219_8`
- field `Func`2 <>9__219_9`
- field `Func`1 <>9__22_0`
- field `Func`1 <>9__22_2`
- field `Func`2 <>9__224_0`
- field `Func`1 <>9__224_2`
- field `Func`2 <>9__225_0`
- field `Action`2 <>9__225_1`
- field `Func`1 <>9__225_2`
- field `Func`2 <>9__225_3`
- field `Action`2 <>9__225_4`
- field `Func`1 <>9__225_5`
- field `Func`1 <>9__225_6`
- field `Func`1 <>9__225_7`
- field `Func`2 <>9__23_0`
- field `Action`2 <>9__23_1`
- field `Func`1 <>9__23_2`
- field `Func`2 <>9__23_3`
- field `Action`2 <>9__23_4`
- field `Func`1 <>9__23_5`
- field `Func`2 <>9__230_0`
- field `Func`1 <>9__230_2`
- field `Func`2 <>9__231_0`
- field `Action`2 <>9__231_1`
- field `Func`1 <>9__231_2`
- field `Func`2 <>9__231_3`
- field `Action`2 <>9__231_4`
- field `Func`1 <>9__231_5`
- field `Func`1 <>9__231_6`
- field `Func`1 <>9__231_7`
- field `Func`2 <>9__240_0`
- field `Func`1 <>9__240_2`
- field `Func`2 <>9__241_0`
- field `Action`2 <>9__241_1`
- field `Func`1 <>9__241_2`
- field `Func`2 <>9__241_3`
- field `Action`2 <>9__241_4`
- field `Func`1 <>9__241_5`
- field `Func`2 <>9__241_6`
- field `Action`2 <>9__241_7`
- field `Func`1 <>9__241_8`
- field `Func`1 <>9__246_0`
- field `Func`1 <>9__246_2`
- field `Func`2 <>9__247_0`
- field `Action`2 <>9__247_1`
- field `Func`1 <>9__247_2`
- field `Func`2 <>9__247_3`
- field `Action`2 <>9__247_4`
- field `Func`1 <>9__247_5`
- field `Func`1 <>9__251_0`
- field `Func`1 <>9__251_2`
- field `Func`2 <>9__252_0`
- field `Action`2 <>9__252_1`
- field `Func`1 <>9__252_2`
- field `Func`1 <>9__256_0`
- field `Func`1 <>9__256_2`
- field `Func`2 <>9__257_0`
- field `Action`2 <>9__257_1`
- field `Func`1 <>9__257_2`
- field `Func`2 <>9__257_3`
- field `Action`2 <>9__257_4`
- field `Func`1 <>9__257_5`
- field `Func`1 <>9__266_0`
- field `Func`1 <>9__266_2`
- field `Func`2 <>9__267_0`
- field `Action`2 <>9__267_1`
- field `Action`2 <>9__267_10`
- field `Func`1 <>9__267_11`
- field `Func`2 <>9__267_12`
- field `Action`2 <>9__267_13`
- field `Func`1 <>9__267_14`
- field `Func`2 <>9__267_15`
- field `Action`2 <>9__267_16`
- field `Func`1 <>9__267_17`
- field `Func`2 <>9__267_18`
- field `Action`2 <>9__267_19`
- field `Func`1 <>9__267_2`
- field `Func`1 <>9__267_20`
- field `Func`2 <>9__267_21`
- field `Action`2 <>9__267_22`
- field `Func`1 <>9__267_23`
- field `Func`2 <>9__267_24`
- field `Action`2 <>9__267_25`
- field `Func`1 <>9__267_26`
- field `Func`2 <>9__267_27`
- field `Action`2 <>9__267_28`
- field `Func`1 <>9__267_29`
- field `Func`2 <>9__267_3`
- field `Func`2 <>9__267_30`
- field `Action`2 <>9__267_31`
- field `Func`1 <>9__267_32`
- field `Action`2 <>9__267_4`
- field `Func`1 <>9__267_5`
- field `Func`2 <>9__267_6`
- field `Action`2 <>9__267_7`
- field `Func`1 <>9__267_8`
- field `Func`2 <>9__267_9`
- field `Func`1 <>9__271_0`
- field `Func`1 <>9__271_2`
- field `Func`2 <>9__272_0`
- field `Action`2 <>9__272_1`
- field `Func`1 <>9__272_2`
- field `Func`2 <>9__272_3`
- field `Action`2 <>9__272_4`
- field `Func`1 <>9__272_5`
- field `Func`1 <>9__276_0`
- field `Func`1 <>9__276_2`
- field `Func`2 <>9__277_0`
- field `Action`2 <>9__277_1`
- field `Action`2 <>9__277_10`
- field `Func`1 <>9__277_11`
- field `Func`2 <>9__277_12`
- field `Action`2 <>9__277_13`
- field `Func`1 <>9__277_14`
- field `Func`2 <>9__277_15`
- field `Action`2 <>9__277_16`
- field `Func`1 <>9__277_17`
- field `Func`2 <>9__277_18`
- field `Action`2 <>9__277_19`
- field `Func`1 <>9__277_2`
- field `Func`1 <>9__277_20`
- field `Func`2 <>9__277_3`
- field `Action`2 <>9__277_4`
- field `Func`1 <>9__277_5`
- field `Func`2 <>9__277_6`
- field `Action`2 <>9__277_7`
- field `Func`1 <>9__277_8`
- field `Func`2 <>9__277_9`
- field `Func`1 <>9__281_0`
- field `Func`1 <>9__281_2`
- field `Func`2 <>9__282_0`
- field `Action`2 <>9__282_1`
- field `Func`1 <>9__282_2`
- field `Func`2 <>9__282_3`
- field `Action`2 <>9__282_4`
- field `Func`1 <>9__282_5`
- field `Func`1 <>9__286_0`
- field `Func`1 <>9__286_2`
- field `Func`2 <>9__287_0`
- field `Action`2 <>9__287_1`
- field `Func`1 <>9__287_2`
- field `Func`2 <>9__287_3`
- field `Action`2 <>9__287_4`
- field `Func`1 <>9__287_5`
- field `Func`1 <>9__291_0`
- field `Func`1 <>9__291_2`
- field `Func`2 <>9__292_0`
- field `Action`2 <>9__292_1`
- field `Func`1 <>9__292_2`
- field `Func`2 <>9__292_3`
- field `Action`2 <>9__292_4`
- field `Func`1 <>9__292_5`
- field `Func`1 <>9__296_0`
- field `Func`1 <>9__296_2`
- field `Func`2 <>9__297_0`
- field `Action`2 <>9__297_1`
- field `Func`1 <>9__297_2`
- field `Func`2 <>9__297_3`
- field `Action`2 <>9__297_4`
- field `Func`1 <>9__297_5`
- field `Func`1 <>9__301_0`
- field `Func`1 <>9__301_2`
- field `Func`2 <>9__302_0`
- field `Action`2 <>9__302_1`
- field `Func`1 <>9__302_2`
- field `Func`2 <>9__302_3`
- field `Action`2 <>9__302_4`
- field `Func`1 <>9__302_5`
- field `Func`1 <>9__306_0`
- field `Func`1 <>9__306_2`
- field `Func`2 <>9__307_0`
- field `Action`2 <>9__307_1`
- field `Func`1 <>9__307_2`
- field `Func`2 <>9__307_3`
- field `Action`2 <>9__307_4`
- field `Func`1 <>9__307_5`
- field `Func`1 <>9__311_0`
- field `Func`1 <>9__311_2`
- field `Func`2 <>9__312_0`
- field `Action`2 <>9__312_1`
- field `Func`1 <>9__312_2`
- field `Func`2 <>9__312_3`
- field `Action`2 <>9__312_4`
- field `Func`1 <>9__312_5`
- field `Func`1 <>9__316_0`
- field `Func`1 <>9__316_2`
- field `Func`2 <>9__317_0`
- field `Action`2 <>9__317_1`
- field `Action`2 <>9__317_10`
- field `Func`1 <>9__317_11`
- field `Func`2 <>9__317_12`
- field `Action`2 <>9__317_13`
- field `Func`1 <>9__317_14`
- field `Func`2 <>9__317_15`
- field `Action`2 <>9__317_16`
- field `Func`1 <>9__317_17`
- field `Func`2 <>9__317_18`
- field `Action`2 <>9__317_19`
- field `Func`1 <>9__317_2`
- field `Func`1 <>9__317_20`
- field `Func`2 <>9__317_3`
- field `Action`2 <>9__317_4`
- field `Func`1 <>9__317_5`
- field `Func`2 <>9__317_6`
- field `Action`2 <>9__317_7`
- field `Func`1 <>9__317_8`
- field `Func`2 <>9__317_9`
- field `Func`1 <>9__321_0`
- field `Func`1 <>9__321_2`
- field `Func`2 <>9__322_0`
- field `Action`2 <>9__322_1`
- field `Func`1 <>9__322_2`
- field `Func`2 <>9__322_3`
- field `Action`2 <>9__322_4`
- field `Func`1 <>9__322_5`
- field `Func`2 <>9__322_6`
- field `Action`2 <>9__322_7`
- field `Func`1 <>9__322_8`
- field `Func`2 <>9__326_0`
- field `Func`1 <>9__326_2`
- field `Func`2 <>9__327_0`
- field `Action`2 <>9__327_1`
- field `Func`1 <>9__327_2`
- field `Func`2 <>9__327_3`
- field `Action`2 <>9__327_4`
- field `Func`1 <>9__327_5`
- field `Func`1 <>9__332_0`
- field `Func`1 <>9__332_2`
- field `Func`2 <>9__333_0`
- field `Action`2 <>9__333_1`
- field `Action`2 <>9__333_10`
- field `Func`1 <>9__333_11`
- field `Func`2 <>9__333_12`
- field `Action`2 <>9__333_13`
- field `Func`1 <>9__333_14`
- field `Func`1 <>9__333_2`
- field `Func`2 <>9__333_3`
- field `Action`2 <>9__333_4`
- field `Func`1 <>9__333_5`
- field `Func`2 <>9__333_6`
- field `Action`2 <>9__333_7`
- field `Func`1 <>9__333_8`
- field `Func`2 <>9__333_9`
- field `Func`1 <>9__341_0`
- field `Func`1 <>9__341_2`
- field `Func`2 <>9__342_0`
- field `Action`2 <>9__342_1`
- field `Func`1 <>9__342_2`
- field `Func`2 <>9__342_3`
- field `Action`2 <>9__342_4`
- field `Func`1 <>9__342_5`
- field `Func`2 <>9__342_6`
- field `Action`2 <>9__342_7`
- field `Func`1 <>9__342_8`
- field `Func`1 <>9__346_0`
- field `Func`1 <>9__346_2`
- field `Func`2 <>9__347_0`
- field `Action`2 <>9__347_1`
- field `Func`1 <>9__347_2`
- field `Func`2 <>9__347_3`
- field `Action`2 <>9__347_4`
- field `Func`1 <>9__347_5`
- field `Func`2 <>9__347_6`
- field `Action`2 <>9__347_7`
- field `Func`1 <>9__347_8`
- field `Func`1 <>9__351_0`
- field `Func`1 <>9__351_2`
- field `Func`2 <>9__352_0`
- field `Action`2 <>9__352_1`
- field `Action`2 <>9__352_10`
- field `Func`1 <>9__352_11`
- field `Func`1 <>9__352_2`
- field `Func`2 <>9__352_3`
- field `Action`2 <>9__352_4`
- field `Func`1 <>9__352_5`
- field `Func`2 <>9__352_6`
- field `Action`2 <>9__352_7`
- field `Func`1 <>9__352_8`
- field `Func`2 <>9__352_9`
- field `Func`1 <>9__356_0`
- field `Func`1 <>9__356_2`
- field `Func`2 <>9__357_0`
- field `Action`2 <>9__357_1`
- field `Func`1 <>9__357_2`
- field `Func`2 <>9__357_3`
- field `Action`2 <>9__357_4`
- field `Func`1 <>9__357_5`
- field `Func`1 <>9__361_0`
- field `Func`1 <>9__361_2`
- field `Func`2 <>9__362_0`
- field `Action`2 <>9__362_1`
- field `Action`2 <>9__362_10`
- field `Func`1 <>9__362_11`
- field `Func`2 <>9__362_12`
- field `Action`2 <>9__362_13`
- field `Func`1 <>9__362_14`
- field `Func`2 <>9__362_15`
- field `Action`2 <>9__362_16`
- field `Func`1 <>9__362_17`
- field `Func`2 <>9__362_18`
- field `Action`2 <>9__362_19`
- field `Func`1 <>9__362_2`
- field `Func`1 <>9__362_20`
- field `Func`2 <>9__362_21`
- field `Action`2 <>9__362_22`
- field `Func`1 <>9__362_23`
- field `Func`2 <>9__362_24`
- field `Action`2 <>9__362_25`
- field `Func`1 <>9__362_26`
- field `Func`2 <>9__362_27`
- field `Action`2 <>9__362_28`
- field `Func`1 <>9__362_29`
- field `Func`2 <>9__362_3`
- field `Func`2 <>9__362_30`
- field `Action`2 <>9__362_31`
- field `Func`1 <>9__362_32`
- field `Func`2 <>9__362_33`
- field `Action`2 <>9__362_34`
- field `Func`1 <>9__362_35`
- field `Func`2 <>9__362_36`
- field `Action`2 <>9__362_37`
- field `Func`1 <>9__362_38`
- field `Func`2 <>9__362_39`
- field `Action`2 <>9__362_4`
- field `Action`2 <>9__362_40`
- field `Func`1 <>9__362_41`
- field `Func`2 <>9__362_42`
- field `Action`2 <>9__362_43`
- field `Func`1 <>9__362_44`
- field `Func`2 <>9__362_45`
- field `Action`2 <>9__362_46`
- field `Func`1 <>9__362_47`
- field `Func`2 <>9__362_48`
- field `Action`2 <>9__362_49`
- field `Func`1 <>9__362_5`
- field `Func`1 <>9__362_50`
- field `Func`2 <>9__362_51`
- field `Action`2 <>9__362_52`
- field `Func`1 <>9__362_53`
- field `Func`2 <>9__362_54`
- field `Action`2 <>9__362_55`
- field `Func`1 <>9__362_56`
- field `Func`2 <>9__362_57`
- field `Action`2 <>9__362_58`
- field `Func`1 <>9__362_59`
- field `Func`2 <>9__362_6`
- field `Func`2 <>9__362_60`
- field `Action`2 <>9__362_61`
- field `Func`1 <>9__362_62`
- field `Action`2 <>9__362_7`
- field `Func`1 <>9__362_8`
- field `Func`2 <>9__362_9`
- field `Func`1 <>9__366_0`
- field `Func`1 <>9__366_2`
- field `Func`2 <>9__367_0`
- field `Action`2 <>9__367_1`
- field `Func`1 <>9__367_2`
- field `Func`2 <>9__367_3`
- field `Action`2 <>9__367_4`
- field `Func`1 <>9__367_5`
- field `Func`1 <>9__371_0`
- field `Func`1 <>9__371_2`
- field `Func`2 <>9__372_0`
- field `Action`2 <>9__372_1`
- field `Func`1 <>9__372_2`
- field `Func`2 <>9__372_3`
- field `Action`2 <>9__372_4`
- field `Func`1 <>9__372_5`
- field `Func`1 <>9__376_0`
- field `Func`1 <>9__376_2`
- field `Func`2 <>9__377_0`
- field `Action`2 <>9__377_1`
- field `Func`1 <>9__377_2`
- field `Func`2 <>9__377_3`
- field `Action`2 <>9__377_4`
- field `Func`1 <>9__377_5`
- field `Func`2 <>9__377_6`
- field `Action`2 <>9__377_7`
- field `Func`1 <>9__377_8`
- field `Func`1 <>9__381_0`
- field `Func`1 <>9__381_2`
- field `Func`2 <>9__382_0`
- field `Action`2 <>9__382_1`
- field `Func`1 <>9__382_2`
- field `Func`1 <>9__386_0`
- field `Func`1 <>9__386_2`
- field `Func`2 <>9__387_0`
- field `Action`2 <>9__387_1`
- field `Action`2 <>9__387_10`
- field `Func`1 <>9__387_11`
- field `Func`2 <>9__387_12`
- field `Action`2 <>9__387_13`
- field `Func`1 <>9__387_14`
- field `Func`2 <>9__387_15`
- field `Action`2 <>9__387_16`
- field `Func`1 <>9__387_17`
- field `Func`2 <>9__387_18`
- field `Action`2 <>9__387_19`
- field `Func`1 <>9__387_2`
- field `Func`1 <>9__387_20`
- field `Func`2 <>9__387_21`
- field `Action`2 <>9__387_22`
- field `Func`1 <>9__387_23`
- field `Func`2 <>9__387_24`
- field `Action`2 <>9__387_25`
- field `Func`1 <>9__387_26`
- field `Func`2 <>9__387_27`
- field `Action`2 <>9__387_28`
- field `Func`1 <>9__387_29`
- field `Func`2 <>9__387_3`
- field `Action`2 <>9__387_4`
- field `Func`1 <>9__387_5`
- field `Func`2 <>9__387_6`
- field `Action`2 <>9__387_7`
- field `Func`1 <>9__387_8`
- field `Func`2 <>9__387_9`
- field `Func`1 <>9__391_0`
- field `Func`1 <>9__391_2`
- field `Func`2 <>9__392_0`
- field `Action`2 <>9__392_1`
- field `Action`2 <>9__392_10`
- field `Func`1 <>9__392_11`
- field `Func`2 <>9__392_12`
- field `Action`2 <>9__392_13`
- field `Func`1 <>9__392_14`
- field `Func`2 <>9__392_15`
- field `Action`2 <>9__392_16`
- field `Func`1 <>9__392_17`
- field `Func`2 <>9__392_18`
- field `Action`2 <>9__392_19`
- field `Func`1 <>9__392_2`
- field `Func`1 <>9__392_20`
- field `Func`2 <>9__392_21`
- field `Action`2 <>9__392_22`
- field `Func`1 <>9__392_23`
- field `Func`2 <>9__392_24`
- field `Action`2 <>9__392_25`
- field `Func`1 <>9__392_26`
- field `Func`2 <>9__392_3`
- field `Action`2 <>9__392_4`
- field `Func`1 <>9__392_5`
- field `Func`2 <>9__392_6`
- field `Action`2 <>9__392_7`
- field `Func`1 <>9__392_8`
- field `Func`2 <>9__392_9`
- field `Func`1 <>9__396_0`
- field `Func`1 <>9__396_2`
- field `Func`2 <>9__397_0`
- field `Action`2 <>9__397_1`
- field `Action`2 <>9__397_10`
- field `Func`1 <>9__397_11`
- field `Func`2 <>9__397_12`
- field `Action`2 <>9__397_13`
- field `Func`1 <>9__397_14`
- field `Func`2 <>9__397_15`
- field `Action`2 <>9__397_16`
- field `Func`1 <>9__397_17`
- field `Func`2 <>9__397_18`
- field `Action`2 <>9__397_19`
- field `Func`1 <>9__397_2`
- field `Func`1 <>9__397_20`
- field `Func`2 <>9__397_21`
- field `Action`2 <>9__397_22`
- field `Func`1 <>9__397_23`
- field `Func`2 <>9__397_24`
- field `Action`2 <>9__397_25`
- field `Func`1 <>9__397_26`
- field `Func`2 <>9__397_27`
- field `Action`2 <>9__397_28`
- field `Func`1 <>9__397_29`
- field `Func`2 <>9__397_3`
- field `Action`2 <>9__397_4`
- field `Func`1 <>9__397_5`
- field `Func`2 <>9__397_6`
- field `Action`2 <>9__397_7`
- field `Func`1 <>9__397_8`
- field `Func`2 <>9__397_9`
- field `Func`1 <>9__401_0`
- field `Func`1 <>9__401_2`
- field `Func`2 <>9__402_0`
- field `Action`2 <>9__402_1`
- field `Action`2 <>9__402_10`
- field `Func`1 <>9__402_11`
- field `Func`1 <>9__402_2`
- field `Func`2 <>9__402_3`
- field `Action`2 <>9__402_4`
- field `Func`1 <>9__402_5`
- field `Func`2 <>9__402_6`
- field `Action`2 <>9__402_7`
- field `Func`1 <>9__402_8`
- field `Func`2 <>9__402_9`
- field `Func`1 <>9__406_0`
- field `Func`1 <>9__406_2`
- field `Func`2 <>9__407_0`
- field `Action`2 <>9__407_1`
- field `Func`1 <>9__407_2`
- field `Func`2 <>9__407_3`
- field `Action`2 <>9__407_4`
- field `Func`1 <>9__407_5`
- field `Func`2 <>9__411_0`
- field `Func`1 <>9__411_2`
- field `Func`2 <>9__412_0`
- field `Func`1 <>9__412_1`
- field `Func`2 <>9__412_2`
- field `Action`2 <>9__412_3`
- field `Func`1 <>9__412_4`
- field `Func`2 <>9__412_5`
- field `Action`2 <>9__412_6`
- field `Func`1 <>9__412_7`
- field `Func`1 <>9__417_0`
- field `Func`1 <>9__417_2`
- field `Func`2 <>9__418_0`
- field `Action`2 <>9__418_1`
- field `Func`1 <>9__418_2`
- field `Func`2 <>9__418_3`
- field `Action`2 <>9__418_4`
- field `Func`1 <>9__418_5`
- field `Func`1 <>9__422_0`
- field `Func`1 <>9__422_2`
- field `Func`2 <>9__423_0`
- field `Action`2 <>9__423_1`
- field `Func`1 <>9__423_2`
- field `Func`2 <>9__423_3`
- field `Action`2 <>9__423_4`
- field `Func`1 <>9__423_5`
- field `Func`2 <>9__427_0`
- field `Func`1 <>9__427_2`
- field `Func`2 <>9__428_0`
- field `Action`2 <>9__428_1`
- field `Action`2 <>9__428_10`
- field `Func`1 <>9__428_11`
- field `Func`2 <>9__428_12`
- field `Action`2 <>9__428_13`
- field `Func`1 <>9__428_14`
- field `Func`2 <>9__428_15`
- field `Action`2 <>9__428_16`
- field `Func`1 <>9__428_17`
- field `Func`2 <>9__428_18`
- field `Action`2 <>9__428_19`
- field `Func`1 <>9__428_2`
- field `Func`1 <>9__428_20`
- field `Func`2 <>9__428_21`
- field `Action`2 <>9__428_22`
- field `Func`1 <>9__428_23`
- field `Func`2 <>9__428_24`
- field `Action`2 <>9__428_25`
- field `Func`1 <>9__428_26`
- field `Func`2 <>9__428_27`
- field `Action`2 <>9__428_28`
- field `Func`1 <>9__428_29`
- field `Func`2 <>9__428_3`
- field `Func`2 <>9__428_30`
- field `Action`2 <>9__428_31`
- field `Func`1 <>9__428_32`
- field `Func`2 <>9__428_33`
- field `Action`2 <>9__428_34`
- field `Func`1 <>9__428_35`
- field `Func`2 <>9__428_36`
- field `Action`2 <>9__428_37`
- field `Func`1 <>9__428_38`
- field `Func`2 <>9__428_39`
- field `Action`2 <>9__428_4`
- field `Action`2 <>9__428_40`
- field `Func`1 <>9__428_41`
- field `Func`2 <>9__428_42`
- field `Action`2 <>9__428_43`
- field `Func`1 <>9__428_44`
- field `Func`2 <>9__428_45`
- field `Action`2 <>9__428_46`
- field `Func`1 <>9__428_47`
- field `Func`2 <>9__428_48`
- field `Action`2 <>9__428_49`
- field `Func`1 <>9__428_5`
- field `Func`1 <>9__428_50`
- field `Func`2 <>9__428_51`
- field `Action`2 <>9__428_52`
- field `Func`1 <>9__428_53`
- field `Func`2 <>9__428_54`
- field `Action`2 <>9__428_55`
- field `Func`1 <>9__428_56`
- field `Func`2 <>9__428_57`
- field `Action`2 <>9__428_58`
- field `Func`1 <>9__428_59`
- field `Func`2 <>9__428_6`
- field `Func`2 <>9__428_60`
- field `Action`2 <>9__428_61`
- field `Func`1 <>9__428_62`
- field `Func`2 <>9__428_63`
- field `Action`2 <>9__428_64`
- field `Func`1 <>9__428_65`
- field `Func`2 <>9__428_66`
- field `Action`2 <>9__428_67`
- field `Func`1 <>9__428_68`
- field `Func`2 <>9__428_69`
- field `Action`2 <>9__428_7`
- field `Action`2 <>9__428_70`
- field `Func`1 <>9__428_71`
- field `Func`2 <>9__428_72`
- field `Action`2 <>9__428_73`
- field `Func`1 <>9__428_74`
- field `Func`2 <>9__428_75`
- field `Action`2 <>9__428_76`
- field `Func`1 <>9__428_77`
- field `Func`1 <>9__428_78`
- field `Func`1 <>9__428_79`
- field `Func`1 <>9__428_8`
- field `Func`1 <>9__428_80`
- field `Func`1 <>9__428_81`
- field `Func`1 <>9__428_82`
- field `Func`2 <>9__428_9`
- field `Func`2 <>9__43_0`
- field `Func`1 <>9__43_2`
- field `Func`1 <>9__433_0`
- field `Func`1 <>9__433_2`
- field `Func`2 <>9__434_0`
- field `Action`2 <>9__434_1`
- field `Action`2 <>9__434_10`
- field `Func`1 <>9__434_11`
- field `Func`2 <>9__434_12`
- field `Action`2 <>9__434_13`
- field `Func`1 <>9__434_14`
- field `Func`2 <>9__434_15`
- field `Action`2 <>9__434_16`
- field `Func`1 <>9__434_17`
- field `Func`2 <>9__434_18`
- field `Action`2 <>9__434_19`
- field `Func`1 <>9__434_2`
- field `Func`1 <>9__434_20`
- field `Func`2 <>9__434_21`
- field `Action`2 <>9__434_22`
- field `Func`1 <>9__434_23`
- field `Func`2 <>9__434_24`
- field `Action`2 <>9__434_25`
- field `Func`1 <>9__434_26`
- field `Func`2 <>9__434_27`
- field `Action`2 <>9__434_28`
- field `Func`1 <>9__434_29`
- field `Func`2 <>9__434_3`
- field `Func`2 <>9__434_30`
- field `Action`2 <>9__434_31`
- field `Func`1 <>9__434_32`
- field `Func`2 <>9__434_33`
- field `Action`2 <>9__434_34`
- field `Func`1 <>9__434_35`
- field `Func`2 <>9__434_36`
- field `Action`2 <>9__434_37`
- field `Func`1 <>9__434_38`
- field `Func`2 <>9__434_39`
- field `Action`2 <>9__434_4`
- field `Action`2 <>9__434_40`
- field `Func`1 <>9__434_41`
- field `Func`2 <>9__434_42`
- field `Action`2 <>9__434_43`
- field `Func`1 <>9__434_44`
- field `Func`2 <>9__434_45`
- field `Action`2 <>9__434_46`
- field `Func`1 <>9__434_47`
- field `Func`2 <>9__434_48`
- field `Action`2 <>9__434_49`
- field `Func`1 <>9__434_5`
- field `Func`1 <>9__434_50`
- field `Func`2 <>9__434_51`
- field `Action`2 <>9__434_52`
- field `Func`1 <>9__434_53`
- field `Func`2 <>9__434_54`
- field `Action`2 <>9__434_55`
- field `Func`1 <>9__434_56`
- field `Func`2 <>9__434_57`
- field `Action`2 <>9__434_58`
- field `Func`1 <>9__434_59`
- field `Func`2 <>9__434_6`
- field `Func`2 <>9__434_60`
- field `Action`2 <>9__434_61`
- field `Func`1 <>9__434_62`
- field `Func`2 <>9__434_63`
- field `Action`2 <>9__434_64`
- field `Func`1 <>9__434_65`
- field `Action`2 <>9__434_7`
- field `Func`1 <>9__434_8`
- field `Func`2 <>9__434_9`
- field `Func`2 <>9__438_0`
- field `Func`1 <>9__438_2`
- field `Func`2 <>9__439_0`
- field `Action`2 <>9__439_1`
- field `Func`1 <>9__439_2`
- field `Func`2 <>9__439_3`
- field `Action`2 <>9__439_4`
- field `Func`1 <>9__439_5`
- field `Func`2 <>9__44_0`
- field `Func`1 <>9__44_1`
- field `Func`2 <>9__44_2`
- field `Func`1 <>9__44_3`
- field `Func`1 <>9__44_4`
- field `Func`1 <>9__44_5`
- field `Func`1 <>9__444_0`
- field `Func`1 <>9__444_2`
- field `Func`2 <>9__445_0`
- field `Action`2 <>9__445_1`
- field `Action`2 <>9__445_10`
- field `Func`1 <>9__445_11`
- field `Func`2 <>9__445_12`
- field `Action`2 <>9__445_13`
- field `Func`1 <>9__445_14`
- field `Func`2 <>9__445_15`
- field `Action`2 <>9__445_16`
- field `Func`1 <>9__445_17`
- field `Func`2 <>9__445_18`
- field `Action`2 <>9__445_19`
- field `Func`1 <>9__445_2`
- field `Func`1 <>9__445_20`
- field `Func`2 <>9__445_21`
- field `Action`2 <>9__445_22`
- field `Func`1 <>9__445_23`
- field `Func`2 <>9__445_24`
- field `Action`2 <>9__445_25`
- field `Func`1 <>9__445_26`
- field `Func`2 <>9__445_27`
- field `Action`2 <>9__445_28`
- field `Func`1 <>9__445_29`
- field `Func`2 <>9__445_3`
- field `Func`2 <>9__445_30`
- field `Action`2 <>9__445_31`
- field `Func`1 <>9__445_32`
- field `Func`2 <>9__445_33`
- field `Action`2 <>9__445_34`
- field `Func`1 <>9__445_35`
- field `Func`2 <>9__445_36`
- field `Action`2 <>9__445_37`
- field `Func`1 <>9__445_38`
- field `Func`2 <>9__445_39`
- field `Action`2 <>9__445_4`
- field `Action`2 <>9__445_40`
- field `Func`1 <>9__445_41`
- field `Func`2 <>9__445_42`
- field `Action`2 <>9__445_43`
- field `Func`1 <>9__445_44`
- field `Func`2 <>9__445_45`
- field `Action`2 <>9__445_46`
- field `Func`1 <>9__445_47`
- field `Func`2 <>9__445_48`
- field `Action`2 <>9__445_49`
- field `Func`1 <>9__445_5`
- field `Func`1 <>9__445_50`
- field `Func`2 <>9__445_51`
- field `Action`2 <>9__445_52`
- field `Func`1 <>9__445_53`
- field `Func`2 <>9__445_54`
- field `Action`2 <>9__445_55`
- field `Func`1 <>9__445_56`
- field `Func`2 <>9__445_57`
- field `Action`2 <>9__445_58`
- field `Func`1 <>9__445_59`
- field `Func`2 <>9__445_6`
- field `Func`2 <>9__445_60`
- field `Action`2 <>9__445_61`
- field `Func`1 <>9__445_62`
- field `Func`2 <>9__445_63`
- field `Action`2 <>9__445_64`
- field `Func`1 <>9__445_65`
- field `Func`2 <>9__445_66`
- field `Action`2 <>9__445_67`
- field `Func`1 <>9__445_68`
- field `Action`2 <>9__445_7`
- field `Func`1 <>9__445_8`
- field `Func`2 <>9__445_9`
- field `Func`1 <>9__461_0`
- field `Func`1 <>9__461_2`
- field `Func`2 <>9__462_0`
- field `Action`2 <>9__462_1`
- field `Func`1 <>9__462_2`
- field `Func`2 <>9__462_3`
- field `Action`2 <>9__462_4`
- field `Func`1 <>9__462_5`
- field `Func`2 <>9__462_6`
- field `Action`2 <>9__462_7`
- field `Func`1 <>9__462_8`
- field `Func`1 <>9__466_0`
- field `Func`1 <>9__470_0`
- field `Func`1 <>9__474_0`
- field `Func`1 <>9__478_0`
- field `Func`1 <>9__482_0`
- field `Func`1 <>9__486_0`
- field `Func`1 <>9__49_0`
- field `Func`1 <>9__49_2`
- field `Func`2 <>9__50_0`
- field `Action`2 <>9__50_1`
- field `Func`1 <>9__50_2`
- field `Func`2 <>9__50_3`
- field `Action`2 <>9__50_4`
- field `Func`1 <>9__50_5`
- field `Func`1 <>9__506_0`
- field `Func`1 <>9__510_0`
- field `Func`1 <>9__514_0`
- field `Func`1 <>9__518_0`
- field `Func`1 <>9__522_0`
- field `Func`1 <>9__526_0`
- field `Func`1 <>9__530_0`
- field `Func`1 <>9__534_0`
- field `Func`1 <>9__538_0`
- field `Func`1 <>9__542_0`
- field `Func`1 <>9__546_0`
- field `Func`1 <>9__550_0`
- field `Func`1 <>9__554_0`
- field `Func`1 <>9__558_0`
- field `Func`1 <>9__562_0`
- field `Func`1 <>9__566_0`
- field `Func`1 <>9__570_0`
- field `Func`1 <>9__574_0`
- field `Func`1 <>9__578_0`
- field `Func`1 <>9__58_0`
- field `Func`1 <>9__58_2`
- field `Func`1 <>9__582_0`
- field `Func`1 <>9__586_0`
- field `Func`2 <>9__59_0`
- field `Action`2 <>9__59_1`
- field `Action`2 <>9__59_10`
- field `Func`1 <>9__59_11`
- field `Func`2 <>9__59_12`
- field `Action`2 <>9__59_13`
- field `Func`1 <>9__59_14`
- field `Func`2 <>9__59_15`
- field `Action`2 <>9__59_16`
- field `Func`1 <>9__59_17`
- field `Func`2 <>9__59_18`
- field `Action`2 <>9__59_19`
- field `Func`1 <>9__59_2`
- field `Func`1 <>9__59_20`
- field `Func`2 <>9__59_21`
- field `Action`2 <>9__59_22`
- field `Func`1 <>9__59_23`
- field `Func`2 <>9__59_24`
- field `Action`2 <>9__59_25`
- field `Func`1 <>9__59_26`
- field `Func`2 <>9__59_3`
- field `Action`2 <>9__59_4`
- field `Func`1 <>9__59_5`
- field `Func`2 <>9__59_6`
- field `Action`2 <>9__59_7`
- field `Func`1 <>9__59_8`
- field `Func`2 <>9__59_9`
- field `Func`1 <>9__590_0`
- field `Func`1 <>9__594_0`
- field `Func`1 <>9__598_0`
- field `Func`1 <>9__602_0`
- field `Func`1 <>9__606_0`
- field `Func`1 <>9__610_0`
- field `Func`1 <>9__614_0`
- field `Func`1 <>9__618_0`
- field `Func`1 <>9__622_0`
- field `Func`1 <>9__626_0`
- field `Func`1 <>9__63_0`
- field `Func`1 <>9__63_2`
- field `Func`1 <>9__630_0`
- field `Func`1 <>9__634_0`
- field `Func`1 <>9__638_0`
- field `Func`2 <>9__64_0`
- field `Action`2 <>9__64_1`
- field `Func`1 <>9__64_2`
- field `Func`2 <>9__64_3`
- field `Action`2 <>9__64_4`
- field `Func`1 <>9__64_5`
- field `Func`1 <>9__642_0`
- field `Func`1 <>9__646_0`
- field `Func`1 <>9__650_0`
- field `Func`1 <>9__654_0`
- field `Func`1 <>9__658_0`
- field `Func`1 <>9__662_0`
- field `Func`1 <>9__666_0`
- field `Func`1 <>9__670_0`
- field `Func`1 <>9__674_0`
- field `Func`1 <>9__678_0`
- field `Func`1 <>9__682_0`
- field `Func`1 <>9__686_0`
- field `Func`1 <>9__690_0`
- field `Func`1 <>9__694_0`
- field `Func`1 <>9__698_0`
- field `Func`1 <>9__702_0`
- field `Func`1 <>9__706_0`
- field `Func`1 <>9__72_0`
- field `Func`1 <>9__72_2`
- field `Func`2 <>9__73_0`
- field `Action`2 <>9__73_1`
- field `Func`1 <>9__73_2`
- field `Func`2 <>9__73_3`
- field `Action`2 <>9__73_4`
- field `Func`1 <>9__73_5`
- field `Func`2 <>9__73_6`
- field `Action`2 <>9__73_7`
- field `Func`1 <>9__73_8`
- field `Func`2 <>9__81_0`
- field `Func`1 <>9__81_2`
- field `Func`2 <>9__82_0`
- field `Func`1 <>9__82_1`
- field `Func`2 <>9__82_2`
- field `Func`1 <>9__82_3`
- field `Func`1 <>9__87_0`
- field `Func`1 <>9__87_2`
- field `Func`2 <>9__88_0`
- field `Action`2 <>9__88_1`
- field `Action`2 <>9__88_10`
- field `Func`1 <>9__88_11`
- field `Func`2 <>9__88_12`
- field `Action`2 <>9__88_13`
- field `Func`1 <>9__88_14`
- field `Func`2 <>9__88_15`
- field `Action`2 <>9__88_16`
- field `Func`1 <>9__88_17`
- field `Func`2 <>9__88_18`
- field `Action`2 <>9__88_19`
- field `Func`1 <>9__88_2`
- field `Func`1 <>9__88_20`
- field `Func`2 <>9__88_21`
- field `Action`2 <>9__88_22`
- field `Func`1 <>9__88_23`
- field `Func`2 <>9__88_24`
- field `Action`2 <>9__88_25`
- field `Func`1 <>9__88_26`
- field `Func`2 <>9__88_3`
- field `Action`2 <>9__88_4`
- field `Func`1 <>9__88_5`
- field `Func`2 <>9__88_6`
- field `Action`2 <>9__88_7`
- field `Func`1 <>9__88_8`
- field `Func`2 <>9__88_9`
- field `Func`1 <>9__92_0`
- field `Func`1 <>9__92_2`
- field `Func`2 <>9__93_0`
- field `Action`2 <>9__93_1`
- field `Func`1 <>9__93_2`
- field `Func`2 <>9__93_3`
- field `Action`2 <>9__93_4`
- field `Func`1 <>9__93_5`
- field `Func`1 <>9__97_0`
- field `Func`1 <>9__97_2`
- field `Func`2 <>9__98_0`
- field `Action`2 <>9__98_1`
- field `Func`1 <>9__98_2`
- field `Func`2 <>9__98_3`
- field `Action`2 <>9__98_4`
- field `Func`1 <>9__98_5`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass102_0
- method `<Create_NullLeaderboardFileEntry>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass108_0
- method `<Create_NullMultiplayerName>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass137_0
- method `<Create_AncientChoiceHistoryEntry>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass142_0
- method `<Create_CardChoiceHistoryEntry>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass147_0
- method `<Create_CardEnchantmentHistoryEntry>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass152_0
- method `<Create_CardTransformationHistoryEntry>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass157_0
- method `<Create_EventOptionHistoryEntry>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass162_0
- method `<Create_MapPointHistoryEntry>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass167_0
- method `<Create_MapPointRoomHistoryEntry>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass17_0
- method `<Create_Vector2>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass172_0
- method `<Create_ModelChoiceHistoryEntry>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass177_0
- method `<Create_PlayerMapPointHistoryEntry>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass182_0
- method `<Create_RunHistory>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass188_0
- method `<Create_RunHistoryPlayer>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass194_0
- method `<Create_AncientCharacterStats>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass200_0
- method `<Create_AncientStats>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass206_0
- method `<Create_BadgeStats>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass212_0
- method `<Create_CardStats>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass218_0
- method `<Create_CharacterStats>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass22_0
- method `<Create_Vector2I>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass224_0
- method `<Create_EncounterStats>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass230_0
- method `<Create_EnemyStats>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass240_0
- method `<Create_FightStats>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass246_0
- method `<Create_SerializableMapDrawingLine>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass251_0
- method `<Create_SerializableMapDrawings>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass256_0
- method `<Create_SerializablePlayerMapDrawings>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass261_0
- method `<Create_MigratingData>b__0(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass266_0
- method `<Create_PrefsSave>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass271_0
- method `<Create_ProfileSave>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass276_0
- method `<Create_SavedProperties>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass281_0
- method `<Create_SavedPropertyBoolean>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass286_0
- method `<Create_SavedPropertyModelId>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass291_0
- method `<Create_SavedPropertySerializableCardArray>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass296_0
- method `<Create_SavedPropertySerializableCard>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass301_0
- method `<Create_SavedPropertyInt32Array>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass306_0
- method `<Create_SavedPropertyInt32>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass311_0
- method `<Create_SavedPropertyString>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass316_0
- method `<Create_SerializableActMap>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass321_0
- method `<Create_SerializableActModel>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass326_0
- method `<Create_SerializableBadge>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass332_0
- method `<Create_SerializableCard>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass341_0
- method `<Create_SerializableEnchantment>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass346_0
- method `<Create_SerializableExtraRunFields>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass351_0
- method `<Create_SerializableMapPoint>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass356_0
- method `<Create_SerializableModifier>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass361_0
- method `<Create_SerializablePlayer>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass366_0
- method `<Create_SerializablePlayerOddsSet>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass371_0
- method `<Create_SerializablePotion>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass376_0
- method `<Create_SerializableRelic>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass381_0
- method `<Create_SerializableRelicGrabBag>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass386_0
- method `<Create_SerializableReward>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass391_0
- method `<Create_SerializableRoom>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass396_0
- method `<Create_SerializableRoomSet>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass401_0
- method `<Create_SerializableRunOddsSet>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass406_0
- method `<Create_SerializableRunRngSet>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass411_0
- method `<Create_SerializableEpoch>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass417_0
- method `<Create_SerializableExtraPlayerFields>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass422_0
- method `<Create_SerializablePlayerRngSet>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass427_0
- method `<Create_SerializableProgress>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass43_0
- method `<Create_LocString>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass433_0
- method `<Create_SerializableRun>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass438_0
- method `<Create_SerializableUnlockedAchievement>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass444_0
- method `<Create_SettingsSave>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass461_0
- method `<Create_SerializableUnlockState>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass49_0
- method `<Create_MapCoord>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass58_0
- method `<Create_ModManifest>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass63_0
- method `<Create_ModSettings>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass72_0
- method `<Create_SettingsSaveMod>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass81_0
- method `<Create_ModelId>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass87_0
- method `<Create_FeedbackData>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass92_0
- method `<Create_NullLeaderboard>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>c__DisplayClass97_0
- method `<Create_NullLeaderboardFile>b__1(JsonSerializerContext _)` -> `JsonPropertyInfo[]`
- field `JsonSerializerOptions options`

## MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext+<>O
- field `Func`1 <0>__LocStringCtorParamInit`
- field `Func`1 <1>__ModelIdCtorParamInit`
- field `Func`1 <10>__EncounterStatsCtorParamInit`
- field `Func`1 <11>__EnemyStatsCtorParamInit`
- field `Func`1 <12>__FightStatsCtorParamInit`
- field `Func`1 <13>__SerializableBadgeCtorParamInit`
- field `Func`1 <14>__SerializableEpochCtorParamInit`
- field `Func`1 <15>__SerializableProgressCtorParamInit`
- field `Func`1 <16>__SerializableUnlockedAchievementCtorParamInit`
- field `Func`1 <2>__NullLeaderboardFileEntryCtorParamInit`
- field `Func`1 <3>__RunHistoryCtorParamInit`
- field `Func`1 <4>__RunHistoryPlayerCtorParamInit`
- field `Func`1 <5>__AncientCharacterStatsCtorParamInit`
- field `Func`1 <6>__AncientStatsCtorParamInit`
- field `Func`1 <7>__BadgeStatsCtorParamInit`
- field `Func`1 <8>__CardStatsCtorParamInit`
- field `Func`1 <9>__CharacterStatsCtorParamInit`

## MegaCrit.Sts2.Core.Saves.Migrations.DuplicateMigrationException

## MegaCrit.Sts2.Core.Saves.Migrations.IMigration
- method `get_FromVersion()` -> `Int32`
- method `get_SaveType()` -> `Type`
- method `get_ToVersion()` -> `Int32`
- method `Migrate(MigratingData saveData)` -> `MigratingData`

## MegaCrit.Sts2.Core.Saves.Migrations.IMigration`1

## MegaCrit.Sts2.Core.Saves.Migrations.IMigrationSubtypes
- method `Get(Int32 i)` -> `Type`
- method `get_All()` -> `IReadOnlyList`1`
- method `get_Count()` -> `Int32`
- field `Type[] _subtypes`
- field `Type _t0`
- field `Type _t1`
- field `Type _t10`
- field `Type _t2`
- field `Type _t3`
- field `Type _t4`
- field `Type _t5`
- field `Type _t6`
- field `Type _t7`
- field `Type _t8`
- field `Type _t9`

## MegaCrit.Sts2.Core.Saves.Migrations.InvalidMigrationPathException

## MegaCrit.Sts2.Core.Saves.Migrations.MigratingData
- method `ConvertJsonNodeToObject(JsonNode node)` -> `Object`
- method `get_Item(String key)` -> `Object`
- method `GetAs(String key)` -> `T`
- method `GetAsOrNull(String key)` -> `Nullable`1`
- method `GetBool(String key)` -> `Boolean`
- method `GetInt(String key)` -> `Int32`
- method `GetList(String key)` -> `List`1`
- method `GetObject(String key)` -> `MigratingData`
- method `GetRawNode(String key)` -> `JsonNode`
- method `GetRawNode()` -> `JsonObject`
- method `GetString(String key)` -> `String`
- method `Has(String key)` -> `Boolean`
- method `Remove(String key)` -> `Void`
- method `Rename(String oldKey, String newKey)` -> `Void`
- method `Set(String key, T value)` -> `Void`
- method `SetList(String key, IEnumerable`1 items)` -> `Void`
- method `SetObject(String key, MigratingData value)` -> `Void`
- method `ToObject()` -> `T`
- field `JsonObject _data`

## MegaCrit.Sts2.Core.Saves.Migrations.MigrationAttribute
- method `get_FromVersion()` -> `Int32`
- method `get_SaveType()` -> `Type`
- method `get_ToVersion()` -> `Int32`
- field `Int32 <FromVersion>k__BackingField`
- field `Type <SaveType>k__BackingField`
- field `Int32 <ToVersion>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Migrations.MigrationBase`1
- method `<.ctor>b__1_0()` -> `MigrationAttribute`
- method `ApplyMigration(MigratingData saveData)` -> `Void`
- method `get_FromVersion()` -> `Int32`
- method `get_SaveType()` -> `Type`
- method `get_ToVersion()` -> `Int32`
- method `Migrate(MigratingData saveData)` -> `MigratingData`
- field `Lazy`1 _migrationAttribute`

## MegaCrit.Sts2.Core.Saves.Migrations.MigrationException

## MegaCrit.Sts2.Core.Saves.Migrations.MigrationManager
- method `CreateNewSave()` -> `T`
- method `DeriveAndSetLatestVersions()` -> `Void`
- method `EnsureVersionSet()` -> `Void`
- method `ExtractSchemaVersion(MigratingData json)` -> `Int32`
- method `GetCurrentVersion()` -> `Int32`
- method `GetDuplicateMigrationSources(Type saveType)` -> `List`1`
- method `GetGapsInMigrationPath(Type saveType)` -> `List`1`
- method `GetLatestVersion()` -> `Int32`
- method `GetMigration(Int32 fromVersion, Int32 toVersion)` -> `IMigration`
- method `GetMigrationsForType(Type saveType)` -> `IEnumerable`1`
- method `GetMinimumSupportedVersion()` -> `Int32`
- method `GetNextVersion(Int32 currentVersion)` -> `Nullable`1`
- method `GetRegisteredSaveTypes()` -> `IEnumerable`1`
- method `InferSchemaVersionFromStructure(MigratingData data)` -> `Nullable`1`
- method `Initialize()` -> `Void`
- method `IsMigrationPathValid(Type saveType)` -> `Void`
- method `LoadSave(String filePath)` -> `ReadSaveResult`1`
- method `LoadSaveFromPath(String filePath)` -> `ReadSaveResult`1`
- method `LoadWithAggressiveRecovery(String filePath, String content)` -> `ReadSaveResult`1`
- method `MigrateDataSequentially(MigratingData jsonObj)` -> `MigratingData`
- method `PreserveCorruptFile(String savePath, ReadSaveStatus status)` -> `Void`
- method `RecoverPartialDataFromCorruptSave(MigratingData data)` -> `T`
- method `RegisterMigration(IMigration migration)` -> `Void`
- method `RepairCommonJsonErrors(String json)` -> `String`
- method `SetMinimumSupportedVersion(Int32 version)` -> `Void`
- method `ShouldPreserveCorrupt(ReadSaveStatus status)` -> `Boolean`
- method `ValidateMigrationPaths()` -> `Void`
- field `Dictionary`2 _latestVersions`
- field `Dictionary`2 _minimumSupportedVersions`
- field `MigrationRegistry _registry`
- field `ISaveStore _saveStore`

## MegaCrit.Sts2.Core.Saves.Migrations.MigrationManager+<>c
- method `<DeriveAndSetLatestVersions>b__6_0(KeyValuePair`2 p)` -> `String`
- method `<DeriveAndSetLatestVersions>b__6_1(IMigration m)` -> `Int32`
- method `<GetDuplicateMigrationSources>b__11_0(IMigration m)` -> `Int32`
- method `<GetDuplicateMigrationSources>b__11_1(IGrouping`2 g)` -> `Boolean`
- method `<GetDuplicateMigrationSources>b__11_2(IGrouping`2 g)` -> `Int32`
- method `<GetGapsInMigrationPath>b__10_0(IMigration m)` -> `Int32`
- method `<GetGapsInMigrationPath>b__10_1(IMigration m)` -> `Int32`
- method `<RepairCommonJsonErrors>b__30_0(Char c)` -> `Boolean`
- method `<RepairCommonJsonErrors>b__30_1(Char c)` -> `Boolean`
- method `<RepairCommonJsonErrors>b__30_2(Char c)` -> `Boolean`
- method `<RepairCommonJsonErrors>b__30_3(Char c)` -> `Boolean`
- field `<>c <>9`
- field `Func`2 <>9__10_0`
- field `Func`2 <>9__10_1`
- field `Func`2 <>9__11_0`
- field `Func`2 <>9__11_1`
- field `Func`2 <>9__11_2`
- field `Func`2 <>9__30_0`
- field `Func`2 <>9__30_1`
- field `Func`2 <>9__30_2`
- field `Func`2 <>9__30_3`
- field `Func`2 <>9__6_0`
- field `Func`2 <>9__6_1`

## MegaCrit.Sts2.Core.Saves.Migrations.MigrationManager+<>c__DisplayClass17_0`1
- method `<GetMigration>b__0(IMigration m)` -> `Boolean`
- field `Int32 fromVersion`
- field `Int32 toVersion`

## MegaCrit.Sts2.Core.Saves.Migrations.MigrationManager+<>c__DisplayClass18_0`1
- method `<GetNextVersion>b__0(IMigration m)` -> `Boolean`
- field `Int32 versionToFind`

## MegaCrit.Sts2.Core.Saves.Migrations.MigrationPathGapException

## MegaCrit.Sts2.Core.Saves.Migrations.MigrationRegistry
- method `get_Migrations()` -> `Dictionary`2`
- method `RegisterAllMigrations(MigrationManager manager)` -> `Void`
- field `Dictionary`2 <Migrations>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Migrations.MissingSchemaVersionException

## MegaCrit.Sts2.Core.Saves.Migrations.PrefsSaves.PrefsSaveV1ToV2
- method `ApplyMigration(MigratingData saveData)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Migrations.ProfileSaves.ProfileSaveV1ToV2
- method `ApplyMigration(MigratingData saveData)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Migrations.ProgressSaves.ProgressSaveV20ToV21
- method `ApplyMigration(MigratingData saveData)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Migrations.RunHistories.RunHistoryV7ToV8
- method `ApplyMigration(MigratingData saveData)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Migrations.RunHistories.RunHistoryV8ToV9
- method `ApplyMigration(MigratingData saveData)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns.SerializableRunV12ToV13
- method `ApplyMigration(MigratingData saveData)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns.SerializableRunV13ToV14
- method `ApplyMigration(MigratingData saveData)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns.SerializableRunV14ToV15
- method `ApplyMigration(MigratingData saveData)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns.SerializableRunV15ToV16
- method `ApplyMigration(MigratingData saveData)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Migrations.SettingsSaves.SettingsSaveV3ToV4
- method `ApplyMigration(MigratingData saveData)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Migrations.SettingsSaves.SettingsSaveV4ToV5
- method `ApplyMigration(MigratingData saveData)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Migrations.Shared.SharedMigrationHelper
- method `MigrateMapPointHistoryCardChoices(JsonNode jsonNode)` -> `Void`
- method `MigrateMapPointHistoryRooms(JsonNode jsonNode)` -> `Void`
- method `RecursiveRemoveSchema(JsonNode node, Int32 depth)` -> `Void`
- method `ReplaceModelIds(JsonNode node, IReadOnlyDictionary`2 renames)` -> `Void`
- field `IReadOnlyDictionary`2 V100Renames`

## MegaCrit.Sts2.Core.Saves.Migrations.Shared.SharedMigrationHelper+<>c
- method `<ReplaceModelIds>b__1_0(KeyValuePair`2 kvp)` -> `String`
- field `<>c <>9`
- field `Func`2 <>9__1_0`

## MegaCrit.Sts2.Core.Saves.MigrationUtil
- method `ArchiveLegacyDirectory(String directoryPath, String archivePath)` -> `Void`
- method `ArchiveLegacyFile(String filePath, String archivePath)` -> `Void`
- method `CopyDirectoryRecursively(String sourceDir, String targetDir)` -> `Void`
- method `MigrateDirectory(String directoryName, String legacyBasePath, String newBasePath)` -> `Boolean`
- method `MigrateFile(String fileName, String legacyBasePath, String newBasePath)` -> `Boolean`

## MegaCrit.Sts2.Core.Saves.PrefsSave
- method `get_FastMode()` -> `FastModeType`
- method `get_IsLongPressEnabled()` -> `Boolean`
- method `get_MuteInBackground()` -> `Boolean`
- method `get_PhobiaMode()` -> `Boolean`
- method `get_SchemaVersion()` -> `Int32`
- method `get_ScreenShakeOptionIndex()` -> `Int32`
- method `get_ShowCardIndices()` -> `Boolean`
- method `get_ShowMultiplayerDrawings()` -> `Boolean`
- method `get_ShowRunTimer()` -> `Boolean`
- method `get_TextEffectsEnabled()` -> `Boolean`
- method `get_UploadData()` -> `Boolean`
- method `set_FastMode(FastModeType value)` -> `Void`
- method `set_IsLongPressEnabled(Boolean value)` -> `Void`
- method `set_MuteInBackground(Boolean value)` -> `Void`
- method `set_PhobiaMode(Boolean value)` -> `Void`
- method `set_SchemaVersion(Int32 value)` -> `Void`
- method `set_ScreenShakeOptionIndex(Int32 value)` -> `Void`
- method `set_ShowCardIndices(Boolean value)` -> `Void`
- method `set_ShowMultiplayerDrawings(Boolean value)` -> `Void`
- method `set_ShowRunTimer(Boolean value)` -> `Void`
- method `set_TextEffectsEnabled(Boolean value)` -> `Void`
- method `set_UploadData(Boolean value)` -> `Void`
- field `FastModeType <FastMode>k__BackingField`
- field `Boolean <IsLongPressEnabled>k__BackingField`
- field `Boolean <MuteInBackground>k__BackingField`
- field `Boolean <PhobiaMode>k__BackingField`
- field `Int32 <SchemaVersion>k__BackingField`
- field `Int32 <ScreenShakeOptionIndex>k__BackingField`
- field `Boolean <ShowCardIndices>k__BackingField`
- field `Boolean <ShowMultiplayerDrawings>k__BackingField`
- field `Boolean <ShowRunTimer>k__BackingField`
- field `Boolean <TextEffectsEnabled>k__BackingField`
- field `Boolean <UploadData>k__BackingField`

## MegaCrit.Sts2.Core.Saves.ProfileAccountScopeMigrator
- method `ArchiveLegacyData()` -> `Void`
- method `HasLegacyData()` -> `Boolean`
- method `MigrateToProfileScopedDirectories()` -> `Void`
- field `Boolean _migrationPerformed`

## MegaCrit.Sts2.Core.Saves.ProfileSave
- method `get_LastProfileId()` -> `Int32`
- method `get_SchemaVersion()` -> `Int32`
- method `set_LastProfileId(Int32 value)` -> `Void`
- method `set_SchemaVersion(Int32 value)` -> `Void`
- field `Int32 <LastProfileId>k__BackingField`
- field `Int32 <SchemaVersion>k__BackingField`

## MegaCrit.Sts2.Core.Saves.ProgressState
- method `AddUnlockedAchievement(Achievement achievement, Int64 unlockTime)` -> `Void`
- method `BuildAchievementLookup()` -> `Dictionary`2`
- method `ClampAncientCharacterStatsFields(List`1 charStats, DeserializationContext ctx)` -> `Void`
- method `ClampAscension(Int32 value, String fieldName, DeserializationContext ctx)` -> `Int32`
- method `ClampCardStatsFields(CardStats stats, DeserializationContext ctx)` -> `Void`
- method `ClampCharacterStatsFields(CharacterStats stats, DeserializationContext ctx)` -> `Void`
- method `ClampFightStatsFields(List`1 fightStats, DeserializationContext ctx)` -> `Void`
- method `ClampNonNegative(Int64 value, String fieldName, DeserializationContext ctx)` -> `Int64`
- method `ClampNonNegativeInt(Int32 value, String fieldName, DeserializationContext ctx)` -> `Int32`
- method `CreateDefault()` -> `ProgressState`
- method `FilterAndSortEpochs()` -> `Void`
- method `FixMissingSlots(List`1 epochs, DeserializationContext ctx)` -> `Void`
- method `FromSerializable(SerializableProgress save, DeserializationContext ctx)` -> `ProgressState`
- method `get_AncientStats()` -> `IReadOnlyDictionary`2`
- method `get_ArchitectDamage()` -> `Int64`
- method `get_BestWinStreak()` -> `Int64`
- method `get_CardStats()` -> `IReadOnlyDictionary`2`
- method `get_CharacterStats()` -> `IReadOnlyDictionary`2`
- method `get_CurrentScore()` -> `Int32`
- method `get_DiscoveredActs()` -> `IReadOnlySet`1`
- method `get_DiscoveredCards()` -> `IReadOnlySet`1`
- method `get_DiscoveredEvents()` -> `IReadOnlySet`1`
- method `get_DiscoveredPotions()` -> `IReadOnlySet`1`
- method `get_DiscoveredRelics()` -> `IReadOnlySet`1`
- method `get_EnableFtues()` -> `Boolean`
- method `get_EncounterStats()` -> `IReadOnlyDictionary`2`
- method `get_EnemyStats()` -> `IReadOnlyDictionary`2`
- method `get_Epochs()` -> `IReadOnlyList`1`
- method `get_FastestVictory()` -> `Int64`
- method `get_FloorsClimbed()` -> `Int64`
- method `get_FtueCompleted()` -> `IReadOnlySet`1`
- method `get_Losses()` -> `Int32`
- method `get_MaxMultiplayerAscension()` -> `Int32`
- method `get_NumberOfRuns()` -> `Int32`
- method `get_PendingCharacterUnlock()` -> `ModelId`
- method `get_PreferredMultiplayerAscension()` -> `Int32`
- method `get_TestSubjectKills()` -> `Int32`
- method `get_TotalPlaytime()` -> `Int64`
- method `get_TotalUnlocks()` -> `Int32`
- method `get_UniqueId()` -> `String`
- method `get_UnlockedAchievements()` -> `IReadOnlyDictionary`2`
- method `get_Wins()` -> `Int32`
- method `get_WongoPoints()` -> `Int32`
- method `GetOrCreateAncientStats(ModelId ancientId)` -> `AncientStats`
- method `GetOrCreateCardStats(ModelId cardId)` -> `CardStats`
- method `GetOrCreateCharacterStats(ModelId characterId)` -> `CharacterStats`
- method `GetOrCreateEncounterStats(ModelId encounterId)` -> `EncounterStats`
- method `GetOrCreateEnemyStats(ModelId enemyId)` -> `EnemyStats`
- method `GetStatsForAncient(ModelId ancientId)` -> `AncientStats`
- method `GetStatsForCharacter(ModelId characterId)` -> `CharacterStats`
- method `HasEpoch(String epochId)` -> `Boolean`
- method `IsAchievementUnlocked(Achievement achievement)` -> `Boolean`
- method `IsEpochObtained(String epochId)` -> `Boolean`
- method `IsEpochRevealed(String epochId)` -> `Boolean`
- method `MarkActAsSeen(ModelId actId)` -> `Boolean`
- method `MarkCardAsSeen(ModelId cardId)` -> `Boolean`
- method `MarkEventAsSeen(ModelId eventId)` -> `Boolean`
- method `MarkFtueAsComplete(String ftueId)` -> `Boolean`
- method `MarkPotionAsSeen(ModelId potionId)` -> `Boolean`
- method `MarkRelicAsSeen(ModelId relicId)` -> `Boolean`
- method `MergeAncientCharacterStatsList(List`1 existing, List`1 incoming)` -> `Void`
- method `MergeCardStats(CardStats existing, CardStats incoming)` -> `Void`
- method `MergeCharacterStats(CharacterStats existing, CharacterStats incoming)` -> `Void`
- method `MergeFastestWinTime(Int64 a, Int64 b)` -> `Int64`
- method `MergeFightStatsList(List`1 existing, List`1 incoming)` -> `Void`
- method `ObtainEpoch(String epochId)` -> `Void`
- method `ObtainEpochOverride(String epochId, EpochState state)` -> `Void`
- method `ParseAchievements(List`1 source, Dictionary`2 target, DeserializationContext ctx)` -> `Void`
- method `ParseAncientStats(List`1 source, Dictionary`2 target, DeserializationContext ctx)` -> `Void`
- method `ParseCardStats(List`1 source, Dictionary`2 target, DeserializationContext ctx)` -> `Void`
- method `ParseCharacterStats(List`1 source, Dictionary`2 target, DeserializationContext ctx)` -> `Void`
- method `ParseDiscoveredSet(List`1 source, HashSet`1 target, String fieldName, DeserializationContext ctx)` -> `Void`
- method `ParseEncounterStats(List`1 source, Dictionary`2 target, DeserializationContext ctx)` -> `Void`
- method `ParseEnemyStats(List`1 source, Dictionary`2 target, DeserializationContext ctx)` -> `Void`
- method `ParseEpochs(List`1 source, List`1 target, DeserializationContext ctx)` -> `Void`
- method `ParseFtues(List`1 source, HashSet`1 target, DeserializationContext ctx)` -> `Void`
- method `RemoveUnlockedAchievement(Achievement achievement)` -> `Boolean`
- method `ResetEpochs()` -> `Void`
- method `ResetFtues()` -> `Void`
- method `RevealEpoch(String epochId)` -> `Void`
- method `set_ArchitectDamage(Int64 value)` -> `Void`
- method `set_CurrentScore(Int32 value)` -> `Void`
- method `set_EnableFtues(Boolean value)` -> `Void`
- method `set_FloorsClimbed(Int64 value)` -> `Void`
- method `set_MaxMultiplayerAscension(Int32 value)` -> `Void`
- method `set_PendingCharacterUnlock(ModelId value)` -> `Void`
- method `set_PreferredMultiplayerAscension(Int32 value)` -> `Void`
- method `set_TestSubjectKills(Int32 value)` -> `Void`
- method `set_TotalPlaytime(Int64 value)` -> `Void`
- method `set_TotalUnlocks(Int32 value)` -> `Void`
- method `set_UniqueId(String value)` -> `Void`
- method `set_WongoPoints(Int32 value)` -> `Void`
- method `ToSerializable()` -> `SerializableProgress`
- method `UnlockSlot(String epochId)` -> `Void`
- method `ValidateModelId(ModelId id, String fieldName, DeserializationContext ctx)` -> `ModelId`
- field `Dictionary`2 _achievementsByName`
- field `Dictionary`2 _ancientStats`
- field `Dictionary`2 _cardStats`
- field `Dictionary`2 _characterStats`
- field `HashSet`1 _discoveredActs`
- field `HashSet`1 _discoveredCards`
- field `HashSet`1 _discoveredEvents`
- field `HashSet`1 _discoveredPotions`
- field `HashSet`1 _discoveredRelics`
- field `Dictionary`2 _encounterStats`
- field `Dictionary`2 _enemyStats`
- field `List`1 _epochs`
- field `HashSet`1 _ftueCompleted`
- field `Dictionary`2 _unlockedAchievements`
- field `Int64 <ArchitectDamage>k__BackingField`
- field `Int32 <CurrentScore>k__BackingField`
- field `Boolean <EnableFtues>k__BackingField`
- field `Int64 <FloorsClimbed>k__BackingField`
- field `Int32 <MaxMultiplayerAscension>k__BackingField`
- field `ModelId <PendingCharacterUnlock>k__BackingField`
- field `Int32 <PreferredMultiplayerAscension>k__BackingField`
- field `Int32 <TestSubjectKills>k__BackingField`
- field `Int64 <TotalPlaytime>k__BackingField`
- field `Int32 <TotalUnlocks>k__BackingField`
- field `String <UniqueId>k__BackingField`
- field `Int32 <WongoPoints>k__BackingField`

## MegaCrit.Sts2.Core.Saves.ProgressState+<>c
- method `<FilterAndSortEpochs>b__150_0(SerializableEpoch e)` -> `Boolean`
- method `<get_BestWinStreak>b__95_0(CharacterStats c)` -> `Int64`
- method `<get_FastestVictory>b__93_0(CharacterStats c)` -> `Int64`
- method `<get_Losses>b__91_0(CharacterStats c)` -> `Int32`
- method `<get_Wins>b__89_0(CharacterStats c)` -> `Int32`
- method `<ToSerializable>b__100_0(KeyValuePair`2 kvp)` -> `SerializableUnlockedAchievement`
- field `<>c <>9`
- field `Func`2 <>9__100_0`
- field `Predicate`1 <>9__150_0`
- field `Func`2 <>9__89_0`
- field `Func`2 <>9__91_0`
- field `Func`2 <>9__93_0`
- field `Func`2 <>9__95_0`

## MegaCrit.Sts2.Core.Saves.ProgressState+<>c__DisplayClass112_0
- method `<ObtainEpoch>b__0(SerializableEpoch e)` -> `Boolean`
- field `String epochId`

## MegaCrit.Sts2.Core.Saves.ProgressState+<>c__DisplayClass113_0
- method `<ObtainEpochOverride>b__0(SerializableEpoch e)` -> `Boolean`
- field `String epochId`

## MegaCrit.Sts2.Core.Saves.ProgressState+<>c__DisplayClass114_0
- method `<UnlockSlot>b__0(SerializableEpoch e)` -> `Boolean`
- field `String epochId`

## MegaCrit.Sts2.Core.Saves.ProgressState+<>c__DisplayClass115_0
- method `<RevealEpoch>b__0(SerializableEpoch e)` -> `Boolean`
- field `String epochId`

## MegaCrit.Sts2.Core.Saves.ProgressState+<>c__DisplayClass123_0
- method `<HasEpoch>b__0(SerializableEpoch e)` -> `Boolean`
- field `String epochId`

## MegaCrit.Sts2.Core.Saves.ProgressState+<>c__DisplayClass124_0
- method `<IsEpochObtained>b__0(SerializableEpoch e)` -> `Boolean`
- field `String epochId`

## MegaCrit.Sts2.Core.Saves.ProgressState+<>c__DisplayClass125_0
- method `<IsEpochRevealed>b__0(SerializableEpoch e)` -> `Boolean`
- field `String epochId`

## MegaCrit.Sts2.Core.Saves.ProgressState+<>c__DisplayClass139_0
- method `<ClampFightStatsFields>b__0(FightStats f)` -> `Boolean`
- field `FightStats fight`

## MegaCrit.Sts2.Core.Saves.ProgressState+<>c__DisplayClass140_0
- method `<ClampAncientCharacterStatsFields>b__0(AncientCharacterStats c)` -> `Boolean`
- field `AncientCharacterStats stats`

## MegaCrit.Sts2.Core.Saves.ProgressState+<>c__DisplayClass144_0
- method `<MergeFightStatsList>b__0(FightStats f)` -> `Boolean`
- field `FightStats incomingFight`

## MegaCrit.Sts2.Core.Saves.ProgressState+<>c__DisplayClass145_0
- method `<MergeAncientCharacterStatsList>b__0(AncientCharacterStats c)` -> `Boolean`
- field `AncientCharacterStats incomingStats`

## MegaCrit.Sts2.Core.Saves.ReadSaveResult`1
- method `get_ErrorMessage()` -> `String`
- method `get_SaveData()` -> `T`
- method `get_Status()` -> `ReadSaveStatus`
- method `get_Success()` -> `Boolean`
- field `String <ErrorMessage>k__BackingField`
- field `T <SaveData>k__BackingField`
- field `ReadSaveStatus <Status>k__BackingField`

## MegaCrit.Sts2.Core.Saves.ReadSaveStatus
- field `ReadSaveStatus FileAccessError`
- field `ReadSaveStatus FileEmpty`
- field `ReadSaveStatus FileNotFound`
- field `ReadSaveStatus FutureVersion`
- field `ReadSaveStatus JsonParseError`
- field `ReadSaveStatus JsonRepaired`
- field `ReadSaveStatus MigrationFailed`
- field `ReadSaveStatus MigrationRequired`
- field `ReadSaveStatus MissingSchemaVersion`
- field `ReadSaveStatus RecoveredWithDataLoss`
- field `ReadSaveStatus Success`
- field `ReadSaveStatus Unrecoverable`
- field `ReadSaveStatus ValidationFailed`
- field `Int32 value__`
- field `ReadSaveStatus VersionTooOld`

## MegaCrit.Sts2.Core.Saves.ReadSaveStatusExtensions
- method `IsRecoverable(ReadSaveStatus status)` -> `Boolean`

## MegaCrit.Sts2.Core.Saves.Runs.EpochIdListConverter
- method `Read(Utf8JsonReader& reader, Type typeToConvert, JsonSerializerOptions options)` -> `List`1`
- method `Write(Utf8JsonWriter writer, List`1 value, JsonSerializerOptions options)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Runs.JsonSerializeConditionAttribute
- method `CheckJsonSerializeConditionsModifier(JsonTypeInfo typeInfo)` -> `Void`
- field `SerializationCondition defaultBehaviour`

## MegaCrit.Sts2.Core.Saves.Runs.JsonSerializeConditionAttribute+<>c__DisplayClass2_0
- method `<CheckJsonSerializeConditionsModifier>b__0(Object _, Object c)` -> `Boolean`
- field `JsonSerializeConditionAttribute attr`
- field `MemberInfo memberInfo`

## MegaCrit.Sts2.Core.Saves.Runs.ModelIdRunSaveConverter
- method `Read(Utf8JsonReader& reader, Type typeToConvert, JsonSerializerOptions options)` -> `ModelId`
- method `Write(Utf8JsonWriter writer, ModelId value, JsonSerializerOptions options)` -> `Void`

## MegaCrit.Sts2.Core.Saves.Runs.SavedProperties
- method `Any()` -> `Boolean`
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Fill(AbstractModel model)` -> `Void`
- method `FillInternal(Object model)` -> `Void`
- method `From(AbstractModel model)` -> `SavedProperties`
- method `FromInternal(Object model, ModelId id)` -> `SavedProperties`
- method `ReadPropertyName(PacketReader reader)` -> `String`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `ToString()` -> `String`
- method `WritePropertyName(PacketWriter writer, String propertyName)` -> `Void`
- field `List`1 bools`
- field `List`1 cardArrays`
- field `List`1 cards`
- field `List`1 intArrays`
- field `List`1 ints`
- field `List`1 modelIds`
- field `List`1 strings`

## MegaCrit.Sts2.Core.Saves.Runs.SavedProperties+<>c
- method `<ToString>b__18_0(SerializableCard c)` -> `String`
- field `<>c <>9`
- field `Func`2 <>9__18_0`

## MegaCrit.Sts2.Core.Saves.Runs.SavedProperties+<>O
- field `Func`2 <0>__ToInt32`

## MegaCrit.Sts2.Core.Saves.Runs.SavedProperties+SavedProperty`1
- field `String name`
- field `T value`

## MegaCrit.Sts2.Core.Saves.Runs.SavedPropertiesTypeCache
- method `CachePropertiesForType(Type type)` -> `Void`
- method `CompareProperties(PropertyInfo p1, PropertyInfo p2)` -> `Int32`
- method `get_NetIdBitSize()` -> `Int32`
- method `GetJsonPropertiesForType(Type t)` -> `List`1`
- method `GetNetIdForPropertyName(String propertyName)` -> `Int32`
- method `GetPropertyNameForNetId(Int32 netId)` -> `String`
- method `InjectTypeIntoCache(Type type)` -> `Void`
- method `set_NetIdBitSize(Int32 value)` -> `Void`
- field `Dictionary`2 _cache`
- field `List`1 _netIdToPropertyNameMap`
- field `Dictionary`2 _propertyNameToNetIdMap`
- field `Int32 <NetIdBitSize>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SavedPropertiesTypeCache+<>c
- method `<CachePropertiesForType>b__8_0(PropertyInfo p)` -> `Boolean`
- field `<>c <>9`
- field `Func`2 <>9__8_0`

## MegaCrit.Sts2.Core.Saves.Runs.SavedPropertiesTypeCache+<>O
- field `Comparison`1 <0>__CompareProperties`

## MegaCrit.Sts2.Core.Saves.Runs.SavedPropertyAttribute
- field `SerializationCondition defaultBehaviour`
- field `Int32 order`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap
- method `Deserialize(PacketReader reader)` -> `Void`
- method `FromActMap(ActMap map)` -> `SerializableActMap`
- method `get_BossPoint()` -> `SerializableMapPoint`
- method `get_GridHeight()` -> `Int32`
- method `get_GridWidth()` -> `Int32`
- method `get_Points()` -> `List`1`
- method `get_SecondBossPoint()` -> `SerializableMapPoint`
- method `get_StartingPoint()` -> `SerializableMapPoint`
- method `get_StartMapPointCoords()` -> `List`1`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_BossPoint(SerializableMapPoint value)` -> `Void`
- method `set_GridHeight(Int32 value)` -> `Void`
- method `set_GridWidth(Int32 value)` -> `Void`
- method `set_Points(List`1 value)` -> `Void`
- method `set_SecondBossPoint(SerializableMapPoint value)` -> `Void`
- method `set_StartingPoint(SerializableMapPoint value)` -> `Void`
- method `set_StartMapPointCoords(List`1 value)` -> `Void`
- field `SerializableMapPoint <BossPoint>k__BackingField`
- field `Int32 <GridHeight>k__BackingField`
- field `Int32 <GridWidth>k__BackingField`
- field `List`1 <Points>k__BackingField`
- field `SerializableMapPoint <SecondBossPoint>k__BackingField`
- field `SerializableMapPoint <StartingPoint>k__BackingField`
- field `List`1 <StartMapPointCoords>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Id()` -> `ModelId`
- method `get_SavedMap()` -> `SerializableActMap`
- method `get_SerializableRooms()` -> `SerializableRoomSet`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Id(ModelId value)` -> `Void`
- method `set_SavedMap(SerializableActMap value)` -> `Void`
- method `set_SerializableRooms(SerializableRoomSet value)` -> `Void`
- field `ModelId <Id>k__BackingField`
- field `SerializableActMap <SavedMap>k__BackingField`
- field `SerializableRoomSet <SerializableRooms>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Id()` -> `String`
- method `get_Rarity()` -> `BadgeRarity`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Id(String value)` -> `Void`
- method `set_Rarity(BadgeRarity value)` -> `Void`
- field `String <Id>k__BackingField`
- field `BadgeRarity <Rarity>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableCard
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Equals(Object obj)` -> `Boolean`
- method `get_CurrentUpgradeLevel()` -> `Int32`
- method `get_Enchantment()` -> `SerializableEnchantment`
- method `get_FloorAddedToDeck()` -> `Nullable`1`
- method `get_Id()` -> `ModelId`
- method `get_Props()` -> `SavedProperties`
- method `GetHashCode()` -> `Int32`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_CurrentUpgradeLevel(Int32 value)` -> `Void`
- method `set_Enchantment(SerializableEnchantment value)` -> `Void`
- method `set_FloorAddedToDeck(Nullable`1 value)` -> `Void`
- method `set_Id(ModelId value)` -> `Void`
- method `set_Props(SavedProperties value)` -> `Void`
- method `ToString()` -> `String`
- field `Int32 <CurrentUpgradeLevel>k__BackingField`
- field `SerializableEnchantment <Enchantment>k__BackingField`
- field `Nullable`1 <FloorAddedToDeck>k__BackingField`
- field `ModelId <Id>k__BackingField`
- field `SavedProperties <Props>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment
- method `Deserialize(PacketReader reader)` -> `Void`
- method `Equals(Object obj)` -> `Boolean`
- method `get_Amount()` -> `Int32`
- method `get_Id()` -> `ModelId`
- method `get_Props()` -> `SavedProperties`
- method `GetHashCode()` -> `Int32`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Amount(Int32 value)` -> `Void`
- method `set_Id(ModelId value)` -> `Void`
- method `set_Props(SavedProperties value)` -> `Void`
- method `ToString()` -> `String`
- field `Int32 <Amount>k__BackingField`
- field `ModelId <Id>k__BackingField`
- field `SavedProperties <Props>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_FreedRepy()` -> `Boolean`
- method `get_StartedWithNeow()` -> `Boolean`
- method `get_TestSubjectKills()` -> `Int32`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_FreedRepy(Boolean value)` -> `Void`
- method `set_StartedWithNeow(Boolean value)` -> `Void`
- method `set_TestSubjectKills(Int32 value)` -> `Void`
- field `Boolean <FreedRepy>k__BackingField`
- field `Boolean <StartedWithNeow>k__BackingField`
- field `Int32 <TestSubjectKills>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint
- method `Deserialize(PacketReader reader)` -> `Void`
- method `FromMapPoint(MapPoint point)` -> `SerializableMapPoint`
- method `get_CanBeModified()` -> `Boolean`
- method `get_ChildCoords()` -> `List`1`
- method `get_Coord()` -> `MapCoord`
- method `get_PointType()` -> `MapPointType`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_CanBeModified(Boolean value)` -> `Void`
- method `set_ChildCoords(List`1 value)` -> `Void`
- method `set_Coord(MapCoord value)` -> `Void`
- method `set_PointType(MapPointType value)` -> `Void`
- field `Boolean <CanBeModified>k__BackingField`
- field `List`1 <ChildCoords>k__BackingField`
- field `MapCoord <Coord>k__BackingField`
- field `MapPointType <PointType>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Id()` -> `ModelId`
- method `get_Props()` -> `SavedProperties`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Id(ModelId value)` -> `Void`
- method `set_Props(SavedProperties value)` -> `Void`
- field `ModelId <Id>k__BackingField`
- field `SavedProperties <Props>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer
- method `Anonymized()` -> `SerializablePlayer`
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_BaseOrbSlotCount()` -> `Int32`
- method `get_CharacterId()` -> `ModelId`
- method `get_CurrentHp()` -> `Int32`
- method `get_Deck()` -> `List`1`
- method `get_DiscoveredCards()` -> `List`1`
- method `get_DiscoveredEnemies()` -> `List`1`
- method `get_DiscoveredEpochs()` -> `List`1`
- method `get_DiscoveredPotions()` -> `List`1`
- method `get_DiscoveredRelics()` -> `List`1`
- method `get_ExtraFields()` -> `SerializableExtraPlayerFields`
- method `get_Gold()` -> `Int32`
- method `get_MaxEnergy()` -> `Int32`
- method `get_MaxHp()` -> `Int32`
- method `get_MaxPotionSlotCount()` -> `Int32`
- method `get_NetId()` -> `UInt64`
- method `get_Odds()` -> `SerializablePlayerOddsSet`
- method `get_Potions()` -> `List`1`
- method `get_RelicGrabBag()` -> `SerializableRelicGrabBag`
- method `get_Relics()` -> `List`1`
- method `get_Rng()` -> `SerializablePlayerRngSet`
- method `get_UnlockState()` -> `SerializableUnlockState`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_BaseOrbSlotCount(Int32 value)` -> `Void`
- method `set_CharacterId(ModelId value)` -> `Void`
- method `set_CurrentHp(Int32 value)` -> `Void`
- method `set_Deck(List`1 value)` -> `Void`
- method `set_DiscoveredCards(List`1 value)` -> `Void`
- method `set_DiscoveredEnemies(List`1 value)` -> `Void`
- method `set_DiscoveredEpochs(List`1 value)` -> `Void`
- method `set_DiscoveredPotions(List`1 value)` -> `Void`
- method `set_DiscoveredRelics(List`1 value)` -> `Void`
- method `set_ExtraFields(SerializableExtraPlayerFields value)` -> `Void`
- method `set_Gold(Int32 value)` -> `Void`
- method `set_MaxEnergy(Int32 value)` -> `Void`
- method `set_MaxHp(Int32 value)` -> `Void`
- method `set_MaxPotionSlotCount(Int32 value)` -> `Void`
- method `set_NetId(UInt64 value)` -> `Void`
- method `set_Odds(SerializablePlayerOddsSet value)` -> `Void`
- method `set_Potions(List`1 value)` -> `Void`
- method `set_RelicGrabBag(SerializableRelicGrabBag value)` -> `Void`
- method `set_Relics(List`1 value)` -> `Void`
- method `set_Rng(SerializablePlayerRngSet value)` -> `Void`
- method `set_UnlockState(SerializableUnlockState value)` -> `Void`
- field `Int32 <BaseOrbSlotCount>k__BackingField`
- field `ModelId <CharacterId>k__BackingField`
- field `Int32 <CurrentHp>k__BackingField`
- field `List`1 <Deck>k__BackingField`
- field `List`1 <DiscoveredCards>k__BackingField`
- field `List`1 <DiscoveredEnemies>k__BackingField`
- field `List`1 <DiscoveredEpochs>k__BackingField`
- field `List`1 <DiscoveredPotions>k__BackingField`
- field `List`1 <DiscoveredRelics>k__BackingField`
- field `SerializableExtraPlayerFields <ExtraFields>k__BackingField`
- field `Int32 <Gold>k__BackingField`
- field `Int32 <MaxEnergy>k__BackingField`
- field `Int32 <MaxHp>k__BackingField`
- field `Int32 <MaxPotionSlotCount>k__BackingField`
- field `UInt64 <NetId>k__BackingField`
- field `SerializablePlayerOddsSet <Odds>k__BackingField`
- field `List`1 <Potions>k__BackingField`
- field `SerializableRelicGrabBag <RelicGrabBag>k__BackingField`
- field `List`1 <Relics>k__BackingField`
- field `SerializablePlayerRngSet <Rng>k__BackingField`
- field `SerializableUnlockState <UnlockState>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_CardRarityOddsValue()` -> `Single`
- method `get_PotionRewardOddsValue()` -> `Single`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_CardRarityOddsValue(Single value)` -> `Void`
- method `set_PotionRewardOddsValue(Single value)` -> `Void`
- field `Single <CardRarityOddsValue>k__BackingField`
- field `Single <PotionRewardOddsValue>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Id()` -> `ModelId`
- method `get_SlotIndex()` -> `Int32`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Id(ModelId value)` -> `Void`
- method `set_SlotIndex(Int32 value)` -> `Void`
- field `ModelId <Id>k__BackingField`
- field `Int32 <SlotIndex>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_FloorAddedToDeck()` -> `Nullable`1`
- method `get_Id()` -> `ModelId`
- method `get_Props()` -> `SavedProperties`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_FloorAddedToDeck(Nullable`1 value)` -> `Void`
- method `set_Id(ModelId value)` -> `Void`
- method `set_Props(SavedProperties value)` -> `Void`
- field `Nullable`1 <FloorAddedToDeck>k__BackingField`
- field `ModelId <Id>k__BackingField`
- field `SavedProperties <Props>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_RelicIdLists()` -> `Dictionary`2`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_RelicIdLists(Dictionary`2 value)` -> `Void`
- field `Dictionary`2 <RelicIdLists>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableReward
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_CardPoolIds()` -> `List`1`
- method `get_CustomDescriptionEncounterSourceId()` -> `ModelId`
- method `get_GoldAmount()` -> `Int32`
- method `get_OptionCount()` -> `Int32`
- method `get_PredeterminedModelId()` -> `ModelId`
- method `get_RarityOdds()` -> `CardRarityOddsType`
- method `get_RewardType()` -> `RewardType`
- method `get_Source()` -> `CardCreationSource`
- method `get_SpecialCard()` -> `SerializableCard`
- method `get_WasGoldStolenBack()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_CardPoolIds(List`1 value)` -> `Void`
- method `set_CustomDescriptionEncounterSourceId(ModelId value)` -> `Void`
- method `set_GoldAmount(Int32 value)` -> `Void`
- method `set_OptionCount(Int32 value)` -> `Void`
- method `set_PredeterminedModelId(ModelId value)` -> `Void`
- method `set_RarityOdds(CardRarityOddsType value)` -> `Void`
- method `set_RewardType(RewardType value)` -> `Void`
- method `set_Source(CardCreationSource value)` -> `Void`
- method `set_SpecialCard(SerializableCard value)` -> `Void`
- method `set_WasGoldStolenBack(Boolean value)` -> `Void`
- field `List`1 <CardPoolIds>k__BackingField`
- field `ModelId <CustomDescriptionEncounterSourceId>k__BackingField`
- field `Int32 <GoldAmount>k__BackingField`
- field `Int32 <OptionCount>k__BackingField`
- field `ModelId <PredeterminedModelId>k__BackingField`
- field `CardRarityOddsType <RarityOdds>k__BackingField`
- field `RewardType <RewardType>k__BackingField`
- field `CardCreationSource <Source>k__BackingField`
- field `SerializableCard <SpecialCard>k__BackingField`
- field `Boolean <WasGoldStolenBack>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_EncounterId()` -> `ModelId`
- method `get_EncounterState()` -> `Dictionary`2`
- method `get_EventId()` -> `ModelId`
- method `get_ExtraRewards()` -> `Dictionary`2`
- method `get_GoldProportion()` -> `Single`
- method `get_IsPreFinished()` -> `Boolean`
- method `get_ParentEventId()` -> `ModelId`
- method `get_RoomType()` -> `RoomType`
- method `get_ShouldResumeParentEvent()` -> `Boolean`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_EncounterId(ModelId value)` -> `Void`
- method `set_EncounterState(Dictionary`2 value)` -> `Void`
- method `set_EventId(ModelId value)` -> `Void`
- method `set_ExtraRewards(Dictionary`2 value)` -> `Void`
- method `set_GoldProportion(Single value)` -> `Void`
- method `set_IsPreFinished(Boolean value)` -> `Void`
- method `set_ParentEventId(ModelId value)` -> `Void`
- method `set_RoomType(RoomType value)` -> `Void`
- method `set_ShouldResumeParentEvent(Boolean value)` -> `Void`
- field `ModelId <EncounterId>k__BackingField`
- field `Dictionary`2 <EncounterState>k__BackingField`
- field `ModelId <EventId>k__BackingField`
- field `Dictionary`2 <ExtraRewards>k__BackingField`
- field `Single <GoldProportion>k__BackingField`
- field `Boolean <IsPreFinished>k__BackingField`
- field `ModelId <ParentEventId>k__BackingField`
- field `RoomType <RoomType>k__BackingField`
- field `Boolean <ShouldResumeParentEvent>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_AncientId()` -> `ModelId`
- method `get_BossEncountersVisited()` -> `Int32`
- method `get_BossId()` -> `ModelId`
- method `get_EliteEncounterIds()` -> `List`1`
- method `get_EliteEncountersVisited()` -> `Int32`
- method `get_EventIds()` -> `List`1`
- method `get_EventsVisited()` -> `Int32`
- method `get_NormalEncounterIds()` -> `List`1`
- method `get_NormalEncountersVisited()` -> `Int32`
- method `get_SecondBossId()` -> `ModelId`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_AncientId(ModelId value)` -> `Void`
- method `set_BossEncountersVisited(Int32 value)` -> `Void`
- method `set_BossId(ModelId value)` -> `Void`
- method `set_EliteEncounterIds(List`1 value)` -> `Void`
- method `set_EliteEncountersVisited(Int32 value)` -> `Void`
- method `set_EventIds(List`1 value)` -> `Void`
- method `set_EventsVisited(Int32 value)` -> `Void`
- method `set_NormalEncounterIds(List`1 value)` -> `Void`
- method `set_NormalEncountersVisited(Int32 value)` -> `Void`
- method `set_SecondBossId(ModelId value)` -> `Void`
- field `ModelId <AncientId>k__BackingField`
- field `Int32 <BossEncountersVisited>k__BackingField`
- field `ModelId <BossId>k__BackingField`
- field `List`1 <EliteEncounterIds>k__BackingField`
- field `Int32 <EliteEncountersVisited>k__BackingField`
- field `List`1 <EventIds>k__BackingField`
- field `Int32 <EventsVisited>k__BackingField`
- field `List`1 <NormalEncounterIds>k__BackingField`
- field `Int32 <NormalEncountersVisited>k__BackingField`
- field `ModelId <SecondBossId>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_UnknownMapPointEliteOddsValue()` -> `Single`
- method `get_UnknownMapPointMonsterOddsValue()` -> `Single`
- method `get_UnknownMapPointShopOddsValue()` -> `Single`
- method `get_UnknownMapPointTreasureOddsValue()` -> `Single`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_UnknownMapPointEliteOddsValue(Single value)` -> `Void`
- method `set_UnknownMapPointMonsterOddsValue(Single value)` -> `Void`
- method `set_UnknownMapPointShopOddsValue(Single value)` -> `Void`
- method `set_UnknownMapPointTreasureOddsValue(Single value)` -> `Void`
- field `Single <UnknownMapPointEliteOddsValue>k__BackingField`
- field `Single <UnknownMapPointMonsterOddsValue>k__BackingField`
- field `Single <UnknownMapPointShopOddsValue>k__BackingField`
- field `Single <UnknownMapPointTreasureOddsValue>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Counters()` -> `Dictionary`2`
- method `get_Seed()` -> `String`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Counters(Dictionary`2 value)` -> `Void`
- method `set_Seed(String value)` -> `Void`
- field `Dictionary`2 <Counters>k__BackingField`
- field `String <Seed>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Runs.SerializationCondition
- field `SerializationCondition AlwaysSave`
- field `SerializationCondition SaveIfNotCollectionEmptyOrNull`
- field `SerializationCondition SaveIfNotPropertyDefault`
- field `SerializationCondition SaveIfNotTypeDefault`
- field `Int32 value__`

## MegaCrit.Sts2.Core.Saves.Runs.SerializationConditionExtensions
- method `GetMemberDefaultValue(MemberInfo info)` -> `Object`
- method `GetTypeDefaultValue(Type t)` -> `Object`
- method `GetUnderlyingType(MemberInfo member)` -> `Type`
- method `ShouldSerialize(SerializationCondition condition, Object candidate, MemberInfo memberInfo)` -> `Boolean`
- field `Dictionary`2 _defaultTypeCache`

## MegaCrit.Sts2.Core.Saves.SaveBatchScope
- method `Dispose()` -> `Void`
- field `SaveManager <saveManager>P`

## MegaCrit.Sts2.Core.Saves.SaveManager
- method `add_ProfileIdChanged(Action`1 value)` -> `Void`
- method `add_Saved(Action value)` -> `Void`
- method `BeginSaveBatch()` -> `SaveBatchScope`
- method `CleanupStaleCurrentRunSaveForProfile(Int32 profileId, String runSaveFileName)` -> `Void`
- method `CleanupStaleCurrentRunSaves()` -> `Void`
- method `CleanupTemporaryFiles()` -> `Void`
- method `ClearInstanceForTesting()` -> `Void`
- method `ConstructDefault()` -> `SaveManager`
- method `DeleteCurrentMultiplayerRun()` -> `Void`
- method `DeleteCurrentRun()` -> `Void`
- method `DeleteDirectoryRecursive(String directory)` -> `Void`
- method `DeleteInDirectoryRecursive(String directory)` -> `Void`
- method `DeleteProfile(Int32 profileId)` -> `Void`
- method `EndSaveBatch()` -> `Void`
- method `EnumerateCloudSyncTasks(CloudSaveStore cloudStore)` -> `IEnumerable`1`
- method `ExtractStartTimeFromRunSave(String json)` -> `Nullable`1`
- method `FromJson(String json)` -> `ReadSaveResult`1`
- method `GenerateUnlockStateFromProgress()` -> `UnlockState`
- method `get_CurrentProfileId()` -> `Int32`
- method `get_CurrentRunSaveTask()` -> `Task`
- method `get_HasMultiplayerRunSave()` -> `Boolean`
- method `get_HasRunSave()` -> `Boolean`
- method `get_Instance()` -> `SaveManager`
- method `get_PrefsSave()` -> `PrefsSave`
- method `get_Progress()` -> `ProgressState`
- method `get_SettingsSave()` -> `SettingsSave`
- method `GetAggregateAscensionCount()` -> `Int32`
- method `GetAggregateAscensionProgress()` -> `Int32`
- method `GetAllRunHistoryNames()` -> `List`1`
- method `GetCardUnlockEpochIds()` -> `String[]`
- method `GetCurrentScore()` -> `Int32`
- method `GetDiscoveredEpochCount()` -> `Int32`
- method `GetEpochIdForUnlock()` -> `String`
- method `GetLatestSchemaVersion()` -> `Int32`
- method `GetPotionUnlockEpochIds()` -> `String[]`
- method `GetProfileScopedPath(String userData)` -> `String`
- method `GetRelicUnlockEpochIds()` -> `String[]`
- method `GetRevealableEpochs()` -> `IEnumerable`1`
- method `GetRunHistoryCount()` -> `Int32`
- method `GetTotalKills()` -> `Int32`
- method `GetTotalUnlockedCards()` -> `Int32`
- method `GetTotalUnlockedPotions()` -> `Int32`
- method `GetTotalUnlockedRelics()` -> `Int32`
- method `GetUnlockableCardCount()` -> `Int32`
- method `GetUnlockablePotionCount()` -> `Int32`
- method `GetUnlockableRelicCount()` -> `Int32`
- method `GetUnlocksRemaining()` -> `Int32`
- method `IncrementUnlock()` -> `String`
- method `InitPrefsData()` -> `ReadSaveResult`1`
- method `InitPrefsDataForTest()` -> `ReadSaveResult`1`
- method `InitProfileId(Nullable`1 profileId)` -> `Void`
- method `InitProgressData()` -> `ReadSaveResult`1`
- method `InitSettingsData()` -> `ReadSaveResult`1`
- method `InitSettingsDataForTest()` -> `ReadSaveResult`1`
- method `IsCompendiumAvailable()` -> `Boolean`
- method `IsEpochRevealed()` -> `Boolean`
- method `IsEpochRevealed(String id)` -> `Boolean`
- method `IsNeowDiscovered()` -> `Boolean`
- method `IsRelicSeen(RelicModel relic)` -> `Boolean`
- method `LoadAndCanonicalizeMultiplayerRunSave(UInt64 localPlayerId)` -> `ReadSaveResult`1`
- method `LoadRunHistory(String fileName)` -> `ReadSaveResult`1`
- method `LoadRunSave()` -> `ReadSaveResult`1`
- method `MarkCardAsSeen(CardModel card)` -> `Void`
- method `MarkFtueAsComplete(String ftueId)` -> `Void`
- method `MarkPotionAsSeen(PotionModel potion)` -> `Void`
- method `MarkRelicAsSeen(RelicModel relic)` -> `Void`
- method `MockInstanceForTesting(SaveManager saveManager)` -> `Void`
- method `ObtainEpoch(String epochId)` -> `Void`
- method `ObtainEpochOverride(String epochId, EpochState state)` -> `Void`
- method `remove_ProfileIdChanged(Action`1 value)` -> `Void`
- method `remove_Saved(Action value)` -> `Void`
- method `ResetFtues()` -> `Void`
- method `ResetTimelineProgress()` -> `Void`
- method `RevealEpoch(String epochId, Boolean isDebug)` -> `Void`
- method `SavePrefsFile()` -> `Void`
- method `SaveProfile()` -> `Void`
- method `SaveProgressFile()` -> `Void`
- method `SaveRun(AbstractRoom preFinishedRoom, Boolean saveProgress)` -> `Task`
- method `SaveRunHistory(RunHistory history)` -> `Void`
- method `SaveSettings()` -> `Void`
- method `SeenFtue(String ftueKey)` -> `Boolean`
- method `set_CurrentRunSaveTask(Task value)` -> `Void`
- method `set_Progress(ProgressState value)` -> `Void`
- method `SetFtuesEnabled(Boolean enabled)` -> `Void`
- method `SwitchProfileId(Int32 profileId)` -> `Void`
- method `SyncCloudToLocal()` -> `Task`
- method `ToJson(T obj)` -> `String`
- method `TryFirstTimeCloudSync()` -> `Task`1`
- method `UnlockSlot(String epochId)` -> `Void`
- method `UpdateProgressAfterCombatWon(Player localPlayer, CombatRoom combatRoom)` -> `Void`
- method `UpdateProgressWithRunData(SerializableRun serializableRun, Boolean victory)` -> `Void`
- field `String[] _agnosticEpochUnlockOrder`
- field `Nullable`1 _currentProfileId`
- field `SaveManager _instance`
- field `MigrationManager _migrationManager`
- field `SaveManager _mockInstance`
- field `PrefsSaveManager _prefsSaveManager`
- field `ProfileSaveManager _profileSaveManager`
- field `ProgressSaveManager _progressSaveManager`
- field `RunHistorySaveManager _runHistorySaveManager`
- field `RunSaveManager _runSaveManager`
- field `ISaveStore _saveStore`
- field `SettingsSaveManager _settingsSaveManager`
- field `Task <CurrentRunSaveTask>k__BackingField`
- field `Action`1 ProfileIdChanged`
- field `Int32 totalAgnosticUnlocks`

## MegaCrit.Sts2.Core.Saves.SaveManager+<>c
- method `<GetAggregateAscensionProgress>b__105_0(CharacterStats stat)` -> `Int32`
- method `<GetTotalKills>b__107_0(EnemyStats enemy)` -> `Int32`
- method `<IsNeowDiscovered>b__110_0(SerializableEpoch e)` -> `Boolean`
- field `<>c <>9`
- field `Func`2 <>9__105_0`
- field `Func`2 <>9__107_0`
- field `Func`2 <>9__110_0`

## MegaCrit.Sts2.Core.Saves.SaveManager+<EnumerateCloudSyncTasks>d__64
- method `<>m__Finally1()` -> `Void`
- method `MoveNext()` -> `Boolean`
- method `System.Collections.Generic.IEnumerable<System.Threading.Tasks.Task>.GetEnumerator()` -> `IEnumerator`1`
- method `System.Collections.Generic.IEnumerator<System.Threading.Tasks.Task>.get_Current()` -> `Task`
- method `System.Collections.IEnumerable.GetEnumerator()` -> `IEnumerator`
- method `System.Collections.IEnumerator.get_Current()` -> `Object`
- method `System.Collections.IEnumerator.Reset()` -> `Void`
- method `System.IDisposable.Dispose()` -> `Void`
- field `Int32 <>1__state`
- field `Task <>2__current`
- field `CloudSaveStore <>3__cloudStore`
- field `IEnumerator`1 <>7__wrap2`
- field `Int32 <>l__initialThreadId`
- field `Int32 <i>5__2`
- field `CloudSaveStore cloudStore`

## MegaCrit.Sts2.Core.Saves.SaveManager+<SaveRun>d__48
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `SaveManager <>4__this`
- field `SaveBatchScope <>7__wrap1`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `AbstractRoom preFinishedRoom`
- field `Boolean saveProgress`

## MegaCrit.Sts2.Core.Saves.SaveManager+<SyncCloudToLocal>d__63
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `SaveManager <>4__this`
- field `IEnumerator`1 <>7__wrap2`
- field `AsyncTaskMethodBuilder <>t__builder`
- field `TaskAwaiter <>u__1`
- field `List`1 <tasks>5__2`

## MegaCrit.Sts2.Core.Saves.SaveManager+<TryFirstTimeCloudSync>d__69
- method `MoveNext()` -> `Void`
- method `SetStateMachine(IAsyncStateMachine stateMachine)` -> `Void`
- field `Int32 <>1__state`
- field `SaveManager <>4__this`
- field `AsyncTaskMethodBuilder`1 <>t__builder`
- field `TaskAwaiter <>u__1`

## MegaCrit.Sts2.Core.Saves.SaveUtil
- method `ActOrDeprecated(ModelId id)` -> `ActModel`
- method `AncientEventOrDeprecated(ModelId id)` -> `AncientEventModel`
- method `CardOrDeprecated(ModelId id)` -> `CardModel`
- method `CharacterOrDeprecated(ModelId id)` -> `CharacterModel`
- method `EnchantmentOrDeprecated(ModelId id)` -> `EnchantmentModel`
- method `EncounterOrDeprecated(ModelId id)` -> `EncounterModel`
- method `EventOrDeprecated(ModelId id)` -> `EventModel`
- method `ModifierOrDeprecated(ModelId id)` -> `ModifierModel`
- method `MonsterOrDeprecated(ModelId id)` -> `MonsterModel`
- method `PotionOrDeprecated(ModelId id)` -> `PotionModel`
- method `RelicOrDeprecated(ModelId id)` -> `RelicModel`

## MegaCrit.Sts2.Core.Saves.SerializableEpoch
- method `get_Id()` -> `String`
- method `get_ObtainDate()` -> `Int64`
- method `get_State()` -> `EpochState`
- method `set_ObtainDate(Int64 value)` -> `Void`
- method `set_State(EpochState value)` -> `Void`
- method `SetObtained(EpochState state)` -> `Void`
- field `String <Id>k__BackingField`
- field `Int64 <ObtainDate>k__BackingField`
- field `EpochState <State>k__BackingField`

## MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_CardShopRemovalsUsed()` -> `Int32`
- method `get_WongoPoints()` -> `Int32`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_CardShopRemovalsUsed(Int32 value)` -> `Void`
- method `set_WongoPoints(Int32 value)` -> `Void`
- field `Int32 <CardShopRemovalsUsed>k__BackingField`
- field `Int32 <WongoPoints>k__BackingField`

## MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Counters()` -> `Dictionary`2`
- method `get_Seed()` -> `UInt32`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Counters(Dictionary`2 value)` -> `Void`
- method `set_Seed(UInt32 value)` -> `Void`
- field `Dictionary`2 <Counters>k__BackingField`
- field `UInt32 <Seed>k__BackingField`

## MegaCrit.Sts2.Core.Saves.SerializableProgress
- method `<.ctor>g__GenerateUniqueId|0_0(Int32 length)` -> `String`
- method `get_AncientStats()` -> `List`1`
- method `get_ArchitectDamage()` -> `Int64`
- method `get_BestWinStreak()` -> `Int64`
- method `get_CardStats()` -> `List`1`
- method `get_CharStats()` -> `List`1`
- method `get_CurrentScore()` -> `Int32`
- method `get_DiscoveredActs()` -> `List`1`
- method `get_DiscoveredCards()` -> `List`1`
- method `get_DiscoveredEvents()` -> `List`1`
- method `get_DiscoveredPotions()` -> `List`1`
- method `get_DiscoveredRelics()` -> `List`1`
- method `get_EnableFtues()` -> `Boolean`
- method `get_EncounterStats()` -> `List`1`
- method `get_EnemyStats()` -> `List`1`
- method `get_Epochs()` -> `List`1`
- method `get_FastestVictory()` -> `Int64`
- method `get_FloorsClimbed()` -> `Int64`
- method `get_FtueCompleted()` -> `List`1`
- method `get_Losses()` -> `Int32`
- method `get_MaxMultiplayerAscension()` -> `Int32`
- method `get_NumberOfRuns()` -> `Int32`
- method `get_PendingCharacterUnlock()` -> `ModelId`
- method `get_PreferredMultiplayerAscension()` -> `Int32`
- method `get_SchemaVersion()` -> `Int32`
- method `get_TestSubjectKills()` -> `Int32`
- method `get_TotalPlaytime()` -> `Int64`
- method `get_TotalUnlocks()` -> `Int32`
- method `get_UniqueId()` -> `String`
- method `get_UnlockedAchievements()` -> `List`1`
- method `get_Wins()` -> `Int32`
- method `get_WongoPoints()` -> `Int32`
- method `GetStatsForAncient(ModelId ancientId)` -> `AncientStats`
- method `GetStatsForCharacter(ModelId characterId)` -> `CharacterStats`
- method `set_AncientStats(List`1 value)` -> `Void`
- method `set_ArchitectDamage(Int64 value)` -> `Void`
- method `set_CardStats(List`1 value)` -> `Void`
- method `set_CharStats(List`1 value)` -> `Void`
- method `set_CurrentScore(Int32 value)` -> `Void`
- method `set_DiscoveredActs(List`1 value)` -> `Void`
- method `set_DiscoveredCards(List`1 value)` -> `Void`
- method `set_DiscoveredEvents(List`1 value)` -> `Void`
- method `set_DiscoveredPotions(List`1 value)` -> `Void`
- method `set_DiscoveredRelics(List`1 value)` -> `Void`
- method `set_EnableFtues(Boolean value)` -> `Void`
- method `set_EncounterStats(List`1 value)` -> `Void`
- method `set_EnemyStats(List`1 value)` -> `Void`
- method `set_Epochs(List`1 value)` -> `Void`
- method `set_FloorsClimbed(Int64 value)` -> `Void`
- method `set_FtueCompleted(List`1 value)` -> `Void`
- method `set_MaxMultiplayerAscension(Int32 value)` -> `Void`
- method `set_PendingCharacterUnlock(ModelId value)` -> `Void`
- method `set_PreferredMultiplayerAscension(Int32 value)` -> `Void`
- method `set_SchemaVersion(Int32 value)` -> `Void`
- method `set_TestSubjectKills(Int32 value)` -> `Void`
- method `set_TotalPlaytime(Int64 value)` -> `Void`
- method `set_TotalUnlocks(Int32 value)` -> `Void`
- method `set_UniqueId(String value)` -> `Void`
- method `set_UnlockedAchievements(List`1 value)` -> `Void`
- method `set_WongoPoints(Int32 value)` -> `Void`
- field `List`1 <AncientStats>k__BackingField`
- field `Int64 <ArchitectDamage>k__BackingField`
- field `List`1 <CardStats>k__BackingField`
- field `List`1 <CharStats>k__BackingField`
- field `Int32 <CurrentScore>k__BackingField`
- field `List`1 <DiscoveredActs>k__BackingField`
- field `List`1 <DiscoveredCards>k__BackingField`
- field `List`1 <DiscoveredEvents>k__BackingField`
- field `List`1 <DiscoveredPotions>k__BackingField`
- field `List`1 <DiscoveredRelics>k__BackingField`
- field `Boolean <EnableFtues>k__BackingField`
- field `List`1 <EncounterStats>k__BackingField`
- field `List`1 <EnemyStats>k__BackingField`
- field `List`1 <Epochs>k__BackingField`
- field `Int64 <FloorsClimbed>k__BackingField`
- field `List`1 <FtueCompleted>k__BackingField`
- field `Int32 <MaxMultiplayerAscension>k__BackingField`
- field `ModelId <PendingCharacterUnlock>k__BackingField`
- field `Int32 <PreferredMultiplayerAscension>k__BackingField`
- field `Int32 <SchemaVersion>k__BackingField`
- field `Int32 <TestSubjectKills>k__BackingField`
- field `Int64 <TotalPlaytime>k__BackingField`
- field `Int32 <TotalUnlocks>k__BackingField`
- field `String <UniqueId>k__BackingField`
- field `List`1 <UnlockedAchievements>k__BackingField`
- field `Int32 <WongoPoints>k__BackingField`

## MegaCrit.Sts2.Core.Saves.SerializableProgress+<>c
- method `<.ctor>b__0_1(String s)` -> `Char`
- method `<get_BestWinStreak>b__112_0(CharacterStats c)` -> `Int64`
- method `<get_FastestVictory>b__110_0(CharacterStats c)` -> `Int64`
- method `<get_Losses>b__108_0(CharacterStats character)` -> `Int32`
- method `<get_Wins>b__106_0(CharacterStats character)` -> `Int32`
- field `<>c <>9`
- field `Func`2 <>9__0_1`
- field `Func`2 <>9__106_0`
- field `Func`2 <>9__108_0`
- field `Func`2 <>9__110_0`
- field `Func`2 <>9__112_0`

## MegaCrit.Sts2.Core.Saves.SerializableProgress+<>c__DisplayClass115_0
- method `<GetStatsForCharacter>b__0(CharacterStats c)` -> `Boolean`
- field `ModelId characterId`

## MegaCrit.Sts2.Core.Saves.SerializableProgress+<>c__DisplayClass116_0
- method `<GetStatsForAncient>b__0(AncientStats a)` -> `Boolean`
- field `ModelId ancientId`

## MegaCrit.Sts2.Core.Saves.SerializableRun
- method `Anonymized()` -> `SerializableRun`
- method `Deserialize(PacketReader reader)` -> `Void`
- method `get_Acts()` -> `List`1`
- method `get_Ascension()` -> `Int32`
- method `get_CurrentActIndex()` -> `Int32`
- method `get_DailyTime()` -> `Nullable`1`
- method `get_EventsSeen()` -> `List`1`
- method `get_ExtraFields()` -> `SerializableExtraRunFields`
- method `get_GameMode()` -> `GameMode`
- method `get_MapDrawings()` -> `SerializableMapDrawings`
- method `get_MapPointHistory()` -> `List`1`
- method `get_Modifiers()` -> `List`1`
- method `get_PlatformType()` -> `PlatformType`
- method `get_Players()` -> `List`1`
- method `get_PreFinishedRoom()` -> `SerializableRoom`
- method `get_RunTime()` -> `Int64`
- method `get_SaveTime()` -> `Int64`
- method `get_SchemaVersion()` -> `Int32`
- method `get_SerializableOdds()` -> `SerializableRunOddsSet`
- method `get_SerializableRng()` -> `SerializableRunRngSet`
- method `get_SerializableSharedRelicGrabBag()` -> `SerializableRelicGrabBag`
- method `get_StartTime()` -> `Int64`
- method `get_VisitedMapCoords()` -> `List`1`
- method `get_WinTime()` -> `Int64`
- method `Serialize(PacketWriter writer)` -> `Void`
- method `set_Acts(List`1 value)` -> `Void`
- method `set_Ascension(Int32 value)` -> `Void`
- method `set_CurrentActIndex(Int32 value)` -> `Void`
- method `set_DailyTime(Nullable`1 value)` -> `Void`
- method `set_EventsSeen(List`1 value)` -> `Void`
- method `set_ExtraFields(SerializableExtraRunFields value)` -> `Void`
- method `set_GameMode(GameMode value)` -> `Void`
- method `set_MapDrawings(SerializableMapDrawings value)` -> `Void`
- method `set_MapPointHistory(List`1 value)` -> `Void`
- method `set_Modifiers(List`1 value)` -> `Void`
- method `set_PlatformType(PlatformType value)` -> `Void`
- method `set_Players(List`1 value)` -> `Void`
- method `set_PreFinishedRoom(SerializableRoom value)` -> `Void`
- method `set_RunTime(Int64 value)` -> `Void`
- method `set_SaveTime(Int64 value)` -> `Void`
- method `set_SchemaVersion(Int32 value)` -> `Void`
- method `set_SerializableOdds(SerializableRunOddsSet value)` -> `Void`
- method `set_SerializableRng(SerializableRunRngSet value)` -> `Void`
- method `set_SerializableSharedRelicGrabBag(SerializableRelicGrabBag value)` -> `Void`
- method `set_StartTime(Int64 value)` -> `Void`
- method `set_VisitedMapCoords(List`1 value)` -> `Void`
- method `set_WinTime(Int64 value)` -> `Void`
- field `List`1 <Acts>k__BackingField`
- field `Int32 <Ascension>k__BackingField`
- field `Int32 <CurrentActIndex>k__BackingField`
- field `Nullable`1 <DailyTime>k__BackingField`
- field `List`1 <EventsSeen>k__BackingField`
- field `SerializableExtraRunFields <ExtraFields>k__BackingField`
- field `GameMode <GameMode>k__BackingField`
- field `SerializableMapDrawings <MapDrawings>k__BackingField`
- field `List`1 <MapPointHistory>k__BackingField`
- field `List`1 <Modifiers>k__BackingField`
- field `PlatformType <PlatformType>k__BackingField`
- field `List`1 <Players>k__BackingField`
- field `SerializableRoom <PreFinishedRoom>k__BackingField`
- field `Int64 <RunTime>k__BackingField`
- field `Int64 <SaveTime>k__BackingField`
- field `Int32 <SchemaVersion>k__BackingField`
- field `SerializableRunOddsSet <SerializableOdds>k__BackingField`
- field `SerializableRunRngSet <SerializableRng>k__BackingField`
- field `SerializableRelicGrabBag <SerializableSharedRelicGrabBag>k__BackingField`
- field `Int64 <StartTime>k__BackingField`
- field `List`1 <VisitedMapCoords>k__BackingField`
- field `Int64 <WinTime>k__BackingField`

## MegaCrit.Sts2.Core.Saves.SerializableRun+<>c
- method `<Anonymized>b__90_0(SerializablePlayer p)` -> `SerializablePlayer`
- method `<Anonymized>b__90_1(List`1 l)` -> `List`1`
- method `<Anonymized>b__90_2(MapPointHistoryEntry h)` -> `MapPointHistoryEntry`
- field `<>c <>9`
- field `Func`2 <>9__90_0`
- field `Func`2 <>9__90_1`
- field `Func`2 <>9__90_2`

## MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement
- method `<Clone>$()` -> `SerializableUnlockedAchievement`
- method `Equals(Object obj)` -> `Boolean`
- method `Equals(SerializableUnlockedAchievement other)` -> `Boolean`
- method `get_Achievement()` -> `String`
- method `get_EqualityContract()` -> `Type`
- method `get_UnlockTime()` -> `Int64`
- method `GetHashCode()` -> `Int32`
- method `op_Equality(SerializableUnlockedAchievement left, SerializableUnlockedAchievement right)` -> `Boolean`
- method `op_Inequality(SerializableUnlockedAchievement left, SerializableUnlockedAchievement right)` -> `Boolean`
- method `PrintMembers(StringBuilder builder)` -> `Boolean`
- method `set_Achievement(String value)` -> `Void`
- method `set_UnlockTime(Int64 value)` -> `Void`
- method `ToString()` -> `String`
- field `String <Achievement>k__BackingField`
- field `Int64 <UnlockTime>k__BackingField`

## MegaCrit.Sts2.Core.Saves.SettingsSave
- method `get_AspectRatioSetting()` -> `AspectRatioSetting`
- method `get_ControllerMapping()` -> `Dictionary`2`
- method `get_ControllerMappingType()` -> `ControllerMappingType`
- method `get_FpsLimit()` -> `Int32`
- method `get_FullConsole()` -> `Boolean`
- method `get_Fullscreen()` -> `Boolean`
- method `get_KeyboardMapping()` -> `Dictionary`2`
- method `get_Language()` -> `String`
- method `get_LimitFpsInBackground()` -> `Boolean`
- method `get_ModSettings()` -> `ModSettings`
- method `get_Msaa()` -> `Int32`
- method `get_ResizeWindows()` -> `Boolean`
- method `get_SchemaVersion()` -> `Int32`
- method `get_SeenEaDisclaimer()` -> `Boolean`
- method `get_SkipIntroLogo()` -> `Boolean`
- method `get_TargetDisplay()` -> `Int32`
- method `get_VolumeAmbience()` -> `Single`
- method `get_VolumeBgm()` -> `Single`
- method `get_VolumeMaster()` -> `Single`
- method `get_VolumeSfx()` -> `Single`
- method `get_VSync()` -> `VSyncType`
- method `get_WindowPosition()` -> `Vector2I`
- method `get_WindowSize()` -> `Vector2I`
- method `set_AspectRatioSetting(AspectRatioSetting value)` -> `Void`
- method `set_ControllerMapping(Dictionary`2 value)` -> `Void`
- method `set_ControllerMappingType(ControllerMappingType value)` -> `Void`
- method `set_FpsLimit(Int32 value)` -> `Void`
- method `set_FullConsole(Boolean value)` -> `Void`
- method `set_Fullscreen(Boolean value)` -> `Void`
- method `set_KeyboardMapping(Dictionary`2 value)` -> `Void`
- method `set_Language(String value)` -> `Void`
- method `set_LimitFpsInBackground(Boolean value)` -> `Void`
- method `set_ModSettings(ModSettings value)` -> `Void`
- method `set_Msaa(Int32 value)` -> `Void`
- method `set_ResizeWindows(Boolean value)` -> `Void`
- method `set_SchemaVersion(Int32 value)` -> `Void`
- method `set_SeenEaDisclaimer(Boolean value)` -> `Void`
- method `set_SkipIntroLogo(Boolean value)` -> `Void`
- method `set_TargetDisplay(Int32 value)` -> `Void`
- method `set_VolumeAmbience(Single value)` -> `Void`
- method `set_VolumeBgm(Single value)` -> `Void`
- method `set_VolumeMaster(Single value)` -> `Void`
- method `set_VolumeSfx(Single value)` -> `Void`
- method `set_VSync(VSyncType value)` -> `Void`
- method `set_WindowPosition(Vector2I value)` -> `Void`
- method `set_WindowSize(Vector2I value)` -> `Void`
- field `AspectRatioSetting <AspectRatioSetting>k__BackingField`
- field `Dictionary`2 <ControllerMapping>k__BackingField`
- field `ControllerMappingType <ControllerMappingType>k__BackingField`
- field `Int32 <FpsLimit>k__BackingField`
- field `Boolean <FullConsole>k__BackingField`
- field `Boolean <Fullscreen>k__BackingField`
- field `Dictionary`2 <KeyboardMapping>k__BackingField`
- field `String <Language>k__BackingField`
- field `Boolean <LimitFpsInBackground>k__BackingField`
- field `ModSettings <ModSettings>k__BackingField`
- field `Int32 <Msaa>k__BackingField`
- field `Boolean <ResizeWindows>k__BackingField`
- field `Int32 <SchemaVersion>k__BackingField`
- field `Boolean <SeenEaDisclaimer>k__BackingField`
- field `Boolean <SkipIntroLogo>k__BackingField`
- field `Int32 <TargetDisplay>k__BackingField`
- field `Single <VolumeAmbience>k__BackingField`
- field `Single <VolumeBgm>k__BackingField`
- field `Single <VolumeMaster>k__BackingField`
- field `Single <VolumeSfx>k__BackingField`
- field `VSyncType <VSync>k__BackingField`
- field `Vector2I <WindowPosition>k__BackingField`
- field `Vector2I <WindowSize>k__BackingField`

## MegaCrit.Sts2.Core.Saves.SnakeCaseJsonStringEnumConverter`1

## MegaCrit.Sts2.Core.Saves.StaticProfileIdProvider
- method `get_CurrentProfileId()` -> `Int32`
- field `Int32 _profileId`

## MegaCrit.Sts2.Core.Saves.Test.MockCloudGodotFileIo
- method `BeginSaveBatch()` -> `Void`
- method `EndSaveBatch()` -> `Void`
- method `ForgetFile(String path)` -> `Void`
- method `HasCloudFiles()` -> `Boolean`
- method `IsFilePersisted(String path)` -> `Boolean`

## MegaCrit.Sts2.Core.Saves.Test.MockGodotFileIo
- method `CanonicalizePath(String& path, Boolean getFullPath)` -> `Void`
- method `CreateDirectory(String directoryPath)` -> `Void`
- method `DeleteDirectory(String directoryPath)` -> `Void`
- method `DeleteFile(String path)` -> `Void`
- method `DeleteTemporaryFiles(String directoryPath)` -> `Void`
- method `DirectoryExists(String path)` -> `Boolean`
- method `FileExists(String path)` -> `Boolean`
- method `get_Calls()` -> `List`1`
- method `get_RenameFileAction()` -> `Action`2`
- method `GetDirectoriesInDirectory(String directoryPath)` -> `String[]`
- method `GetFilesInDirectory(String directoryPath)` -> `String[]`
- method `GetFileSize(String path)` -> `Int32`
- method `GetFullPath(String filename)` -> `String`
- method `GetLastModifiedTime(String path)` -> `DateTimeOffset`
- method `ReadFile(String path)` -> `String`
- method `ReadFileAsync(String path)` -> `Task`1`
- method `RenameFile(String sourcePath, String destinationPath)` -> `Void`
- method `set_RenameFileAction(Action`2 value)` -> `Void`
- method `SetLastModifiedTime(String path, DateTimeOffset time)` -> `Void`
- method `WriteFile(String path, String content)` -> `Void`
- method `WriteFile(String path, Byte[] bytes)` -> `Void`
- method `WriteFileAsync(String path, String content)` -> `Task`
- method `WriteFileAsync(String path, Byte[] bytes)` -> `Task`
- field `ConcurrentDictionary`2 _directories`
- field `ConcurrentDictionary`2 _files`
- field `String _saveDir`
- field `List`1 <Calls>k__BackingField`
- field `Action`2 <RenameFileAction>k__BackingField`
- field `Boolean DoSteamSpecificError`
- field `Func`1 getCurrentTime`
- field `Boolean ShouldFailTimestampSync`
- field `Boolean ShouldFailWrites`

## MegaCrit.Sts2.Core.Saves.Test.MockGodotFileIo+<>c
- method `<GetDirectoriesInDirectory>b__32_2(String path)` -> `String`
- method `<GetFilesInDirectory>b__31_1(String path)` -> `String`
- field `<>c <>9`
- field `Func`2 <>9__31_1`
- field `Func`2 <>9__32_2`

## MegaCrit.Sts2.Core.Saves.Test.MockGodotFileIo+<>c__DisplayClass31_0
- method `<GetFilesInDirectory>b__0(String path)` -> `Boolean`
- field `String prefix`

## MegaCrit.Sts2.Core.Saves.Test.MockGodotFileIo+<>c__DisplayClass32_0
- method `<GetDirectoriesInDirectory>b__0(String path)` -> `Boolean`
- method `<GetDirectoriesInDirectory>b__1(String path)` -> `String`
- field `String prefix`

## MegaCrit.Sts2.Core.Saves.Test.MockGodotFileIo+<>c__DisplayClass35_0
- method `<DeleteTemporaryFiles>b__0(String path)` -> `Boolean`
- field `String prefix`

## MegaCrit.Sts2.Core.Saves.Test.MockGodotFileIo+File
- field `String content`
- field `Boolean forgotten`
- field `Nullable`1 lastModifiedTime`

## MegaCrit.Sts2.Core.Saves.Test.MockGodotFileIo+Methods
- field `String createDirectory`
- field `String deleteDirectory`
- field `String deleteFile`
- field `String deleteTemporaryFiles`
- field `String fileExists`
- field `String getDirectoriesInDirectory`
- field `String getFilesInDirectory`
- field `String getFullPath`
- field `String getLastModifiedTime`
- field `String readFile`
- field `String readFileAsync`
- field `String renameFile`
- field `String writeFile`
- field `String writeFileAsync`

## MegaCrit.Sts2.Core.Saves.UserDataPathProvider
- method `get_IsRunningModded()` -> `Boolean`
- method `get_SavesDir()` -> `String`
- method `GetAccountScopedBasePath(String dataType, Nullable`1 platformOverride, Nullable`1 userIdOverride)` -> `String`
- method `GetLegacyPreAccountPath(String dataType)` -> `String`
- method `GetPlatformDirectoryName(PlatformType platform)` -> `String`
- method `GetProfileDir(Int32 profileId)` -> `String`
- method `GetProfileScopedBasePath(Int32 profileId, Nullable`1 platformOverride, Nullable`1 userIdOverride)` -> `String`
- method `GetProfileScopedPath(Int32 profileId, String dataType, Nullable`1 platformOverride, Nullable`1 userIdOverride)` -> `String`
- method `IsLegacyPath(String path)` -> `Boolean`
- method `set_IsRunningModded(Boolean value)` -> `Void`
- field `Boolean <IsRunningModded>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Validation.DeserializationContext
- method `Fatal(String message)` -> `Void`
- method `get_CurrentPath()` -> `String`
- method `get_Errors()` -> `IReadOnlyList`1`
- method `get_FatalCount()` -> `Int32`
- method `get_HasFatal()` -> `Boolean`
- method `get_WarningCount()` -> `Int32`
- method `PopPath()` -> `Void`
- method `PushPath(String segment)` -> `Void`
- method `Warn(String message)` -> `Void`
- field `List`1 _errors`
- field `Stack`1 _pathSegments`

## MegaCrit.Sts2.Core.Saves.Validation.DeserializationContext+<>c
- method `<get_FatalCount>b__15_0(ValidationError e)` -> `Boolean`
- method `<get_HasFatal>b__11_0(ValidationError e)` -> `Boolean`
- method `<get_WarningCount>b__13_0(ValidationError e)` -> `Boolean`
- field `<>c <>9`
- field `Func`2 <>9__11_0`
- field `Func`2 <>9__13_0`
- field `Func`2 <>9__15_0`

## MegaCrit.Sts2.Core.Saves.Validation.ValidationError
- method `<Clone>$()` -> `ValidationError`
- method `Deconstruct(ValidationSeverity& Severity, String& Path, String& Message)` -> `Void`
- method `Equals(Object obj)` -> `Boolean`
- method `Equals(ValidationError other)` -> `Boolean`
- method `get_EqualityContract()` -> `Type`
- method `get_IsFatal()` -> `Boolean`
- method `get_Message()` -> `String`
- method `get_Path()` -> `String`
- method `get_Severity()` -> `ValidationSeverity`
- method `GetHashCode()` -> `Int32`
- method `op_Equality(ValidationError left, ValidationError right)` -> `Boolean`
- method `op_Inequality(ValidationError left, ValidationError right)` -> `Boolean`
- method `PrintMembers(StringBuilder builder)` -> `Boolean`
- method `set_Message(String value)` -> `Void`
- method `set_Path(String value)` -> `Void`
- method `set_Severity(ValidationSeverity value)` -> `Void`
- method `ToString()` -> `String`
- field `String <Message>k__BackingField`
- field `String <Path>k__BackingField`
- field `ValidationSeverity <Severity>k__BackingField`

## MegaCrit.Sts2.Core.Saves.Validation.ValidationSeverity
- field `ValidationSeverity Fatal`
- field `Int32 value__`
- field `ValidationSeverity Warning`

