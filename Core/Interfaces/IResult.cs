namespace Core.Interfaces;
public interface IResult
{
    bool IsSuccess { get; }
    string? Error { get; }
}
