using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services.Gerar;

public interface IGerarOrdemRetrabalhoValidatorService
{
    public Task<GerarOrdemRetrabalhoValidationResult> ValidateAsync(AgregacaoNaoConformidade agregacaoNaoConformidade);
    public IGerarOrdemRetrabalhoValidatorService ValidateOperacaoEngenhariaFinal();
    public IGerarOrdemRetrabalhoValidatorService ValidateOperacaoEngenhariaDuplicada();
    public IGerarOrdemRetrabalhoValidatorService ValidateOdf();
    public IGerarOrdemRetrabalhoValidatorService ValidateLote(OrdemRetrabalhoInput input);
    public IGerarOrdemRetrabalhoValidatorService ValidateQuantidade(OrdemRetrabalhoInput input);
    public IGerarOrdemRetrabalhoValidatorService ValidateStatusRnc();
}