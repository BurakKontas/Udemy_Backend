using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Udemy.Common.Consul;

var builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .ConfigureHttpClient((context, handler) =>
    {
        // This is required to allow the reverse proxy to make requests to insecure servers. (For debugging)
        if(isDevelopment)
            handler.SslOptions.RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true;
        
    });

builder.Services.AddConsul(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy();

app.Run();
