using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos.Events;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.Defeitos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Defeitos.Services;

public class DefeitoService : IDefeitoService, ITransientDependency
{
    private readonly IRepository<Defeito> _defeitos;
    private readonly IServiceBus _serviceBus;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IRepository<DefeitoNaoConformidade> _defeitoNaoConformidades;
    private readonly IRepository<Solucao> _solucoes;
    private readonly IRepository<Causa> _causas;

    public DefeitoService(IRepository<Defeito> defeitos, IServiceBus serviceBus, ICurrentTenant currentTenant, 
        ICurrentEnvironment currentEnvironment, IDateTimeProvider dateTimeProvider,
        IRepository<DefeitoNaoConformidade> defeitoNaoConformidades, IRepository<Solucao> solucoes,
        IRepository<Causa> causas)
    {
        _defeitos = defeitos;
        _serviceBus = serviceBus;
        _currentTenant = currentTenant;
        _currentEnvironment = currentEnvironment;
        _dateTimeProvider = dateTimeProvider;
        _defeitoNaoConformidades = defeitoNaoConformidades;
        _solucoes = solucoes;
        _causas = causas;
    }
    
    public async Task<DefeitoOutput> Get(Guid id)
    {
        var defeito = await _defeitos.FirstOrDefaultAsync(e => e.Id == id);
        if (defeito == null)
        {
            return null;
        }

        var output = new DefeitoOutput(defeito);
        return output;
    }

    public async Task<PagedResultDto<DefeitoViewOutput>> GetViewList(PagedFilteredAndSortedRequestInput input)
    {
         var query = (from defeito in _defeitos.AsNoTracking()
                join solucoes in _solucoes.AsNoTracking()
                    on defeito.IdSolucao equals solucoes.Id
                    into solucoesJoinedTable
                from solucao in solucoesJoinedTable.DefaultIfEmpty()
                join causas in _causas.AsNoTracking()
                    on defeito.IdCausa equals causas.Id
                    into causasJoinedTable
                from causa in causasJoinedTable.DefaultIfEmpty()
                select new DefeitoViewOutput
                {
                    Id = defeito.Id,
                    Codigo = defeito.Codigo,
                    IdSolucao = defeito.IdSolucao,
                    IdCausa = defeito.IdCausa,
                    Descricao = defeito.Descricao,
                    Detalhamento = defeito.Detalhamento,
                    IsAtivo = defeito.IsAtivo,
                    CodigoCausa = causa.Codigo,
                    CodigoSolucao = solucao.Codigo,
                    DescricaoCausa = causa.Descricao,
                    DescricaoSolucao = solucao.Descricao
                })
             .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);
        
        var totalCount = await query.CountAsync();

        var itens = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListAsync();
        
        return new PagedResultDto<DefeitoViewOutput>
        {
            TotalCount = totalCount,
            Items = itens
        };
    }

    public async Task<PagedResultDto<DefeitoOutput>> GetList(PagedFilteredAndSortedRequestInput input)
    {
        var query = _defeitos
            .AsNoTracking()
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting)
            .Select(e => new DefeitoOutput(e));
        
        var totalCount = await query.CountAsync();

        var itens = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListAsync();
        
        return new PagedResultDto<DefeitoOutput>
        {
            TotalCount = totalCount,
            Items = itens
        };
    }

    public async Task<ValidationResult> Create(DefeitoInput input)
    {
        var defeito = new Defeito(input);

        var defeitoInserido = await _defeitos.InsertAsync(defeito, true);
        await _serviceBus.Publish(new DefeitoCreated(defeitoInserido, Guid.NewGuid(),
            _dateTimeProvider.UtcNow(), _currentTenant.Id, _currentEnvironment.Id.Value));
        return ValidationResult.Ok;
    }

    public async Task<ValidationResult> Update(Guid id, DefeitoInput input)
    {
        var entity = await _defeitos.FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null)
        {
            return ValidationResult.NotFound;
        }

        entity.Update(input);

        var defeitoAlterado = await _defeitos.UpdateAsync(entity, true);
        await _serviceBus.Publish(new DefeitoUpdated(defeitoAlterado, Guid.NewGuid(),
            _dateTimeProvider.UtcNow(), _currentTenant.Id, _currentEnvironment.Id.Value));
        return ValidationResult.Ok;
    }

    public async Task<ValidationResult> Delete(Guid id)
    {
        var defeitoUtilizado = _defeitoNaoConformidades.Any(e => e.IdDefeito == id);
        if (defeitoUtilizado)
        {
            return ValidationResult.EntidadeEmUso;
        }
        var defeito = await _defeitos.FirstOrDefaultAsync(e => e.Id == id);
        if (defeito == null)
        {
            return ValidationResult.NotFound;
        }
        await _defeitos.DeleteAsync(defeito, true);
        await _serviceBus.Publish(new DefeitoDeleted(defeito, Guid.NewGuid(),
            _dateTimeProvider.UtcNow(), _currentTenant.Id, _currentEnvironment.Id.Value));

        return ValidationResult.Ok;
    }
    public async Task<ValidationResult> ChangeStatus(Guid id, bool isAtivo)
    {
        var entidade = await _defeitos.FindAsync(id);
        entidade.IsAtivo = isAtivo;
        
        await _defeitos.UpdateAsync(entidade, true);
        return ValidationResult.Ok;
    }
}