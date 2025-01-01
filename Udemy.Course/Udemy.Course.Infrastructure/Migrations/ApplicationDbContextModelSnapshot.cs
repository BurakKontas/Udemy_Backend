﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Udemy.Course.Infrastructure.Contexts;

#nullable disable

namespace Udemy.Course.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("LessonId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.AuditLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CourseId1")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("CourseId1");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ApprovedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CertificateTemplateUrl")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("HasCertificate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<List<Guid>>("InstructorIds")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<bool>("IsApproved")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsFeatured")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Language")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasDefaultValue("English");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.CourseDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTimeOffset?>("DiscountEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DiscountStartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("DiscountedPrice")
                        .HasColumnType("numeric");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("PreviewVideoUrl")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("RateCount")
                        .HasColumnType("integer");

                    b.Property<decimal>("RateValue")
                        .HasColumnType("numeric");

                    b.Property<TimeSpan>("TotalDuration")
                        .HasColumnType("interval");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ViewCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CourseId")
                        .IsUnique();

                    b.ToTable("CourseDetails");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Enrollment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CompletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<List<Guid>>("CompletedLessons")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("EnrolledAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId", "CourseId")
                        .IsUnique();

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Favorite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("AddedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Favorite");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Lesson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<Guid>("LessonCategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("VideoUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("LessonCategoryId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.LessonCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("LessonCategories");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Like", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Like");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AnswerCount")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("LessonId")
                        .HasColumnType("uuid");

                    b.Property<int>("LikeCount")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Rate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Rate");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Answer", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Attachment", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Lesson", "Lesson")
                        .WithMany("Attachments")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.AuditLog", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Course", null)
                        .WithMany("AuditLogs")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Udemy.Course.Domain.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId1");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Comment", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Course", null)
                        .WithMany("Comments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.CourseDetails", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Course", "Course")
                        .WithOne("CourseDetails")
                        .HasForeignKey("Udemy.Course.Domain.Entities.CourseDetails", "CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Enrollment", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Favorite", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Course", "Course")
                        .WithMany("Favorites")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Lesson", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Course", "Course")
                        .WithMany("Lessons")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Udemy.Course.Domain.Entities.LessonCategory", "LessonCategory")
                        .WithMany("Lessons")
                        .HasForeignKey("LessonCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("LessonCategory");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.LessonCategory", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Course", "Course")
                        .WithMany("LessonCategories")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Like", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Question", "Question")
                        .WithMany("Likes")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Question", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Lesson", "Lesson")
                        .WithMany("Questions")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Rate", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Comment", null)
                        .WithOne("Rate")
                        .HasForeignKey("Udemy.Course.Domain.Entities.Rate", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Tag", b =>
                {
                    b.HasOne("Udemy.Course.Domain.Entities.Course", "Course")
                        .WithMany("Tags")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Comment", b =>
                {
                    b.Navigation("Rate");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Course", b =>
                {
                    b.Navigation("AuditLogs");

                    b.Navigation("Comments");

                    b.Navigation("CourseDetails");

                    b.Navigation("Enrollments");

                    b.Navigation("Favorites");

                    b.Navigation("LessonCategories");

                    b.Navigation("Lessons");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Lesson", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.LessonCategory", b =>
                {
                    b.Navigation("Lessons");
                });

            modelBuilder.Entity("Udemy.Course.Domain.Entities.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Likes");
                });
#pragma warning restore 612, 618
        }
    }
}
