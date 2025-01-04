using System.Globalization;
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
        var minioPortString = Environment.GetEnvironmentVariable("MINIO_PORT");

        minioPortString ??= "9000";

        var minioPort = int.Parse(minioPortString, CultureInfo.InvariantCulture);

        services.AddMinio(configureClient => configureClient
            .WithEndpoint(minioEndpoint, minioPort)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(false)
            .Build());

        services.AddScoped<IMinioService, MinioService>();

        services.AddConsul(configuration);

        return services;
    }
}