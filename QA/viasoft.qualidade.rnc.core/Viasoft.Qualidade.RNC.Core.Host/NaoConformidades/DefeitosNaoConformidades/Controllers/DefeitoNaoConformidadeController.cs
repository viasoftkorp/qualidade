using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades")]
public class DefeitoNaoConformidadeController : BaseController
{
    private readonly IDefeitoNaoConformidadeService _defeitoNaoConformidadeService;
    private readonly IDefeitoNaoConformidadeViewService _defeitoNaoConformidadeViewService;

    public DefeitoNaoConformidadeController(IDefeitoNaoConformidadeService  defeitoNaoConformidadeService,
        IDefeitoNaoConformidadeViewService defeitoNaoConformidadeViewService)
    {
        _defeitoNaoConformidadeService = defeitoNaoConformidadeService;
        _defeitoNaoConformidadeViewService = defeitoNaoConformidadeViewService;
    }
    
    [HttpGet("{idNaoConformidade:guid}/defeitos/{id:guid}")]
    public async Task<ActionResult> GetView([FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid id)
    {
        var result = await _defeitoNaoConformidadeService.Get(idNaoConformidade, id);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{idNaoConformidade:guid}/defeitos")]
    public async Task<ActionResult> GetListView([FromRoute] Guid idNaoConformidade,
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var result = await _defeitoNaoConformidadeViewService.GetListView(idNaoConformidade, input);
        return Ok(result);
    }


    [HttpPost("{idNaoConformidade:guid}/defeitos")]
    public async Task<ActionResult> Insert([FromRoute] Guid idNaoConformidade,
        [FromBody] DefeitoNaoConformidadeInput input)
    {
        await _defeitoNaoConformidadeService.Insert(idNaoConformidade, input);
        return Ok();
    }

    [HttpPut("{idNaoConformidade:guid}/defeitos/{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid idNaoConformidade, [FromRoute] Guid id,
        [FromBody] DefeitoNaoConformidadeInput input)
    {
        await _defeitoNaoConformidadeService.Update(idNaoConformidade, id, input);
        return Ok();
    }

    [HttpDelete("{idNaoConformidade:guid}/defeitos/{id:guid}")]
    public async Task<ActionResult> Remove([FromRoute] Guid idNaoConformidade, [FromRoute] Guid id)
    {
        await _defeitoNaoConformidadeService.Remove(idNaoConformidade, id);
        return Ok();
    }
}