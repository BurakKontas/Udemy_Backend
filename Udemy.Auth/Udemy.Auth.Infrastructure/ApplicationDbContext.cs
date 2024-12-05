using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Udemy.Auth.Domain;

namespace Udemy.Auth.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User, Role, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
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