using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Providers;

public interface ILocalProvider: IBaseProxyService<LocalOutput>
{
    public Task<LocalOutput> GetByCode(int code);
    public Task<ListResultDto<LocalOutput>> GetAll(PagedFilteredAndSortedRequestInput filter);
}