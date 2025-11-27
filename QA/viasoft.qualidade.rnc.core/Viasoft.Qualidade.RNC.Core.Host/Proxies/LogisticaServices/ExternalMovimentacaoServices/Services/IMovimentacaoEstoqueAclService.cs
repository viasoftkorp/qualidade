using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Services;

public interface IMovimentacaoEstoqueAclService
{
    public Task<ExternalMovimentarEstoqueListaInput> GetExternalMovimentarEstoqueListaInput(MovimentarEstoqueListaInput input);
    public MovimentarEstoqueListaOutput GetMovimentarEstoqueListaOutput(ExternalMovimentarEstoqueItemOutput input);
}