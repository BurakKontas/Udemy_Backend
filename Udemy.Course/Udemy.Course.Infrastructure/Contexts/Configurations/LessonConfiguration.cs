using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Infrastructure.Contexts.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.Description)
            .HasMaxLength(1000);

        builder.Property(l => l.VideoUrl)
            .HasMaxLength(500);

        builder.HasMany(l => l.Attachments)
            .WithOne(a => a.Lesson)
            .HasForeignKey(a => a.LessonId);

        builder.HasOne(l => l.LessonCategory)
            .WithMany(lc => lc.Lessons)
            .HasForeignKey(l => l.LessonCategoryId);

        builder.HasOne(l => l.Course)
            .WithMany(c => c.Lessons)
            .HasForeignKey(l => l.CourseId);

        builder.HasMany(l => l.Questions)
            .WithOne(q => q.Lesson)
            .HasForeignKey(q => q.LessonId);
    }
}