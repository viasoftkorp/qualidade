using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntradaRateioLote.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntradaRateioLote.Providers;

public class ItemNotaFiscalEntradaRateioLoteProvider : IItemNotaFiscalEntradaRateioLoteProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly ICurrentCompany _currentCompany;
    private const string ServiceName = "Korp.Legacy.Compras";
    private const string BaseEndpoint = "legacy/compras/itens-notas-fiscais-rateio-lote";
    
    public ItemNotaFiscalEntradaRateioLoteProvider(IApiClientCallBuilder apiClientCallBuilder, ICurrentCompany currentCompany)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _currentCompany = currentCompany;
    }
    public async Task<PagedResultDto<ItemNotaFiscalEntradaRateioLoteOutput>> GetList(GetListItemNotaFiscalRateioLoteInput input)
    {
        input.IdEmpresa = _currentCompany.Id;
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var itensNotasFiscais = await callBuilder.ResponseCallAsync<PagedResultDto<ItemNotaFiscalEntradaRateioLoteOutput>>();
        return itensNotasFiscais;
    }

    public async Task<ItemNotaFiscalEntradaRateioLoteOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var notaFiscal = await callBuilder.ResponseCallAsync<ItemNotaFiscalEntradaRateioLoteOutput>();
        return notaFiscal;
    }
}