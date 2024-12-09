using Udemy.APIGateway.API;
using Udemy.APIGateway.API.Middlewares;
using Udemy.Common.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConsul(builder.Configuration);
builder.Services.AddYarp(builder.Configuration, builder.Environment);
builder.Services.AddTransient<IMiddleware, IdentityMiddleware>();
builder.Services.AddHttpClient<IdentityMiddleware>(client =>
{
    client.BaseAddress = new Uri("https://udemy.auth.api:5001");
});

var app = builder.Build();

app.MapReverseProxy();

app.UseMiddleware<IdentityMiddleware>();

app.Run();
