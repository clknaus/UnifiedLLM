using System.Text.Json.Serialization;

namespace Core.Models;
public class ModelsResponse
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("data")]
    public IList<ModelInfo> Data { get; set; }
}
