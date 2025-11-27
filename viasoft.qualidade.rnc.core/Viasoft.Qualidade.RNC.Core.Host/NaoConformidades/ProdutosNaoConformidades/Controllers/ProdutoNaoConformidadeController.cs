using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades/{idNaoConformidade:guid}/produtos")]
public class ProdutoNaoConformidadeController : BaseController
{
    private readonly IProdutoNaoConformidadeService _produtoNaoConformidadeService;
    private readonly IProdutoNaoConformidadeViewService _produtoNaoConformidadeViewService;

    public ProdutoNaoConformidadeController(
        IProdutoNaoConformidadeService produtoNaoConformidadeService,
        IProdutoNaoConformidadeViewService produtoNaoConformidadeViewService)
    {
        _produtoNaoConformidadeService = produtoNaoConformidadeService;
        _produtoNaoConformidadeViewService = produtoNaoConformidadeViewService;
    }

    [HttpGet("{idProduto:guid}")]
    public async Task<ActionResult> Get([FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid idProduto)
    {
        var result = await _produtoNaoConformidadeService.Get(idNaoConformidade, idProduto);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult> GetListView(
        [FromRoute] Guid idNaoConformidade, [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var result = await _produtoNaoConformidadeViewService.GetListView(idNaoConformidade, input);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Insert([FromRoute] Guid idNaoConformidade,
        [FromBody] ProdutoNaoConformidadeInput input)
    {
        await _produtoNaoConformidadeService.Insert(idNaoConformidade, input);
        return Ok();
    }

    [HttpPut("{idProduto:guid}")]
    public async Task<ActionResult> Update([FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid idProduto,
        [FromBody] ProdutoNaoConformidadeInput input)
    {
        await _produtoNaoConformidadeService.Update(idNaoConformidade, idProduto, input);
        return Ok();
    }

    [HttpDelete("{idProduto:guid}")]
    public async Task<ActionResult> Remove([FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid idProduto)
    {
        await _produtoNaoConformidadeService.Remove(idNaoConformidade, idProduto);
        return Ok();
    }
}