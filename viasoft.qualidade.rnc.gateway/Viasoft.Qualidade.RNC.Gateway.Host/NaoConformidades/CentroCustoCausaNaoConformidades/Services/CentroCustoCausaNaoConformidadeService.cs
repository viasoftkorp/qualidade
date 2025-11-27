using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Services;

public class CentroCustoCausaNaoConformidadeService : ICentroCustoCausaNaoConformidadeService, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Viasoft.Qualidade.RNC.Core";
    private static string BasePath(Guid idNaoConformidade) => $"qualidade/rnc/core/nao-conformidades/{idNaoConformidade}";

    public CentroCustoCausaNaoConformidadeService(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }
    public async Task<HttpResponseMessage> GetList(Guid idNaoConformidade, Guid idCausaNaoConformidade, PagedFilteredAndSortedRequestInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath(idNaoConformidade)}/causas/{idCausaNaoConformidade}/centros-custo?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var result = await callBuilder.CallAsync();
        return result.HttpResponseMessage;
    }
    public async Task<PagedResultDto<CentroCustoCausaNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade, PagedFilteredAndSortedRequestInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BasePath(idNaoConformidade)}/causas/centros-custo/get-view-list?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var result = await callBuilder.ResponseCallAsync<PagedResultDto<CentroCustoCausaNaoConformidadeViewOutput>>();
        return result;
    }
}
