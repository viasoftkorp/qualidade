using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ConclusoesNaoConformidades.Services;

public class ConclusaoNaoConformidadeProvider : IConclusaoNaoConformidadeProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/nao-conformidades";

    public ConclusaoNaoConformidadeProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<HttpResponseMessage> ConcluirNaoConformidade(Guid idNaoConformidade,
        ConclusaoNaoConformidadeInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/conclusao")
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }
    public async Task<HttpResponseMessage> Estornar(Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/conclusao")
            .WithHttpMethod(HttpMethod.Delete)
            .Build();

        var response = await callBuilder.CallAsync();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> CalcularCicloTempo(Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/conclusao/calcular-ciclo-tempo")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<ConclusaoNaoConformidadeOutput> Get(Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/conclusao")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var conclusao = await callBuilder.ResponseCallAsync<ConclusaoNaoConformidadeOutput>();
        return conclusao;
    }
}