using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atria.Data.Migrations
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
                name: "TB_ROLE",
                columns: table => new
                {
                    IDROLE = table.Column<int>(name: "ID_ROLE", type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOME = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NORMALIZEDNOME = table.Column<string>(name: "NORMALIZED_NOME", type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CONCURRENCYSTAMP = table.Column<string>(name: "CONCURRENCY_STAMP", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ROLE", x => x.IDROLE);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    IDUSUARIO = table.Column<int>(name: "ID_USUARIO", type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EMAIL = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SENHAHASH = table.Column<string>(name: "SENHA_HASH", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATACADASTRO = table.Column<DateTime>(name: "DATA_CADASTRO", type: "datetime(6)", nullable: false),
                    USERNAME = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NORMALIZEDUSERNAME = table.Column<string>(name: "NORMALIZED_USERNAME", type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NORMALIZEDEMAIL = table.Column<string>(name: "NORMALIZED_EMAIL", type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EMAILCONFIRMED = table.Column<bool>(name: "EMAIL_CONFIRMED", type: "tinyint(1)", nullable: false),
                    SECURITYSTAMP = table.Column<string>(name: "SECURITY_STAMP", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CONCURRENCYSTAMP = table.Column<string>(name: "CONCURRENCY_STAMP", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TELEFONE = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TELEFONECONFIRMADO = table.Column<bool>(name: "TELEFONE_CONFIRMADO", type: "tinyint(1)", nullable: false),
                    DOISFATORESATIVADO = table.Column<bool>(name: "DOIS_FATORES_ATIVADO", type: "tinyint(1)", nullable: false),
                    BLOQUEIOFIM = table.Column<DateTimeOffset>(name: "BLOQUEIO_FIM", type: "datetime(6)", nullable: true),
                    BLOQUEIOATIVADO = table.Column<bool>(name: "BLOQUEIO_ATIVADO", type: "tinyint(1)", nullable: false),
                    FALHASACESSO = table.Column<int>(name: "FALHAS_ACESSO", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.IDUSUARIO);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_ROLE_CLAIM",
                columns: table => new
                {
                    IDROLECLAIM = table.Column<int>(name: "ID_ROLE_CLAIM", type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FKROLE = table.Column<int>(name: "FK_ROLE", type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ROLE_CLAIM", x => x.IDROLECLAIM);
                    table.ForeignKey(
                        name: "FK_TB_ROLE_CLAIM_TB_ROLE_FK_ROLE",
                        column: x => x.FKROLE,
                        principalTable: "TB_ROLE",
                        principalColumn: "ID_ROLE",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_USER_CLAIM",
                columns: table => new
                {
                    IDCLAIM = table.Column<int>(name: "ID_CLAIM", type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FKUSUARIO = table.Column<int>(name: "FK_USUARIO", type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USER_CLAIM", x => x.IDCLAIM);
                    table.ForeignKey(
                        name: "FK_TB_USER_CLAIM_TB_USUARIO_FK_USUARIO",
                        column: x => x.FKUSUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_USER_LOGIN",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FKUSUARIO = table.Column<int>(name: "FK_USUARIO", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USER_LOGIN", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_TB_USER_LOGIN_TB_USUARIO_FK_USUARIO",
                        column: x => x.FKUSUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_USER_ROLE",
                columns: table => new
                {
                    FKUSUARIO = table.Column<int>(name: "FK_USUARIO", type: "int", nullable: false),
                    FKROLE = table.Column<int>(name: "FK_ROLE", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USER_ROLE", x => new { x.FKUSUARIO, x.FKROLE });
                    table.ForeignKey(
                        name: "FK_TB_USER_ROLE_TB_ROLE_FK_ROLE",
                        column: x => x.FKROLE,
                        principalTable: "TB_ROLE",
                        principalColumn: "ID_ROLE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_USER_ROLE_TB_USUARIO_FK_USUARIO",
                        column: x => x.FKUSUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TB_USER_TOKEN",
                columns: table => new
                {
                    FKUSUARIO = table.Column<int>(name: "FK_USUARIO", type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USER_TOKEN", x => new { x.FKUSUARIO, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_TB_USER_TOKEN_TB_USUARIO_FK_USUARIO",
                        column: x => x.FKUSUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "TB_ROLE",
                column: "NORMALIZED_NOME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_ROLE_CLAIM_FK_ROLE",
                table: "TB_ROLE_CLAIM",
                column: "FK_ROLE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USER_CLAIM_FK_USUARIO",
                table: "TB_USER_CLAIM",
                column: "FK_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USER_LOGIN_FK_USUARIO",
                table: "TB_USER_LOGIN",
                column: "FK_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USER_ROLE_FK_ROLE",
                table: "TB_USER_ROLE",
                column: "FK_ROLE");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "TB_USUARIO",
                column: "NORMALIZED_EMAIL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "TB_USUARIO",
                column: "NORMALIZED_USERNAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_TB_USUARIO_EMAIL",
                table: "TB_USUARIO",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_ROLE_CLAIM");

            migrationBuilder.DropTable(
                name: "TB_USER_CLAIM");

            migrationBuilder.DropTable(
                name: "TB_USER_LOGIN");

            migrationBuilder.DropTable(
                name: "TB_USER_ROLE");

            migrationBuilder.DropTable(
                name: "TB_USER_TOKEN");

            migrationBuilder.DropTable(
                name: "TB_ROLE");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");
        }
    }
}
