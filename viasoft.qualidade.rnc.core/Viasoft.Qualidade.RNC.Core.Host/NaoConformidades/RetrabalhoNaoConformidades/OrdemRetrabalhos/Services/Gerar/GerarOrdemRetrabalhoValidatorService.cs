using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.Configuracoes.Gerais;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.EstoqueLocais;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyParametros.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services.Gerar;

public class GerarOrdemRetrabalhoValidatorService : IGerarOrdemRetrabalhoValidatorService, ITransientDependency
{
    private OrdemRetrabalhoInput OrdemRetrabalhoInput { get; set; }
    private readonly IRepository<OrdemRetrabalhoNaoConformidade> _ordemRetrabalhoNaoConformidadeRepository;
    private readonly IRepository<ConfiguracaoGeral> _configuracaoGerais;
    private readonly ILegacyParametrosProvider _legacyParametrosProvider;
    private readonly IEstoqueLocalAclService _estoqueLocalAclService;
    private readonly IOrdemProducaoProvider _ordemProducaoProvider;
    private readonly IOperacaoService _operacaoService;
    private bool _isValidateOperacaoEngenhariaFinal;
    private bool _isValidateOperacaoEngenhariaDuplicada;
    private bool _isValidateOdf;
    private bool _isValidateLote;
    private bool _isValidateQuantidade;
    private bool _isValidateStatusRnc;

    public GerarOrdemRetrabalhoValidatorService(IRepository<OrdemRetrabalhoNaoConformidade> ordemRetrabalhoNaoConformidadeRepository,
        IRepository<ConfiguracaoGeral> configuracaoGerais, ILegacyParametrosProvider legacyParametrosProvider,
        IEstoqueLocalAclService estoqueLocalAclService, IOrdemProducaoProvider ordemProducaoProvider, IOperacaoService operacaoService)
    {
        _ordemRetrabalhoNaoConformidadeRepository = ordemRetrabalhoNaoConformidadeRepository;
        _configuracaoGerais = configuracaoGerais;
        _legacyParametrosProvider = legacyParametrosProvider;
        _estoqueLocalAclService = estoqueLocalAclService;
        _ordemProducaoProvider = ordemProducaoProvider;
        _operacaoService = operacaoService;
    }

    public async Task<GerarOrdemRetrabalhoValidationResult> ValidateAsync(
        AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        if (_isValidateStatusRnc)
        {
            if (agregacaoNaoConformidade.NaoConformidade.Status == StatusNaoConformidade.Fechado)
            {
                return GerarOrdemRetrabalhoValidationResult.RncFechada;
            }
        }
        if (_isValidateOdf)
        {
            if (!HasNumeroOdf(agregacaoNaoConformidade))
            {
                return GerarOrdemRetrabalhoValidationResult.OdfObrigatorio;
            }

            if (await HasOdfRetrabalho(agregacaoNaoConformidade))
            {
                return GerarOrdemRetrabalhoValidationResult.OdfRetrabalhoJaGerada;
            }

            if (!await IsOdfClosed(agregacaoNaoConformidade))
            {
                return GerarOrdemRetrabalhoValidationResult.OdfNaoFinalizada;
            }
        }

        if (_isValidateLote)
        {
            if (!HasNumeroLote(agregacaoNaoConformidade))
            {
                return GerarOrdemRetrabalhoValidationResult.LoteObrigatorio;
            }

            var configuracoes = await _configuracaoGerais.FirstAsync();
            if (configuracoes.ConsiderarApenasSaldoApontado)
            {
                if (!await OdfApontada(agregacaoNaoConformidade))
                {
                    return GerarOrdemRetrabalhoValidationResult.OdfNaoApontada;
                }
            }
        }
        
        if (_isValidateOperacaoEngenhariaFinal)
        {
            if (!OperacaoFinalValida(agregacaoNaoConformidade))
            {
                return GerarOrdemRetrabalhoValidationResult.OperacaoFinalNaoEncontrada;
            }
        }
        
        if (_isValidateOperacaoEngenhariaDuplicada)
        {
            if (HasOperacaoEngenhariaDuplicada(agregacaoNaoConformidade))
            {
                return GerarOrdemRetrabalhoValidationResult.OperacaoEngenhariaDuplicada;
            }
        }

        if (_isValidateQuantidade)
        {
            var isUtilizarReservaDePedidoNaLocalizacaoDeEstoque =
                await _legacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque();

            var quantidadeValida = true;
            if (isUtilizarReservaDePedidoNaLocalizacaoDeEstoque)
            {
                quantidadeValida = await ValidarQuantidadeConsiderandoReserva();
            }
            else if(OrdemRetrabalhoInput != null)
            {
                quantidadeValida = await ValidarQuantidade();
            }

            if (!quantidadeValida)
            {
                return GerarOrdemRetrabalhoValidationResult.QuantidadeInvalida;

            }
        }

        return GerarOrdemRetrabalhoValidationResult.Ok;
    }

