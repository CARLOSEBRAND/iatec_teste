using EmprestimoBancario.Business;
using EmprestimoBancario.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EmprestimoBancario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimoController : ControllerBase
    {
        public EmprestimoController(BancoDeDadosContexto @object)
        {
            Object = @object;
        }

        public BancoDeDadosContexto Object { get; }

        /// <summary>
        /// Cria um empréstimo.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/emprestimo
        ///     {
        ///      "quantia" : 500,
        ///      "linhaDeCreditoId" : 1
        ///     }
        /// </remarks>
        /// <param name="emprestimo"></param>
        /// <returns>O ID do empréstimo criado</returns>
        /// <response code="201">Retorna o ID do empréstimo criado</response>
        /// <response code="400">Retorna em caso de erros de validação</response>   
        [HttpPost]
        public ActionResult<int> Criar(Emprestimo emprestimo)
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

        /// <summary>
        /// Lista todos os empréstimos.
        /// </summary>
        /// <returns>Todos os empréstimos.</returns>
        /// <response code="200">Retorna todos os empréstimos</response>
        /// <response code="400">Retorna em caso de erros de validação</response>   
        /// GET: api/emprestimo
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

        /// <summary>
        /// Lista um empréstimo por ID.
        /// </summary>
        /// <returns>Retorna um empréstimo identificado pelo ID.</returns>
        /// <response code="200">Retorna um empréstimo</response>
        /// <response code="400">Retorna em caso de erros de validação</response>   
        /// GET: api/emprestimo
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

        /// <summary>
        /// Solicita ao Banco a aprovação de um empréstimo.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/emprestimo
        ///     {
        ///      "InvestidorId" : 2,
        ///      "Porcentagem" : 30,
        ///      "confirma" : "S"
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns>Vazio</returns>
        /// <response code="204">Sem conteudo de resposta</response>
        /// <response code="400">Retorna em caso de erros de validação</response> 
        [HttpPut("{id:int}/aprovar")]
        public ActionResult<EmprestimoBusiness> Aprovar([FromRoute] int id, [FromBody] AprovarEmprestimoModel model)
        {
            var business = new EmprestimoBusiness();
            try
            {
                business.Aprovar(id, model.InvestidorId, model.Porcentagem, model.Confirma);
                return NoContent();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Verifica o status de aprovação dos Bancos e realiza a aprovação do empréstimo.
        /// </summary>
        /// <returns>Empréstimo Aprovado/Negado/Pendente.</returns>
        /// <param name="id"></param>
        /// <response code="200">Retorna mensagem Empréstimo Aprovado/Negado/Pendente.</response>
        /// <response code="400">Retorna em caso de erros de validação</response>  
        /// GET: api/emprestimo
        [HttpGet("{id:int}/status")]
        public ActionResult<EmprestimoBusiness> VerificaStatus([FromRoute] int id)
        {
            var business = new EmprestimoBusiness();
            try
            {
                var mensagem = business.VerificaStatus(id);
                return Ok(mensagem);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Aumenta o valor de um empréstimo.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/emprestimo
        ///     {
        ///      "quantia" : 100,
        ///      "taxa" : 2.75
        ///     }
        /// </remarks> 
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns>Vazio</returns>
        /// <response code="204">Sem conteudo de resposta</response>
        /// <response code="400">Retorna em caso de erros de validação</response> 
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

        /// <summary>
        /// Realiza o pagamento de um empréstimo.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/emprestimo
        ///     {
        ///      "quantia" : 500,
        ///      "taxa" : 10.75
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns>Vazio</returns>
        /// <response code="204">Sem conteudo de resposta</response>
        /// <response code="400">Retorna em caso de erros de validação</response> 
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

        public char Confirma { get; set; }
    }
}
