namespace Core.Models
{
    public interface IHashingUtility
    {
        public string CreateSha256Hash(object data);
    }
}