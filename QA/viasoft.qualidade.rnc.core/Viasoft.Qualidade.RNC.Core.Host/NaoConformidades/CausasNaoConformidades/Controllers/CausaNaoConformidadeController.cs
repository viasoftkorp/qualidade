using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades")]
public class CausaNaoConformidadeController : BaseController
{
    private readonly ICausaNaoConformidadeService _causaNaoConformidadeService;
    private readonly ICausaNaoConformidadeViewService _causaNaoConformidadeViewService;

    public CausaNaoConformidadeController(
        ICausaNaoConformidadeService causaNaoConformidadeService,
        ICausaNaoConformidadeViewService causaNaoConformidadeViewService)
    {
        _causaNaoConformidadeService = causaNaoConformidadeService;
        _causaNaoConformidadeViewService = causaNaoConformidadeViewService;
    }

    [HttpGet("{idNaoConformidade:guid}/causas/{id:guid}")]
    public async Task<ActionResult> Get([FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid idCausa)
    {
        var result = await _causaNaoConformidadeService.Get(idNaoConformidade, idCausa);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{idNaoConformidade:guid}/defeitos/{idDefeito:guid}/causas")]
    public async Task<ActionResult> GetListView([FromRoute] Guid idNaoConformidade, [FromRoute] Guid idDefeito,
        [FromQuery] GetListWithDefeitoIdFlagInput input)
    {
        var result = await _causaNaoConformidadeViewService.GetListView(idNaoConformidade, idDefeito, input);
        return Ok(result);
    }


    [HttpPost("{idNaoConformidade:guid}/causas")]
    public async Task<ActionResult> Insert([FromRoute] Guid idNaoConformidade,
        [FromBody] CausaNaoConformidadeInput input)
    {
        await _causaNaoConformidadeService.Insert(idNaoConformidade, input);
        return Ok();
    }

    [HttpPut("{idNaoConformidade:guid}/causas/{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid idNaoConformidade, [FromRoute] Guid id,
        [FromBody] CausaNaoConformidadeInput input)
    {
        await _causaNaoConformidadeService.Update(idNaoConformidade, id, input);
        return Ok();
    }

    [HttpDelete("{idNaoConformidade:guid}/causas/{id:guid}")]
    public async Task<ActionResult> Remove([FromRoute] Guid idNaoConformidade, [FromRoute] Guid id)
    {
        await _causaNaoConformidadeService.Remove(idNaoConformidade, id);
        return Ok();
    }
}