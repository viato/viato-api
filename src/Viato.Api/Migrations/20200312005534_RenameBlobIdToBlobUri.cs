using Microsoft.EntityFrameworkCore.Migrations;

namespace Viato.Api.Migrations
{
    public partial class RenameBlobIdToBlobUri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoBlobId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ImageBlobId",
                table: "ContributionPipelines");

            migrationBuilder.AddColumn<string>(
                name: "LogoBlobUri",
                table: "Organizations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoBlobUri",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "LogoBlobId",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageBlobId",
                table: "ContributionPipelines",
                type: "text",
                nullable: true);
        }
    }
}
