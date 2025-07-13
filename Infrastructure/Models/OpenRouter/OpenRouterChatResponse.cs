using Core.Domain.Interfaces;
using Core.Domain.Models;

namespace Application.Models;
public class OpenRouterChatResponse : IChatResponse
{
    public string Id { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public string Model { get; set; }
    public IEnumerable<ChatChoice> Choices { get; set; }
    //public IUsage Usage { get; set; }
    //public IEnumerable<IToolCall> ToolCalls { get; set; }
    public string SessionId { get; set; }
    public string ChatId { get; set; }
    public string SystemFingerprint { get; set; }
}

//public class Usage
//{
//    public int PromptTokens { get; set; }
//    public int CompletionTokens { get; set; }
//    public int TotalTokens { get; set; }
//}

//public class ToolCall
//{
//    public string Id { get; set; }
//    public string Type { get; set; }
//    public ToolFunction Function { get; set; }
//}

//public class ToolFunction
//{
//    public string Name { get; set; }
//    public string Arguments { get; set; } // usually raw JSON string
//}
