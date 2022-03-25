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
        public ActionResult<Emprestimo> Criar(Emprestimo emprestimo)
        {
            var business = new EmprestimoBusiness();
            try
            {
                business.Criar(emprestimo);
                return Created("", emprestimo.Id);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:int}/aprovar")]
        public ActionResult Aprovar([FromRoute] int id, [FromBody] AprovarEmprestimoModel model)
        {
            var business = new EmprestimoBusiness();
            try
            {
                business.Aprovar(id, model.InvestidorId, model.Porcentagem);
                return NoContent();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:int}/status")]
        public ActionResult VerificaStatus([FromRoute] int id)
        {
            var business = new EmprestimoBusiness();
            try
            {
                business.VerificaStatus(id);
                return NoContent();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:int}/aumentar")]
        public ActionResult Aumentar([FromRoute] int id, [FromBody] PagarEmprestimoModel model)
        {
            try
            {
                var business = new EmprestimoBusiness();
                business.AumentarEmprestimo(id, model.Quantia);
                return NoContent();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }           
            
        }


        [HttpPut("{id:int}/pagar")]
        public ActionResult Diminuir([FromRoute] int id, [FromBody] PagarEmprestimoModel model)
        {
            try
            {
                var business = new EmprestimoBusiness();
                business.PagarEmprestimo(id, model.Quantia, model.Taxa);
                return NoContent();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }         
        }
    }

    public class PagarEmprestimoModel
    {
        public double Quantia { get; set; }
        public double Taxa { get; set; }
    }

    public class AprovarEmprestimoModel
    {
        public int InvestidorId { get; set; }
        public double Porcentagem { get; set; }
    }
}
