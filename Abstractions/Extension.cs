using Core.General.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Abstractions.Extension;
public static class Extension
{
    public static async Task<T> TryDeserializeJsonAsync<T>(this Stream stream, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await JsonSerializer.DeserializeAsync<T>(
                stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                cancellationToken
            );

            return result ?? throw new InvalidOperationException("Deserialized object was null.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to deserialize JSON content.", ex);
        }
    }
    public static T ThrowIfNull<T>(this T? obj, [CallerArgumentExpression("obj")] string? paramName = null) where T : class
    {
        if (obj is null)
            throw new ArgumentNullException(paramName);

        return obj;
    }

    public static Result<T> AsResultSuccess<T>(this T? obj) where T : class => Result<T>.Success(obj);
    public static Result<T> AsResultFailed<T>(this T? obj, string? message = null, ErrorType errorType = default) where T : class => Result<T>.Failure(message, errorType: errorType);
    public static Result<T> AsResultFailed<U, T>(this Result<U>? obj) where U : class => Result<T>.Failure<U>(obj!);
    public static Result<T> AsResultFailed<T>(this Result<T>? obj) where T : class => Result<T>.Failure(obj!);

    public static async Task<Result<T>> TryOrReturn<T>(
        this Func<Task<T>> operation,
        ErrorType error
    )
    {
        try
        {
            var result = await operation();
            return Result<T>.Success(result);
        }
        catch
        {
            return Result<T>.Failure(error);
        }
    }

    public static Result<T> TryOrReturn<T>(
        this Func<T> operation,
        ErrorType error
    )
    {
        try
        {
            return Result<T>.Success(operation());
        }
        catch
        {
            return Result<T>.Failure(error);
        }
    }
}
