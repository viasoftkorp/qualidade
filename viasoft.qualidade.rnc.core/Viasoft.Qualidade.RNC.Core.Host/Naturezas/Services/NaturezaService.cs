using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Naturezas.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Naturezas.Services;

public class NaturezaService : INaturezaService, ITransientDependency
{
    private readonly IRepository<Natureza> _naturezas;
    private readonly IRepository<NaoConformidade> _naoConformidades;

    public NaturezaService(IRepository<Natureza> natureza, IRepository<NaoConformidade> naoConformidades)
    {
        _naturezas = natureza;
        _naoConformidades = naoConformidades;
    }

    public async Task<NaturezaOutput> Get(Guid id)
    {
        var natureza = await _naturezas.FirstOrDefaultAsync(e => e.Id == id);
        if (natureza == null)
        {
            return null;
        }

        var output = new NaturezaOutput(natureza);
        return output;
    }
    
    public async Task<PagedResultDto<NaturezaViewOutput>> GetViewList(PagedFilteredAndSortedRequestInput input)
    {
        var query = _naturezas
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);
        
        var totalCount = await query.CountAsync();

        var natureza = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .Select(natureza => new NaturezaViewOutput(natureza))
            .ToListAsync();

        return new PagedResultDto<NaturezaViewOutput>
        {
            TotalCount = totalCount,
            Items = natureza
        };
    }
    
    public async Task<PagedResultDto<NaturezaOutput>> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var query = _naturezas
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);
        
        var totalCount = await query.CountAsync();

        var natureza = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .Select(natureza => new NaturezaOutput(natureza))
            .ToListAsync();

        return new PagedResultDto<NaturezaOutput>
        {
            TotalCount = totalCount,
            Items = natureza
        };
    }

    public async Task<ValidationResult> Create(NaturezaInput input)
    {
        var natureza = new Natureza(input);
        
        await _naturezas.InsertAsync(natureza, true);
        return ValidationResult.Ok;
    }


    public async Task<ValidationResult> Update(Guid id, NaturezaInput input)
    {
        var entity = await _naturezas.FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null)
        {
            return ValidationResult.NotFound;
        }

        entity.Update(input);

        await _naturezas.UpdateAsync(entity, true);
        return ValidationResult.Ok;
    }


    public async Task<ValidationResult> Delete(Guid id)
    {
        var entidadeEmUso = await _naoConformidades.AnyAsync(e => e.IdNatureza == id);
        if (entidadeEmUso)
        {
            return ValidationResult.EntidadeEmUso;
        }
        var natureza = await _naturezas.FirstOrDefaultAsync(e => e.Id == id);
        if (natureza == null )
        {
            return ValidationResult.NotFound;
        }

        await _naturezas.DeleteAsync(natureza, true);
        
        return ValidationResult.Ok;
    }

    public async Task<ValidationResult> ChangeStatus(Guid id, bool isAtivo)
    {
        var entidade = await _naturezas.FindAsync(id);
        entidade.IsAtivo = isAtivo;
        
        await _naturezas.UpdateAsync(entidade, true);
        return ValidationResult.Ok;
    }
}
