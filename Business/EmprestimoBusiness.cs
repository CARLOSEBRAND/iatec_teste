using EmprestimoBancario.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EmprestimoBancario.Business
{
    public class EmprestimoBusiness
    {
        public void Criar(Emprestimo emprestimo)
        {
            var bancoDedados = new BancoDeDadosContexto();

            if (emprestimo == null)
                throw new ValidationException("O empréstimo não pode ser nulo");

            if(emprestimo.Quantia <= 0)
                throw new ValidationException("A quantia deve ser maior que zero");

            if(emprestimo.LinhaDeCreditoId == default)
                throw new ValidationException("A linha de crédito é obrigatória");

            var linhaDeCredito = bancoDedados.LinhaDeCredito
                .Include(x => x.Empresa)
                .Include(x => x.Investimentos)
                    .ThenInclude(x => x.Investidor)
                .Include(x => x.Investimentos)
                    .ThenInclude(x => x.Taxas)
                .FirstOrDefault(x => x.Id == emprestimo.LinhaDeCreditoId);

            if(linhaDeCredito is null)
                throw new ValidationException("A linha de crédito é obrigatória");
            
            if (emprestimo.Quantia > linhaDeCredito.Limite)
                throw new ValidationException("A quantia deve ser maior que zero");

            if (emprestimo.InvestimentoDeEmprestimos.Any())
                throw new ValidationException("Os investimentos não devem ser informados");

            emprestimo.InvestimentoDeEmprestimos = linhaDeCredito.Investimentos.Select(x => new InvestimentoDeEmprestimo
            {
                InvestimentoId = x.Id,
                Quantia = emprestimo.Quantia * x.Porcentagem / 100,
            }).ToList();
        }

        public void AumentarEmprestimo(Emprestimo emprestimo, double quantia)
        {
            if (emprestimo == null)
                throw new ValidationException("O empréstimo não pode ser nulo");

            if (quantia <= 0)
                throw new ValidationException("A quantia deve ser maior que zero");

            if (emprestimo.Quantia + quantia > emprestimo.LinhaDeCredito.Limite)
                throw new ValidationException("Não há limite disponível");

            emprestimo.Quantia += quantia;
            emprestimo.InvestimentoDeEmprestimos.ForEach(x =>
            {
                x.Quantia += (quantia * x.Investimento.Porcentagem / 100);
            });
        }

        public void PagarEmprestimo(Emprestimo emprestimo, double quantia, double taxa)
        {
            if (emprestimo == null)
                throw new ValidationException("O empréstimo não pode ser nulo");

            if (quantia <= 0)
                throw new ValidationException("A quantia deve ser maior que zero");

            if (taxa <= 0)
                throw new ValidationException("A taxa deve ser maior que zero");

            if (emprestimo.Quantia - quantia < 0)
                throw new ValidationException("A quantia paga excede o valor restante do empréstimo");

            emprestimo.Quantia -= quantia;
            emprestimo.InvestimentoDeEmprestimos.ForEach(x =>
            {
                x.Quantia -= (quantia * x.Investimento.Porcentagem);
                x.Investimento.Taxas.Add(new Taxa
                {
                    Quantia = taxa * x.Investimento.Porcentagem / 100,
                    Data = DateTime.Now
                });
            });
        }
    }
}