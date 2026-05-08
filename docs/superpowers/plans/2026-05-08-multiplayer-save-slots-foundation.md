# Multiplayer Save Slots Foundation Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build the clean STS2 mod scaffold and the tested save-bank/active-slot swap foundation for Multiplayer Save Slots.

**Architecture:** Phase 1 keeps all risky behavior in pure file-system services before patching live game flow. The mod will be a `net9.0` C# DLL with Harmony initialization, while the save bank, metadata, checksum, backup, and active-save switcher are plain C# classes covered by a no-NuGet console test harness. A small inspector tool will document concrete STS2 hook candidates for the next implementation plan.

**Tech Stack:** C# `net9.0`, `System.Text.Json`, `System.Security.Cryptography.SHA256`, Harmony, STS2/Godot assemblies, and future release automation. Tests use a local console runner instead of xUnit/NUnit to avoid external package dependencies.

---

## Scope Check

The approved design includes three coupled areas:

- core save-bank and active-slot swap behavior
- in-game picker UI and host-flow patches
- save-sync Harmony patches

This plan implements the first area and produces a discovery report for the patch areas. Do not implement the picker or Harmony save-sync hooks in this phase; exact method signatures must be confirmed against the installed STS2 assembly first.

## File Structure

- `MultiplayerSaveSlots.sln` - solution for mod, tests, and tools.
- `MultiplayerSaveSlots.csproj` - STS2 mod DLL project.
- `MultiplayerSaveSlots.json` - STS2 mod manifest.
- `src/MultiplayerSaveSlotsMod.cs` - mod initializer and Harmony bootstrap.
- `src/Core/MultiplayerGameMode.cs` - supported multiplayer gamemodes.
- `src/Core/PlayerIdentity.cs` - display name and stable player id model.
- `src/Core/CampaignMetadata.cs` - campaign metadata stored beside save payloads.
- `src/Core/CampaignIndex.cs` - save-bank index format.
- `src/Core/CampaignCreateRequest.cs` - creation input for new campaigns.
- `src/Core/CampaignLabeler.cs` - compact auto-label generation.
- `src/Storage/FileChecksum.cs` - SHA-256 helper.
- `src/Storage/BackupManager.cs` - timestamped backup creation.
- `src/Storage/JsonFile.cs` - consistent JSON read/write helper.
- `src/Storage/SaveBankPaths.cs` - path calculation for the bank and active save.
- `src/Storage/MultiplayerSaveBank.cs` - campaign create/list/load/update behavior.
- `src/Storage/ActiveSaveState.cs` - ownership/checksum record for the active slot.
- `src/Storage/ActiveSaveSwitcher.cs` - activate and sync-back operations.
- `tests/MultiplayerSaveSlots.Tests.csproj` - no-NuGet console test project.
- `tests/TestProgram.cs` - test runner entry point.
- `tests/TestCase.cs` - tiny test registration model.
- `tests/AssertEx.cs` - assertion helpers.
- `tests/TempDirectory.cs` - disposable temp directory helper.
- `tests/CampaignLabelerTests.cs` - label-generation tests.
- `tests/MultiplayerSaveBankTests.cs` - save-bank tests.
- `tests/ActiveSaveSwitcherTests.cs` - activation/sync safety tests.
- `tools/Sts2Inspector/Sts2Inspector.csproj` - reflection tool for STS2 assembly discovery.
- `tools/Sts2Inspector/Program.cs` - prints/save hook candidates.
- `docs/implementation/hook-discovery.md` - checked-in results from the inspector.
- `README.md` - local development notes.

## Prerequisite

- [ ] **Step 1: Verify the build can target .NET 9**

Run:

```bash
dotnet --info
find "$HOME/.nuget/packages/microsoft.netcore.app.ref" -maxdepth 1 -mindepth 1 -type d -printf '%f\n' 2>/dev/null | rg '^9\.'
```

Expected: a .NET SDK is available and the second command prints at least one `9.x` targeting-pack version. The SDK itself may be .NET 10; the mod still targets `net9.0` because STS2 targets `.NETCoreApp,Version=v9.0` and the local `autopath` mod targets `net9.0`.

### Task 1: Scaffold Mod Project

**Files:**
- Create: `MultiplayerSaveSlots.sln`
- Create: `MultiplayerSaveSlots.csproj`
- Create: `MultiplayerSaveSlots.json`
- Create: `src/MultiplayerSaveSlotsMod.cs`
- Modify: `.gitignore`

