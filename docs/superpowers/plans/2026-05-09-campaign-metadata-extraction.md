# Campaign Metadata Extraction Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Populate campaign roster labels and best-effort act/floor metadata for newly finalized, duplicated, and synced multiplayer save campaigns.

**Architecture:** Keep storage ignorant of STS2 internals by passing metadata through `CampaignCreateRequest` and sync/recovery runtime services. Add a small runtime extractor for STS2 `StartRunLobby` and `SerializableRun` objects, and use a Harmony patch to capture the lobby roster when a pending new run begins. Progress extraction is best-effort and must never block save copy, backup, or checksum safety.

**Tech Stack:** C#/.NET 9, STS2 assemblies, Harmony, Godot UI model tests, existing handwritten test harness.

---

## File Structure

- Modify `src/Core/CampaignLabeler.cs`: switch compact labels to `First, Second +N`.
- Modify `src/Core/CampaignCreateRequest.cs`: add optional `ActOrFloor`.
- Create `src/Core/CampaignMetadataSnapshot.cs`: common roster/progress transfer object.
- Create `src/Core/RunProgressLabeler.cs`: pure best-effort progress label formatting from act index and history counts.
- Modify `src/Storage/MultiplayerSaveBank.cs`: persist `request.ActOrFloor`.
- Modify `src/Storage/ActiveSaveSwitcher.cs`: allow sync-back to refresh progress without changing roster labels.
- Modify `src/UI/MultiplayerSavePickerModel.cs`: omit unknown progress text.
- Modify `src/Runtime/HostFlowContinuations.cs`: pass pending metadata into finalization.
- Modify `src/Runtime/HostFlowSession.cs`: store pending new-run metadata.
- Modify `src/Runtime/SaveSyncController.cs`: pass pending metadata into active-save sync.
- Modify `src/Runtime/ActiveSaveRecovery.cs`: capture metadata when duplicating unmanaged active saves.
- Modify `src/Runtime/Sts2HostFlowRuntime.cs`: wire the STS2 metadata extractor into sync/recovery and expose lobby capture.
- Create `src/Runtime/ICampaignMetadataExtractor.cs`: runtime interface for active-save metadata capture.
- Create `src/Runtime/Sts2CampaignMetadataExtractor.cs`: STS2-specific roster/progress extraction.
- Create `src/Patches/StartRunLobbyMetadataPatch.cs`: capture lobby roster when a pending new run starts.
- Modify `tests/CampaignLabelerTests.cs`: update label expectations.
- Create `tests/RunProgressLabelerTests.cs`: pure progress-label tests.
- Modify `tests/TestProgram.cs`: register new tests.
- Modify `tests/MultiplayerSaveBankTests.cs`: verify `ActOrFloor` persistence.
- Modify `tests/HostFlowControllerTests.cs`: verify pending metadata reaches finalization and picker display omits unknown progress.
- Modify `tests/ActiveSaveSwitcherTests.cs`: verify duplicate recovery and sync-back progress metadata.
- Modify `README.md`: update Phase 7 status and smoke checklist.

---

### Task 1: Compact Labels And Picker Subtitles

**Files:**
- Modify: `src/Core/CampaignLabeler.cs`
- Modify: `src/UI/MultiplayerSavePickerModel.cs`
- Modify: `tests/CampaignLabelerTests.cs`
- Modify: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing label and picker tests**

Update `tests/CampaignLabelerTests.cs` expectations:

```csharp
private static void TwoPlayers()
{
    var label = CampaignLabeler.Build([
        new PlayerIdentity("steam:1", "buddy1"),
        new PlayerIdentity("steam:2", "buddy2")
    ]);

    AssertEx.Equal("buddy1, buddy2", label);
}

private static void ManyPlayers()
{
    var label = CampaignLabeler.Build([
        new PlayerIdentity("steam:1", "buddy1"),
        new PlayerIdentity("steam:2", "buddy2"),
        new PlayerIdentity("steam:3", "buddy3"),
        new PlayerIdentity("steam:4", "buddy4"),
        new PlayerIdentity("steam:5", "buddy5")
    ]);

    AssertEx.Equal("buddy1, buddy2 +3", label);
}
```

