using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Udemy.Common.Extensions;

namespace Udemy.Payment.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConnectionString = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION") ?? throw new ArgumentNullException($"env:RABBITMQ_CONNECTION");
        services.AddMassTransitExtension(rabbitMqConnectionString, typeof(DependencyInjection).Assembly);

        return services;
    }
}