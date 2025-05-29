using System.Text.Json.Serialization;

namespace Application.Models;
public class OpenWebUIChatResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("choices")]
    public List<ChatChoice> Choices { get; set; }

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }

    [JsonPropertyName("tool_calls")]
    public List<ToolCall> ToolCalls { get; set; }

    [JsonPropertyName("session_id")]
    public string SessionId { get; set; }

    [JsonPropertyName("chat_id")]
    public string ChatId { get; set; }
}

public class ChatChoice
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("message")]
    public ChatMessage Message { get; set; }

    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; }
}

public class Usage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}

public class ToolCall
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("function")]
    public ToolFunction Function { get; set; }
}

public class ToolFunction
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("arguments")]
    public string Arguments { get; set; } // usually raw JSON string
}
