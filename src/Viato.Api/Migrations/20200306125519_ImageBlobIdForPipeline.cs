using Microsoft.EntityFrameworkCore.Migrations;

namespace Viato.Api.Migrations
{
    public partial class ImageBlobIdForPipeline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageBlobId",
                table: "ContributionPipelines",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBlobId",
                table: "ContributionPipelines");
        }
    }
}
