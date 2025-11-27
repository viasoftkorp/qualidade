using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFinanceiros.CentroCustos.Providers;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.EstoqueLocais.Providers;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFinanceiros.CentroCustos.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("centros-custo")]
public class CentroCustoController: BaseController
{
    private readonly ICentroCustoProvider _centroCustoProvider;

    public CentroCustoController(ICentroCustoProvider centroCustoProvider)
    {
        _centroCustoProvider = centroCustoProvider;
    }
    [HttpGet]
    public async Task<HttpResponseMessageResult> GetList([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var result = await _centroCustoProvider.GetList(input);
        return new HttpResponseMessageResult(result);
    }
    [HttpGet("{id}")]
    public async Task<HttpResponseMessageResult> GetById([FromRoute]Guid id)
    {
        var result = await _centroCustoProvider.GetById(id);
        return new HttpResponseMessageResult(result);
    }
}