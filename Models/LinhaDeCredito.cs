using System.Collections.Generic;

namespace EmprestimoBancario.Models
{
    public class LinhaDeCredito
    {
        public int Id { get; set; }
        public float Limite { get; set; }
        public List<Investimento> Investimentos { get; set; }
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
