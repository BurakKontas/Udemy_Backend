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
        var receivedApiKey = context.Request.Headers["X-Api-Key"];

        if (!receivedApiKey.Equals(API_KEY)) throw new AuthenticationException("API_KEY is not valid.");

        var roles = context.Request.Headers["X-User-Roles"].FirstOrDefault()?.Split(",");
        var isAuthenticated = context.Request.Headers["X-User-IsAuthenticated"].FirstOrDefault();
        var name = context.Request.Headers["X-User-Name"].FirstOrDefault();
        var authType = context.Request.Headers["X-User-AuthType"].FirstOrDefault();

        if (roles == null || isAuthenticated == null || name == null || authType == null)
        {
            // some of the informations are missing
            await next(context);
            return;
        }

        var identity = new GenericIdentity(name, authType);
        var principal = new GenericPrincipal(identity, roles);

        context.User = principal;

        await next(context);
    }
}