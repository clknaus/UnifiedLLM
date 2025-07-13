using Core.Domain.Interfaces;

namespace Core.Domain.Models;
public class FunctionCall : IFunctionCall
{
    public string Name { get; set; }
    public string Arguments { get; set; }
}
