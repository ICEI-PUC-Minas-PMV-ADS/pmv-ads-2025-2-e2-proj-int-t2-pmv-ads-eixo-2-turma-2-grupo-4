using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atria.Data.Migrations
{
    /// <inheritdoc />
    public partial class DomainEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_COMUNIDADE",
                columns: table => new
                {
                    IDCOMUNIDADE = table.Column<int>(name: "ID_COMUNIDADE", type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESCRICAO = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATACRIACAO = table.Column<DateTime>(name: "DATA_CRIACAO", type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_COMUNIDADE", x => x.IDCOMUNIDADE);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_LISTA_LEITURA",
                columns: table => new
                {
                    IDLISTA = table.Column<int>(name: "ID_LISTA", type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESCRICAO = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FKUSUARIO = table.Column<int>(name: "FK_USUARIO", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LISTA_LEITURA", x => x.IDLISTA);
                    table.ForeignKey(
                        name: "FK_LISTA_USUARIO",
                        column: x => x.FKUSUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_MATERIAL",
                columns: table => new
                {
                    IDMATERIAL = table.Column<int>(name: "ID_MATERIAL", type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TITULO = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESCRICAO = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TIPO = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FKUSUARIOCRIADOR = table.Column<int>(name: "FK_USUARIO_CRIADOR", type: "int", nullable: false),
                    DATACRIACAO = table.Column<DateTime>(name: "DATA_CRIACAO", type: "datetime(6)", nullable: false),
                    STATUS = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MATERIAL", x => x.IDMATERIAL);
                    table.ForeignKey(
                        name: "FK_MATERIAL_USUARIO",
                        column: x => x.FKUSUARIOCRIADOR,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_GRUPO_ESTUDO",
                columns: table => new
                {
                    IDGRUPO = table.Column<int>(name: "ID_GRUPO", type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESCRICAO = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FKCOMUNIDADE = table.Column<int>(name: "FK_COMUNIDADE", type: "int", nullable: true),
                    DATACRIACAO = table.Column<DateTime>(name: "DATA_CRIACAO", type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_GRUPO_ESTUDO", x => x.IDGRUPO);
                    table.ForeignKey(
                        name: "FK_GRUPO_COMUNIDADE",
                        column: x => x.FKCOMUNIDADE,
                        principalTable: "TB_COMUNIDADE",
                        principalColumn: "ID_COMUNIDADE",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_POSTAGEM",
                columns: table => new
                {
                    IDPOSTAGEM = table.Column<int>(name: "ID_POSTAGEM", type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CONTEUDO = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATAPOSTAGEM = table.Column<DateTime>(name: "DATA_POSTAGEM", type: "datetime(6)", nullable: false),
                    NOFORUMGERAL = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FKUSUARIO = table.Column<int>(name: "FK_USUARIO", type: "int", nullable: false),
                    FKCOMUNIDADE = table.Column<int>(name: "FK_COMUNIDADE", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_POSTAGEM", x => x.IDPOSTAGEM);
                    table.ForeignKey(
                        name: "FK_POSTAGEM_COMUNIDADE",
                        column: x => x.FKCOMUNIDADE,
                        principalTable: "TB_COMUNIDADE",
                        principalColumn: "ID_COMUNIDADE",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_POSTAGEM_USUARIO",
                        column: x => x.FKUSUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_USUARIO_COMUNIDADE",
                columns: table => new
                {
                    FKUSUARIO = table.Column<int>(name: "FK_USUARIO", type: "int", nullable: false),
                    FKCOMUNIDADE = table.Column<int>(name: "FK_COMUNIDADE", type: "int", nullable: false),
                    DATAENTRADA = table.Column<DateTime>(name: "DATA_ENTRADA", type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO_COMUNIDADE", x => new { x.FKUSUARIO, x.FKCOMUNIDADE });
                    table.ForeignKey(
                        name: "FK_USUARIOCOMUNIDADE_COMUNIDADE",
                        column: x => x.FKCOMUNIDADE,
                        principalTable: "TB_COMUNIDADE",
                        principalColumn: "ID_COMUNIDADE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USUARIOCOMUNIDADE_USUARIO",
                        column: x => x.FKUSUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_AVALIACAO",
                columns: table => new
                {
                    IDAVALIACAO = table.Column<int>(name: "ID_AVALIACAO", type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOTA = table.Column<int>(type: "int", nullable: false),
                    TIPOAVALIACAO = table.Column<string>(name: "TIPO_AVALIACAO", type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FKUSUARIO = table.Column<int>(name: "FK_USUARIO", type: "int", nullable: false),
                    FKMATERIAL = table.Column<int>(name: "FK_MATERIAL", type: "int", nullable: false),
                    RESENHA = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AVALIACAO", x => x.IDAVALIACAO);
                    table.ForeignKey(
                        name: "FK_AVALIACAO_MATERIAL",
                        column: x => x.FKMATERIAL,
                        principalTable: "TB_MATERIAL",
                        principalColumn: "ID_MATERIAL",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AVALIACAO_USUARIO",
                        column: x => x.FKUSUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_LISTA_TEM_MATERIAL",
                columns: table => new
                {
                    FKLISTA = table.Column<int>(name: "FK_LISTA", type: "int", nullable: false),
                    FKMATERIAL = table.Column<int>(name: "FK_MATERIAL", type: "int", nullable: false),
                    ORDEM = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LISTA_TEM_MATERIAL", x => new { x.FKLISTA, x.FKMATERIAL });
                    table.ForeignKey(
                        name: "FK_LISTATEMMATERIAL_LISTA",
                        column: x => x.FKLISTA,
                        principalTable: "TB_LISTA_LEITURA",
                        principalColumn: "ID_LISTA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LISTATEMMATERIAL_MATERIAL",
                        column: x => x.FKMATERIAL,
                        principalTable: "TB_MATERIAL",
                        principalColumn: "ID_MATERIAL",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_USUARIO_GRUPO",
                columns: table => new
                {
                    FKUSUARIO = table.Column<int>(name: "FK_USUARIO", type: "int", nullable: false),
                    FKGRUPO = table.Column<int>(name: "FK_GRUPO", type: "int", nullable: false),
                    DATAENTRADA = table.Column<DateTime>(name: "DATA_ENTRADA", type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO_GRUPO", x => new { x.FKUSUARIO, x.FKGRUPO });
                    table.ForeignKey(
                        name: "FK_USUARIOGRUPO_GRUPO",
                        column: x => x.FKGRUPO,
                        principalTable: "TB_GRUPO_ESTUDO",
                        principalColumn: "ID_GRUPO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USUARIOGRUPO_USUARIO",
                        column: x => x.FKUSUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TB_AVALIACAO_FK_MATERIAL",
                table: "TB_AVALIACAO",
                column: "FK_MATERIAL");

            migrationBuilder.CreateIndex(
                name: "UX_AVALIACAO_USUARIO_MATERIAL",
                table: "TB_AVALIACAO",
                columns: new[] { "FK_USUARIO", "FK_MATERIAL" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_GRUPO_ESTUDO_FK_COMUNIDADE",
                table: "TB_GRUPO_ESTUDO",
                column: "FK_COMUNIDADE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LISTA_LEITURA_FK_USUARIO",
                table: "TB_LISTA_LEITURA",
                column: "FK_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LISTA_TEM_MATERIAL_FK_MATERIAL",
                table: "TB_LISTA_TEM_MATERIAL",
                column: "FK_MATERIAL");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MATERIAL_FK_USUARIO_CRIADOR",
                table: "TB_MATERIAL",
                column: "FK_USUARIO_CRIADOR");

            migrationBuilder.CreateIndex(
                name: "IX_TB_POSTAGEM_FK_COMUNIDADE",
                table: "TB_POSTAGEM",
                column: "FK_COMUNIDADE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_POSTAGEM_FK_USUARIO",
                table: "TB_POSTAGEM",
                column: "FK_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_COMUNIDADE_FK_COMUNIDADE",
                table: "TB_USUARIO_COMUNIDADE",
                column: "FK_COMUNIDADE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_GRUPO_FK_GRUPO",
                table: "TB_USUARIO_GRUPO",
                column: "FK_GRUPO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_AVALIACAO");

            migrationBuilder.DropTable(
                name: "TB_LISTA_TEM_MATERIAL");

            migrationBuilder.DropTable(
                name: "TB_POSTAGEM");

            migrationBuilder.DropTable(
                name: "TB_USUARIO_COMUNIDADE");

            migrationBuilder.DropTable(
                name: "TB_USUARIO_GRUPO");

            migrationBuilder.DropTable(
                name: "TB_LISTA_LEITURA");

            migrationBuilder.DropTable(
                name: "TB_MATERIAL");

            migrationBuilder.DropTable(
                name: "TB_GRUPO_ESTUDO");

            migrationBuilder.DropTable(
                name: "TB_COMUNIDADE");
        }
    }
}
