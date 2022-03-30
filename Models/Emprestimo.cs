using System.Collections.Generic;

namespace EmprestimoBancario.Models
{
    public enum Status
    {
        APROVADO = 'A',
        NEGADO = 'N',
        PENDENTE = 'P'

    }

    public class Emprestimo
    {
        public int Id { get; set; }
        
        public double Quantia { get; set; }
        
        public int LinhaDeCreditoId { get; set; }

        public LinhaDeCredito LinhaDeCredito { get; set; }

        public Status Status { get; set; }

        public List<InvestimentoDeEmprestimo> InvestimentoDeEmprestimo { get; set; } = new List<InvestimentoDeEmprestimo>();
    }
}