Add a picker test in `tests/HostFlowControllerTests.cs`:

```csharp
yield return new TestCase("picker subtitle omits unknown progress", PickerSubtitleOmitsUnknownProgress);

private static void PickerSubtitleOmitsUnknownProgress()
{
    var row = MultiplayerSavePickerRow.Campaign(new CampaignMetadata(
        "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
        MultiplayerGameMode.Standard,
        "buddy1, buddy2 +2",
        [
            new PlayerIdentity("1", "buddy1"),
            new PlayerIdentity("2", "buddy2"),
            new PlayerIdentity("3", "buddy3"),
            new PlayerIdentity("4", "buddy4")
        ],
        DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
        DateTimeOffset.Parse("2026-05-08T01:00:00Z"),
        null,
        "checksum",
        null));

    AssertEx.Equal("buddy1, buddy2 +2", row.Title);
    AssertEx.Equal("4 players", row.Subtitle);
}
```

- [ ] **Step 2: Run tests to verify they fail**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: failures for the changed label expectations and unknown-progress subtitle.

- [ ] **Step 3: Implement label and subtitle behavior**

Update `src/Core/CampaignLabeler.cs`:

```csharp
return roster.Count switch
{
    0 => "Unknown party",
    1 => NormalizeName(roster[0]),
    2 => $"{NormalizeName(roster[0])}, {NormalizeName(roster[1])}",
    _ => $"{NormalizeName(roster[0])}, {NormalizeName(roster[1])} +{roster.Count - 2}"
};
```

Update `src/UI/MultiplayerSavePickerModel.cs`:

```csharp
var playerLabel = metadata.Roster.Count == 1 ? "1 player" : $"{metadata.Roster.Count} players";
var subtitle = string.IsNullOrWhiteSpace(metadata.ActOrFloor)
    ? playerLabel
    : $"{metadata.ActOrFloor} - {playerLabel}";
```

Return `subtitle` in the `MultiplayerSavePickerRow`.

- [ ] **Step 4: Run tests to verify Task 1 passes**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass after updating existing assertions that still expect `+` labels.

- [ ] **Step 5: Commit Task 1**

```bash
git add src/Core/CampaignLabeler.cs src/UI/MultiplayerSavePickerModel.cs tests/CampaignLabelerTests.cs tests/HostFlowControllerTests.cs
git commit -m "feat: compact campaign metadata display"
```

---

### Task 2: Persist Creation Progress Metadata

**Files:**
- Create: `src/Core/CampaignMetadataSnapshot.cs`
- Modify: `src/Core/CampaignCreateRequest.cs`
- Modify: `src/Storage/MultiplayerSaveBank.cs`
- Modify: `tests/MultiplayerSaveBankTests.cs`

- [ ] **Step 1: Write failing save-bank metadata test**

Add or update `PersistsMetadataContentsAndJson` in `tests/MultiplayerSaveBankTests.cs`:

```csharp
var metadata = bank.CreateCampaign(new CampaignCreateRequest(
    MultiplayerGameMode.Standard,
    roster,
    payload,
    createdAt,
    "Floor 18"));

AssertEx.Equal("Floor 18", roundTrip.ActOrFloor);
```

- [ ] **Step 2: Run tests to verify failure**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile failure until `CampaignCreateRequest` accepts `ActOrFloor`, or assertion failure while the bank still writes null.

- [ ] **Step 3: Add metadata snapshot and request field**

Create `src/Core/CampaignMetadataSnapshot.cs`:

```csharp
namespace MultiplayerSaveSlots.Core;

public sealed record CampaignMetadataSnapshot(
    IReadOnlyList<PlayerIdentity> Roster,
    string? ActOrFloor)
{
    public static CampaignMetadataSnapshot Empty { get; } = new([], null);
}
```

Update `src/Core/CampaignCreateRequest.cs`:

```csharp
public sealed record CampaignCreateRequest(
    MultiplayerGameMode GameMode,
    IReadOnlyList<PlayerIdentity> Roster,
    string SavePayloadPath,
    DateTimeOffset CreatedAtUtc,
    string? ActOrFloor = null);
```

Update `src/Storage/MultiplayerSaveBank.cs`:

