using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("solucoes")]
public class SolucaoController : BaseController
{
    private readonly ISolucaoService _solucoesService;

    public SolucaoController(ISolucaoService solucaoService)
    {
        _solucoesService = solucaoService;
    }
    
    [HttpGet("{id:guid}")] 
    public async Task<ActionResult> Get([FromRoute] Guid id)
    {
        var output = await _solucoesService.Get(id);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult> GetList([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await _solucoesService.GetList(input);
        return Ok(output);
    }
    [HttpGet("view")]
    public async Task<ActionResult> GetViewList([FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await _solucoesService.GetViewList(input);
        return Ok(output);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] SolucaoInput input)
    {
        await _solucoesService.Create(input);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] SolucaoInput input)
    {
        var result = await _solucoesService.Update(id, input);
        return result == ValidationResult.Ok ? Ok() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var validationResult = await _solucoesService.Delete(id);
        if (validationResult == ValidationResult.EntidadeEmUso)
        {
            return UnprocessableEntity(validationResult);
        }
        return validationResult == ValidationResult.Ok ? Ok(validationResult) : NotFound(validationResult);
    }
    [HttpPatch("{id:guid}/ativacao")]
    public async Task<ActionResult> Ativar([FromRoute] Guid id)
    {
        var result = await _solucoesService.ChangeStatus(id, true);
        return Ok(result);
    }
    [HttpPatch("{id:guid}/inativacao")]
    public async Task<ActionResult> Inativar([FromRoute] Guid id)
    {
        var result = await _solucoesService.ChangeStatus(id, false);
        return Ok(result);
    }
    
    [HttpGet("{idSolucao:guid}/produtos/{id:guid}")] 
    public async Task<ActionResult> GetProdutoSolucaoView([FromRoute] Guid id)
    {
        var output = await _solucoesService.GetProdutoSolucaoView(id);
        return output != null ? Ok(output) : NotFound();
    }
    
    [HttpPost("{idSolucao:guid}/produtos")]
    public async Task<ActionResult> AddProduto([FromBody] ProdutoSolucaoInput input)
    {
        await _solucoesService.AddProduto(input);
        return Ok();
    }

    [HttpPut("{idSolucao:guid}/produtos/{id:guid}")]
    public async Task<ActionResult> UpdateProduto([FromRoute] Guid id, [FromBody] ProdutoSolucaoInput input)
    {
        var result = await _solucoesService.UpdateProduto(id, input);
        return result == ValidationResult.Ok ? Ok() : NotFound();
    }

    [HttpDelete("{idSolucao:guid}/produtos/{id:guid}")]
    public async Task<ActionResult> DeleteProduto([FromRoute] Guid id)
    {
        var output = await _solucoesService.DeleteProduto(id);
        return output == ValidationResult.Ok ? Ok() : NotFound();
    }
    
    [HttpGet("{idSolucao:guid}/servicos/{id:guid}")] 
    public async Task<ActionResult> GetServicoSolucaoView([FromRoute] Guid id)
    {
        var output = await _solucoesService.GetServicoSolucaoView(id);
        return output != null ? Ok(output) : NotFound();
    }
    
    [HttpPost("{idSolucao:guid}/servicos")]
    public async Task<ActionResult<ServicoValidationResult>> AddServico([FromBody] ServicoSolucaoInput input)
    {
        var result = await _solucoesService.AddServico(input);
        if (result == ServicoValidationResult.Ok)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut("{idSolucao:guid}/servicos/{id:guid}")]
    public async Task<ActionResult<ServicoValidationResult>> UpdateServico([FromRoute] Guid id, [FromBody] ServicoSolucaoInput input)
    {
        var result = await _solucoesService.UpdateServico(id, input);
        
        return result == ServicoValidationResult.Ok ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{idSolucao:guid}/servicos/{id:guid}")]
    public async Task<ActionResult> DeleteServico([FromRoute] Guid id)
    {
        var output = await _solucoesService.DeleteServico(id);
        return output == ValidationResult.Ok ? Ok() : NotFound();
    }
    
    [HttpGet("{idSolucao:guid}/produtos")]
    public async Task<ActionResult> GetProdutoSolucaoList(
        [FromQuery] PagedFilteredAndSortedRequestInput input, [FromRoute] Guid idSolucao)
    {
        var output = await _solucoesService.GetProdutoSolucaoList(input, idSolucao);
        return Ok(output);
    }
    
    [HttpGet("{idSolucao:guid}/servicos")]
    public async Task<ActionResult> GetServicoSolucaoList(
        [FromQuery] PagedFilteredAndSortedRequestInput input, [FromRoute] Guid idSolucao)
    {
        var output = await _solucoesService.GetServicoSolucaoList(input, idSolucao);
        return Ok(output);
    }
    
}