- [ ] **Step 1: Create `MultiplayerSaveSlots.csproj`**

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <EnableDynamicLoading>true</EnableDynamicLoading>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
    <CopyLocalLockFileAssemblies>false</CopyLocalLockFileAssemblies>
  </PropertyGroup>

  <PropertyGroup>
    <STS2Path Condition="$([MSBuild]::IsOSPlatform('Linux'))">$(HOME)/.local/share/Steam/steamapps/common/Slay the Spire 2</STS2Path>
    <STS2Path Condition="$([MSBuild]::IsOSPlatform('OSX'))">$(HOME)/Library/Application Support/Steam/steamapps/common/Slay the Spire 2</STS2Path>
    <STS2Path Condition="$([MSBuild]::IsOSPlatform('Windows'))">C:/Program Files (x86)/Steam/steamapps/common/Slay the Spire 2</STS2Path>

    <STS2DataPath Condition="$([MSBuild]::IsOSPlatform('Linux'))">$(STS2Path)/data_sts2_linuxbsd_x86_64</STS2DataPath>
    <STS2DataPath Condition="$([MSBuild]::IsOSPlatform('OSX'))">$(STS2Path)/data_sts2_macos_universal</STS2DataPath>
    <STS2DataPath Condition="$([MSBuild]::IsOSPlatform('Windows'))">$(STS2Path)/data_sts2_windows_x86_64</STS2DataPath>
  </PropertyGroup>

  <ItemGroup>
    <Reference Include="sts2">
      <HintPath>$(STS2DataPath)/sts2.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="GodotSharp">
      <HintPath>$(STS2DataPath)/GodotSharp.dll</HintPath>
      <Private>false</Private>
    </Reference>
    <Reference Include="0Harmony">
      <HintPath>$(STS2DataPath)/0Harmony.dll</HintPath>
      <Private>false</Private>
    </Reference>
  </ItemGroup>
</Project>
```

- [ ] **Step 2: Create `MultiplayerSaveSlots.json`**

```json
{
  "id": "MultiplayerSaveSlots",
  "name": "Multiplayer Save Slots",
  "author": "jadistanbelly",
  "description": "Adds multiple host-owned multiplayer save slots while preserving vanilla party validation.",
  "version": "0.1.0",
  "has_pck": false,
  "has_dll": true,
  "dependencies": [],
  "affects_gameplay": false
}
```

- [ ] **Step 3: Create `src/MultiplayerSaveSlotsMod.cs`**

```csharp
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace MultiplayerSaveSlots;

[ModInitializer(nameof(Initialize))]
public static class MultiplayerSaveSlotsMod
{
    internal static Harmony? Harmony { get; private set; }

    public static void Initialize()
    {
        Harmony = new Harmony("com.jadistanbelly.multiplayersaveslots");
        Harmony.PatchAll(typeof(MultiplayerSaveSlotsMod).Assembly);
        GD.Print("[MultiplayerSaveSlots] Initialized");
    }
}
```

- [ ] **Step 4: Create the solution**

Run:

```bash
dotnet new sln -n MultiplayerSaveSlots
dotnet sln MultiplayerSaveSlots.sln add MultiplayerSaveSlots.csproj
```

Expected: `MultiplayerSaveSlots.sln` exists and includes `MultiplayerSaveSlots.csproj`.

- [ ] **Step 5: Update `.gitignore`**

Ensure it contains:

```gitignore
.superpowers/
bin/
obj/
.vs/
*.user
*.suo
TestResults/
```

- [ ] **Step 6: Build the scaffold**

Run:

```bash
dotnet build MultiplayerSaveSlots.sln
```

Expected: build succeeds.

- [ ] **Step 7: Commit**

```bash
git add .gitignore MultiplayerSaveSlots.sln MultiplayerSaveSlots.csproj MultiplayerSaveSlots.json src/MultiplayerSaveSlotsMod.cs
git commit -m "feat: scaffold multiplayer save slots mod"
```

### Task 2: Add No-NuGet Test Harness

**Files:**
- Create: `tests/MultiplayerSaveSlots.Tests.csproj`
- Create: `tests/TestProgram.cs`
- Create: `tests/TestCase.cs`
- Create: `tests/AssertEx.cs`
- Create: `tests/TempDirectory.cs`
- Modify: `MultiplayerSaveSlots.sln`

- [ ] **Step 1: Create `tests/MultiplayerSaveSlots.Tests.csproj`**

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="../MultiplayerSaveSlots.csproj" />
  </ItemGroup>
</Project>
```

- [ ] **Step 2: Create `tests/TestCase.cs`**

```csharp
namespace MultiplayerSaveSlots.Tests;

public sealed record TestCase(string Name, Action Run);
```

- [ ] **Step 3: Create `tests/AssertEx.cs`**

```csharp
namespace MultiplayerSaveSlots.Tests;

public static class AssertEx
{
    public static void Equal<T>(T expected, T actual, string? message = null)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new InvalidOperationException(message ?? $"Expected {expected}, got {actual}");
    }

    public static void True(bool condition, string? message = null)
    {
        if (!condition)
            throw new InvalidOperationException(message ?? "Expected condition to be true");
    }

    public static void False(bool condition, string? message = null)
    {
        if (condition)
            throw new InvalidOperationException(message ?? "Expected condition to be false");
    }

    public static void Throws<TException>(Action action) where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException)
        {
            return;
        }

        throw new InvalidOperationException($"Expected exception {typeof(TException).Name}");
    }
}
```

- [ ] **Step 4: Create `tests/TempDirectory.cs`**

