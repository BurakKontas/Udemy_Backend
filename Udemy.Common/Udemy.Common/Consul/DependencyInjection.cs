using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Udemy.Common.Consul;

public static class DependencyInjection
{
    /// <summary>
    /// Do not forget to add CONSUL_URL, API_HOST, and API_HTTPS_PORT to your environment variables.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(cfg =>
        {
            var consulUrl = Environment.GetEnvironmentVariable("CONSUL_URL") ?? throw new ArgumentNullException($"env:CONSUL_URL");
            cfg.Address = new Uri(consulUrl);
        }));

        services.AddSingleton<IHostedService, ConsulHostedService>();
        services.AddScoped<IConsulDiscoveryService, ConsulDiscoveryService>();

        return services;
    }
}