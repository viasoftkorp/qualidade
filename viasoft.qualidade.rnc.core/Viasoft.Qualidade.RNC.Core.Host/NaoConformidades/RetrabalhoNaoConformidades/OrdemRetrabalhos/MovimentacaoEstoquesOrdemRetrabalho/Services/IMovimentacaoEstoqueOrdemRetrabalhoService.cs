using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Services;

public interface IMovimentacaoEstoqueOrdemRetrabalhoService
{
    public Task<MovimentarEstoqueListaOutput> MovimentarEstoqueLista(NaoConformidade naoConformidade, 
        OrdemRetrabalhoNaoConformidade ordemRetrabalhoNaoConformidade);

    public Task<MovimentarEstoqueListaOutput> EstornarMovimentacaoEstoqueLista(NaoConformidade naoConformidade,
        OrdemRetrabalhoNaoConformidade ordemRetrabalhoNaoConformidade);
}