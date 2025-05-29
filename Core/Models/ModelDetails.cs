using System.Security;
using System.Text.Json.Serialization;

namespace Core.Models;
public class ModelDetails
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("owned_by")]
    public string OwnedBy { get; set; }

    [JsonPropertyName("permission")]
    public List<Permission> Permission { get; set; }

    [JsonPropertyName("root")]
    public string Root { get; set; }

    [JsonPropertyName("parent")]
    public string Parent { get; set; }
}
