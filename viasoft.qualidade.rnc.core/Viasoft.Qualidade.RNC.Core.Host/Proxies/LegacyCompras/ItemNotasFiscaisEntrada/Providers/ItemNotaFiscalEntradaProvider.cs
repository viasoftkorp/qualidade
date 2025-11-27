using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Providers;

public class ItemNotaFiscalEntradaProvider : IItemNotaFiscalEntradaProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly ICurrentCompany _currentCompany;
    private const string ServiceName = "Korp.Legacy.Compras";
    private const string BaseEndpoint = "legacy/compras/itens-notas-fiscais";
    
    public ItemNotaFiscalEntradaProvider(IApiClientCallBuilder apiClientCallBuilder, ICurrentCompany currentCompany)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _currentCompany = currentCompany;
    }
    public async Task<PagedResultDto<ItemNotaFiscalEntradaOutput>> GetList(GetListItemNotaFiscalInput input)
    {
        input.IdEmpresa = _currentCompany.Id;
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var itensNotasFiscais = await callBuilder.ResponseCallAsync<PagedResultDto<ItemNotaFiscalEntradaOutput>>();
        return itensNotasFiscais;
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