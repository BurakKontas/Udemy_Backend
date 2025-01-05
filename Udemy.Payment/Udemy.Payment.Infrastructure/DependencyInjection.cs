using Iyzipay;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Udemy.Common.Consul;
using Udemy.Common.Extensions;
using Udemy.Common.Helpers;
using Udemy.Payment.Domain.Interfaces;
using Udemy.Payment.Infrastructure.Contexts;
using Udemy.Payment.Infrastructure.Repositories;

namespace Udemy.Payment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var apikey = configuration["iyzipay:api_key"] ?? throw new ArgumentNullException($"env:iyzipay:api_key");
        var secretKey = configuration["iyzipay:api_secret"] ?? throw new ArgumentNullException($"env:iyzipay:api_key");
        var baseUrl = configuration["iyzipay:base_url"] ?? throw new ArgumentNullException($"env:iyzipay:base_url");

        var options = new Options
        {
            ApiKey = apikey,
            SecretKey = secretKey,
            BaseUrl = baseUrl
        };

        services.AddSingleton(options);

        services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
        {
            var connectionString = PostgresConnectionOptions.FromEnvironment()
                .BuildConnectionString();
            optionsBuilder.UseNpgsql(connectionString);
        });

        services.AddScoped<IIyzipayRepository, IyzipayRepository>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IUserDataRepository, UserDataRepository>();
        services.AddScoped<IBasketRepository, BasketRepository>();

        services.AddConsul(configuration);

        return services;
    }
}