```csharp
ActOrFloor: request.ActOrFloor);
```

- [ ] **Step 4: Run tests to verify Task 2 passes**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 5: Commit Task 2**

```bash
git add src/Core/CampaignMetadataSnapshot.cs src/Core/CampaignCreateRequest.cs src/Storage/MultiplayerSaveBank.cs tests/MultiplayerSaveBankTests.cs
git commit -m "feat: persist campaign progress metadata"
```

---

### Task 3: Pure Progress Labeling

**Files:**
- Create: `src/Core/RunProgressLabeler.cs`
- Create: `tests/RunProgressLabelerTests.cs`
- Modify: `tests/TestProgram.cs`

- [ ] **Step 1: Write failing progress-label tests**

Create `tests/RunProgressLabelerTests.cs`:

```csharp
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Tests;

public static class RunProgressLabelerTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("progress label uses floor when history exists", UsesFloorWhenHistoryExists));
        tests.Add(new TestCase("progress label uses act when only act is known", UsesActWhenOnlyActIsKnown));
        tests.Add(new TestCase("progress label omits invalid progress", OmitsInvalidProgress));
    }

    private static void UsesFloorWhenHistoryExists()
    {
        AssertEx.Equal("Floor 18", RunProgressLabeler.Build(currentActIndex: 1, completedFloorCount: 17));
    }

    private static void UsesActWhenOnlyActIsKnown()
    {
        AssertEx.Equal("Act 2", RunProgressLabeler.Build(currentActIndex: 1, completedFloorCount: 0));
    }

    private static void OmitsInvalidProgress()
    {
        AssertEx.Equal(null, RunProgressLabeler.Build(currentActIndex: -1, completedFloorCount: 0));
    }
}
```

Register it in `tests/TestProgram.cs`:

```csharp
RunProgressLabelerTests.Register(tests);
```

- [ ] **Step 2: Run tests to verify failure**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile failure because `RunProgressLabeler` does not exist.

- [ ] **Step 3: Implement `RunProgressLabeler`**

Create `src/Core/RunProgressLabeler.cs`:

```csharp
namespace MultiplayerSaveSlots.Core;

public static class RunProgressLabeler
{
    public static string? Build(int currentActIndex, int completedFloorCount)
    {
        if (completedFloorCount > 0)
            return $"Floor {completedFloorCount + 1}";

        if (currentActIndex >= 0)
            return $"Act {currentActIndex + 1}";

        return null;
    }
}
```

- [ ] **Step 4: Run tests to verify Task 3 passes**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 5: Commit Task 3**

```bash
git add src/Core/RunProgressLabeler.cs tests/RunProgressLabelerTests.cs tests/TestProgram.cs
git commit -m "feat: add run progress labels"
```

---

### Task 4: Thread Metadata Through Runtime Finalization And Recovery

**Files:**
- Modify: `src/Runtime/HostFlowContinuations.cs`
- Modify: `src/Runtime/HostFlowSession.cs`
- Modify: `src/Runtime/SaveSyncController.cs`
- Modify: `src/Runtime/ActiveSaveRecovery.cs`
- Modify: `src/Runtime/Sts2HostFlowRuntime.cs`
- Modify: `src/Storage/ActiveSaveSwitcher.cs`
- Modify: `tests/HostFlowControllerTests.cs`
- Modify: `tests/ActiveSaveSwitcherTests.cs`

- [ ] **Step 1: Write failing runtime metadata tests**

Add to `tests/HostFlowControllerTests.cs`:

```csharp
yield return new TestCase("save sync finalizes pending new run with metadata", SaveSyncFinalizesPendingNewRunWithMetadata);

private static void SaveSyncFinalizesPendingNewRunWithMetadata()
{
    var sync = new FakeActiveSaveSync { FinalizedCampaignId = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb" };
    var session = new HostFlowSession();
    session.SelectNewRun(MultiplayerGameMode.Custom);
    session.CapturePendingNewRunMetadata(new CampaignMetadataSnapshot(
        [new PlayerIdentity("steam:1", "buddy1")],
        "Floor 18"));
    var controller = new SaveSyncController(sync, session, new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));

    var result = controller.SyncAfterVanillaSave();

    AssertEx.True(result.Success);
    AssertEx.Equal(1, sync.FinalizeCount);
    AssertEx.Equal("buddy1", sync.FinalizedMetadata?.Roster[0].DisplayName);
    AssertEx.Equal("Floor 18", sync.FinalizedMetadata?.ActOrFloor);
}
```

