using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Models;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos.Acls;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Services;

public class OperacaoRetrabalhoNaoConformidadeService : IOperacaoRetrabalhoNaoConformidadeService, ITransientDependency
{
    private readonly IOperacaoRetrabalhoAclService _operacaoRetrabalhoAclService;
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IRepository<OperacaoRetrabalhoNaoConformidade> _operacaoRetrabalhoNaoConformidades;
    private readonly IOperacaoRetrabalhoNaoConformidadeValidatorService _operacaoRetrabalhoNaoConformidadeValidatorService;
    private readonly IRepository<Operacao> _operacoes;
    private readonly IRepository<Recurso> _recursos;
    private readonly IOperacaoRetrabalhoProxyService _operacaoRetrabalhoProxyService;

    public OperacaoRetrabalhoNaoConformidadeService(IOperacaoRetrabalhoAclService operacaoRetrabalhoAclService,
        INaoConformidadeRepository naoConformidadeRepository, IRepository<OperacaoRetrabalhoNaoConformidade> operacaoRetrabalhoNaoConformidades,
        IOperacaoRetrabalhoNaoConformidadeValidatorService operacaoRetrabalhoNaoConformidadeValidatorService,
        IRepository<Operacao> operacoes, IRepository<Recurso> recursos, IOperacaoRetrabalhoProxyService operacaoRetrabalhoProxyService)
    {
        _operacaoRetrabalhoAclService = operacaoRetrabalhoAclService;
        _naoConformidadeRepository = naoConformidadeRepository;
        _operacaoRetrabalhoNaoConformidades = operacaoRetrabalhoNaoConformidades;
        _operacaoRetrabalhoNaoConformidadeValidatorService = operacaoRetrabalhoNaoConformidadeValidatorService;
        _operacoes = operacoes;
        _recursos = recursos;
        _operacaoRetrabalhoProxyService = operacaoRetrabalhoProxyService;
    }
    public async Task<OperacaoRetrabalhoNaoConformidadeOutput> Create(Guid idNaoConformidade, OperacaoRetrabalhoNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository
            .Operacoes()
            .Get(idNaoConformidade);

        var operacaoRetrabalhoValidationResult = await Validate(naoConformidade, input);

        if (!operacaoRetrabalhoValidationResult.Success)
        {
            return operacaoRetrabalhoValidationResult;
        }
        
        var idOperacaoRetrabalhoNaoConformidade = Guid.NewGuid();

        var createOperacaoRetrabalhoInput = new GerarOperacaoRetrabalhoAclInput
        {
            OperacaoRetrabalhoNaoConformidade = new OperacaoRetrabalhoNaoConformidadeModel
            {
                Quantidade = input.Quantidade,
                NumeroOperacaoARetrabalhar = input.NumeroOperacaoARetrabalhar,
                Id = idOperacaoRetrabalhoNaoConformidade,
            },
            NumeroOdf = naoConformidade.NaoConformidade.NumeroOdf
        };
        
        var gerarOperacaoRetrabalhoExternalInput = await _operacaoRetrabalhoAclService
            .GetGerarOperacaoRetrabalhoExternalInput(createOperacaoRetrabalhoInput, input.Maquinas);
        
        var result = await _operacaoRetrabalhoProxyService.Create(gerarOperacaoRetrabalhoExternalInput);
        
        var operacoesAclOutput = await _operacaoRetrabalhoAclService.GetGerarOperacaoRetrabalhoAclOutput(result);

        if (operacoesAclOutput.Success)
        {
            var operacoesGeradas = operacoesAclOutput.OperacoesAdicionadas.Select(e => new Operacao
            {
                 IdRecurso = e.IdMaquina,
                 NumeroOperacao = e.NumeroOperacao,
                 IdOperacaoRetrabalhoNaoConformdiade = idOperacaoRetrabalhoNaoConformidade,
            }).ToList();
            
            var operacaoRetrabalhoNaoConformidade = new OperacaoRetrabalhoNaoConformidade
            {                
                Id = idOperacaoRetrabalhoNaoConformidade,
                Quantidade = input.Quantidade,
                NumeroOperacaoARetrabalhar = input.NumeroOperacaoARetrabalhar,
                IdNaoConformidade = idNaoConformidade,
                Operacoes = operacoesGeradas
            };
            await _operacaoRetrabalhoNaoConformidades.InsertAsync(operacaoRetrabalhoNaoConformidade, true);
            
            var output = new OperacaoRetrabalhoNaoConformidadeOutput
            {
                IdNaoConformidade = idNaoConformidade,
                NumeroOperacaoARetrabalhar = input.NumeroOperacaoARetrabalhar,
                Quantidade = input.Quantidade,
                Id = idOperacaoRetrabalhoNaoConformidade,
                Message = result.Message,
                Success = result.Success,
                Operacoes = operacoesGeradas.Select(e => new OperacaoOutput(e)).ToList(),
                ValidationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.Ok
            };
            return output;
        }
        else
        {
            var output = new OperacaoRetrabalhoNaoConformidadeOutput
            {
                Message = result.Message,
                Success = result.Success,
                ValidationResult = operacoesAclOutput.ValidationResult
            };
            return output;
        }
    }

