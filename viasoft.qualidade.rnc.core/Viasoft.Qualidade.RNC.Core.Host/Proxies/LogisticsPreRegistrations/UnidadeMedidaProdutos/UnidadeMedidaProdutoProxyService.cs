using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsPreRegistrations.UnidadeMedidaProdutos;

public class UnidadeMedidaProdutoProxyService : BaseProxyService<UnidadeMedidaProdutoOutput>, IUnidadeMedidaProdutoProxyService, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Logistics.PreRegistration";
    private const string BasePath = "/logistics/preregistration/unidades-medida-produto";

    public UnidadeMedidaProdutoProxyService(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }
    
    public override async Task<UnidadeMedidaProdutoOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var medidas = await callBuilder.ResponseCallAsync<UnidadeMedidaProdutoOutput>();
        return medidas;
    }
    
    public override async Task<ListResultDto<UnidadeMedidaProdutoOutput>> GetAll(PagedFilteredAndSortedRequestInput filter)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{filter.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var medidas = await callBuilder.ResponseCallAsync<ListResultDto<UnidadeMedidaProdutoOutput>>();
        return medidas;    
    }


    protected override JsonNetFilterRule GetGetAllAdvancedFilter(object value)
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
}