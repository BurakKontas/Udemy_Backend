using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Infrastructure.Contexts.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.Type)
            .HasMaxLength(50);

        builder.Property(a => a.Size)
            .IsRequired();
    }
}