using Application.Interfaces;
using Application.Models;
using Core.Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class ModelController(IQueryHandler<IModelsResponse> getAvailableModelsQueryHandler) : ControllerBase
{
    [HttpGet("models")]
    public async Task<IActionResult> GetModels() => 
        (await getAvailableModelsQueryHandler.HandleAsync()).AsActionResult();
}

[ApiController]
[Route("api/v1/[controller]")]
public sealed class ChatController(ICommandHandler<IChatRequest, IChatResponse> createChatCompletionCommandHandler) : ControllerBase
{
    [HttpPost("completions")]
    public async Task<IActionResult> ChatCompletions([FromBody] OpenWebUIChatRequest request) =>
        (await createChatCompletionCommandHandler.HandleAsync(request)).AsActionResult();
}

// TODO Test injection
