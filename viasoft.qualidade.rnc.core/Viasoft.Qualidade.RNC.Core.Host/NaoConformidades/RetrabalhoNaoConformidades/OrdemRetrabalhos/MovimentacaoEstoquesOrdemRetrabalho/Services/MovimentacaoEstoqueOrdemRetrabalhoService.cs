using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyParametros.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Services;

public class MovimentacaoEstoqueOrdemRetrabalhoService : IMovimentacaoEstoqueOrdemRetrabalhoService, ITransientDependency
{
    private readonly ILogger<MovimentacaoEstoqueOrdemRetrabalhoService> _logger;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly ICurrentTenant _currentTenant;
    private readonly IMovimentacaoEstoqueAclService _movimentacaoEstoqueAclService;
    private readonly IOrdemProducaoProvider _ordemProducaoProvider;
    private readonly ILegacyParametrosProvider _legacyParametrosProvider;
    private readonly IExternalMovimentacaoService _externalMovimentacaoService;

    public MovimentacaoEstoqueOrdemRetrabalhoService(ILogger<MovimentacaoEstoqueOrdemRetrabalhoService> logger, 
        ICurrentEnvironment currentEnvironment, ICurrentTenant currentTenant, IMovimentacaoEstoqueAclService movimentacaoEstoqueAclService,
        IOrdemProducaoProvider ordemProducaoProvider, ILegacyParametrosProvider legacyParametrosProvider,
        IExternalMovimentacaoService externalMovimentacaoService)
    {
        _logger = logger;
        _currentEnvironment = currentEnvironment;
        _currentTenant = currentTenant;
        _movimentacaoEstoqueAclService = movimentacaoEstoqueAclService;
        _ordemProducaoProvider = ordemProducaoProvider;
        _legacyParametrosProvider = legacyParametrosProvider;
        _externalMovimentacaoService = externalMovimentacaoService;
    }
    public async Task<MovimentarEstoqueListaOutput> MovimentarEstoqueLista(NaoConformidade naoConformidade, 
        OrdemRetrabalhoNaoConformidade ordemRetrabalhoNaoConformidade)
    {
        var utilizarReservaDePedidoNaLocalizacaoDeEstoque = await _legacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque();

        var numeroOdf = naoConformidade.NumeroOdf;
        if (utilizarReservaDePedidoNaLocalizacaoDeEstoque)
        {
            var ordemProducao = await _ordemProducaoProvider.GetByNumeroOdf(naoConformidade.NumeroOdf.Value, true);
            numeroOdf = ordemProducao.NumeroOdfDestino;
        }
        
        var input = new MovimentarEstoqueListaInput
        {
            Quantidade = ordemRetrabalhoNaoConformidade.Quantidade,
            IdLocalDestino = ordemRetrabalhoNaoConformidade.IdLocalDestino,
            IdLocalOrigem = ordemRetrabalhoNaoConformidade.IdLocalOrigem,
            CodigoArmazem = ordemRetrabalhoNaoConformidade.CodigoArmazem,
            DataFabricacao = ordemRetrabalhoNaoConformidade.DataFabricacao,
            DataValidade = ordemRetrabalhoNaoConformidade.DataValidade,
            IdProduto = naoConformidade.IdProduto,
            NumeroPedido = naoConformidade.NumeroPedido,
            NumeroLote = naoConformidade.NumeroLote,
            NumeroOdfOrigem = numeroOdf.Value,
            NumeroOdfDestino = ordemRetrabalhoNaoConformidade.NumeroOdfRetrabalho
        };
        var externalMovimentarEstoqueListaInput = await _movimentacaoEstoqueAclService.GetExternalMovimentarEstoqueListaInput(input);

        var movimentacaoOutput =
            await _externalMovimentacaoService.MovimentarEstoqueLista(externalMovimentarEstoqueListaInput);

        var result = _movimentacaoEstoqueAclService.GetMovimentarEstoqueListaOutput(movimentacaoOutput);
        
        if (!result.Success)
        {
            _logger.LogError($"Erro ao realizar movimentação de estoque para o tenant {_currentTenant.Id} " +
                             $"e environment {_currentEnvironment.Id}, mensagem de retorno: {result.Message}"); 
        }
        
        return result;
    }
    public async Task<MovimentarEstoqueListaOutput> EstornarMovimentacaoEstoqueLista(NaoConformidade naoConformidade, 
        OrdemRetrabalhoNaoConformidade ordemRetrabalhoNaoConformidade)
    {
        var utilizarReservaDePedidoNaLocalizacaoDeEstoque = await _legacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque();

        var numeroOdf = naoConformidade.NumeroOdf;
        if (utilizarReservaDePedidoNaLocalizacaoDeEstoque)
        {
            var ordemProducao = await _ordemProducaoProvider.GetByNumeroOdf(naoConformidade.NumeroOdf.Value, true);
            numeroOdf = ordemProducao.NumeroOdfDestino;
        }
        
        var input = new MovimentarEstoqueListaInput
        {
            Quantidade = ordemRetrabalhoNaoConformidade.Quantidade,
            //Locais invertidos por ser estorno
            IdLocalDestino = ordemRetrabalhoNaoConformidade.IdLocalOrigem,
            IdLocalOrigem = ordemRetrabalhoNaoConformidade.IdLocalDestino,
            CodigoArmazem = ordemRetrabalhoNaoConformidade.CodigoArmazem,
            DataFabricacao = ordemRetrabalhoNaoConformidade.DataFabricacao,
            DataValidade = ordemRetrabalhoNaoConformidade.DataValidade,
            IdProduto = naoConformidade.IdProduto,
            NumeroPedido = naoConformidade.NumeroPedido,
            NumeroLote = naoConformidade.NumeroLote,
            NumeroOdfOrigem = ordemRetrabalhoNaoConformidade.NumeroOdfRetrabalho,
            NumeroOdfDestino = numeroOdf.Value
        };
        var externalMovimentarEstoqueListaInput = await _movimentacaoEstoqueAclService.GetExternalMovimentarEstoqueListaInput(input);

        var movimentacaoOutput =
            await _externalMovimentacaoService.MovimentarEstoqueLista(externalMovimentarEstoqueListaInput);

        var result = _movimentacaoEstoqueAclService.GetMovimentarEstoqueListaOutput(movimentacaoOutput);
        
        if (!result.Success)
        {
            _logger.LogError($"Erro ao realizar movimentação de estoque para o tenant {_currentTenant.Id} " +
                             $"e environment {_currentEnvironment.Id}, mensagem de retorno: {result.Message}"); 
        }
        
        return result;
    }
}