using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Extensions;
public static class Extension
{
    public static IActionResult AsActionResult<T>(this Result<T> result)
    {
        if (result == null || !result.IsSuccess)
            return new BadRequestResult();

        return new OkObjectResult(result.Value);
    }
}
