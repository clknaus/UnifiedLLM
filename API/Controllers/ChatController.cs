using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using UnifiedLLM.Core.Models;

namespace API.Controllers;

[ApiController]
[Route("api/v1")]
public class ModelController(IChatService chatService) : ControllerBase
{
    [HttpGet("models")]
    public async Task<IActionResult> GetModels()
    {
        var models = await chatService.GetAvailableModelsAsync();
        return Ok(models);
    }
}

[ApiController]
[Route("api/v1/[controller]")]
public class ChatController(IChatService chatService) : ControllerBase
{
    [HttpPost("completions")]
    public async Task<IActionResult> ChatCompletions([FromBody] OpenWebUIChatRequest request)
    {
        return Ok(new OpenWebUIChatResponse
        {
            Id = "chatcmpl-abc123",
            Object = "chat.completion.chunk",
            Created = 1748455199,
            Model = "deepseek/deepseek-r1-0528:free",
            Choices = new List<Core.Models.ChatChoice>
            {
                new Core.Models.ChatChoice
                {
                    Message = new Core.Models.ChatMessage
                    {
                        Role = "assistant",
                        Content = "hello"
                    },
                    Index = 0,
                    FinishReason = "end"
                }
            }
        });
    }

    [HttpGet("models")]
    public async Task<IActionResult> GetModels() {
        var models = await chatService.GetAvailableModelsAsync();
        return Ok(models);
    }
}