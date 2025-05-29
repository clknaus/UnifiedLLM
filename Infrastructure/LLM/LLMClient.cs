using Abstractions;
using Abstractions.Interfaces;
using API.Configuration;
using Core.Models;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UnifiedLLM.Core.Models;

namespace UnifiedLLM.Clients;
public class LLMClient : ILLMClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientService _httpClientService;
    private readonly UnifiedLLMOptions _opts;

    public LLMClient(HttpClient httpClient, IOptions<UnifiedLLMOptions> opts, IHttpClientService httpClientService)
    {
        _httpClient = httpClient;
        _httpClientService = httpClientService;
        _opts = opts.Value;
        _httpClient.BaseAddress = new Uri(_opts.BaseUrl);
        _httpClientService.BaseAddress = _opts.BaseUrl;
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _opts.ApiKey);
    }

    public async Task<ChatResponse> CreateChatCompletionAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        request.Temperature = _opts.DefaultTemperature;
        request.MaxTokens = _opts.DefaultMaxTokens;

        var json = JsonSerializer.Serialize(request);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync("/v1/chat/completions", content);
        response.EnsureSuccessStatusCode();

        return await JsonSerializer.DeserializeAsync<ChatResponse>(
                   await response.Content.ReadAsStreamAsync()
               ) ?? throw new InvalidOperationException("Invalid API response");
    }

    public async IAsyncEnumerable<string> StreamChatCompletionAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        request.MaxTokens = request.MaxTokens;
        request.Temperature = request.Temperature;
        var streamReq = new ChatRequest
        {
            Provider = request.Provider,
            Model = request.Model,
            Messages = request.Messages,
            Temperature = request.Temperature,
            MaxTokens = request.MaxTokens
        };
        // ensure streaming flag in raw JSON if needed
        var json = JsonSerializer.Serialize(streamReq);
        using var httpReq = new HttpRequestMessage(HttpMethod.Post, "/v1/chat/completions")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        httpReq.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

        using var response = await _httpClient.SendAsync(httpReq, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        using var reader = new System.IO.StreamReader(await response.Content.ReadAsStreamAsync());
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data: "))
                continue;
            var jsonData = line.Substring("data: ".Length);
            if (jsonData.Trim() == "[DONE]")
                yield break;
            var partial = JsonSerializer.Deserialize<ChatResponse>(jsonData);
            if (partial?.Choices != null)
            {
                foreach (var c in partial.Choices)
                    if (c.Delta.Content != null)
                        yield return c.Delta.Content;
            }
        }
    }

    public Task<ChatResponse> CreateChatCompletionWithJsonModeAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ChatResponse> CreateChatCompletionWithToolsAsync(ChatRequest request, IEnumerable<ToolDefinition> tools, ToolChoice toolChoice = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ChatResponse> CreateChatCompletionWithUserAsync(ChatRequest request, string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<ModelsResponse>> GetAvailableModelsAsync(CancellationToken cancellationToken = default)
    {
        //var response = await _httpClient.GetAsync("v1/models", cancellationToken);
        try
        {
            var response = await _httpClientService.TryGetContentStreamAsync("v1/models", cancellationToken);
            return (await response.TryDeserializeJsonAsync<ModelsResponse>())
                    .AsResultSuccess();
        }
        catch (Exception ex)
        {
            return Result<ModelsResponse>.Failure("Error on processing models.");
        }
    }

    public Task<ModelDetails> GetModelDetailsAsync(string modelId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

