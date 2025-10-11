using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddComunidadeMembro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComunidadeMembros",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ComunidadeId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsModAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComunidadeMembros", x => new { x.ComunidadeId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_ComunidadeMembros_TB_COMUNIDADE_ComunidadeId",
                        column: x => x.ComunidadeId,
                        principalTable: "TB_COMUNIDADE",
                        principalColumn: "ID_COMUNIDADE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComunidadeMembros_TB_USUARIO_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ComunidadeMembros_UsuarioId",
                table: "ComunidadeMembros",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComunidadeMembros");
        }
    }
}