    public async Task<OperacaoRetrabalhoNaoConformidadeOutput> Get(Guid idNaoConformidade)
    {
        var naoConformidade = await _naoConformidadeRepository
            .Operacoes()
            .Get(idNaoConformidade);

        if (naoConformidade.NaoConformidade.OperacaoRetrabalho != null)
        {
            var output = new OperacaoRetrabalhoNaoConformidadeOutput(naoConformidade.NaoConformidade.OperacaoRetrabalho)
            {
                Success = true
            };
            return output; 
        }

        return null;
    }

    public async Task<PagedResultDto<OperacaoViewOutput>> GetOperacoesView(Guid idOperacaoRetrabalho,
        PagedFilteredAndSortedRequestInput input)
    {
        var query = (from operacao in _operacoes.AsNoTracking()
            where operacao.IdOperacaoRetrabalhoNaoConformdiade == idOperacaoRetrabalho
            join recurso in _recursos.AsNoTracking()
                on operacao.IdRecurso equals recurso.Id
            select new OperacaoViewOutput
            {
                Id = operacao.Id,
                IdRecurso = operacao.IdRecurso,
                NumeroOperacao = operacao.NumeroOperacao,
                IdOperacaoRetrabalhoNaoConformdiade = operacao.IdOperacaoRetrabalhoNaoConformdiade,
                DescricaoRecurso = recurso.Descricao,
                CodigoRecurso = recurso.Codigo,
                Status = operacao.Status
            })
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting)
            .OrderBy(e => e.NumeroOperacao);

        var totalCount = await query.CountAsync();
        var itens = await query.ToListAsync();

        var output = new PagedResultDto<OperacaoViewOutput>
        {
            TotalCount = totalCount,
            Items = itens
        };
        return output;
    }

    private async Task<OperacaoRetrabalhoNaoConformidadeOutput> Validate(AgregacaoNaoConformidade agregacaoNaoConformidade, 
        OperacaoRetrabalhoNaoConformidadeInput operacaoRetrabalhoNaoConformidadeInput)
    {
        var statusRncValidationResult = _operacaoRetrabalhoNaoConformidadeValidatorService.ValidateStatusRnc(agregacaoNaoConformidade.NaoConformidade);

        if (statusRncValidationResult != OperacaoRetrabalhoNaoConformidadeValidationResult.Ok)
        {
            return new OperacaoRetrabalhoNaoConformidadeOutput
            {
                Message = "Não é possível gerar operação retrabalho quando RNC já concluído",
                ValidationResult = statusRncValidationResult
            };        
        }
        
        var operacaoRetrabalhoAlreadyExistResult =
            _operacaoRetrabalhoNaoConformidadeValidatorService.ValidateOperacaoRetrabalhoJaExistente(agregacaoNaoConformidade);
        if( operacaoRetrabalhoAlreadyExistResult != OperacaoRetrabalhoNaoConformidadeValidationResult.Ok)
        {
            return new OperacaoRetrabalhoNaoConformidadeOutput
            {
                Message = "Operação de retrabalho já criada",
                ValidationResult = operacaoRetrabalhoAlreadyExistResult
            };
        };
        
        var nenhumaMaquinaCadastradaValidationResult =
            _operacaoRetrabalhoNaoConformidadeValidatorService.ValidateMaquina(operacaoRetrabalhoNaoConformidadeInput);

        if (nenhumaMaquinaCadastradaValidationResult != OperacaoRetrabalhoNaoConformidadeValidationResult.Ok)
        {
            return new OperacaoRetrabalhoNaoConformidadeOutput
            {
                Message = "É necessário ao menos uma máquina para gerar operação de retrabalho",
                ValidationResult = nenhumaMaquinaCadastradaValidationResult
            };
        }

        var validateOdfApontadaValidationResult = await _operacaoRetrabalhoNaoConformidadeValidatorService.ValidateOdfApontada(agregacaoNaoConformidade);
        
        if (validateOdfApontadaValidationResult != OperacaoRetrabalhoNaoConformidadeValidationResult.Ok)
        {
            return new OperacaoRetrabalhoNaoConformidadeOutput
            {
                Message = "A Odf informada, ainda não foi apontada",
                ValidationResult = validateOdfApontadaValidationResult
            };
        }

        var odfAbertaValidationResult = await _operacaoRetrabalhoNaoConformidadeValidatorService.ValidateOdfAberta(agregacaoNaoConformidade);

        if (odfAbertaValidationResult != OperacaoRetrabalhoNaoConformidadeValidationResult.Ok)
        {
            return new OperacaoRetrabalhoNaoConformidadeOutput
            {
                Message = "A Odf informada foi encerrada",
                ValidationResult = odfAbertaValidationResult
            };        
        }
        
        return new OperacaoRetrabalhoNaoConformidadeOutput
        {
            Success = true,
            ValidationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.Ok
        };
    }
}