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
using Viasoft.Qualidade.RNC.Core.Domain.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Services;

public class ConclusaoNaoConformidadeService : IConclusaoNaoConformidadeService, ITransientDependency
{
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceBus _serviceBus;
    private readonly IRepository<ConclusaoNaoConformidade> _conclusaoNaoConformidades;
    private readonly ICurrentCompany _currentCompany;

    public ConclusaoNaoConformidadeService(INaoConformidadeRepository naoConformidadeRepository,
        IDateTimeProvider dateTimeProvider,
        ICurrentTenant currentTenant, ICurrentEnvironment currentEnvironment, IUnitOfWork unitOfWork,
        IServiceBus serviceBus, IRepository<ConclusaoNaoConformidade> conclusaoNaoConformidades,
        ICurrentCompany currentCompany)
    {
        _naoConformidadeRepository = naoConformidadeRepository;
        _dateTimeProvider = dateTimeProvider;
        _currentTenant = currentTenant;
        _currentEnvironment = currentEnvironment;
        _unitOfWork = unitOfWork;
        _serviceBus = serviceBus;
        _conclusaoNaoConformidades = conclusaoNaoConformidades;
        _currentCompany = currentCompany;
    }

    public async Task ConcluirNaoConformidade(Guid idNaoConformidade, ConclusaoNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        if (naoConformidade.NaoConformidade.Status != StatusNaoConformidade.Fechado)
        {
            var concluirCommand = new ConcluirNaoConformidadeCommand(input);
            concluirCommand.ConclusaoNaoConformidade.CompanyId = _currentCompany.Id;

            naoConformidade.Process(concluirCommand, _dateTimeProvider, _currentTenant.Id,
                _currentEnvironment.Id.Value);
            await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
        }
    }

    public async Task<int> CalcularCicloTempo(Guid idNaoConformidade)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        return DateTime.UtcNow.Day - naoConformidade.NaoConformidade.CreationTime.Day;
    }

    public async Task<ConclusaoNaoConformidadeOutput> Get (Guid idNaoConformidade)
    {
        var entity = await _conclusaoNaoConformidades
            .Where(e => e.CompanyId == _currentCompany.Id)
            .Where(entity => entity.IdNaoConformidade.Equals(idNaoConformidade))
            .Select(entity => new ConclusaoNaoConformidadeOutput(entity))
            .FirstOrDefaultAsync();
        return entity;
    }

    public async Task Estornar(Guid idNaoConformidade)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        if (naoConformidade.NaoConformidade.Status == StatusNaoConformidade.Fechado)
        {
            var estornarCommand = new EstornarConclusaoCommand();

            naoConformidade.Process(estornarCommand, _dateTimeProvider, _currentTenant.Id,
                _currentEnvironment.Id.Value);
            await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
        }
    }
}