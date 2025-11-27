using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.Produtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;

public class OrdemProducaoProviderAclService : IOrdemProducaoProviderAclService, ITransientDependency
{
    private readonly IProdutosProxyService _produtosProxyService;
    public OrdemProducaoProviderAclService(IProdutosProxyService produtosProxyService)
    {
        _produtosProxyService = produtosProxyService;
    }
    public async Task<List<OrdemProducaoOutput>> ProcessGetList(List<OrdemProducao> ordensProducao)
    {
        var produtosDictionary = await GetProdutosDictionary(ordensProducao.ConvertAll(e => e.ProdutoDTO.Codigo));

        var result = ordensProducao.Select(ordemProducao => new OrdemProducaoOutput
        {
            NumeroOdf = ordemProducao.Odf,
            NumeroPedido = ordemProducao.NumeroPedido,
            IdProduto = produtosDictionary[ordemProducao.ProdutoDTO.Codigo],
            IsRetrabalho = ordemProducao.Retrabalho,
            Revisao = ordemProducao.Revisao,
            Observacao = ordemProducao.Observacao,
            DataEntrega = DateTime.ParseExact(ordemProducao.DataEntrega, "dd.MM.yyyy", new CultureInfo("pt-BR")),
            DataInicio = string.IsNullOrWhiteSpace(ordemProducao.DataInicio)
                ? null
                : DateTime.ParseExact(ordemProducao.DataInicio, "dd.MM.yyyy", new CultureInfo("pt-BR")),
            Quantidade = ordemProducao.QuantidadeOrdem,
            NumeroOdfDestino = ordemProducao.OdfDestino,
            OdfFinalizada = ordemProducao.OrdemFechada || ordemProducao.OrdemEncerrada == "S"
        }).ToList();
        
        return result;

    }

    private async Task<Dictionary<string, Guid>> GetProdutosDictionary(List<string> codigosProduto)
    {
        var advancedFilter = new JsonNetFilterRule
        {
            Condition = "AND",
            Rules = new List<JsonNetFilterRule>
            {
                new JsonNetFilterRule()
                {
                    Field = "Code",
                    Operator = "in",
                    Type = "string",
                    Value = codigosProduto
                }
            }
        };

        var filter = new PagedFilteredAndSortedRequestInput
        {
            AdvancedFilter = JsonConvert.SerializeObject(advancedFilter),
            MaxResultCount = 50,
            SkipCount = 0
        };
        var produtosPaginados = await _produtosProxyService.GetAll(filter);
        
        var itensDictionary = produtosPaginados.Items.ToDictionary(e => e.Codigo, e => e.Id);
        
        return itensDictionary;
    }
}