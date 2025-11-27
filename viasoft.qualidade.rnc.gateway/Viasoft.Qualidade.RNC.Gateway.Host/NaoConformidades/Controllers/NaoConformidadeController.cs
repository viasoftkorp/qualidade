using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.Authentication.Proxy.Dtos;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades")]
public class NaoConformidadeController : BaseController
{
    private readonly INaoConformidadeProvider _naoConformidadeProvider;

    public NaoConformidadeController(INaoConformidadeProvider naoConformidadeProvider)
    {
        _naoConformidadeProvider = naoConformidadeProvider;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult<NaoConformidadeOutput>> Get([FromRoute] Guid id)
    {
        var output = await _naoConformidadeProvider.Get(id);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult<PagedResultDto<NaoConformidadeViewOutput>>> GetList(
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var pagedResult = await _naoConformidadeProvider.GetList(input);
        return Ok(pagedResult);
    }

    [HttpPost]
    [Authorize(Policies.CreateNaoConformidade)]
    public async Task<IActionResult> Create([FromBody] NaoConformidadeInput naoConformidade)
    {
        var responseMessage = await _naoConformidadeProvider.Create(naoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policies.UpdateNaoConformidade)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] NaoConformidadeInput naoConformidade)
    {
        var responseMessage = await _naoConformidadeProvider.Update(id, naoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policies.DeleteNaoConformidade)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var responseMessage = await _naoConformidadeProvider.Delete(id);
        return new HttpResponseMessageResult(responseMessage);
    }
}