using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Services;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("acoes-preventivas")]
public class AcaoPreventivaController : BaseController
{
    private readonly IAcaoPreventivaService _acaoPreventivaService;

    public AcaoPreventivaController(IAcaoPreventivaService acaoPreventivaService)
    {
        _acaoPreventivaService = acaoPreventivaService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get([FromRoute] Guid id)
    {
        var output = await _acaoPreventivaService.Get(id);

        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult> GetList([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await _acaoPreventivaService.GetList(input);
        return Ok(output);
    }
    [HttpGet("view")]
    public async Task<ActionResult> GetViewList([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await _acaoPreventivaService.GetViewList(input);
        return Ok(output);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] AcaoPreventivaInput input)
    {
        await _acaoPreventivaService.Create(input);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] AcaoPreventivaInput input)
    {
        var result = await _acaoPreventivaService.Update(id, input);
        
        if (result == ValidationResult.Ok)
        {
            return Ok();
        }

        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ValidationResult>> Delete([FromRoute] Guid id)
    {
        var output = await _acaoPreventivaService.Delete(id);
        switch (output)
        {
            case ValidationResult.Ok:
                return Ok(output);
            case ValidationResult.NotFound:
                return NotFound(output);
            case ValidationResult.EntidadeEmUso:
                return UnprocessableEntity(output);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    [HttpPatch("{id:guid}/ativacao")]
    public async Task<ActionResult> Ativar([FromRoute] Guid id)
    {
        var result = await _acaoPreventivaService.ChangeStatus(id, true);
        return Ok(result);
    }
    [HttpPatch("{id:guid}/inativacao")]
    public async Task<ActionResult> Inativar([FromRoute] Guid id)
    {
        var result = await _acaoPreventivaService.ChangeStatus(id, false);
        return Ok(result);
    }
}