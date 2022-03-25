using System.Collections.Generic;

namespace EmprestimoBancario.Models
{
    public enum Aprovado
    {
        SIM,
        NÃO
    }

    public class Emprestimo
    {
        public int Id { get; set; }
        public double Quantia { get; set; }
        public List<InvestimentoDeEmprestimo> InvestimentoDeEmprestimos { get; set; } = new List<InvestimentoDeEmprestimo>();
        public LinhaDeCredito LinhaDeCredito { get; set; }
        public int LinhaDeCreditoId { get; set; }
        public Aprovado Aprovado { get; set; }  
    }
}
