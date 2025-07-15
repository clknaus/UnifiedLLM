using Core.Domain.Models;

namespace Core.Domain.Interfaces;
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