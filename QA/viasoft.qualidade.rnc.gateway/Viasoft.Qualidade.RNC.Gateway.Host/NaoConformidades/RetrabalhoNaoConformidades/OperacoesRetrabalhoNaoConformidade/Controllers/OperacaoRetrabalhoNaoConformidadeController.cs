using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.Authentication.Proxy.Dtos;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades/{idNaoConformidade:guid}/retrabalho/operacoes")]
public class OperacaoRetrabalhoNaoConformidadeController : BaseController
{
    private readonly IOperacaoRetrabalhoNaoConformidadeService _operacaoRetrabalhoNaoConformidadeService;

    public OperacaoRetrabalhoNaoConformidadeController(IOperacaoRetrabalhoNaoConformidadeService operacaoRetrabalhoNaoConformidadeService)
    {
        _operacaoRetrabalhoNaoConformidadeService = operacaoRetrabalhoNaoConformidadeService;
    }
    [HttpPost]
    [Authorize(RetrabalhoPolicies.GerarOperacaoRetrabalhoPolicy)]
    public async Task<HttpResponseMessageResult> Create([FromRoute] Guid idNaoConformidade, [FromBody] OperacaoRetrabalhoNaoConformidadeInput input)
    {
        var result = await _operacaoRetrabalhoNaoConformidadeService.Create(idNaoConformidade, input);

        var output = new HttpResponseMessageResult(result);
        return output;
    }
    [HttpGet]
    public async Task<OperacaoRetrabalhoNaoConformidade> Get([FromRoute] Guid idNaoConformidade)
    {
        var result = await _operacaoRetrabalhoNaoConformidadeService.Get(idNaoConformidade);

        return result;
    }
    [HttpGet("{idOperacaoRetrabalho:guid}/operacoes")]    
    public async Task<PagedResultDto<OperacaoViewOutput>> Get([FromRoute] Guid idNaoConformidade, [FromRoute] Guid idOperacaoRetrabalho,
    [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var result = await _operacaoRetrabalhoNaoConformidadeService
            .GetOperacoesView(idNaoConformidade, idOperacaoRetrabalho, input);

        return result;
    }
}