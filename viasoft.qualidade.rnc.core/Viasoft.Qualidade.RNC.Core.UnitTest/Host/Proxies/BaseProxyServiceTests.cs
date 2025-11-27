using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFinanceiros.CentroCustos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFinanceiros.CentroCustos.Providers;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Proxies;

public class BaseProxyServiceTests : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Se for fornecido 1 id, deve realizar 1 busca")]
    public async Task GetAllPaginandoTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var service = GetService(dependencies);

        var ids = new List<Guid>
        {
            TestUtils.ObjectMother.Guids[0]
        };

        var expectedAdvancedFilter = GetAdvancedFilter(ids);
        
        var expectedFilter = new PagedFilteredAndSortedRequestInput
        {
            SkipCount = 0,
            MaxResultCount = 50,
            AdvancedFilter = JsonConvert.SerializeObject(expectedAdvancedFilter)
        };

        dependencies.ApiClientCallBuilder
            .WithServiceName("Korp.Legacy.Financeiro")
            .WithEndpoint($"legacy/financeiro/centros-custo?{expectedFilter.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build()
            .ResponseCallAsync<PagedResultDto<CentroCustoOutput>>()
            .Returns(new PagedResultDto<CentroCustoOutput>
            {
                Items = new List<CentroCustoOutput>
                {
                    GetCentroCustoOutput(0)
                },
                TotalCount = 1
            });
        // Act
        await service.GetAllByIdsPaginando(ids);
        
        // Assert
        dependencies.ApiClientCallBuilder
            .Received(2)
            .WithServiceName("Korp.Legacy.Financeiro")
            .WithEndpoint($"legacy/financeiro/centros-custo?{expectedFilter.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();
    }
    
    [Fact(DisplayName = "Se for fornecido 50 ids, deve realizar 1 busca")]
    public async Task GetAllPaginandoTest2()
    {
        // Arrange
        var dependencies = GetDependencies();
        var service = GetService(dependencies);

        var ids = GetIds(50);

        var expectedAdvancedFilter = GetAdvancedFilter(ids);
        
        var expectedFilter = new PagedFilteredAndSortedRequestInput
        {
            SkipCount = 0,
            MaxResultCount = 50,
            AdvancedFilter = JsonConvert.SerializeObject(expectedAdvancedFilter)
        };

        dependencies.ApiClientCallBuilder
            .WithServiceName("Korp.Legacy.Financeiro")
            .WithEndpoint($"legacy/financeiro/centros-custo?{expectedFilter.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build()
            .ResponseCallAsync<PagedResultDto<CentroCustoOutput>>()
            .Returns(new PagedResultDto<CentroCustoOutput>
            {
                Items = new List<CentroCustoOutput>
                {
                    GetCentroCustoOutput(0)
                },
                TotalCount = 1
            });
        // Act
        await service.GetAllByIdsPaginando(ids);
        
        // Assert
        dependencies.ApiClientCallBuilder
            .Received(2)
            .WithServiceName("Korp.Legacy.Financeiro")
            .WithEndpoint($"legacy/financeiro/centros-custo?{expectedFilter.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();
    }
    
    [Fact(DisplayName = "Se for fornecido 51 ids, deve realizar 2 busca")]
    public async Task GetAllPaginandoTest3()
    {
        // Arrange
        var dependencies = GetDependencies();
        var service = GetService(dependencies);

        var ids = GetIds(51);

        var expectedAdvancedFilterPrimeiraBusca = GetAdvancedFilter(ids.Skip(0).Take(50).ToList());
        var expectedAdvancedFilterSegundaBusca = GetAdvancedFilter(ids.Skip(50).Take(50).ToList());
        
        var expectedFilterPrimeiraBusca = new PagedFilteredAndSortedRequestInput
        {
            SkipCount = 0,
            MaxResultCount = 50,
            AdvancedFilter = JsonConvert.SerializeObject(expectedAdvancedFilterPrimeiraBusca)
        };
        var expectedFilterSegundaBusca = new PagedFilteredAndSortedRequestInput
        {
            SkipCount = 0,
            MaxResultCount = 50,
            AdvancedFilter = JsonConvert.SerializeObject(expectedAdvancedFilterSegundaBusca)
        };

        dependencies.ApiClientCallBuilder
            .WithServiceName("Korp.Legacy.Financeiro")
            .WithEndpoint($"legacy/financeiro/centros-custo?{expectedFilterPrimeiraBusca.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build()
            .ResponseCallAsync<PagedResultDto<CentroCustoOutput>>()
            .Returns(new PagedResultDto<CentroCustoOutput>
            {
                Items = new List<CentroCustoOutput>
                {
                    GetCentroCustoOutput(0)
                },
                TotalCount = 1
            });
        dependencies.ApiClientCallBuilder
            .WithServiceName("Korp.Legacy.Financeiro")
            .WithEndpoint($"legacy/financeiro/centros-custo?{expectedFilterSegundaBusca.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build()
            .ResponseCallAsync<PagedResultDto<CentroCustoOutput>>()
            .Returns(new PagedResultDto<CentroCustoOutput>
            {
                Items = new List<CentroCustoOutput>
                {
                    GetCentroCustoOutput(0)
                },
                TotalCount = 1
            });
        // Act
        await service.GetAllByIdsPaginando(ids);
        
        // Assert
       await dependencies.ApiClientCallBuilder
            .WithServiceName("Korp.Legacy.Financeiro")
            .WithEndpoint($"legacy/financeiro/centros-custo?{expectedFilterPrimeiraBusca.ToHttpGetQueryParameter()}")
            .Received(2)
            .WithHttpMethod(HttpMethod.Get)
            .Build()
            .ResponseCallAsync<PagedResultDto<CentroCustoOutput>>();
        await dependencies.ApiClientCallBuilder
            .Received(5)
            .WithServiceName("Korp.Legacy.Financeiro")
            .WithEndpoint($"legacy/financeiro/centros-custo?{expectedFilterSegundaBusca.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build()
            .ResponseCallAsync<PagedResultDto<CentroCustoOutput>>();
    }
    
    [Fact(DisplayName = "Se buscadas entidades, deve retorna-las numa lista")]
    public async Task GetAllPaginandoTest4()
    {
        // Arrange
        var dependencies = GetDependencies();
        var service = GetService(dependencies);

        var ids = GetIds(51);

        var expectedAdvancedFilterPrimeiraBusca = GetAdvancedFilter(ids.Skip(0).Take(50).ToList());
        var expectedAdvancedFilterSegundaBusca = GetAdvancedFilter(ids.Skip(50).Take(50).ToList());
        
        var expectedFilterPrimeiraBusca = new PagedFilteredAndSortedRequestInput
        {
            SkipCount = 0,
            MaxResultCount = 50,
            AdvancedFilter = JsonConvert.SerializeObject(expectedAdvancedFilterPrimeiraBusca)
        };
        var expectedFilterSegundaBusca = new PagedFilteredAndSortedRequestInput
        {
            SkipCount = 0,
            MaxResultCount = 50,
            AdvancedFilter = JsonConvert.SerializeObject(expectedAdvancedFilterSegundaBusca)
        };

        dependencies.ApiClientCallBuilder
            .WithServiceName("Korp.Legacy.Financeiro")
            .WithEndpoint($"legacy/financeiro/centros-custo?{expectedFilterPrimeiraBusca.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build()
            .ResponseCallAsync<PagedResultDto<CentroCustoOutput>>()
            .Returns(new PagedResultDto<CentroCustoOutput>
            {
                Items = new List<CentroCustoOutput>
                {
                    GetCentroCustoOutput(0)
                },
                TotalCount = 1
            });
        dependencies.ApiClientCallBuilder
            .WithServiceName("Korp.Legacy.Financeiro")
            .WithEndpoint($"legacy/financeiro/centros-custo?{expectedFilterSegundaBusca.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build()
            .ResponseCallAsync<PagedResultDto<CentroCustoOutput>>()
            .Returns(new PagedResultDto<CentroCustoOutput>
            {
                Items = new List<CentroCustoOutput>
                {
                    GetCentroCustoOutput(1)
                },
                TotalCount = 1
            });

        var expectedResult = new List<CentroCustoOutput>
        {
            GetCentroCustoOutput(0),
            GetCentroCustoOutput(1)

        };
        // Act
        var result = await service.GetAllByIdsPaginando(ids);
        
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    private List<Guid> GetIds(int countOfIds)
    {
        var ids = new List<Guid>();
        for (int i = 0; i < countOfIds; i++)
        {
            ids.Add(Guid.NewGuid());
        }

        return ids;
    }

    private JsonNetFilterRule GetAdvancedFilter(List<Guid> value)
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
                    Value = value
                }
            }
        };
        return advancedFilter;
    }

    private CentroCustoOutput GetCentroCustoOutput(int index)
    {
        var centroCustoOutput = new CentroCustoOutput
        {
            Id = TestUtils.ObjectMother.Guids[index],
            Codigo = TestUtils.ObjectMother.Ints[index].ToString(),
            Descricao = TestUtils.ObjectMother.Strings[index],
            IsSintetico = false
        };
        return centroCustoOutput;
    }
    private class Dependencies
    {
        public IApiClientCallBuilder ApiClientCallBuilder { get; set; }
    }

    private Dependencies GetDependencies()
    {
        var dependencies = new Dependencies
        {
            ApiClientCallBuilder = Substitute.For<IApiClientCallBuilder>()
        };
        return dependencies;
    }

    private CentroCustoProvider GetService(Dependencies dependencies)
    {
        var service = new CentroCustoProvider(dependencies.ApiClientCallBuilder);
        return service;
    }
}