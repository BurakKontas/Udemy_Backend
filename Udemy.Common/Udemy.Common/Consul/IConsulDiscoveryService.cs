using Consul;

namespace Udemy.Common.Consul;

public interface IConsulDiscoveryService
{
    Task<string> GetServiceUriAsync(string serviceName);
    Task<Dictionary<string, AgentService>> GetServices();
}