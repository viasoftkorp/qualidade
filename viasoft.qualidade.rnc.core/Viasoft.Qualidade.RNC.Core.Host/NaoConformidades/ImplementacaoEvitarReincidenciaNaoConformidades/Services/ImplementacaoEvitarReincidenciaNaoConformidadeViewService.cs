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
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Services;

public class ImplementacaoEvitarReincidenciaNaoConformidadeViewService : IImplementacaoEvitarReincidenciaNaoConformidadeViewService, ITransientDependency
{
    private readonly IRepository<ImplementacaoEvitarReincidenciaNaoConformidade> _repository;
    private readonly IRepository<Usuario> _usuarios;
    private readonly ICurrentCompany _currentCompany;

    public ImplementacaoEvitarReincidenciaNaoConformidadeViewService(
        IRepository<ImplementacaoEvitarReincidenciaNaoConformidade> repository, IRepository<Usuario> usuarios,
        ICurrentCompany currentCompany)
    {
        _repository = repository;
        _usuarios = usuarios;
        _currentCompany = currentCompany;
    }

    public async Task<PagedResultDto<ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade,
        GetListViewInput input)
    {
        var query = (from acoesPreventivasNaoConformidade in _repository
                where acoesPreventivasNaoConformidade.CompanyId == _currentCompany.Id
                join responsavel in _usuarios.AsNoTracking()
                    on acoesPreventivasNaoConformidade.IdResponsavel equals responsavel.Id
                    into responsavelJoinedTable
                from responsavel in responsavelJoinedTable.DefaultIfEmpty()
                join auditor in _usuarios.AsNoTracking()
                    on acoesPreventivasNaoConformidade.IdAuditor equals auditor.Id
                    into auditorJoinedTable
                from auditor in auditorJoinedTable.DefaultIfEmpty()
                select new ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput
                {
                    Id = acoesPreventivasNaoConformidade.Id,
                    IdNaoConformidade = acoesPreventivasNaoConformidade.IdNaoConformidade,
                    IdResponsavel = acoesPreventivasNaoConformidade.IdResponsavel,
                    DataAnalise = acoesPreventivasNaoConformidade.DataAnalise,
                    DataPrevistaImplantacao = acoesPreventivasNaoConformidade.DataPrevistaImplantacao,
                    IdAuditor = acoesPreventivasNaoConformidade.IdAuditor,
                    DataVerificacao = acoesPreventivasNaoConformidade.DataVerificacao,
                    NovaData = acoesPreventivasNaoConformidade.NovaData,
                    AcaoImplementada = acoesPreventivasNaoConformidade.AcaoImplementada,
                    Descricao = acoesPreventivasNaoConformidade.Descricao,
                    IdDefeitoNaoConformidade = acoesPreventivasNaoConformidade.IdDefeitoNaoConformidade,
                    Responsavel = $"{responsavel.Nome} {responsavel.Sobrenome}",
                    Auditor = $"{auditor.Nome} {auditor.Sobrenome}"
                })
            .Where(acao => acao.IdNaoConformidade.Equals(idNaoConformidade))
            .WhereIf(input.IdDefeito.HasValue,e => e.IdDefeitoNaoConformidade == input.IdDefeito)
            .AsNoTracking()
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);
        var totalCount = await query.CountAsync();
        var itens = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListAsync();
        var output = new PagedResultDto<ImplementacaoEvitarReincidenciaNaoConformidadeViewOutput>(totalCount, itens);
        return output;
    }
}