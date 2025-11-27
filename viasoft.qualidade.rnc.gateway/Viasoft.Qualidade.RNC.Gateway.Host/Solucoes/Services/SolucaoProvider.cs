using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Services;

public class SolucaoProvider : ISolucaoProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;

    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private const string BasePath = "/qualidade/rnc/core/solucoes";


    public SolucaoProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }

    public async Task<SolucaoOutput> Get(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var solucao = await callBuilder.ResponseCallAsync<SolucaoOutput>();
        return solucao;
    }

    public async Task<PagedResultDto<SolucaoOutput>> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var solucao = await callBuilder.ResponseCallAsync<PagedResultDto<SolucaoOutput>>();
        return solucao;
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

    public async Task<HttpResponseMessage> Create(SolucaoInput input)
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

    public async Task<HttpResponseMessage> Update(Guid id, SolucaoInput input)
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


    public async Task<HttpResponseMessage> AddProduto(ProdutoSolucaoInput input, Guid idSolucao)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idSolucao}/produtos")
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> UpdateProduto(Guid id, ProdutoSolucaoInput input, Guid idSolucao)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idSolucao}/produtos/{id}")
            .WithHttpMethod(HttpMethod.Put)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task DeleteProduto(Guid id, Guid idSolucao)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idSolucao}/produtos/{id}")
            .WithHttpMethod(HttpMethod.Delete)
            .Build();

        var response = await callBuilder.CallAsync<string>();
    }

    public async Task<HttpResponseMessage> AddServico(ServicoSolucaoInput input, Guid idSolucao)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idSolucao}/servicos")
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task<HttpResponseMessage> UpdateServico(Guid id, ServicoSolucaoInput input, Guid idSolucao)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idSolucao}/servicos/{id}")
            .WithHttpMethod(HttpMethod.Put)
            .WithBody(input)
            .Build();

        var response = await callBuilder.CallAsync<string>();
        return response.HttpResponseMessage;
    }

    public async Task DeleteServico(Guid id, Guid idSolucao)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idSolucao}/servicos/{id}")
            .WithHttpMethod(HttpMethod.Delete)
            .Build();

        var response = await callBuilder.CallAsync<string>();
    }

    public async Task<ProdutoSolucaoViewOutput> GetProdutoSolucaoView(Guid id, Guid idSolucao)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idSolucao}/produtos/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var output = await callBuilder.ResponseCallAsync<ProdutoSolucaoViewOutput>();
        return output;
    }

    public async Task<ServicoSolucaoViewOutput> GetServicoSolucaoView(Guid id, Guid idSolucao)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idSolucao}/servicos/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var output = await callBuilder.ResponseCallAsync<ServicoSolucaoViewOutput>();
        return output;
    }

    public async Task<PagedResultDto<ProdutoSolucaoViewOutput>> GetProdutoSolucaoList(
        PagedFilteredAndSortedRequestInput input, Guid idSolucao)
    {
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idSolucao}/produtos?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var output = await callBuilder.ResponseCallAsync<PagedResultDto<ProdutoSolucaoViewOutput>>();
        return output;
    }

    public async Task<PagedResultDto<ServicoSolucaoViewOutput>> GetServicoSolucaoList(
        PagedFilteredAndSortedRequestInput input, Guid idSolucao)
    {
        var queryParameters = input.ToHttpGetQueryParameter();
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath}/{idSolucao}/servicos/?{queryParameters}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var output = await callBuilder.ResponseCallAsync<PagedResultDto<ServicoSolucaoViewOutput>>();
        return output;
    }
}