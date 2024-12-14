using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace Udemy.Common.Middlewares;

public class AuthMiddleware : IMiddleware
{
    private readonly string API_KEY = Environment.GetEnvironmentVariable("API_KEY") ?? throw new ArgumentNullException($"env:API_KEY");

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await Middleware(context);
        await next(context);
    }

    public async Task Middleware(HttpContext context)
    {
        var receivedApiKey = context.Request.Headers["X-Api-Key"];

        if (!receivedApiKey.Equals(API_KEY)) throw new AuthenticationException("API_KEY is not valid.");
    }
}