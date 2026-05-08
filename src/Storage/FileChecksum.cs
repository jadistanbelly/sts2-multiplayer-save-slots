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
