using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.ActionResults;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ProdutosNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.ProdutosNaoConformidades.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("nao-conformidades/{idNaoConformidade:guid}/produtos")]
public class ProdutoNaoConformidadeController : BaseController
{
    private readonly IProdutoNaoConformidadeProvider _produtoNaoConformidadeProvider;

    public ProdutoNaoConformidadeController(
        IProdutoNaoConformidadeProvider produtoNaoConformidadeProvider)
    {
        _produtoNaoConformidadeProvider = produtoNaoConformidadeProvider;
    }

    [HttpGet("{idProduto:guid}")]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult<SolucaoOutput>> Get([FromRoute] Guid idProduto, [FromRoute] Guid idNaoConformidade)
    {
        var output = await _produtoNaoConformidadeProvider.Get(idProduto, idNaoConformidade);
        return output != null ? Ok(output) : NotFound();
    }

    [HttpGet]
    [Authorize(Policies.ReadNaoConformidade)]
    public async Task<ActionResult<PagedResultDto<SolucaoOutput>>> GetList(
        [FromQuery] PagedFilteredAndSortedRequestInput input, [FromRoute] Guid idNaoConformidade)
    {
        var pagedResult = await _produtoNaoConformidadeProvider.GetList(input, idNaoConformidade);
        return pagedResult != null ? Ok(pagedResult) : NotFound();
    }

    [HttpPost]
    [Authorize(Policies.CreateNaoConformidade)]
    public async Task<IActionResult> Create([FromBody] ProdutoNaoConformidadeInput input,
        [FromRoute] Guid idNaoConformidade)
    {
        var responseMessage = await _produtoNaoConformidadeProvider.Create(input, idNaoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }

    [HttpPut("{idProduto:guid}")]
    [Authorize(Policies.UpdateNaoConformidade)]
    public async Task<IActionResult> Update([FromRoute] Guid idProduto, [FromBody] ProdutoNaoConformidadeInput input,
        [FromRoute] Guid idNaoConformidade)
    {
        var responseMessage = await _produtoNaoConformidadeProvider.Update(idProduto, input, idNaoConformidade);
        return new HttpResponseMessageResult(responseMessage);
    }


    [HttpDelete("{idProduto:guid}")]
    [Authorize(Policies.DeleteNaoConformidade)]
    public async Task<ActionResult> Delete([FromRoute] Guid idProduto, [FromRoute] Guid idNaoConformidade)
    {
        await _produtoNaoConformidadeProvider.Delete(idProduto, idNaoConformidade);
        return Ok();
    }
}