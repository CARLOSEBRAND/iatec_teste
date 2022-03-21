using EmprestimoBancario.Models;
using System.Linq;

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
        }
    }
}
