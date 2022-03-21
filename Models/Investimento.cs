using System.Collections.Generic;

namespace EmprestimoBancario.Models
{
    public class Investimento
    {
        public int Id { get; set; }
        public int InvestidorId { get; set; }
        public Banco Investidor { get; set; }
        public double Porcentagem { get; set; }
        public List<Taxa> Taxas { get; set; } = new List<Taxa>();
    }
}
