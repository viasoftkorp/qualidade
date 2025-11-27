using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.Locais.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.Locais.Providers;

public class LocalProvider : ILocalProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Legacy.Logistica";
    private const string BaseEndpoint = "legacy/logistica/locais";
    
    public LocalProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }
    public async Task<PagedResultDto<LocalOutput>> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var locais = await callBuilder.ResponseCallAsync<PagedResultDto<LocalOutput>>();
        return locais;
    }

    public async Task<LocalOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var local = await callBuilder.ResponseCallAsync<LocalOutput>();
        return local;
    }
}