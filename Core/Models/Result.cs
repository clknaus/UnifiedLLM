using Core.Interfaces;

namespace Core.Models;
public class Result<T> : IResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string Error { get; }
    public ErrorType? ErrorType { get; }

    private Result(bool isSuccess, T? value, string? error, ErrorType? errorType)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error ?? "An error occurred while processing the request.";
        ErrorType = errorType;
    }

    public static Result<T> Success(T value) => new(isSuccess: true, value: value, error: null, errorType: null);
    public static Result<T> Failure() => new(isSuccess: false, value: default, error: null, errorType: null);
    public static Result<T> Failure(string? error = null, ErrorType? errorType = null) => new(isSuccess: false, value: default, error: error, errorType: errorType);
    public static Result<T> Failure(ErrorType? errorType = null) => new(isSuccess: false, value: default, error: null, errorType: errorType);
}

public enum ErrorType
{
    Unknown,
    NotFound,
    Validation,
    Conflict,
    Unauthorized,
}

