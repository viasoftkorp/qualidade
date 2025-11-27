using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.Naturezas.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Naturezas.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Naturezas.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("naturezas")]
public class NaturezaController : BaseController
{
    private readonly INaturezaProvider _naturezasProvider;

    public NaturezaController(INaturezaProvider naturezaProvider)
    {
        _naturezasProvider = naturezaProvider;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NaturezaOutput>> Get([FromRoute] Guid id)
    {
        var output = await _naturezasProvider.Get(id);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<NaturezaOutput>>> GetList(
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var pagedResult = await _naturezasProvider.GetList(input);
        return pagedResult != null ? Ok(pagedResult) : NotFound();
    }
    
    [HttpGet("view")]
    [Authorize(Policies.ReadNatureza)]
    public async Task<HttpResponseMessageResult> GetViewList(
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var result = await _naturezasProvider.GetViewList(input);
        return new HttpResponseMessageResult(result);
    }

    [HttpPost]
    [Authorize(Policies.CreateNatureza)]
    public async Task<IActionResult> Create([FromBody] NaturezaInput natureza)
    {
        var responseMessage = await _naturezasProvider.Create(natureza);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{id}")]
    [Authorize(Policies.UpdateNatureza)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] NaturezaInput natureza)
    {
        var responseMessage = await _naturezasProvider.Update(id, natureza);
        return new HttpResponseMessageResult(responseMessage);
    }


    [HttpDelete("{id}")]
    [Authorize(Policies.DeleteNatureza)]
    public async Task<HttpResponseMessageResult> Delete([FromRoute] Guid id)
    {
        var result = await _naturezasProvider.Delete(id);
        return new HttpResponseMessageResult(result);
    }
    
    [HttpPatch("{id}/ativacao")]
    [Authorize(Policies.DeleteNatureza)]
    public async Task<HttpResponseMessageResult> Ativar([FromRoute] Guid id)
    {
        var result = await _naturezasProvider.Ativar(id);
        return new HttpResponseMessageResult(result);
    }
    [HttpPatch("{id}/inativacao")]
    [Authorize(Policies.DeleteNatureza)]
    public async Task<HttpResponseMessageResult> Inativar([FromRoute] Guid id)
    {
        var result = await _naturezasProvider.Inativar(id);
        return new HttpResponseMessageResult(result);
    }
}