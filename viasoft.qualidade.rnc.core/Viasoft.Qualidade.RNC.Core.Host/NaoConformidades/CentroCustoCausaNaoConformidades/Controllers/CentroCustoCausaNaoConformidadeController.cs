using System;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.CentroCustos;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CentroCustoCausaNaoConformidades.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades/{idNaoConformidade:guid}")]
public class CentroCustoCausaNaoConformidadeController : BaseController
{
    private readonly IRepository<CentroCustoCausaNaoConformidade> _centroCustoCausaNaoConformidades;
    private readonly IRepository<CentroCusto> _centroCustos;
    private readonly ICurrentCompany _currentCompany;

    public CentroCustoCausaNaoConformidadeController(IRepository<CentroCustoCausaNaoConformidade> centroCustoCausaNaoConformidades,
        IRepository<CentroCusto> centroCustos, ICurrentCompany currentCompany)
    {
        _centroCustoCausaNaoConformidades = centroCustoCausaNaoConformidades;
        _centroCustos = centroCustos;
        _currentCompany = currentCompany;
    }
    [HttpGet("causas/{idCausaNaoConformidade:guid}/centros-custo")]
    public async Task<PagedResultDto<CentroCustoCausaNaoConformidadeOutput>> GetList([FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid idCausaNaoConformidade, [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var query = _centroCustoCausaNaoConformidades.AsNoTracking()
            .Where(e => e.IdNaoConformidade == idNaoConformidade)
            .Where(e => e.IdCausaNaoConformidade == idCausaNaoConformidade)
            .Where(e => e.CompanyId == _currentCompany.Id)
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();
        var itens = await query
            .Select(e => new CentroCustoCausaNaoConformidadeOutput(e))
            .ToListAsync();

        return new PagedResultDto<CentroCustoCausaNaoConformidadeOutput>
        {
            TotalCount = totalCount,
            Items = itens
        };
    }
    [HttpGet("causas/{idCausaNaoConformidade:guid}/centros-custo/get-view-list")]
    public async Task<PagedResultDto<CentroCustoCausaNaoConformidadeViewOutput>> GetViewList([FromRoute] Guid idNaoConformidade,
        [FromRoute] Guid idCausaNaoConformidade, [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await GetViewListResult(idNaoConformidade, idCausaNaoConformidade, input);
        return output;
    }

    [HttpGet("causas/centros-custo/get-view-list")]
    public async Task<PagedResultDto<CentroCustoCausaNaoConformidadeViewOutput>> GetViewList([FromRoute] Guid idNaoConformidade,
        [FromQuery] PagedFilteredAndSortedRequestInput input)
    {
        var output = await GetViewListResult(idNaoConformidade, null, input);
        return output;
    }

    public async Task<PagedResultDto<CentroCustoCausaNaoConformidadeViewOutput>> GetViewListResult(Guid idNaoConformidade,
        Guid? idCausaNaoConformidade, PagedFilteredAndSortedRequestInput input)
    {
        var query = (from centroCustoCausaNaoConformidade in _centroCustoCausaNaoConformidades.AsNoTracking()
                .Where(centroCustoCausaNaoConformidade => centroCustoCausaNaoConformidade.CompanyId == _currentCompany.Id)
                .Where(centroCustoCausaNaoConformidade => centroCustoCausaNaoConformidade.IdNaoConformidade == idNaoConformidade)
                .WhereIf(idCausaNaoConformidade.HasValue, centroCustoCausaNaoConformidade => centroCustoCausaNaoConformidade.IdCausaNaoConformidade == idCausaNaoConformidade.Value)
            join centroCusto in _centroCustos.AsNoTracking()
                on centroCustoCausaNaoConformidade.IdCentroCusto equals centroCusto.Id
            select new CentroCustoCausaNaoConformidadeViewOutput
            {
                Id = centroCustoCausaNaoConformidade.Id,
                IdNaoConformidade = centroCustoCausaNaoConformidade.IdNaoConformidade,
                IsCentroCustoSintetico = centroCusto.IsSintetico,
                CodigoCentroCusto = centroCusto.Codigo,
                DescricaoCentroCusto = centroCusto.Descricao
            }).ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();
        var itens = await query.ToListAsync();

        return new PagedResultDto<CentroCustoCausaNaoConformidadeViewOutput>
        {
            TotalCount = totalCount,
            Items = itens
        };
    }
}
