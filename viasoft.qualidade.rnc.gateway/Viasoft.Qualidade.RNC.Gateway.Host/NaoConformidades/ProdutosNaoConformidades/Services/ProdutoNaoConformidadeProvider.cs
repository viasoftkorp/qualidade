using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ProdutosNaoConformidades.Services;

public class ProdutoNaoConformidadeProvider : IProdutoNaoConformidadeProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;

    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/nao-conformidades";


    public ProdutoNaoConformidadeProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<ProdutoNaoConformidadeOutput> Get(Guid id, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/produtos/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var causa = await callBuilder.ResponseCallAsync<ProdutoNaoConformidadeOutput>();
        return causa;
    }

    public async Task<PagedResultDto<ProdutoNaoConformidadeViewOutput>> GetList(
        PagedFilteredAndSortedRequestInput input, Guid idNaoConformidade)
    {
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/produtos?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var causa = await callBuilder.ResponseCallAsync<PagedResultDto<ProdutoNaoConformidadeViewOutput>>();
        return causa;
    }

    public async Task<HttpResponseMessage> Create(ProdutoNaoConformidadeInput input, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/produtos")
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> Update(Guid id, ProdutoNaoConformidadeInput input,
        Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/produtos/{id}")
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
            .WithEndpoint($"{BasePath}/{idNaoConformidade}/produtos/{id}")
            .WithHttpMethod(HttpMethod.Delete)
            .Build();

        await callBuilder.CallAsync<string>();
    }
}