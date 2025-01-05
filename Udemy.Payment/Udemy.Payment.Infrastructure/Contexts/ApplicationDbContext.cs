using Microsoft.EntityFrameworkCore;
using Udemy.Payment.Domain.Entities;

namespace Udemy.Payment.Infrastructure.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<CardEntity> Cards { get; set; }
    public DbSet<BasketEntity> Baskets { get; set; }
    public DbSet<UserDataEntity> UserDatas { get; set; }
    public DbSet<PaymentEntity> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(builder);
    }
}