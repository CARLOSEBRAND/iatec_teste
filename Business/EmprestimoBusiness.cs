using EmprestimoBancario.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmprestimoBancario.Business
{
    public class EmprestimoBusiness
    {
        public void Criar(Emprestimo emprestimo)
        {

            BancoDeDadosContexto bancoDedados = new();
            LinhaDeCredito linhaDeCredito = bancoDedados.LinhaDeCredito
                .Include(x => x.Empresa).Include(x => x.Investimentos)
                    .ThenInclude(x => x.Investidor).Include(x => x.Investimentos)
                    .ThenInclude(x => x.Taxas).FirstOrDefault(x => x.Id == emprestimo.LinhaDeCreditoId);

            if (emprestimo == null)
                throw new ValidationException("O empréstimo não pode ser nulo");

            if(emprestimo.Quantia <= 0)
                throw new ValidationException("A quantia deve ser maior que zero");

            if(emprestimo.LinhaDeCreditoId == default)
                throw new ValidationException("A linha de crédito é obrigatória");

            if(linhaDeCredito is null)
                throw new ValidationException("A linha de crédito é obrigatória");
            
            if (emprestimo.Quantia > linhaDeCredito.Limite)
                throw new ValidationException("A quantia deve ser menor que o limite da linha de crédito");

            //if (emprestimo.InvestimentoDeEmprestimos.Any())
            //    throw new ValidationException("Os investimentos não devem ser informados");

            emprestimo.InvestimentoDeEmprestimos = linhaDeCredito.Investimentos.Select(x => new InvestimentoDeEmprestimo
            {
                InvestimentoId = x.Id,
                Quantia = emprestimo.Quantia * x.Porcentagem / 100,
                Data = DateTime.Now
            }).ToList();

            emprestimo.Aprovado = Aprovado.NÃO;

            bancoDedados.Emprestimos.Add(emprestimo);
            bancoDedados.SaveChanges();
        }

        public void Aprovar(int id, int investidorId, double porcentagem)
        {
            BancoDeDadosContexto bancoDeDados = new();

            AprovacaoEmprestimo aprovacaoEmprestimo = bancoDeDados.AprovacaoEmprestimo
                .Where(p => p.Id == id && p.InvestidorId == investidorId).SingleOrDefault(c=> c == null);
            if (aprovacaoEmprestimo == null)
            {
               aprovacaoEmprestimo = new AprovacaoEmprestimo();
            }
            
            aprovacaoEmprestimo.EmprestimoId = id;
            aprovacaoEmprestimo.InvestidorId = investidorId;
            aprovacaoEmprestimo.Porcentagem = porcentagem;

            bancoDeDados.AprovacaoEmprestimo.Add(aprovacaoEmprestimo);
            bancoDeDados.SaveChanges();
        }

        public void VerificaStatus(int id)
        {
            BancoDeDadosContexto bancoDeDados = new();
            var aprovacaoEmprestimo = bancoDeDados.AprovacaoEmprestimo.Where(p => p.EmprestimoId == id ).ToList();
             
            if (aprovacaoEmprestimo.Any(x => x.Porcentagem > 100))
                throw new ValidationException("A porcentagem de investimento deve ser menor ou igual a cem");

            if (aprovacaoEmprestimo.Sum(x => x.Porcentagem) != 100)
                throw new ValidationException("Uma linha de crédito não pode ser criada caso a soma dos investimentos não seja igual ao limite da linha de crédito");

            Emprestimo emprestimo = bancoDeDados.Emprestimos.Where(p => p.Id == id).First();
            if (aprovacaoEmprestimo.Sum(x => x.Porcentagem) == 100)
            {
                emprestimo.Aprovado = Aprovado.SIM;
            }
            bancoDeDados.Emprestimos.Add(emprestimo);
            bancoDeDados.SaveChanges();
        }

        public void AumentarEmprestimo(int id, double quantia)
        {
            BancoDeDadosContexto bancoDeDados = new();

            Emprestimo emprestimo = bancoDeDados.Emprestimos
                .Include(x => x.InvestimentoDeEmprestimos).ThenInclude(x => x.Investimento)
                    .ThenInclude(x => x.Taxas).Include(x => x.LinhaDeCredito)
                    .ThenInclude(x => x.Investimentos).FirstOrDefault(x => x.Id == id);

            if (emprestimo is null)
                throw new ValidationException("Empréstimo não disponível");

            if (emprestimo == null)
                throw new ValidationException("O empréstimo não pode ser nulo");

            if (quantia <= 0)
                throw new ValidationException("A quantia deve ser maior que zero");

            if (emprestimo.Quantia + quantia > emprestimo.LinhaDeCredito.Limite)
                throw new ValidationException("Não há limite disponível");

            emprestimo.Quantia += quantia;

            List<InvestimentoDeEmprestimo> investimentoDeEmprestimo = emprestimo.LinhaDeCredito.Investimentos
                .Select( investimento => new InvestimentoDeEmprestimo
                {
                    InvestimentoId = investimento.Id,
                    Quantia = quantia * investimento.Porcentagem / 100,
                    Data = DateTime.Now
            }).ToList();

            emprestimo.InvestimentoDeEmprestimos.AddRange(investimentoDeEmprestimo);
                        
            bancoDeDados.SaveChanges();
        }

        public void PagarEmprestimo(int id, double quantia, double taxa)
        {
            BancoDeDadosContexto bancoDeDados = new();

            Emprestimo emprestimo = bancoDeDados.Emprestimos.Include(x => x.InvestimentoDeEmprestimos)
                .ThenInclude(x => x.Investimento).ThenInclude(x => x.Taxas)
                .Include(x => x.LinhaDeCredito).ThenInclude(x => x.Investimentos).FirstOrDefault(x => x.Id == id);

            if (emprestimo is null)
                throw new ValidationException("Empréstimo não disponível");
            
            if (emprestimo == null)
                throw new ValidationException("O empréstimo não pode ser nulo");

            if (quantia <= 0)
                throw new ValidationException("A quantia deve ser maior que zero");

            if (taxa <= 0)
                throw new ValidationException("A taxa deve ser maior que zero");

            if (emprestimo.Quantia - quantia < 0)
                throw new ValidationException("A quantia paga excede o valor restante do empréstimo");

            emprestimo.Quantia -= quantia;

            emprestimo.LinhaDeCredito.Investimentos
                .ForEach( investimento => {
                    InvestimentoDeEmprestimo investimentoDeEmprestimo = new()
                    {
                        Investimento = investimento,
                        InvestimentoId = investimento.Id,
                        Quantia = quantia * investimento.Porcentagem / 100 * -1,
                        Data = DateTime.Now
                    };

                    investimentoDeEmprestimo.Investimento.Taxas.Add(new Taxa
                    {
                        Quantia = taxa * investimento.Porcentagem / 100,
                        Data = DateTime.Now
                    });

                    emprestimo.InvestimentoDeEmprestimos.Add(investimentoDeEmprestimo);
                });

            bancoDeDados.SaveChanges();
        }
    }
}