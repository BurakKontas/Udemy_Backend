using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Udemy.Payment.Domain.Entities;

namespace Udemy.Payment.Infrastructure.Contexts.Configurations;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItemEntity>
{
    public void Configure(EntityTypeBuilder<BasketItemEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(255).IsRequired();
        builder.Property(x => x.ItemType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.BasketId).HasMaxLength(50).IsRequired();
    }
}