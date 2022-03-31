using Microsoft.EntityFrameworkCore.Migrations;

namespace EmprestimoBancario.Migrations
{
    public partial class CamposAdicionais : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<char>(
                name: "Confirmado",
                table: "Investimento",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: 'P');

            migrationBuilder.AddColumn<double>(
                name: "PorcentagemAprovada",
                table: "Investimento",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<char>(
                name: "Status",
                table: "Emprestimo",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: 'P');
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmado",
                table: "Investimento");

            migrationBuilder.DropColumn(
                name: "PorcentagemAprovada",
                table: "Investimento");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Emprestimo");
        }
    }
}
