# Multiplayer Save Slots Host Flow Picker Implementation Plan

> **Archived historical plan:** This document is preserved for context only. It is not an active implementation plan; unchecked checklist items below do not indicate pending work. See `docs/superpowers/README.md` for archive policy.

> **Archived worker note:** This was the original execution guidance. Do not execute this document as current work unless a new issue or branch explicitly reactivates it.

**Goal:** Add the first in-game host-flow save picker so `Multiplayer -> Host -> Select Gamemode` opens a save-slot choice before vanilla hosting continues.

**Architecture:** Phase 2 keeps save mutation behind the Phase 1 storage services and adds a thin host-flow layer around STS2 menu hooks. A controller builds a picker model, activates selected existing campaigns through `ActiveSaveSwitcher`, and delegates actual hosting/loading back to vanilla-compatible continuation adapters. The picker is code-built Godot UI so the mod remains a DLL + manifest package with no `.pck` asset requirement.

**Tech Stack:** C# `net9.0`, Godot UI nodes, STS2 `ModInitializer`, Harmony, `System.Text.Json`, existing no-NuGet console tests, and local `sts2.dll` references.

---

## Scope Check

This plan implements:

- host button visibility when a vanilla multiplayer save exists
- host gamemode interception through `NMultiplayerHostSubmenu.StartHost(GameMode)`
- a code-built modal picker with `Start New Run`, campaign rows, and cancel
- existing-campaign activation before load continuation
- controller tests and thin patch tests
- manual in-game smoke-test docs

This plan does not implement:

- save sync after vanilla writes
- pending new-run finalization into the save bank
- roster extraction from live lobbies
- recovery UI beyond cancel/fallback error popups
- PCK/scenes/assets

## File Structure

- Create: `src/Runtime/Clock.cs` - `IClock` and UTC clock implementation.
- Create: `src/Runtime/OperationResult.cs` - non-throwing result for UI/controller outcomes.
- Create: `src/Runtime/MultiplayerSaveRuntimePaths.cs` - STS2 active save, state, and bank path calculation.
- Create: `src/Runtime/HostFlowSession.cs` - in-memory selected/pending campaign state for later sync hooks.
- Create: `src/Runtime/HostFlowController.cs` - pure host-flow decision logic.
- Create: `src/Runtime/HostFlowContinuations.cs` - interfaces for start-new and load-existing vanilla continuations.
- Create: `src/Runtime/Sts2HostFlowRuntime.cs` - STS2 adapters for paths, save bank, switcher, and continuations.
- Create: `src/UI/MultiplayerSavePickerModel.cs` - picker row and action models.
- Create: `src/UI/MultiplayerSavePickerModal.cs` - code-built Godot modal UI.
- Create: `src/Patches/MultiplayerSubmenuPatch.cs` - keeps vanilla Host visible.
- Create: `src/Patches/MultiplayerHostSubmenuPatch.cs` - intercepts gamemode selection and shows picker.
- Create: `tests/HostFlowControllerTests.cs` - pure controller tests with fakes.
- Create: `tests/HostFlowPatchTests.cs` - mapping and guard tests for hook helpers.
- Modify: `tests/TestProgram.cs` - register new test cases.
- Modify: `README.md` - add Phase 2 test/install status.
- Modify: `docs/implementation/hook-discovery.md` - append confirmed Phase 2 hook notes.

## Confirmed STS2 Hook Facts

- `NMultiplayerSubmenu.UpdateButtons()` hides Host whenever `SaveManager.Instance.HasMultiplayerRunSave` is true.
- `NMultiplayerHostSubmenu.StartHost(GameMode gameMode)` is called by Standard, Daily, and Custom button handlers before vanilla networking starts.
- `NMultiplayerHostSubmenu.StartHostAsync(GameMode gameMode, Control loadingOverlay, NSubmenuStack stack)` starts a new hosted lobby with vanilla behavior.
- `NMultiplayerSubmenu.StartHost(SerializableRun run)` starts the vanilla loaded-run host path.
- `RunSaveManager.GetRunSavePath(profileId, "current_run_mp.save")` returns the vanilla active multiplayer save path.

