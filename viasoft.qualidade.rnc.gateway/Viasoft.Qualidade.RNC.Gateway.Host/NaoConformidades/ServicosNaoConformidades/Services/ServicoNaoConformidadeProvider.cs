using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ServicosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ServicosNaoConformidades.Services;

public class ServicoNaoConformidadeProvider : IServicoNaoConformidadeProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;

    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/nao-conformidades";


    public ServicoNaoConformidadeProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<ServicoNaoConformidadeOutput> Get(Guid id, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/servicos/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var causa = await callBuilder.ResponseCallAsync<ServicoNaoConformidadeOutput>();
        return causa;
    }

    public async Task<PagedResultDto<ServicoNaoConformidadeViewOutput>> GetList(
        PagedFilteredAndSortedRequestInput input, Guid idNaoConformidade, Guid idSolucao)
    {
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/servicos?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var causa = await callBuilder.ResponseCallAsync<PagedResultDto<ServicoNaoConformidadeViewOutput>>();
        return causa;
    }

    public async Task<HttpResponseMessage> Create(ServicoNaoConformidadeInput input, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/servicos")
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> Update(Guid id, ServicoNaoConformidadeInput input,
        Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/servicos/{id}")
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
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/servicos/{id}")
            .WithHttpMethod(HttpMethod.Delete)
            .Build();

        await callBuilder.CallAsync<string>();
    }
}