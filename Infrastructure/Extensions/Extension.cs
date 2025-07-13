using Core.General.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Extensions;
public static class Extension
{
    public static IActionResult AsActionResult<T>(this Result<T> result)
    {
        if (result?.IsSuccess ?? false)
        {
            return new OkObjectResult(result.Value);
        }

        return new BadRequestObjectResult(new ProblemDetails
        {
            Title = "Invalid request",
            Detail = result?.Error ?? "An error occurred while processing the request.",
            Status = StatusCodes.Status400BadRequest
        });
    }
}
