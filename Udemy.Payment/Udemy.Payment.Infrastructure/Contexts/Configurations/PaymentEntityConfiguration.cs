using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Udemy.Payment.Domain.Entities;

namespace Udemy.Payment.Infrastructure.Contexts.Configurations;

public class PaymentEntityConfiguration : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.PaymentDate).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.IpAddress).HasMaxLength(50);

        builder.HasOne(x => x.UserData)
            .WithMany()
            .HasForeignKey(x => x.UserDataId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Basket)
            .WithMany()
            .HasForeignKey(x => x.BasketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Card)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.CardId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}