```csharp
namespace MultiplayerSaveSlots.Tests;

public sealed class TempDirectory : IDisposable
{
    public string Path { get; } = System.IO.Path.Combine(
        System.IO.Path.GetTempPath(),
        "mss-tests",
        Guid.NewGuid().ToString("N"));

    public TempDirectory()
    {
        Directory.CreateDirectory(Path);
    }

    public void Dispose()
    {
        if (Directory.Exists(Path))
            Directory.Delete(Path, recursive: true);
    }
}
```

- [ ] **Step 5: Create `tests/TestProgram.cs`**

```csharp
using MultiplayerSaveSlots.Tests;

var tests = new List<TestCase>();
CampaignLabelerTests.Register(tests);
MultiplayerSaveBankTests.Register(tests);
ActiveSaveSwitcherTests.Register(tests);

var failures = 0;
foreach (var test in tests)
{
    try
    {
        test.Run();
        Console.WriteLine($"PASS {test.Name}");
    }
    catch (Exception ex)
    {
        failures++;
        Console.Error.WriteLine($"FAIL {test.Name}: {ex.Message}");
    }
}

Console.WriteLine($"{tests.Count - failures}/{tests.Count} tests passed");
return failures == 0 ? 0 : 1;
```

- [ ] **Step 6: Add empty test registration classes so the runner compiles**

Create `tests/CampaignLabelerTests.cs`:

```csharp
namespace MultiplayerSaveSlots.Tests;

public static class CampaignLabelerTests
{
    public static void Register(List<TestCase> tests)
    {
    }
}
```

Create `tests/MultiplayerSaveBankTests.cs`:

```csharp
namespace MultiplayerSaveSlots.Tests;

public static class MultiplayerSaveBankTests
{
    public static void Register(List<TestCase> tests)
    {
    }
}
```

Create `tests/ActiveSaveSwitcherTests.cs`:

```csharp
namespace MultiplayerSaveSlots.Tests;

public static class ActiveSaveSwitcherTests
{
    public static void Register(List<TestCase> tests)
    {
    }
}
```

- [ ] **Step 7: Add tests project to solution**

Run:

```bash
dotnet sln MultiplayerSaveSlots.sln add tests/MultiplayerSaveSlots.Tests.csproj
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: `0/0 tests passed` and exit code `0`.

- [ ] **Step 8: Commit**

```bash
git add MultiplayerSaveSlots.sln tests
git commit -m "test: add console test harness"
```

### Task 3: Implement Domain Model and Campaign Labels

**Files:**
- Create: `src/Core/MultiplayerGameMode.cs`
- Create: `src/Core/PlayerIdentity.cs`
- Create: `src/Core/CampaignMetadata.cs`
- Create: `src/Core/CampaignIndex.cs`
- Create: `src/Core/CampaignCreateRequest.cs`
- Create: `src/Core/CampaignLabeler.cs`
- Modify: `tests/CampaignLabelerTests.cs`

- [ ] **Step 1: Replace `tests/CampaignLabelerTests.cs` with failing tests**

```csharp
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Tests;

public static class CampaignLabelerTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("labeler uses one player name", OnePlayer));
        tests.Add(new TestCase("labeler uses two player names", TwoPlayers));
        tests.Add(new TestCase("labeler compacts large roster", ManyPlayers));
        tests.Add(new TestCase("labeler handles empty roster", EmptyRoster));
    }

    private static void OnePlayer()
    {
        var label = CampaignLabeler.Build([
            new PlayerIdentity("steam:1", "buddy1")
        ]);

        AssertEx.Equal("buddy1", label);
    }

    private static void TwoPlayers()
    {
        var label = CampaignLabeler.Build([
            new PlayerIdentity("steam:1", "buddy1"),
            new PlayerIdentity("steam:2", "buddy2")
        ]);

        AssertEx.Equal("buddy1 + buddy2", label);
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

        AssertEx.Equal("buddy1 + buddy2 + 3 more", label);
    }

    private static void EmptyRoster()
    {
        AssertEx.Equal("Unknown party", CampaignLabeler.Build([]));
    }
}
```

- [ ] **Step 2: Run tests and verify failure**

Run:

```bash
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: build fails because `MultiplayerSaveSlots.Core` types do not exist.

- [ ] **Step 3: Create `src/Core/MultiplayerGameMode.cs`**

```csharp
namespace MultiplayerSaveSlots.Core;

public enum MultiplayerGameMode
{
    Standard,
    Daily,
    Custom
}
```

- [ ] **Step 4: Create `src/Core/PlayerIdentity.cs`**

```csharp
namespace MultiplayerSaveSlots.Core;

public sealed record PlayerIdentity(string? StableId, string DisplayName);
```

- [ ] **Step 5: Create `src/Core/CampaignMetadata.cs`**

```csharp
namespace MultiplayerSaveSlots.Core;

public sealed record CampaignMetadata(
    string CampaignId,
    MultiplayerGameMode GameMode,
    string Label,
    IReadOnlyList<PlayerIdentity> Roster,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset LastPlayedAtUtc,
    string? ActiveChecksum,
    string? PayloadChecksum,
    string? ActOrFloor);
```

- [ ] **Step 6: Create `src/Core/CampaignIndex.cs`**

