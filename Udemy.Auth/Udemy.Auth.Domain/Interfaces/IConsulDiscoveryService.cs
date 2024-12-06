using Consul;

namespace Udemy.Auth.Domain.Interfaces;

public interface IConsulDiscoveryService
{
    Task<string> GetServiceUriAsync(string serviceName);
    Task<Dictionary<string, AgentService>> GetServices();
}