using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Migrations;
using MudBlazor.Charts;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GesCPSI_Project.Migrations
{
    /// <inheritdoc />
    public partial class RestorePersonneMoraleFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 🔧 ÉTAPE 1 : Nettoyage tolérant — UserModel peut ne pas exister
            migrationBuilder.Sql(@"
        ALTER TABLE ""TypesActModels"" 
        DROP CONSTRAINT IF EXISTS ""FK_TypesActModels_UserModel_IdUser"";
    ");

            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_UserModel_NameUser"";");

            // 🔧 ÉTAPE 2 : Supprime l'ancienne table UserModel si elle existe
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS ""UserModel"" CASCADE;");

            // 🔧 ÉTAPE 3 : Crée la table Users from scratch (Identity + champs métier)
            migrationBuilder.Sql(@"
        CREATE TABLE IF NOT EXISTS ""Users"" (
            ""Id"" SERIAL PRIMARY KEY,
            ""KeycloakId"" text NULL,
            ""Fonction"" text NULL,
            ""LockoutEnd"" timestamp with time zone NULL,
            ""DateCreation"" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
            ""IsActive"" boolean NOT NULL DEFAULT true
        );
    ");

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

            migrationBuilder.AlterColumn<string>(
                name: "KeycloakId",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Fonction",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DerniereConnexion",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MustChangePassword",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NomComplet",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "character varying(256)",
            maxLength: 256,
                nullable: true);

           // 🔧 Renomme la PK auto-générée par le CREATE TABLE en "PK_Users"
//    pour qu'EF Core la retrouve sous ce nom (utilisé dans le snapshot).
migrationBuilder.Sql(@"
    DO $$
    DECLARE
        pk_name text;
    BEGIN
        -- Récupère le nom actuel de la PK sur la table Users
        SELECT conname INTO pk_name
        FROM pg_constraint
        WHERE conrelid = '""Users""'::regclass
          AND contype = 'p';

        -- Si elle existe et n'est pas déjà nommée 'PK_Users', on la renomme
        IF pk_name IS NOT NULL AND pk_name <> 'PK_Users' THEN
            EXECUTE format('ALTER TABLE ""Users"" RENAME CONSTRAINT %I TO ""PK_Users""', pk_name);
        END IF;
    END $$;
");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            // 🔧 ÉTAPE 1 : Rendre IdUser nullable AVANT de mettre des NULL et créer la FK
            migrationBuilder.AlterColumn<int>(
                name: "IdUser",
                table: "TypesActModels",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            // 🔧 ÉTAPE 2 : Nettoyer les IdUser orphelins (les anciens UserModel n'existent plus)
            migrationBuilder.Sql(@"
    UPDATE ""TypesActModels"" 
    SET ""IdUser"" = NULL 
    WHERE ""IdUser"" IS NOT NULL;
");

            // 🔧 ÉTAPE 3 : Maintenant on peut créer la FK proprement
            migrationBuilder.AddForeignKey(
                name: "FK_TypesActModels_Users_IdUser",
                table: "TypesActModels",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypesActModels_Users_IdUser",
                table: "TypesActModels");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "Users");

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

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DerniereConnexion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MustChangePassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NomComplet",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "UserModel");

            migrationBuilder.RenameColumn(
                name: "LockoutEnd",
                table: "UserModel",
                newName: "DateModification");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserModel",
                newName: "IdUser");

            migrationBuilder.AlterColumn<string>(
                name: "KeycloakId",
                table: "UserModel",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Fonction",
                table: "UserModel",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Departement",
                table: "UserModel",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmailUser",
                table: "UserModel",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameUser",
                table: "UserModel",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SurnameUser",
                table: "UserModel",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModel",
                table: "UserModel",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_UserModel_NameUser",
                table: "UserModel",
                column: "NameUser");

            migrationBuilder.AddForeignKey(
                name: "FK_TypesActModels_UserModel_IdUser",
                table: "TypesActModels",
                column: "IdUser",
                principalTable: "UserModel",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
