using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Udemy.Common.ExceptionMiddlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception) when (exception is ArgumentNullException)
        {
            await HandleExceptionAsync(context, exception, "ArgumentNullFailure", StatusCodes.Status400BadRequest);
        }
        catch (Exception exception) when (exception is AuthenticationFailureException)
        {
            await HandleExceptionAsync(context, exception, "AuthenticationFailure", StatusCodes.Status403Forbidden);
        }
        catch (Exception exception) when (exception is ValidationException)
        {
            await HandleExceptionAsync(context, exception, "ValidationFailure", StatusCodes.Status400BadRequest);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception, "ServerError", StatusCodes.Status500InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, string type, int statusCode)
    {
        var exceptionId = Guid.NewGuid();
        var exceptionMessage = exception.Message;

        logger.LogError(exception, "Id: {@Id}\nError Message: {@ExceptionMessage}\nStackTrace: {@StackTrace}",
            exceptionId, exceptionMessage, exception.StackTrace);

        var problemDetails = new ProblemDetails
        {
            Title = $"{type} Error: {exceptionMessage}",
            Type = type,
            Status = statusCode,
            Detail = exceptionId.ToString(),
            Instance = context.Request.Path
        };

        context.Response.StatusCode = problemDetails.Status.Value;
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}