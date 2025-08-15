using Core.Domain.Interfaces;
using Core.Domain.Models;
using Core.General.Models;

namespace Core.Domain.Entities;
public class ChatRequest : Entity<Guid>, IChatRequest
{
    public new string? Id { get; set; }
    public string Model { get; set; }
    public IEnumerable<ChatMessage> Messages { get; set; }
    public bool Stream { get; set; }

    public ChatRequest() 
    {
        Id = base.Id.ToString();
    }
    
    public ChatRequest(IChatRequest chatRequest)
    {
        ArgumentNullException.ThrowIfNull(chatRequest);

        Id = chatRequest.Id ?? base.Id.ToString();
        Model = chatRequest.Model;
        Messages = chatRequest.Messages;
        Stream = chatRequest.Stream;
    }
}