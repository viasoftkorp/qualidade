using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos.Acls;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Services;

public interface IOrdemRetrabalhoAclService
{
    public Task<ExternalGerarOrdemRetrabalhoInput> GetExternalGerarOrdemRetrabalhoInput(GerarOrdemRetrabalhoInput input);
    public Task<ExternalEstornarOrdemRetrabalhoInput> GetExternalEstornarOrdemRetrabalhoInput(int numeroOdfOrigem,
        OrdemRetrabalhoNaoConformidade ordemRetrabalhoNaoConformidade);
}