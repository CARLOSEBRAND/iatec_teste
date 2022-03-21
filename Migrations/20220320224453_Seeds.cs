using Microsoft.EntityFrameworkCore.Migrations;

namespace EmprestimoBancario.Migrations
{
    public partial class Seeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Empresa", new string[] { "Nome", "Documento" }, new string[] { "Luiza e Geraldo Eletrônica ME", "69.950.207/0001-23" });
            migrationBuilder.InsertData("Empresa", new string[] { "Nome", "Documento" }, new string[] { "Cláudia e Betina Financeira ME", "08.551.812/0001-37" });
            migrationBuilder.InsertData("Empresa", new string[] { "Nome", "Documento" }, new string[] { "Isadora e Mariane Gráfica ME", "62.939.102/0001-24" });
            migrationBuilder.InsertData("Empresa", new string[] { "Nome", "Documento" }, new string[] { "Benício e Levi Pizzaria Delivery Ltda", "77.676.766/0001-15" });

            migrationBuilder.InsertData("Banco", new string[] { "Nome", "Documento" }, new string[] { "Bradesco", "75.467.990/0001-71" });
            migrationBuilder.InsertData("Banco", new string[] { "Nome", "Documento" }, new string[] { "Itaú", "54.490.274/0001-35" });
            migrationBuilder.InsertData("Banco", new string[] { "Nome", "Documento" }, new string[] { "Banco do Brasil", "33.454.795/0001-67" });
            migrationBuilder.InsertData("Banco", new string[] { "Nome", "Documento" }, new string[] { "Santander", "01.806.589/0001-82" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("Empresa", new string[] { "Nome", "Documento" }, new string[] { "Luiza e Geraldo Eletrônica ME", "69.950.207/0001-23" });
            migrationBuilder.DeleteData("Empresa", new string[] { "Nome", "Documento" }, new string[] { "Cláudia e Betina Financeira ME", "08.551.812/0001-37" });
            migrationBuilder.DeleteData("Empresa", new string[] { "Nome", "Documento" }, new string[] { "Isadora e Mariane Gráfica ME", "62.939.102/0001-24" });
            migrationBuilder.DeleteData("Empresa", new string[] { "Nome", "Documento" }, new string[] { "Benício e Levi Pizzaria Delivery Ltda", "77.676.766/0001-15" });

            migrationBuilder.DeleteData("Banco", new string[] { "Nome", "Documento" }, new string[] { "Bradesco", "75.467.990/0001-71" });
            migrationBuilder.DeleteData("Banco", new string[] { "Nome", "Documento" }, new string[] { "Itaú", "54.490.274/0001-35" });
            migrationBuilder.DeleteData("Banco", new string[] { "Nome", "Documento" }, new string[] { "Banco do Brasil", "33.454.795/0001-67" });
            migrationBuilder.DeleteData("Banco", new string[] { "Nome", "Documento" }, new string[] { "Santander", "01.806.589/0001-82" });
        }
    }
}
