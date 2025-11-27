using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyVendas.ItemPedidoVendas.Providers;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LogisticsProducts.Providers;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Vendas.ErpPerson.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Vendas.ErpPerson.Providers;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Providers;

public class OrdemProducaoProviderAclService : IOrdemProducaoProviderAclService, ITransientDependency
{
    private readonly IProdutoProvider _produtoProvider;
    private readonly IPersonProvider _personProvider;
    private readonly IItemPedidoVendaProvider _itemPedidoVendaProvider;
    private const int MaxResultCount = 50;
    public OrdemProducaoProviderAclService(IProdutoProvider produtoProvider, IPersonProvider personProvider, 
        IItemPedidoVendaProvider itemPedidoVendaProvider)
    {
        _produtoProvider = produtoProvider;
        _personProvider = personProvider;
        _itemPedidoVendaProvider = itemPedidoVendaProvider;
    }
    public async Task<List<OrdemProducaoOutput>> ProcessGetList(List<OrdemProducao> ordensProducao)
    {
        var codigosProdutos = ordensProducao.Select(e => e.ProdutoDTO.Codigo)
            .Concat(ordensProducao.Select(e => e.CodigoProdutoFaturamento))
            .Distinct()
            .ToList();
        var produtosDictionary = await GetProdutosDictionary(codigosProdutos);
        var clientesDictionary = await GetClientesDictionary(ordensProducao.ConvertAll(e => e.CodigoCliente));
        
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
            IdProdutoFaturamento = !string.IsNullOrWhiteSpace(ordemProducao.CodigoProdutoFaturamento)
                ? produtosDictionary[ordemProducao.CodigoProdutoFaturamento]
                : null,
            NumeroOdfFaturamento = ordemProducao.OdfFaturamento,
            NumeroLote = ordemProducao.Lote,
            OdfFinalizada = ordemProducao.OrdemFechada || ordemProducao.OrdemEncerrada == "S",
            PossuiPartida = ordemProducao.PossuiPartida,
            IdCliente = !string.IsNullOrWhiteSpace(ordemProducao.CodigoCliente) && ordemProducao.CodigoCliente != "000"
                ? clientesDictionary[ordemProducao.CodigoCliente]
                : null
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
            MaxResultCount = MaxResultCount,
            SkipCount = 0
        };
        var produtosPaginados = await _produtoProvider.GetPagelessProdutosList(filter);
        return produtosPaginados.Items.ToDictionary(e => e.Codigo, e => e.Id);
    }
    private async Task<Dictionary<string, Guid>> GetClientesDictionary(List<string> codigosClientes)
    {
        var skipCount = 0;

        var clientes = new List<PersonOutput>();

        var numeroBuscas = Math.Ceiling((double)codigosClientes.Count / MaxResultCount);

        for (int i = 0; i < numeroBuscas; i++)
        {
            var codigosPaginados = codigosClientes.Skip(skipCount).Take(MaxResultCount).ToList();
            
            var advancedFilter = new JsonNetFilterRule
            {
                Condition = "and",
                Rules = new List<JsonNetFilterRule>
                {
                    new()
                    {
                        Field = "Codigo",
                        Operator = "in",
                        Type = "string",
                        Value = codigosPaginados
                    }
                }
            };
            
            var filter = new GetAllPersonInput
            {
                TipoPessoa = PersonType.Customer,
                SkipCount = 0,
                MaxResultCount = MaxResultCount,
                AdvancedFilter = JsonConvert.SerializeObject(advancedFilter)
            };
            
            var pessoas = await _personProvider.GetAll(filter);
            
            clientes.AddRange(pessoas.Items);
            
            skipCount += MaxResultCount;
        }

        var output = clientes.ToDictionary(e => e.Codigo, e => e.Id);
        return output;  
    }
}