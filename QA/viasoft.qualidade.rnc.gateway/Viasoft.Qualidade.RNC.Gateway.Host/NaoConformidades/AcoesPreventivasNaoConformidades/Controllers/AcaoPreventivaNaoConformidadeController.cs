using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades")]
public class AcaoPreventivaNaoConformidadeController : BaseController
{
    private readonly IAcaoPreventivaNaoConformidadeProvider _acoesPreventivasNaoConformidadeProvider;

    public AcaoPreventivaNaoConformidadeController(
        IAcaoPreventivaNaoConformidadeProvider acaoPreventivaNaoConformidadeProvider)
    {
        _acoesPreventivasNaoConformidadeProvider = acaoPreventivaNaoConformidadeProvider;
    }

    [HttpGet("{idNaoConformidade:guid}/acoes-preventivas/{idAcaoPreventiva:guid}")]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult<AcaoPreventivaNaoConformidadeViewOutput>> Get([FromRoute] Guid idAcaoPreventiva,
        [FromRoute] Guid idNaoConformidade)
    {
        var output = await _acoesPreventivasNaoConformidadeProvider.Get(idNaoConformidade, idAcaoPreventiva);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet("{idNaoConformidade:guid}/defeitos/{idDefeitoNaoConformidade:guid}/acoes-preventivas")]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult<PagedResultDto<AcaoPreventivaNaoConformidadeViewOutput>>> GetList(
        [FromRoute] Guid idNaoConformidade, [FromRoute] Guid idDefeitoNaoConformidade,
        [FromQuery] GetListWithDefeitoIdFlagInput input)
    {
        var pagedResult =
            await _acoesPreventivasNaoConformidadeProvider.GetList(idNaoConformidade, idDefeitoNaoConformidade, input, true);
        return pagedResult != null ? Ok(pagedResult) : NotFound();
    }

    [HttpPost("{idNaoConformidade:guid}/acoes-preventivas")]
    [Authorize(Policies.CreateNaoConformidade)]
    public async Task<IActionResult> Create([FromRoute] Guid idNaoConformidade,
        [FromBody] AcaoPreventivaNaoConformidadeInput AcaoPreventivaNaoConformidade)
    {
        var responseMessage =
            await _acoesPreventivasNaoConformidadeProvider.Create(idNaoConformidade, AcaoPreventivaNaoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{idNaoConformidade:guid}/acoes-preventivas/{idAcaoPreventiva:guid}")]
    [Authorize(Policies.UpdateNaoConformidade)]
    public async Task<IActionResult> Update([FromRoute] Guid idNaoConformidade, [FromRoute] Guid idAcaoPreventiva,
        [FromBody] AcaoPreventivaNaoConformidadeInput AcaoPreventivaNaoConformidade)
    {
        var responseMessage = await _acoesPreventivasNaoConformidadeProvider.Update(idNaoConformidade, idAcaoPreventiva,
            AcaoPreventivaNaoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }


    [HttpDelete("{idNaoConformidade:guid}/acoes-preventivas/{idAcaoPreventiva:guid}")]
    [Authorize(Policies.DeleteNaoConformidade)]
    public async Task<ActionResult> Delete([FromRoute] Guid idNaoConformidade, [FromRoute] Guid idAcaoPreventiva)
    {
        await _acoesPreventivasNaoConformidadeProvider.Delete(idNaoConformidade, idAcaoPreventiva);
        return Ok();
    }
}