using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Udemy.Course.Infrastructure.Contexts.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Domain.Entities.Course>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Course> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .HasMaxLength(2000);

        builder.Property(c => c.ImageUrl)
            .HasMaxLength(500);

        builder.Property(c => c.Price)
            .HasPrecision(18, 2);

        builder.Property(c => c.DiscountedPrice)
            .HasPrecision(18, 2);

        builder.HasMany(c => c.Tags)
            .WithOne()
            .HasForeignKey(t => t.Id);
    }
}