using Core.Interfaces;
using Core.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Core.Extensions;
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
    public static Result<U> AsResultFailed<U>(this IResult? result) where U : class
    {
        return Result<U>.Failure(result?.Error ?? "Error");
    }
}
