using System.Text.Json.Serialization;

namespace Application.Models;
public class OpenWebUIModelsResponse
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("data")]
    public IList<OpenWebUIModelInfo> Data { get; set; }
}
