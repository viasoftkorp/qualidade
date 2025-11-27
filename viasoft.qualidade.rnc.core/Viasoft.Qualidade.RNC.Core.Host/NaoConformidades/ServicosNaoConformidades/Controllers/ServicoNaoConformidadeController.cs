using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Controllers;
    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades/{idNaoConformidade:guid}/servicos")]
public class ServicoNaoConformidadeController : BaseController
{
    private readonly IServicoNaoConformidadeservice _servicoNaoConformidadeservice;
    private readonly IServicoNaoConformidadeViewService _servicoNaoConformidadeViewService;

    public ServicoNaoConformidadeController(IServicoNaoConformidadeservice servicoNaoConformidadeservice,
        IServicoNaoConformidadeViewService servicoNaoConformidadeViewService)
    {
        _servicoNaoConformidadeservice = servicoNaoConformidadeservice;
        _servicoNaoConformidadeViewService = servicoNaoConformidadeViewService;
    }

    [HttpGet("{idServico:guid}")]
    public async Task<ActionResult> Get([FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid idServico)
    {
        var result = await _servicoNaoConformidadeservice.Get(idNaoConformidade, idServico);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult> GetListView(
        [FromRoute] Guid idNaoConformidade, [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var result = await _servicoNaoConformidadeViewService.GetListView(idNaoConformidade, input);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServicoValidationResult>> Insert([FromRoute] Guid idNaoConformidade,
        [FromBody] ServicoNaoConformidadeInput input)
    {
        var result = await _servicoNaoConformidadeservice.Insert(idNaoConformidade, input);
        if (result == ServicoValidationResult.Ok)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut("{idServico:guid}")]
    public async Task<ActionResult<ServicoValidationResult>> Update([FromRoute] Guid idNaoConformidade, [FromRoute] Guid idServico,
        [FromBody] ServicoNaoConformidadeInput input)
    {
        var result = await _servicoNaoConformidadeservice.Update(idNaoConformidade, idServico, input);
        if (result == ServicoValidationResult.Ok)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("{idServico:guid}")]
    public async Task<ActionResult> Remove([FromRoute] Guid idNaoConformidade, [FromRoute] Guid idServico)
    {
        await _servicoNaoConformidadeservice.Remove(idNaoConformidade, idServico);
        return Ok();
    }
}