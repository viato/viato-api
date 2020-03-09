using Microsoft.EntityFrameworkCore.Migrations;

namespace Viato.Api.Migrations
{
    public partial class AddOrganizationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDnsVerified",
                table: "Organizations");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Organizations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Organizations");

            migrationBuilder.AddColumn<bool>(
                name: "IsDnsVerified",
                table: "Organizations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
