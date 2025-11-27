using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Services.Recursos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("recursos")]
public class RecursosController : BaseController
{
    private readonly IRecursoProvider _recursoProvider;

    public RecursosController(IRecursoProvider recursoProvider)
    {
        _recursoProvider = recursoProvider;
    }

    [HttpGet]
    public async Task<ActionResult<ProdutoOutput>> GetRecursosList(
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await _recursoProvider.GetRecursosList(input);
        return Ok(output);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProdutoOutput>> GetRecurso(
        [FromRoute] Guid id)

    {
        var output = await _recursoProvider.GetRecurso(id);
        return output != null ? Ok(output) : NotFound();
    }
}