```csharp
namespace MultiplayerSaveSlots.Core;

public sealed record CampaignIndex(IReadOnlyList<string> CampaignIds)
{
    public static CampaignIndex Empty { get; } = new([]);
}
```

- [ ] **Step 7: Create `src/Core/CampaignCreateRequest.cs`**

```csharp
namespace MultiplayerSaveSlots.Core;

public sealed record CampaignCreateRequest(
    MultiplayerGameMode GameMode,
    IReadOnlyList<PlayerIdentity> Roster,
    string SavePayloadPath,
    DateTimeOffset CreatedAtUtc);
```

- [ ] **Step 8: Create `src/Core/CampaignLabeler.cs`**

```csharp
namespace MultiplayerSaveSlots.Core;

public static class CampaignLabeler
{
    public static string Build(IReadOnlyList<PlayerIdentity> roster)
    {
        var names = roster
            .Select(player => string.IsNullOrWhiteSpace(player.DisplayName)
                ? "Unknown"
                : player.DisplayName.Trim())
            .ToList();

        return names.Count switch
        {
            0 => "Unknown party",
            1 => names[0],
            2 => $"{names[0]} + {names[1]}",
            _ => $"{names[0]} + {names[1]} + {names.Count - 2} more"
        };
    }
}
```

- [ ] **Step 9: Run tests and verify pass**

Run:

```bash
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: `4/4 tests passed`.

- [ ] **Step 10: Commit**

```bash
git add src/Core tests/CampaignLabelerTests.cs
git commit -m "feat: add campaign metadata model"
```

### Task 4: Implement Checksums, JSON, and Backups

**Files:**
- Create: `src/Storage/FileChecksum.cs`
- Create: `src/Storage/JsonFile.cs`
- Create: `src/Storage/BackupManager.cs`
- Create: `tests/StorageUtilityTests.cs`
- Modify: `tests/TestProgram.cs`

- [ ] **Step 1: Add storage utility tests**

Create `tests/StorageUtilityTests.cs`:

```csharp
using MultiplayerSaveSlots.Storage;

namespace MultiplayerSaveSlots.Tests;

public static class StorageUtilityTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("checksum changes with file content", ChecksumChanges));
        tests.Add(new TestCase("backup copies source file", BackupCopiesSource));
    }

    private static void ChecksumChanges()
    {
        using var temp = new TempDirectory();
        var file = Path.Combine(temp.Path, "save.dat");

        File.WriteAllText(file, "first");
        var first = FileChecksum.Sha256(file);

        File.WriteAllText(file, "second");
        var second = FileChecksum.Sha256(file);

        AssertEx.False(first == second);
    }

    private static void BackupCopiesSource()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "active.save");
        var backupDir = Path.Combine(temp.Path, "backup");
        File.WriteAllText(source, "payload");

        var backup = BackupManager.CreateBackup(source, backupDir, "activate", new DateTimeOffset(2026, 5, 8, 12, 30, 0, TimeSpan.Zero));

        AssertEx.True(File.Exists(backup));
        AssertEx.Equal("payload", File.ReadAllText(backup));
        AssertEx.True(Path.GetFileName(backup).Contains("activate"));
    }
}
```

Modify `tests/TestProgram.cs` so registration includes:

```csharp
StorageUtilityTests.Register(tests);
```

- [ ] **Step 2: Run tests and verify failure**

Run:

```bash
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: build fails because `MultiplayerSaveSlots.Storage` types do not exist.

- [ ] **Step 3: Create `src/Storage/FileChecksum.cs`**

```csharp
using System.Security.Cryptography;

namespace MultiplayerSaveSlots.Storage;

public static class FileChecksum
{
    public static string Sha256(string path)
    {
        using var stream = File.OpenRead(path);
        var hash = SHA256.HashData(stream);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
```

- [ ] **Step 4: Create `src/Storage/JsonFile.cs`**

```csharp
using System.Text.Json;

namespace MultiplayerSaveSlots.Storage;

public static class JsonFile
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static T Read<T>(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, Options)
            ?? throw new InvalidOperationException($"Could not deserialize {path}");
    }

    public static void Write<T>(string path, T value)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)
            ?? throw new InvalidOperationException($"Path has no directory: {path}"));

        var tempPath = path + ".tmp";
        File.WriteAllText(tempPath, JsonSerializer.Serialize(value, Options));
        File.Move(tempPath, path, overwrite: true);
    }
}
```

- [ ] **Step 5: Create `src/Storage/BackupManager.cs`**

```csharp
namespace MultiplayerSaveSlots.Storage;

public static class BackupManager
{
    public static string CreateBackup(string sourcePath, string backupDirectory, string reason, DateTimeOffset timestampUtc)
    {
        if (!File.Exists(sourcePath))
            throw new FileNotFoundException("Cannot back up missing file", sourcePath);

        Directory.CreateDirectory(backupDirectory);
        var safeReason = string.Concat(reason.Select(ch => char.IsLetterOrDigit(ch) ? ch : '-'));
        var fileName = $"{timestampUtc:yyyyMMdd-HHmmss}-{safeReason}-{Path.GetFileName(sourcePath)}";
        var destination = Path.Combine(backupDirectory, fileName);
        File.Copy(sourcePath, destination, overwrite: false);
        return destination;
    }
}
```

