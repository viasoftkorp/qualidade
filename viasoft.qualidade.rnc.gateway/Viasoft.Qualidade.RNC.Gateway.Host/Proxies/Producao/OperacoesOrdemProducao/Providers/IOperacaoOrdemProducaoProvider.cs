using System.Threading.Tasks;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OperacoesOrdemProducao.Dtos;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OperacoesOrdemProducao.Providers;

public interface IOperacaoOrdemProducaoProvider
{
    Task<PagedResultDto<OperacaoOrdemProducaoDto>> GetList(GetListOrdemProducaoInput input);
}