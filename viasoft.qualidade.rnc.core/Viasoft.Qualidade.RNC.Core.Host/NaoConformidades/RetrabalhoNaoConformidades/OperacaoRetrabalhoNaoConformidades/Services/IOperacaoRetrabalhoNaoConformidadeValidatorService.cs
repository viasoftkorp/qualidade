using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Services;

public interface IOperacaoRetrabalhoNaoConformidadeValidatorService
{
    public OperacaoRetrabalhoNaoConformidadeValidationResult ValidateOperacaoRetrabalhoJaExistente(AgregacaoNaoConformidade naoConformidade);
    public OperacaoRetrabalhoNaoConformidadeValidationResult ValidateMaquina(OperacaoRetrabalhoNaoConformidadeInput operacaoRetrabalhoNaoConformidadeInput);
    public Task<OperacaoRetrabalhoNaoConformidadeValidationResult> ValidateOdfApontada(AgregacaoNaoConformidade agregacaoNaoConformidade);

    public Task<OperacaoRetrabalhoNaoConformidadeValidationResult> ValidateOdfAberta(
        AgregacaoNaoConformidade agregacaoNaoConformidade);

    public OperacaoRetrabalhoNaoConformidadeValidationResult ValidateStatusRnc(NaoConformidade naoConformidade);
}