using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades/agregacoes")]
public class NaoConformidadeAgregacaoController : BaseController
{
    private readonly INaoConformidadeProvider _naoConformidadeProvider;

    public NaoConformidadeAgregacaoController(INaoConformidadeProvider naoConformidadeProvider)
    {
        _naoConformidadeProvider = naoConformidadeProvider;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AgregacaoNaoConformidadeOutput>> Get([FromRoute] Guid id)
    {
        var output = await _naoConformidadeProvider.GetAgregacao(id);
        return output != null ? Ok(output) : NotFound();
    }
}