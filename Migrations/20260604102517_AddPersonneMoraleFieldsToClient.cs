using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GesCPSI_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonneMoraleFieldsToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CapitalSocial",
                table: "ClientModels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePVDelib",
                table: "ClientModels",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FonctionRepresentant",
                table: "ClientModels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormeJuridique",
                table: "ClientModels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumIdentifFiscal",
                table: "ClientModels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefPVDelib",
                table: "ClientModels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistreCommerce",
                table: "ClientModels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiegeSocial",
                table: "ClientModels",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapitalSocial",
                table: "ClientModels");

            migrationBuilder.DropColumn(
                name: "DatePVDelib",
                table: "ClientModels");

            migrationBuilder.DropColumn(
                name: "FonctionRepresentant",
                table: "ClientModels");

            migrationBuilder.DropColumn(
                name: "FormeJuridique",
                table: "ClientModels");

            migrationBuilder.DropColumn(
                name: "NumIdentifFiscal",
                table: "ClientModels");

            migrationBuilder.DropColumn(
                name: "RefPVDelib",
                table: "ClientModels");

            migrationBuilder.DropColumn(
                name: "RegistreCommerce",
                table: "ClientModels");

            migrationBuilder.DropColumn(
                name: "SiegeSocial",
                table: "ClientModels");
        }
    }
}