## Task 1: Runtime Models And Path Locator

**Files:**
- Create: `src/Runtime/Clock.cs`
- Create: `src/Runtime/OperationResult.cs`
- Create: `src/Runtime/MultiplayerSaveRuntimePaths.cs`
- Create: `src/Runtime/HostFlowSession.cs`
- Test: `tests/HostFlowControllerTests.cs`
- Modify: `tests/TestProgram.cs`

- [ ] **Step 1: Write failing runtime path/session tests**

Add these test registrations to `tests/TestProgram.cs`:

```csharp
tests.AddRange(HostFlowControllerTests.All());
```

Create `tests/HostFlowControllerTests.cs` with the first tests:

```csharp
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.Tests;

public static class HostFlowControllerTests
{
    public static IEnumerable<TestCase> All()
    {
        yield return new TestCase("runtime paths place bank beside active multiplayer save", RuntimePathsPlaceBankBesideActiveSave);
        yield return new TestCase("host flow session tracks existing campaign selection", SessionTracksExistingCampaignSelection);
        yield return new TestCase("host flow session tracks pending new run", SessionTracksPendingNewRun);
    }

    private static void RuntimePathsPlaceBankBesideActiveSave()
    {
        var paths = MultiplayerSaveRuntimePaths.FromActiveSavePath("/tmp/sts2/profile1/saves/current_run_mp.save");

        AssertEx.Equal("/tmp/sts2/profile1/saves/current_run_mp.save", paths.ActiveSavePath);
        AssertEx.Equal("/tmp/sts2/profile1/saves/MultiplayerSaveSlots", paths.BankRootDirectory);
        AssertEx.Equal("/tmp/sts2/profile1/saves/MultiplayerSaveSlots/active-state.json", paths.ActiveStatePath);
    }

    private static void SessionTracksExistingCampaignSelection()
    {
        var session = new HostFlowSession();

        session.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

        AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", session.SelectedCampaignId);
        AssertEx.Equal(MultiplayerGameMode.Standard, session.SelectedGameMode);
        AssertEx.False(session.IsPendingNewRun);
    }

    private static void SessionTracksPendingNewRun()
    {
        var session = new HostFlowSession();

        session.SelectNewRun(MultiplayerGameMode.Custom);

        AssertEx.Equal(null, session.SelectedCampaignId);
        AssertEx.Equal(MultiplayerGameMode.Custom, session.SelectedGameMode);
        AssertEx.True(session.IsPendingNewRun);
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile fails because `MultiplayerSaveSlots.Runtime` types do not exist.

- [ ] **Step 3: Implement runtime basics**

Create `src/Runtime/Clock.cs`:

```csharp
namespace MultiplayerSaveSlots.Runtime;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
```

Create `src/Runtime/OperationResult.cs`:

```csharp
namespace MultiplayerSaveSlots.Runtime;

public sealed record OperationResult(bool Success, string? ErrorMessage)
{
    public static OperationResult Ok() => new(true, null);
    public static OperationResult Fail(string message) => new(false, message);
}

public sealed record OperationResult<T>(bool Success, T? Value, string? ErrorMessage)
{
    public static OperationResult<T> Ok(T value) => new(true, value, null);
    public static OperationResult<T> Fail(string message) => new(false, default, message);
}
```

Create `src/Runtime/MultiplayerSaveRuntimePaths.cs`:

```csharp
namespace MultiplayerSaveSlots.Runtime;

