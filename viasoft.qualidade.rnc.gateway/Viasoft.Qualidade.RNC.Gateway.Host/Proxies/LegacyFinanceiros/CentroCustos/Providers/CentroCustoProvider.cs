using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFinanceiros.CentroCustos.Providers;

public class CentroCustoProvider: ICentroCustoProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Korp.Legacy.Financeiro";
    private const string BasePath = "legacy/financeiro/centros-custo";
    
    public CentroCustoProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }
    public async Task<HttpResponseMessage> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var result = await callBuilder.CallAsync();
        return result.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var result = await callBuilder.CallAsync();
        return result.HttpResponseMessage;
    }
}