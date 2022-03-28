using EmprestimoBancario.Business;
using EmprestimoBancario.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EmprestimoBancario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimoController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Emprestimo>> Listar()
        {
            try {
                EmprestimoBusiness business = new();
                List<Emprestimo> emprestimo = business.ListaEmprestimo();
                return Ok(emprestimo);
            }
            catch (ValidationException) {
                return BadRequest("Erro Interno.");
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<List<Emprestimo>> ListarPorId([FromRoute] int id)
        {
            try {
                var business = new EmprestimoBusiness();
                List<Emprestimo> emprestimo = business.ListaEmprestimo(id, true);
                return Ok(emprestimo);
            } catch (ValidationException) {
                return BadRequest("Erro Interno.");
            }
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
        public ActionResult<EmprestimoBusiness> Aumentar([FromRoute] int id, [FromBody] PagarEmprestimoModel model)
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
        public ActionResult<EmprestimoBusiness> Diminuir([FromRoute] int id, [FromBody] PagarEmprestimoModel model)
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
