using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Causas.Services;

public class CausaProvider : ICausaProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;

    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/causas";

    public CausaProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<CausaOutput> Get(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var causa = await callBuilder.ResponseCallAsync<CausaOutput>();
        return causa;
    }

    public async Task<PagedResultDto<CausaOutput>> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var causa = await callBuilder.ResponseCallAsync<PagedResultDto<CausaOutput>>();
        return causa;
    }
    public async Task<HttpResponseMessage> GetViewList(PagedFilteredAndSortedRequestInput input)
    {
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/view?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var response = await callBuilder.CallAsync();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> Create(CausaInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}")
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> Update(Guid id, CausaInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Put)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> Delete(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Delete)
            .Build();

        var response = await callBuilder.CallAsync();
        return response.HttpResponseMessage;
    }
    public async Task<HttpResponseMessage> Inativar(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}/inativacao")
            .WithHttpMethod(HttpMethod.Patch)
            .Build();

        var response = await callBuilder.CallAsync();
        return response.HttpResponseMessage;    
    }

    public async Task<HttpResponseMessage> Ativar(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}/ativacao")
            .WithHttpMethod(HttpMethod.Patch)
            .Build();

        var response = await callBuilder.CallAsync();
        return response.HttpResponseMessage;    
    }
}