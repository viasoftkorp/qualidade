using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ReclamacoesNaoConformidades.Services;

public class ReclamacaoNaoConformidadeProvider : IReclamacaoNaoConformidadeProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/nao-conformidades";

    public ReclamacaoNaoConformidadeProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<HttpResponseMessage> Insert(Guid idNaoConformidade, ReclamacaoNaoConformidadeInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/reclamacao")
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> Update(Guid idNaoConformidade, ReclamacaoNaoConformidadeInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/reclamacao")
            .WithHttpMethod(HttpMethod.Put)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<ReclamacaoNaoConformidadeOutput> Get(Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/reclamacao")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var reclamacao = await callBuilder.ResponseCallAsync<ReclamacaoNaoConformidadeOutput>();
        return reclamacao;
    }
}