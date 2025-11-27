using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocais.Providers;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocalViews.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocalViews.Providers;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocalViews.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("estoque-pedido-venda-estoque-local-views")]
public class EstoquePedidoVendaEstoqueLocalController: BaseController
{
    private readonly IEstoquePedidoVendaEstoqueLocalViewProvider _estoquePedidoVendaEstoqueLocalViewProvider;

    public EstoquePedidoVendaEstoqueLocalController(IEstoquePedidoVendaEstoqueLocalViewProvider estoquePedidoVendaEstoqueLocalViewProvider)
    {
        _estoquePedidoVendaEstoqueLocalViewProvider = estoquePedidoVendaEstoqueLocalViewProvider;
    }
    [HttpGet]
    public async Task<PagedResultDto<EstoquePedidoVendaEstoqueLocalViewOutput>> GetList([FromQuery] ListEstoquePedidoVendaEstoqueLocalInput input)
    {
        var result = await _estoquePedidoVendaEstoqueLocalViewProvider.GetList(input);
        return result;
    }
    [HttpGet("{id}")]
    public async Task<EstoquePedidoVendaEstoqueLocalViewOutput> GetById([FromRoute]Guid id)
    {
        var result = await _estoquePedidoVendaEstoqueLocalViewProvider.GetById(id);
        return result;
    }
}