using Core.General.Models;
using Microsoft.AspNetCore.Http;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Infrastructure.Extensions;
public static class Extension
{
    public static IResult ToMinimalApiResult<T>(this Result<T> result)
    {
        return result == null
            ? throw new ArgumentNullException(nameof(result))
            : result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.ErrorMessage);
    }
}
