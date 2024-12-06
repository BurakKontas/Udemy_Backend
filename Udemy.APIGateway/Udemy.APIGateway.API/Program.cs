using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Udemy.APIGateway.API;
using Udemy.Common.Consul;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddConsul(builder.Configuration);
builder.Services.AddYarp(builder.Configuration, builder.Environment);

var app = builder.Build();

app.MapReverseProxy();

app.Run();
