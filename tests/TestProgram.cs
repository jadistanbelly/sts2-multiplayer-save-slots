using MultiplayerSaveSlots.Tests;

var tests = new List<TestCase>();
CampaignLabelerTests.Register(tests);
MultiplayerSaveBankTests.Register(tests);
ActiveSaveSwitcherTests.Register(tests);
StorageUtilityTests.Register(tests);

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
