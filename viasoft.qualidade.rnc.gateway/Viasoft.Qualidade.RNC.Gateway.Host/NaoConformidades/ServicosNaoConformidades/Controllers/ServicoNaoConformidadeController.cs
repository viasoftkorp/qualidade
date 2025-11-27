using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ServicosNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ServicosNaoConformidades.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades/{idNaoConformidade:guid}/servicos")]
public class ServicoNaoConformidadeController : BaseController
{
    private readonly IServicoNaoConformidadeProvider _servicoNaoConformidadeProvider;

    public ServicoNaoConformidadeController(
        IServicoNaoConformidadeProvider servicoNaoConformidadeProvider)
    {
        _servicoNaoConformidadeProvider = servicoNaoConformidadeProvider;
    }

    [HttpGet("{idServico:guid}")]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult<SolucaoOutput>> Get([FromRoute] Guid idServico, [FromRoute] Guid idNaoConformidade)
    {
        var output = await _servicoNaoConformidadeProvider.Get(idServico, idNaoConformidade);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult<PagedResultDto<SolucaoOutput>>> GetList(
        [FromQuery] PagedFilteredAndSortedRequestInput input, [FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid idSolucao)
    {
        var pagedResult = await _servicoNaoConformidadeProvider.GetList(input, idNaoConformidade, idSolucao);
        return pagedResult != null ? Ok(pagedResult) : NotFound();
    }

    [HttpPost]
    [Authorize(Policies.CreateNaoConformidade)]
    public async Task<IActionResult> Create([FromBody] ServicoNaoConformidadeInput input,
        [FromRoute] Guid idNaoConformidade)
    {
        var responseMessage = await _servicoNaoConformidadeProvider.Create(input, idNaoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{idServico:guid}")]
    [Authorize(Policies.UpdateNaoConformidade)]
    public async Task<IActionResult> Update([FromRoute] Guid idServico, [FromBody] ServicoNaoConformidadeInput input,
        [FromRoute] Guid idNaoConformidade)
    {
        var responseMessage = await _servicoNaoConformidadeProvider.Update(idServico, input, idNaoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }


    [HttpDelete("{idServico:guid}")]
    [Authorize(Policies.DeleteNaoConformidade)]
    public async Task<ActionResult> Delete([FromRoute] Guid idServico, [FromRoute] Guid idNaoConformidade)
    {
        await _servicoNaoConformidadeProvider.Delete(idServico, idNaoConformidade);
        return Ok();
    }
}