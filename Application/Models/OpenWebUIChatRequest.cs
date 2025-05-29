using Core.Models;
using System.Text.Json.Serialization;

namespace Application.Models;
public class OpenWebUIChatRequest
{
    [JsonPropertyName("provider")]
    public string? Provider { get; set; } // Make nullable

    [JsonPropertyName("model")]
    public string Model { get; set; } // Required — exists in request

    [JsonPropertyName("messages")]
    public IList<ChatMessage> Messages { get; set; } // Required — exists in request

    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; } = 0.7;

    [JsonPropertyName("top_p")]
    public double? TopP { get; set; } = 1.0;

    [JsonPropertyName("n")]
    public int? N { get; set; } = 1;

    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; } = 256;

    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;

    [JsonPropertyName("stop")]
    public IList<string>? Stop { get; set; }

    [JsonPropertyName("presence_penalty")]
    public double? PresencePenalty { get; set; } = 0.0;

    [JsonPropertyName("frequency_penalty")]
    public double? FrequencyPenalty { get; set; } = 0.0;

    [JsonPropertyName("logit_bias")]
    public IDictionary<string, int>? LogitBias { get; set; }

    [JsonPropertyName("user")]
    public string? User { get; set; }

    [JsonPropertyName("tools")]
    public IList<ToolDefinition>? Tools { get; set; }

    [JsonPropertyName("tool_choice")]
    public ToolChoice? ToolChoice { get; set; }

    [JsonPropertyName("params")]
    public Dictionary<string, object>? Params { get; set; }

    [JsonPropertyName("tool_servers")]
    public List<object>? ToolServers { get; set; }

    [JsonPropertyName("features")]
    public Features? Features { get; set; }

    [JsonPropertyName("variables")]
    public Dictionary<string, string>? Variables { get; set; }

    [JsonPropertyName("model_item")]
    public ModelItem? ModelItem { get; set; }

    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }

    [JsonPropertyName("chat_id")]
    public string? ChatId { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("background_tasks")]
    public BackgroundTasks? BackgroundTasks { get; set; }
}

public class ChatMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}

public class Features
{
    [JsonPropertyName("image_generation")]
    public bool ImageGeneration { get; set; }

    [JsonPropertyName("code_interpreter")]
    public bool CodeInterpreter { get; set; }

    [JsonPropertyName("web_search")]
    public bool WebSearch { get; set; }
}

public class ModelItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("ownedBy")]
    public string OwnedBy { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("owned_by")]
    public string OwnedByAlt { get; set; }

    [JsonPropertyName("openai")]
    public OpenAIModel OpenAI { get; set; }

    [JsonPropertyName("urlIdx")]
    public int UrlIdx { get; set; }

    [JsonPropertyName("actions")]
    public List<object> Actions { get; set; }

    [JsonPropertyName("tags")]
    public List<object> Tags { get; set; }
}

public class OpenAIModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("ownedBy")]
    public string OwnedBy { get; set; }
}

public class BackgroundTasks
{
    [JsonPropertyName("title_generation")]
    public bool TitleGeneration { get; set; }

    [JsonPropertyName("tags_generation")]
    public bool TagsGeneration { get; set; }
}

public class ToolDefinition
{
    public string Type { get; set; } = "function"; // Currently, only "function" is supported
    public FunctionDefinition Function { get; set; }
}
