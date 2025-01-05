using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Udemy.Payment.Domain.Entities;

namespace Udemy.Payment.Infrastructure.Contexts.Configurations;

public class UserDataEntityConfiguration : IEntityTypeConfiguration<UserDataEntity>
{
    public void Configure(EntityTypeBuilder<UserDataEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Surname).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
        builder.Property(x => x.RegistrationDate).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.GsmNumber).HasMaxLength(15);
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.Country).HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(255);
        builder.Property(x => x.ZipCode).HasMaxLength(20);
    }
}