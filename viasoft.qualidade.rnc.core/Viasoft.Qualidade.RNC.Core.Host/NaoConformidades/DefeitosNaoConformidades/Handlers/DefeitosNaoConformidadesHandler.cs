using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.DefeitosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Services;
using Viasoft.Qualidade.RNC.Core.Host.Defeitos.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Handlers;

public class DefeitosNaoConformidadesHandler : IHandleMessages<DefeitoNaoConformidadeRemovido>,
    IHandleMessages<DefeitoNaoConformidadeInserido>,
    IHandleMessages<DefeitoNaoConformidadeAtualizado>
{
    private readonly IAcaoPreventivaNaoConformidadeService _acaoPreventivaNaoConformidadeService;
    private readonly IRepository<AcaoPreventivaNaoConformidade> _acaoPreventivaNaoConformidades;
    private readonly ICausaNaoConformidadeService _causaNaoConformidadeService;
    private readonly IRepository<CausaNaoConformidade> _causaNaoConformidades;
    private readonly ISolucaoNaoConformidadeService _solucaoNaoConformidadeService;
    private readonly IRepository<SolucaoNaoConformidade> _solucaoNaoConformidades;
    private readonly IDefeitoService _defeitoService;
    private readonly ICausaService _causaService;
    private readonly ISolucaoService _solucaoService;


    public DefeitosNaoConformidadesHandler(IAcaoPreventivaNaoConformidadeService acaoPreventivaNaoConformidadeService,
        IRepository<AcaoPreventivaNaoConformidade> acaoPreventivaNaoConformidades,
        ICausaNaoConformidadeService causaNaoConformidadeService,
        IRepository<CausaNaoConformidade> causaNaoConformidades,
        ISolucaoNaoConformidadeService solucaoNaoConformidadeService,
        IRepository<SolucaoNaoConformidade> solucaoNaoConformidades,
        IDefeitoService defeitoService, ICausaService causaService, ISolucaoService solucaoService)
    {
        _acaoPreventivaNaoConformidadeService = acaoPreventivaNaoConformidadeService;
        _acaoPreventivaNaoConformidades = acaoPreventivaNaoConformidades;
        _causaNaoConformidadeService = causaNaoConformidadeService;
        _causaNaoConformidades = causaNaoConformidades;
        _solucaoNaoConformidadeService = solucaoNaoConformidadeService;
        _solucaoNaoConformidades = solucaoNaoConformidades;
        _defeitoService = defeitoService;
        _causaService = causaService;
        _solucaoService = solucaoService;
    }

    public async Task Handle(DefeitoNaoConformidadeRemovido message)
    {
        var acoes = await _acaoPreventivaNaoConformidades.Where(entity =>
                entity.IdDefeitoNaoConformidade.Equals(message.IdDefeito))
            .Select(entity => new AcaoPreventivaNaoConformidade(entity)).ToListAsync();

        var causas = await _causaNaoConformidades.Where(entity =>
                entity.IdDefeitoNaoConformidade.Equals(message.IdDefeito))
            .Select(entity => new CausaNaoConformidade(entity)).ToListAsync();

        var solucoes = await _solucaoNaoConformidades.Where(entity =>
                entity.IdDefeitoNaoConformidade.Equals(message.IdDefeito))
            .Select(entity => new SolucaoNaoConformidade(entity)).ToListAsync();

        foreach (var acao in acoes)
        {
            await _acaoPreventivaNaoConformidadeService.Remove(acao.IdNaoConformidade, acao.Id);
        }

        foreach (var causa in causas)
        {
            await _causaNaoConformidadeService.Remove(causa.IdNaoConformidade, causa.Id);
        }

        foreach (var solucao in solucoes)
        {
            await _solucaoNaoConformidadeService.Remove(solucao.IdNaoConformidade, solucao.Id);
        }
    }

    public async Task Handle(DefeitoNaoConformidadeInserido message)
    {
        var defeito = await _defeitoService.Get(message.Command.DefeitoNaoConformidade.IdDefeito);
        if (defeito.IdCausa != null)
        {
            var causa = await _causaService.Get(defeito.IdCausa.Value);
            var input = new CausaNaoConformidadeInput
            {
                Id = Guid.NewGuid(),
                IdNaoConformidade = message.Command.DefeitoNaoConformidade.IdNaoConformidade,
                IdDefeitoNaoConformidade = message.Command.DefeitoNaoConformidade.Id,
                Detalhamento = causa.Detalhamento,
                IdCausa = causa.Id
            };

            await _causaNaoConformidadeService.Insert(message.Command.DefeitoNaoConformidade.IdNaoConformidade, input);
        }

        if (defeito.IdSolucao != null)
        {
            var solucao = await _solucaoService.Get(defeito.IdSolucao.Value);
            var input = new SolucaoNaoConformidadeInput
            {
                Id = Guid.NewGuid(),
                IdNaoConformidade = message.Command.DefeitoNaoConformidade.IdNaoConformidade,
                IdDefeitoNaoConformidade = message.Command.DefeitoNaoConformidade.Id,
                SolucaoImediata = solucao.Imediata,
                IdSolucao = solucao.Id,
                Detalhamento = solucao.Detalhamento,
                DataAnalise = null,
                DataPrevistaImplantacao = null,
                IdResponsavel = null,
                CustoEstimado = 0,
                NovaData = null,
                DataVerificacao = null,
                IdAuditor = null,
            };
            await _solucaoNaoConformidadeService.Insert(message.Command.DefeitoNaoConformidade.IdNaoConformidade, input);
        }
    }

    public async Task Handle(DefeitoNaoConformidadeAtualizado message)
    {
        if (message.Command.DefeitoNaoConformidade.IdDefeito != message.IdDefeitoAnterior)
        {
            var defeito = await _defeitoService.Get(message.Command.DefeitoNaoConformidade.IdDefeito);
            if (defeito.IdCausa != null)
            {
                var causa = await _causaService.Get(defeito.IdCausa.Value);
                var causasNaoConformidade =
                    await _causaNaoConformidades
                        .Where(entity => entity.IdNaoConformidade.Equals(message.Command.DefeitoNaoConformidade.IdNaoConformidade))
                        .Where(entity => entity.IdDefeitoNaoConformidade.Equals(message.Command.DefeitoNaoConformidade.Id))
                        .Select(entity => new CausaNaoConformidadeOutput(entity)).ToListAsync();
                var input = new CausaNaoConformidadeInput
                {
                    Id = Guid.NewGuid(),
                    IdNaoConformidade = message.Command.DefeitoNaoConformidade.IdNaoConformidade,
                    IdDefeitoNaoConformidade = message.Command.DefeitoNaoConformidade.Id,
                    Detalhamento = causa.Detalhamento,
                    IdCausa = causa.Id
                };
                foreach (var causaNaoConformidade in causasNaoConformidade)
                {
                    if (defeito.IdCausa == causaNaoConformidade.IdCausa)
                    {
                        break;
                    }

                    if (causasNaoConformidade.Count == 0)
                    {
                        await _causaNaoConformidadeService.Insert(message.Command.DefeitoNaoConformidade.IdNaoConformidade, input);
                    }

                    if (defeito.IdCausa != causaNaoConformidade.IdCausa)
                    {
                        await _causaNaoConformidadeService.Remove(message.Command.DefeitoNaoConformidade.IdNaoConformidade,
                            causaNaoConformidade.Id);
                        await _causaNaoConformidadeService.Insert(message.Command.DefeitoNaoConformidade.IdNaoConformidade, input);
                    }
                }
            }

            if (defeito.IdSolucao != null)
            {
                var solucao = await _solucaoService.Get(defeito.IdSolucao.Value);
                var solucoesNaoConformidade =
                    await _solucaoNaoConformidades
                        .Where(entity => entity.IdNaoConformidade.Equals(message.Command.DefeitoNaoConformidade.IdNaoConformidade))
                        .Where(entity => entity.IdDefeitoNaoConformidade.Equals(message.Command.DefeitoNaoConformidade.Id))
                        .Select(entity => new SolucaoNaoConformidadeOutput(entity)).ToListAsync();
                var input = new SolucaoNaoConformidadeInput
                {
                    Id = Guid.NewGuid(),
                    IdNaoConformidade = message.Command.DefeitoNaoConformidade.IdNaoConformidade,
                    IdDefeitoNaoConformidade = message.Command.DefeitoNaoConformidade.Id,
                    SolucaoImediata = solucao.Imediata,
                    IdSolucao = solucao.Id,
                    Detalhamento = solucao.Detalhamento,
                    DataAnalise = null,
                    DataPrevistaImplantacao = null,
                    IdResponsavel = null,
                    CustoEstimado = 0,
                    NovaData = null,
                    DataVerificacao = null,
                    IdAuditor = null,
                };
                foreach (var solucaoNaoConformidade in solucoesNaoConformidade)
                {
                    if (defeito.IdSolucao == solucaoNaoConformidade.IdSolucao)
                    {
                        break;
                    }

                    if (solucoesNaoConformidade.Count == 0)
                    {
                        await _solucaoNaoConformidadeService.Insert(message.Command.DefeitoNaoConformidade.IdNaoConformidade, input);
                    }

                    if (defeito.IdSolucao != solucaoNaoConformidade.IdSolucao)
                    {
                        await _solucaoNaoConformidadeService.Remove(message.Command.DefeitoNaoConformidade.IdNaoConformidade,
                            solucaoNaoConformidade.Id);
                        await _solucaoNaoConformidadeService.Insert(message.Command.DefeitoNaoConformidade.IdNaoConformidade, input);
                    }
                }
            }
        }
    }
}