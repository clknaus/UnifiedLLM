using Core.General.Interfaces;
using Microsoft.Extensions.Logging;

namespace Core.General.Models;
public class Result<T> : IResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public T? ValueOrThrow => IsSuccess ? Value : throw new Exception("Result object was not provided.");
    public string ErrorMessage { get; }
    public Exception Exception { get; }
    public ErrorType? ErrorType { get; }
    public LogLevel LogLevel { get; } = LogLevel.None;

    private Result(bool isSuccess, T? value, string? message, Exception? exception, ErrorType? errorType, LogLevel logLevel)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = IsFailure ? message ?? "An error occurred while processing the request." : string.Empty;
        ErrorType = errorType;
    }

    public static Result<T> Success(T value) => new(isSuccess: true, value: value, message: null, exception: null, errorType: null, logLevel: LogLevel.None);
    public static Result<T> Failure() => new(isSuccess: false, value: default, message: null, exception: null, errorType: null, logLevel: LogLevel.Error);
    public static Result<T> Failure(string? message = null, Exception? exception = null, ErrorType? errorType = null, LogLevel logLevel = LogLevel.Error) => new(isSuccess: false, value: default, message: message, exception: exception, errorType: errorType, logLevel: logLevel);
    public static Result<T> Failure<U>(Result<U> result) => new(isSuccess: false, value: default, message: result.ErrorMessage, exception: result.Exception, errorType: result.ErrorType, logLevel: result.LogLevel);
    public static Result<T> Failure(ErrorType? errorType = null) => new(isSuccess: false, value: default, message: null, exception: null, errorType: errorType, logLevel: LogLevel.Error);
}

public enum ErrorType
{
    Unknown,
    NotFound,
    Validation,
    Conflict,
    Unauthorized,
}

