using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Course.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRateToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RateCount",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "RateValue",
                table: "Courses",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RateCount",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "RateValue",
                table: "Courses");
        }
    }
}
