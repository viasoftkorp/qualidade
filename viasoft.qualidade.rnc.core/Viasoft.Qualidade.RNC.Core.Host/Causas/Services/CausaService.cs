using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Causas.Services;

public class CausaService : ICausaService, ITransientDependency
{
    private readonly IRepository<Causa> _causas;
    private readonly IRepository<Defeito> _defeitos;
    private readonly IRepository<CausaNaoConformidade> _causaNaoConformidades;

    public CausaService(IRepository<Causa> causa, IRepository<Defeito> defeitos, 
        IRepository<CausaNaoConformidade> causaNaoConformidades)
    {
        _causas = causa;
        _defeitos = defeitos;
        _causaNaoConformidades = causaNaoConformidades;
    }

    public async Task<CausaOutput> Get(Guid id)
    {
        var causa = await _causas.FirstOrDefaultAsync(e => e.Id == id);
        if (causa == null)
        {
            return null;
        }
        var output = new CausaOutput(causa);
        return output;
    }

    public async Task<PagedResultDto<CausaOutput>> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var query = _causas
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();

        var causa = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .Select(causa => new CausaOutput(causa))
            .ToListAsync();

        return new PagedResultDto<CausaOutput>
        {
            TotalCount = totalCount,
            Items = causa
        };
    }
    
    public async Task<PagedResultDto<CausaViewOutput>> GetViewList(PagedFilteredAndSortedRequestInput input)
    {
        var query = _causas
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();

        var causa = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .Select(causa => new CausaViewOutput(causa))
            .ToListAsync();

        return new PagedResultDto<CausaViewOutput>
        {
            TotalCount = totalCount,
            Items = causa
        };
    }

    public async Task<ValidationResult> Create(CausaInput input)
    {
        var causa = new Causa(input);

        await _causas.InsertAsync(causa, true);
        return ValidationResult.Ok;
    }


    public async Task<ValidationResult> Update(Guid id, CausaInput input)
    {
        var entity = await _causas.FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null)
        {
            return ValidationResult.NotFound;
        }

        entity.Update(input);

        await _causas.UpdateAsync(entity, true);
        return ValidationResult.Ok;
    }


    public async Task<ValidationResult> Delete(Guid id)
    {
        var entidadeEmUsoPorDefeito = await _defeitos.AnyAsync(e => e.IdCausa == id);
        if (entidadeEmUsoPorDefeito)
        {
            return ValidationResult.EntidadeEmUso;
        }

        var entidadeEmUsoPorNaoConformidade = await _causaNaoConformidades.AnyAsync(e => e.IdCausa == id);
        if (entidadeEmUsoPorNaoConformidade)
        {
            return ValidationResult.EntidadeEmUso;
        }
        
        var causa = await _causas.FirstOrDefaultAsync(e => e.Id == id);
        if (causa == null)
        {
            return ValidationResult.NotFound;
        }

        await _causas.DeleteAsync(causa, true);

        return ValidationResult.Ok;
    }
    public async Task<ValidationResult> ChangeStatus(Guid id, bool isAtivo)
    {
        var entidade = await _causas.FindAsync(id);
        entidade.IsAtivo = isAtivo;
        
        await _causas.UpdateAsync(entidade, true);
        return ValidationResult.Ok;
    }
}

