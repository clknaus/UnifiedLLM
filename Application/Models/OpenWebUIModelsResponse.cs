using Core.Domain.Interfaces;
using Core.Domain.Models;

namespace Application.Models;
public class OpenWebUIModelsResponse : IModelsResponse
{
    public string Object { get; set; }
    public IList<ModelInfo> Data { get; set; }
}
