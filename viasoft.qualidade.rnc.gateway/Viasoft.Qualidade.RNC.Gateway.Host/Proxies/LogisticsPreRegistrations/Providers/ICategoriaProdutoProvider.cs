using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.LogisticsPreRegistrations.Providers;

public interface ICategoriaProdutoProvider
{
    Task<PagedResultDto<CategoriaProdutoOutput>> GetList(PagedFilteredAndSortedRequestInput input);
}