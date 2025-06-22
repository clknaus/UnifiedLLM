using Core.Interfaces;
using Core.Models;

namespace Core.Entities;
public class ChatRequest : BaseEntity, IChatRequest 
{
    public string Model { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IEnumerable<ChatMessage> Messages { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool Stream { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string? SessionId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string? ChatId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string? Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}