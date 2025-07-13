using Core.Domain.Models;

namespace Core.Domain.Interfaces;

public interface IModelsResponse
{
    public string Object { get; set; }
    public IList<ModelInfo> Data { get; set; }
}
