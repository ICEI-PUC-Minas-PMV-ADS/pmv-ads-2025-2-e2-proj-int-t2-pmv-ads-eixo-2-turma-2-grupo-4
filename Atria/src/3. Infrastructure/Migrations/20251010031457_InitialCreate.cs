using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<string>(type: "varchar(255)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NOME = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EMAIL = table.Column<string>(type: "varchar(255)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SENHA_HASH = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TIPO_USUARIO = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATA_CADASTRO = table.Column<DateTime>(type: "datetime", nullable: false),
                    MATRICULA = table.Column<string>(type: "varchar(255)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AREA_ATUACAO = table.Column<string>(type: "varchar(255)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.ID_USUARIO);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_COMUNIDADE",
                columns: table => new
                {
                    ID_COMUNIDADE = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOME = table.Column<string>(type: "varchar(255)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESCRICAO = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    IS_FORUM_GERAL = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    FK_CRIADOR = table.Column<string>(type: "varchar(255)", maxLength: 36, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_COMUNIDADE", x => x.ID_COMUNIDADE);
                    table.ForeignKey(
                        name: "FK_TB_COMUNIDADE_TB_USUARIO_FK_CRIADOR",
                        column: x => x.FK_CRIADOR,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_LISTA_LEITURA",
                columns: table => new
                {
                    ID_LISTA_LEITURA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOME = table.Column<string>(type: "varchar(255)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FK_CRIADOR = table.Column<string>(type: "varchar(255)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LISTA_LEITURA", x => x.ID_LISTA_LEITURA);
                    table.ForeignKey(
                        name: "FK_TB_LISTA_LEITURA_TB_USUARIO_FK_CRIADOR",
                        column: x => x.FK_CRIADOR,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_MATERIAL",
                columns: table => new
                {
                    ID_MATERIAL = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TITULO = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AUTOR = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ANO_PUBLICACAO = table.Column<int>(type: "int", nullable: false),
                    STATUS = table.Column<string>(type: "varchar(255)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TIPO_MATERIAL = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FK_PROFESSOR_CADASTRO = table.Column<string>(type: "varchar(255)", maxLength: 36, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DOI = table.Column<string>(type: "varchar(255)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ISBN = table.Column<string>(type: "varchar(255)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EDITORA = table.Column<string>(type: "varchar(255)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MATERIAL", x => x.ID_MATERIAL);
                    table.ForeignKey(
                        name: "FK_TB_MATERIAL_TB_USUARIO_FK_PROFESSOR_CADASTRO",
                        column: x => x.FK_PROFESSOR_CADASTRO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_NOTIFICACAO",
                columns: table => new
                {
                    ID_NOTIFICACAO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CONTEUDO = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    FK_USUARIO = table.Column<string>(type: "varchar(255)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_NOTIFICACAO", x => x.ID_NOTIFICACAO);
                    table.ForeignKey(
                        name: "FK_TB_NOTIFICACAO_TB_USUARIO_FK_USUARIO",
                        column: x => x.FK_USUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_GRUPO_ESTUDO",
                columns: table => new
                {
                    ID_GRUPO_ESTUDO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOME = table.Column<string>(type: "varchar(255)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    FK_COMUNIDADE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GRUPO_ESTUDO", x => x.ID_GRUPO_ESTUDO);
                    table.ForeignKey(
                        name: "FK_TB_GRUPO_ESTUDO_TB_COMUNIDADE_FK_COMUNIDADE",
                        column: x => x.FK_COMUNIDADE,
                        principalTable: "TB_COMUNIDADE",
                        principalColumn: "ID_COMUNIDADE",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_POSTAGEM",
                columns: table => new
                {
                    ID_POSTAGEM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TITULO = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CONTEUDO = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    NO_FORUM_GERAL = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FK_AUTOR = table.Column<string>(type: "varchar(255)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FK_COMUNIDADE = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_POSTAGEM", x => x.ID_POSTAGEM);
                    table.CheckConstraint("CHK_POSTAGEM_LOCAL", "(FK_COMUNIDADE IS NULL AND NO_FORUM_GERAL = 1) OR (FK_COMUNIDADE IS NOT NULL AND NO_FORUM_GERAL = 0)");
                    table.ForeignKey(
                        name: "FK_TB_POSTAGEM_TB_COMUNIDADE_FK_COMUNIDADE",
                        column: x => x.FK_COMUNIDADE,
                        principalTable: "TB_COMUNIDADE",
                        principalColumn: "ID_COMUNIDADE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_POSTAGEM_TB_USUARIO_FK_AUTOR",
                        column: x => x.FK_AUTOR,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_AVALIACAO",
                columns: table => new
                {
                    ID_AVALIACAO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOTA = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: false),
                    RESENHA = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATA_AVALIACAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    TIPO_AVALIACAO = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TEXTO_ESPECIALISTA = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FK_AUTOR = table.Column<string>(type: "varchar(255)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FK_MATERIAL = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AVALIACAO", x => x.ID_AVALIACAO);
                    table.ForeignKey(
                        name: "FK_TB_AVALIACAO_TB_MATERIAL_FK_MATERIAL",
                        column: x => x.FK_MATERIAL,
                        principalTable: "TB_MATERIAL",
                        principalColumn: "ID_MATERIAL",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_AVALIACAO_TB_USUARIO_FK_AUTOR",
                        column: x => x.FK_AUTOR,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_LISTA_TEM_MATERIAL",
                columns: table => new
                {
                    FK_LISTA_LEITURA = table.Column<int>(type: "int", nullable: false),
                    FK_MATERIAL = table.Column<int>(type: "int", nullable: false),
                    DATA_ADICAO = table.Column<DateTime>(type: "datetime", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LISTA_TEM_MATERIAL", x => new { x.FK_LISTA_LEITURA, x.FK_MATERIAL });
                    table.ForeignKey(
                        name: "FK_TB_LISTA_TEM_MATERIAL_TB_LISTA_LEITURA_FK_LISTA_LEITURA",
                        column: x => x.FK_LISTA_LEITURA,
                        principalTable: "TB_LISTA_LEITURA",
                        principalColumn: "ID_LISTA_LEITURA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_LISTA_TEM_MATERIAL_TB_MATERIAL_FK_MATERIAL",
                        column: x => x.FK_MATERIAL,
                        principalTable: "TB_MATERIAL",
                        principalColumn: "ID_MATERIAL",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_COMENTARIO",
                columns: table => new
                {
                    ID_COMENTARIO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CONTEUDO = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime", nullable: false),
                    FK_AUTOR = table.Column<string>(type: "varchar(255)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FK_POSTAGEM = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_COMENTARIO", x => x.ID_COMENTARIO);
                    table.ForeignKey(
                        name: "FK_TB_COMENTARIO_TB_POSTAGEM_FK_POSTAGEM",
                        column: x => x.FK_POSTAGEM,
                        principalTable: "TB_POSTAGEM",
                        principalColumn: "ID_POSTAGEM",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_COMENTARIO_TB_USUARIO_FK_AUTOR",
                        column: x => x.FK_AUTOR,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TB_AVALIACAO_FK_AUTOR_FK_MATERIAL",
                table: "TB_AVALIACAO",
                columns: new[] { "FK_AUTOR", "FK_MATERIAL" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_AVALIACAO_FK_MATERIAL",
                table: "TB_AVALIACAO",
                column: "FK_MATERIAL");

            migrationBuilder.CreateIndex(
                name: "IX_TB_COMENTARIO_FK_AUTOR",
                table: "TB_COMENTARIO",
                column: "FK_AUTOR");

            migrationBuilder.CreateIndex(
                name: "IX_TB_COMENTARIO_FK_POSTAGEM",
                table: "TB_COMENTARIO",
                column: "FK_POSTAGEM");

            migrationBuilder.CreateIndex(
                name: "IX_TB_COMUNIDADE_FK_CRIADOR",
                table: "TB_COMUNIDADE",
                column: "FK_CRIADOR");

            migrationBuilder.CreateIndex(
                name: "IX_TB_GRUPO_ESTUDO_FK_COMUNIDADE",
                table: "TB_GRUPO_ESTUDO",
                column: "FK_COMUNIDADE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LISTA_LEITURA_FK_CRIADOR",
                table: "TB_LISTA_LEITURA",
                column: "FK_CRIADOR");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LISTA_TEM_MATERIAL_FK_MATERIAL",
                table: "TB_LISTA_TEM_MATERIAL",
                column: "FK_MATERIAL");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MATERIAL_FK_PROFESSOR_CADASTRO",
                table: "TB_MATERIAL",
                column: "FK_PROFESSOR_CADASTRO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_NOTIFICACAO_FK_USUARIO",
                table: "TB_NOTIFICACAO",
                column: "FK_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_POSTAGEM_FK_AUTOR",
                table: "TB_POSTAGEM",
                column: "FK_AUTOR");

            migrationBuilder.CreateIndex(
                name: "IX_TB_POSTAGEM_FK_COMUNIDADE",
                table: "TB_POSTAGEM",
                column: "FK_COMUNIDADE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_EMAIL",
                table: "TB_USUARIO",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_MATRICULA",
                table: "TB_USUARIO",
                column: "MATRICULA",
                unique: true,
                filter: "MATRICULA IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_AVALIACAO");

            migrationBuilder.DropTable(
                name: "TB_COMENTARIO");

            migrationBuilder.DropTable(
                name: "TB_GRUPO_ESTUDO");

            migrationBuilder.DropTable(
                name: "TB_LISTA_TEM_MATERIAL");

            migrationBuilder.DropTable(
                name: "TB_NOTIFICACAO");

            migrationBuilder.DropTable(
                name: "TB_POSTAGEM");

            migrationBuilder.DropTable(
                name: "TB_LISTA_LEITURA");

            migrationBuilder.DropTable(
                name: "TB_MATERIAL");

            migrationBuilder.DropTable(
                name: "TB_COMUNIDADE");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");
        }
    }
}
