using Application.Interfaces;
using Application.Models;
using Core.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class ModelController(IChatService chatService) : ControllerBase
{
    [HttpGet("models")]
    public async Task<IActionResult> GetModels() => (await chatService.GetAvailableModelsAsync()).AsActionResult();
}

[ApiController]
[Route("api/v1/[controller]")]
public sealed class ChatController(IChatService chatService) : ControllerBase
{
    [HttpPost("completions")]
    public async Task<IActionResult> ChatCompletions([FromBody] OpenWebUIChatRequest request)
    {
        if (request?.Model == null)
            return BadRequest();

        var response = await chatService.CreateChatCompletionAsync(request);
        return response.AsActionResult();
    }
}