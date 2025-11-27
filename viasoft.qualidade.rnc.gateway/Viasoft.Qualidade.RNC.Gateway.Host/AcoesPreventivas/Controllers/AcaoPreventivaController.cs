using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.AcoesPreventivas.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.AcoesPreventivas.Services;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;

namespace Viasoft.Qualidade.RNC.Gateway.Host.AcoesPreventivas.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("acoes-preventivas")]
public class AcaoPreventivaController : BaseController
{
    private readonly IAcaoPreventivaProvider _acaoPreventivaProvider;

    public AcaoPreventivaController(IAcaoPreventivaProvider acaoPreventivaProvider)
    {
        _acaoPreventivaProvider = acaoPreventivaProvider;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get([FromRoute] Guid id)
    {
        var output = await _acaoPreventivaProvider.Get(id);

        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet]
    public async Task<HttpResponseMessageResult> GetList(
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var result = await _acaoPreventivaProvider.GetList(input);
        return new HttpResponseMessageResult(result);
    }
    
    [HttpGet("view")]
    [Authorize(Policies.ReadAcaoPreventiva)]
    public async Task<HttpResponseMessageResult> GetViewList(
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var result = await _acaoPreventivaProvider.GetViewList(input);
        return new HttpResponseMessageResult(result);
    }

    [HttpPost]
    [Authorize(Policies.CreateAcaoPreventiva)]
    public async Task<IActionResult> Create([FromBody] AcaoPreventivaInput input)
    {
        var responseMessage = await _acaoPreventivaProvider.Create(input);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policies.UpdateAcaoPreventiva)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] AcaoPreventivaInput input)
    {
        var responseMessage = await _acaoPreventivaProvider.Update(id, input);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policies.DeleteAcaoPreventiva)]
    public async Task<HttpResponseMessageResult> Delete([FromRoute] Guid id)
    {
        var result = await _acaoPreventivaProvider.Delete(id);
        return new HttpResponseMessageResult(result);
    }
    [HttpPatch("{id}/ativacao")]
    [Authorize(Policies.DeleteAcaoPreventiva)]
    public async Task<HttpResponseMessageResult> Ativar([FromRoute] Guid id)
    {
        var result = await _acaoPreventivaProvider.Ativar(id);
        return new HttpResponseMessageResult(result);
    }
    [HttpPatch("{id}/inativacao")]
    [Authorize(Policies.DeleteAcaoPreventiva)]
    public async Task<HttpResponseMessageResult> Inativar([FromRoute] Guid id)
    {
        var result = await _acaoPreventivaProvider.Inativar(id);
        return new HttpResponseMessageResult(result);
    }
}