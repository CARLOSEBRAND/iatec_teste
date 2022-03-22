using System;

namespace EmprestimoBancario.Models
{
    public class InvestimentoDeEmprestimo
    {
        public int Id { get; set; }
        public double Quantia { get; set; }
        public Investimento Investimento { get; set; }
        public int InvestimentoId { get; set; }
        public DateTime Data { get; set; }
    }
}
