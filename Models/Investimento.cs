using System.Collections.Generic;

namespace EmprestimoBancario.Models
{
    public class Investimento
    {
        public int Id { get; set; }

        public int InvestidorId { get; set; }

        public Banco Investidor { get; set; }

        public double Porcentagem { get; set; }

        public double PorcentagemAprovada { get; set; }

        public char Confirmado { get; set; } = 'P';

        public List<Taxa> Taxas { get; set; } = new List<Taxa>();

    }    
}
