using Core.Domain.Interfaces;
using Core.Domain.Models;
using Core.General.Models;

namespace Core.Domain.Entities;
public class ChatResponse : Entity<Guid>, IChatResponse
{
    public new string Id { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public string Model { get; set; }
    public IEnumerable<ChatChoice> Choices { get; set; }
    public string? SystemFingerprint { get; set; }

    public ChatResponse()
    {
        Id = base.Id.ToString();
    }

    public ChatResponse(IChatResponse chatResponse)
    {
        ArgumentNullException.ThrowIfNull(chatResponse);

        Id = chatResponse.Id ?? base.Id.ToString();
        Object = chatResponse.Object;
        Created = chatResponse.Created;
        Model = chatResponse.Model;
        Choices = chatResponse.Choices;
        SystemFingerprint = chatResponse.SystemFingerprint;
    }
}