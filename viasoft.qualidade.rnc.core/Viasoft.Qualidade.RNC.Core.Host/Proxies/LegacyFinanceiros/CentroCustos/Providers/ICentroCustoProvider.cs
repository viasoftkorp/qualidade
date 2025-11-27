using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFinanceiros.CentroCustos.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFinanceiros.CentroCustos.Providers;

public interface ICentroCustoProvider : IBaseProxyService<CentroCustoOutput>
{
    public Task<ListResultDto<CentroCustoOutput>> GetAll(PagedFilteredAndSortedRequestInput filter);
}