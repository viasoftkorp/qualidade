using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Controllers;

[ApiVersion(2024.1)]
[ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
[ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
[ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
[Route("ordens-producao")]
public class OrdemProducaoController : BaseController
{
    private readonly IOrdemProducaoProvider _ordemProducaoProvider;

    public OrdemProducaoController(IOrdemProducaoProvider ordemProducaoProvider)
    {
        _ordemProducaoProvider = ordemProducaoProvider;
    }
    [HttpGet]
    public async Task<PagedResultDto<OrdemProducaoOutput>> GetList([FromQuery] GetListOrdemProducaoInput input)
    {
        var result = await  _ordemProducaoProvider.GetList(input);
        return result;
    }
}