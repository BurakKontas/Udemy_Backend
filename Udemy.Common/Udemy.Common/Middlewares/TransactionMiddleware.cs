using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Udemy.Common.Middlewares;

public class TransactionMiddleware<TContext>(TContext dbContext) : IMiddleware
    where TContext : DbContext
{
    private readonly TContext _dbContext = dbContext;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Method == HttpMethods.Post ||
            context.Request.Method == HttpMethods.Put ||
            context.Request.Method == HttpMethods.Delete)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await next(context);
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        else
        {
            await next(context);
        }
    }
}