- [ ] **Step 6: Run tests and verify pass**

Run:

```bash
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 7: Commit**

```bash
git add src/Storage tests/StorageUtilityTests.cs tests/TestProgram.cs
git commit -m "feat: add storage utilities"
```

### Task 5: Implement Save Bank

**Files:**
- Create: `src/Storage/SaveBankPaths.cs`
- Create: `src/Storage/MultiplayerSaveBank.cs`
- Modify: `tests/MultiplayerSaveBankTests.cs`

- [ ] **Step 1: Replace `tests/MultiplayerSaveBankTests.cs` with failing tests**

```csharp
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Storage;

namespace MultiplayerSaveSlots.Tests;

public static class MultiplayerSaveBankTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("save bank creates campaign with payload and metadata", CreatesCampaign));
        tests.Add(new TestCase("save bank lists campaigns by gamemode", ListsByGameMode));
    }

    private static void CreatesCampaign()
    {
        using var temp = new TempDirectory();
        var payload = Path.Combine(temp.Path, "vanilla.save");
        File.WriteAllText(payload, "run-payload");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(
            MultiplayerGameMode.Standard,
            [new PlayerIdentity("steam:1", "buddy1")],
            payload,
            new DateTimeOffset(2026, 5, 8, 12, 0, 0, TimeSpan.Zero)));

        var campaignDir = Path.Combine(temp.Path, "MultiSaves", "saves", metadata.CampaignId);
        AssertEx.True(File.Exists(Path.Combine(campaignDir, "metadata.json")));
        AssertEx.True(File.Exists(Path.Combine(campaignDir, "multiplayer_run.save")));
        AssertEx.Equal("buddy1", metadata.Label);
        AssertEx.Equal("run-payload", File.ReadAllText(Path.Combine(campaignDir, "multiplayer_run.save")));
    }

    private static void ListsByGameMode()
    {
        using var temp = new TempDirectory();
        var standardPayload = Path.Combine(temp.Path, "standard.save");
        var dailyPayload = Path.Combine(temp.Path, "daily.save");
        File.WriteAllText(standardPayload, "standard");
        File.WriteAllText(dailyPayload, "daily");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], standardPayload, DateTimeOffset.UtcNow));
        bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Daily, [], dailyPayload, DateTimeOffset.UtcNow));

        var standard = bank.ListCampaigns(MultiplayerGameMode.Standard);
        var daily = bank.ListCampaigns(MultiplayerGameMode.Daily);

        AssertEx.Equal(1, standard.Count);
        AssertEx.Equal(1, daily.Count);
        AssertEx.Equal(MultiplayerGameMode.Standard, standard[0].GameMode);
        AssertEx.Equal(MultiplayerGameMode.Daily, daily[0].GameMode);
    }
}
```

- [ ] **Step 2: Run tests and verify failure**

Run:

```bash
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: build fails because `SaveBankPaths` and `MultiplayerSaveBank` do not exist.

- [ ] **Step 3: Create `src/Storage/SaveBankPaths.cs`**

```csharp
namespace MultiplayerSaveSlots.Storage;

public sealed class SaveBankPaths
{
    public SaveBankPaths(string rootDirectory)
    {
        RootDirectory = rootDirectory;
    }

    public string RootDirectory { get; }
    public string IndexPath => Path.Combine(RootDirectory, "index.json");
    public string SavesDirectory => Path.Combine(RootDirectory, "saves");

    public string CampaignDirectory(string campaignId) => Path.Combine(SavesDirectory, campaignId);
    public string MetadataPath(string campaignId) => Path.Combine(CampaignDirectory(campaignId), "metadata.json");
    public string PayloadPath(string campaignId) => Path.Combine(CampaignDirectory(campaignId), "multiplayer_run.save");
    public string BackupDirectory(string campaignId) => Path.Combine(CampaignDirectory(campaignId), "backup");
}
```

- [ ] **Step 4: Create `src/Storage/MultiplayerSaveBank.cs`**

