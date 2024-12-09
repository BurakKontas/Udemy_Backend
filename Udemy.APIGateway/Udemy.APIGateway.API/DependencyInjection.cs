using System.Reflection;
using Consul;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy.Configuration;
using DestinationConfig = Yarp.ReverseProxy.Configuration.DestinationConfig;
using RouteConfig = Yarp.ReverseProxy.Configuration.RouteConfig;

namespace Udemy.APIGateway.API;

public static class DependencyInjection
{
    public static IServiceCollection AddYarp(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        ConfigureConsulClients(services, out var routeList, out var clusterList);

        services.AddSingleton<IProxyConfigProvider>(new InMemoryConfigProvider(routeList, clusterList));

        services.AddReverseProxy()
            .ConfigureHttpClient((context, handler) =>
            {
                // This is required to allow the reverse proxy to make requests to insecure servers. (For debugging)
                var isDevelopment = environment.IsDevelopment();
                if (isDevelopment)
                    handler.SslOptions.RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            });


        return services;

    }

    // Only used in ConfigureConsulClients for logging purposes
    private abstract class ConfigureConsultClients {}

    private static void ConfigureConsulClients(IServiceCollection services, out List<RouteConfig> routeList, out List<ClusterConfig> clusterList)
    {
        var scope = services.BuildServiceProvider().CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ConfigureConsultClients>>();
        var consulClient = scope.ServiceProvider.GetRequiredService<IConsulClient>();

        routeList = [];
        clusterList = [];
        var consulServices = consulClient.Agent.Services().Result;
        var serviceList = consulServices.Response.Values
            .Select(x => new { x.Service, x.Port, x.Address })
            .DistinctBy(x => x.Service)
            .ToList();

        foreach (var service in serviceList)
        {
            var name = Assembly.GetExecutingAssembly().GetName().Name?.Split(".")[1].ToLower();
            var discriminator = service.Service.Split(".")[1].ToLower(); // e.g "Udemy.Auth.API" -> "Auth"
            if (discriminator == name)
                continue;

            var clusterId = $"cluster_{discriminator}";
            var routeId = $"route_{discriminator}";

            var cluster = new ClusterConfig
            {
                ClusterId = clusterId,
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    {
                        $"http-destination-{discriminator}",
                        new DestinationConfig
                        {
                            //Address = $"{service.Address}:{service.Port}",
                            Address = $"https://host.docker.internal:{service.Port}", // Postman does not work with service.Address since its in docker and postman is not
                            Metadata = new Dictionary<string, string>
                            {
                                { "Scheme", "http" }
                            }
                        }
                    }
                }
            };

            var route = new RouteConfig
            {
                RouteId = routeId,
                ClusterId = clusterId,
                Match = new RouteMatch
                {
                    Path = $"/api/{discriminator}/{{**catch-all}}"
                },
                //Metadata = new Dictionary<string, string>
                //{
                //    { "AuthorizationPolicy", $"{discriminator.ToLower()}-policy" }
                //},
                Transforms = new List<IReadOnlyDictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        { "PathPattern", "{**catch-all}" }
                    }
                }
            };

            routeList.Add(route);
            clusterList.Add(cluster);
            logger.LogInformation("Route: {RouteId}, Cluster: {ClusterId}, Path: /api/{Discriminator}/{{**catch-all}}",
                routeId, clusterId, discriminator);
        }
    }
}