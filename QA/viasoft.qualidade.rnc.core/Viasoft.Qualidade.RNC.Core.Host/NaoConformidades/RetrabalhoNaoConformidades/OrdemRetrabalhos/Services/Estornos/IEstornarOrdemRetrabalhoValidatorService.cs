using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services.Estornos;

public interface IEstornarOrdemRetrabalhoValidatorService
{
    public Task<EstornarOrdemRetrabalhoValidationResult> ValidateAsync(AgregacaoNaoConformidade agregacaoNaoConformidade);
    public IEstornarOrdemRetrabalhoValidatorService ValidateOdfRetrabalho();
    public IEstornarOrdemRetrabalhoValidatorService ValidateHistoricoApontamento();
    public IEstornarOrdemRetrabalhoValidatorService ValidateOrigemInspecaoSaida();
    public IEstornarOrdemRetrabalhoValidatorService ValidateStatusRnc();
}