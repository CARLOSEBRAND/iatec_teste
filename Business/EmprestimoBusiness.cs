using EmprestimoBancario.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

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
                throw new ValidationException("O empréstimo não pode ser nulo.");

            if(emprestimo.Quantia <= 0)
                throw new ValidationException("A quantia deve ser maior que zero.");

            if(emprestimo.LinhaDeCreditoId == default)
                throw new ValidationException("A linha de crédito é obrigatória.");

            if(linhaDeCredito is null)
                throw new ValidationException("A linha de crédito é obrigatória.");
            
            if (emprestimo.Quantia > linhaDeCredito.Limite)
                throw new ValidationException("A quantia deve ser menor que o limite da linha de crédito.");

            if (emprestimo.InvestimentoDeEmprestimo.Any())
                throw new ValidationException("Os investimentos não devem ser informados.");

            emprestimo.Status = 'P';

            bancoDedados.Emprestimo.Add(emprestimo);
            bancoDedados.SaveChanges();
        }

        public void Aprovar(int id, int investidorId, double novaPorcentagem, char confirma)
        {
            BancoDeDadosContexto bancoDeDados = new();

            Emprestimo emprestimo = bancoDeDados.Emprestimo
                .Include(x => x.InvestimentoDeEmprestimo).ThenInclude(x => x.Investimento)
                .ThenInclude(x => x.Taxas).Include(x => x.LinhaDeCredito)
                .ThenInclude(x => x.Investimentos).FirstOrDefault(x => x.Id == id);

            var investimentos = emprestimo.LinhaDeCredito.Investimentos.ToList();

            var investidorAtual = investimentos.Find(x => x.InvestidorId == investidorId);

            if (novaPorcentagem > investidorAtual.Porcentagem && investimentos.All(x => x.Confirmado == 'P'))
                throw new ValidationException("O limite de investimento para esta linha de crédito é de até "+ investidorAtual.Porcentagem + "%.");

            var investimentosAtualNegados = investimentos.Where(x => x.Confirmado == 'N').Sum(x => x.Porcentagem);
            var investimentoAtualConfirmados = investimentos.Where(x => x.Confirmado == 'S').Sum(x => x.Porcentagem);
            var investimentosConfirmados = investimentos.Where(x => x.Confirmado == 'S').Sum(x => x.PorcentagemAprovada);

            double limite = 0;
            if (investimentosConfirmados == 0) {
                limite = Math.Abs(investimentosAtualNegados + investidorAtual.Porcentagem);
                
            } else {
                limite = Math.Abs((investimentosConfirmados - investimentosAtualNegados) - investimentoAtualConfirmados) + investidorAtual.Porcentagem;
            }

            if (novaPorcentagem > limite)
                 throw new ValidationException("O limite de investimento para esta linha de crédito é de até " + limite + "%.");

            investidorAtual.PorcentagemAprovada = novaPorcentagem;
            investidorAtual.Confirmado = confirma;

            bancoDeDados.SaveChanges();
        }

        public string VerificaStatus(int id)
        {
            BancoDeDadosContexto bancoDeDados = new();

            Emprestimo emprestimo = bancoDeDados.Emprestimo
               .Include(x => x.InvestimentoDeEmprestimo).ThenInclude(x => x.Investimento)
               .ThenInclude(x => x.Taxas).Include(x => x.LinhaDeCredito)
               .ThenInclude(x => x.Investimentos).FirstOrDefault(x => x.Id == id);

            var investimentos = emprestimo.LinhaDeCredito.Investimentos.ToList();

            if (investimentos.Any(x => x.Confirmado == 'P'))
                throw new ValidationException("Ainda existem solicitações de aprovação pendentes.");

            var investimentosConfirmados = investimentos.Where(x => x.Confirmado == 'S').Sum(x => x.PorcentagemAprovada);

            if (investimentosConfirmados == 100) {
                emprestimo.Status = 'A';
            } else {
                emprestimo.Status = 'N';
            }

            emprestimo.InvestimentoDeEmprestimo = emprestimo.LinhaDeCredito.Investimentos
                .Select(x => new InvestimentoDeEmprestimo {
                    InvestimentoId = x.Id,
                    Quantia = emprestimo.Quantia * x.PorcentagemAprovada / 100,
                    Data = DateTime.Now
                }).ToList();

            bancoDeDados.Emprestimo.Update(emprestimo);
            bancoDeDados.SaveChanges();
            
            if (investimentosConfirmados == 100) {
                return "Empréstimo Aprovado.";
            } else {
                return "Empréstimo Negado.";
            }
        }

        public void AumentarEmprestimo(int id, double quantia)
        {
            BancoDeDadosContexto bancoDeDados = new();

            Emprestimo emprestimo = bancoDeDados.Emprestimo
                .Include(x => x.InvestimentoDeEmprestimo).ThenInclude(x => x.Investimento)
                    .ThenInclude(x => x.Taxas).Include(x => x.LinhaDeCredito)
                    .ThenInclude(x => x.Investimentos).FirstOrDefault(x => x.Id == id);

            if (emprestimo is null)
                throw new ValidationException("Empréstimo não disponível.");

            if (emprestimo == null)
                throw new ValidationException("O empréstimo não pode ser nulo.");

            if (emprestimo.Status != 'A')
                throw new ValidationException("O empréstimo ainda não foi aprovado.");

            if (quantia <= 0)
                throw new ValidationException("A quantia deve ser maior que zero.");

            if (emprestimo.Quantia + quantia > emprestimo.LinhaDeCredito.Limite)
                throw new ValidationException("Não há limite disponível.");

            emprestimo.Quantia += quantia;

            List<InvestimentoDeEmprestimo> investimentoDeEmprestimo = emprestimo.LinhaDeCredito.Investimentos
                .Select( investimento => new InvestimentoDeEmprestimo
                {
                    InvestimentoId = investimento.Id,
                    Quantia = quantia * investimento.PorcentagemAprovada / 100,
                    Data = DateTime.Now
            }).ToList();

            emprestimo.InvestimentoDeEmprestimo.AddRange(investimentoDeEmprestimo);
                        
            bancoDeDados.SaveChanges();
        }

        public void PagarEmprestimo(int id, double quantia, double taxa)
        {
            BancoDeDadosContexto bancoDeDados = new();

            Emprestimo emprestimo = bancoDeDados.Emprestimo.Include(x => x.InvestimentoDeEmprestimo)
                .ThenInclude(x => x.Investimento).ThenInclude(x => x.Taxas)
                .Include(x => x.LinhaDeCredito).ThenInclude(x => x.Investimentos).FirstOrDefault(x => x.Id == id);

            if (emprestimo is null)
                throw new ValidationException("Empréstimo não disponível.");
            
            if (emprestimo == null)
                throw new ValidationException("O empréstimo não pode ser nulo.");

            if (emprestimo.Status != 'A')
                throw new ValidationException("O empréstimo ainda não foi aprovado.");

            if (quantia <= 0)
                throw new ValidationException("A quantia deve ser maior que zero.");

            if (taxa <= 0)
                throw new ValidationException("A taxa deve ser maior que zero.");

            if (emprestimo.Quantia - quantia < 0)
                throw new ValidationException("A quantia paga excede o valor restante do empréstimo.");

            emprestimo.Quantia -= quantia;

            emprestimo.LinhaDeCredito.Investimentos
                .ForEach( investimento => {
                    InvestimentoDeEmprestimo investimentoDeEmprestimo = new()
                    {
                        Investimento = investimento,
                        InvestimentoId = investimento.Id,
                        Quantia = quantia * investimento.PorcentagemAprovada / 100 * -1,
                        Data = DateTime.Now
                    };

                    investimentoDeEmprestimo.Investimento.Taxas.Add(new Taxa
                    {
                        Quantia = taxa * investimento.PorcentagemAprovada / 100,
                        Data = DateTime.Now
                    });

                    emprestimo.InvestimentoDeEmprestimo.Add(investimentoDeEmprestimo);
                });

            bancoDeDados.SaveChanges();
        }

        public List<Emprestimo> ListaEmprestimo([Optional] int id, [Optional] bool useId)
        {
            try
            {
                using BancoDeDadosContexto bancoDeDados = new();
                var listaDeEmprestimo = bancoDeDados.Emprestimo.Include(x => x.InvestimentoDeEmprestimo)
                    .ThenInclude(x => x.Investimento).ThenInclude(x => x.Taxas).Include(x => x.InvestimentoDeEmprestimo)
                    .ThenInclude(x => x.Investimento).ThenInclude(x => x.Investidor).Include(x => x.LinhaDeCredito)
                    .ThenInclude(x => x.Investimentos).Include(x => x.LinhaDeCredito).ThenInclude(x => x.Empresa).ToList();

                if (!useId)
                {
                    List<Emprestimo> result = listaDeEmprestimo;
                    return result;
                }
                else
                {
                    List<Emprestimo> result = new();
                    var listaFiltro = listaDeEmprestimo.FirstOrDefault(x => x.Id == id);
                    if (listaFiltro != null) result.Add(listaFiltro);
                    return result;
                }
            }
            catch (DataBaseException e)
            {
                throw new DataBaseException(e.Message);
            }
        }
        
    }
}