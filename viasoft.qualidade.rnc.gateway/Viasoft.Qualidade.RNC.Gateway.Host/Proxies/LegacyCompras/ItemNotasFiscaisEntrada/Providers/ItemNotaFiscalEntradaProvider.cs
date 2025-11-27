using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Providers;

public class ItemNotaFiscalEntradaProvider : IItemNotaFiscalEntradaProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private const string ServiceName = "Korp.Legacy.Compras";
    private const string BaseEndpoint = "legacy/compras/itens-notas-fiscais";
    
    public ItemNotaFiscalEntradaProvider(IApiClientCallBuilder apiClientCallBuilder)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
    }
    public async Task<PagedResultDto<ItemNotaFiscalEntradaOutput>> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var notasFiscais = await callBuilder.ResponseCallAsync<PagedResultDto<ItemNotaFiscalEntradaOutput>>();
        return notasFiscais;
    }

    public async Task<ItemNotaFiscalEntradaOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var notaFiscal = await callBuilder.ResponseCallAsync<ItemNotaFiscalEntradaOutput>();
        return notaFiscal;
    }
}