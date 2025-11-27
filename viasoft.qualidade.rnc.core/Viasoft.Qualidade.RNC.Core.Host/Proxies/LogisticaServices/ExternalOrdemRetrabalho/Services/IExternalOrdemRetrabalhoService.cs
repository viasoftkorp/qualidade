using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Services;

public interface IExternalOrdemRetrabalhoService
{
    public Task<ExternalGerarOrdemRetrabalhoOutput> GerarOrdemRetrabalho(ExternalGerarOrdemRetrabalhoInput input);
    Task<OrdemRetrabalhoNaoConformidadeOutput> EstornarOrdemRetrabalho(ExternalEstornarOrdemRetrabalhoInput input);
}