using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Udemy.CDN.Domain.Interfaces;
using Udemy.Common.Consul;

namespace Udemy.CDN.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var minioEndpoint = Environment.GetEnvironmentVariable("MINIO_URL");
        var accessKey = Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY");
        var secretKey = Environment.GetEnvironmentVariable("MINIO_SECRET_KEY");
        services.AddMinio(configureClient => configureClient
            .WithEndpoint(minioEndpoint)
            .WithCredentials(accessKey, secretKey)
            .Build());

        services.AddScoped<IMinioService, MinioService>();

        services.AddConsul(configuration);

        return services;
    }
}