public sealed record MultiplayerSaveRuntimePaths(
    string ActiveSavePath,
    string BankRootDirectory,
    string ActiveStatePath)
{
    public static MultiplayerSaveRuntimePaths FromActiveSavePath(string activeSavePath)
    {
        var saveDirectory = Path.GetDirectoryName(activeSavePath);
        if (string.IsNullOrWhiteSpace(saveDirectory))
            saveDirectory = Directory.GetCurrentDirectory();

        var bankRoot = Path.Combine(saveDirectory, "MultiplayerSaveSlots");
        return new MultiplayerSaveRuntimePaths(
            activeSavePath,
            bankRoot,
            Path.Combine(bankRoot, "active-state.json"));
    }
}
```

Create `src/Runtime/HostFlowSession.cs`:

```csharp
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public sealed class HostFlowSession
{
    public string? SelectedCampaignId { get; private set; }
    public MultiplayerGameMode? SelectedGameMode { get; private set; }
    public bool IsPendingNewRun { get; private set; }

    public void SelectExistingCampaign(string campaignId, MultiplayerGameMode gameMode)
    {
        SelectedCampaignId = campaignId;
        SelectedGameMode = gameMode;
        IsPendingNewRun = false;
    }

    public void SelectNewRun(MultiplayerGameMode gameMode)
    {
        SelectedCampaignId = null;
        SelectedGameMode = gameMode;
        IsPendingNewRun = true;
    }

    public void Clear()
    {
        SelectedCampaignId = null;
        SelectedGameMode = null;
        IsPendingNewRun = false;
    }
}
```

- [ ] **Step 4: Run test to verify it passes**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 5: Commit**

```bash
git add src/Runtime tests/HostFlowControllerTests.cs tests/TestProgram.cs
git commit -m "feat: add host flow runtime state"
```

## Task 2: Picker Model And Host Flow Controller

**Files:**
- Create: `src/UI/MultiplayerSavePickerModel.cs`
- Create: `src/Runtime/HostFlowController.cs`
- Create: `src/Runtime/HostFlowContinuations.cs`
- Modify: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing controller tests**

Append these cases to `HostFlowControllerTests.All()`:

```csharp
yield return new TestCase("controller builds picker model with start new and campaign rows", ControllerBuildsPickerModel);
yield return new TestCase("controller starts new run through continuation", ControllerStartsNewRunThroughContinuation);
yield return new TestCase("controller activates existing campaign before load continuation", ControllerActivatesExistingCampaign);
yield return new TestCase("controller does not continue when activation fails", ControllerStopsWhenActivationFails);
```

Add test fakes and tests:

```csharp
private static void ControllerBuildsPickerModel()
{
    var bank = new FakeHostFlowSaveBank
    {
        Campaigns =
        [
            new CampaignMetadata(
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                MultiplayerGameMode.Standard,
                "buddy1 + buddy2",
                [new PlayerIdentity("1", "buddy1"), new PlayerIdentity("2", "buddy2")],
                DateTimeOffset.Parse("2026-05-08T00:00:00Z"),
                DateTimeOffset.Parse("2026-05-08T01:00:00Z"),
                null,
                "checksum",
                "Floor 7")
        ]
    };
    var controller = CreateController(bank);

    var model = controller.BuildPickerModel(MultiplayerGameMode.Standard);

    AssertEx.Equal(MultiplayerGameMode.Standard, model.GameMode);
    AssertEx.Equal(2, model.Rows.Count);
    AssertEx.Equal(PickerRowKind.StartNewRun, model.Rows[0].Kind);
    AssertEx.Equal("Start New Run", model.Rows[0].Title);
    AssertEx.Equal(PickerRowKind.Campaign, model.Rows[1].Kind);
    AssertEx.Equal("buddy1 + buddy2", model.Rows[1].Title);
    AssertEx.Equal("Floor 7 - 2 players", model.Rows[1].Subtitle);
}

private static void ControllerStartsNewRunThroughContinuation()
{
    var continuation = new FakeHostFlowContinuation();
    var session = new HostFlowSession();
    var controller = CreateController(new FakeHostFlowSaveBank(), continuation: continuation, session: session);

    var result = controller.SelectStartNewRun(MultiplayerGameMode.Daily);

    AssertEx.True(result.Success);
    AssertEx.Equal(MultiplayerGameMode.Daily, session.SelectedGameMode);
    AssertEx.True(session.IsPendingNewRun);
    AssertEx.Equal(1, continuation.StartNewRunCount);
    AssertEx.Equal(0, continuation.LoadExistingCount);
}

