using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Naturezas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Naturezas.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.Naturezas.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("naturezas")]
public class NaturezaController : BaseController
{
    private readonly INaturezaService _naturezasService;

    public NaturezaController(INaturezaService naturezaService)
    {
        _naturezasService = naturezaService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get([FromRoute] Guid id)
    {
        var output = await _naturezasService.Get(id);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult> GetList([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await _naturezasService.GetList(input);
        return Ok(output);
    }
    
    [HttpGet("view")]
    public async Task<ActionResult> GetViewList([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await _naturezasService.GetViewList(input);
        return Ok(output);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] NaturezaInput input)
    {
         await _naturezasService.Create(input);
         return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] NaturezaInput input)
    {
        var result = await _naturezasService.Update(id, input);
        if (result == ValidationResult.Ok)
        {
            return Ok();
        }

        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var output = await _naturezasService.Delete(id);
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
        var result = await _naturezasService.ChangeStatus(id, true);
        return Ok(result);
    }
    [HttpPatch("{id:guid}/inativacao")]
    public async Task<ActionResult> Inativar([FromRoute] Guid id)
    {
        var result = await _naturezasService.ChangeStatus(id, false);
        return Ok(result);
    }
}