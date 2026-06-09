using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GesCPSI_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddActeWorkflowFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateArchivage",
                table: "TypesActModels",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEnvoiValidation",
                table: "TypesActModels",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatutWorkflow",
                table: "TypesActModels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdActe = table.Column<int>(type: "integer", nullable: false),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "text", nullable: true),
                    DateAction = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdUser = table.Column<int>(type: "integer", nullable: true),
                    StatutAvant = table.Column<int>(type: "integer", nullable: true),
                    StatutApres = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_TypesActModels_IdActe",
                        column: x => x.IdActe,
                        principalTable: "TypesActModels",
                        principalColumn: "IdActe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_IdActe",
                table: "AuditLogs",
                column: "IdActe");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_IdUser",
                table: "AuditLogs",
                column: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "DateArchivage",
                table: "TypesActModels");

            migrationBuilder.DropColumn(
                name: "DateEnvoiValidation",
                table: "TypesActModels");

            migrationBuilder.DropColumn(
                name: "StatutWorkflow",
                table: "TypesActModels");
        }
    }
}
