﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Udemy.Course.Domain.Entities;

namespace Udemy.Course.Infrastructure.Contexts.Configurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Value)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasMany(a => a.Likes)
            .WithOne()
            .HasForeignKey(l => l.AnswerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}