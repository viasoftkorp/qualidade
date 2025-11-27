using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LogisticsProducts.Providers;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LogisticsProducts.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("produtos")]
public class LogisticsProductsController : BaseController
{
    private readonly IProdutoProvider _produtosProvider;

    public LogisticsProductsController(IProdutoProvider produtoProvider)
    {
        _produtosProvider = produtoProvider;
    }

    [HttpGet]
    public async Task<ActionResult<ProdutoOutput>> GetProdutosList(
        [FromQuery] GetProdutosListInput input)
    {
        var deserializeFilter = JsonConvert.DeserializeObject<JsonNetFilterRule>(input.AdvancedFilter);

        if (!string.IsNullOrWhiteSpace(input.CodigoCategoria))
        {
            var rule = new JsonNetFilterRule()
            {
                Field = "LegacyCategoryCode",
                Operator = "in",
                Type = "string",
                Value = input.CodigoCategoria
            };
            deserializeFilter.Rules.Add(rule);
            input.AdvancedFilter = JsonConvert.SerializeObject(deserializeFilter);
        }
        var produtoFilter = new PagedFilteredAndSortedRequestInput
        {
            AdvancedFilter = input.AdvancedFilter,
            MaxResultCount = input.MaxResultCount,
            Filter = input.Filter,
            Sorting = input.Sorting,
            SkipCount = input.SkipCount
        }; 
        var output = await _produtosProvider.GetProdutosList(produtoFilter);
        return Ok(output);
    }
    
    [HttpGet("pageless")]
    public async Task<ActionResult<ProdutoOutput>> GetPagelessProdutosList([FromQuery] GetProdutosListInput input)
    {
        var output = await _produtosProvider.GetPagelessProdutosList(input);
        return Ok(output);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProdutoOutput>> GetProduto(
        [FromRoute] Guid id)

    {
        var output = await _produtosProvider.GetProduto(id);
        return output != null ? Ok(output) : NotFound();
    }
    [HttpGet("{code}")]
    public async Task<ActionResult<ProdutoOutput>> GetByCode([FromRoute] string code)
    {
        var output = await _produtosProvider.GetByCode(code);
        return output != null ? Ok(output) : NotFound();
    }
}