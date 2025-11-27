using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.AspNetCore.Controller;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Commands;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services.Gerar;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Controllers;

    [ApiVersion(2024.1)]
    [ApiVersion(2024.2), ApiVersion(2024.3), ApiVersion(2024.4), ApiVersion(2025.1), ApiVersion(2025.2), ApiVersion(2025.3), ApiVersion(2025.4)]
    [ApiVersion(2026.1), ApiVersion(2026.2), ApiVersion(2026.3), ApiVersion(2026.4), ApiVersion(2027.1), ApiVersion(2027.2), ApiVersion(2027.3), ApiVersion(2027.4)]
    [ApiVersion(2028.1), ApiVersion(2028.2), ApiVersion(2028.3), ApiVersion(2028.4), ApiVersion(2029.1), ApiVersion(2029.2), ApiVersion(2029.3), ApiVersion(2029.4)]
    [Route("nao-conformidades/{idNaoConformidade:guid}/retrabalho/ordens")]
public class OrdemRetrabalhoController : BaseController
{
    private readonly IOrdemRetrabalhoService _ordemRetrabalhoService;
    private readonly IRepository<OrdemRetrabalhoNaoConformidade> _ordemRetrabalhoNaoConformidades;
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IServiceBus _serviceBus;
    private readonly IGerarOrdemRetrabalhoValidatorService _gerarOrdemRetrabalhoValidatorService;

    public OrdemRetrabalhoController(IOrdemRetrabalhoService ordemRetrabalhoService,
        IRepository<OrdemRetrabalhoNaoConformidade> ordemRetrabalhoNaoConformidades,
        INaoConformidadeRepository naoConformidadeRepository, IServiceBus serviceBus,
        IGerarOrdemRetrabalhoValidatorService gerarOrdemRetrabalhoValidatorService)
    {
        _ordemRetrabalhoService = ordemRetrabalhoService;
        _ordemRetrabalhoNaoConformidades = ordemRetrabalhoNaoConformidades;
        _naoConformidadeRepository = naoConformidadeRepository;
        _serviceBus = serviceBus;
        _gerarOrdemRetrabalhoValidatorService = gerarOrdemRetrabalhoValidatorService;
    }

    [HttpPost]
    public async Task<ActionResult<OrdemRetrabalhoNaoConformidadeOutput>> GerarOrdemRetrabalho([FromRoute] Guid idNaoConformidade,
        [FromBody] OrdemRetrabalhoInput input)
    {
        var agregacaoNaoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);

        var resultadoValidacoes = await VerificarPodeGerar(agregacaoNaoConformidade, input);
        if (!resultadoValidacoes.Success)
        {
            return UnprocessableEntity(resultadoValidacoes);
        }
        var result = await _ordemRetrabalhoService.GerarOrdemRetrabalho(agregacaoNaoConformidade, input);
        
        if (result.Success)
        {
            var movimentarEstoqueItemCommand = new MovimentarEstoqueOrdemRetrabalhoCommand
            {
                IdNaoConformidade = idNaoConformidade,
                IsEstorno = false
            };
            await _serviceBus.SendLocal(movimentarEstoqueItemCommand);
        }
        
