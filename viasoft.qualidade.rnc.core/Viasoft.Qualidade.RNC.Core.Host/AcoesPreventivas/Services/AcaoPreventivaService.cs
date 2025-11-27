using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Services;

public class AcaoPreventivaService : IAcaoPreventivaService, ITransientDependency
{
    private readonly IRepository<AcaoPreventiva> _acaoPreventiva;
    private readonly IRepository<AcaoPreventivaNaoConformidade> _acaoPreventivaNaoConformidades;
    private readonly IRepository<Usuario> _usuarios;

    public AcaoPreventivaService(IRepository<AcaoPreventiva> acaoPreventiva, IRepository<AcaoPreventivaNaoConformidade> acaoPreventivaNaoConformidades,
        IRepository<Usuario> usuarios)
    {
        _acaoPreventiva = acaoPreventiva;
        _acaoPreventivaNaoConformidades = acaoPreventivaNaoConformidades;
        _usuarios = usuarios;
    }

    public async Task<AcaoPreventivaOutput> Get(Guid id)
    {
        var acaoPreventiva = await _acaoPreventiva.FirstOrDefaultAsync(e => e.Id == id);

        if (acaoPreventiva == null)
        {
            return null;
        }

        var output = new AcaoPreventivaOutput(acaoPreventiva);
        return output;
    }

    public async Task<PagedResultDto<AcaoPreventivaOutput>> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var query = _acaoPreventiva
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);
        var totalCount = await query.CountAsync();

        var acoesPreventivas = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .Select(acaoPreventiva => new AcaoPreventivaOutput(acaoPreventiva))
            .ToListAsync();

        return new PagedResultDto<AcaoPreventivaOutput>
        {
            TotalCount = totalCount,
            Items = acoesPreventivas
        };
    }
    public async Task<PagedResultDto<AcaoPreventivaViewOutput>> GetViewList(PagedFilteredAndSortedRequestInput input)
    {
        var query = (from acaoPreventiva in _acaoPreventiva.AsNoTracking()
                join usuarios in _usuarios.AsNoTracking()
                    on acaoPreventiva.IdResponsavel equals usuarios.Id
                    into usuariosJoinedTable
                from responsavel in usuariosJoinedTable.DefaultIfEmpty()
                select new AcaoPreventivaViewOutput
                {
                    Id = acaoPreventiva.Id,
                    Codigo = acaoPreventiva.Codigo,
                    Descricao = acaoPreventiva.Descricao,
                    Detalhamento = acaoPreventiva.Detalhamento,
                    IsAtivo = acaoPreventiva.IsAtivo,
                    IdResponsavel = acaoPreventiva.IdResponsavel,
                    NomeResponsavel = $"{responsavel.Nome} {responsavel.Sobrenome}"
                })
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);
        var totalCount = await query.CountAsync();

        var acoesPreventivas = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListAsync();

        return new PagedResultDto<AcaoPreventivaViewOutput>
        {
            TotalCount = totalCount,
            Items = acoesPreventivas
        };
    }

    public async Task<ValidationResult> Create(AcaoPreventivaInput input)
    {
        var acaoPreventiva = new AcaoPreventiva(input);

        await _acaoPreventiva.InsertAsync(acaoPreventiva, true);

        return ValidationResult.Ok;
    }

    public async Task<ValidationResult> Update(Guid id, AcaoPreventivaInput input)
    {
        var acaoPreventiva = await _acaoPreventiva.FirstOrDefaultAsync(e => e.Id == id);

        if (acaoPreventiva == null)
        {
            return ValidationResult.NotFound;
        }

        acaoPreventiva.Update(input);

        await _acaoPreventiva.UpdateAsync(acaoPreventiva, true);
        return ValidationResult.Ok;
    }

    public async Task<ValidationResult> Delete(Guid id)
    {
        var entidadeUtilizada = await _acaoPreventivaNaoConformidades.AnyAsync(e => e.IdAcaoPreventiva == id);

        if (entidadeUtilizada)
        {
            return ValidationResult.EntidadeEmUso;
        }
        var acaoPreventiva = await _acaoPreventiva.FirstOrDefaultAsync(e => e.Id == id);
        
        if (acaoPreventiva == null)
        {
            return ValidationResult.NotFound;
        }

        await _acaoPreventiva.DeleteAsync(acaoPreventiva, true);

        return ValidationResult.Ok;
    }
    public async Task<ValidationResult> ChangeStatus(Guid id, bool isAtivo)
    {
        var entidade = await _acaoPreventiva.FindAsync(id);
        entidade.IsAtivo = isAtivo;
        
        await _acaoPreventiva.UpdateAsync(entidade, true);
        return ValidationResult.Ok;
    }
}