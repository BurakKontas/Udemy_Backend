using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Udemy.Common.Helpers;

public static class MigrationExtensions
{
    public static void ApplyMigrations<TDbContext>(this IApplicationBuilder app) where TDbContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        context.Database.Migrate();
    }
}