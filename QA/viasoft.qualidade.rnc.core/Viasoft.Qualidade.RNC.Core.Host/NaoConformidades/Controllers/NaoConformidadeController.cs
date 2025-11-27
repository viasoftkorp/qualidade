using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades")]
public class NaoConformidadeController : BaseController
{
    private readonly INaoConformidadeService _naoConformidadeService;
    private readonly INaoConformidadeViewService _naoConformidadeViewService;

    public NaoConformidadeController(INaoConformidadeService naoConformidadeService, INaoConformidadeViewService naoConformidadeViewService)
    {
        _naoConformidadeService = naoConformidadeService;
        _naoConformidadeViewService = naoConformidadeViewService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get([FromRoute] Guid id)
    {
        var output = await _naoConformidadeService.Get(id);
        if (output is null)
        {
            return NotFound();
        }
        return Ok(output);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetListView([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await _naoConformidadeViewService.GetListView(input);
        return Ok(output);
    }
    [HttpGet("getView/{idNaoConformidade:guid}")]
    public async Task<ActionResult<NaoConformidadeViewOutput>> GetView([FromRoute]Guid idNaoConformidade)
    {
        var output = await _naoConformidadeViewService.GetView(idNaoConformidade);
        return Ok(output);
    }
    
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] NaoConformidadeInput input)
    {
        var result = await _naoConformidadeService.Create(input);
        if (result == NaoConformidadeValidationResult.Ok)
        {
            return Ok(result);
        }

        return UnprocessableEntity(result);
    } 
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] NaoConformidadeInput input)
    {
        var result = await _naoConformidadeService.Update(id, input);
        if (result == NaoConformidadeValidationResult.Ok)
        {
            return Ok(result);
        }

        return UnprocessableEntity(result);   
    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        await _naoConformidadeService.Delete(id);
        return Ok();
    }
}