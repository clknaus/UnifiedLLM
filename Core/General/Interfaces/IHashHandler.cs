namespace Core.General.Interfaces;
public interface IHashHandler
{
    bool Equals(byte[] hash1, byte[] hash2);
    bool Equals(object hash1, object hash2);
    bool Equals(string hash1, string hash2);
    bool IsHash(string hash);
    string Hash(object hash);
}