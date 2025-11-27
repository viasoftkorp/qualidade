using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Services;

public interface IExternalMovimentacaoService
{
    public Task<ExternalMovimentarEstoqueItemOutput> MovimentarEstoqueLista(ExternalMovimentarEstoqueListaInput input);
}