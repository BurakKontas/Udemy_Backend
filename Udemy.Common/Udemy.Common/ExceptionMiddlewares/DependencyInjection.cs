using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Udemy.Common.ExceptionMiddlewares;

public static class DependencyInjection
{
    public static IServiceCollection AddExceptionMiddlewares(this IServiceCollection services)
    {
        // Register custom middleware services
        services.AddTransient<ArgumentNullExceptionHandler>();
        services.AddTransient<ValidationExceptionHandler>();
        services.AddTransient<GlobalExceptionHandler>();

        return services;
    }

    public static WebApplication AddExceptionMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ArgumentNullExceptionHandler>();
        app.UseMiddleware<ValidationExceptionHandler>();
        app.UseMiddleware<GlobalExceptionHandler>();

        return app;
    } 
}