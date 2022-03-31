using EmprestimoBancario.Business;
using EmprestimoBancario.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EmprestimoBancario.Controllers
{
    [Route("api/linha-de-credito")]
    [ApiController]
    public class LinhaDeCreditoController : ControllerBase
    {
        public LinhaDeCreditoController(BancoDeDadosContexto @object)
        {
            Object = @object;
        }

        public BancoDeDadosContexto Object { get; }

        /// <summary>
        /// Cria uma linha de crédito.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/linha-de-credito
        ///     {
        ///      "Limite" : 1000,
	    ///      "EmpresaId" : 1,
        ///      "Investimentos" : [
        ///				{"InvestidorId":1, "Porcentagem":25},
        ///				{"InvestidorId":2, "Porcentagem":25},
        ///				{"InvestidorId":3, "Porcentagem":25},
        ///				{"InvestidorId":4, "Porcentagem":25}
		///		    ]  
        ///     }
        /// </remarks> 
        /// <param name="linhaDeCredito"></param>
        /// <returns>O ID de uma linha de crédito criada</returns>
        /// <response code="201">Retorna o ID de uma linha de crédito criada</response>
        /// <response code="400">Retorna em caso de erros de validação</response>  
        [HttpPost]
        public ActionResult<int> Criar([FromBody] LinhaDeCredito linhaDeCredito)
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

        /// <summary>
        /// Lista todas as linhas de crédito.
        /// </summary>
        /// <returns>Lista de linhas de crédito.</returns>
        /// GET: api/linha-de-credito
        /// <response code="200">Retorna todas as linhas de crédito</response>
        /// <response code="400">Retorna em caso de erros de validação</response>
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

        /// <summary>
        /// Lista uma linhas de crédito por Id.
        /// </summary>
        /// <returns>Lista de linhas de crédito.</returns>
        /// GET: api/linha-de-credito
        /// <response code="200">Retorna uma linha de crédito</response>
        /// <response code="400">Retorna em caso de erros de validação</response>
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
