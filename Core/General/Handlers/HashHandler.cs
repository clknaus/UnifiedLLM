namespace Core.General.Handler;

using Core.General.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

/// <summary>
/// A utility class for creating cryptographic hashes.
/// </summary>
public class HashHandler : IHashHandler
{
    public bool Equals(object hash1, object hash2)
    {
        if (hash1 == null || hash2 == null)
            return false;

        string hashed1 = CreateSha256Hash(hash1).ToString();
        string hashed2 = CreateSha256Hash(hash2).ToString();

        return Equals(hashed1, hashed2);
    }
    public bool Equals(byte[] hash1, byte[] hash2) => hash1.SequenceEqual(hash2);
    public bool Equals(string hash1, string hash2) => IsHash(hash1) && IsHash(hash2) && hash1.SequenceEqual(hash2);
    public bool IsHash(string hash) => hash is not null && hash.Length > 0;
    public string Hash(object hash) => CreateSha256Hash(hash);

    /// <summary>
    /// Computes the SHA-256 hash of an object.
    /// </summary>
    /// <param name="data">The object to hash. It will be serialized to JSON.</param>
    /// <returns>A lowercase SHA-256 hex string. Returns an empty string if the input is null.</returns>
    private string CreateSha256Hash(object data)
    {
        if (data == null)
            return string.Empty;

        // 1. Serialize the object to a consistent JSON string
        string jsonString = JsonSerializer.Serialize(data);

        // 2. Convert the string to a byte array
        byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

        // 3. Compute the hash using the static HashData method
        byte[] hashBytes = SHA256.HashData(bytes);

        // 4. Convert the hash byte array to a lowercase hex string
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}