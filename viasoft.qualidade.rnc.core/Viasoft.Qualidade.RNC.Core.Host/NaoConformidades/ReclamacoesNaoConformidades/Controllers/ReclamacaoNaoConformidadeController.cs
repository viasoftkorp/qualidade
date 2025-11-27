using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Controllers;
    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades")]
public class ReclamacaoNaoConformidadeController : BaseController
{
    private readonly IReclamacaoNaoConformidadeService _reclamacaoNaoConformidadeService;

    public ReclamacaoNaoConformidadeController(IReclamacaoNaoConformidadeService reclamacaoNaoConformidadeService)
    {
        _reclamacaoNaoConformidadeService = reclamacaoNaoConformidadeService;
    }
    [HttpPost("{idNaoConformidade:guid}/reclamacao")]
    public async Task<ActionResult> Insert([FromRoute] Guid idNaoConformidade,
        [FromBody] ReclamacaoNaoConformidadeInput input)
    {
        await _reclamacaoNaoConformidadeService.Insert(idNaoConformidade, input);
        return Ok();
    }
    
    [HttpPut("{idNaoConformidade:guid}/reclamacao")]
    public async Task<ActionResult> Update([FromRoute] Guid idNaoConformidade,
        [FromBody] ReclamacaoNaoConformidadeInput input)
    {
        await _reclamacaoNaoConformidadeService.Update(idNaoConformidade, input);
        return Ok();
    }
    
    [HttpGet("{idNaoConformidade:guid}/reclamacao")]
    public async Task<ActionResult> Get([FromRoute] Guid idNaoConformidade)
    {
        var result = await _reclamacaoNaoConformidadeService.Get(idNaoConformidade);
        return Ok(result);
    }
}