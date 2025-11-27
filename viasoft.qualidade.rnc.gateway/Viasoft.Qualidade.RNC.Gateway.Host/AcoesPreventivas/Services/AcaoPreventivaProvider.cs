using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.Authentication.Proxy;
using Viasoft.Core.Authentication.Proxy.Dtos.Inputs;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.AcoesPreventivas.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.AcoesPreventivas.Services;

public class AcaoPreventivaProvider : IAcaoPreventivaProvider, ITransientDependency
{
    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/acoes-preventivas";

    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly IAuthenticationProxyService _authenticationProxyService;
    private readonly IAmbientDataCallOptionsResolver _ambientDataCallOptionsResolver;

    public AcaoPreventivaProvider(IApiClientCallBuilder apiClientCallBuilder, IAuthenticationProxyService authenticationProxyService,
        IAmbientDataCallOptionsResolver ambientDataCallOptionsResolver)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _authenticationProxyService = authenticationProxyService;
        _ambientDataCallOptionsResolver = ambientDataCallOptionsResolver;
    }

    public async Task<AcaoPreventivaOutput> Get(Guid id)
    {
        var call = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var result = await call.ResponseCallAsync<AcaoPreventivaOutput>();

        return result;
    }

    public async Task<HttpResponseMessage> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var queryParameters = input.ToHttpGetQueryParameter();
        var getListCall = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var response = await getListCall.CallAsync();

        return response.HttpResponseMessage;
    }
    
    public async Task<HttpResponseMessage> GetViewList(PagedFilteredAndSortedRequestInput input)
    {
        var queryParameters = input.ToHttpGetQueryParameter();
        var getListCall = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/view?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

       var response = await getListCall.CallAsync();

       return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> Create(AcaoPreventivaInput input)
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

    public async Task<HttpResponseMessage> Update(Guid id, AcaoPreventivaInput input)
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