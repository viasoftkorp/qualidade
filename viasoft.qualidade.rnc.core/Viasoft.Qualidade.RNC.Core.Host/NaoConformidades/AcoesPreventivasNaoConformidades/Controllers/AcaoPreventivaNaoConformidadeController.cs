using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades")]
public class AcaoPreventivaNaoConformidadeController : BaseController
{
    private readonly IAcaoPreventivaNaoConformidadeService _acaoPreventivaNaoConformidadeService;
    private readonly IAcaoPreventivaNaoConformidadeViewService _acaoPreventivaNaoConformidadeViewService;

    public AcaoPreventivaNaoConformidadeController(
        IAcaoPreventivaNaoConformidadeService acaoPreventivaNaoConformidadeService,
        IAcaoPreventivaNaoConformidadeViewService acaoPreventivaNaoConformidadeViewService)
    {
        _acaoPreventivaNaoConformidadeService = acaoPreventivaNaoConformidadeService;
        _acaoPreventivaNaoConformidadeViewService = acaoPreventivaNaoConformidadeViewService;
    }

    [HttpGet("{idNaoConformidade:guid}/acoes-preventivas/{idAcaoPreventiva:guid}")]
    public async Task<ActionResult> Get([FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid idAcaoPreventiva)
    {
        var result = await _acaoPreventivaNaoConformidadeService.Get(idNaoConformidade, idAcaoPreventiva);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{idNaoConformidade:guid}/defeitos/{idDefeitoNaoConformidade:guid}/acoes-preventivas")]
    public async Task<ActionResult> GetListView(
        [FromRoute] Guid idNaoConformidade, [FromRoute] Guid idDefeitoNaoConformidade , [FromQuery] GetListWithDefeitoIdFlagInput input)
    {
        var result = await _acaoPreventivaNaoConformidadeViewService.GetListView(idNaoConformidade, idDefeitoNaoConformidade, input);
        return Ok(result);
    }

    [HttpPost("{idNaoConformidade:guid}/acoes-preventivas")]
    public async Task<ActionResult> Insert([FromRoute] Guid idNaoConformidade,
        [FromBody] AcaoPreventivaNaoConformidadeInput input)
    {
        await _acaoPreventivaNaoConformidadeService.Insert(idNaoConformidade, input);
        return Ok();
    }

    [HttpPut("{idNaoConformidade:guid}/acoes-preventivas/{idAcaoPreventiva:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid idNaoConformidade, [FromRoute] Guid idAcaoPreventiva, [FromBody] AcaoPreventivaNaoConformidadeInput input)
    {
       await _acaoPreventivaNaoConformidadeService.Update(idNaoConformidade, idAcaoPreventiva, input);
       return Ok();
    }
    
    [HttpDelete("{idNaoConformidade:guid}/acoes-preventivas/{idAcaoPreventiva:guid}")]
    public async Task<ActionResult> Remove([FromRoute] Guid idNaoConformidade, [FromRoute] Guid idAcaoPreventiva)
    {
        await _acaoPreventivaNaoConformidadeService.Remove(idNaoConformidade, idAcaoPreventiva);
        return Ok();
    }
}