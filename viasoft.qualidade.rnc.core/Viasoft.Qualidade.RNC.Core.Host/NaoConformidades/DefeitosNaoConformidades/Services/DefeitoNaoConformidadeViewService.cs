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
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Services;

public class DefeitoNaoConformidadeViewService: IDefeitoNaoConformidadeViewService, ITransientDependency
{
    private readonly IRepository<DefeitoNaoConformidade> _defeitoNaoConformidades;
    private readonly IRepository<Defeito> _defeitos;
    private readonly ICurrentCompany _currentCompany;


    public DefeitoNaoConformidadeViewService(IRepository<DefeitoNaoConformidade> defeitoNaoConformidades, IRepository<Defeito> defeitos,
        ICurrentCompany currentCompany)
    {
        _defeitoNaoConformidades = defeitoNaoConformidades;
        _defeitos = defeitos;
        _currentCompany = currentCompany;
    }

    public async Task<PagedResultDto<DefeitoNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade,
        PagedFilteredAndSortedRequestInput input)
    {
        var query = (from defeitoNaoConformidade in _defeitoNaoConformidades
                where defeitoNaoConformidade.CompanyId == _currentCompany.Id
                join defeito in _defeitos
                        on defeitoNaoConformidade.IdDefeito equals defeito.Id
                                into defeitoJoinedTable
                from defeito in defeitoJoinedTable.DefaultIfEmpty()
                    select new DefeitoNaoConformidadeViewOutput
                        {
                            Id = defeitoNaoConformidade.Id,
                            Codigo = defeito.Codigo,
                            Descricao = defeito.Descricao,
                            IdNaoConformidade = defeitoNaoConformidade.IdNaoConformidade,
                            IdDefeito = defeito.Id,
                            Quantidade = defeitoNaoConformidade.Quantidade,
                            Detalhamento = defeitoNaoConformidade.Detalhamento,
                        })
            .Where(entity => entity.IdNaoConformidade.Equals(idNaoConformidade))
            .AsNoTracking()
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);
           
        var totalCount = await query.CountAsync();
        var itens = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListAsync();

        var output = new PagedResultDto<DefeitoNaoConformidadeViewOutput>(totalCount, itens);
        return output;
    }
}