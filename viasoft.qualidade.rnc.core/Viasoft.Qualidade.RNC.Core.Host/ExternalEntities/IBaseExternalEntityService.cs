using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Entities;

namespace Viasoft.Qualidade.RNC.Core.Host.ExternalEntities;

public interface IBaseExternalEntityService<TEntity> where TEntity : Entity
{
    public Task BatchInserirNaoCadastrados(List<Guid> idsEntidades);
    public Task InserirSeNaoCadastrado(Guid idEntidade);
    public Task InserirSeNaoCadastrado(Guid idEntidade, Expression<Func<TEntity, bool>> expressao);
}