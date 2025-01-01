using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Infrastructure.Contexts.Configurations;

public class CourseDetailsConfiguration : IEntityTypeConfiguration<CourseDetails>
{
    public void Configure(EntityTypeBuilder<CourseDetails> builder)
    {
        builder.HasKey(cd => cd.Id);

        builder.Property(cd => cd.Description)
            .IsRequired()
            .HasMaxLength(500);
    }
}