```csharp
using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Storage;

public sealed class MultiplayerSaveBank
{
    private readonly SaveBankPaths _paths;

    public MultiplayerSaveBank(SaveBankPaths paths)
    {
        _paths = paths;
    }

    public CampaignMetadata CreateCampaign(CampaignCreateRequest request)
    {
        if (!File.Exists(request.SavePayloadPath))
            throw new FileNotFoundException("Save payload does not exist", request.SavePayloadPath);

        EnsureCreated();

        var campaignId = Guid.NewGuid().ToString("N");
        var campaignDir = _paths.CampaignDirectory(campaignId);
        Directory.CreateDirectory(campaignDir);

        var payloadPath = _paths.PayloadPath(campaignId);
        File.Copy(request.SavePayloadPath, payloadPath, overwrite: false);
        var payloadChecksum = FileChecksum.Sha256(payloadPath);

        var metadata = new CampaignMetadata(
            campaignId,
            request.GameMode,
            CampaignLabeler.Build(request.Roster),
            request.Roster,
            request.CreatedAtUtc,
            request.CreatedAtUtc,
            ActiveChecksum: null,
            PayloadChecksum: payloadChecksum,
            ActOrFloor: null);

        JsonFile.Write(_paths.MetadataPath(campaignId), metadata);
        var index = ReadIndex();
        JsonFile.Write(_paths.IndexPath, new CampaignIndex(index.CampaignIds.Concat([campaignId]).ToList()));

        return metadata;
    }

    public IReadOnlyList<CampaignMetadata> ListCampaigns(MultiplayerGameMode gameMode)
    {
        EnsureCreated();
        return ReadIndex().CampaignIds
            .Select(id => JsonFile.Read<CampaignMetadata>(_paths.MetadataPath(id)))
            .Where(metadata => metadata.GameMode == gameMode)
            .OrderByDescending(metadata => metadata.LastPlayedAtUtc)
            .ToList();
    }

    public CampaignMetadata GetCampaign(string campaignId)
    {
        EnsureCreated();
        return JsonFile.Read<CampaignMetadata>(_paths.MetadataPath(campaignId));
    }

    public string GetPayloadPath(string campaignId) => _paths.PayloadPath(campaignId);
    public string GetBackupDirectory(string campaignId) => _paths.BackupDirectory(campaignId);

    public void UpdateMetadata(CampaignMetadata metadata)
    {
        EnsureCreated();
        JsonFile.Write(_paths.MetadataPath(metadata.CampaignId), metadata);
    }

    private void EnsureCreated()
    {
        Directory.CreateDirectory(_paths.RootDirectory);
        Directory.CreateDirectory(_paths.SavesDirectory);
        if (!File.Exists(_paths.IndexPath))
            JsonFile.Write(_paths.IndexPath, CampaignIndex.Empty);
    }

    private CampaignIndex ReadIndex() => File.Exists(_paths.IndexPath)
        ? JsonFile.Read<CampaignIndex>(_paths.IndexPath)
        : CampaignIndex.Empty;
}
```

- [ ] **Step 5: Run tests and verify pass**

Run:

```bash
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 6: Commit**

```bash
git add src/Storage/SaveBankPaths.cs src/Storage/MultiplayerSaveBank.cs tests/MultiplayerSaveBankTests.cs
git commit -m "feat: add multiplayer save bank"
```

### Task 6: Implement Active Save Switcher

**Files:**
- Create: `src/Storage/ActiveSaveState.cs`
- Create: `src/Storage/ActiveSaveSwitcher.cs`
- Modify: `tests/ActiveSaveSwitcherTests.cs`

- [ ] **Step 1: Replace `tests/ActiveSaveSwitcherTests.cs` with failing tests**

```csharp
using MultiplayerSaveSlots.Core;
using MultiplayerSaveSlots.Storage;

namespace MultiplayerSaveSlots.Tests;

public static class ActiveSaveSwitcherTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("switcher activates campaign into active save slot", ActivatesCampaign));
        tests.Add(new TestCase("switcher syncs active save back to selected campaign", SyncsBack));
        tests.Add(new TestCase("switcher rejects sync when active checksum changed before selection", RejectsUnexpectedActiveChange));
    }

    private static void ActivatesCampaign()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);

        AssertEx.Equal("campaign", File.ReadAllText(active));
        AssertEx.True(File.Exists(Path.Combine(temp.Path, "active-state.json")));
    }

    private static void SyncsBack()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.WriteAllText(active, "campaign-progress");
        switcher.SyncBack(DateTimeOffset.UtcNow);

        AssertEx.Equal("campaign-progress", File.ReadAllText(bank.GetPayloadPath(metadata.CampaignId)));
    }

    private static void RejectsUnexpectedActiveChange()
    {
        using var temp = new TempDirectory();
        var source = Path.Combine(temp.Path, "source.save");
        var active = Path.Combine(temp.Path, "active.save");
        File.WriteAllText(source, "campaign");

        var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
        var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
        var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

        switcher.Activate(metadata.CampaignId, DateTimeOffset.UtcNow);
        File.Delete(Path.Combine(temp.Path, "active-state.json"));

        AssertEx.Throws<InvalidOperationException>(() => switcher.SyncBack(DateTimeOffset.UtcNow));
    }
}
```

- [ ] **Step 2: Run tests and verify failure**

Run:

```bash
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: build fails because `ActiveSaveSwitcher` and `ActiveSaveState` do not exist.

- [ ] **Step 3: Create `src/Storage/ActiveSaveState.cs`**

```csharp
namespace MultiplayerSaveSlots.Storage;

public sealed record ActiveSaveState(
    string CampaignId,
    string ActiveChecksumAfterActivation,
    DateTimeOffset ActivatedAtUtc);
```

- [ ] **Step 4: Create `src/Storage/ActiveSaveSwitcher.cs`**

