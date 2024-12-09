using Udemy.APIGateway.API;
using Udemy.APIGateway.API.Middlewares;
using Udemy.Common.Consul;
using Udemy.Common.ExceptionMiddlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConsul(builder.Configuration);
builder.Services.AddYarp(builder.Configuration, builder.Environment);
builder.Services.AddHttpClient();
builder.Services.AddTransient<IdentityMiddleware>();

builder.Services.AddExceptionMiddlewares();

var app = builder.Build();

app.MapReverseProxy();

app.AddExceptionMiddlewares();
app.UseMiddleware<IdentityMiddleware>();

app.Run();
