using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies;

public interface IBaseProxyService<TEntityOutput>
{
    public Task<List<TEntityOutput>> GetAllByIdsPaginando(List<Guid> ids);
    public Task<TEntityOutput> GetById(Guid id);
}