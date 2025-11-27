using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ServicosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Services;

public class ServicoNaoConformidadeService : IServicoNaoConformidadeservice, ITransientDependency
{
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceBus _serviceBus;
    private readonly IRepository<ServicoNaoConformidade> _servicoNaoConformidades;
    private readonly ICurrentCompany _currentCompany;
    private readonly IServicoValidatorService _servicoValidatorService;

    public ServicoNaoConformidadeService(INaoConformidadeRepository naoConformidadeRepository, IDateTimeProvider dateTimeProvider,
        ICurrentTenant currentTenant, ICurrentEnvironment currentEnvironment, IUnitOfWork unitOfWork,
        IServiceBus serviceBus, IRepository<ServicoNaoConformidade> servicoNaoConformidades,
        ICurrentCompany currentCompany, IServicoValidatorService servicoValidatorService)
    {
        _naoConformidadeRepository = naoConformidadeRepository;
        _dateTimeProvider = dateTimeProvider;
        _currentTenant = currentTenant;
        _currentEnvironment = currentEnvironment;
        _unitOfWork = unitOfWork;
        _serviceBus = serviceBus;
        _servicoNaoConformidades = servicoNaoConformidades;
        _currentCompany = currentCompany;
        _servicoValidatorService = servicoValidatorService;
    }

    public async Task<ServicoNaoConformidadeOutput> Get (Guid idNaoConformidade, Guid id)
    {
        var entity = await _servicoNaoConformidades.Where(entity => entity.IdNaoConformidade.Equals(idNaoConformidade))
            .Where(entity => entity.Id.Equals(id))
            .Select(entity => new ServicoNaoConformidadeOutput(entity))
            .FirstOrDefaultAsync();
        return entity;
    }
    public async Task<ServicoValidationResult> Update(Guid idNaoConformidade, Guid idServicoSolucaoNaoConformidade, ServicoNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        if (OperacaoEngenhariaJaUtilizada(naoConformidade, input))
        {
            return ServicoValidationResult.OperacaoEngenhariaJaUtilizada;
        }

        var tempoValido = _servicoValidatorService.ValidarTempo(input.Horas, input.Minutos);
        if (!tempoValido)
        {
            return ServicoValidationResult.TempoInvalido;
        }

        var atualizarCommand = new AlterarServicosNaoConformidadeCommand(input);
        atualizarCommand.ServicoNaoConformidade.Id = idServicoSolucaoNaoConformidade;
        atualizarCommand.ServicoNaoConformidade.CompanyId = _currentCompany.Id;

        naoConformidade.Process(atualizarCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
        return ServicoValidationResult.Ok;
    }

    public async Task<ServicoValidationResult> Insert(Guid idNaoConformidade, ServicoNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        if (OperacaoEngenhariaJaUtilizada(naoConformidade, input))
        {
            return ServicoValidationResult.OperacaoEngenhariaJaUtilizada;
        }
        
        var tempoValido = _servicoValidatorService.ValidarTempo(input.Horas, input.Minutos);
        if (!tempoValido)
        {
            return ServicoValidationResult.TempoInvalido;
        }
        
        var inserirCommand = new InserirServicoNaoConformidadeCommand(input);
        inserirCommand.ServicoNaoConformidade.CompanyId = _currentCompany.Id;

        naoConformidade.Process(inserirCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        inserirCommand.ServicoNaoConformidade.CompanyId = _currentCompany.Id;

        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
        return ServicoValidationResult.Ok;
    }

    public async Task Remove(Guid idNaoConformidade, Guid idServicoSolucaoNaoConformidade)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var removerCommand = new RemoverServicoNaoConformidadeCommand(idServicoSolucaoNaoConformidade);
        naoConformidade.Process(removerCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

    private bool OperacaoEngenhariaJaUtilizada(AgregacaoNaoConformidade agregacaoNaoConformidade, ServicoNaoConformidadeInput input)
    {
        var servicos = agregacaoNaoConformidade.ServicoNaoConformidades;
        var operacaoEngenhariaJaUtilizada = servicos.Any(e => e.Id != input.Id 
                                                              && e.OperacaoEngenharia == input.OperacaoEngenharia);
        return operacaoEngenhariaJaUtilizada;
    }
}