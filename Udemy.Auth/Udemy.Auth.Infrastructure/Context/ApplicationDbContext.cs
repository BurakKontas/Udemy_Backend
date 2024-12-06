using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Udemy.Auth.Domain;

namespace Udemy.Auth.Infrastructure.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<Domain.User, Role, string>(options), IDataProtectionKeyContext
{
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Domain.User>(entity =>
        {
            entity.Property(e => e.Initials)
                .HasMaxLength(5);
        });

        builder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.HasDefaultSchema("identity");
    }
}