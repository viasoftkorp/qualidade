using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Controllers;
    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades/{idNaoConformidade:guid}/conclusao")]

public class ConclusaoNaoConformidadeController : BaseController
{
    private readonly IConclusaoNaoConformidadeService _conclusaoNaoConformidadeService;

    public ConclusaoNaoConformidadeController(IConclusaoNaoConformidadeService conclusaoNaoConformidadeService)
    {
        _conclusaoNaoConformidadeService = conclusaoNaoConformidadeService;
    }

    [HttpPost]
    public async Task<ActionResult> ConcluirNaoConformidade([FromRoute] Guid idNaoConformidade,
        [FromBody] ConclusaoNaoConformidadeInput input)
    {
        await _conclusaoNaoConformidadeService.ConcluirNaoConformidade(idNaoConformidade, input);
        return Ok();
    }
    [HttpGet("calcular-ciclo-tempo")]
    public async Task<ActionResult<int>> CalcularCicloTempo([FromRoute] Guid idNaoConformidade)
    {
        var result = await _conclusaoNaoConformidadeService.CalcularCicloTempo(idNaoConformidade);
        return Ok(result);
    }
    [HttpGet]
    public async Task<ActionResult> GetView([FromRoute] Guid idNaoConformidade)
    {
        var result = await _conclusaoNaoConformidadeService.Get(idNaoConformidade);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpDelete]
    public async Task<ActionResult> Estornar([FromRoute] Guid idNaoConformidade)
    {
        await _conclusaoNaoConformidadeService.Estornar(idNaoConformidade);
        return Ok();
    }
}