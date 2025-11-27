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

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Recursos;

public class RecursosProxyService : BaseProxyService<RecursoOutput>, IRecursosProxyService, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Engenharia.Core";
    private const string BasePath = "/engenharia/core/recursos";

    public RecursosProxyService(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public override async Task<ListResultDto<RecursoOutput>> GetAll(PagedFilteredAndSortedRequestInput filter)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{filter.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var output = await callBuilder.ResponseCallAsync<PagedResultDto<RecursoOutput>>();
        return output;
        
    }

    public override async Task<RecursoOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var recurso = await callBuilder.ResponseCallAsync<RecursoOutput>();
        return recurso;
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