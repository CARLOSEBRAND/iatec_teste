using Microsoft.EntityFrameworkCore.Migrations;

namespace EmprestimoBancario.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Aprovado",
                table: "Investimento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Aprovado",
                table: "Emprestimos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AprovacaoEmprestimo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestidorId = table.Column<int>(type: "int", nullable: false),
                    EmprestimoId = table.Column<int>(type: "int", nullable: false),
                    Porcentagem = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AprovacaoEmprestimo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AprovacaoEmprestimo_Banco_InvestidorId",
                        column: x => x.InvestidorId,
                        principalTable: "Banco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AprovacaoEmprestimo_InvestidorId",
                table: "AprovacaoEmprestimo",
                column: "InvestidorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AprovacaoEmprestimo");

            migrationBuilder.DropColumn(
                name: "Aprovado",
                table: "Investimento");

            migrationBuilder.DropColumn(
                name: "Aprovado",
                table: "Emprestimos");
        }
    }
}
