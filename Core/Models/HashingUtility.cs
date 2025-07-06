namespace Core.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

/// <summary>
/// A utility class for creating cryptographic hashes.
/// </summary>
public class HashingUtility : IHashingUtility
{
    /// <summary>
    /// Computes the SHA-256 hash of an object.
    /// </summary>
    /// <param name="data">The object to hash. It will be serialized to JSON.</param>
    /// <returns>A lowercase SHA-256 hex string. Returns an empty string if the input is null.</returns>
    public string CreateSha256Hash(object data)
    {
        if (data == null)
        {
            return string.Empty;
        }

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