using System.Collections.Generic;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Dtos;

namespace Viasoft.Qualidade.RNC.Gateway.Host.Proxies.Producao.OrdensProducao.Providers;

public interface IOrdemProducaoProviderAclService
{
    public Task<List<OrdemProducaoOutput>> ProcessGetList(List<OrdemProducao> ordensProducao);
}