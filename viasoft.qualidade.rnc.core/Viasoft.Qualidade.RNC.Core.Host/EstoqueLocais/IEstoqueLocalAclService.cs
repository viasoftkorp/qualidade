using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.EstoqueLocais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoqueLocais.Providers;

namespace Viasoft.Qualidade.RNC.Core.Host.EstoqueLocais;

public interface IEstoqueLocalAclService
{
    public Task<PagedResultDto<EstoqueLocalOutput>> GetList(GetListEstoqueLocalInput input);
    public Task<EstoqueLocalOutput> GetById(Guid id);
}