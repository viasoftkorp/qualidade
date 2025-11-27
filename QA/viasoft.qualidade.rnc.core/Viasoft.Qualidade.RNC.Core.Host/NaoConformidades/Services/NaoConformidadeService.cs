using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.Identity.Abstractions;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.Utils.Consts;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;

public class NaoConformidadeService : INaoConformidadeService, ITransientDependency
{
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceBus _serviceBus;
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<NaoConformidade> _naoConformidades;
    private readonly INaoConformidadeValidationService _naoConformidadeValidationService;
    private readonly ICurrentCompany _currentCompany;

    public NaoConformidadeService(INaoConformidadeRepository naoConformidadeRepository,
        IDateTimeProvider dateTimeProvider, ICurrentTenant currentTenant, ICurrentEnvironment currentEnvironment,
        IUnitOfWork unitOfWork, IServiceBus serviceBus, ICurrentUser currentUser,
        IRepository<NaoConformidade> naoConformidades, INaoConformidadeValidationService naoConformidadeValidationService,
        ICurrentCompany currentCompany)
    {
        _naoConformidadeRepository = naoConformidadeRepository;
        _dateTimeProvider = dateTimeProvider;
        _currentTenant = currentTenant;
        _currentEnvironment = currentEnvironment;
        _unitOfWork = unitOfWork;
        _serviceBus = serviceBus;
        _currentUser = currentUser;
        _naoConformidades = naoConformidades;
        _naoConformidadeValidationService = naoConformidadeValidationService;
        _currentCompany = currentCompany;
    }

    public async Task<NaoConformidadeOutput> Get(Guid id)
    {
        var entity = await _naoConformidades.FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null)
        {
            return null;
        }

