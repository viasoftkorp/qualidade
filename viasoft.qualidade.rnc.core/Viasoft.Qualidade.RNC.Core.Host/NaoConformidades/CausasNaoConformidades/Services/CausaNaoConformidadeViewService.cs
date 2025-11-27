using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Services;

public class CausaNaoConformidadeViewService : ICausaNaoConformidadeViewService, ITransientDependency
{
    private readonly IRepository<CausaNaoConformidade> _causaNaoConformidades;
    private readonly IRepository<Causa> _causa;
    private readonly ICurrentCompany _currentCompany;


    public CausaNaoConformidadeViewService(IRepository<CausaNaoConformidade> causaNaoConformidades,
        IRepository<Causa> causa, ICurrentCompany currentCompany)
    {
        _causaNaoConformidades = causaNaoConformidades;
        _causa = causa;
        _currentCompany = currentCompany;
    }

    public async Task<PagedResultDto<CausaNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade,
        Guid idDefeitoNaoConformidade, GetListWithDefeitoIdFlagInput input)
    {
        var query = (from causaNaoConformidade in _causaNaoConformidades
                where causaNaoConformidade.CompanyId == _currentCompany.Id
                join causa in _causa
                    on causaNaoConformidade.IdCausa equals causa.Id into causaJoinedTable
                from causa in causaJoinedTable.DefaultIfEmpty()
                select new CausaNaoConformidadeViewOutput
                {
                    Id = causaNaoConformidade.Id,
                    IdNaoConformidade = causaNaoConformidade.IdNaoConformidade,
                    IdDefeitoNaoConformidade = causaNaoConformidade.IdDefeitoNaoConformidade,
                    Detalhamento = causaNaoConformidade.Detalhamento,
                    IdCausa = causaNaoConformidade.IdCausa,
                    Codigo = causa.Codigo,
                    Descricao = causa.Descricao,
                })
            .Where(causa => causa.IdNaoConformidade.Equals(idNaoConformidade))
            .WhereIf(input.UsarIdDefeito,causa => causa.IdDefeitoNaoConformidade.Equals(idDefeitoNaoConformidade))
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting)
            .AsNoTracking();
       
        var totalCount = await query.CountAsync();
        if (input.UsarIdDefeito == false && totalCount < 50)
        {
            input.MaxResultCount = totalCount;
        }
        var itens = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListAsync();
        var output = new PagedResultDto<CausaNaoConformidadeViewOutput>(totalCount, itens);
        return output;
    }
}