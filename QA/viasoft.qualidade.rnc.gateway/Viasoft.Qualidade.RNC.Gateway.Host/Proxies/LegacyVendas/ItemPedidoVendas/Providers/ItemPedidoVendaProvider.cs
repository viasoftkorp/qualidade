using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyVendas.ItemPedidoVendas.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyVendas.ItemPedidoVendas.Providers;

public class ItemPedidoVendaProvider : IItemPedidoVendaProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly ICurrentCompany _currentCompany;
    private const string ServiceName = "Korp.Legacy.Vendas";
    private const string BaseEndpoint = "legacy/vendas/itens-pedido-venda";
    
    public ItemPedidoVendaProvider(IApiClientCallBuilder apiClientCallBuilder,
        ICurrentCompany currentCompany)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _currentCompany = currentCompany;
    }
    public async Task<PagedResultDto<ItemPedidoVendaOutput>> GetList(GetListItemPedidoVendaInput input)
    {
        input.IdEmpresa = _currentCompany.Id;
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var result = await callBuilder.ResponseCallAsync<PagedResultDto<ItemPedidoVendaOutput>>();
        return result;
    }
}