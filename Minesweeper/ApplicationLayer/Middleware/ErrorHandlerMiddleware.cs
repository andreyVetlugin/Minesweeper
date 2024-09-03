using Minesweeper.ApplicationLayer.CustomExceptions;

namespace Minesweeper.ApplicationLayer.Middleware;

using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class ErrorHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex) when (ex is InvalidMapParametersException or InvalidTurnParameterException)
        {
            await HandleBadRequestExceptionsAsync(context, ex);
        }
    }

    private Task HandleBadRequestExceptionsAsync(HttpContext context, Exception exception)
    {
        var result = JsonSerializer.Serialize(new { error = exception.Message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return context.Response.WriteAsync(result);
    }
}