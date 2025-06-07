using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Infrastructure.Models;

public class StreamResult<T> : IActionResult
{
    private readonly IAsyncEnumerable<T> _stream;

    public StreamResult(IAsyncEnumerable<T> stream)
    {
        _stream = stream;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        // Set the response headers for streaming
        response.ContentType = "application/json";
        response.StatusCode = 200;

        // Write the chunks to the response body as they come in
        await foreach (var chunk in _stream)
        {
            var json = JsonSerializer.Serialize(chunk);
            var jsonData = $"data: {json}\n\n"; // This is the SSE format
            await response.WriteAsync(jsonData);
            await response.Body.FlushAsync(); // Ensure data is sent immediately
        }

        // Optionally, you can add an ending message like "[DONE]" to indicate the end of the stream
        await response.WriteAsync("data: [DONE]\n\n");
        await response.Body.FlushAsync();
    }
}

