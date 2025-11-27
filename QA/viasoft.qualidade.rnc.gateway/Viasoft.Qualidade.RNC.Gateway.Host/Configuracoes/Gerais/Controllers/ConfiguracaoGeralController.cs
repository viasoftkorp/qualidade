using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.Authentication.Proxy.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;
using Viasoft.Qualidade.RNC.Gateway.Host.Configuracoes.Gerais.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Configuracoes.Gerais.Services;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Configuracoes.Gerais.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("configuracoes-gerais")]
public class ConfiguracaoGeralController : BaseController
{
    private readonly IConfiguracaoGeralService _configuracaoGeralService;

    public ConfiguracaoGeralController(IConfiguracaoGeralService configuracaoGeralService)
    {
        _configuracaoGeralService = configuracaoGeralService;
    }
    [HttpGet]
    public async Task<HttpResponseMessageResult> Get()
    {
        var result = await _configuracaoGeralService.Get();
        return new HttpResponseMessageResult(result);
    }

    [HttpPut]
    [Authorize(Policy = Policies.AtualizarConfiguracoesGerais)]
    public async Task<HttpResponseMessageResult> Update([FromBody] ConfiguracaoGeralInput input)
    {
        var result = await _configuracaoGeralService.Update(input);
        return new HttpResponseMessageResult(result);
    }
}