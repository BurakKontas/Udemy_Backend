using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Udemy.Auth.Domain;

namespace Udemy.Auth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddAuthentication()
            //.AddCookie(IdentityConstants.ApplicationScheme, options =>
            //{
            //    options.Cookie.Expiration = TimeSpan.FromDays(150);
            //})
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "udemydb";
        var username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "udemyuser";
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "udemypassword";

        var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";


        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}