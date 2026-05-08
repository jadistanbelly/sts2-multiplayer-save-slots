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
