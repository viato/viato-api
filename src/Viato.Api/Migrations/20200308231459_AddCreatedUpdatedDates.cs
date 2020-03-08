using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Viato.Api.Migrations
{
    public partial class AddCreatedUpdatedDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Organizations",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "IsDnsVerified",
                table: "Organizations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Contributions",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Contributions",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "ContributionProof",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "ContributionProof",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "ContributionPipelines",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "ContributionPipelines",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "IsDnsVerified",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ContributionProof");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ContributionProof");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ContributionPipelines");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ContributionPipelines");
        }
    }
}
