using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System;

namespace Udemy.Auth.API.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var exceptionId = Guid.NewGuid();
            var exceptionMessage = exception.Message;
            logger.LogError(exception, "Id: {@Id}\nError Message: {@ExceptionMessage}\nStackTrace: {@StackTrace}", exceptionId, exceptionMessage, exception.StackTrace);

            var problemDetails = new ProblemDetails
            {
                Title = $"Server Error: {exceptionMessage}",
                Status = StatusCodes.Status500InternalServerError,
                Instance = context.Request.Path,
                Detail = exceptionId.ToString(),
            };
            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}