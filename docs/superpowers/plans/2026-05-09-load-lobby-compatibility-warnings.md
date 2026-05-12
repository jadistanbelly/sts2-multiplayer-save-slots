# Load-Lobby Compatibility Warnings Implementation Plan

> **Archived historical plan:** This document is preserved for context only. It is not an active implementation plan; unchecked checklist items below do not indicate pending work. See `docs/superpowers/README.md` for archive policy.

> **Archived worker note:** This was the original execution guidance. Do not execute this document as current work unless a new issue or branch explicitly reactivates it.

**Goal:** Warn hosts at loaded-run embark time when the selected campaign roster appears to differ from the current load lobby participant ids.

**Architecture:** Add a pure compatibility checker and a small runtime guard, then patch `NMultiplayerLoadGameScreen.ShouldAllowRunToBegin()` by wrapping its returned task. The guard fails open on errors and shows each exact mismatch once through the existing modal error-popup path.

**Tech Stack:** C# `net9.0`, Harmony, STS2/Godot assemblies, existing console test runner, existing smoke setup shell tests.

---

### Task 1: Add Compatibility Checker

**Files:**
- Create: `src/Runtime/CampaignCompatibilityChecker.cs`
- Modify: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing checker tests**

Add tests for matching rosters, missing expected players, extra current players, empty expected roster, missing stable ids, and stable warning keys.

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: tests fail because `CampaignCompatibilityChecker` does not exist.

- [ ] **Step 2: Implement checker**

Create `CampaignCompatibilityWarning` and `CampaignCompatibilityChecker`. Use only non-empty `StableId` values for comparison and produce compact missing/extra display text from player display names.

- [ ] **Step 3: Verify checker tests**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 4: Commit**

```bash
git add src/Runtime/CampaignCompatibilityChecker.cs tests/HostFlowControllerTests.cs
git commit -m "feat: add campaign compatibility checker"
```

### Task 2: Add Session Acknowledgement And Guard

**Files:**
- Modify: `src/Runtime/HostFlowSession.cs`
- Create: `src/Runtime/LoadLobbyCompatibilityGuard.cs`
- Modify: `tests/HostFlowControllerTests.cs`

- [ ] **Step 1: Write failing guard tests**

Add tests proving the first identical mismatch warns and returns false, the second identical mismatch allows, and no selected campaign allows.

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: tests fail because session acknowledgement and the guard do not exist.

- [ ] **Step 2: Implement session acknowledgement and guard**

Add `ShouldShowCompatibilityWarning(string warningKey)` to `HostFlowSession`. Implement `LoadLobbyCompatibilityGuard` with injected campaign lookup, current-roster provider, and warning sink.

- [ ] **Step 3: Verify guard tests**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 4: Commit**

```bash
git add src/Runtime/HostFlowSession.cs src/Runtime/LoadLobbyCompatibilityGuard.cs tests/HostFlowControllerTests.cs
git commit -m "feat: add load lobby compatibility guard"
```

### Task 3: Add STS2 Patch And Runtime Wiring

**Files:**
- Create: `src/Patches/LoadLobbyCompatibilityPatch.cs`
- Modify: `src/Runtime/Sts2CampaignMetadataExtractor.cs`
- Modify: `src/Runtime/Sts2HostFlowRuntime.cs`
- Modify: `docs/implementation/hook-discovery.md`
- Modify: `tests/HostFlowPatchTests.cs`

- [ ] **Step 1: Write failing patch tests**

Add tests for the async patch helper: vanilla false stays false, vanilla true invokes the guard, and the patch type exists.

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: tests fail because `LoadLobbyCompatibilityPatch` does not exist.

- [ ] **Step 2: Implement patch and runtime wiring**

Patch `NMultiplayerLoadGameScreen.ShouldAllowRunToBegin()`. Add runtime helpers that read `_runLobby`, build current roster from `LoadRunLobby`, and show warnings with `NErrorPopup`.

- [ ] **Step 3: Verify patch tests**

Run:

```bash
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: build succeeds with 0 warnings/errors and all tests pass.

- [ ] **Step 4: Commit**

```bash
git add src/Patches/LoadLobbyCompatibilityPatch.cs src/Runtime/Sts2CampaignMetadataExtractor.cs src/Runtime/Sts2HostFlowRuntime.cs docs/implementation/hook-discovery.md tests/HostFlowPatchTests.cs
git commit -m "feat: warn on load lobby party mismatch"
```

### Task 4: Document And Verify

**Files:**
- Modify: `README.md`

- [ ] **Step 1: Update README status and smoke checklist**

Mention Phase 10 compatibility warnings and add a smoke checklist item for selecting an existing campaign, changing the participant set, pressing Embark, and confirming the warning appears once.

- [ ] **Step 2: Run full verification**

Run:

```bash
rg -n "Phase 10|compatibility warning|Embark" README.md docs/implementation/hook-discovery.md
DOTNET_ROLL_FORWARD=Major dotnet build MultiplayerSaveSlots.sln
DOTNET_ROLL_FORWARD=Major dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
tests/smoke-setup-local-tests.sh
bash -n scripts/smoke-setup-local.sh tests/smoke-setup-local-tests.sh
git diff --check
```

Expected: README and hook-discovery contain the new text, build succeeds, unit tests pass, smoke setup tests pass, shell syntax passes, and `git diff --check` prints nothing.

- [ ] **Step 3: Commit**

```bash
git add README.md
git commit -m "docs: document load lobby compatibility warnings"
```