Update `FakeActiveSaveSync.FinalizePendingNewRun` signature and store metadata:

```csharp
public CampaignMetadataSnapshot? FinalizedMetadata { get; private set; }

public OperationResult<string> FinalizePendingNewRun(
    MultiplayerGameMode gameMode,
    CampaignMetadataSnapshot metadata,
    DateTimeOffset nowUtc)
{
    FinalizeCount++;
    FinalizedGameMode = gameMode;
    FinalizedMetadata = metadata;
    return FinalizeFailure is null
        ? OperationResult<string>.Ok(FinalizedCampaignId)
        : OperationResult<string>.Fail(FinalizeFailure);
}
```

Add to `tests/ActiveSaveSwitcherTests.cs`:

```csharp
tests.Add(new TestCase("recovery duplicates active save with captured metadata", RecoveryDuplicatesActiveSaveWithCapturedMetadata));
tests.Add(new TestCase("sync-back refreshes progress metadata", SyncBackRefreshesProgressMetadata));
```

Use a fake extractor in those tests:

```csharp
private sealed class FakeCampaignMetadataExtractor(CampaignMetadataSnapshot snapshot) : ICampaignMetadataExtractor
{
    public CampaignMetadataSnapshot CaptureActiveSaveMetadata() => snapshot;
}
```

- [ ] **Step 2: Run tests to verify failure**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile failures for missing `CapturePendingNewRunMetadata`, changed interface, and extractor type.

- [ ] **Step 3: Update runtime contracts and session**

Update `src/Runtime/HostFlowContinuations.cs`:

```csharp
OperationResult<string> FinalizePendingNewRun(
    MultiplayerGameMode gameMode,
    CampaignMetadataSnapshot metadata,
    DateTimeOffset nowUtc);
```

Update `src/Runtime/HostFlowSession.cs`:

```csharp
public CampaignMetadataSnapshot PendingNewRunMetadata { get; private set; } = CampaignMetadataSnapshot.Empty;

public void CapturePendingNewRunMetadata(CampaignMetadataSnapshot metadata)
{
    if (IsPendingNewRun)
        PendingNewRunMetadata = metadata;
}

public void SelectNewRun(MultiplayerGameMode gameMode)
{
    SelectedCampaignId = null;
    SelectedGameMode = gameMode;
    IsPendingNewRun = true;
    PendingNewRunMetadata = CampaignMetadataSnapshot.Empty;
}
```

Clear metadata in `SelectExistingCampaign` and `Clear`.

- [ ] **Step 4: Add active-save extractor interface and wire sync/recovery**

Create `src/Runtime/ICampaignMetadataExtractor.cs`:

```csharp
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public interface ICampaignMetadataExtractor
{
    CampaignMetadataSnapshot CaptureActiveSaveMetadata();
}

public sealed class EmptyCampaignMetadataExtractor : ICampaignMetadataExtractor
{
    public CampaignMetadataSnapshot CaptureActiveSaveMetadata() => CampaignMetadataSnapshot.Empty;
}
```

In `SaveSyncController.FinalizePendingNewRun`, call:

```csharp
var finalized = _activeSaveSync.FinalizePendingNewRun(gameMode, _session.PendingNewRunMetadata, _clock.UtcNow);
```

In `ActiveSaveRecoveryService`, accept an optional `ICampaignMetadataExtractor`, safely capture metadata for duplicate recovery, and use:

```csharp
var snapshot = CaptureMetadataOrEmpty();
var metadata = _bank.CreateCampaign(new CampaignCreateRequest(
    gameMode,
    snapshot.Roster,
    _activeSavePath,
    nowUtc,
    snapshot.ActOrFloor));
```

In `Sts2ActiveSaveSync.FinalizePendingNewRun`, merge pending metadata with active-save metadata:

