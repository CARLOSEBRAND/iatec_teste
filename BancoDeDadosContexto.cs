using EmprestimoBancario.Models;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoBancario
{
    public class BancoDeDadosContexto : DbContext
    {
        public DbSet<Emprestimo> Emprestimo { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<LinhaDeCredito> LinhaDeCredito { get; set; }        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EmprestimoBancario");
        }
    }
}
