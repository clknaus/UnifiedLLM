using Core.Domain.Models;

namespace Core.Domain.Interfaces;
    public interface IChatRequest
    {
        //string? Provider { get; set; }
        string? Id { get; set; }
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
    //string? SessionId { get; set; }
    //string? ChatId { get; set; }
    //BackgroundTasks? BackgroundTasks { get; set; }
}

