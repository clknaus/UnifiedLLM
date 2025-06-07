using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class ModelController(IChatService chatService) : ControllerBase
{
    [HttpGet("models")]
    public async Task<IActionResult> GetModels()
    {
        var response = await chatService.GetAvailableModelsAsync();
        if (response == null)
            return BadRequest();

        return Ok(response);
    }
}

[ApiController]
[Route("api/v1/[controller]")]
public sealed class ChatController(IChatService chatService) : ControllerBase
{
    [HttpPost("completions")]
    public async Task<IActionResult> ChatCompletions([FromBody] OpenWebUIChatRequest request)
    {
        if (request == null)
            return BadRequest();

        var response = await chatService.CreateChatCompletionAsync(request);
        if (response == null)
            return BadRequest();

        return Ok(response);
    }
}