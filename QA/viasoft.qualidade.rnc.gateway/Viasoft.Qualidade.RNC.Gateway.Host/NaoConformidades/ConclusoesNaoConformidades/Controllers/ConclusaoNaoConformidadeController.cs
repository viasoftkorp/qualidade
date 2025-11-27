using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.Authentication.Proxy.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ConclusoesNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ConclusoesNaoConformidades.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades/{idNaoConformidade:guid}/conclusao")]
public class ConclusaoNaoConformidadeController : BaseController
{
    private readonly IConclusaoNaoConformidadeProvider _conclusaoNaoConformidadeProvider;

    public ConclusaoNaoConformidadeController(IConclusaoNaoConformidadeProvider conclusaoNaoConformidadeProvider)
    {
        _conclusaoNaoConformidadeProvider = conclusaoNaoConformidadeProvider;
    }

    [HttpPost]
    [Authorize(Policies.ConcluirNaoConformidade)]
    public async Task<IActionResult> ConcluirNaoConformidade([FromRoute] Guid idNaoConformidade,
        [FromBody] ConclusaoNaoConformidadeInput input)
    {
        var responseMessage = await _conclusaoNaoConformidadeProvider.ConcluirNaoConformidade(idNaoConformidade, input);
        return new HttpResponseMessageResult(responseMessage);
    }
    
    [HttpDelete]
    [Authorize(Policies.EstornarConclusaoNaoConformidade)]
    public async Task<IActionResult> Estornar([FromRoute] Guid idNaoConformidade)
    {
        var responseMessage = await _conclusaoNaoConformidadeProvider.Estornar(idNaoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }
    
    [HttpGet("calcular-ciclo-tempo")]
    public async Task<IActionResult> CalcularCicloTempo([FromRoute] Guid idNaoConformidade)
    {
        var responseMessage = await _conclusaoNaoConformidadeProvider.CalcularCicloTempo(idNaoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpGet]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult> Get([FromRoute] Guid idNaoConformidade)
    {
        var output = await _conclusaoNaoConformidadeProvider.Get(idNaoConformidade);
        return output != null ? Ok(output) : NotFound();
    }
}