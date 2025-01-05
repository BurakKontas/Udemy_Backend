using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Course.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CourseEnrollmentIsPaid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Enrollments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Courses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Courses");
        }
    }
}
