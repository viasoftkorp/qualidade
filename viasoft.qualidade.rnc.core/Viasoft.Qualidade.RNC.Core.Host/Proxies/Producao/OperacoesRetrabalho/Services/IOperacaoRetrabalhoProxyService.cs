using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Services;

public interface IOperacaoRetrabalhoProxyService
{
    public Task<GerarOperacaoRetrabalhoExternalOutput> Create(GerarOperacaoRetrabalhoExternalInput externalInput);
}
