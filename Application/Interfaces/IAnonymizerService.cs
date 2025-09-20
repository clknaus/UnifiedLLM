using Core.Domain.Interfaces;
using Core.General.Models;

namespace Application.Interfaces;
public interface IAnonymizerService
{
    public Task<Result<IChatRequest>> Anonymize(IChatRequest value);
    public Task<Result<IChatResponse>> Deanonymize(IChatResponse value);
}
