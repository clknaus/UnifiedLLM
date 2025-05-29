using Core.Models;
using System.Text.Json.Serialization;

namespace Infrastructure.Models;
public class ModelListResponse
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("data")]
    public IList<ModelInfo> Data { get; set; }
}
