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
using Viasoft.Core.MultiTenancy.Abstractions.Extensions;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.SolucoesNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Services;

public class SolucaoNaoConformidadeService : ISolucaoNaoConformidadeService, ITransientDependency
{
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceBus _serviceBus;
    private readonly IRepository<SolucaoNaoConformidade> _solucaoNaoConformidades;
    private readonly ICurrentCompany _currentCompany;

    public SolucaoNaoConformidadeService(INaoConformidadeRepository naoConformidadeRepository, IDateTimeProvider dateTimeProvider,
        ICurrentTenant currentTenant, ICurrentEnvironment currentEnvironment, IUnitOfWork unitOfWork,
        IServiceBus serviceBus, IRepository<SolucaoNaoConformidade> solucaoNaoConformidades,
        ICurrentCompany currentCompany)
    {
        _naoConformidadeRepository = naoConformidadeRepository;
        _dateTimeProvider = dateTimeProvider;
        _currentTenant = currentTenant;
        _currentEnvironment = currentEnvironment;
        _unitOfWork = unitOfWork;
        _serviceBus = serviceBus;
        _solucaoNaoConformidades = solucaoNaoConformidades;
        _currentCompany = currentCompany;
    }
    
    public async Task<SolucaoNaoConformidadeOutput> Get (Guid idNaoConformidade, Guid id)
    {
        var entity = await _solucaoNaoConformidades.Where(entity => entity.IdNaoConformidade.Equals(idNaoConformidade))
            .Where(entity => entity.Id.Equals(id))
            .Select(entity => new SolucaoNaoConformidadeOutput(entity))
            .FirstOrDefaultAsync();
        return entity;
    }
    public async Task Update(Guid idNaoConformidade, Guid idSolucaoNaoConformidade, SolucaoNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var atualizarCommand = new AlterarSolucaoCommand(input);
        atualizarCommand.SolucaoNaoConformidade.CompanyId = _currentCompany.Id;

        atualizarCommand.SolucaoNaoConformidade.Id = idSolucaoNaoConformidade;
        naoConformidade.Process(atualizarCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

    public async Task Insert(Guid idNaoConformidade, SolucaoNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var inserirCommand = new InserirSolucaoCommand(input);
        inserirCommand.SolucaoNaoConformidade.CompanyId = _currentCompany.Id;

        naoConformidade.Process(inserirCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

    public async Task Remove(Guid idNaoConformidade, Guid idSolucaoNaoConformidade)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var solucaoNaoConformidade = await _solucaoNaoConformidades.FindAsync(idSolucaoNaoConformidade);
        
        var removerCommand = new RemoverSolucaoCommand(solucaoNaoConformidade.Id);
        
        naoConformidade.Process(removerCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }
    
}