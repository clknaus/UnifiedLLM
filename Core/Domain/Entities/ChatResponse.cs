using Core.Domain.Interfaces;
using Core.Domain.Models;
using Core.General.Models;

namespace Core.Domain.Entities;
public class ChatResponse : Entity<Guid>, IChatResponse
{
    public string Object { get; set; }
    public long Created { get; set; }
    public string Model { get; set; }
    public IEnumerable<ChatChoice> Choices { get; set; }
    public string SystemFingerprint { get; set; }
    string IChatResponse.Id { get; set; }
}