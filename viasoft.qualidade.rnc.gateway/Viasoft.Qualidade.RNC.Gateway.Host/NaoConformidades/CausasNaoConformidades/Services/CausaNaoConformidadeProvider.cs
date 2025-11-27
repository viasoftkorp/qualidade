using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Services;

public class CausaNaoConformidadeProvider : ICausaNaoConformidadeProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;

    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/nao-conformidades";


    public CausaNaoConformidadeProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<CausaNaoConformidadeOutput> Get(Guid id, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/causas/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var causa = await callBuilder.ResponseCallAsync<CausaNaoConformidadeOutput>();
        return causa;
    }

    public async Task<PagedResultDto<CausaNaoConformidadeViewOutput>> GetList(GetListWithDefeitoIdFlagInput input,
        Guid idNaoConformidade, Guid idDefeito, bool usarIdDefeito)
    {
        input.UsarIdDefeito = usarIdDefeito;
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/defeitos/{idDefeito}/causas?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var causa = await callBuilder.ResponseCallAsync<PagedResultDto<CausaNaoConformidadeViewOutput>>();
        return causa;
    }

    public async Task<HttpResponseMessage> Create(CausaNaoConformidadeInput input, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/causas")
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> Update(Guid id, CausaNaoConformidadeInput input, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/causas/{id}")
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
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/causas/{id}")
            .WithHttpMethod(HttpMethod.Delete)
            .Build();

        await callBuilder.CallAsync<string>();
    }
}