using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.Produtos;

public interface IProdutosProxyService : IBaseProxyService<ProdutoOutput>
{
    public Task<ListResultDto<ProdutoOutput>> GetAll(PagedFilteredAndSortedRequestInput filter);
}