using MultiplayerSaveSlots.Core;

namespace MultiplayerSaveSlots.Tests;

public static class RunProgressLabelerTests
{
    public static void Register(List<TestCase> tests)
    {
        tests.Add(new TestCase("progress label uses act and floor when both exist", UsesActAndFloorWhenBothExist));
        tests.Add(new TestCase("progress label uses floor when only history exists", UsesFloorWhenOnlyHistoryExists));
        tests.Add(new TestCase("progress label uses act when only act is known", UsesActWhenOnlyActIsKnown));
        tests.Add(new TestCase("progress label omits invalid progress", OmitsInvalidProgress));
    }

    private static void UsesActAndFloorWhenBothExist()
    {
        AssertEx.Equal("Act 2 - Floor 18", RunProgressLabeler.Build(currentActIndex: 1, completedFloorCount: 17));
    }

    private static void UsesFloorWhenOnlyHistoryExists()
    {
        AssertEx.Equal("Floor 18", RunProgressLabeler.Build(currentActIndex: -1, completedFloorCount: 17));
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
