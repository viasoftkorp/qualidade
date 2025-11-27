using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoqueLocais.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoqueLocais.Providers;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocais.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendaEstoqueLocais.Providers;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendas.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoquePedidoVendas.Providers;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyParametros.Providers;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoqueLocais.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("estoque-locais")]
public class EstoqueLocalController: BaseController
{
    private readonly IEstoqueLocalProvider _estoqueLocalProvider;

    public EstoqueLocalController(IEstoqueLocalProvider estoqueLocalProvider)
    {
        _estoqueLocalProvider = estoqueLocalProvider;
    }
    [HttpGet]
    public async Task<PagedResultDto<EstoqueLocalOutput>> GetList([FromQuery] GetListEstoqueLocalInput input)
    {
        var result = await _estoqueLocalProvider.GetList(input);
        return result;
    }
    [HttpGet("{id}")]
    public async Task<EstoqueLocalOutput> GetById([FromRoute]Guid id)
    {
        var result = await _estoqueLocalProvider.GetById(id);
        return result;
    }
}