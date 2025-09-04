using Application.Interfaces;
using Application.Models;
using Core.Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

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
            ICommandHandler<IChatRequest, IChatResponse> handler,
            [FromBody] OpenWebUIChatRequest request
        ) =>
        {
            var result = await handler.HandleAsync(request);
            return result.ToMinimalApiResult();
        })
        .WithName("ChatCompletions")
        .WithTags("Chat"); // Swagger grouping
    }
}
