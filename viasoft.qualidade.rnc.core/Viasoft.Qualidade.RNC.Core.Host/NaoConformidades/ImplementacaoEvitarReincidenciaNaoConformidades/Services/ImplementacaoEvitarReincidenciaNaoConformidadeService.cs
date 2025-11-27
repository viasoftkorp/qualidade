using System;
using System.Linq;
using System.Linq.Dynamic.Core;
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
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Commands;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Services;

public class ImplementacaoEvitarReincidenciaNaoConformidadeService : IImplementacaoEvitarReincidenciaNaoConformidadeService, ITransientDependency
{
    private readonly IRepository<ImplementacaoEvitarReincidenciaNaoConformidade> _repository;
    private readonly ICurrentCompany _currentCompany;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceBus _serviceBus;

    public ImplementacaoEvitarReincidenciaNaoConformidadeService(IRepository<ImplementacaoEvitarReincidenciaNaoConformidade> repository,
        ICurrentCompany currentCompany, ICurrentTenant currentTenant, ICurrentEnvironment currentEnvironment, INaoConformidadeRepository naoConformidadeRepository, 
        IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork, IServiceBus serviceBus)
    {
        _repository = repository;
        _currentCompany = currentCompany;
        _currentTenant = currentTenant;
        _currentEnvironment = currentEnvironment;
        _naoConformidadeRepository = naoConformidadeRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
        _serviceBus = serviceBus;
    }
    public async Task Insert(Guid idNaoConformidade, ImplementacaoEvitarReincidenciaNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var inserirCommand = new InserirImplementacaoEvitarReincidenciaNaoConformidadeCommand(input);
        inserirCommand.ImplementacaoEvitarReincidenciaNaoConformidade.CompanyId = _currentCompany.Id;

        naoConformidade.Process(inserirCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

    public async Task Update(Guid idNaoConformidade, ImplementacaoEvitarReincidenciaNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var atualizarCommand = new AlterarImplementacaoEvitarReincidenciaNaoConformidadeCommand(input);
        atualizarCommand.ImplementacaoEvitarReincidenciaNaoConformidade.CompanyId = _currentCompany.Id;

        naoConformidade.Process(atualizarCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

    public async Task Remove(Guid idNaoConformidade, Guid idImplementacaoEvitarReincidencia)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var removerItemCommand = new RemoverImplementacaoEvitarReincidenciaNaoConformidadeCommand(idImplementacaoEvitarReincidencia);
        naoConformidade.Process(removerItemCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

    public async Task<ImplementacaoEvitarReincidenciaNaoConformidadeOutput> GetById(Guid id)
    {
        var entity = await _repository
            .Where(e => e.CompanyId == _currentCompany.Id)
            .Where(entity => entity.Id.Equals(id))
            .Select(entity => new ImplementacaoEvitarReincidenciaNaoConformidadeOutput(entity))
            .FirstOrDefaultAsync();
        return entity;
    }
}