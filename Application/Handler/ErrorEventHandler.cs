using Core.Domain.Events;
using Core.General.Models;
using Core.Supportive.Enums;

namespace Application.Handler;

public class ErrorEventHandler : AsyncDomainEventHandler<AnonymizerErrorEvent>
{
    public override async Task<Result<HandlerResult>> HandleAsync(AnonymizerErrorEvent domainEvent)
    {
        // handler act in application layer without changeing data persistancy or business logic

        Console.WriteLine("Chat completed.");
        // Some logic ...
        return Success;
    }
}