```csharp
var activeMetadata = CaptureMetadataOrEmpty();
var roster = metadata.Roster.Count > 0 ? metadata.Roster : activeMetadata.Roster;
var actOrFloor = activeMetadata.ActOrFloor ?? metadata.ActOrFloor;
var created = _bank.CreateCampaign(new CampaignCreateRequest(gameMode, roster, _activeSavePath, nowUtc, actOrFloor));
```

- [ ] **Step 5: Allow sync-back to refresh progress**

Update `src/Storage/ActiveSaveSwitcher.cs`:

```csharp
public void SyncBack(DateTimeOffset nowUtc, string? actOrFloor = null)
```

In metadata update:

```csharp
ActOrFloor = actOrFloor ?? metadata.ActOrFloor
```

Update `Sts2ActiveSaveSync.SyncBack`:

```csharp
var metadata = CaptureMetadataOrEmpty();
_switcher.SyncBack(nowUtc, metadata.ActOrFloor);
```

- [ ] **Step 6: Run tests to verify Task 4 passes**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 7: Commit Task 4**

```bash
git add src/Runtime src/Storage/ActiveSaveSwitcher.cs tests/HostFlowControllerTests.cs tests/ActiveSaveSwitcherTests.cs
git commit -m "feat: thread campaign metadata through runtime"
```

---

### Task 5: STS2 Metadata Extraction And Lobby Hook

**Files:**
- Create: `src/Runtime/Sts2CampaignMetadataExtractor.cs`
- Create: `src/Patches/StartRunLobbyMetadataPatch.cs`
- Modify: `src/Runtime/Sts2HostFlowRuntime.cs`
- Modify: `docs/implementation/hook-discovery.md`

- [ ] **Step 1: Implement STS2 extractor**

Create `src/Runtime/Sts2CampaignMetadataExtractor.cs`:

```csharp
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public sealed class Sts2CampaignMetadataExtractor : ICampaignMetadataExtractor
{
    public CampaignMetadataSnapshot CaptureActiveSaveMetadata()
    {
        var platform = PlatformUtil.PrimaryPlatform;
        var localPlayerId = PlatformUtil.GetLocalPlayerId(platform);
        var save = SaveManager.Instance.LoadAndCanonicalizeMultiplayerRunSave(localPlayerId);
        return save.Success && save.SaveData is not null
            ? FromSerializableRun(save.SaveData)
            : CampaignMetadataSnapshot.Empty;
    }

    public static CampaignMetadataSnapshot FromStartRunLobby(StartRunLobby lobby) =>
        new(CapturePlayers(lobby.Players, lobby.NetService.Platform), null);

    public static CampaignMetadataSnapshot FromSerializableRun(SerializableRun run)
    {
        var roster = run.Players
            .Select(player => ToIdentity(run.PlatformType, player.NetId, null))
            .ToList();
        var completedFloors = run.MapPointHistory?.Sum(act => act.Count) ?? 0;
        return new CampaignMetadataSnapshot(
            roster,
            RunProgressLabeler.Build(run.CurrentActIndex, completedFloors));
    }

    private static IReadOnlyList<PlayerIdentity> CapturePlayers(
        IReadOnlyList<LobbyPlayer> players,
        PlatformType platform) =>
        players
            .OrderBy(player => player.slotId)
            .Select(player => ToIdentity(platform, player.id, player.slotId))
            .ToList();

    private static PlayerIdentity ToIdentity(PlatformType platform, ulong playerId, int? slotId)
    {
        var stableId = $"{platform}:{playerId}";
        try
        {
            var name = PlatformUtil.GetPlayerName(platform, playerId);
            return new PlayerIdentity(stableId, string.IsNullOrWhiteSpace(name) ? FallbackName(playerId, slotId) : name);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to resolve player name for {stableId}: {ex.Message}");
            return new PlayerIdentity(stableId, FallbackName(playerId, slotId));
        }
    }

    private static string FallbackName(ulong playerId, int? slotId) =>
        slotId is null ? playerId.ToString() : $"Player {slotId.Value + 1}";
}
```

- [ ] **Step 2: Add lobby metadata patch**

Create `src/Patches/StartRunLobbyMetadataPatch.cs`:

