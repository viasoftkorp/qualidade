using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;

public interface INaoConformidadeValidationService
{
    public NaoConformidadeValidationResult ValidarChangeStatus(NaoConformidadeInput input);
    public NaoConformidadeValidationResult ValidarCampoCliente(NaoConformidadeInput input);
    public NaoConformidadeValidationResult ValidarCampoFornecedor(NaoConformidadeInput input);
    public Task<NaoConformidadeValidationResult> ValidarCampoOdf(NaoConformidadeInput input);
    public NaoConformidadeValidationResult ValidarCampoNotaFiscal(NaoConformidadeInput input);
    public Task<NaoConformidadeValidationResult> ValidarCampoProduto(NaoConformidadeInput input);
    public Task<NaoConformidadeValidationResult> ValidarCampoLote(NaoConformidadeInput input);
}