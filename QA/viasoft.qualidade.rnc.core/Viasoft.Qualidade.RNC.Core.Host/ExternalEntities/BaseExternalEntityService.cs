using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Entities;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Qualidade.RNC.Core.Host.Proxies;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalEntities;

public abstract class BaseExternalEntityService<TEntity, TEntityProviderOutput> : IBaseExternalEntityService<TEntity> where TEntity : Entity
{
    private readonly IRepository<TEntity> _repository;
    private readonly IBaseProxyService<TEntityProviderOutput> _provider;
    private readonly IUnitOfWork _unitOfWork;

    protected abstract TEntity ParseProviderOutputToEntity(TEntityProviderOutput outputFromProvider);

    protected virtual Task InserirSubEntidades(TEntity entity)
    {
        return Task.CompletedTask;
    }
    
    protected virtual Task BatchInserirSubEntidades(List<TEntityProviderOutput> entities)
    {
        return Task.CompletedTask;
    }

    protected BaseExternalEntityService(IRepository<TEntity> repository, IBaseProxyService<TEntityProviderOutput> provider,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _provider = provider;
        _unitOfWork = unitOfWork;
    }
    public virtual async Task InserirSeNaoCadastrado(Guid idEntidade)
    {
        await InserirSeNaoCadastrado(idEntidade, entidade => entidade.Id == idEntidade);
    }

    public virtual async Task InserirSeNaoCadastrado(Guid idEntidade, Expression<Func<TEntity, bool>> expressao)
    {
        var entidadeJaInserida = await _repository
            .AsNoTracking()
            .Where(expressao)
            .AnyAsync();

        if (entidadeJaInserida)
        {
            return;
        }    
        
        var entityFromProvider = await _provider.GetById(idEntidade);

        var entityInput = ParseProviderOutputToEntity(entityFromProvider);
        
        await _repository.InsertAsync(entityInput, true);

        await InserirSubEntidades(entityInput);

    }

    public async Task BatchInserirNaoCadastrados(List<Guid> idsEntidades)
    {
        var idsEntidadesInseridas = await _repository
            .AsNoTracking()
            .Where(e => idsEntidades.Contains(e.Id))
            .Select(e => e.Id)
            .ToListAsync();

        var idsEntidadesNaoInseridas = idsEntidades.Except(idsEntidadesInseridas).ToList();

        if (!idsEntidadesNaoInseridas.Any())
        {
            return;
        }
        
        var entitiesFromProvider = await _provider.GetAllByIdsPaginando(idsEntidadesNaoInseridas);
        
        using (_unitOfWork.Begin())
        {
            foreach (var entityFromProvider in entitiesFromProvider)
            {
                var entityInput = ParseProviderOutputToEntity(entityFromProvider);
        
                await _repository.InsertAsync(entityInput);
            }
            
            await _unitOfWork.CompleteAsync();
        }

        await BatchInserirSubEntidades(entitiesFromProvider);
    }
}