using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Udemy.Common.ExceptionMiddlewares;

public class ArgumentNullExceptionHandler(ILogger<ArgumentNullExceptionHandler> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ArgumentNullException exception)
        {
            var exceptionId = Guid.NewGuid();
            var exceptionMessage = exception.Message;
            logger.LogError(exception, "Id: {@Id}\nError Message: {@ExceptionMessage}\nStackTrace: {@StackTrace}", exceptionId, exceptionMessage, exception.StackTrace);

            var problemDetails = new ProblemDetails
            {
                Title = $"ArgumentNullError: {exceptionMessage}",
                Type = "ArgumentNullFailure",
                Status = StatusCodes.Status400BadRequest,
                Detail = exceptionId.ToString(),
                Instance = context.Request.Path
            };

            context.Response.StatusCode = problemDetails.Status.Value;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}