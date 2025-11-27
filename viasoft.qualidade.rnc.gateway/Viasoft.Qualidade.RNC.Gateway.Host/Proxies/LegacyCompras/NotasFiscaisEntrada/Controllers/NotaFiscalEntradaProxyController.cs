using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("notas-fiscais-entrada")]
public class NotaFiscalEntradaProxyController : BaseController
{
    private readonly INotaFiscalEntradaProvider _notaFiscalEntradaProvider;

    public NotaFiscalEntradaProxyController(INotaFiscalEntradaProvider notaFiscalEntradaProvider)
    {
        _notaFiscalEntradaProvider = notaFiscalEntradaProvider;
    }
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<NotaFiscalEntradaOutput>>> GetList([FromQuery] GetListNotaFiscalInput input)
    {
        var notasFiscais = await _notaFiscalEntradaProvider.GetList(input);
        return Ok(notasFiscais);

    }
    [HttpGet("{id}")]
    public async Task<ActionResult<PagedResultDto<NotaFiscalEntradaOutput>>> GetById([FromRoute] Guid id)
    {
        var notaFiscal = await _notaFiscalEntradaProvider.GetById(id);
        return Ok(notaFiscal);
    }
}