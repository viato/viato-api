using Microsoft.EntityFrameworkCore.Migrations;

namespace Viato.Api.Migrations
{
    public partial class AddDomainColumnForOrg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Domain",
                table: "Organizations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Domain",
                table: "Organizations",
                column: "Domain",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organizations_Domain",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Domain",
                table: "Organizations");
        }
    }
}
