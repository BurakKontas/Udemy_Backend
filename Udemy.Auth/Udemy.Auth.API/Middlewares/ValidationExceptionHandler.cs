using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Udemy.Auth.API.Middlewares;

public class ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException exception)
        {
            var exceptionId = Guid.NewGuid();
            var exceptionMessage = exception.Message;
            logger.LogError(exception, "Id: {@Id}\nError Message: {@ExceptionMessage}\nStackTrace: {@StackTrace}", exceptionId, exceptionMessage, exception.StackTrace);

            var problemDetails = new ProblemDetails
            {
                Title = $"Validation Error: {exceptionMessage}",
                Type = "ValidationFailure",
                Status = StatusCodes.Status400BadRequest,
                Detail = exceptionId.ToString(),
                Instance = context.Request.Path
            };

            if (exception.Errors != null)
            {
                problemDetails.Extensions["errors"] = exception.Errors;
            }

            context.Response.StatusCode = problemDetails.Status.Value;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}