using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Udemy.Common.Middlewares;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthMiddleware(this IServiceCollection services)
    {
        services.AddTransient<AuthMiddleware>();

        return services;
    }

    public static WebApplication AddAuthMiddleware(this WebApplication app)
    {
        app.UseMiddleware<AuthMiddleware>();

        return app;
    }
}