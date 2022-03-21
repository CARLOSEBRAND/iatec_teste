using EmprestimoBancario.Business;
using EmprestimoBancario.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EmprestimoBancario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimoController : ControllerBase
    {
        [HttpGet]
        public ActionResult Listar()
        {
            var bancoDedados = new BancoDeDadosContexto();
            var emprestimos = bancoDedados.Emprestimos
                .Include(x => x.InvestimentoDeEmprestimos)
                    .ThenInclude(x => x.Investimento)
                        .ThenInclude(x => x.Taxas)
                .Include(x => x.InvestimentoDeEmprestimos)
                    .ThenInclude(x => x.Investimento)
                        .ThenInclude(x => x.Investidor)
                .Include(x => x.LinhaDeCredito)
                    .ThenInclude(x => x.Investimentos)
                .Include(x => x.LinhaDeCredito)
                    .ThenInclude(x => x.Empresa);

            return Ok(emprestimos);
        }

        [HttpGet("{id:int}")]
        public ActionResult ListarPorId([FromRoute] int id)
        {
            var bancoDedados = new BancoDeDadosContexto();
            var emprestimo = bancoDedados.Emprestimos
                .Include(x => x.InvestimentoDeEmprestimos)
                    .ThenInclude(x => x.Investimento)
                        .ThenInclude(x => x.Taxas)
                .Include(x => x.InvestimentoDeEmprestimos)
                    .ThenInclude(x => x.Investimento)
                        .ThenInclude(x => x.Investidor)
                .Include(x => x.LinhaDeCredito)
                    .ThenInclude(x => x.Investimentos)
                .Include(x => x.LinhaDeCredito)
                    .ThenInclude(x => x.Empresa)
                .FirstOrDefault(x => x.Id == id);

            return Ok(emprestimo);
        }

        [HttpPost]
        public ActionResult Criar(Emprestimo emprestimo)
        {
            var bancoDedados = new BancoDeDadosContexto();
            var business = new EmprestimoBusiness();
            business.Criar(emprestimo);

            bancoDedados.Emprestimos.Add(emprestimo);
            bancoDedados.SaveChanges();
            return Created("", emprestimo.Id);
        }

        [HttpPut("{id:int}/aumentar")]
        public ActionResult Aumentar([FromRoute] int id, [FromBody] PagarEmprestimoModel model)
        {
            var bancoDeDados = new BancoDeDadosContexto();
            var emprestimo = bancoDeDados.Emprestimos
                .Include(x => x.InvestimentoDeEmprestimos)
                    .ThenInclude(x => x.Investimento)
                        .ThenInclude(x => x.Taxas)
                .Include(x => x.LinhaDeCredito)
                    .ThenInclude(x => x.Investimentos)
                .FirstOrDefault(x => x.Id == id);

            if(emprestimo is null)
                return NotFound();

            var business = new EmprestimoBusiness();
            business.AumentarEmprestimo(emprestimo, model.Quantia);

            bancoDeDados.SaveChanges();
            return NoContent();
        }


        [HttpPut("{id:int}/pagar")]
        public ActionResult Diminuir([FromRoute] int id, [FromBody] PagarEmprestimoModel model)
        {
            var bancoDeDados = new BancoDeDadosContexto();
            var emprestimo = bancoDeDados.Emprestimos
                .Include(x => x.InvestimentoDeEmprestimos)
                    .ThenInclude(x => x.Investimento)
                        .ThenInclude(x => x.Taxas)
                .Include(x => x.LinhaDeCredito)
                    .ThenInclude(x => x.Investimentos)
                .FirstOrDefault(x => x.Id == id);

            if (emprestimo is null)
                return NotFound();

            var business = new EmprestimoBusiness();
            business.PagarEmprestimo(emprestimo, model.Quantia, model.Taxa);

            bancoDeDados.SaveChanges();
            return NoContent();
        }

    }

    public class PagarEmprestimoModel
    {
        public double Quantia { get; set; }
        public double Taxa { get; set; }
    }
}