        return result.Success ? Ok(result) : UnprocessableEntity(result);
    }

    [HttpDelete]
    public async Task<ActionResult<OrdemRetrabalhoNaoConformidadeOutput>> EstornarOrdemRetrabalho([FromRoute] Guid idNaoConformidade)
    {
        var resultadoValidacoes = await VerificarPodeEstornar(idNaoConformidade);

        if (!resultadoValidacoes.Success)
        {
            return UnprocessableEntity(resultadoValidacoes);
        }
        var agregacaoNaoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);

        var ordemRetrabalhoNaoConformidade =
            await _ordemRetrabalhoNaoConformidades.FirstAsync(e =>
                e.IdNaoConformidade == agregacaoNaoConformidade.NaoConformidade.Id);

        var result =
            await _ordemRetrabalhoService.EstornarOrdemRetrabalho(agregacaoNaoConformidade,
                ordemRetrabalhoNaoConformidade, true);
        
        if (result.Success)
        {                
            var movimentarEstoqueItemCommand = new MovimentarEstoqueOrdemRetrabalhoCommand
            {
                IdNaoConformidade = idNaoConformidade,
                IsEstorno = true
            };
            await _serviceBus.SendLocal(movimentarEstoqueItemCommand);
        }

        return result.Success ? Ok(result) : UnprocessableEntity(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid idNaoConformidade)
    {
        var ordemRetrabalho = await _ordemRetrabalhoService.Get(idNaoConformidade);

        return  ordemRetrabalho != null ? Ok(ordemRetrabalho) : Ok();
    }
    
    [HttpGet("GetView")]
    public async Task<IActionResult> GetView([FromRoute] Guid idNaoConformidade)
    {
        var ordemRetrabalho = await _ordemRetrabalhoService.GetView(idNaoConformidade);

        return  ordemRetrabalho != null ? Ok(ordemRetrabalho) : Ok();
    }

    [HttpGet("can-generate")]
    public async Task<IActionResult> CanGenerateOrdemRetrabalho([FromRoute] Guid idNaoConformidade, [FromQuery] bool isFullValidation)
    {
        var canGenerate = await _ordemRetrabalhoService.CanGenerate(idNaoConformidade, null, isFullValidation);
        return Ok(canGenerate);
    }
    

    private async Task<OrdemRetrabalhoNaoConformidadeOutput> VerificarPodeEstornar(Guid idNaoConformidade)
    {
        var canEstornarOrdemRetrabalho = await _ordemRetrabalhoService.CanEstornar(idNaoConformidade);

        var output = new OrdemRetrabalhoNaoConformidadeOutput();
        if (canEstornarOrdemRetrabalho == EstornarOrdemRetrabalhoValidationResult.OdfRetrabalhoNaoEncontrada)
        {
            output = new OrdemRetrabalhoNaoConformidadeOutput
            {
                Success = false,
                Message = "Odf de retrabalho não encontrada"
            };
        }

        if (canEstornarOrdemRetrabalho == EstornarOrdemRetrabalhoValidationResult.OdfRetrabalhoJaApontada)
        {
            output = new OrdemRetrabalhoNaoConformidadeOutput
            {
                Success = false,
                Message = "Odf de retrabalho já iniciou seu processo produtivo"
            };
        }

        if (canEstornarOrdemRetrabalho == EstornarOrdemRetrabalhoValidationResult.RncComOrigemInspecaoSaida)
        {
            output = new OrdemRetrabalhoNaoConformidadeOutput
            {
                Success = false,
                Message = "Não é possível estornar RNC com origem de inspeção de saída"
            };
        }

        if (canEstornarOrdemRetrabalho == EstornarOrdemRetrabalhoValidationResult.RncFechada)
        {
            output = new OrdemRetrabalhoNaoConformidadeOutput
            {
                Success = false,
                Message = "Não é possível estornar odf retrabalho quando RNC já concluído"
            };
        }

        return output;
    }

    private async Task<OrdemRetrabalhoNaoConformidadeOutput> VerificarPodeGerar(AgregacaoNaoConformidade agregacao, OrdemRetrabalhoInput input)
    {
        var canGenerateOrdemRetrabalho = await _gerarOrdemRetrabalhoValidatorService
            .ValidateStatusRnc()
            .ValidateOperacaoEngenhariaFinal()
            .ValidateOperacaoEngenhariaDuplicada()
            .ValidateOdf()
            .ValidateLote(input)
            .ValidateQuantidade(input)
            .ValidateAsync(agregacao);

        switch (canGenerateOrdemRetrabalho)
        {
            case GerarOrdemRetrabalhoValidationResult.OperacaoFinalNaoEncontrada:
                return new OrdemRetrabalhoNaoConformidadeOutput
                {
                    Message = "Não foi encontrado serviço com operação engenharia \"999\"",
                    Success = false
                };
            case GerarOrdemRetrabalhoValidationResult.OperacaoEngenhariaDuplicada:
                return new OrdemRetrabalhoNaoConformidadeOutput
                {
                    Message = "Existem mais de um serviço com a mesma operação engenharia",
                    Success = false
                };
            case GerarOrdemRetrabalhoValidationResult.OdfRetrabalhoJaGerada:
                return new OrdemRetrabalhoNaoConformidadeOutput
                {
                    Message = "Já existe Odf de retrabalho para este relatório de não conformidade",
                    Success = false
                };
            case GerarOrdemRetrabalhoValidationResult.LoteObrigatorio:
                return new OrdemRetrabalhoNaoConformidadeOutput
                {
                    Message = "Lote é obrigatório",
                    Success = false
                };
            case GerarOrdemRetrabalhoValidationResult.OdfObrigatorio:
                return new OrdemRetrabalhoNaoConformidadeOutput
                {
                    Message = "Odf é obrigatório",
                    Success = false
                };
            case GerarOrdemRetrabalhoValidationResult.QuantidadeInvalida:
                return new OrdemRetrabalhoNaoConformidadeOutput
                {
                    Message = "A quantidade informada ultrapassa o valor em estoque",
                    Success = false
                };
            case GerarOrdemRetrabalhoValidationResult.OdfNaoApontada:
                return new OrdemRetrabalhoNaoConformidadeOutput
                {
                    Message = "A Odf informada, ainda não foi apontada",
                    Success = false
                };
            case GerarOrdemRetrabalhoValidationResult.OdfNaoFinalizada:
                return new OrdemRetrabalhoNaoConformidadeOutput
                {
                    Message = "Odf não encontra-se finalizada, para realizar um retrabalho deve ser gerada uma operação de retrabalho",
                    Success = false
                };
            case GerarOrdemRetrabalhoValidationResult.RncFechada:
                return new OrdemRetrabalhoNaoConformidadeOutput
                {
                    Message = "Não é possível gerar odf retrabalho quando RNC já concluído",
                    Success = false
                };
            default:
                return new OrdemRetrabalhoNaoConformidadeOutput
                {
                    Success = true
                };
        }
    }
}