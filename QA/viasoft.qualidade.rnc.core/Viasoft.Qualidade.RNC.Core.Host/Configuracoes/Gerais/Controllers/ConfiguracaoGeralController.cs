using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.Configuracoes.Gerais;
using Viasoft.Qualidade.RNC.Core.Host.Configuracoes.Gerais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.FrontendUrls;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyParametros.Providers;

namespace Viasoft.Qualidade.RNC.Core.Host.Configuracoes.Gerais.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("configuracoes-gerais")]
public class ConfiguracaoGeralController : BaseController
{
    private readonly IRepository<ConfiguracaoGeral> _configuracaoGerais;
    private readonly ILegacyParametrosProvider _legacyParametrosProvider;
    private readonly IFrontendUrl _frontendUrl;

    public ConfiguracaoGeralController(IRepository<ConfiguracaoGeral> configuracaoGerais,
        ILegacyParametrosProvider legacyParametrosProvider, IFrontendUrl frontendUrl)
    {
        _configuracaoGerais = configuracaoGerais;
        _legacyParametrosProvider = legacyParametrosProvider;
        _frontendUrl = frontendUrl;
    }
    [HttpGet]
    public async Task<ActionResult<ConfiguracaoGeralOutput>> Get()
    {
        var configuracoes = await _configuracaoGerais.FirstAsync();
        var utilizarReservaDePedidoNaLocalizacaoDeEstoque = await _legacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque();

        var output = new ConfiguracaoGeralOutput(configuracoes)
        {
            UtilizarReservaDePedidoNaLocalizacaoDeEstoque = utilizarReservaDePedidoNaLocalizacaoDeEstoque,
            FrontendUrl = _frontendUrl.Value
        };
        return Ok(output);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] ConfiguracaoGeralInput input)
    {
        var configuracoes = await _configuracaoGerais.FirstAsync();
        configuracoes.Update(input);
        await _configuracaoGerais.UpdateAsync(configuracoes, true);
        return Ok();
    }
}