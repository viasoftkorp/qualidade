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
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.CausasNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Services;

public class CausaNaoConformidadeService : ICausaNaoConformidadeService,ITransientDependency
{
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceBus _serviceBus;
    private readonly IRepository<CausaNaoConformidade> _causaNaoConformidades;
    private readonly ICurrentCompany _currentCompany;

    public CausaNaoConformidadeService(INaoConformidadeRepository naoConformidadeRepository, IDateTimeProvider dateTimeProvider,
        ICurrentTenant currentTenant, ICurrentEnvironment currentEnvironment, IUnitOfWork unitOfWork,
        IServiceBus serviceBus, IRepository<CausaNaoConformidade> causaNaoConformidades,
        ICurrentCompany currentCompany)
    {
        _naoConformidadeRepository = naoConformidadeRepository;
        _dateTimeProvider = dateTimeProvider;
        _currentTenant = currentTenant;
        _currentEnvironment = currentEnvironment;
        _unitOfWork = unitOfWork;
        _serviceBus = serviceBus;
        _causaNaoConformidades = causaNaoConformidades;
        _currentCompany = currentCompany;
    }

    public async Task<CausaNaoConformidadeOutput> Get (Guid idNaoConformidade, Guid id)
    {
        var entity = await _causaNaoConformidades.Where(entity => entity.IdNaoConformidade.Equals(idNaoConformidade))
            .Where(entity => entity.Id.Equals(id))
            .Select(entity => new CausaNaoConformidadeOutput(entity))
            .FirstOrDefaultAsync();
        return entity;
    }
    public async Task Update(Guid idNaoConformidade, Guid id, CausaNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var atualizarCommand = new AlterarCausaCommand(input);
        atualizarCommand.CausaNaoConformidade.CompanyId = _currentCompany.Id;

        atualizarCommand.CausaNaoConformidade.Id = id;
        naoConformidade.Process(atualizarCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value, _currentCompany.Id);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

    public async Task Insert(Guid idNaoConformidade, CausaNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var inserirCommand = new InserirCausaCommand(input);
        inserirCommand.CausaNaoConformidade.CompanyId = _currentCompany.Id;
        naoConformidade.Process(inserirCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value, _currentCompany.Id);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

    public async Task Remove(Guid idNaoConformidade, Guid id)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var removerCommand = new RemoverCausaCommand(id);
        naoConformidade.Process(removerCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

}
