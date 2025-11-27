using System;
using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoqueLocais.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoqueLocais.Providers;

public interface IEstoqueLocalProvider
{
    public Task<PagedResultDto<EstoqueLocal>> GetList(GetListEstoqueLocalInput input);
    public Task<EstoqueLocal> GetById(Guid id);
    public Task<EstoqueLocal> GetByLegacyId(int legacyId);
}

public class GetListEstoqueLocalInput : PagedFilteredAndSortedRequestInput
{
    public Guid? IdEmpresa { get; set; }
}