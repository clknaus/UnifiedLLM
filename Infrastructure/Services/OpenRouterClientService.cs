using Abstractions;
using Application.Models;
using Core.Domain.Interfaces;
using Core.General.Models;
using Infrastructure.Interfaces.OpenRouter;
using Infrastructure.Models.OpenRouter;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;
public class OpenRouterClientService : IOpenRouterClientService
{
    private readonly HttpClient _httpClient;
    private readonly OpenRouterConfiguration _opts;

    public OpenRouterClientService(HttpClient httpClient, IOptions<OpenRouterConfiguration> opts)
    {
        _httpClient = httpClient;
        _opts = opts.Value;
        _httpClient.BaseAddress = new Uri(_opts.BaseUrl);
    }

    public async Task<Result<IChatResponse>> CreateChatCompletionAsync(IChatRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            string json = JsonSerializer.Serialize(request, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync("v1/chat/completions", content, cancellationToken);
            response.EnsureSuccessStatusCode();
            _httpClient?.Dispose();

            var res = await JsonSerializer.DeserializeAsync<OpenRouterChatResponse>(
                       await response.Content.ReadAsStreamAsync(cancellationToken),
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                       cancellationToken
                   );

            return res.AsResultSuccess<IChatResponse>();
        }
        catch (Exception)
        {
            _httpClient?.Dispose();
            return Result<IChatResponse>.Failure();
        }
    }

    public async IAsyncEnumerable<OpenRouterChatStreamResponse?> GetChatCompletionStreamAsync(IChatRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string json = JsonSerializer.Serialize(request, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await _httpClient.PostAsync("v1/chat/completions", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            // Skip empty lines or event delimiters
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Look for 'data:' prefix in SSE format
            if (line.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                string jsonData = line.Substring(5).Trim();  // Extract JSON after "data:"

                // Special case: Check for the "[DONE]" marker which indicates end of stream or no more data
                if (jsonData.Equals("[DONE]", StringComparison.OrdinalIgnoreCase))
                {
                    // Optionally handle the end of the stream here, maybe notify the caller.
                    yield break;  // End the stream
                }

                if (!string.IsNullOrEmpty(jsonData))
                {
                    OpenRouterChatStreamResponse chatResponse = null;
                    try
                    {
                        // Attempt to deserialize the JSON data chunk
                        chatResponse = JsonSerializer.Deserialize<OpenRouterChatStreamResponse>(jsonData);
                    }
                    catch (JsonException ex)
                    {
                        // If deserialization fails, log the error and continue processing other chunks
                        Console.WriteLine($"Error deserializing data: {jsonData}. Exception: {ex.Message}");
                    }

                    if (chatResponse != null)
                    {
                        // Yield each response chunk as it arrives
                        yield return chatResponse;
                    }
                }
            }
        }
    }

    public async Task<Result<IModelsResponse>> GetAvailableModelsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("v1/models", cancellationToken);
            var res = await JsonSerializer.DeserializeAsync<OpenRouterModelsResponse>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                cancellationToken
            );
            _httpClient.Dispose();

            return res.AsResultSuccess<IModelsResponse>();
        }
        catch (Exception)
        {
            _httpClient?.Dispose();
            return Result<IModelsResponse>.Failure();
        }
    }

    //public IAsyncEnumerable<string> StreamChatCompletionAsync(ChatRequest request, CancellationToken cancellationToken = default)
    //{
    //    return StreamChatCompletionsInternalAsync(request, cancellationToken);
    //}

    //private async IAsyncEnumerable<string> StreamChatCompletionsInternalAsync(ChatRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    //{
    //    var streamReq = new ChatRequest
    //    {
    //        //Provider = request.Provider,
    //        //Model = request.Model,
    //        //Messages = request.Messages,
    //        //Temperature = request.Temperature,
    //        //MaxTokens = request.MaxTokens
    //    };

    //    string json;
    //    try
    //    {
    //        json = JsonSerializer.Serialize(streamReq);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new InvalidOperationException("Failed to serialize the chat request.", ex);
    //    }

    //    using var httpReq = new HttpRequestMessage(HttpMethod.Post, "/v1/chat/completions")
    //    {
    //        Content = new StringContent(json, Encoding.UTF8, "application/json")
    //    };
    //    httpReq.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

    //    HttpResponseMessage response;
    //    try
    //    {
    //        response = await _httpClient.SendAsync(httpReq, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
    //        response.EnsureSuccessStatusCode();
    //    }
    //    catch (HttpRequestException ex)
    //    {
    //        throw new InvalidOperationException("HTTP request failed.", ex);
    //    }

    //    await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
    //    using var reader = new StreamReader(stream);

    //    int malformedCount = 0;
    //    const int malformedThreshold = 5;

    //    while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
    //    {
    //        string? line;

    //        try
    //        {
    //            line = await reader.ReadLineAsync();
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new InvalidOperationException("Failed while reading streamed content.", ex);
    //        }

    //        if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data: "))
    //            continue;

    //        var jsonData = line.Substring("data: ".Length).Trim();

    //        if (jsonData == "[DONE]")
    //            yield break;

    //        bool isMalformed = false;
    //        ChatResponse? partial = null;

    //        try
    //        {
    //            partial = JsonSerializer.Deserialize<ChatResponse>(jsonData);
    //        }
    //        catch (JsonException ex)
    //        {
    //            Console.Error.WriteLine($"Malformed JSON skipped: {jsonData}\n{ex}");
    //            malformedCount++;
    //            isMalformed = true;
    //        }

    //        if (isMalformed)
    //        {
    //            if (malformedCount >= malformedThreshold)
    //            {
    //                Console.Error.WriteLine($"Too many malformed chunks ({malformedCount}). Stopping stream.");
    //                yield break;
    //            }

    //            yield return "<!-- invalid chunk -->";
    //            continue;
    //        }
    //        else
    //        {
    //            malformedCount = 0;
    //        }

    //        if (partial?.Choices != null)
    //        {
    //            foreach (var c in partial.Choices)
    //            {
    //                if (c.Delta?.Content != null)
    //                    yield return c.Delta.Content;
    //            }
    //        }
    //    }
    //}


    //public Task<ChatResponse> CreateChatCompletionWithJsonModeAsync(ChatRequest request, CancellationToken cancellationToken = default)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<ChatResponse> CreateChatCompletionWithToolsAsync(ChatRequest request, IEnumerable<ToolDefinition> tools, ToolChoice toolChoice = null, CancellationToken cancellationToken = default)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<ChatResponse> CreateChatCompletionWithUserAsync(ChatRequest request, string userId, CancellationToken cancellationToken = default)
    //{
    //    throw new NotImplementedException();
    //}


    //public Task<ModelDetails> GetModelDetailsAsync(string modelId, CancellationToken cancellationToken = default)
    //{
    //    throw new NotImplementedException();
    //}
}

