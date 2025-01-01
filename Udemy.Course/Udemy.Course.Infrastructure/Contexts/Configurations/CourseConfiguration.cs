using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Infrastructure.Contexts.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Domain.Entities.Course>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Course> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Level)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.Language)
            .HasDefaultValue("English")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.CertificateTemplateUrl)
            .HasMaxLength(500);

        #region Requireds

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(c => c.IsApproved)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(c => c.IsFeatured)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(c => c.HasCertificate)
            .HasDefaultValue(false)
            .IsRequired();

        #endregion

        #region Relationships

        builder.HasMany(c => c.AuditLogs)
            .WithOne()
            .HasForeignKey(a => a.CourseId);

        builder.HasMany(c => c.Comments)
            .WithOne()
            .HasForeignKey(r => r.CourseId);

        builder.HasMany(c => c.Favorites)
            .WithOne()
            .HasForeignKey(f => f.CourseId);

        builder.HasMany(c => c.Tags)
            .WithOne()
            .HasForeignKey(t => t.CourseId);

        builder.HasMany(c => c.LessonCategories)
            .WithOne()
            .HasForeignKey(lc => lc.CourseId);

        builder.HasMany(c => c.Lessons)
            .WithOne()
            .HasForeignKey(l => l.CourseId);

        builder.HasMany(c => c.Enrollments)
            .WithOne()
            .HasForeignKey(e => e.CourseId);

        builder.HasOne(c => c.CourseDetails)
            .WithOne(cd => cd.Course)
            .HasForeignKey<CourseDetails>(cd => cd.CourseId);

        #endregion
    }
}