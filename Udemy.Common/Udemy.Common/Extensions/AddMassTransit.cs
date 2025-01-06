using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Udemy.Common.Extensions;

public static class AddMassTransit
{
    public static IServiceCollection AddMassTransitExtension(this IServiceCollection services, Assembly assembly)
    {
        var rabbitMqConnectionString = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION") ?? "amqp://admin:admin123@rabbitmq:5672";

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumers(assembly);

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqConnectionString);

                cfg.UseInMemoryOutbox(context);

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}