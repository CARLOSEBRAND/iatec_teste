using EmprestimoBancario.Business;
using EmprestimoBancario.Models;
using Microsoft.AspNetCore.Mvc;
using System;
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
            try {
                var business = new LinhaDeCreditoBusiness();
                business.Criar(linhaDeCredito);
                return Created("", linhaDeCredito.Id);
            }
            catch (Exception ex) {
                if (ex is ValidationException) {
                    return BadRequest(ex.Message);
                } else {
                    return BadRequest("Erro Interno.");
                    throw;
                }
            }
        }

        [HttpGet]
        public ActionResult<List<LinhaDeCredito>> Listar()
        {
            try
            {
                var business = new LinhaDeCreditoBusiness();
                List<LinhaDeCredito> linhasDeCredito = business.ListaLinhaDeCredito();
                return Ok(linhasDeCredito);
            }
            catch (DataBaseException)
            {
                return BadRequest("Erro Interno.");
                throw;
            }                      
        }


        [HttpGet("{id:int}")]
        public ActionResult<List<LinhaDeCredito>> ListarPorId([FromRoute] int id)
        {
            try {
                var business = new LinhaDeCreditoBusiness();
                List<LinhaDeCredito> linhaDeCredito = business.ListaLinhaDeCredito(id, true);
                return Ok(linhaDeCredito);
            }
            catch (DataBaseException) {
                return BadRequest("Erro Interno.");
            }
        }
    }
}
