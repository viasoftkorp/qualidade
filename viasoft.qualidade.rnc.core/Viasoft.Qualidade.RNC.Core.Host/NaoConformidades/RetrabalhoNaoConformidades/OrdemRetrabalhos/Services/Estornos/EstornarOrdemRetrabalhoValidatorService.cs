using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services.Estornos;

public class EstornarOrdemRetrabalhoValidatorService : IEstornarOrdemRetrabalhoValidatorService, ITransientDependency
{
    private readonly IRepository<OrdemRetrabalhoNaoConformidade> _ordemRetrabalhoNaoConformidadeRepository;
    private readonly IOrdemProducaoProvider _ordemProducaoProvider;
    private readonly IOperacaoService _operacaoService;
    private bool _isValidateOdfRetrabalho;
    private bool _isValidateHistoricoApontamento;
    private bool _isValidateOrigemInspecaoSaida;
    private bool _isValidateStatusRnc;
    
    public EstornarOrdemRetrabalhoValidatorService(IRepository<OrdemRetrabalhoNaoConformidade> ordemRetrabalhoNaoConformidadeRepository,
        IOrdemProducaoProvider ordemProducaoProvider, IOperacaoService operacaoService)
    {
        _ordemRetrabalhoNaoConformidadeRepository = ordemRetrabalhoNaoConformidadeRepository;
        _ordemProducaoProvider = ordemProducaoProvider;
        _operacaoService = operacaoService;
    }
    public async Task<EstornarOrdemRetrabalhoValidationResult> ValidateAsync(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        if (_isValidateStatusRnc)
        {
            if (agregacaoNaoConformidade.NaoConformidade.Status == StatusNaoConformidade.Fechado)
            {
                return EstornarOrdemRetrabalhoValidationResult.RncFechada;
            }
        }
        
        if (_isValidateOdfRetrabalho)
        {
            if (!await HasOdfRetrabalho(agregacaoNaoConformidade))
            {
                return EstornarOrdemRetrabalhoValidationResult.OdfRetrabalhoNaoEncontrada;
            }
        }

        if (_isValidateHistoricoApontamento)
        {
            if (await OdfJaApontada(agregacaoNaoConformidade))
            {
                return EstornarOrdemRetrabalhoValidationResult.OdfRetrabalhoJaApontada;
            }
        }

        if (_isValidateOrigemInspecaoSaida)
        {
            if (IsOrigemInspecaoSaida(agregacaoNaoConformidade))
            {
                return EstornarOrdemRetrabalhoValidationResult.RncComOrigemInspecaoSaida;
            }
        }
        return EstornarOrdemRetrabalhoValidationResult.Ok;
    }
    
    public IEstornarOrdemRetrabalhoValidatorService ValidateOdfRetrabalho()
    {
        _isValidateOdfRetrabalho = true;
        return this;
    }
    public IEstornarOrdemRetrabalhoValidatorService ValidateHistoricoApontamento()
    {
        _isValidateHistoricoApontamento = true;
        return this;
    }
    public IEstornarOrdemRetrabalhoValidatorService ValidateStatusRnc()
    {
        _isValidateStatusRnc = true;
        return this;
    }
    public IEstornarOrdemRetrabalhoValidatorService ValidateOrigemInspecaoSaida()
    {
        _isValidateOrigemInspecaoSaida = true;
        return this;
    }

    private async Task<bool> HasOdfRetrabalho(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        var ordemRetrabalhoNaoConformidade = await
            _ordemRetrabalhoNaoConformidadeRepository.FirstOrDefaultAsync(
                e => e.IdNaoConformidade == agregacaoNaoConformidade.NaoConformidade.Id);
        if (ordemRetrabalhoNaoConformidade == null)
        {
            return false;
        }
        var odfRetrabalho = await _ordemProducaoProvider.GetByNumeroOdf(ordemRetrabalhoNaoConformidade.NumeroOdfRetrabalho, false);
        
        return odfRetrabalho != null;
    }

    private async Task<bool> OdfJaApontada(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        var ordemRetrabalhoNaoConformidade = await
            _ordemRetrabalhoNaoConformidadeRepository.FirstAsync(
                e => e.IdNaoConformidade == agregacaoNaoConformidade.NaoConformidade.Id);
        
        var result = _operacaoService.ValidarOdfPossuiApontamento(ordemRetrabalhoNaoConformidade.NumeroOdfRetrabalho);

        return await result;
    }

    private bool IsOrigemInspecaoSaida(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        return agregacaoNaoConformidade.NaoConformidade.Origem == OrigemNaoConformidade.InpecaoSaida;
    }
    
}