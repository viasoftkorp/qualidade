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
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Services;

public class SolucaoNaoConformidadeViewService : ISolucaoNaoConformidadeViewService, ITransientDependency
{
    private readonly IRepository<SolucaoNaoConformidade> _solucaoNaoConformidades;
    private readonly IRepository<Solucao> _solucao;
    private readonly IRepository<Usuario> _usuarios;
    private readonly ICurrentCompany _currentCompany;

    public SolucaoNaoConformidadeViewService(IRepository<SolucaoNaoConformidade> solucaoNaoConformidades,
        IRepository<Solucao> solucao, IRepository<Usuario> usuarios, ICurrentCompany currentCompany)
    {
        _solucaoNaoConformidades = solucaoNaoConformidades;
        _solucao = solucao;
        _usuarios = usuarios;
        _currentCompany = currentCompany;
    }

    public async Task<PagedResultDto<SolucaoNaoConformidadeViewOutput>> GetListView(Guid idNaoConformidade,
        Guid idDefeito, GetListWithDefeitoIdFlagInput input)
    {
        var query = (from solucoesNaoConformidade in _solucaoNaoConformidades
                where solucoesNaoConformidade.CompanyId == _currentCompany.Id
                join solucoes in _solucao
                        on solucoesNaoConformidade.IdSolucao equals solucoes.Id into solucaoJoinedTable
                from solucao in solucaoJoinedTable.DefaultIfEmpty()
                join responsavel in _usuarios.AsNoTracking()
                    on solucoesNaoConformidade.IdResponsavel equals responsavel.Id
                    into responsavelJoinedTable
                from responsavel in responsavelJoinedTable.DefaultIfEmpty()
                join auditor in _usuarios.AsNoTracking()
                    on solucoesNaoConformidade.IdAuditor equals auditor.Id
                    into auditorJoinedTable
                from auditor in auditorJoinedTable.DefaultIfEmpty()
                    select  new SolucaoNaoConformidadeViewOutput
                {
                    Id = solucoesNaoConformidade.Id,
                    IdNaoConformidade = solucoesNaoConformidade.IdNaoConformidade,
                    IdDefeitoNaoConformidade = solucoesNaoConformidade.IdDefeitoNaoConformidade,
                    SolucaoImediata = solucoesNaoConformidade.SolucaoImediata,
                    DataAnalise = solucoesNaoConformidade.DataAnalise,
                    DataPrevistaImplantacao = solucoesNaoConformidade.DataPrevistaImplantacao,
                    IdResponsavel = solucoesNaoConformidade.IdResponsavel,
                    CustoEstimado = solucoesNaoConformidade.CustoEstimado,
                    NovaData = solucoesNaoConformidade.NovaData,
                    Responsavel = $"{responsavel.Nome} {responsavel.Sobrenome}",
                    Auditor = $"{auditor.Nome} {auditor.Sobrenome}",
                    DataVerificacao = solucoesNaoConformidade.DataVerificacao,
                    IdSolucao = solucoesNaoConformidade.IdSolucao,
                    Detalhamento = solucoesNaoConformidade.Detalhamento,
                    Codigo = solucao.Codigo,
                    Descricao = solucao.Descricao,
                    IdAuditor = solucoesNaoConformidade.IdAuditor,
                })
            .Where(entity => entity.IdNaoConformidade.Equals(idNaoConformidade))
            .WhereIf(input.UsarIdDefeito, entity => entity.IdDefeitoNaoConformidade.Equals(idDefeito))
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
        var output = new PagedResultDto<SolucaoNaoConformidadeViewOutput>(totalCount, itens);
        return output;
    }
}