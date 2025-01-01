using Udemy.Auth.Application;
using Udemy.Auth.Infrastructure;
using Udemy.Auth.Infrastructure.Context;
using Udemy.Common.ExceptionMiddlewares;
using Udemy.Common.Extensions;
using Udemy.Common.Helpers;
using Udemy.Common.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAuthMiddleware();
builder.Services.AddApiVersioningExtension();

builder.Services.AddExceptionMiddlewares();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations<ApplicationDbContext>();
}

//app.MapIdentityApi<User>();

//app.UseMiddleware<TransactionMiddleware<ApplicationDbContext>>(); // it causes an error but idk why

app.AddExceptionMiddlewares();

app.AddAuthMiddleware();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
