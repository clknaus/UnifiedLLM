using Core.General.Interfaces;

namespace Core.General.Models;
public class Result<T> : IResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public T? ValueOrThrow => IsSuccess ? Value : throw new Exception("Result object was not provided.");
    public string ErrorMessage { get; }
    public ErrorType? ErrorType { get; }

    private Result(bool isSuccess, T? value, string? error, ErrorType? errorType)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = IsFailure ? error ?? "An error occurred while processing the request." : string.Empty;
        ErrorType = errorType;
    }

    public static Result<T> Success(T value) => new(isSuccess: true, value: value, error: null, errorType: null);
    public static Result<T> Failure() => new(isSuccess: false, value: default, error: null, errorType: null);
    public static Result<T> Failure(string? error = null, ErrorType? errorType = null) => new(isSuccess: false, value: default, error: error, errorType: errorType);
    public static Result<T> Failure<U>(Result<U> result) => new(isSuccess: false, value: default, error: result.ErrorMessage, errorType: result.ErrorType);
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