```csharp
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MultiplayerSaveSlots.Runtime;
using HarmonyLib;

namespace MultiplayerSaveSlots.Patches;

[HarmonyPatch(typeof(StartRunLobby), nameof(StartRunLobby.BeginRunForAllPlayers), [typeof(string), typeof(List<ModifierModel>)])]
public static class StartRunLobbyMetadataPatch
{
    [HarmonyPrefix]
    public static void Prefix(StartRunLobby __instance)
    {
        try
        {
            if (!Sts2HostFlowRuntime.Session.IsPendingNewRun)
                return;

            var snapshot = Sts2CampaignMetadataExtractor.FromStartRunLobby(__instance);
            Sts2HostFlowRuntime.Session.CapturePendingNewRunMetadata(snapshot);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MultiplayerSaveSlots] Failed to capture lobby metadata: {ex.Message}");
        }
    }
}
```

- [ ] **Step 3: Wire extractor into runtime factories**

In `src/Runtime/Sts2HostFlowRuntime.cs`, pass `new Sts2CampaignMetadataExtractor()` into:

```csharp
new ActiveSaveRecoveryService(bank, switcher, paths.ActiveSavePath, paths.ActiveStatePath, new Sts2CampaignMetadataExtractor())
```

and:

```csharp
new Sts2ActiveSaveSync(bank, switcher, paths.ActiveSavePath, new Sts2CampaignMetadataExtractor())
```

- [ ] **Step 4: Document hook discovery update**

Append to `docs/implementation/hook-discovery.md`:

```markdown
## Phase 7 Metadata Hooks

Phase 7 captures pending new-run roster metadata from `StartRunLobby.BeginRunForAllPlayers(...)` before vanilla begins the run for all players. The lobby exposes `Players`, `NetService.Platform`, and each `LobbyPlayer.id`/`slotId`; display names are resolved through `PlatformUtil.GetPlayerName(platform, playerId)`.

Active-save progress metadata is extracted from `SaveManager.Instance.LoadAndCanonicalizeMultiplayerRunSave(...)` into `SerializableRun`, using `CurrentActIndex`, `MapPointHistory`, `Players`, and `PlatformType`.
```

- [ ] **Step 5: Build and run tests**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: build succeeds and tests pass.

- [ ] **Step 6: Commit Task 5**

```bash
git add src/Runtime/Sts2CampaignMetadataExtractor.cs src/Patches/StartRunLobbyMetadataPatch.cs src/Runtime/Sts2HostFlowRuntime.cs docs/implementation/hook-discovery.md
git commit -m "feat: capture sts2 campaign metadata"
```

---

### Task 6: Documentation And Final Verification

**Files:**
- Modify: `README.md`

- [ ] **Step 1: Update README status and smoke checklist**

Change the status text so it no longer says roster labels are placeholder metadata. Add a concise note:

```markdown
Newly finalized or duplicated runs now use live lobby/save metadata where STS2 exposes it. Campaign titles compact large rosters as `First, Second +N`, and picker subtitles include best-effort progress such as `Floor 18` when safely readable.
```

Add a smoke checklist item:

```markdown
- Verify the picker shows real roster labels, compacts 4+ player runs as `First, Second +N`, and omits progress when no safe act/floor value is available.
```

- [ ] **Step 2: Run final automated checks**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
tests/smoke-setup-local-tests.sh
git diff --check
bash -n scripts/smoke-setup-local.sh tests/smoke-setup-local-tests.sh
```

Expected:

- build succeeds with 0 errors
- test harness passes all tests
- smoke setup shell tests pass
- diff check has no output
- shell syntax checks pass

- [ ] **Step 3: Review branch diff**

Run:

```bash
git status --short
git diff --stat main...HEAD
git diff -- docs/superpowers/specs/2026-05-09-campaign-metadata-extraction-design.md docs/superpowers/plans/2026-05-09-campaign-metadata-extraction.md
```

Expected: only Phase 7 metadata, docs, tests, and plan files changed.

- [ ] **Step 4: Commit docs**

```bash
git add README.md
git commit -m "docs: document campaign metadata extraction"
```

- [ ] **Step 5: Prepare PR**

Run:

```bash
git status --short
git log --oneline main..HEAD
```

Expected: clean worktree and focused Phase 7 commits ready for review.
