using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Udemy.Common.Extensions;

public static class AddMassTransit
{
    public static IServiceCollection AddMassTransitExtension(this IServiceCollection services, string connectionString, Assembly assembly)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumers(assembly);

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(connectionString);

                cfg.UseInMemoryOutbox(context);

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}