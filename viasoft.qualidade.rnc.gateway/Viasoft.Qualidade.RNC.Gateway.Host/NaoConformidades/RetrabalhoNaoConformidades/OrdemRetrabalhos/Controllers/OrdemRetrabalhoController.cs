using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades/{idNaoConformidade:guid}/retrabalho/ordens")]
public class OrdemRetrabalhoController : BaseController
{
    private readonly IOrdemRetrabalhoNaoConformidadeService _ordemRetrabalhoNaoConformidadeService;

    public OrdemRetrabalhoController(IOrdemRetrabalhoNaoConformidadeService ordemRetrabalhoNaoConformidadeService)
    {
        _ordemRetrabalhoNaoConformidadeService = ordemRetrabalhoNaoConformidadeService;
    }
    [HttpPost]
    [Authorize(RetrabalhoPolicies.GerarOdfRetrabalhoPolicy)]
    public async Task<HttpResponseMessageResult> GerarOrdemRetrabalho([FromRoute] Guid idNaoConformidade, [FromBody] OrdemRetrabalhoInput input)
    {
        var result = await _ordemRetrabalhoNaoConformidadeService.GerarOrdemRetrabalho(idNaoConformidade, input);

        var output = new HttpResponseMessageResult(result);
        return output;
    }
    [HttpDelete]
    [Authorize(RetrabalhoPolicies.EstornarOdfRetrabalhoPolicy)]
    public async Task<HttpResponseMessageResult> EstornarOrdemRetrabalho([FromRoute] Guid idNaoConformidade)
    {
        var result = await _ordemRetrabalhoNaoConformidadeService.EstornarOrdemRetrabalho(idNaoConformidade);
        var output = new HttpResponseMessageResult(result);
        return output;
    }
    [HttpGet]
    public async Task<HttpResponseMessageResult> Get([FromRoute] Guid idNaoConformidade)
    {
        var result = await _ordemRetrabalhoNaoConformidadeService.Get(idNaoConformidade);
        var output = new HttpResponseMessageResult(result);
        return output;
    }
    [HttpGet("can-generate")]
    public async Task<HttpResponseMessageResult>CanGenerateOrdemRetrabalho([FromRoute] Guid idNaoConformidade, [FromQuery] bool isFullValidation)
    {
        var result = await _ordemRetrabalhoNaoConformidadeService.GetCanGenerateOrdemRetrabalho(idNaoConformidade, isFullValidation);
        var output = new HttpResponseMessageResult(result);
        return output;
    }

}