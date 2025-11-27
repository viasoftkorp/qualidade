using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Services;

public class NotaFiscalSaidaProvider : INotaFiscalSaidaProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Korp.Legacy.Faturamento";
    private const string BaseEndpoint = "legacy/faturamento/notas-fiscais";

    public NotaFiscalSaidaProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }
    public async Task<PagedResultDto<NotaFiscalSaidaOutput>> GetList(GetListNotasFiscaisInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var notasFiscais = await callBuilder.ResponseCallAsync<PagedResultDto<NotaFiscalSaidaOutput>>();
        return notasFiscais;
    }

    public async Task<NotaFiscalSaidaOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var notaFiscal = await callBuilder.ResponseCallAsync<NotaFiscalSaidaOutput>();
        return notaFiscal;
    }
}