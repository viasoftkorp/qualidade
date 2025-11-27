using System;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Produtos.Services;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Recursos.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades/{idNaoConformidade:guid}/retrabalho/operacoes")]
public class OperacaoRetrabalhoNaoConformidadeController : BaseController
{
    private readonly IOperacaoRetrabalhoNaoConformidadeService _operacaoRetrabalhoNaoConformidadeService;
    private readonly IProdutoService _produtoService;
    private readonly IRecursoService _recursoService;

    public OperacaoRetrabalhoNaoConformidadeController(IOperacaoRetrabalhoNaoConformidadeService operacaoRetrabalhoNaoConformidadeService,
        IProdutoService produtoService, IRecursoService recursoService)
    {
        _operacaoRetrabalhoNaoConformidadeService = operacaoRetrabalhoNaoConformidadeService;
        _produtoService = produtoService;
        _recursoService = recursoService;
    }
    [HttpPost]
    public async Task<ActionResult<OperacaoRetrabalhoNaoConformidadeOutput>> Create([FromRoute] Guid idNaoConformidade,
        [FromBody] OperacaoRetrabalhoNaoConformidadeInput input)
    {
        var idsProdutos = input.Maquinas
            .SelectMany(e => e.Materiais)
            .Select(e => e.IdProduto)
            .Distinct()
            .ToList();

        await _produtoService.BatchInserirNaoCadastrados(idsProdutos);

        var idsRecursos = input.Maquinas
            .Select(e => e.IdRecurso)
            .Distinct()
            .ToList();
        
        await _recursoService.BatchInserirNaoCadastrados(idsRecursos);
        
        var result = await _operacaoRetrabalhoNaoConformidadeService.Create(idNaoConformidade, input);

        if (result.ValidationResult != OperacaoRetrabalhoNaoConformidadeValidationResult.Ok)
        {
            return UnprocessableEntity(result);
        }
        return Ok();
    }
    
    [HttpGet]
    public async Task<ActionResult<OperacaoRetrabalhoNaoConformidadeOutput>> Get([FromRoute] Guid idNaoConformidade)
    {
        var operacaoRetrabalho = await _operacaoRetrabalhoNaoConformidadeService.Get(idNaoConformidade);
        return operacaoRetrabalho != null ? Ok(operacaoRetrabalho) : Ok();
    }
    
    [HttpGet("{idOperacaoRetrabalho:guid}/operacoes")]
    public async Task<ActionResult<PagedResultDto<OperacaoViewOutput>>> GetOperacoes([FromRoute] Guid idOperacaoRetrabalho, 
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var operacaoRetrabalho = 
            await _operacaoRetrabalhoNaoConformidadeService.GetOperacoesView(idOperacaoRetrabalho, input);
        return Ok(operacaoRetrabalho);
    }
}
