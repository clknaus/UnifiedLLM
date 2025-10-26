using Application.Interfaces;
using Application.Models;
using Core.Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Endpoints;
public static class ChatEndpoints
{
    public static void Map(this WebApplication app)
    {
        app.MapGet("/api/v1/models", async (
            IQueryHandler<IModelsResponse> handler
        ) =>
        {
            var result = await handler.HandleAsync();
            return result.ToMinimalApiResult();
        })
        .WithName("GetModels")
        .WithTags("Models");

        app.MapPost("/api/v1/chat/completions", async (
            HttpContext context,
            ICommandHandler<IChatRequest, IChatResponse> handler,
            CancellationToken cancellationToken,
            [FromBody] OpenWebUIChatRequest request
        ) =>
        {
            var response = context.Response;
            response.Headers.ContentType = "text/event-stream; charset=utf-8";
            response.Headers.CacheControl = "no-cache";
            response.Headers.Connection = "keep-alive";

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            try
            {
                await foreach (var chunkResult in handler.HandleStreamAsync(request, cancellationToken))
                {

                    if (chunkResult.IsFailure)
                    {
                        await response.WriteAsync($"data: {JsonSerializer.Serialize(new { error = chunkResult.ErrorMessage }, jsonSerializerOptions)}\n\n", cancellationToken);
                        await response.Body.FlushAsync(cancellationToken);
                        break;
                    }

                    await response.WriteAsync($"data: {JsonSerializer.Serialize(chunkResult.Value, jsonSerializerOptions)}\n\n", cancellationToken);
                    await response.Body.FlushAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                await response.WriteAsync($"data: {JsonSerializer.Serialize(new { error = "The Message was terminated due to an Error." }, jsonSerializerOptions)}\n\n", cancellationToken);
                await response.Body.FlushAsync(cancellationToken);
                // TODO log
            }
        });

    }
}
