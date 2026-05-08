using System.Text.Json;
using System.Text.Json.Serialization;

namespace MultiplayerSaveSlots.Storage;

public static class JsonFile
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public static T Read<T>(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, Options)
            ?? throw new InvalidOperationException($"Could not deserialize {path}");
    }

    public static void Write<T>(string path, T value)
    {
        var directory = Path.GetDirectoryName(path);
        var tempDirectory = string.IsNullOrEmpty(directory)
            ? Directory.GetCurrentDirectory()
            : directory;
        Directory.CreateDirectory(tempDirectory);

        var tempPath = Path.Combine(tempDirectory, Path.GetFileName(path) + ".tmp");
        File.WriteAllText(tempPath, JsonSerializer.Serialize(value, Options));
        File.Move(tempPath, path, overwrite: true);
    }
}
