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

        var roles = context.Request.Headers["X-User-Roles"].FirstOrDefault()?.Split(",");
        var isAuthenticated = context.Request.Headers["X-User-IsAuthenticated"].FirstOrDefault();
        var name = context.Request.Headers["X-User-Name"].FirstOrDefault();
        var authType = context.Request.Headers["X-User-AuthType"].FirstOrDefault();

        if (roles == null || isAuthenticated == null || name == null || authType == null)
        {
            return;
        }

        var identity = new GenericIdentity(name, authType);
        var principal = new GenericPrincipal(identity, roles);

        context.User = principal;
    }
}