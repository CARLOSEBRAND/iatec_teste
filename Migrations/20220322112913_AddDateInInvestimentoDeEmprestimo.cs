using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmprestimoBancario.Migrations
{
    public partial class AddDateInInvestimentoDeEmprestimo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emprestimo_LinhaDeCredito_LinhaDeCreditoId",
                table: "Emprestimo");

            migrationBuilder.DropForeignKey(
                name: "FK_Investimento_Banco_InvestidorId",
                table: "Investimento");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestimentoDeEmprestimo_Investimento_InvestimentoId",
                table: "InvestimentoDeEmprestimo");

            migrationBuilder.DropForeignKey(
                name: "FK_LinhaDeCredito_Empresa_EmpresaId",
                table: "LinhaDeCredito");

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaId",
                table: "LinhaDeCredito",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InvestimentoId",
                table: "InvestimentoDeEmprestimo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "InvestimentoDeEmprestimo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "InvestidorId",
                table: "Investimento",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LinhaDeCreditoId",
                table: "Emprestimo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Emprestimo_LinhaDeCredito_LinhaDeCreditoId",
                table: "Emprestimo",
                column: "LinhaDeCreditoId",
                principalTable: "LinhaDeCredito",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Investimento_Banco_InvestidorId",
                table: "Investimento",
                column: "InvestidorId",
                principalTable: "Banco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestimentoDeEmprestimo_Investimento_InvestimentoId",
                table: "InvestimentoDeEmprestimo",
                column: "InvestimentoId",
                principalTable: "Investimento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LinhaDeCredito_Empresa_EmpresaId",
                table: "LinhaDeCredito",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emprestimo_LinhaDeCredito_LinhaDeCreditoId",
                table: "Emprestimo");

            migrationBuilder.DropForeignKey(
                name: "FK_Investimento_Banco_InvestidorId",
                table: "Investimento");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestimentoDeEmprestimo_Investimento_InvestimentoId",
                table: "InvestimentoDeEmprestimo");

            migrationBuilder.DropForeignKey(
                name: "FK_LinhaDeCredito_Empresa_EmpresaId",
                table: "LinhaDeCredito");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "InvestimentoDeEmprestimo");

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaId",
                table: "LinhaDeCredito",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "InvestimentoId",
                table: "InvestimentoDeEmprestimo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "InvestidorId",
                table: "Investimento",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LinhaDeCreditoId",
                table: "Emprestimo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Emprestimo_LinhaDeCredito_LinhaDeCreditoId",
                table: "Emprestimo",
                column: "LinhaDeCreditoId",
                principalTable: "LinhaDeCredito",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Investimento_Banco_InvestidorId",
                table: "Investimento",
                column: "InvestidorId",
                principalTable: "Banco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestimentoDeEmprestimo_Investimento_InvestimentoId",
                table: "InvestimentoDeEmprestimo",
                column: "InvestimentoId",
                principalTable: "Investimento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LinhaDeCredito_Empresa_EmpresaId",
                table: "LinhaDeCredito",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
