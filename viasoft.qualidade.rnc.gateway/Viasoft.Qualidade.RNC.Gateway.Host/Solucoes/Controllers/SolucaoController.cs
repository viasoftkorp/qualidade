using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("solucoes")]
public class SolucaoController : BaseController
{
    private readonly ISolucaoProvider _solucoesProvider;

    public SolucaoController(ISolucaoProvider solucaoProvider)
    {
        _solucoesProvider = solucaoProvider;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SolucaoOutput>> Get([FromRoute] Guid id)
    {
        var output = await _solucoesProvider.Get(id);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<SolucaoOutput>>> GetList(
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var pagedResult = await _solucoesProvider.GetList(input);
        return pagedResult != null ? Ok(pagedResult) : NotFound();
    }
    [HttpGet("view")]
    [Authorize(Policies.ReadSolucao)]
    public async Task<HttpResponseMessageResult> GetViewList(
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var result = await _solucoesProvider.GetViewList(input);
        return new HttpResponseMessageResult(result);
    }

    [HttpPost]
    [Authorize(Policies.CreateSolucao)]
    public async Task<IActionResult> Create([FromBody] SolucaoInput solucao)
    {
        var responseMessage = await _solucoesProvider.Create(solucao);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{id}")]
    [Authorize(Policies.UpdateSolucao)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] SolucaoInput solucao)
    {
        var responseMessage = await _solucoesProvider.Update(id, solucao);
        return new HttpResponseMessageResult(responseMessage);
    }


    [HttpDelete("{id}")]
    [Authorize(Policies.DeleteSolucao)]
    public async Task<HttpResponseMessageResult> Delete([FromRoute] Guid id)
    {
        var result = await _solucoesProvider.Delete(id);
        return new HttpResponseMessageResult(result);
    }
    [HttpPatch("{id}/ativacao")]
    [Authorize(Policies.DeleteSolucao)]
    public async Task<HttpResponseMessageResult> Ativar([FromRoute] Guid id)
    {
        var result = await _solucoesProvider.Ativar(id);
        return new HttpResponseMessageResult(result);
    }
    [HttpPatch("{id}/inativacao")]
    [Authorize(Policies.DeleteSolucao)]
    public async Task<HttpResponseMessageResult> Inativar([FromRoute] Guid id)
    {
        var result = await _solucoesProvider.Inativar(id);
        return new HttpResponseMessageResult(result);
    }

    [HttpGet("{idSolucao:guid}/produtos/{id}")]
    [Authorize(Policies.ReadSolucao)]
    public async Task<ActionResult<ProdutoSolucaoViewOutput>> GetProdutoSolucaoView([FromRoute] Guid idSolucao,
        [FromRoute] Guid id)
    {
        var output = await _solucoesProvider.GetProdutoSolucaoView(id, idSolucao);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet("{idSolucao:guid}/produtos")]
    [Authorize(Policies.ReadSolucao)]
    public async Task<ActionResult<ProdutoSolucaoViewOutput>> GetProdutoSolucaoList(
        [FromQuery] PagedFilteredAndSortedRequestInput input, [FromRoute] Guid idSolucao)
    {
        var output = await _solucoesProvider.GetProdutoSolucaoList(input, idSolucao);
        return Ok(output);
    }


    [HttpPost("{idSolucao:guid}/produtos")]
    [Authorize(Policies.CreateSolucao)]
    public async Task<IActionResult> AddProduto([FromBody] ProdutoSolucaoInput input, [FromRoute] Guid idSolucao)
    {
        var responseMessage = await _solucoesProvider.AddProduto(input, idSolucao);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{idSolucao:guid}/produtos/{id:guid}")]
    [Authorize(Policies.UpdateSolucao)]
    public async Task<IActionResult> UpdateProduto([FromRoute] Guid idSolucao, [FromRoute] Guid id,
        [FromBody] ProdutoSolucaoInput input)
    {
        var responseMessage = await _solucoesProvider.UpdateProduto(id, input, idSolucao);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpDelete("{idSolucao:guid}/produtos/{id:guid}")]
    [Authorize(Policies.DeleteSolucao)]
    public async Task<ActionResult> DeleteProduto([FromRoute] Guid id, [FromRoute] Guid idSolucao)
    {
        await _solucoesProvider.DeleteProduto(id, idSolucao);
        return Ok();
    }

    [HttpGet("{idSolucao:guid}/servicos/{id:guid}")]
    [Authorize(Policies.ReadSolucao)]
    public async Task<ActionResult<ServicoSolucaoViewOutput>> GetServicoSolucaoView([FromRoute] Guid id,
        [FromRoute] Guid idSolucao)
    {
        var output = await _solucoesProvider.GetServicoSolucaoView(id, idSolucao);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet("{idSolucao:guid}/servicos")]
    [Authorize(Policies.ReadSolucao)]
    public async Task<ActionResult<ServicoSolucaoViewOutput>> GetServicoSolucaoList(
        [FromQuery] PagedFilteredAndSortedRequestInput input, [FromRoute] Guid idSolucao)
    {
        var output = await _solucoesProvider.GetServicoSolucaoList(input, idSolucao);
        return Ok(output);
    }

    [HttpPost("{idSolucao:guid}/servicos/")]
    [Authorize(Policies.CreateSolucao)]
    public async Task<IActionResult> AddServico([FromBody] ServicoSolucaoInput input, [FromRoute] Guid idSolucao)
    {
        var responseMessage = await _solucoesProvider.AddServico(input, idSolucao);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{idSolucao:guid}/servicos/{id}")]
    [Authorize(Policies.UpdateSolucao)]
    public async Task<IActionResult> UpdateServico([FromRoute] Guid id, [FromBody] ServicoSolucaoInput input,
        [FromRoute] Guid idSolucao)
    {
        var responseMessage = await _solucoesProvider.UpdateServico(id, input, idSolucao);
        return new HttpResponseMessageResult(responseMessage);
    }


    [HttpDelete("{idSolucao:guid}/servicos/{id}")]
    [Authorize(Policies.DeleteSolucao)]
    public async Task<ActionResult> DeleteServico([FromRoute] Guid id, [FromRoute] Guid idSolucao)
    {
        await _solucoesProvider.DeleteServico(id, idSolucao);
        return Ok();
    }
}