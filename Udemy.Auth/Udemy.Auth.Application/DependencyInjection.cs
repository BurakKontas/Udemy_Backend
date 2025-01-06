using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Udemy.Auth.Application.Services;
using Udemy.Auth.Domain.Interfaces;
using Udemy.Common.Extensions;

namespace Udemy.Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<IAuthService, AuthService>();

        services.AddMassTransitExtension(typeof(DependencyInjection).Assembly);

        return services;
    }
}