```csharp
namespace MultiplayerSaveSlots.Storage;

public sealed class ActiveSaveSwitcher
{
    private readonly MultiplayerSaveBank _bank;
    private readonly string _activeSavePath;
    private readonly string _statePath;

    public ActiveSaveSwitcher(MultiplayerSaveBank bank, string activeSavePath, string statePath)
    {
        _bank = bank;
        _activeSavePath = activeSavePath;
        _statePath = statePath;
    }

    public void Activate(string campaignId, DateTimeOffset nowUtc)
    {
        var payloadPath = _bank.GetPayloadPath(campaignId);
        if (!File.Exists(payloadPath))
            throw new FileNotFoundException("Campaign payload is missing", payloadPath);

        Directory.CreateDirectory(Path.GetDirectoryName(_activeSavePath)
            ?? throw new InvalidOperationException($"Active save path has no directory: {_activeSavePath}"));

        if (File.Exists(_activeSavePath))
            BackupManager.CreateBackup(_activeSavePath, _bank.GetBackupDirectory(campaignId), "before-activate-active", nowUtc);

        File.Copy(payloadPath, _activeSavePath, overwrite: true);
        var checksum = FileChecksum.Sha256(_activeSavePath);
        JsonFile.Write(_statePath, new ActiveSaveState(campaignId, checksum, nowUtc));

        var metadata = _bank.GetCampaign(campaignId);
        _bank.UpdateMetadata(metadata with
        {
            ActiveChecksum = checksum,
            PayloadChecksum = FileChecksum.Sha256(payloadPath),
            LastPlayedAtUtc = nowUtc
        });
    }

    public void SyncBack(DateTimeOffset nowUtc)
    {
        if (!File.Exists(_statePath))
            throw new InvalidOperationException("Cannot sync active save without active campaign state");

        if (!File.Exists(_activeSavePath))
            throw new FileNotFoundException("Active multiplayer save is missing", _activeSavePath);

        var state = JsonFile.Read<ActiveSaveState>(_statePath);
        var payloadPath = _bank.GetPayloadPath(state.CampaignId);
        if (!File.Exists(payloadPath))
            throw new FileNotFoundException("Campaign payload is missing", payloadPath);

        BackupManager.CreateBackup(payloadPath, _bank.GetBackupDirectory(state.CampaignId), "before-sync-bank", nowUtc);
        File.Copy(_activeSavePath, payloadPath, overwrite: true);

        var metadata = _bank.GetCampaign(state.CampaignId);
        _bank.UpdateMetadata(metadata with
        {
            ActiveChecksum = FileChecksum.Sha256(_activeSavePath),
            PayloadChecksum = FileChecksum.Sha256(payloadPath),
            LastPlayedAtUtc = nowUtc
        });
    }
}
```

- [ ] **Step 5: Run tests and verify pass**

Run:

```bash
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 6: Commit**

```bash
git add src/Storage/ActiveSaveState.cs src/Storage/ActiveSaveSwitcher.cs tests/ActiveSaveSwitcherTests.cs
git commit -m "feat: add active save switcher"
```

### Task 7: Tighten Active Save Sync Safety

**Files:**
- Modify: `src/Storage/ActiveSaveState.cs`
- Modify: `src/Storage/ActiveSaveSwitcher.cs`
- Modify: `tests/ActiveSaveSwitcherTests.cs`

- [ ] **Step 1: Add failing checksum safety test**

Add this test registration:

```csharp
tests.Add(new TestCase("switcher records previous active checksum and backs up active file", BacksUpPreviousActive));
```

Add this test method:

```csharp
private static void BacksUpPreviousActive()
{
    using var temp = new TempDirectory();
    var source = Path.Combine(temp.Path, "source.save");
    var active = Path.Combine(temp.Path, "active.save");
    File.WriteAllText(source, "campaign");
    File.WriteAllText(active, "previous-active");

    var bank = new MultiplayerSaveBank(new SaveBankPaths(Path.Combine(temp.Path, "MultiSaves")));
    var metadata = bank.CreateCampaign(new CampaignCreateRequest(MultiplayerGameMode.Standard, [], source, DateTimeOffset.UtcNow));
    var switcher = new ActiveSaveSwitcher(bank, active, Path.Combine(temp.Path, "active-state.json"));

    switcher.Activate(metadata.CampaignId, new DateTimeOffset(2026, 5, 8, 13, 0, 0, TimeSpan.Zero));

    var backups = Directory.GetFiles(bank.GetBackupDirectory(metadata.CampaignId));
    AssertEx.Equal(1, backups.Length);
    AssertEx.Equal("previous-active", File.ReadAllText(backups[0]));
}
```

- [ ] **Step 2: Extend `ActiveSaveState`**

```csharp
namespace MultiplayerSaveSlots.Storage;

public sealed record ActiveSaveState(
    string CampaignId,
    string? ActiveChecksumBeforeActivation,
    string ActiveChecksumAfterActivation,
    DateTimeOffset ActivatedAtUtc);
```

- [ ] **Step 3: Update `ActiveSaveSwitcher.Activate` state creation**

Replace the existing backup/state block with:

```csharp
string? checksumBeforeActivation = null;
if (File.Exists(_activeSavePath))
{
    checksumBeforeActivation = FileChecksum.Sha256(_activeSavePath);
    BackupManager.CreateBackup(_activeSavePath, _bank.GetBackupDirectory(campaignId), "before-activate-active", nowUtc);
}

