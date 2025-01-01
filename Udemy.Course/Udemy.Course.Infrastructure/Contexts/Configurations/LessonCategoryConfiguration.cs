using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Infrastructure.Contexts.Configurations;

public class LessonCategoryConfiguration : IEntityTypeConfiguration<LessonCategory>
{
    public void Configure(EntityTypeBuilder<LessonCategory> builder)
    {
        builder.HasKey(lc => lc.Id);

        builder.Property(lc => lc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(lc => lc.Description)
            .HasMaxLength(500);

        builder.HasMany(lc => lc.Lessons)
            .WithOne(l => l.LessonCategory)
            .HasForeignKey(l => l.LessonCategoryId);

        builder.HasOne(lc => lc.Course)
            .WithMany(c => c.LessonCategories)
            .HasForeignKey(lc => lc.CourseId);
    }
}