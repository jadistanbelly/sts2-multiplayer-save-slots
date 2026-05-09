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
