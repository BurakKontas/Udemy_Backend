using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Udemy.Auth.API.Middlewares;

public class ValidationExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Validation Error",
                Type = "ValidationFailure",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            if (ex.Errors != null)
            {
                problemDetails.Extensions["errors"] = ex.Errors;
            }

            context.Response.StatusCode = problemDetails.Status.Value;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}