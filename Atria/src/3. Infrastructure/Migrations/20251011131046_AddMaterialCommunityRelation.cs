using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialCommunityRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComunidadeIdComunidade",
                table: "TB_MATERIAL",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FkComunidade",
                table: "TB_MATERIAL",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvitedBy",
                table: "ComunidadeMembros",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsPending",
                table: "ComunidadeMembros",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TB_MATERIAL_ComunidadeIdComunidade",
                table: "TB_MATERIAL",
                column: "ComunidadeIdComunidade");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_MATERIAL_TB_COMUNIDADE_ComunidadeIdComunidade",
                table: "TB_MATERIAL",
                column: "ComunidadeIdComunidade",
                principalTable: "TB_COMUNIDADE",
                principalColumn: "ID_COMUNIDADE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_MATERIAL_TB_COMUNIDADE_ComunidadeIdComunidade",
                table: "TB_MATERIAL");

            migrationBuilder.DropIndex(
                name: "IX_TB_MATERIAL_ComunidadeIdComunidade",
                table: "TB_MATERIAL");

            migrationBuilder.DropColumn(
                name: "ComunidadeIdComunidade",
                table: "TB_MATERIAL");

            migrationBuilder.DropColumn(
                name: "FkComunidade",
                table: "TB_MATERIAL");

            migrationBuilder.DropColumn(
                name: "InvitedBy",
                table: "ComunidadeMembros");

            migrationBuilder.DropColumn(
                name: "IsPending",
                table: "ComunidadeMembros");
        }
    }
}
