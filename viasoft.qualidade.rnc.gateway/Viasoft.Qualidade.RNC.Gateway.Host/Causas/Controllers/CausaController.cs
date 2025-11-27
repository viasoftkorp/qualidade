using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Causas.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Causas.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("causas")]
public class CausaController : BaseController
{
    private readonly ICausaProvider _causasProvider;

    public CausaController(ICausaProvider causaProvider)
    {
        _causasProvider = causaProvider;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CausaOutput>> Get([FromRoute] Guid id)
    {
        var output = await _causasProvider.Get(id);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<CausaOutput>>> GetList([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var pagedResult = await _causasProvider.GetList(input);
        return pagedResult != null ? Ok(pagedResult) : NotFound();
    }
    [HttpGet("view")]
    [Authorize(Policies.ReadCausa)]
    public async Task<HttpResponseMessageResult> GetViewList(
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var result = await _causasProvider.GetViewList(input);
        return new HttpResponseMessageResult(result);
    }

    [HttpPost]
    [Authorize(Policies.CreateCausa)]
    public async Task<IActionResult> Create([FromBody] CausaInput Causa)
    {
        var responseMessage = await _causasProvider.Create(Causa);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{id}")]
    [Authorize(Policies.UpdateCausa)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CausaInput Causa)
    {
        var responseMessage = await _causasProvider.Update(id, Causa);
        return new HttpResponseMessageResult(responseMessage);
    }


    [HttpDelete("{id}")]
    [Authorize(Policies.DeleteCausa)]
    public async Task<HttpResponseMessageResult> Delete([FromRoute] Guid id)
    {
        var output = await _causasProvider.Delete(id);
        return new HttpResponseMessageResult(output);
    }
    [HttpPatch("{id}/ativacao")]
    [Authorize(Policies.DeleteCausa)]
    public async Task<HttpResponseMessageResult> Ativar([FromRoute] Guid id)
    {
        var result = await _causasProvider.Ativar(id);
        return new HttpResponseMessageResult(result);
    }
    [HttpPatch("{id}/inativacao")]
    [Authorize(Policies.DeleteCausa)]
    public async Task<HttpResponseMessageResult> Inativar([FromRoute] Guid id)
    {
        var result = await _causasProvider.Inativar(id);
        return new HttpResponseMessageResult(result);
    }
}