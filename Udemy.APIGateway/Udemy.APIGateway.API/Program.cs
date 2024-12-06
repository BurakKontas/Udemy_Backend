using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;

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

var app = builder.Build();

app.MapReverseProxy();

app.Run();
