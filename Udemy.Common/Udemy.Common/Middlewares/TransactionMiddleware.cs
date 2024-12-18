using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Udemy.Common.Middlewares;

public class TransactionMiddleware<TContext>(RequestDelegate next, ILogger<TransactionMiddleware<TContext>> logger, TContext dbContext) where TContext : DbContext
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            await next(httpContext);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError($"Transaction rolled back due to error: {ex.Message}");
            throw;
        }
    }
}
