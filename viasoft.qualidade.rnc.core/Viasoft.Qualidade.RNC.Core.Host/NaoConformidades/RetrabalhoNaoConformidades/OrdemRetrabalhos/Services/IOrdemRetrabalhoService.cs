using System;
using System.Threading.Tasks;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services;

public interface IOrdemRetrabalhoService
{
    public Task<OrdemRetrabalhoNaoConformidadeOutput> GerarOrdemRetrabalho(AgregacaoNaoConformidade agregacaoNaoConformidade, OrdemRetrabalhoInput input);

    public Task<OrdemRetrabalhoNaoConformidadeOutput> EstornarOrdemRetrabalho(AgregacaoNaoConformidade agregacaoNaoConformidade,
        OrdemRetrabalhoNaoConformidade ordemRetrabalhoNaoConformidade, bool notificarUsuario);

    public Task<OrdemRetrabalhoNaoConformidadeOutput> Get(Guid idNaoConformidade);
    public Task<OrdemRetrabalhoNaoConformidadeViewOutput> GetView(Guid idNaoConformidade);

    public Task<GerarOrdemRetrabalhoValidationResult> CanGenerate(Guid idNaoConformidade, OrdemRetrabalhoInput input,
        bool isFullValidation);

    public Task<EstornarOrdemRetrabalhoValidationResult> CanEstornar(Guid idNaoConformidade);
}