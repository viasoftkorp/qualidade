using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades/{idNaoConformidade}/implementacao-evitar-reincidencias")]
public class ImplementacaoEvitarReincidenciaNaoConformidadeController : BaseController
{
    private readonly IImplementacaoEvitarReincidenciaNaoConformidadeService
        _implementacaoEvitarReincidenciaNaoConformidadeService;

    private readonly IImplementacaoEvitarReincidenciaNaoConformidadeViewService _implementacaoEvitarReincidenciaNaoConformidadeViewService;

    public ImplementacaoEvitarReincidenciaNaoConformidadeController(
        IImplementacaoEvitarReincidenciaNaoConformidadeService implementacaoEvitarReincidenciaNaoConformidadeService,
        IImplementacaoEvitarReincidenciaNaoConformidadeViewService implementacaoEvitarReincidenciaNaoConformidadeViewService)
    {
        _implementacaoEvitarReincidenciaNaoConformidadeService = implementacaoEvitarReincidenciaNaoConformidadeService;
        _implementacaoEvitarReincidenciaNaoConformidadeViewService = implementacaoEvitarReincidenciaNaoConformidadeViewService;
    }

    [HttpGet]
    public async Task<ActionResult> GetListView([FromRoute] Guid idNaoConformidade, [FromQuery] GetListViewInput input)
    {
        var result =
            await _implementacaoEvitarReincidenciaNaoConformidadeViewService.GetListView(idNaoConformidade, input);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _implementacaoEvitarReincidenciaNaoConformidadeService.GetById(id);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Insert([FromRoute] Guid idNaoConformidade,
        [FromBody] ImplementacaoEvitarReincidenciaNaoConformidadeInput input)
    {
        await _implementacaoEvitarReincidenciaNaoConformidadeService.Insert(idNaoConformidade, input);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] Guid idNaoConformidade,
        [FromBody] ImplementacaoEvitarReincidenciaNaoConformidadeInput input)
    {
        await _implementacaoEvitarReincidenciaNaoConformidadeService.Update(idNaoConformidade, input);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Remove([FromRoute] Guid idNaoConformidade, [FromRoute] Guid id)
    {
        await _implementacaoEvitarReincidenciaNaoConformidadeService.Remove(idNaoConformidade, id);
        return Ok();
    }
}