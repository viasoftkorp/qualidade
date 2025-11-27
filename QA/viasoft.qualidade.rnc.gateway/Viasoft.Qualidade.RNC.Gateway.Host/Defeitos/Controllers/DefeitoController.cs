using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.Authentication.Proxy.Dtos;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.Defeitos.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Defeitos.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Defeitos.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("defeitos")]
public class DefeitoController : BaseController
{
    private readonly IDefeitoProvider _defeitoProvider;

    public DefeitoController(IDefeitoProvider defeitoProvider)
    {
        _defeitoProvider = defeitoProvider;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DefeitoOutput>> Get([FromRoute] Guid id)
    {
        var output = await _defeitoProvider.Get(id);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<DefeitoViewOutput>>> GetList(
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var pagedResult = await _defeitoProvider.GetList(input);
        return pagedResult != null ? Ok(pagedResult) : NotFound();
    }
    [HttpGet("view")]
    [Authorize(Policies.ReadDefeito)]

    public async Task<HttpResponseMessageResult> GetViewList(
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var result = await _defeitoProvider.GetViewList(input);
        return new HttpResponseMessageResult(result);
    }

    [HttpPost]
    [Authorize(Policies.CreateDefeito)]
    public async Task<IActionResult> Create([FromBody] DefeitoInput Causa)
    {
        var responseMessage = await _defeitoProvider.Create(Causa);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{id}")]
    [Authorize(Policies.UpdateDefeito)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] DefeitoInput Causa)
    {
        var responseMessage = await _defeitoProvider.Update(id, Causa);
        return new HttpResponseMessageResult(responseMessage);
    }


    [HttpDelete("{id}")]
    [Authorize(Policies.DeleteDefeito)]
    public async Task<HttpResponseMessageResult> Delete([FromRoute] Guid id)
    {
        var result = await _defeitoProvider.Delete(id);
        return new HttpResponseMessageResult(result);
    }
    [HttpPatch("{id}/ativacao")]
    [Authorize(Policies.DeleteDefeito)]
    public async Task<HttpResponseMessageResult> Ativar([FromRoute] Guid id)
    {
        var result = await _defeitoProvider.Ativar(id);
        return new HttpResponseMessageResult(result);
    }
    [HttpPatch("{id}/inativacao")]
    [Authorize(Policies.DeleteDefeito)]
    public async Task<HttpResponseMessageResult> Inativar([FromRoute] Guid id)
    {
        var result = await _defeitoProvider.Inativar(id);
        return new HttpResponseMessageResult(result);
    }
}