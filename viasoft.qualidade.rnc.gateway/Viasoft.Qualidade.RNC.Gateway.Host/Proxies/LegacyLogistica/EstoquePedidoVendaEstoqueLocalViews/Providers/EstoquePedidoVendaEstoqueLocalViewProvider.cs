using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viasoft.Core.ApiClient;
using Viasoft.Core.ApiClient.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocais.Providers;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocalViews.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocalViews.Providers;

public class EstoquePedidoVendaEstoqueLocalViewProvider: IEstoquePedidoVendaEstoqueLocalViewProvider, ITransientDependency
{
    private readonly IApiClientCallBuilder _apiClientCallBuilder;
    private readonly ICurrentCompany _currentCompany;
    private const string ServiceName = "Viasoft.Legacy.Logistica";
    private const string BaseEndpoint = "legacy/logistica/estoque-pedido-venda-estoque-local-views";
    public EstoquePedidoVendaEstoqueLocalViewProvider(IApiClientCallBuilder apiClientCallBuilder, ICurrentCompany currentCompany)
    {
        _apiClientCallBuilder = apiClientCallBuilder;
        _currentCompany = currentCompany;
    }
    public async Task<PagedResultDto<EstoquePedidoVendaEstoqueLocalViewOutput>> GetList(ListEstoquePedidoVendaEstoqueLocalInput input)
    {
        input.IdEmpresa = _currentCompany.Id;

        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}?{input.ToHttpGetQueryParameter()}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var estoques = await callBuilder.ResponseCallAsync<PagedResultDto<EstoquePedidoVendaEstoqueLocalViewOutput>>();
        return estoques;
    }

    public async Task<EstoquePedidoVendaEstoqueLocalViewOutput> GetById(Guid id)
    {
        var callBuilder = _apiClientCallBuilder
            .WithServiceName(ServiceName)
            .WithEndpoint($"{BaseEndpoint}/{id}")
            .WithHttpMethod(HttpMethod.Get)
            .Build();

        var estoques = await callBuilder.ResponseCallAsync<EstoquePedidoVendaEstoqueLocalViewOutput>();
        return estoques;
    }
}