using Core.General.Interfaces;
using Core.General.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Core.General.Extensions;
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
    public static string ThrowIfNullOrEmpty(this string value, [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(paramName);

        return value;
    }

    public static Result<T> AsResultSuccess<T>(this T? obj) where T : class => Result<T>.Success(obj);
    public static Result<U> AsResultFailed<U>(this IResult? result) where U : class
    {
        return Result<U>.Failure(result?.ErrorMessage ?? "Error");
    }

}
