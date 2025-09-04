namespace Core.General.Interfaces;
public interface IResult
{
    bool IsSuccess { get; }
    string? ErrorMessage { get; }
}
