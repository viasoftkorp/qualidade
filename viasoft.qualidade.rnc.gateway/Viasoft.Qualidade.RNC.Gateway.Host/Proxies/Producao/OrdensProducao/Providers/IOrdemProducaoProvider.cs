using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Providers;

public interface IOrdemProducaoProvider
{
    public Task<PagedResultDto<OrdemProducaoOutput>> GetList(GetListOrdemProducaoInput input);
}
public class GetListOrdemProducaoInput : PagedFilteredAndSortedRequestInput
{
    public int NumeroOdf { get; set; }
    public bool FiltrarApenasEmitidas { get; set; }
}