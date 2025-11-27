using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Domain.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Services;

public class MovimentacaoEstoqueAclService: IMovimentacaoEstoqueAclService, ITransientDependency
{
    private readonly ILocalProvider _localProvider;
    private readonly IRepository<Produto> _produtos;
    private readonly ICurrentCompany _currentCompany;

    public MovimentacaoEstoqueAclService(ILocalProvider localProvider, IRepository<Produto> produtos, 
        ICurrentCompany currentCompany)
    {
        _localProvider = localProvider;
        _produtos = produtos;
        _currentCompany = currentCompany;
    }
    public async Task<ExternalMovimentarEstoqueListaInput> GetExternalMovimentarEstoqueListaInput(MovimentarEstoqueListaInput input)
    {
        var locais = await GetLocais(new List<Guid> { input.IdLocalOrigem, input.IdLocalDestino });

        var localDestino = locais.Find(e => e.Id == input.IdLocalDestino);
        var localOrigem = locais.Find(e => e.Id == input.IdLocalOrigem);
        
        var dataFabricacao = input.DataFabricacao.HasValue
            ? input.DataFabricacao.Value.AddDateMask()
            : "";
        
        var dataValidade = input.DataValidade.HasValue
            ? input.DataValidade.Value.AddDateMask()
            : "";
        
        var numeroPedido =
            ErpNumeroPedidoConventions.GetNumeroPedido(input.NumeroPedido, true);

        var codigoProduto = await GetCodigoProduto(input.IdProduto);
        
        var externalInput = new ExternalMovimentarEstoqueListaInput
        {
            Itens = new List<ExternalMovimentarEstoqueItemInput>
            {
                new ExternalMovimentarEstoqueItemInput
                {
                    Lotes = new List<ExternalMovimentarEstoqueLoteInput>
                    {
                        new ExternalMovimentarEstoqueLoteInput()
                        {
                            Documento = $"Transferência estoque, local {localOrigem.Codigo} para local {localDestino.Codigo}",
                            Quantidade = input.Quantidade,
                            DataFabricacao = dataFabricacao,
                            PedidoVendaDestino = numeroPedido,
                            PedidoVendaOrigem = numeroPedido,
                            CodigoProduto = codigoProduto,
                            OdfOrigem = input.NumeroOdfOrigem,
                            OdfDestino = input.NumeroOdfDestino,
                            DataValidade = dataValidade,
                            LoteOrigem = input.NumeroLote,
                            LoteDestino = input.NumeroLote,
                            CodigoLocalOrigem = localOrigem.Codigo,
                            CodigoLocalDestino = localDestino.Codigo,
                            IdEmpresa = _currentCompany.LegacyId,
                            PesoBruto = 0,
                            PesoLiquido = 0,
                            CodigoArmazemDestino = input.CodigoArmazem,
                            CodigoArmazemOrigem = input.CodigoArmazem,
                            CodigoDeBarras = "",
                            IdEstoqueLocal = null,
                            PickingItemLoteId = "",
                            TransferindoParaLocalRetrabalho = true
                        }
                    }
                }
            }
        };

        return externalInput;
    }

    public MovimentarEstoqueListaOutput GetMovimentarEstoqueListaOutput(ExternalMovimentarEstoqueItemOutput input)
    {
        var output = new MovimentarEstoqueListaOutput
        {
            Success = input.Error == null,
            Message = input.Error != null ? input.Error.message : "",
            DtoRetorno = input.Resultado?.First().Resultados.First()
        };
        
        return output;
    }

    private async Task<List<LocalOutput>> GetLocais(List<Guid> idsLocais)
    {
        var advancedFilter = new JsonNetFilterRule
        {
            Condition = "AND",
            Rules = new List<JsonNetFilterRule>
            {
                new JsonNetFilterRule()
                {
                    Field = "Id",
                    Operator = "in",
                    Type = "string",
                    Value = idsLocais
                }
            }
        };
        var input = new PagedFilteredAndSortedRequestInput
        {
            AdvancedFilter = JsonConvert.SerializeObject(advancedFilter),
            MaxResultCount = 2,
            SkipCount = 0
        };
        var result = await _localProvider.GetAll(input);
        return result.Items;
    }
    private async Task<string> GetCodigoProduto(Guid idProduto)
    {
        var produto = await _produtos.FirstAsync(e=> e.Id == idProduto);
        return produto.Codigo;
    }
}