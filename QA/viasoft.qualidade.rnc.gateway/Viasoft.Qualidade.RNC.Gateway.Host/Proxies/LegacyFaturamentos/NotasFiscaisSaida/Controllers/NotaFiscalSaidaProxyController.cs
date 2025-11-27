using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("notas-fiscais-saida")]
public class NotaFiscalSaidaProxyController : BaseController
{
    private readonly INotaFiscalSaidaProvider _notaFiscalEntradaProvider;

    public NotaFiscalSaidaProxyController(INotaFiscalSaidaProvider notaFiscalSaidaProvider)
    {
        _notaFiscalEntradaProvider = notaFiscalSaidaProvider;
    }
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<NotaFiscalSaidaOutput>>> GetList([FromQuery] GetListNotasFiscaisInput input)
    {
        var notasFiscais = await _notaFiscalEntradaProvider.GetList(input);
        return Ok(notasFiscais);

    }
    [HttpGet("{id}")]
    public async Task<ActionResult<PagedResultDto<NotaFiscalSaidaOutput>>> GetById([FromRoute] Guid id)
    {
        var notaFiscal = await _notaFiscalEntradaProvider.GetById(id);
        return Ok(notaFiscal);
    }
}