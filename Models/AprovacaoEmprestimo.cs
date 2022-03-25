using System.Collections.Generic;

namespace EmprestimoBancario.Models
{
    public class AprovacaoEmprestimo
    {
        public int Id { get; set; }
        public int InvestidorId { get; set; }
        public int EmprestimoId { get; set; }
        public Banco Investidor { get; set; }
        public double Porcentagem { get; set; }        
    }    
}
