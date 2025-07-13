using Core.Domain.Interfaces;
using Core.Domain.Models;

namespace Application.Models;
public class OpenWebUIChatRequest : IChatRequest
{
    //public string? Provider { get; set; }
    public string Model { get; set; }
    public IEnumerable<ChatMessage> Messages { get; set; }
    public bool Stream { get; set; } = false;
    //public double? Temperature { get; set; } = 0.7;
    //public double? TopP { get; set; } = 1.0;
    //public int? N { get; set; } = 1;
    //public int? MaxTokens { get; set; } = 256;
    //public IEnumerable<string>? Stop { get; set; }
    //public double? PresencePenalty { get; set; } = 0.0;
    //public double? FrequencyPenalty { get; set; } = 0.0;
    //public IDictionary<string, int>? LogitBias { get; set; }
    //public string? User { get; set; }
    //public IEnumerable<ToolDefinition>? Tools { get; set; }
    //public ToolChoice? ToolChoice { get; set; }
    //public IDictionary<string, object>? Params { get; set; }
    //public IEnumerable<object>? ToolServers { get; set; }
    //public Features? Features { get; set; }
    //public IDictionary<string, string>? Variables { get; set; }
    //public ModelItem? ModelItem { get; set; }
    public string? SessionId { get; set; }
    public string? ChatId { get; set; }
    public string? Id { get; set; }
    //public BackgroundTasks? BackgroundTasks { get; set; }
}

//public class ChatMessage : IChatMessage
//{
//    public string Role { get; set; }
//    public string Content { get; set; }
//}

//public class Features : IFeatures
//{
//    public bool ImageGeneration { get; set; }
//    public bool CodeInterpreter { get; set; }
//    public bool WebSearch { get; set; }
//}

//public class ModelItem : IModelItem
//{
//    public string Id { get; set; }
//    public string Object { get; set; }
//    public long Created { get; set; }
//    public string OwnedBy { get; set; }
//    public string Name { get; set; }
//    public string OwnedByAlt { get; set; }
//    public IOpenAIModel OpenAI { get; set; }
//    public int UrlIdx { get; set; }
//    public IEnumerable<object> Actions { get; set; }
//    public IEnumerable<object> Tags { get; set; }
//}

//public class OpenAIModel : IOpenAIModel
//{
//    public string Id { get; set; }
//    public string Object { get; set; }
//    public long Created { get; set; }
//    public string OwnedBy { get; set; }
//}

//public class BackgroundTasks : IBackgroundTasks
//{
//    public bool TitleGeneration { get; set; }
//    public bool TagsGeneration { get; set; }
//}

//public class ToolDefinition : IToolDefinition
//{
//    public IFunctionDefinition Function { get; set; }
//}

//public class FunctionDefinition : IFunctionDefinition
//{
//    public string Name { get; set; }
//    public string Description { get; set; }
//    public IDictionary<string, IFunctionParameter> Parameters { get; set; }
//}
