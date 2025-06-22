using Core.Models;

namespace Core.Interfaces;
public interface IChatRequest
{
    //string? Provider { get; set; }
    string Model { get; set; }
    IEnumerable<ChatMessage> Messages { get; set; }
    //double? Temperature { get; set; }
    //double? TopP { get; set; }
    //int? N { get; set; }
    //int? MaxTokens { get; set; }
    bool Stream { get; set; }
    //IEnumerable<string>? Stop { get; set; }
    //double? PresencePenalty { get; set; }
    //double? FrequencyPenalty { get; set; }
    //IDictionary<string, int>? LogitBias { get; set; }
    //string? User { get; set; }
    //IEnumerable<ToolDefinition>? Tools { get; set; }
    //ToolChoice? ToolChoice { get; set; }
    //IDictionary<string, object>? Params { get; set; }
    //IEnumerable<object>? ToolServers { get; set; }
    //Features? Features { get; set; }
    //IDictionary<string, string>? Variables { get; set; }
    //ModelItem? ModelItem { get; set; }
    string? SessionId { get; set; }
    string? ChatId { get; set; }
    string? Id { get; set; }
    //BackgroundTasks? BackgroundTasks { get; set; }
}

public interface IChatMessage
{
    string Role { get; set; }
    string Content { get; set; }
}

public interface IFeatures
{
    bool ImageGeneration { get; set; }
    bool CodeInterpreter { get; set; }
    bool WebSearch { get; set; }
}

public interface IModelItem
{
    string Id { get; set; }
    string Object { get; set; }
    long Created { get; set; }
    string OwnedBy { get; set; }
    string Name { get; set; }
    string OwnedByAlt { get; set; }
    OpenAIModel OpenAI { get; set; }
    int UrlIdx { get; set; }
    IEnumerable<object> Actions { get; set; }
    IEnumerable<object> Tags { get; set; }
}

public interface IOpenAIModel
{
    string Id { get; set; }
    string Object { get; set; }
    long Created { get; set; }
    string OwnedBy { get; set; }
}

public interface IBackgroundTasks
{
    public bool TitleGeneration { get; set; }
    public bool TagsGeneration { get; set; }
}

public interface IToolDefinition
{
    FunctionDefinition Function { get; set; }
}

public interface IToolChoice
{
    string Type { get; set; }
    FunctionCall Function { get; set; }
}

public interface IFunctionDefinition
{
    string Name { get; set; }
    string Description { get; set; }
    IDictionary<string, FunctionParameter> Parameters { get; set; }
}

public interface IFunctionCall
{
    string Name { get; set; }
    string Arguments { get; set; }
}

public interface IFunctionParameter
{
    string Type { get; set; }
    string Description { get; set; }
    IDictionary<string, FunctionParameter> Properties { get; set; }
    IList<string> Required { get; set; }
}
