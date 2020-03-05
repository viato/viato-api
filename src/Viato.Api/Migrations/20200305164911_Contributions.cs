using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Viato.Api.Migrations
{
    public partial class Contributions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DnsVerified",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DomainName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContributionPipelines",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<int>(nullable: false),
                    Types = table.Column<int>(nullable: false),
                    SourceOrgId = table.Column<long>(nullable: false),
                    DestinationOrgId = table.Column<long>(nullable: false),
                    ContributionCurrency = table.Column<string>(nullable: true),
                    CollectedAmount = table.Column<decimal>(nullable: false),
                    AmountLimit = table.Column<decimal>(nullable: false),
                    DateLimit = table.Column<DateTimeOffset>(nullable: true),
                    OwnerPublicKey = table.Column<string>(nullable: true),
                    OwnerPrivateKey = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributionPipelines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContributionPipelines_AspNetUsers_DestinationOrgId",
                        column: x => x.DestinationOrgId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContributionPipelines_AspNetUsers_SourceOrgId",
                        column: x => x.SourceOrgId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContributionProof",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<int>(nullable: false),
                    Network = table.Column<string>(nullable: true),
                    BlockchainTransactionId = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributionProof", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contributions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContributionPipelineId = table.Column<long>(nullable: false),
                    ContributorId = table.Column<long>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    TorTokenId = table.Column<string>(nullable: true),
                    TorToken = table.Column<string>(nullable: true),
                    ContributionProofId = table.Column<long>(nullable: true),
                    IsPrivate = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contributions_ContributionPipelines_ContributionPipelineId",
                        column: x => x.ContributionPipelineId,
                        principalTable: "ContributionPipelines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contributions_ContributionProof_ContributionProofId",
                        column: x => x.ContributionProofId,
                        principalTable: "ContributionProof",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contributions_AspNetUsers_ContributorId",
                        column: x => x.ContributorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContributionPipelines_DestinationOrgId",
                table: "ContributionPipelines",
                column: "DestinationOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_ContributionPipelines_SourceOrgId",
                table: "ContributionPipelines",
                column: "SourceOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_ContributionPipelineId",
                table: "Contributions",
                column: "ContributionPipelineId");

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_ContributionProofId",
                table: "Contributions",
                column: "ContributionProofId");

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_ContributorId",
                table: "Contributions",
                column: "ContributorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contributions");

            migrationBuilder.DropTable(
                name: "ContributionPipelines");

            migrationBuilder.DropTable(
                name: "ContributionProof");

            migrationBuilder.DropColumn(
                name: "DnsVerified",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DomainName",
                table: "AspNetUsers");
        }
    }
}
