using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.PushNotifications.Abstractions.Notification;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Events;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.OrdemProducao.Events;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalHandlers.ProducaoApontamento.Operacoes;

public class ApontamentoHandler : IHandleMessages<ApontamentoInicioProducaoEvent>,
    IHandleMessages<ApontamentoFimProducaoEvent>, IHandleMessages<ApontamentoOperacaoRemovidaEvent>,
    IHandleMessages<ApontamentoEstornadoEvent>, IHandleMessages<OrdemProducaoEncerradaEvent>,
    IHandleMessages<OrdemProducaoCanceladaEvent>
{
    private readonly IRepository<Operacao> _operacoes;
    private readonly IRepository<OrdemRetrabalhoNaoConformidade> _ordemRetrabalhoNaoConformidades;
    private readonly IPushNotification _pushNotification;
    private readonly IOperacaoService _operacaoService;
    private const string BuscarStatusOperacoesCommand = "Viasoft.Qualidade.RNC.Core.BuscarStatusOperacoes";
    private const string BuscarStatusOrdemRetrabalhoCommand = "Viasoft.Qualidade.RNC.Core.BuscarStatusOrdemRetrabalho";
    public ApontamentoHandler(IRepository<Operacao> operacoes, IRepository<OrdemRetrabalhoNaoConformidade> ordemRetrabalhoNaoConformidades,
        IPushNotification pushNotification, IOperacaoService operacaoService)
    {
        _operacoes = operacoes;
        _ordemRetrabalhoNaoConformidades = ordemRetrabalhoNaoConformidades;
        _pushNotification = pushNotification;
        _operacaoService = operacaoService;
    }
    public async Task Handle(ApontamentoInicioProducaoEvent message)
    {
        var isRetrabalho = message.ApontamentoProducaoEventDto.IsOperacaoRetrabalho ||
                                     message.ApontamentoProducaoEventDto.IsOrdemRetrabalho;
        if (!isRetrabalho)
        {
            return;
        }

        var operacao =
            await GetOperacaoByNumeroOdfENumeroOperacao(message.ApontamentoProducaoEventDto.NumeroOdf, message.ApontamentoProducaoEventDto.NumeroOperacao);

        if (operacao is null)
        {
            var ordemRetrabalho = await GetOrdemRetrabalhoByNumeroOdf(message.ApontamentoProducaoEventDto.NumeroOdf);
            if (ordemRetrabalho is null)
            {
                return;
            }

            await ChangeStatusOrdemRetrabalho(ordemRetrabalho, StatusProducaoRetrabalho.Produzindo);
            return;
        }

        await ChangeStatusOperacao(operacao, StatusProducaoRetrabalho.Produzindo);

    }
    public async Task Handle(ApontamentoFimProducaoEvent message)
    {
        if (!message.ApontamentoProducaoEventDto.IsOperacaoRetrabalho || !message.ApontamentoProducaoEventDto.IsUltimoApontamentoOperacao)
        {
            return;
        }

        var operacao =
            await GetOperacaoByNumeroOdfENumeroOperacao(message.ApontamentoProducaoEventDto.NumeroOdf, message.ApontamentoProducaoEventDto.NumeroOperacao);

        if (operacao is null)
        {
            return;
        }
        
        await ChangeStatusOperacao(operacao, StatusProducaoRetrabalho.Encerrada);
    }
    
    public async Task Handle(ApontamentoOperacaoRemovidaEvent message)
    {
        if (!message.ApontamentoProducaoEventEventDto.IsOperacaoRetrabalho)
        {
            return;
        }

        var operacao =
            await GetOperacaoByNumeroOdfENumeroOperacao(message.ApontamentoProducaoEventEventDto.NumeroOdf, message.ApontamentoProducaoEventEventDto.NumeroOperacao);

        if (operacao is null)
        {
            return;
        }
        await ChangeStatusOperacao(operacao, StatusProducaoRetrabalho.Cancelada);
    }
    
    public async Task Handle(ApontamentoEstornadoEvent message)
    {
        var operacao =
            await GetOperacaoByNumeroOdfENumeroOperacao(message.ApontamentoProducaoEventEventDto.NumeroOdf, message.ApontamentoProducaoEventEventDto.NumeroOperacao);

        OperacaoSaldoDto saldoOperacao;
        
        if (operacao is null)
        {
            var ordemRetrabalho = await GetOrdemRetrabalhoByNumeroOdf(message.ApontamentoProducaoEventEventDto.NumeroOdf);
            if (ordemRetrabalho is null)
            {
                return;
            }
            saldoOperacao = await GetOperacaoSaldo(message.ApontamentoProducaoEventEventDto.NumeroOdf,
                message.ApontamentoProducaoEventEventDto.NumeroOperacao);
            
            if (ordemRetrabalho.Quantidade != saldoOperacao.Saldo)
            {
                await ChangeStatusOrdemRetrabalho(ordemRetrabalho, StatusProducaoRetrabalho.Produzindo);
            }
            else
            {
                await ChangeStatusOrdemRetrabalho(ordemRetrabalho, StatusProducaoRetrabalho.Aberta);
            }

            return;
        }
        
        saldoOperacao = await GetOperacaoSaldo(message.ApontamentoProducaoEventEventDto.NumeroOdf,
            message.ApontamentoProducaoEventEventDto.NumeroOperacao);
        
        if (operacao.OperacaoRetrabalhoNaoConformidade.Quantidade != saldoOperacao.Saldo)
        {
            await ChangeStatusOperacao(operacao, StatusProducaoRetrabalho.Produzindo);
        }
        else
        {
            await ChangeStatusOperacao(operacao, StatusProducaoRetrabalho.Aberta);
        }
    }
    
    public async Task Handle(OrdemProducaoEncerradaEvent message)
    {
        if(!message.OrdemProducaoEventEventDto.IsOrdemRetrabalho)
        {
            return;
        }
        var ordemProducao =
            await GetOrdemRetrabalhoByNumeroOdf(message.OrdemProducaoEventEventDto.NumeroOdf);

        if (ordemProducao is null)
        {
            return;
        }

        await ChangeStatusOrdemRetrabalho(ordemProducao, StatusProducaoRetrabalho.Encerrada);
    }
    
    public async Task Handle(OrdemProducaoCanceladaEvent message)
    {
        if(!message.OrdemProducaoEventEventDto.IsOrdemRetrabalho)
        {
            return;
        }
        var ordemProducao =
            await GetOrdemRetrabalhoByNumeroOdf(message.OrdemProducaoEventEventDto.NumeroOdf);

        if (ordemProducao is null)
        {
            return;
        }

        await ChangeStatusOrdemRetrabalho(ordemProducao, StatusProducaoRetrabalho.Cancelada);    
    }


    private async Task<Operacao> GetOperacaoByNumeroOdfENumeroOperacao(int numeroOdf, string numeroOperacao)
    {
        var operacao = await _operacoes
            .Include(e => e.OperacaoRetrabalhoNaoConformidade)
            .Include(e => e.OperacaoRetrabalhoNaoConformidade.NaoConformidade)
            .FirstOrDefaultAsync(e =>
                e.OperacaoRetrabalhoNaoConformidade.NaoConformidade.NumeroOdf == numeroOdf
                && e.NumeroOperacao == numeroOperacao);
        
        return operacao;
    }

    private async Task ChangeStatusOperacao(Operacao operacao, StatusProducaoRetrabalho novoStatus)
    {
        operacao.ChangeStatus(novoStatus);

        var numeroOdf = operacao.OperacaoRetrabalhoNaoConformidade.NaoConformidade.NumeroOdf;
        
        await _pushNotification.SendCommandAsync(BuscarStatusOperacoesCommand, numeroOdf);
        
        await _operacoes.UpdateAsync(operacao, true);
        
    }
    
    private async Task ChangeStatusOrdemRetrabalho(OrdemRetrabalhoNaoConformidade ordemRetrabalho, StatusProducaoRetrabalho novoStatus)
    {
        ordemRetrabalho.ChangeStatus(novoStatus);

        var numeroOdf = ordemRetrabalho.NumeroOdfRetrabalho;
        
        await _pushNotification.SendCommandAsync(BuscarStatusOrdemRetrabalhoCommand, numeroOdf);

        await _ordemRetrabalhoNaoConformidades.UpdateAsync(ordemRetrabalho, true);
    }

    private async Task<OrdemRetrabalhoNaoConformidade> GetOrdemRetrabalhoByNumeroOdf(int numeroOdfEncerrada)
    {
        var ordemRetrabalho =
            await _ordemRetrabalhoNaoConformidades.FirstOrDefaultAsync(e => e.NumeroOdfRetrabalho == numeroOdfEncerrada);
        return ordemRetrabalho;
    }

    private async Task<OperacaoSaldoDto> GetOperacaoSaldo(int numeroOdf, string numeroOperacao)
    {
        var operacaoApontamento = await _operacaoService.GetByNumeroOdfENumeroOperacao(numeroOdf, numeroOperacao);
        var operacaoSaldo =
            await _operacaoService.GetApontamentoOperacaoByLegacyIdOperacao(operacaoApontamento.IdOperacao);
        return operacaoSaldo.OperacaoSaldo[0];
    }
}