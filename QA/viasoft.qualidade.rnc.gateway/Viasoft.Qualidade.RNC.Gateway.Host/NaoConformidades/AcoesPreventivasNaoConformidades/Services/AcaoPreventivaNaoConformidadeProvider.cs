using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;

public class AcaoPreventivaNaoConformidadeProvider : IAcaoPreventivaNaoConformidadeProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/nao-conformidades";

    public AcaoPreventivaNaoConformidadeProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<AcaoPreventivaNaoConformidadeViewOutput> Get(Guid idNaoConformidade, Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/acoes-preventivas/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var acao = await callBuilder.ResponseCallAsync<AcaoPreventivaNaoConformidadeViewOutput>();
        return acao;
    }

    public async Task<PagedResultDto<AcaoPreventivaNaoConformidadeViewOutput>> GetList(Guid idNaoConformidade,
        Guid idDefeitoNaoConformidade, GetListWithDefeitoIdFlagInput input, bool usarIdDefeito)
    {
        input.UsarIdDefeito = usarIdDefeito;
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint(
                $"{BasePath}/{idNaoConformidade}/defeitos/{idDefeitoNaoConformidade}/acoes-preventivas?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var acao = await callBuilder.ResponseCallAsync<PagedResultDto<AcaoPreventivaNaoConformidadeViewOutput>>();
        return acao;
    }

    public async Task<HttpResponseMessage> Create(Guid idNaoConformidade, AcaoPreventivaNaoConformidadeInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/acoes-preventivas")
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> Update(Guid idNaoConformidade, Guid id,
        AcaoPreventivaNaoConformidadeInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/acoes-preventivas/{id}")
            .WithHttpMethod(HttpMethod.Put)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task Delete(Guid idNaoConformidade, Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/acoes-preventivas/{id}")
            .WithHttpMethod(HttpMethod.Delete)
            .Build();

        var response = await callBuilder.CallAsync<string>();
    }
}