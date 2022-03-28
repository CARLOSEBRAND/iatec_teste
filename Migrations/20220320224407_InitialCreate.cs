using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmprestimoBancario.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Documento = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Documento = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinhaDeCredito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Limite = table.Column<float>(type: "real", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinhaDeCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinhaDeCredito_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Emprestimo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantia = table.Column<double>(type: "float", nullable: false),
                    LinhaDeCreditoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprestimo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emprestimo_LinhaDeCredito_LinhaDeCreditoId",
                        column: x => x.LinhaDeCreditoId,
                        principalTable: "LinhaDeCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Investimento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestidorId = table.Column<int>(type: "int", nullable: true),
                    Porcentagem = table.Column<double>(type: "float", nullable: false),
                    LinhaDeCreditoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investimento_Banco_InvestidorId",
                        column: x => x.InvestidorId,
                        principalTable: "Banco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Investimento_LinhaDeCredito_LinhaDeCreditoId",
                        column: x => x.LinhaDeCreditoId,
                        principalTable: "LinhaDeCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestimentoDeEmprestimo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantia = table.Column<double>(type: "float", nullable: false),
                    InvestimentoId = table.Column<int>(type: "int", nullable: true),
                    EmprestimoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestimentoDeEmprestimo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestimentoDeEmprestimo_Emprestimo_EmprestimoId",
                        column: x => x.EmprestimoId,
                        principalTable: "Emprestimo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestimentoDeEmprestimo_Investimento_InvestimentoId",
                        column: x => x.InvestimentoId,
                        principalTable: "Investimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Taxa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantia = table.Column<double>(type: "float", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvestimentoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Taxa_Investimento_InvestimentoId",
                        column: x => x.InvestimentoId,
                        principalTable: "Investimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimo_LinhaDeCreditoId",
                table: "Emprestimo",
                column: "LinhaDeCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_Investimento_InvestidorId",
                table: "Investimento",
                column: "InvestidorId");

            migrationBuilder.CreateIndex(
                name: "IX_Investimento_LinhaDeCreditoId",
                table: "Investimento",
                column: "LinhaDeCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestimentoDeEmprestimo_EmprestimoId",
                table: "InvestimentoDeEmprestimo",
                column: "EmprestimoId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestimentoDeEmprestimo_InvestimentoId",
                table: "InvestimentoDeEmprestimo",
                column: "InvestimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_LinhaDeCredito_EmpresaId",
                table: "LinhaDeCredito",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxa_InvestimentoId",
                table: "Taxa",
                column: "InvestimentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestimentoDeEmprestimo");

            migrationBuilder.DropTable(
                name: "Taxa");

            migrationBuilder.DropTable(
                name: "Emprestimo");

            migrationBuilder.DropTable(
                name: "Investimento");

            migrationBuilder.DropTable(
                name: "Banco");

            migrationBuilder.DropTable(
                name: "LinhaDeCredito");

            migrationBuilder.DropTable(
                name: "Empresa");
        }
    }
}
