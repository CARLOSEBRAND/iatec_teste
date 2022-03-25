using EmprestimoBancario.Business;
using EmprestimoBancario.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EmprestimoBancario.Controllers
{
    [Route("api/linha-de-credito")]
    [ApiController]
    public class LinhaDeCreditoController : ControllerBase
    {
        [HttpPost]
        public ActionResult<LinhaDeCredito> Criar([FromBody] LinhaDeCredito linhaDeCredito)
        {
            var business = new LinhaDeCreditoBusiness();
            try
            {
                business.Criar(linhaDeCredito);
                return Created("", linhaDeCredito.Id);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
                throw;
            }
        }

        [HttpGet]
        public ActionResult<List<LinhaDeCredito>> Listar()
        {
            var bancoDeDados = new BancoDeDadosContexto();
            var linhasDeCredito = bancoDeDados.LinhaDeCredito
                .Include(x => x.Empresa)
                .Include(x => x.Investimentos)
                    .ThenInclude(x => x.Investidor)
                .Include(x => x.Investimentos)
                    .ThenInclude(x => x.Taxas);

            return Ok(linhasDeCredito);
        }


        [HttpGet("{id:int}")]
        public ActionResult<List<LinhaDeCredito>> ListarPorId([FromRoute] int id)
        {
            var bancoDeDados = new BancoDeDadosContexto();
            var linhasDeCredito = bancoDeDados.LinhaDeCredito
                .Include(x => x.Empresa)
                .Include(x => x.Investimentos)
                    .ThenInclude(x => x.Investidor)
                .Include(x => x.Investimentos)
                    .ThenInclude(x => x.Taxas);

            return Ok(linhasDeCredito.FirstOrDefault(x => x.Id == id));
        }

    }
}
