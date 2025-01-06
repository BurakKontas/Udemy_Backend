using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Udemy.Payment.Domain.Entities;

namespace Udemy.Payment.Infrastructure.Contexts.Configurations;

public class BasketEntityConfiguration : IEntityTypeConfiguration<BasketEntity>
{
    public void Configure(EntityTypeBuilder<BasketEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.BasketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}