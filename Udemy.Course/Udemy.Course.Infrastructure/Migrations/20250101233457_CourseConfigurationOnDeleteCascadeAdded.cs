using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Course.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CourseConfigurationOnDeleteCascadeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Courses_CourseId1",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_CourseId1",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "AuditLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseId1",
                table: "AuditLogs",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CourseId1",
                table: "AuditLogs",
                column: "CourseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Courses_CourseId1",
                table: "AuditLogs",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
