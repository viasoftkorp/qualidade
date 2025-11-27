using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Services;

public class ImplementacaoEvitarReincidenciaNaoConformidadeService : IImplementacaoEvitarReincidenciaNaoConformidadeService, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private string BasePath(Guid idNaoConformidade) => $"/qualidade/rnc/core/nao-conformidades/{idNaoConformidade}/implementacao-evitar-reincidencias";
    
    public ImplementacaoEvitarReincidenciaNaoConformidadeService(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }
    public async Task<PagedResultDto<ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade, GetListViewInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath(idNaoConformidade)}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();
        var output = await callBuilder.ResponseCallAsync<PagedResultDto<ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput>>();
        return output;    
    }

    public async Task<HttpResponseMessageResult> GetById(Guid id, Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath(idNaoConformidade)}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();
        var callResult = await callBuilder.CallAsync();
        var output = new HttpResponseMessageResult(callResult.HttpResponseMessage);
        return output;    
    }

    public async Task<HttpResponseMessageResult> Insert(Guid idNaoConformidade, ImplementacaoEvitarReincidenciaNaoConformidadeInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath(idNaoConformidade)}")
            .WithHttpMethod(HttpMethod.Post)
            .WithBody(input)
            .Build();
        var callResult = await callBuilder.CallAsync();
        var output = new HttpResponseMessageResult(callResult.HttpResponseMessage);
        return output;
    }

    public async Task<HttpResponseMessageResult> Update(Guid idNaoConformidade, Guid id, ImplementacaoEvitarReincidenciaNaoConformidadeInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath(idNaoConformidade)}/{id}")
            .WithHttpMethod(HttpMethod.Put)
            .WithBody(input)
            .Build();
        var callResult = await callBuilder.CallAsync();
        var output = new HttpResponseMessageResult(callResult.HttpResponseMessage);
        return output;
        
    }

    public async Task<HttpResponseMessageResult> Remove(Guid idNaoConformidade, Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath(idNaoConformidade)}/{id}")
            .WithHttpMethod(HttpMethod.Delete)
            .Build();
        var callResult = await callBuilder.CallAsync();
        var output = new HttpResponseMessageResult(callResult.HttpResponseMessage);
        return output;
    }
}