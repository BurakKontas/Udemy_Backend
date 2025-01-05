using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Udemy.Payment.Domain.Entities;

namespace Udemy.Payment.Infrastructure.Contexts.Configurations;

public class CardEntityConfiguration : IEntityTypeConfiguration<CardEntity>
{
    public void Configure(EntityTypeBuilder<CardEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CardUserKey).HasMaxLength(200).IsRequired();
        builder.Property(x => x.CardToken).HasMaxLength(200).IsRequired(false);  // CardToken opsiyonel hale getirildi
        builder.Property(x => x.BinNumber).HasMaxLength(6).IsRequired();
        builder.Property(x => x.LastFourDigits).HasMaxLength(4).IsRequired();
        builder.Property(x => x.CardType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.CardAssociation).HasMaxLength(50).IsRequired();
        builder.Property(x => x.CardFamily).HasMaxLength(50).IsRequired();
        builder.Property(x => x.CardAlias).HasMaxLength(100).IsRequired(false); // CardAlias opsiyonel hale getirildi
        builder.Property(x => x.CardBankCode).HasColumnType("bigint").IsRequired(false); // CardBankCode opsiyonel
        builder.Property(x => x.CardBankName).HasMaxLength(100).IsRequired(false);  // CardBankName opsiyonel

        builder.HasMany(x => x.Payments)
            .WithOne(x => x.Card)
            .HasForeignKey(x => x.CardId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}