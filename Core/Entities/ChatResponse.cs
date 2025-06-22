using Core.Interfaces;
using Core.Models;

namespace Core.Entities;
public class ChatResponse : BaseEntity, IChatResponse
{
    public string Object { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public long Created { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Model { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IEnumerable<ChatChoice> Choices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string SystemFingerprint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    string IChatResponse.Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}