using Core.Domain.Events;
using Core.General.Models;
using Core.Supportive.Enums;

namespace Application.Handler;

public class ChatCompletedHandler : AsyncDomainEventHandler<ChatCycleCompletedEvent>
{
    public override async Task<Result<HandlerResult>> HandleAsync(ChatCycleCompletedEvent domainEvent)
    {
        // handler act in application layer without chaning data persistancy or business logic

        Console.WriteLine("Chat completed.");
        // Some logic ...
        return Success;
    }
}