    public IGerarOrdemRetrabalhoValidatorService ValidateOperacaoEngenhariaFinal()
    {
        _isValidateOperacaoEngenhariaFinal = true;
        return this;
    }

    public IGerarOrdemRetrabalhoValidatorService ValidateOperacaoEngenhariaDuplicada()
    {
        _isValidateOperacaoEngenhariaDuplicada = true;
        return this;
    }

    public IGerarOrdemRetrabalhoValidatorService ValidateOdf()
    {
        _isValidateOdf = true;
        return this;
    }
    public IGerarOrdemRetrabalhoValidatorService ValidateStatusRnc()
    {
        _isValidateStatusRnc = true;
        return this;
    }

    public IGerarOrdemRetrabalhoValidatorService ValidateLote(OrdemRetrabalhoInput input)
    {
        if (OrdemRetrabalhoInput == null)
        {
            OrdemRetrabalhoInput = input;
        }

        _isValidateLote = true;
        return this;
    }

    public IGerarOrdemRetrabalhoValidatorService ValidateQuantidade(OrdemRetrabalhoInput input)
    {
        if (OrdemRetrabalhoInput == null)
        {
            OrdemRetrabalhoInput = input;
        }

        _isValidateQuantidade = true;
        return this;
    }

    private bool OperacaoFinalValida(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        var servicos = agregacaoNaoConformidade.ServicoNaoConformidades;
        var hasServicoOperacaoFinal = servicos.Any(e => e.OperacaoEngenharia == "999");
        return hasServicoOperacaoFinal;
    }

    private bool HasOperacaoEngenhariaDuplicada(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        var servicos = agregacaoNaoConformidade.ServicoNaoConformidades;
        var hasOperacaoEngenhariaDuplicada = servicos
            .GroupBy(e => e.OperacaoEngenharia)
            .Any(grupo => grupo.Count() > 1);
        return hasOperacaoEngenhariaDuplicada;
    }

    private async Task<bool> HasOdfRetrabalho(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        var ordemRetrabalhoNaoConformidade = await
            _ordemRetrabalhoNaoConformidadeRepository.FirstOrDefaultAsync(
                e => e.IdNaoConformidade == agregacaoNaoConformidade.NaoConformidade.Id);
        return ordemRetrabalhoNaoConformidade != null;
    }
    
    private async Task<bool> IsOdfClosed(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        var numeroOdf = agregacaoNaoConformidade.NaoConformidade.NumeroOdf.Value;
        var ordemProducao = await _ordemProducaoProvider.GetByNumeroOdf(numeroOdf, false);

        return ordemProducao.OdfFinalizada;
    }

    private bool HasNumeroLote(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        var hasLote = !string.IsNullOrWhiteSpace(agregacaoNaoConformidade.NaoConformidade.NumeroLote);
        return hasLote;
    }

    private bool HasNumeroOdf(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        var hasNumeroOdf = agregacaoNaoConformidade.NaoConformidade.NumeroOdf.HasValue;
        return hasNumeroOdf;
    }
    
    private async Task<bool> ValidarQuantidade()
    {
        var estoqueLocal = await _estoqueLocalAclService.GetById(OrdemRetrabalhoInput.IdEstoqueLocalOrigem);
        var quantidadeValida = OrdemRetrabalhoInput.Quantidade <= estoqueLocal.Quantidade;
        return quantidadeValida;
        
    }

    private async Task<bool> ValidarQuantidadeConsiderandoReserva()
    {
        var estoqueLocalOrigem = await _estoqueLocalAclService.GetById(OrdemRetrabalhoInput.IdEstoqueLocalOrigem);
        
        var isQuantidadeValida = OrdemRetrabalhoInput.Quantidade <= estoqueLocalOrigem.Quantidade;
        return isQuantidadeValida;
    }
    private async Task<bool> OdfApontada(AgregacaoNaoConformidade agregacaoNaoConformidade)
    {
        var numeroOdf = agregacaoNaoConformidade.NaoConformidade.NumeroOdf.Value;
        
        var result = await _operacaoService.ValidarOdfPossuiApontamento(numeroOdf);

        return result;
    }
}