using MultiplayerSaveSlots.Tests;
using System.Reflection;

AppDomain.CurrentDomain.AssemblyResolve += (_, args) =>
{
    var assemblyName = new AssemblyName(args.Name).Name;
    if (assemblyName is not ("sts2" or "GodotSharp" or "0Harmony"))
        return null;

    var sts2DataPath = GetSts2DataPath();
    if (sts2DataPath is null)
        return null;

    var assemblyPath = Path.Combine(sts2DataPath, $"{assemblyName}.dll");
    return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
};

var tests = new List<TestCase>();
CampaignLabelerTests.Register(tests);
MultiplayerSaveBankTests.Register(tests);
ActiveSaveSwitcherTests.Register(tests);
StorageUtilityTests.Register(tests);
tests.AddRange(HostFlowControllerTests.All());
tests.AddRange(HostFlowPatchTests.All());

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

static string? GetSts2DataPath()
{
    var overridePath = Environment.GetEnvironmentVariable("STS2DataPath");
    if (!string.IsNullOrWhiteSpace(overridePath))
        return overridePath;

    var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    if (OperatingSystem.IsLinux())
        return Path.Combine(home, ".local/share/Steam/steamapps/common/Slay the Spire 2/data_sts2_linuxbsd_x86_64");

    if (OperatingSystem.IsMacOS())
        return Path.Combine(home, "Library/Application Support/Steam/steamapps/common/Slay the Spire 2/data_sts2_macos_universal");

    if (OperatingSystem.IsWindows())
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "Steam/steamapps/common/Slay the Spire 2/data_sts2_windows_x86_64");

    return null;
}
