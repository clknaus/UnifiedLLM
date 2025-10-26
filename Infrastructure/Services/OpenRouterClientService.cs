using Application.Models;
using Core.Domain.Interfaces;
using Core.General.Extensions;
using Core.General.Models;
using Infrastructure.Interfaces.Providers.OpenRouter;
using Infrastructure.Models.OpenRouter;
using Infrastructure.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Services;
public class OpenRouterClientService : IProviderClientService
{
    private readonly HttpClient _httpClient;
    private readonly OpenRouterConfiguration _opts;

public OpenRouterClientService(HttpClient httpClient, IOptions<OpenRouterConfiguration> opts)
    {
        _httpClient = httpClient;
        _opts = opts.Value;
        _httpClient.BaseAddress = new Uri(_opts.BaseUrl);
    }

    public async IAsyncEnumerable<Result<IChatResponse>> TryStreamChatCompletionAsync(
        IChatRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        request.Stream = true;
        HttpResponseMessage? response = null;
        Result<IChatResponse>? result = null;

        try
        {
            string json = JsonSerializer.Serialize(request, JsonDefaults.CachedJsonOptions_PropertyNamingPolicyCamelCase);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "v1/chat/completions")
            {
                Content = content
            };

            response = await _httpClient.SendAsync(
                httpRequest,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            response?.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            result = Result<IChatResponse>.Failure(ex.Message);
        }

        if (result?.IsFailure == true)
        {
            yield return result;
            yield break;
        }

        await using var stream = await response!.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);
        var jsonSerializerOptions = new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(cancellationToken);

            if (!string.IsNullOrEmpty(line) && line.StartsWith("data: ", StringComparison.OrdinalIgnoreCase))
            {
                var data = line["data: ".Length..];
                if (data.Equals("[DONE]", StringComparison.OrdinalIgnoreCase))
                    yield break;

                Result<IChatResponse>? chunkResult;
                try
                {
                    var chunk = JsonSerializer.Deserialize<OpenRouterChatResponse>(data, JsonDefaults.CachedJsonOptions_PropertyNamingPolicyCamelCase_DefaultIgnoreConditionWhenWritingNull);

                    chunkResult = chunk != null
                        ? chunk.AsResultSuccess<IChatResponse>()
                        : Result<IChatResponse>.Failure("Empty chunk.");
                }
                catch (JsonException)
                {
                    chunkResult = Result<IChatResponse>.Failure("Malformed response chunk from provider.");
                }

                yield return chunkResult;
            }
            else
            {
                continue;
            }
        }
    }

    public async Task<Result<IChatResponse>> TryCreateChatCompletionAsync(IChatRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            string json = JsonSerializer.Serialize(request, JsonDefaults.CachedJsonOptions_PropertyNamingPolicyCamelCase);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync("v1/chat/completions", content, cancellationToken);
            response.EnsureSuccessStatusCode();

            _httpClient?.Dispose();

            var res = await JsonSerializer.DeserializeAsync<OpenRouterChatResponse>(
                       await response.Content.ReadAsStreamAsync(cancellationToken),
                       JsonDefaults.CachedJsonOptions_PropertyNameCaseInsensitive,
                       cancellationToken
                   );

            return res.AsResultSuccess<IChatResponse>();
        }
        catch (Exception)
        {
            _httpClient?.Dispose();
            return Result<IChatResponse>.Failure("response from provider wasn't processed.");
        }
    }

    public async Task<Result<IModelsResponse>> TryGetAvailableModelsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("v1/models", cancellationToken);
            var res = await JsonSerializer.DeserializeAsync<OpenRouterModelsResponse>(
                await response.Content.ReadAsStreamAsync(cancellationToken),
                JsonDefaults.CachedJsonOptions_PropertyNameCaseInsensitive,
                cancellationToken
            );
            _httpClient.Dispose();

            return res.AsResultSuccess<IModelsResponse>();
        }
        catch (Exception ex)
        {
            _httpClient?.Dispose();
            return Result<IModelsResponse>.Failure(exception: ex, logLevel: LogLevel.Error);
        }
    }
}

