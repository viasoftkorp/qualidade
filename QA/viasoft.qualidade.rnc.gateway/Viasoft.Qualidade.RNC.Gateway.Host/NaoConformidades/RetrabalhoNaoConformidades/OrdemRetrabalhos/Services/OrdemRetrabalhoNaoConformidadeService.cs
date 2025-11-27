using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services;

public class OrdemRetrabalhoNaoConformidadeService: IOrdemRetrabalhoNaoConformidadeService, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private string BasePath(Guid idNaoConformidade) => $"/qualidade/rnc/core/nao-conformidades/{idNaoConformidade}/retrabalho/ordens";
    public OrdemRetrabalhoNaoConformidadeService(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }
    public async Task<HttpResponseMessage> GerarOrdemRetrabalho(Guid idNaoConformidade, OrdemRetrabalhoInput input)
    {
       var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint(BasePath(idNaoConformidade))
            .WithBody(input)
            .WithHttpMethod(HttpMethod.Post)
            .Build();

       var result = await callBuilder.CallAsync<OrdemRetrabalhoOutput>();
        return result.HttpResponseMessage;
    } 
    public async Task<HttpResponseMessage> Get(Guid idNaoConformidade)
    {
       var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint(BasePath(idNaoConformidade))
            .WithHttpMethod(HttpMethod.Get)
            .Build();

       var result = await callBuilder.CallAsync();
        return result.HttpResponseMessage;
    }

    public async Task<OrdemRetrabalhoNaoConformidadeViewOutput> GetView(Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath(idNaoConformidade)}/GetView")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var result = await callBuilder.ResponseCallAsync<OrdemRetrabalhoNaoConformidadeViewOutput>();
        return result;    
    }

    public async Task<HttpResponseMessage> EstornarOrdemRetrabalho(Guid idNaoConformidade)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint(BasePath(idNaoConformidade))
            .WithHttpMethod(HttpMethod.Delete)
            .Build();

        var result = await callBuilder.CallAsync<OrdemRetrabalhoOutput>();
        return result.HttpResponseMessage;
    }
    public async Task<HttpResponseMessage> GetCanGenerateOrdemRetrabalho(Guid idNaoConformidade, bool isFullValidation)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath(idNaoConformidade)}/can-generate?isFullValidation={isFullValidation}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var canGenerate = await callBuilder.CallAsync<GerarOrdemRetrabalhoValidationResult>();
        return canGenerate.HttpResponseMessage;
    }
}