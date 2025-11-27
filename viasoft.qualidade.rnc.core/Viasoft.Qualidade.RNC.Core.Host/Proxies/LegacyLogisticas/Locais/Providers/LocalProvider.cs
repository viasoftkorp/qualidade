using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Providers;

public class LocalProvider : BaseProxyService<LocalOutput>, ILocalProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Legacy.Logistica";
    private const string BaseEndpoint = "legacy/logistica/locais";
    
    public LocalProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public override async Task<LocalOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var local = await callBuilder.ResponseCallAsync<LocalOutput>();
        return local;
    }

    public async Task<LocalOutput> GetByCode(int code)
    {
        var advancedFilter = new JsonNetFilterRule
        {
            Condition = "AND",
            Rules = new List<JsonNetFilterRule>
            {
                new JsonNetFilterRule()
                {
                    Field = "Codigo",
                    Operator = "equal",
                    Type = "integer",
                    Value = code
                }
            }
        };

        var input = new PagedFilteredAndSortedRequestInput
        {
            AdvancedFilter = JsonConvert.SerializeObject(advancedFilter),
            MaxResultCount = 1,
            SkipCount = 0
        };
        var result = await GetAll(input);
        return result.Items.First();
    }

    public override async Task<ListResultDto<LocalOutput>> GetAll(PagedFilteredAndSortedRequestInput filter)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}?{filter.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var locais = await callBuilder.ResponseCallAsync<PagedResultDto<LocalOutput>>();
        return locais;    
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