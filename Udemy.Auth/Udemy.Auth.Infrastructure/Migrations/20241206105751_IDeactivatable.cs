using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IDeactivatable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactivated",
                schema: "identity",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactivated",
                schema: "identity",
                table: "AspNetUsers");
        }
    }
}
