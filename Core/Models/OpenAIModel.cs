using Core.Interfaces;

namespace Core.Models;
public class OpenAIModel : IOpenAIModel
{
    public string Id { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public string OwnedBy { get; set; }
}
