using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Services;

public class OperacaoRetrabalhoNaoConformidadeService : IOperacaoRetrabalhoNaoConformidadeService, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private string BasePath(Guid idNaoConformidade) => $"/qualidade/rnc/core/nao-conformidades/{idNaoConformidade}/retrabalho/operacoes";
    
    public OperacaoRetrabalhoNaoConformidadeService(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }
    public async Task<HttpResponseMessage> Create(Guid idNaoConformidade, OperacaoRetrabalhoNaoConformidadeInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint(BasePath(idNaoConformidade))
            .WithBody(input)
            .WithHttpMethod(HttpMethod.Post)
            .Build();

        var result = await callBuilder.CallAsync();
        return result.HttpResponseMessage;    
    }

    public async Task<OperacaoRetrabalhoNaoConformidade> Get(Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint(BasePath(idNaoConformidade))
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var result = await callBuilder.ResponseCallAsync<OperacaoRetrabalhoNaoConformidade>();
        return result;
    }
    public async Task<PagedResultDto<OperacaoViewOutput>> GetOperacoesView(Guid idNaoConformidade, Guid idOperacaoRetrabalho, 
        PagedFilteredAndSortedRequestInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath(idNaoConformidade)}/{idOperacaoRetrabalho}/operacoes?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var result = await callBuilder.ResponseCallAsync<PagedResultDto<OperacaoViewOutput>>();
        return result;        
    }
}