private static void ControllerActivatesExistingCampaign()
{
    var activator = new FakeActiveSaveActivator();
    var continuation = new FakeHostFlowContinuation();
    var session = new HostFlowSession();
    var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation, session);

    var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

    AssertEx.True(result.Success);
    AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", activator.ActivatedCampaignId);
    AssertEx.Equal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", session.SelectedCampaignId);
    AssertEx.Equal(0, continuation.StartNewRunCount);
    AssertEx.Equal(1, continuation.LoadExistingCount);
}

private static void ControllerStopsWhenActivationFails()
{
    var activator = new FakeActiveSaveActivator { Failure = "Active save has unsynced changes" };
    var continuation = new FakeHostFlowContinuation();
    var controller = CreateController(new FakeHostFlowSaveBank(), activator, continuation);

    var result = controller.SelectExistingCampaign("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", MultiplayerGameMode.Standard);

    AssertEx.False(result.Success);
    AssertEx.Equal("Active save has unsynced changes", result.ErrorMessage);
    AssertEx.Equal(0, continuation.StartNewRunCount);
    AssertEx.Equal(0, continuation.LoadExistingCount);
}
```

Add fakes in the same test file:

```csharp
private static HostFlowController CreateController(
    FakeHostFlowSaveBank? bank = null,
    FakeActiveSaveActivator? activator = null,
    FakeHostFlowContinuation? continuation = null,
    HostFlowSession? session = null)
{
    return new HostFlowController(
        bank ?? new FakeHostFlowSaveBank(),
        activator ?? new FakeActiveSaveActivator(),
        continuation ?? new FakeHostFlowContinuation(),
        session ?? new HostFlowSession(),
        new FixedClock(DateTimeOffset.Parse("2026-05-08T12:00:00Z")));
}

private sealed class FixedClock(DateTimeOffset utcNow) : IClock
{
    public DateTimeOffset UtcNow { get; } = utcNow;
}

private sealed class FakeHostFlowSaveBank : IHostFlowSaveBank
{
    public IReadOnlyList<CampaignMetadata> Campaigns { get; init; } = [];
    public IReadOnlyList<CampaignMetadata> ListCampaigns(MultiplayerGameMode gameMode) =>
        Campaigns.Where(campaign => campaign.GameMode == gameMode).ToList();
}

private sealed class FakeActiveSaveActivator : IActiveSaveActivator
{
    public string? ActivatedCampaignId { get; private set; }
    public string? Failure { get; init; }

    public OperationResult Activate(string campaignId, DateTimeOffset nowUtc)
    {
        if (Failure is not null)
            return OperationResult.Fail(Failure);

        ActivatedCampaignId = campaignId;
        return OperationResult.Ok();
    }
}

private sealed class FakeHostFlowContinuation : IHostFlowContinuation
{
    public int StartNewRunCount { get; private set; }
    public int LoadExistingCount { get; private set; }

    public OperationResult StartNewRun(MultiplayerGameMode gameMode)
    {
        StartNewRunCount++;
        return OperationResult.Ok();
    }

