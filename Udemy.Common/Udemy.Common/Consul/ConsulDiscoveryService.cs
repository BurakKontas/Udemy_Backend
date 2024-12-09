using System.Net;
using Consul;

namespace Udemy.Common.Consul;

public class ConsulDiscoveryService(IConsulClient consulClient) : IConsulDiscoveryService
{
    private readonly IConsulClient _consulClient = consulClient;

    public async Task<string> GetServiceUriAsync(string serviceName)
    {
        var services = await _consulClient.Agent.Services();
        var service = services.Response.Values.FirstOrDefault(s => s.Service == serviceName);

        if (service == null)
            throw new Exception($"Service not found: {serviceName}");

        return $"{service.Address}:{service.Port}";
    }

    public async Task<Dictionary<string, AgentService>> GetServices()
    {
        var services = await _consulClient.Agent.Services();

        if (services.StatusCode != HttpStatusCode.OK)
            throw new Exception("Failed to retrieve services from Consul.");

        return services.Response;
    }
}