using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GesCPSI_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplateTypeToAjoutAct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TemplateType",
                table: "AjoutActModels",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateType",
                table: "AjoutActModels");
        }
    }
}
