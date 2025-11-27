using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades")]
public class AgregacaoController : BaseController
{
    private readonly INaoConformidadeRepository _naoConformidadeRepository;


    public AgregacaoController(INaoConformidadeRepository naoConformidadeRepository)
    {
        _naoConformidadeRepository = naoConformidadeRepository;
    }

    [HttpGet("{id:guid}/agregacao")]
    public async Task<ActionResult> GetAgregacao([FromRoute] Guid id)
    {
        var agregacao = await _naoConformidadeRepository
            .Operacoes()
            .Get(id);
        if (agregacao is null)
        {
            return NotFound("Não Encontrado!");
        }

        var output = new AgregacaoNaoConformidadeOutput(agregacao);

        return Ok(output);
    }
}