    public OperationResult LoadExistingRun()
    {
        LoadExistingCount++;
        return OperationResult.Ok();
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: compile fails because picker/controller interfaces do not exist.

- [ ] **Step 3: Implement picker model and controller**

Create `src/UI/MultiplayerSavePickerModel.cs`:

```csharp
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.UI;

public enum PickerRowKind
{
    StartNewRun,
    Campaign
}

public sealed record MultiplayerSavePickerModel(
    MultiplayerGameMode GameMode,
    IReadOnlyList<MultiplayerSavePickerRow> Rows);

public sealed record MultiplayerSavePickerRow(
    PickerRowKind Kind,
    string Title,
    string Subtitle,
    string? CampaignId)
{
    public static MultiplayerSavePickerRow StartNew() =>
        new(PickerRowKind.StartNewRun, "Start New Run", "Create a separate multiplayer run", null);

    public static MultiplayerSavePickerRow Campaign(CampaignMetadata metadata)
    {
        var progress = string.IsNullOrWhiteSpace(metadata.ActOrFloor) ? "Unknown progress" : metadata.ActOrFloor;
        var playerLabel = metadata.Roster.Count == 1 ? "1 player" : $"{metadata.Roster.Count} players";
        return new MultiplayerSavePickerRow(
            PickerRowKind.Campaign,
            metadata.Label,
            $"{progress} - {playerLabel}",
            metadata.CampaignId);
    }
}
```

Create `src/Runtime/HostFlowContinuations.cs`:

```csharp
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Runtime;

public interface IHostFlowSaveBank
{
    IReadOnlyList<CampaignMetadata> ListCampaigns(MultiplayerGameMode gameMode);
}

public interface IActiveSaveActivator
{
    OperationResult Activate(string campaignId, DateTimeOffset nowUtc);
}

public interface IHostFlowContinuation
{
    OperationResult StartNewRun(MultiplayerGameMode gameMode);
    OperationResult LoadExistingRun();
}
```

Create `src/Runtime/HostFlowController.cs`:

```csharp
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.UI;

namespace MultiplayerSaveSlots.Runtime;

public sealed class HostFlowController
{
    private readonly IHostFlowSaveBank _bank;
    private readonly IActiveSaveActivator _activator;
    private readonly IHostFlowContinuation _continuation;
    private readonly HostFlowSession _session;
    private readonly IClock _clock;

    public HostFlowController(
        IHostFlowSaveBank bank,
        IActiveSaveActivator activator,
        IHostFlowContinuation continuation,
        HostFlowSession session,
        IClock clock)
    {
        _bank = bank;
        _activator = activator;
        _continuation = continuation;
        _session = session;
        _clock = clock;
    }

    public MultiplayerSavePickerModel BuildPickerModel(MultiplayerGameMode gameMode)
    {
        var rows = new List<MultiplayerSavePickerRow> { MultiplayerSavePickerRow.StartNew() };
        rows.AddRange(_bank.ListCampaigns(gameMode).Select(MultiplayerSavePickerRow.Campaign));
        return new MultiplayerSavePickerModel(gameMode, rows);
    }

    public OperationResult SelectStartNewRun(MultiplayerGameMode gameMode)
    {
        _session.SelectNewRun(gameMode);
        return _continuation.StartNewRun(gameMode);
    }

    public OperationResult SelectExistingCampaign(string campaignId, MultiplayerGameMode gameMode)
    {
        var activation = _activator.Activate(campaignId, _clock.UtcNow);
        if (!activation.Success)
            return activation;

        _session.SelectExistingCampaign(campaignId, gameMode);
        return _continuation.LoadExistingRun();
    }
}
```

- [ ] **Step 4: Run test to verify it passes**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 5: Commit**

```bash
git add src/Runtime src/UI tests/HostFlowControllerTests.cs
git commit -m "feat: add host flow controller"
```

## Task 3: STS2 Runtime Adapters

**Files:**
- Create: `src/Runtime/Sts2HostFlowRuntime.cs`
- Modify: `src/Runtime/HostFlowContinuations.cs`
- Modify: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing adapter test for exception-to-result behavior**

Add:

```csharp
yield return new TestCase("active save activator maps exceptions to failed result", ActiveSaveActivatorMapsExceptions);
```

Test:

```csharp
private static void ActiveSaveActivatorMapsExceptions()
{
    var activator = new DelegateActiveSaveActivator(() => throw new InvalidOperationException("bad active save"));

    var result = activator.Activate("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", DateTimeOffset.Parse("2026-05-08T12:00:00Z"));

    AssertEx.False(result.Success);
    AssertEx.Equal("bad active save", result.ErrorMessage);
}
```

- [ ] **Step 2: Run test to verify it fails**

Expected: compile fails because `DelegateActiveSaveActivator` does not exist.

- [ ] **Step 3: Implement STS2 adapters**

Update `HostFlowContinuations.cs` with reusable adapter helpers:

```csharp
public sealed class DelegateActiveSaveActivator : IActiveSaveActivator
{
    private readonly Action<string, DateTimeOffset> _activate;

    public DelegateActiveSaveActivator(Action<string, DateTimeOffset> activate)
    {
        _activate = activate;
    }

    public OperationResult Activate(string campaignId, DateTimeOffset nowUtc)
    {
        try
        {
            _activate(campaignId, nowUtc);
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }
}
```

Create `src/Runtime/Sts2HostFlowRuntime.cs`:

```csharp
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Storage;

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
            MultiplayerHostSubmenuPatch.ResumeVanillaStart(_hostSubmenu, Sts2HostFlowRuntime.ModeMap.ToSts2GameMode(gameMode));
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
            var platformType = PlatformUtil.PrimaryPlatform;
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
}
```

The `MultiplayerHostSubmenuPatch.ResumeVanillaStart` helper is implemented in Task 5.

- [ ] **Step 4: Run build to surface adapter compile gaps**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
```

Expected before Task 5: compile fails only because `MultiplayerHostSubmenuPatch.ResumeVanillaStart` is missing.

- [ ] **Step 5: Defer commit until Task 5**

Do not commit this task alone if it does not compile. Continue directly into Task 5.

## Task 4: Code-Built Picker Modal

**Files:**
- Create: `src/UI/MultiplayerSavePickerModal.cs`
- Modify: `src/UI/MultiplayerSavePickerModel.cs`

- [ ] **Step 1: Build the modal UI**

Create `src/UI/MultiplayerSavePickerModal.cs`:

```csharp
using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.UI;

public sealed partial class MultiplayerSavePickerModal : Control, IScreenContext
{
    private readonly HostFlowController _controller;
    private readonly MultiplayerSavePickerModel _model;
    private Control? _defaultFocusedControl;

    private MultiplayerSavePickerModal(HostFlowController controller, MultiplayerSavePickerModel model)
    {
        _controller = controller;
        _model = model;
        Name = "MultiplayerSavePickerModal";
    }

    public Control? DefaultFocusedControl => _defaultFocusedControl;

    public static void Show(HostFlowController controller, MultiplayerGameMode gameMode)
    {
        var model = controller.BuildPickerModel(gameMode);
        NModalContainer.Instance?.Add(new MultiplayerSavePickerModal(controller, model));
    }

    public override void _Ready()
    {
        AnchorLeft = 0;
        AnchorTop = 0;
        AnchorRight = 1;
        AnchorBottom = 1;

        var panel = new PanelContainer
        {
            CustomMinimumSize = new Vector2(720, 520)
        };
        panel.SetAnchorsPreset(LayoutPreset.Center);
        panel.OffsetLeft = -360;
        panel.OffsetTop = -260;
        panel.OffsetRight = 360;
        panel.OffsetBottom = 260;
        AddChild(panel);

        var root = new VBoxContainer();
        root.AddThemeConstantOverride("separation", 12);
        panel.AddChild(root);

        root.AddChild(new Label
        {
            Text = $"Multiplayer Saves - {_model.GameMode}",
            HorizontalAlignment = HorizontalAlignment.Center
        });

        var scroll = new ScrollContainer
        {
            SizeFlagsVertical = SizeFlags.ExpandFill
        };
        root.AddChild(scroll);

        var rows = new VBoxContainer();
        rows.AddThemeConstantOverride("separation", 8);
        scroll.AddChild(rows);

        foreach (var row in _model.Rows)
        {
            var button = new Button
            {
                Text = string.IsNullOrWhiteSpace(row.Subtitle) ? row.Title : $"{row.Title}\n{row.Subtitle}",
                CustomMinimumSize = new Vector2(640, 56),
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            button.Pressed += () => SelectRow(row);
            rows.AddChild(button);
            _defaultFocusedControl ??= button;
        }

        var cancel = new Button { Text = "Cancel", CustomMinimumSize = new Vector2(180, 44) };
        cancel.Pressed += Close;
        root.AddChild(cancel);
        _defaultFocusedControl ??= cancel;
    }

    private void SelectRow(MultiplayerSavePickerRow row)
    {
        OperationResult result = row.Kind == PickerRowKind.StartNewRun
            ? _controller.SelectStartNewRun(_model.GameMode)
            : _controller.SelectExistingCampaign(row.CampaignId!, _model.GameMode);

        if (result.Success)
        {
            Close();
            return;
        }

        ShowError(result.ErrorMessage ?? "Unable to continue multiplayer host flow.");
    }

    private static void ShowError(string message)
    {
        var popup = NErrorPopup.Create("Multiplayer Save Slots", message, showReportBugButton: false);
        if (popup is not null)
            NModalContainer.Instance?.Add(popup);
    }

    private static void Close()
    {
        NModalContainer.Instance?.Clear();
    }
}
```

- [ ] **Step 2: Run build**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
```

Expected before Task 5: compile fails only for the missing patch helper if Task 3 is already present.

## Task 5: Harmony Patches

**Files:**
- Create: `src/Patches/MultiplayerSubmenuPatch.cs`
- Create: `src/Patches/MultiplayerHostSubmenuPatch.cs`
- Create: `tests/HostFlowPatchTests.cs`
- Modify: `tests/TestProgram.cs`
- Modify: `src/Runtime/Sts2HostFlowRuntime.cs`

- [ ] **Step 1: Write failing mapping/guard tests**

Register:

```csharp
tests.AddRange(HostFlowPatchTests.All());
```

Create `tests/HostFlowPatchTests.cs`:

```csharp
using MegaCrit.Sts2.Core.Runs;
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Runtime;

namespace MultiplayerSaveSlots.Tests;

public static class HostFlowPatchTests
{
    public static IEnumerable<TestCase> All()
    {
        yield return new TestCase("game mode map handles standard daily and custom", GameModeMapHandlesAllModes);
    }

    private static void GameModeMapHandlesAllModes()
    {
        var map = new MultiplayerSaveGameModeMap();

        AssertEx.Equal(MultiplayerGameMode.Standard, map.ToMultiplayerGameMode(GameMode.Standard));
        AssertEx.Equal(MultiplayerGameMode.Daily, map.ToMultiplayerGameMode(GameMode.Daily));
        AssertEx.Equal(MultiplayerGameMode.Custom, map.ToMultiplayerGameMode(GameMode.Custom));
        AssertEx.Equal(GameMode.Standard, map.ToSts2GameMode(MultiplayerGameMode.Standard));
        AssertEx.Equal(GameMode.Daily, map.ToSts2GameMode(MultiplayerGameMode.Daily));
        AssertEx.Equal(GameMode.Custom, map.ToSts2GameMode(MultiplayerGameMode.Custom));
    }
}
```

- [ ] **Step 2: Run tests to verify failure**

Expected: compile fails until patch/runtime files exist and compile.

- [ ] **Step 3: Implement submenu host-visibility patch**

Create `src/Patches/MultiplayerSubmenuPatch.cs`:

```csharp
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

namespace MultiplayerSaveSlots.Patches;

[HarmonyPatch(typeof(NMultiplayerSubmenu), "UpdateButtons")]
public static class MultiplayerSubmenuPatch
{
    [HarmonyPostfix]
    public static void Postfix(NMultiplayerSubmenu __instance)
    {
        var hostButton = Traverse.Create(__instance).Field("_hostButton").GetValue<NSubmenuButton>();
        if (hostButton is not null)
            hostButton.Visible = true;
    }
}
```

- [ ] **Step 4: Implement host gamemode intercept patch**

Create `src/Patches/MultiplayerHostSubmenuPatch.cs`:

```csharp
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Runs;
using MultiplayerSaveSlots.Runtime;
using MultiplayerSaveSlots.UI;

namespace MultiplayerSaveSlots.Patches;

[HarmonyPatch(typeof(NMultiplayerHostSubmenu), nameof(NMultiplayerHostSubmenu.StartHost))]
public static class MultiplayerHostSubmenuPatch
{
    private static bool _resumingVanilla;

    [HarmonyPrefix]
    public static bool Prefix(NMultiplayerHostSubmenu __instance, GameMode gameMode)
    {
        if (_resumingVanilla)
            return true;

        var controller = Sts2HostFlowRuntime.CreateController(__instance);
        var multiplayerGameMode = Sts2HostFlowRuntime.ModeMap.ToMultiplayerGameMode(gameMode);
        MultiplayerSavePickerModal.Show(controller, multiplayerGameMode);
        return false;
    }

    public static void ResumeVanillaStart(NMultiplayerHostSubmenu hostSubmenu, GameMode gameMode)
    {
        _resumingVanilla = true;
        try
        {
            hostSubmenu.StartHost(gameMode);
        }
        finally
        {
            _resumingVanilla = false;
        }
    }
}
```

Add this using to `src/Runtime/Sts2HostFlowRuntime.cs`:

```csharp
using MultiplayerSaveSlots.Patches;
```

- [ ] **Step 5: Run build and tests**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: build succeeds and all tests pass.

- [ ] **Step 6: Commit**

```bash
git add src/Runtime src/UI src/Patches tests
git commit -m "feat: add multiplayer host save picker"
```

## Task 6: Docs And In-Game Smoke Checklist

**Files:**
- Modify: `README.md`
- Modify: `docs/implementation/hook-discovery.md`

- [ ] **Step 1: Update README status**

Add:

```markdown
## Phase 2 Status

The host-flow picker phase adds a modal after `Multiplayer -> Host -> Select Gamemode`.
It can start a new vanilla run or activate an existing campaign payload from the save bank before resuming the vanilla loaded-run host flow.

Save sync after vanilla writes is not complete yet, so newly started runs are not finalized into separate bank campaigns until the next phase.
```

- [ ] **Step 2: Add smoke test checklist**

Add:

```markdown
## In-Game Smoke Test

1. Build the mod.
2. Copy `MultiplayerSaveSlots.json` and `bin/Debug/MultiplayerSaveSlots.dll` to `<STS2>/mods/MultiplayerSaveSlots/`.
3. Launch STS2 with mods enabled.
4. Open `Multiplayer`.
5. Confirm `Host` is visible even when a current multiplayer save exists.
6. Select `Host -> Standard`.
7. Confirm the `Multiplayer Saves - Standard` picker appears.
8. Choose `Start New Run`.
9. Confirm vanilla hosting continues.
```

- [ ] **Step 3: Update hook discovery**

Append:

```markdown
## Phase 2 Hook Selection

Phase 2 uses a postfix on `NMultiplayerSubmenu.UpdateButtons()` to keep the Host button visible even when vanilla detects a current multiplayer save.

Phase 2 uses a prefix on `NMultiplayerHostSubmenu.StartHost(GameMode)` as the picker insertion point. This method runs after Standard/Daily/Custom has been selected and before `StartHostAsync` starts the vanilla networking flow.

Existing campaign selections activate the campaign payload into `current_run_mp.save`, then call the vanilla loaded-run continuation through `NMultiplayerSubmenu.StartHost(SerializableRun)` where possible.
```

- [ ] **Step 4: Run verification**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: build succeeds and all tests pass.

- [ ] **Step 5: Commit**

```bash
git add README.md docs/implementation/hook-discovery.md
git commit -m "docs: document host flow picker testing"
```

## Final Verification

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
git status --short --branch
```

Expected:

- build succeeds
- tests pass
- branch contains only intended Phase 2 commits

## Implementation Notes

- Keep `multisave-multiplayer` out of all exploration and implementation.
- Do not add `.pck`, `.tscn`, or Godot editor-generated assets in this phase.
- Do not duplicate vanilla multiplayer host networking code because other mods may patch the vanilla methods, including lobby-limit removal mods.
- Private-field traversal is acceptable for menu UI visibility and stack lookup in this phase; do not use it to mutate save payloads.
- Existing campaign activation may fail with checksum/ownership errors. Show the error and stop instead of continuing.
- `Start New Run` is vanilla continuation only in this phase. Save-bank finalization belongs to the save-sync phase.
