using Core.Domain.Events;
using Core.General.Models;

namespace Core.Domain.Entities;
public class Anonymizer : AggregateRoot<Guid, Anonymizer>
{
    public required string Original { get ; init; }
    public required string Replacement { get; init; }

    public override Result<Anonymizer> ValidateThenRaiseEvent()
    {
        string error = string.Empty;

        if (string.IsNullOrWhiteSpace(Original))
            error += $"{nameof(Original)} can't be null or whitespace";

        if (string.IsNullOrWhiteSpace(Replacement))
            error += $"{nameof(Original)} can't be null or whitespace";

        if (!string.IsNullOrEmpty(error))
        {
            AddDomainEvent(new AnonymizerErrorEvent(Id, error));
            return Result<Anonymizer>.Failure(error);
        }

        return Result<Anonymizer>.Success(this);
    }
}
