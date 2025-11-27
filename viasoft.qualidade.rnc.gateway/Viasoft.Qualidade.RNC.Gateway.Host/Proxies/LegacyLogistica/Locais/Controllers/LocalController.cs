using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.Locais.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.Locais.Providers;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyLogistica.Locais.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("locais")]

public class LocalController : BaseController
{
    private readonly ILocalProvider _localProvider;

    public LocalController(ILocalProvider localProvider)
    {
        _localProvider = localProvider;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<LocalOutput>>> GetList([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var itens = await _localProvider.GetList(input);
        return Ok(itens);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<LocalOutput>> GetById([FromRoute] Guid id)
    {
        var item = await _localProvider.GetById(id);
        return Ok(item);
    }
}