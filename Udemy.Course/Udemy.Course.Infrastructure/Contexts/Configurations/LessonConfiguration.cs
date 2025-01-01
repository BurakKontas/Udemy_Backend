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
            .WithOne()
            .HasForeignKey(a => a.LessonId);

        builder.HasMany(l => l.Questions)
            .WithOne()
            .HasForeignKey(q => q.LessonId);
    }
}