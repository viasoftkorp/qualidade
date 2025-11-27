using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades/{idNaoConformidade}/implementacao-evitar-reincidencias")]

public class ImplementacaoEvitarReincidenciaNaoConformidadeController : BaseController
{
    private readonly IImplementacaoEvitarReincidenciaNaoConformidadeService _implementacaoEvitarReincidenciaNaoConformidadeService;

    public ImplementacaoEvitarReincidenciaNaoConformidadeController(IImplementacaoEvitarReincidenciaNaoConformidadeService 
        implementacaoEvitarReincidenciaNaoConformidadeService)
    {
        _implementacaoEvitarReincidenciaNaoConformidadeService = implementacaoEvitarReincidenciaNaoConformidadeService;
    }

    [HttpGet]
    public async Task<PagedResultDto<ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput>> GetListView([FromRoute] Guid idNaoConformidade,
        [FromQuery] GetListViewInput input)
    {
        var result = await _implementacaoEvitarReincidenciaNaoConformidadeService.GetListView(idNaoConformidade, input);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<HttpResponseMessageResult> GetById([FromRoute] Guid id, [FromRoute] Guid idNaoConformidade)
    {
        var result = await _implementacaoEvitarReincidenciaNaoConformidadeService.GetById(id, idNaoConformidade);
        return result;
    }

    [HttpPost]
    public async Task<HttpResponseMessageResult> Insert([FromRoute] Guid idNaoConformidade,
        [FromBody] ImplementacaoEvitarReincidenciaNaoConformidadeInput input)
    {
        var result = await _implementacaoEvitarReincidenciaNaoConformidadeService.Insert(idNaoConformidade, input);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<HttpResponseMessageResult> Update([FromRoute] Guid idNaoConformidade, Guid id,
        [FromBody] ImplementacaoEvitarReincidenciaNaoConformidadeInput input)
    {
        var result = await _implementacaoEvitarReincidenciaNaoConformidadeService.Update(idNaoConformidade, id, input);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<HttpResponseMessageResult> Remove([FromRoute] Guid idNaoConformidade, [FromRoute] Guid id)
    {
        var result = await _implementacaoEvitarReincidenciaNaoConformidadeService.Remove(idNaoConformidade, id);
        return result;
    }
}