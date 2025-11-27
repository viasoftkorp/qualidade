using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.DefeitosNaoConformidades.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades")]
public class DefeitoNaoConformidadeController : BaseController
{
    private readonly IDefeitoNaoConformidadeProvider _defeitoNaoConformidadeProvider;

    public DefeitoNaoConformidadeController(IDefeitoNaoConformidadeProvider defeitoNaoConformidadeProvider)
    {
        _defeitoNaoConformidadeProvider = defeitoNaoConformidadeProvider;
    }

    [HttpGet("{idNaoConformidade:guid}/defeitos/{id:guid}")]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult> Get([FromRoute] Guid id, [FromRoute] Guid idNaoConformidade)
    {
        var output = await _defeitoNaoConformidadeProvider.Get(id, idNaoConformidade);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet("{idNaoConformidade:guid}/defeitos")]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult> GetList(
        [FromQuery] PagedFilteredAndSortedRequestInput input, [FromRoute] Guid idNaoConformidade)
    {
        var pagedResult = await _defeitoNaoConformidadeProvider.GetList(input, idNaoConformidade);
        return pagedResult != null ? Ok(pagedResult) : NotFound();
    }

    [HttpPost("{idNaoConformidade:guid}/defeitos")]
    [Authorize(Policies.CreateNaoConformidade)]
    public async Task<IActionResult> Create([FromBody] DefeitoNaoConformidadeInput input,
        [FromRoute] Guid idNaoConformidade)
    {
        var responseMessage = await _defeitoNaoConformidadeProvider.Create(input, idNaoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{idNaoConformidade:guid}/defeitos/{id:guid}")]
    [Authorize(Policies.UpdateNaoConformidade)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] DefeitoNaoConformidadeInput input,
        [FromRoute] Guid idNaoConformidade)
    {
        var responseMessage = await _defeitoNaoConformidadeProvider.Update(id, input, idNaoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }


    [HttpDelete("{idNaoConformidade:guid}/defeitos/{id:guid}")]
    [Authorize(Policies.DeleteNaoConformidade)]
    public async Task<ActionResult> Delete([FromRoute] Guid id, [FromRoute] Guid idNaoConformidade)
    {
        await _defeitoNaoConformidadeProvider.Delete(id, idNaoConformidade);
        return Ok();
    }
}