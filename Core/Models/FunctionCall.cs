using Core.Interfaces;

namespace Core.Models;
public class FunctionCall : IFunctionCall
{
    public string Name { get; set; }
    public string Arguments { get; set; }
}
