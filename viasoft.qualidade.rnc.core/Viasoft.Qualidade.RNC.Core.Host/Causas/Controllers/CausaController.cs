using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Services;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Causas.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("causas")]
public class CausaController : BaseController
{
    private readonly ICausaService _causasService;

    public CausaController(ICausaService causaService)
    {
        _causasService = causaService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get([FromRoute] Guid id)
    {
        var output = await _causasService.Get(id);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult> GetList([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await _causasService.GetList(input);
        return Ok(output);
    }
    
    [HttpGet("view")]
    public async Task<ActionResult> GetViewList([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await _causasService.GetViewList(input);
        return Ok(output);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CausaInput input)
    {
        await _causasService.Create(input);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] CausaInput input)
    {
        var result = await _causasService.Update(id, input);
        if (result == ValidationResult.Ok)
        {
            return Ok();
        }

        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var validationResult = await _causasService.Delete(id);
        if (validationResult == ValidationResult.EntidadeEmUso)
        {
            return UnprocessableEntity(validationResult);
        }
        return validationResult == ValidationResult.Ok ? Ok(validationResult) : NotFound(validationResult);
    }
    [HttpPatch("{id:guid}/ativacao")]
    public async Task<ActionResult> Ativar([FromRoute] Guid id)
    {
        var result = await _causasService.ChangeStatus(id, true);
        return Ok(result);
    }
    [HttpPatch("{id:guid}/inativacao")]
    public async Task<ActionResult> Inativar([FromRoute] Guid id)
    {
        var result = await _causasService.ChangeStatus(id, false);
        return Ok(result);
    }
}