using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atria.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCamposPerfilUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AREA_ESTUDO",
                table: "TB_USUARIO",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PROJETOS",
                table: "TB_USUARIO",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TRILHA_CONHECIMENTO",
                table: "TB_USUARIO",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AREA_ESTUDO",
                table: "TB_USUARIO");

            migrationBuilder.DropColumn(
                name: "PROJETOS",
                table: "TB_USUARIO");

            migrationBuilder.DropColumn(
                name: "TRILHA_CONHECIMENTO",
                table: "TB_USUARIO");
        }
    }
}
