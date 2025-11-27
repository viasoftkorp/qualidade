using System.Linq;
using System.Threading.Tasks;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Services;

public class OperacaoRetrabalhoNaoConformidadeValidatorService : IOperacaoRetrabalhoNaoConformidadeValidatorService, ITransientDependency
{
    private readonly IOperacaoService _operacaoService;
    private readonly IOrdemProducaoProvider _ordemProducaoProvider;

    public OperacaoRetrabalhoNaoConformidadeValidatorService(IOperacaoService operacaoService, 
        IOrdemProducaoProvider ordemProducaoProvider)
    {
        _operacaoService = operacaoService;
        _ordemProducaoProvider = ordemProducaoProvider;
    }

    public OperacaoRetrabalhoNaoConformidadeValidationResult ValidateStatusRnc(NaoConformidade naoConformidade)
    {
        if (naoConformidade.Status == StatusNaoConformidade.Fechado)
        {
            return OperacaoRetrabalhoNaoConformidadeValidationResult.RncFechada;
        }

        return OperacaoRetrabalhoNaoConformidadeValidationResult.Ok;
    }
    public OperacaoRetrabalhoNaoConformidadeValidationResult ValidateOperacaoRetrabalhoJaExistente(AgregacaoNaoConformidade naoConformidade)
    {
        if (naoConformidade.NaoConformidade.OperacaoRetrabalho != null)
        {
            return OperacaoRetrabalhoNaoConformidadeValidationResult.OperacaoRetrabalhoJaExiste;
        }

        return OperacaoRetrabalhoNaoConformidadeValidationResult.Ok;
    }
    
    public OperacaoRetrabalhoNaoConformidadeValidationResult ValidateMaquina(OperacaoRetrabalhoNaoConformidadeInput operacaoRetrabalhoNaoConformidadeInput)
    {
        if(!operacaoRetrabalhoNaoConformidadeInput.Maquinas.Any())
        {
            return OperacaoRetrabalhoNaoConformidadeValidationResult.NenhumMaquinaCadastrada;
        }

        return OperacaoRetrabalhoNaoConformidadeValidationResult.Ok;
    }
    
    public async Task<OperacaoRetrabalhoNaoConformidadeValidationResult> ValidateOdfApontada(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        var numeroOdf = agregacaoNaoConformidade.NaoConformidade.NumeroOdf.Value;
        
        var odfPossuiApontamento = await _operacaoService.ValidarOdfPossuiApontamento(numeroOdf);

        if (!odfPossuiApontamento)
        {
            return OperacaoRetrabalhoNaoConformidadeValidationResult.OdfNaoApontada;
        }

        return OperacaoRetrabalhoNaoConformidadeValidationResult.Ok;
    }
    public async Task<OperacaoRetrabalhoNaoConformidadeValidationResult> ValidateOdfAberta(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        var numeroOdf = agregacaoNaoConformidade.NaoConformidade.NumeroOdf.Value;
        
        var ordemProducao = await _ordemProducaoProvider.GetByNumeroOdf(numeroOdf, false);

        if (ordemProducao.OdfFinalizada)
        {
            return OperacaoRetrabalhoNaoConformidadeValidationResult.OdfFinalizada;
        }

        return OperacaoRetrabalhoNaoConformidadeValidationResult.Ok;
    }
}