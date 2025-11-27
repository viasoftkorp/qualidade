using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Host.EstoqueLocais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoqueLocais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoqueLocais.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoquePedidoVendaEstoqueLocais.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyParametros.Providers;

namespace Viasoft.Qualidade.RNC.Core.Host.EstoqueLocais;

public class EstoqueLocalAclService : IEstoqueLocalAclService, ITransientDependency
{
    private readonly ILegacyParametrosProvider _legacyParametrosProvider;
    private readonly IEstoqueLocalProvider _estoqueLocalProvider;
    private readonly IEstoquePedidoVendaEstoqueLocalProvider _estoquePedidoVendaEstoqueLocalProvider;

    public EstoqueLocalAclService(ILegacyParametrosProvider legacyParametrosProvider, IEstoqueLocalProvider estoqueLocalProvider,
        IEstoquePedidoVendaEstoqueLocalProvider estoquePedidoVendaEstoqueLocalProvider)
    {
        _legacyParametrosProvider = legacyParametrosProvider;
        _estoqueLocalProvider = estoqueLocalProvider;
        _estoquePedidoVendaEstoqueLocalProvider = estoquePedidoVendaEstoqueLocalProvider;
    }
    public async Task<PagedResultDto<EstoqueLocalOutput>> GetList(GetListEstoqueLocalInput input)
    {
        var utilizarReservaDePedidoNaLocalizacaoDeEstoque = await _legacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque();
        if (utilizarReservaDePedidoNaLocalizacaoDeEstoque)
        {
            var estoques = await _estoquePedidoVendaEstoqueLocalProvider.GetList(input);
            var estoquesLocaisDictionary = await GetEstoqueLocaisByIds(estoques.Items.ConvertAll(e => e.IdEstoqueLocal));
            
            var output = new PagedResultDto<EstoqueLocalOutput>
            {
                Items = estoques.Items.Select(e =>
                {
                    var estoqueLocal = estoquesLocaisDictionary[e.IdEstoqueLocal];
                    return new EstoqueLocalOutput
                    {
                        Id = e.Id,
                        Quantidade = e.Quantidade,
                        CodigoLocal = estoqueLocal.CodigoLocal,
                        DataFabricacao = estoqueLocal.DataFabricacao,
                        DataValidade = estoqueLocal.DataValidade,
                        CodigoArmazem = estoqueLocal.CodigoArmazem,
                        IdLocal = estoqueLocal.IdLocal
                    };
                }).ToList(),
                TotalCount = estoques.TotalCount
            };
            return output;
        }
        else
        {
            var estoques = await _estoqueLocalProvider.GetList(input);
            var output = new PagedResultDto<EstoqueLocalOutput>
            {
                Items = estoques.Items.Select(e => new EstoqueLocalOutput
                {
                    Id = e.Id,
                    Quantidade = e.Quantidade,
                    CodigoLocal = e.CodigoLocal,
                    DataFabricacao = e.DataFabricacao,
                    DataValidade = e.DataValidade,
                    CodigoArmazem = e.CodigoArmazem,
                    IdLocal = e.IdLocal
                }).ToList(),
                TotalCount = estoques.TotalCount
            };
            return output;
        }
    }

    public async Task<EstoqueLocalOutput> GetById(Guid id)
    {
        var utilizarReservaDePedidoNaLocalizacaoDeEstoque = await _legacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque();
        if (utilizarReservaDePedidoNaLocalizacaoDeEstoque)
        {
            var estoquePedidoVendaEstoqueLocal = await _estoquePedidoVendaEstoqueLocalProvider.GetById(id);

            var estoqueLocal = await _estoqueLocalProvider.GetById(estoquePedidoVendaEstoqueLocal.IdEstoqueLocal);
            
            var output = new EstoqueLocalOutput
            {
                Id = estoquePedidoVendaEstoqueLocal.Id,
                Quantidade = estoquePedidoVendaEstoqueLocal.Quantidade,
                CodigoLocal = estoqueLocal.CodigoLocal,
                DataFabricacao = estoqueLocal.DataFabricacao,
                DataValidade = estoqueLocal.DataValidade,
                CodigoArmazem = estoqueLocal.CodigoArmazem,
                IdLocal = estoqueLocal.IdLocal
            };
            return output;
        }
        else
        {
            var estoque = await _estoqueLocalProvider.GetById(id);
            var output = new EstoqueLocalOutput
            {
                Id = estoque.Id,
                Quantidade = estoque.Quantidade,
                CodigoLocal = estoque.CodigoLocal,
                DataFabricacao = estoque.DataFabricacao,
                DataValidade = estoque.DataValidade,
                CodigoArmazem = estoque.CodigoArmazem,
                IdLocal = estoque.IdLocal
            };
            return output;
        }
    }

    private async Task<Dictionary<Guid, EstoqueLocal>> GetEstoqueLocaisByIds(List<Guid> idsEstoques)
    {
        var maxResultCount = 50;
        var skipCount = 0;
        var itens = new List<EstoqueLocal>();
        long totalCount;

        var advancedFilter = new JsonNetFilterRule
        {
            Condition = "and",
            Rules = new List<JsonNetFilterRule>
            {
                new()
                {
                    Field = "Id",
                    Operator = "in",
                    Type = "string",
                    Value = idsEstoques
                }
            }
        };
        do
        {
            var filter = new GetListEstoqueLocalInput
            {
                AdvancedFilter = JsonConvert.SerializeObject(advancedFilter),
                MaxResultCount = maxResultCount,
                SkipCount = skipCount
            };
            var estoques = await _estoqueLocalProvider.GetList(filter);

            totalCount = estoques.TotalCount;
            skipCount += maxResultCount;
            
            itens.AddRange(estoques.Items);
        } while (totalCount > itens.Count);

        return itens.ToDictionary(e => e.Id, e => e);
    }
}