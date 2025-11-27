using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.Authentication.Proxy.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ReclamacoesNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ReclamacoesNaoConformidades.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades")]
public class ReclamacaoNaoConformidadeController : BaseController
{
    private readonly IReclamacaoNaoConformidadeProvider _reclamacaoNaoConformidadeProvider;

    public ReclamacaoNaoConformidadeController(IReclamacaoNaoConformidadeProvider reclamacaoNaoConformidadeProvider)
    {
        _reclamacaoNaoConformidadeProvider = reclamacaoNaoConformidadeProvider;
    }

    [HttpPost("{idNaoConformidade:guid}/reclamacao")]
    [Authorize(Policies.UpdateNaoConformidade)]
    public async Task<IActionResult> Insert([FromRoute] Guid idNaoConformidade,
        [FromBody] ReclamacaoNaoConformidadeInput input)
    {
        var responseMessage = await _reclamacaoNaoConformidadeProvider.Insert(idNaoConformidade, input);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{idNaoConformidade:guid}/reclamacao")]
    [Authorize(Policies.UpdateNaoConformidade)]
    public async Task<IActionResult> Update([FromRoute] Guid idNaoConformidade,
        [FromBody] ReclamacaoNaoConformidadeInput input)
    {
        var responseMessage = await _reclamacaoNaoConformidadeProvider.Update(idNaoConformidade, input);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpGet("{idNaoConformidade:guid}/reclamacao")]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult> Get([FromRoute] Guid idNaoConformidade)
    {
        var output = await _reclamacaoNaoConformidadeProvider.Get(idNaoConformidade);
        return Ok(output);
    }
}