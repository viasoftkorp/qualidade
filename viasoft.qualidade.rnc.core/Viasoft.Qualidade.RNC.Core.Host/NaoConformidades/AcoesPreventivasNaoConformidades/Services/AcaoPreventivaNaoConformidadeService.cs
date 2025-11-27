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
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.AcoesPreventivasNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;

public class AcaoPreventivaNaoConformidadeService : IAcaoPreventivaNaoConformidadeService, ITransientDependency
{
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceBus _serviceBus;
    private readonly IRepository<AcaoPreventivaNaoConformidade> _acaoPreventivaNaoConformidades;
    private readonly ICurrentCompany _currentCompany;

    public AcaoPreventivaNaoConformidadeService(INaoConformidadeRepository naoConformidadeRepository,
        IDateTimeProvider dateTimeProvider,
        ICurrentTenant currentTenant, ICurrentEnvironment currentEnvironment, IUnitOfWork unitOfWork,
        IServiceBus serviceBus, IRepository<AcaoPreventivaNaoConformidade> acaoPreventivaNaoConformidades,
        ICurrentCompany currentCompany)
    {
        _naoConformidadeRepository = naoConformidadeRepository;
        _dateTimeProvider = dateTimeProvider;
        _currentTenant = currentTenant;
        _currentEnvironment = currentEnvironment;
        _unitOfWork = unitOfWork;
        _serviceBus = serviceBus;
        _acaoPreventivaNaoConformidades = acaoPreventivaNaoConformidades;
        _currentCompany = currentCompany;
    }
    
    public async Task<AcaoPreventivaNaoConformidadeOutput> Get (Guid idNaoConformidade, Guid id)
    {
        var entity = await _acaoPreventivaNaoConformidades.Where(entity => entity.IdNaoConformidade.Equals(idNaoConformidade))
            .Where(entity => entity.Id.Equals(id))
            .Select(entity => new AcaoPreventivaNaoConformidadeOutput(entity))
            .FirstOrDefaultAsync();
        return entity;
    }

    public async Task Update(Guid idNaoConformidade, Guid idAcaoPreventivaNaoConformidade,
        AcaoPreventivaNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var atualizarCommand = new AlterarAcaoPreventivaCommand(input);
        atualizarCommand.AcaoPreventivaNaoConformidade.CompanyId = _currentCompany.Id;

        atualizarCommand.AcaoPreventivaNaoConformidade.Id = idAcaoPreventivaNaoConformidade;
        naoConformidade.Process(atualizarCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

    public async Task Insert(Guid idNaoConformidade, AcaoPreventivaNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var inserirCommand = new InserirAcaoPreventivaCommand(input);
        inserirCommand.AcaoPreventivaNaoConformidade.CompanyId = _currentCompany.Id;
        naoConformidade.Process(inserirCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

    public async Task Remove(Guid idNaoConformidade, Guid idAcaoPreventivaNaoConformidade)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var removerItemCommand = new RemoverAcaoPreventivaCommand(idAcaoPreventivaNaoConformidade);
        naoConformidade.Process(removerItemCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }
}