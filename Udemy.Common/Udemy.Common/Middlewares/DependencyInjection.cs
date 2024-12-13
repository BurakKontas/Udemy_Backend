using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
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
}