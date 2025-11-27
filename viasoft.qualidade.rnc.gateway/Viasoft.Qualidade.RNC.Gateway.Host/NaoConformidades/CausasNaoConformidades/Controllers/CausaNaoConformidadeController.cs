using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.CausasNaoConformidades.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades")]
public class CausaNaoConformidadeController : BaseController
{
    private readonly ICausaNaoConformidadeProvider _causaNaoConformidadeProvider;

    public CausaNaoConformidadeController(ICausaNaoConformidadeProvider causaNaoConformidadeProvider)
    {
        _causaNaoConformidadeProvider = causaNaoConformidadeProvider;
    }

    [HttpGet("{idNaoConformidade:guid}/causas/{id:guid}")]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult<SolucaoOutput>> Get([FromRoute] Guid id, [FromRoute] Guid idNaoConformidade)
    {
        var output = await _causaNaoConformidadeProvider.Get(id, idNaoConformidade);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet("{idNaoConformidade:guid}/defeitos/{idDefeito:guid}/causas")]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult<PagedResultDto<SolucaoOutput>>> GetList(
        [FromQuery] GetListWithDefeitoIdFlagInput input, [FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid idDefeito)
    {
        var pagedResult = await _causaNaoConformidadeProvider.GetList(input, idNaoConformidade, idDefeito, true);
        return pagedResult != null ? Ok(pagedResult) : NotFound();
    }

    [HttpPost("{idNaoConformidade:guid}/causas")]
    [Authorize(Policies.CreateNaoConformidade)]
    public async Task<IActionResult> Create([FromBody] CausaNaoConformidadeInput input,
        [FromRoute] Guid idNaoConformidade)
    {
        var responseMessage = await _causaNaoConformidadeProvider.Create(input, idNaoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{idNaoConformidade:guid}/causas/{id:guid}")]
    [Authorize(Policies.UpdateNaoConformidade)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CausaNaoConformidadeInput input,
        [FromRoute] Guid idNaoConformidade)
    {
        var responseMessage = await _causaNaoConformidadeProvider.Update(id, input, idNaoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }


    [HttpDelete("{idNaoConformidade:guid}/causas/{id:guid}")]
    [Authorize(Policies.DeleteNaoConformidade)]
    public async Task<ActionResult> Delete([FromRoute] Guid id, [FromRoute] Guid idNaoConformidade)
    {
        await _causaNaoConformidadeProvider.Delete(id, idNaoConformidade);
        return Ok();
    }
}