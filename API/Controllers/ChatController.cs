using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class ModelController(IChatApplicationService chatService) : ControllerBase
{
    [HttpGet("models")]
    public async Task<IActionResult> GetModels() => Ok(await chatService.GetAvailableModelsAsync());
}

[ApiController]
[Route("api/v1/[controller]")]
public sealed class ChatController(IChatApplicationService chatService) : ControllerBase
{
    [HttpPost("completions")]
    public async Task<IActionResult> ChatCompletions([FromBody] OpenWebUIChatRequest request)
    {
        var response = await chatService.CreateChatCompletionAsync(request);
        return Ok(new OpenWebUIChatResponse
        {
            Id = "chatcmpl-abc123",
            Object = "chat.completion.chunk",
            Created = 1748455199,
            Model = "deepseek/deepseek-r1-0528:free",
            Choices = new List<ChatChoice>
            {
                new ChatChoice
                {
                    Message = new ChatMessage
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
}