using Core.Interfaces;

namespace Core.Models;
public class Result<T> : IResult
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error ?? "Error");
    public static Result<T> Failure() => new(false, default, error: null);
}

