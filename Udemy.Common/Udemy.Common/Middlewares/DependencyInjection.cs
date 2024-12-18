using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Udemy.Common.Middlewares;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthMiddleware(this IServiceCollection services)
    {
        services.AddTransient<AuthMiddleware>();
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Auth.Authentication";
            })
            .AddScheme<AuthenticationSchemeOptions, AuthAuthenticationHandler>("Auth.Authentication", _ => {});
        services.AddAuthorization();

        return services;
    }

    public static WebApplication AddAuthMiddleware(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<AuthMiddleware>();

        return app;
    }

    public static IServiceCollection AddTransactionMiddleware<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        services.AddTransient<TransactionMiddleware<TContext>>();

        return services;
    }

    public static WebApplication AddTransactionMiddleware<TContext>(this WebApplication app) where TContext : DbContext
    {
        app.UseMiddleware<TransactionMiddleware<TContext>>();

        return app;
    }
}