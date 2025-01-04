using Udemy.CDN.Infrastructure;
using Udemy.Common.Consul;
using Udemy.Common.ExceptionMiddlewares;
using Udemy.Common.Extensions;
using Udemy.Common.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthMiddleware();
builder.Services.AddApiVersioningExtension();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionMiddlewares();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddExceptionMiddlewares();

app.AddAuthMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
