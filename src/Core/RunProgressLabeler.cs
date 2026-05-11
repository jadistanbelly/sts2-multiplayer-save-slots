namespace MultiplayerSaveSlots.Core;

public static class RunProgressLabeler
{
    public static string? Build(int currentActIndex, int completedFloorCount)
    {
        if (currentActIndex >= 0)
        {
            var act = $"Act {currentActIndex + 1}";
            return completedFloorCount > 0
                ? $"{act} - Floor {completedFloorCount + 1}"
                : act;
        }

        if (completedFloorCount > 0)
            return $"Floor {completedFloorCount + 1}";

        return null;
    }
}
