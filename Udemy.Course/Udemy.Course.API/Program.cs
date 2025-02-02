using Asp.Versioning;
using Udemy.Common.ExceptionMiddlewares;
using Udemy.Common.Extensions;
using Udemy.Common.Helpers;
using Udemy.Common.Middlewares;
using Udemy.Course.Application;
using Udemy.Course.Infrastructure;
using Udemy.Course.Infrastructure.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionMiddlewares();
builder.Services.AddAuthMiddleware();
builder.Services.AddApiVersioningExtension();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations<ApplicationDbContext>();
}

app.AddExceptionMiddlewares();

app.AddAuthMiddleware();

app.UseMiddleware<TransactionMiddleware<ApplicationDbContext>>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
