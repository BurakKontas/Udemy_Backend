﻿using System.Reflection;
using Consul;
using Microsoft.Extensions.Hosting;

namespace Udemy.Common.Consul;

public class ConsulHostedService(IConsulClient consulClient) : IHostedService
{
    private readonly IConsulClient _consulClient = consulClient;
    private string _registrationId = null!;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var name = Assembly.GetEntryAssembly()?.GetName().Name;
        var tags = name?.Split(".");
        var host = Environment.GetEnvironmentVariable("API_HOST") ?? throw new ArgumentNullException($"env:API_HOST");
        var port = Convert.ToInt32(Environment.GetEnvironmentVariable("API_HTTPS_PORT") ?? throw new ArgumentNullException($"env:API_HTTPS_PORT"));
        var registration = new AgentServiceRegistration
        {
            ID = Guid.NewGuid().ToString(),
            Name = name,
            Address = $"https://{host}",
            Port = port,
            Tags = tags
        };

        _registrationId = registration.ID;

        await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
        Console.WriteLine($"Service registered to consul as [{name}].");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _consulClient.Agent.ServiceDeregister(_registrationId, cancellationToken);
        Console.WriteLine("Service removed from consul.");
    }
}
