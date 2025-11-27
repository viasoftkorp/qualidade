using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.SolucoesNaoConformidades.Services;

public class SolucaoNaoConformidadeProvider : ISolucaoNaoConformidadeProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;

    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/nao-conformidades";


    public SolucaoNaoConformidadeProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<SolucaoNaoConformidadeOutput> Get(Guid id, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/solucoes/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var solucao = await callBuilder.ResponseCallAsync<SolucaoNaoConformidadeOutput>();
        return solucao;
    }

    public async Task<PagedResultDto<SolucaoNaoConformidadeViewOutput>> GetList(GetListWithDefeitoIdFlagInput input,
        Guid idNaoConformidade, Guid idDefeito, bool usarIdDefeito)
    {
        input.UsarIdDefeito = usarIdDefeito;
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/defeitos/{idDefeito}/solucoes?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var solucao = await callBuilder.ResponseCallAsync<PagedResultDto<SolucaoNaoConformidadeViewOutput>>();
        return solucao;
    }

    public async Task<HttpResponseMessage> Create(SolucaoNaoConformidadeInput input, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/solucoes")
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> Update(Guid id, SolucaoNaoConformidadeInput input, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/solucoes/{id}")
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
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/solucoes/{id}")
            .WithHttpMethod(HttpMethod.Delete)
            .Build();

        await callBuilder.CallAsync<string>();
    }
}