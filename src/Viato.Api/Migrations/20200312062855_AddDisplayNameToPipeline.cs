using Microsoft.EntityFrameworkCore.Migrations;

namespace Viato.Api.Migrations
{
    public partial class AddDisplayNameToPipeline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ContributionPipelines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "ContributionPipelines",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ContributionPipelines");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "ContributionPipelines");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Organizations",
                type: "text",
                nullable: true);
        }
    }
}
