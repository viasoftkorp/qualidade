using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.ProdutosEmpresas.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsProducts.ProdutosEmpresas;

public interface IProdutoEmpresaProxyService : IBaseProxyService<ProdutoEmpresaOutput>
{
    Task<PagedResultDto<ProdutoEmpresaOutput>> GetAll(GetProdutoEmpresaListInput input);
}