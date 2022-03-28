using EmprestimoBancario.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace EmprestimoBancario.Business
{
    public class LinhaDeCreditoBusiness
    {
        public void Criar(LinhaDeCredito linhaDeCredito)
        {
            if (linhaDeCredito is null)
                throw new ValidationException("Linha de crédito não pode ser nulo");

            if (linhaDeCredito.EmpresaId == default)
                throw new ValidationException("A empresa é obrigatória");

            if (linhaDeCredito.Investimentos is null || !linhaDeCredito.Investimentos.Any())
                throw new ValidationException("Um linha de crédito só pode ser criado caso exista investimentos");

            if(linhaDeCredito.Limite <= 0)
                throw new ValidationException("O limite deve ser maior que zero");

            if(linhaDeCredito.Investimentos.Any(x => x.InvestidorId == default))
                throw new ValidationException("Os investimentos precisa de um investidor");

            if (linhaDeCredito.Investimentos.Any(x => x.Porcentagem <= 0))
                throw new ValidationException("A porcentagem de investimento deve ser maior que zero");

            if (linhaDeCredito.Investimentos.Any(x => x.Porcentagem > 100))
                throw new ValidationException("A porcentagem de investimento deve ser menor ou igual a cem");

            if (linhaDeCredito.Investimentos.Sum(x => x.Porcentagem) != 100)
                throw new ValidationException("Uma linha de crédito não pode ser criada caso a soma dos investimentos não seja igual ao limite da linha de crédito");

            try {
                using var bancoDeDados = new BancoDeDadosContexto();
                bancoDeDados.Add(linhaDeCredito);
                bancoDeDados.SaveChanges();
            } catch (DataBaseException e ) {
                throw new DataBaseException(e.Message);
            }
        }

        public List<LinhaDeCredito> ListaLinhaDeCredito([Optional] int id, [Optional] bool useId)
        {
            try {
                using BancoDeDadosContexto bancoDeDados = new();
                var listDelinhasDeCredito = bancoDeDados.LinhaDeCredito
                     .Include(x => x.Empresa).Include(x => x.Investimentos)
                     .ThenInclude(x => x.Investidor).Include(x => x.Investimentos)
                     .ThenInclude(x => x.Taxas).ToList();
               
                if (!useId) {
                    List<LinhaDeCredito> result = listDelinhasDeCredito;
                    return result; 
                } else {
                    List<LinhaDeCredito> result = new();
                    var listaFiltro = listDelinhasDeCredito.FirstOrDefault(x => x.Id == id);
                    if (listaFiltro != null) result.Add(listaFiltro);
                    return result;
                }
            }
            catch (DataBaseException e) {
                throw new DataBaseException(e.Message);
            }
        }
    }
}
