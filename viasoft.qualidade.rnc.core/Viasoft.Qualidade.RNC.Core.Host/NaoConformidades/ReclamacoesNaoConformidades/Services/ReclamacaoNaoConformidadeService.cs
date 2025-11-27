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
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ReclamacaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ReclamacaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Services;

public class ReclamacaoNaoConformidadeService : IReclamacaoNaoConformidadeService, ITransientDependency
{
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceBus _serviceBus;
    private readonly IRepository<ReclamacaoNaoConformidade> _reclamacaoNaoConformidades;
    private readonly ICurrentCompany _currentCompany;

    public ReclamacaoNaoConformidadeService(INaoConformidadeRepository naoConformidadeRepository,
        IDateTimeProvider dateTimeProvider,
        ICurrentTenant currentTenant, ICurrentEnvironment currentEnvironment, IUnitOfWork unitOfWork,
        IServiceBus serviceBus, IRepository<ReclamacaoNaoConformidade> reclamacaoNaoConformidades,
        ICurrentCompany currentCompany)
    {
        _naoConformidadeRepository = naoConformidadeRepository;
        _dateTimeProvider = dateTimeProvider;
        _currentTenant = currentTenant;
        _currentEnvironment = currentEnvironment;
        _unitOfWork = unitOfWork;
        _serviceBus = serviceBus;
        _reclamacaoNaoConformidades = reclamacaoNaoConformidades;
        _currentCompany = currentCompany;
    }

    public async Task Insert(Guid idNaoConformidade, ReclamacaoNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var inserirReclamacaoCommand = new InserirReclamacaoNaoConformidadeCommand(input);
        
        inserirReclamacaoCommand.ReclamacaoNaoConformidade.CompanyId = _currentCompany.Id;
        naoConformidade.Process(inserirReclamacaoCommand, _dateTimeProvider, _currentTenant.Id,
                _currentEnvironment.Id.Value);

        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
        
    }
    public async Task Update(Guid idNaoConformidade, ReclamacaoNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var atualizarCommand = new AlterarReclamacaoNaoConformidadeCommand(input);
        atualizarCommand.ReclamacaoNaoConformidade.CompanyId = _currentCompany.Id;

        naoConformidade.Process(atualizarCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);

        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }
    public async Task<ReclamacaoNaoConformidadeOutput> Get (Guid idNaoConformidade)
    {
        var entity = await _reclamacaoNaoConformidades
            .Where(e => e.CompanyId == _currentCompany.Id)
            .Where(entity => entity.IdNaoConformidade.Equals(idNaoConformidade))
            .Select(entity => new ReclamacaoNaoConformidadeOutput(entity))
            .FirstOrDefaultAsync();
        return entity;
    }
}