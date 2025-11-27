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
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;

public class AcaoPreventivaNaoConformidadeViewService : IAcaoPreventivaNaoConformidadeViewService, ITransientDependency
{
    private readonly IRepository<AcaoPreventivaNaoConformidade> _acaoPreventivaNaoConformidades;
    private readonly IRepository<AcaoPreventiva> _acaoPreventivas;
    private readonly IRepository<Usuario> _usuarios;
    private readonly ICurrentCompany _currentCompany;

    public AcaoPreventivaNaoConformidadeViewService(
        IRepository<AcaoPreventivaNaoConformidade> acaoPreventivaNaoConformidades,
        IRepository<AcaoPreventiva> acaoPreventivas, IRepository<Usuario> usuarios,
        ICurrentCompany currentCompany)
    {
        _acaoPreventivaNaoConformidades = acaoPreventivaNaoConformidades;
        _acaoPreventivas = acaoPreventivas;
        _usuarios = usuarios;
        _currentCompany = currentCompany;
    }

    public async Task<PagedResultDto<AcaoPreventivaNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade,
        Guid idDefeitoNaoConformidade,
        GetListWithDefeitoIdFlagInput input)
    {
        var query = (from acoesPreventivasNaoConformidade in _acaoPreventivaNaoConformidades
                where acoesPreventivasNaoConformidade.CompanyId == _currentCompany.Id
                join acoesPreventivas in _acaoPreventivas
                    on acoesPreventivasNaoConformidade.IdAcaoPreventiva equals acoesPreventivas.Id
                    into acoesJoinedTable
                from acoesPreventivas in acoesJoinedTable.DefaultIfEmpty()
                join responsavel in _usuarios.AsNoTracking()
                    on acoesPreventivasNaoConformidade.IdResponsavel equals responsavel.Id
                    into responsavelJoinedTable
                from responsavel in responsavelJoinedTable.DefaultIfEmpty()
                join auditor in _usuarios.AsNoTracking()
                    on acoesPreventivasNaoConformidade.IdAuditor equals auditor.Id
                    into auditorJoinedTable
                from auditor in auditorJoinedTable.DefaultIfEmpty()
                select new AcaoPreventivaNaoConformidadeViewOutput
                {
                    Id = acoesPreventivasNaoConformidade.Id,
                    IdNaoConformidade = acoesPreventivasNaoConformidade.IdNaoConformidade,
                    IdDefeitoNaoConformidade = acoesPreventivasNaoConformidade.IdDefeitoNaoConformidade,
                    IdAcaoPreventiva = acoesPreventivasNaoConformidade.IdAcaoPreventiva,
                    Descricao = acoesPreventivas.Descricao,
                    Codigo = acoesPreventivas.Codigo,
                    Acao = acoesPreventivasNaoConformidade.Acao,
                    Responsavel = $"{responsavel.Nome} {responsavel.Sobrenome}",
                    Auditor = $"{auditor.Nome} {auditor.Sobrenome}",
                    Detalhamento = acoesPreventivasNaoConformidade.Detalhamento,
                    IdResponsavel = acoesPreventivasNaoConformidade.IdResponsavel,
                    DataAnalise = acoesPreventivasNaoConformidade.DataAnalise,
                    DataPrevistaImplantacao = acoesPreventivasNaoConformidade.DataPrevistaImplantacao,
                    IdAuditor = acoesPreventivasNaoConformidade.IdAuditor,
                    Implementada = acoesPreventivasNaoConformidade.Implementada,
                    DataVerificacao = acoesPreventivasNaoConformidade.DataVerificacao,
                    NovaData = acoesPreventivasNaoConformidade.NovaData
                })
            .Where(acao => acao.IdNaoConformidade.Equals(idNaoConformidade))
            .WhereIf(input.UsarIdDefeito,acao => acao.IdDefeitoNaoConformidade.Equals(idDefeitoNaoConformidade))
            .AsNoTracking()
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);
        var totalCount = await query.CountAsync();
        if (input.UsarIdDefeito == false && totalCount < 50)
        {
            input.MaxResultCount = totalCount;
        }
        var itens = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListAsync();
        var output = new PagedResultDto<AcaoPreventivaNaoConformidadeViewOutput>(totalCount, itens);
        return output;
    }
}