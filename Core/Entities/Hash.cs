using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Core.Entities;

// v1
public sealed class Hash : Entity<Guid>
{
    private bool _hasHashCode => HashCode.Length > 0;
    public byte[] HashCode { get; set; }

    bool Equals(byte[] hash) => HashCode.SequenceEqual(hash);
    string Hashing(object hash) => CreateSha256Hash(hash);
    bool HasHashCode() => _hasHashCode;
    bool HasHashCode(object hash) => (Hashing(hash) ?? string.Empty) != string.Empty;
    
    // TODO: raise OnSetHashStoreEntityEvent
    public SetHash(object unhashed)
    {
     
    }

    /// <summary>
    /// Computes the SHA-256 hash of an object.
    /// </summary>
    /// <param name="data">The object to hash. It will be serialized to JSON.</param>
    /// <returns>A lowercase SHA-256 hex string. Returns an empty string if the input is null.</returns>
    public string CreateSha256Hash(object data)
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