        var output = new NaoConformidadeOutput(entity);
        return output;
    }

    public async Task<NaoConformidadeValidationResult> Create(NaoConformidadeInput input)
    {
        var naoConformidadeValidationResult = await ValidarNaoConformidade(input);

        if (naoConformidadeValidationResult != NaoConformidadeValidationResult.Ok)
        {
            return naoConformidadeValidationResult;
        }
        var naoConformidade = new AgregacaoNaoConformidade();
        input.CompanyId = _currentCompany.Id;

        var inserirCommand = new InserirNaoConformidadeCommand(input);

        try
        {
            using (_unitOfWork.Begin())
            {
                naoConformidade.Process(inserirCommand, _dateTimeProvider, _currentTenant.Id,
                    _currentEnvironment.Id.Value, _currentUser.Id);

                await naoConformidade.SalvarNaoConformidade(_naoConformidadeRepository);
                foreach (var domainEvent in naoConformidade.DomainEvents)
                {
                    await _serviceBus.Publish(domainEvent);
                }

                if (input.Incompleta)
                {
                    var command = new VerificarNaoConformidadeIncompletaCommand
                    {
                        IdNaoConformidade = input.Id
                    };
                    await _serviceBus.DeferLocal(new TimeSpan(24,0,0), command);
                }

                await _unitOfWork.CompleteAsync();
            }
        }
        catch (Exception e)
        {
            var postgresException = e.InnerException as PostgresException;
            if (postgresException?.SqlState == PostgresExceptionConsts.ChaveUnicaViolada)
            {
                await RetrySalvarNaoConformidade(naoConformidade, 1);
            }
            else
            {
                throw;
            }
        }


        return NaoConformidadeValidationResult.Ok;
    }

    public async Task<NaoConformidadeValidationResult> Update(Guid id, NaoConformidadeInput input)
    {
        var naoConformidadeValidationResult = await ValidarNaoConformidade(input);

        if (naoConformidadeValidationResult != NaoConformidadeValidationResult.Ok)
        {
            return naoConformidadeValidationResult;
        }

        var agregacaoNaoConformidade = await _naoConformidadeRepository.Get(id);

        input.CompanyId = _currentCompany.Id;
        var alterarCommand = new AlterarNaoConformidadeCommand(input);

        agregacaoNaoConformidade.Process(alterarCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        try
        {
            await agregacaoNaoConformidade.CommitUpdate(agregacaoNaoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
        }
        catch (Exception e)
        {
            var postgresException = e.InnerException as PostgresException;
            if (postgresException?.SqlState == PostgresExceptionConsts.ChaveUnicaViolada)
            {
                await RetryAtualizarNaoConformidade(agregacaoNaoConformidade, 1);
            }
            else
            {
                throw;
            }
        }
        return NaoConformidadeValidationResult.Ok;
    }

    public async Task Delete(Guid id)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(id);
        var removerCommand = new RemoverNaoConformidadeCommand(id);
        naoConformidade.Process(removerCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        using (_unitOfWork.Begin())
        {
            await naoConformidade.DeletarNaoConformidade(_naoConformidadeRepository);
            await _unitOfWork.CompleteAsync();
        }
    }

    private async Task<NaoConformidadeValidationResult> ValidarNaoConformidade(NaoConformidadeInput input)
    {
        var resultadoValidacaoStatus = _naoConformidadeValidationService.ValidarChangeStatus(input);

        if (resultadoValidacaoStatus != NaoConformidadeValidationResult.Ok)
        {
            return resultadoValidacaoStatus;
        }

        var clienteValidacao = _naoConformidadeValidationService.ValidarCampoCliente(input);
        if (clienteValidacao != NaoConformidadeValidationResult.Ok)
        {
            return clienteValidacao;
        };
        var fornecedorValidacao = _naoConformidadeValidationService.ValidarCampoFornecedor(input);
        if (fornecedorValidacao != NaoConformidadeValidationResult.Ok)
        {
            return fornecedorValidacao;
        };
        var odfValidacao = await _naoConformidadeValidationService.ValidarCampoOdf(input);
        if (odfValidacao != NaoConformidadeValidationResult.Ok)
        {
            return odfValidacao;
        };
        var notaFiscalValidacao = _naoConformidadeValidationService.ValidarCampoNotaFiscal(input);
        if (notaFiscalValidacao != NaoConformidadeValidationResult.Ok)
        {
            return notaFiscalValidacao;
        };
        var produtoValidacao = await _naoConformidadeValidationService.ValidarCampoProduto(input);
        if (produtoValidacao != NaoConformidadeValidationResult.Ok)
        {
            return produtoValidacao;
        };
        var loteValidacao = await _naoConformidadeValidationService.ValidarCampoLote(input);
        if (loteValidacao != NaoConformidadeValidationResult.Ok)
        {
            return loteValidacao;
        };
        return NaoConformidadeValidationResult.Ok;
    }

    public async Task RetrySalvarNaoConformidade(AgregacaoNaoConformidade agregacao, int tentativaAtual)
    {
        if (tentativaAtual > 5)
        {
            return;
        }
        agregacao.NaoConformidadeInserir.Codigo++;

        using (_unitOfWork.Begin())
        {
            await agregacao.SalvarNaoConformidade(_naoConformidadeRepository);
            foreach (var domainEvent in agregacao.DomainEvents)
            {
                await _serviceBus.Publish(domainEvent);
            }

            if (agregacao.NaoConformidadeInserir.Incompleta)
            {
                var command = new VerificarNaoConformidadeIncompletaCommand
                {
                    IdNaoConformidade = agregacao.NaoConformidadeInserir.Id
                };
                await _serviceBus.DeferLocal(new TimeSpan(24,0,0), command);
            }
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception e)
            {
                var postgresException = e.InnerException as PostgresException;

                if ( postgresException.SqlState == PostgresExceptionConsts.ChaveUnicaViolada)
                {
                    await RetrySalvarNaoConformidade(agregacao, tentativaAtual + 1);
                }
            }
        }
    }
    public async Task RetryAtualizarNaoConformidade(AgregacaoNaoConformidade agregacao, int tentativaAtual)
    {
        if (tentativaAtual > 5)
        {
            return;
        }
        agregacao.NaoConformidadeAlterar.Codigo++;

        using (_unitOfWork.Begin())
        {
            try
            {
                await agregacao.CommitUpdate(agregacao, _unitOfWork, _serviceBus, _naoConformidadeRepository);
            }
            catch (Exception e)
            {
                var postgresException = e.InnerException as PostgresException;

                if ( postgresException.SqlState == PostgresExceptionConsts.ChaveUnicaViolada)
                {
                    await RetryAtualizarNaoConformidade(agregacao, tentativaAtual + 1);
                }
            }
        }
    }
}
