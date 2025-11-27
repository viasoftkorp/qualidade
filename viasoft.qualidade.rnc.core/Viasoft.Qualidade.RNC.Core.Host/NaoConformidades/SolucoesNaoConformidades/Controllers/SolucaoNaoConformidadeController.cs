using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Controllers;
    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades")]
public class SolucaoNaoConformidadeController : BaseController
{
    private readonly ISolucaoNaoConformidadeService _solucaoNaoConformidadeService;
    private readonly ISolucaoNaoConformidadeViewService _solucaoNaoConformidadeViewService;

    public SolucaoNaoConformidadeController(ISolucaoNaoConformidadeService solucaoNaoConformidadeService,
        ISolucaoNaoConformidadeViewService solucaoNaoConformidadeViewService)
    {
        _solucaoNaoConformidadeService = solucaoNaoConformidadeService;
        _solucaoNaoConformidadeViewService = solucaoNaoConformidadeViewService;
    }
    [HttpGet("{idNaoConformidade:guid}/solucoes/{id:guid}")]
    public async Task<ActionResult> Get([FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid id)
    {
        var result = await _solucaoNaoConformidadeService.Get(idNaoConformidade, id);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }
    
    [HttpGet("{idNaoConformidade:guid}/defeitos/{idDefeito:guid}/solucoes")]
    public async Task<ActionResult> GetListView(
        [FromRoute] Guid idNaoConformidade, [FromQuery] GetListWithDefeitoIdFlagInput input, [FromRoute] Guid idDefeito)
    {
        var result = await _solucaoNaoConformidadeViewService.GetListView(idNaoConformidade, idDefeito, input);
        return Ok(result);
    }

    [HttpPost("{idNaoConformidade:guid}/solucoes")]
    public async Task<ActionResult> Insert([FromRoute] Guid idNaoConformidade,
        [FromBody] SolucaoNaoConformidadeInput input)
    {
        await _solucaoNaoConformidadeService.Insert(idNaoConformidade, input);
        return Ok();
    }

    [HttpPut("{idNaoConformidade:guid}/solucoes/{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid idNaoConformidade, [FromRoute] Guid id,
        [FromBody] SolucaoNaoConformidadeInput input)
    {
        await _solucaoNaoConformidadeService.Update(idNaoConformidade, id, input);
        return Ok();
    }

    [HttpDelete("{idNaoConformidade:guid}/solucoes/{id:guid}")]
    public async Task<ActionResult> Remove([FromRoute] Guid idNaoConformidade, [FromRoute] Guid id)
    {
        await _solucaoNaoConformidadeService.Remove(idNaoConformidade, id);
        return Ok();
    }
}