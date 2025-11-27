using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Services;

public class NotaFiscalEntradaProvider : INotaFiscalEntradaProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly ICurrentCompany _currentCompany;
    private const string ServiceName = "Korp.Legacy.Compras";
    private const string BaseEndpoint = "legacy/compras/notas-fiscais";

    public NotaFiscalEntradaProvider(IApiClientCallBuilder apiClientCallBuilder, ICurrentCompany currentCompany)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _currentCompany = currentCompany;
    }
    public async Task<PagedResultDto<NotaFiscalEntradaOutput>> GetList(GetListNotaFiscalInput input)
    {
        input.IdEmpresa = _currentCompany.Id;
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var notasFiscais = await callBuilder.ResponseCallAsync<PagedResultDto<NotaFiscalEntradaOutput>>();
        return notasFiscais;
    }

    public async Task<NotaFiscalEntradaOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var notaFiscal = await callBuilder.ResponseCallAsync<NotaFiscalEntradaOutput>();
        return notaFiscal;
    }
}