using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Udemy.Auth.API.Middlewares;

public class ArgumentNullExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ArgumentNullException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Argument Null Exception",
                Type = "ArgumentFailure",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = problemDetails.Status.Value;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}