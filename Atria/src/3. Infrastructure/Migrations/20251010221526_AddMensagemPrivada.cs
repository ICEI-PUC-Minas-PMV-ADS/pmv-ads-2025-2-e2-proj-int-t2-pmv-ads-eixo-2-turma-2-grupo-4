using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMensagemPrivada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_MENSAGEM_PRIVADA",
                columns: table => new
                {
                    ID_MENSAGEM = table.Column<string>(type: "varchar(255)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CONTEUDO = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATA_ENVIO = table.Column<DateTime>(type: "datetime", nullable: false),
                    LIDA = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FK_REMETENTE = table.Column<string>(type: "varchar(255)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FK_DESTINATARIO = table.Column<string>(type: "varchar(255)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MENSAGEM_PRIVADA", x => x.ID_MENSAGEM);
                    table.ForeignKey(
                        name: "FK_TB_MENSAGEM_PRIVADA_TB_USUARIO_FK_DESTINATARIO",
                        column: x => x.FK_DESTINATARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_MENSAGEM_PRIVADA_TB_USUARIO_FK_REMETENTE",
                        column: x => x.FK_REMETENTE,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MENSAGEM_PRIVADA_FK_DESTINATARIO",
                table: "TB_MENSAGEM_PRIVADA",
                column: "FK_DESTINATARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MENSAGEM_PRIVADA_FK_REMETENTE",
                table: "TB_MENSAGEM_PRIVADA",
                column: "FK_REMETENTE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_MENSAGEM_PRIVADA");
        }
    }
}
