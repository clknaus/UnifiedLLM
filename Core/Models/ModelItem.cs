using Core.Interfaces;

namespace Core.Models;
public class ModelItem : IModelItem
{
    public string Id { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public string OwnedBy { get; set; }
    public string Name { get; set; }
    public string OwnedByAlt { get; set; }
    public OpenAIModel OpenAI { get; set; }
    public int UrlIdx { get; set; }
    public IEnumerable<object> Actions { get; set; }
    public IEnumerable<object> Tags { get; set; }
}
