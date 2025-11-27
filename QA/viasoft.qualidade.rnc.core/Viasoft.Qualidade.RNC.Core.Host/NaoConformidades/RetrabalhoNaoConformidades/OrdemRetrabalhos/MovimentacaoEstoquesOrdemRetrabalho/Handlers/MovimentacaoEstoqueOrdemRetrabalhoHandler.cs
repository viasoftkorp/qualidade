using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.PushNotifications.Abstractions.Contracts;
using Viasoft.PushNotifications.Abstractions.Notification;
using Viasoft.Qualidade.RNC.Core.Domain.MovimentacaoEstoques.UpdateNotifications;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Commands;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoqueLocais.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Handlers;

public class MovimentacaoEstoqueOrdemRetrabalhoHandler : IHandleMessages<MovimentarEstoqueItemMessage>
{
    private readonly IMovimentacaoEstoqueOrdemRetrabalhoService _movimentacaoEstoqueOrdemRetrabalhoService;
    private readonly IRepository<OrdemRetrabalhoNaoConformidade> _ordemRetrabalhoNaoConformidades;
    private readonly IOrdemRetrabalhoService _ordemRetrabalhoService;
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IPushNotification _pushNotification;
    private readonly IEstoqueLocalProvider _estoqueLocalProvider;

    public MovimentacaoEstoqueOrdemRetrabalhoHandler(IMovimentacaoEstoqueOrdemRetrabalhoService movimentacaoEstoqueOrdemRetrabalhoService,
        IRepository<OrdemRetrabalhoNaoConformidade> ordemRetrabalhoNaoConformidades,
        IOrdemRetrabalhoService ordemRetrabalhoService, INaoConformidadeRepository naoConformidadeRepository,
        IPushNotification pushNotification, IEstoqueLocalProvider estoqueLocalProvider)
    {
        _movimentacaoEstoqueOrdemRetrabalhoService = movimentacaoEstoqueOrdemRetrabalhoService;
        _ordemRetrabalhoNaoConformidades = ordemRetrabalhoNaoConformidades;
        _ordemRetrabalhoService = ordemRetrabalhoService;
        _naoConformidadeRepository = naoConformidadeRepository;
        _pushNotification = pushNotification;
        _estoqueLocalProvider = estoqueLocalProvider;
    }
    public async Task Handle(MovimentarEstoqueItemMessage message)
    {
        if (message.IsEstorno)
        {
            await MovimentarEstoqueEstorno(message);
            return;
        }
        var agregacaoNaoConformidade = await _naoConformidadeRepository.Get(message.IdNaoConformidade);

        var ordemRetrabalhoNaoConformidade =
            await _ordemRetrabalhoNaoConformidades.FirstAsync(e => e.IdNaoConformidade == message.IdNaoConformidade);
        
        var movimentacaoSaldoResult =
            await _movimentacaoEstoqueOrdemRetrabalhoService.MovimentarEstoqueLista(agregacaoNaoConformidade.NaoConformidade, ordemRetrabalhoNaoConformidade);
        
        await SalvarRetorno(ordemRetrabalhoNaoConformidade, movimentacaoSaldoResult);
        
        await _ordemRetrabalhoNaoConformidades.UpdateAsync(ordemRetrabalhoNaoConformidade, true);

        var movimentacaoEstoqueProcessadaUpdateNotification = new MovimentacaoEstoqueProcessadaUpdateNotification
        {
            IdNaoConformidade = message.IdNaoConformidade,
            Success = movimentacaoSaldoResult.Success,
        };
        
        if (!movimentacaoSaldoResult.Success)
        {            
            movimentacaoEstoqueProcessadaUpdateNotification.Message = movimentacaoSaldoResult.Message;

            var estornoResult = await _ordemRetrabalhoService.EstornarOrdemRetrabalho(agregacaoNaoConformidade, ordemRetrabalhoNaoConformidade, false);
            if (estornoResult.Success)
            {
                await _ordemRetrabalhoNaoConformidades.DeleteAsync(ordemRetrabalhoNaoConformidade, true);
            }
            await _pushNotification.SendAsync(new Payload
            {
                Header = "Erro ao realizar movimentação de estoque, odf de retrabalho estornada.",
                Body = movimentacaoSaldoResult.Message
            }, agregacaoNaoConformidade.NaoConformidade.CreatorId.Value, true);
        }
        await _pushNotification.SendUpdateAsync(movimentacaoEstoqueProcessadaUpdateNotification);
    }

    private async Task MovimentarEstoqueEstorno(MovimentarEstoqueItemMessage message)
    {
        var agregacaoNaoConformidade = await _naoConformidadeRepository.Get(message.IdNaoConformidade);
        
        var ordemRetrabalhoNaoConformidade =
            await _ordemRetrabalhoNaoConformidades.FirstAsync(e => e.IdNaoConformidade == message.IdNaoConformidade);
        
        var movimentacaoSaldoResult =
            await _movimentacaoEstoqueOrdemRetrabalhoService.EstornarMovimentacaoEstoqueLista(agregacaoNaoConformidade.NaoConformidade, ordemRetrabalhoNaoConformidade);

        var movimentacaoEstoqueProcessadaUpdateNotification = new MovimentacaoEstoqueProcessadaUpdateNotification
        {
            IdNaoConformidade = message.IdNaoConformidade,
            Success = movimentacaoSaldoResult.Success,
        };
        
        if (movimentacaoSaldoResult.Success)
        {
            await _ordemRetrabalhoNaoConformidades.DeleteAsync(ordemRetrabalhoNaoConformidade, true);
        }
        else
        {
            movimentacaoEstoqueProcessadaUpdateNotification.Message = movimentacaoSaldoResult.Message;
            
            var payload = new Payload
            {
                Header = $"Erro ao realizar movimentação de estoque",
                Body = movimentacaoSaldoResult.Message
            };
            await _pushNotification.SendAsync(payload, agregacaoNaoConformidade.NaoConformidade.CreatorId.Value, true);
        }
        await _pushNotification.SendUpdateAsync(movimentacaoEstoqueProcessadaUpdateNotification);
    }

    private async Task SalvarRetorno(OrdemRetrabalhoNaoConformidade ordemRetrabalhoNaoConformidade, MovimentarEstoqueListaOutput retorno)
    {

        if (retorno.Success)
        {
            var estoqueLocalDestino = await _estoqueLocalProvider.GetByLegacyId(retorno.DtoRetorno.LegacyIdEstoqueLocalDestino);
            ordemRetrabalhoNaoConformidade.IdEstoqueLocalDestino = estoqueLocalDestino.Id;
        }
        else
        {
            ordemRetrabalhoNaoConformidade.MovimentacaoEstoqueMensagemRetorno = retorno.Message;
        }
    }
}