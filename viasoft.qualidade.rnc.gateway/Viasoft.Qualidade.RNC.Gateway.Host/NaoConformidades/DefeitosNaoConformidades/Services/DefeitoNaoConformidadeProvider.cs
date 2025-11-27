using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Services;

public class DefeitoNaoConformidadeProvider : IDefeitoNaoConformidadeProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;

    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/nao-conformidades";


    public DefeitoNaoConformidadeProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<DefeitoNaoConformidadeOutput> Get(Guid id, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/defeitos/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var defeito = await callBuilder.ResponseCallAsync<DefeitoNaoConformidadeOutput>();
        return defeito;
    }

    public async Task<PagedResultDto<DefeitoNaoConformidadeViewOutput>> GetList(PagedFilteredAndSortedRequestInput input,
        Guid idNaoConformidade)
    {
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/defeitos?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var defeito = await callBuilder.ResponseCallAsync<PagedResultDto<DefeitoNaoConformidadeViewOutput>>();
        return defeito;
    }

    public async Task<HttpResponseMessage> Create(DefeitoNaoConformidadeInput input, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/defeitos")
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> Update(Guid id, DefeitoNaoConformidadeInput input, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/defeitos/{id}")
            .WithHttpMethod(HttpMethod.Put)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task Delete(Guid id, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/defeitos/{id}")
            .WithHttpMethod(HttpMethod.Delete)
            .Build();

        await callBuilder.CallAsync<string>();
    }
}