using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Udemy.Common.ExceptionMiddlewares;

public static class DependencyInjection
{
    public static IServiceCollection AddExceptionMiddlewares(this IServiceCollection services)
    {
        // Register custom middleware services
        services.AddTransient<GlobalExceptionHandler>();

        return services;
    }

    public static WebApplication AddExceptionMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandler>();

        return app;
    } 
}