File.Copy(payloadPath, _activeSavePath, overwrite: true);
var checksum = FileChecksum.Sha256(_activeSavePath);
JsonFile.Write(_statePath, new ActiveSaveState(campaignId, checksumBeforeActivation, checksum, nowUtc));
```

- [ ] **Step 4: Run tests and verify pass**

Run:

```bash
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```

Expected: all tests pass.

- [ ] **Step 5: Commit**

```bash
git add src/Storage/ActiveSaveState.cs src/Storage/ActiveSaveSwitcher.cs tests/ActiveSaveSwitcherTests.cs
git commit -m "feat: track active save ownership checksums"
```

### Task 8: Add STS2 Assembly Inspector

**Files:**
- Create: `tools/Sts2Inspector/Sts2Inspector.csproj`
- Create: `tools/Sts2Inspector/Program.cs`
- Modify: `MultiplayerSaveSlots.sln`
- Create: `docs/implementation/hook-discovery.md`

- [ ] **Step 1: Create `tools/Sts2Inspector/Sts2Inspector.csproj`**

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

- [ ] **Step 2: Create `tools/Sts2Inspector/Program.cs`**

```csharp
using System.Reflection;

var sts2Path = args.Length > 0
    ? args[0]
    : Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".local/share/Steam/steamapps/common/Slay the Spire 2/data_sts2_linuxbsd_x86_64/sts2.dll");

var assembly = Assembly.LoadFrom(sts2Path);
var terms = new[] { "Multiplayer", "Save", "RunSave", "Lobby", "HostSubmenu", "LoadGame" };

foreach (var type in assembly.GetTypes()
    .Where(type => terms.Any(term => type.FullName?.Contains(term, StringComparison.OrdinalIgnoreCase) == true))
    .OrderBy(type => type.FullName))
{
    Console.WriteLine($"## {type.FullName}");
    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .OrderBy(method => method.Name))
    {
        var parameters = string.Join(", ", method.GetParameters().Select(parameter => $"{parameter.ParameterType.Name} {parameter.Name}"));
        Console.WriteLine($"- method `{method.Name}({parameters})` -> `{method.ReturnType.Name}`");
    }

    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .OrderBy(field => field.Name))
    {
        Console.WriteLine($"- field `{field.FieldType.Name} {field.Name}`");
    }

    Console.WriteLine();
}
```

- [ ] **Step 3: Add tool to solution**

Run:

```bash
dotnet sln MultiplayerSaveSlots.sln add tools/Sts2Inspector/Sts2Inspector.csproj
```

Expected: tool project is added to the solution.

- [ ] **Step 4: Run inspector and write discovery doc**

Run:

```bash
mkdir -p docs/implementation
dotnet run --project tools/Sts2Inspector/Sts2Inspector.csproj -- "$HOME/.local/share/Steam/steamapps/common/Slay the Spire 2/data_sts2_linuxbsd_x86_64/sts2.dll" > docs/implementation/hook-discovery.md
```

Expected: `docs/implementation/hook-discovery.md` contains headings for candidate STS2 types, including multiplayer submenu and save manager types.

- [ ] **Step 5: Commit**

```bash
git add MultiplayerSaveSlots.sln tools/Sts2Inspector docs/implementation/hook-discovery.md
git commit -m "chore: add sts2 hook discovery tool"
```

### Task 9: Add README and Push

**Files:**
- Create: `README.md`

- [ ] **Step 1: Create `README.md`**

```markdown
# Multiplayer Save Slots

Slay the Spire 2 mod that lets a host keep multiple multiplayer campaign saves instead of abandoning the single vanilla co-op save.

## Status

Private development. The current implementation phase is focused on the save-bank and active-slot swap foundation.

## Design

See `docs/superpowers/specs/2026-05-08-multiplayer-save-slots-design.md`.

## Build

Requires a .NET SDK that can target `net9.0` and Slay the Spire 2 installed through Steam.

```bash
dotnet build MultiplayerSaveSlots.sln
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
```
```

- [ ] **Step 2: Run verification**

Run:

```bash
dotnet build MultiplayerSaveSlots.sln
dotnet run --project tests/MultiplayerSaveSlots.Tests.csproj
git status --short
```

Expected:

- build succeeds
- tests pass
- only `README.md` is untracked before committing

- [ ] **Step 3: Commit README**

```bash
git add README.md
git commit -m "docs: add project readme"
```

- [ ] **Step 4: Push**

```bash
git push
```

Expected: `main` is pushed to the private GitHub repository.

## Self-Review Notes

- Spec coverage: this plan covers scaffold, save bank, metadata labels, active-slot swap, backups, checksum state, and hook discovery. Picker UI and Harmony save-sync patches are intentionally deferred until discovery identifies stable targets.
- Red-flag scan: no incomplete or unspecified code tasks are present.
- Type consistency: domain and storage types are introduced before tests or subsequent steps depend on them.
