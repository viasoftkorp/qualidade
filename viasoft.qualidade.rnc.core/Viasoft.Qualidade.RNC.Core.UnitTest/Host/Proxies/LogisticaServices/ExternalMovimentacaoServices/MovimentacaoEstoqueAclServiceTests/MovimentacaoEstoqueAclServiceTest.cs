using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Domain.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Services;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.
    MovimentacaoEstoqueAclServiceTests;

public abstract class MovimentacaoEstoqueAclServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    protected LocalOutput GetLocalOutput(int index)
    {
        var localOutput = new LocalOutput
        {
            Id = TestUtils.ObjectMother.Guids[index],
            Codigo = TestUtils.ObjectMother.Ints[index],
            Descricao = TestUtils.ObjectMother.Strings[index],
            IsBloquearMovimentacao = true
        };
        return localOutput;
    }

    protected ExternalMovimentarEstoqueListaInput GetExternalMovimentarEstoqueListaInput(int index)
    {
        var externalMovimentarEstoqueListaInput = new ExternalMovimentarEstoqueListaInput
        {
            Itens = new List<ExternalMovimentarEstoqueItemInput>
            {
                new ExternalMovimentarEstoqueItemInput
                {
                    Lotes = new List<ExternalMovimentarEstoqueLoteInput>
                    {
                        new ExternalMovimentarEstoqueLoteInput()
                        {
                            Documento = $"Transferência de estoque do local 1 para o local 43",
                            Quantidade = TestUtils.ObjectMother.Ints[index],
                            DataFabricacao = TestUtils.ObjectMother.Datas[index].AddDateMask(),
                            PedidoVendaDestino = TestUtils.ObjectMother.Ints[index].ToString(),
                            PedidoVendaOrigem = TestUtils.ObjectMother.Ints[index].ToString(),
                            CodigoProduto = TestUtils.ObjectMother.Ints[index].ToString(),
                            OdfOrigem = TestUtils.ObjectMother.Ints[index],
                            OdfDestino = TestUtils.ObjectMother.Ints[index],
                            DataValidade = TestUtils.ObjectMother.Datas[index].AddDateMask(),
                            LoteOrigem = TestUtils.ObjectMother.Ints[index].ToString(),
                            LoteDestino = TestUtils.ObjectMother.Ints[index].ToString(),
                            CodigoLocalOrigem = TestUtils.ObjectMother.Ints[index],
                            CodigoLocalDestino = TestUtils.ObjectMother.Ints[index],
                            IdEmpresa = TestUtils.ObjectMother.Ints[index],
                            PesoBruto = 0,
                            PesoLiquido = 0,
                            CodigoArmazemDestino = TestUtils.ObjectMother.Ints[index].ToString(),
                            CodigoArmazemOrigem = TestUtils.ObjectMother.Ints[index].ToString(),
                            CodigoDeBarras = "",
                            IdEstoqueLocal = null,
                            PickingItemLoteId = "",
                            TransferindoParaLocalRetrabalho = true
                        }
                    }
                }
            }
        };
        return externalMovimentarEstoqueListaInput;
    }
    protected void MockarRetornoGetLocais(Dependencies dependencies, List<Guid> expectedIdsLocais, 
        PagedResultDto<LocalOutput> expectedOutput)
    {
        var expectedAdvancedFilter = new JsonNetFilterRule
        {
            Condition = "AND",
            Rules = new List<JsonNetFilterRule>
            {
                new JsonNetFilterRule()
                {
                    Field = "Id",
                    Operator = "in",
                    Type = "string",
                    Value = expectedIdsLocais
                }
            }
        };
        var expectedInput = new PagedFilteredAndSortedRequestInput
        {
            AdvancedFilter = JsonConvert.SerializeObject(expectedAdvancedFilter),
            MaxResultCount = 2,
            SkipCount = 0
        };

        dependencies.LocalProvider
            .GetAll(Arg.Is<PagedFilteredAndSortedRequestInput>(e => e.IsEquivalentTo(expectedInput)))
            .Returns(expectedOutput);
    }
    
    protected async Task MockarRetornoGetProdutos(Dependencies dependencies, Produto expectedProduto)
    {
        await dependencies.Produtos.InsertAsync(expectedProduto, true);
    }
    protected class Dependencies
    {
        public ILocalProvider LocalProvider {get;set;}
        public IRepository<Produto> Produtos {get;set;}
        public ICurrentCompany CurrentCompany {get;set;}
    }

    protected Dependencies GetDependencies()
    {
        var dependencies = new Dependencies
        {
            LocalProvider = Substitute.For<ILocalProvider>(),
            Produtos = ServiceProvider.GetService<IRepository<Produto>>(),
            CurrentCompany = TestUtils.ObjectMother.GetCurrentCompany()
        };
        return dependencies;
    }

    protected MovimentacaoEstoqueAclService GetService(Dependencies dependencies)
    {
        var movimentacaoEstoqueAclService = new MovimentacaoEstoqueAclService(dependencies.LocalProvider, dependencies.Produtos, 
            dependencies.CurrentCompany);
        return movimentacaoEstoqueAclService;
    }
}