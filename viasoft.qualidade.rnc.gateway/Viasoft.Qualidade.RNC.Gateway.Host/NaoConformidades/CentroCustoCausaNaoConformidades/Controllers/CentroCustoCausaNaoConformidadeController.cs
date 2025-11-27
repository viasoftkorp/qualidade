using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades/{idNaoConformidade:guid}/causas/{idCausaNaoConformidade:guid}/centros-custo")]
public class CentroCustoCausaNaoConformidadeController : BaseController
{
    private readonly ICentroCustoCausaNaoConformidadeService _centroCustoCausaNaoConformidadeService;

    public CentroCustoCausaNaoConformidadeController(ICentroCustoCausaNaoConformidadeService centroCustoCausaNaoConformidadeService)
    {
        _centroCustoCausaNaoConformidadeService = centroCustoCausaNaoConformidadeService;
    }

    [HttpGet]
    public async Task<HttpResponseMessageResult> GetList([FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid idCausaNaoConformidade, [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var result = await _centroCustoCausaNaoConformidadeService.GetList(idNaoConformidade, idCausaNaoConformidade, input);
        return new HttpResponseMessageResult(result);
    }
}
