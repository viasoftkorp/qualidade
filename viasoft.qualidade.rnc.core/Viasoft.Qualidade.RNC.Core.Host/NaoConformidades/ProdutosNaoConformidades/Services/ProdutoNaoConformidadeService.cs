using System;
using System.Collections.Generic;
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
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Commands.ProdutosNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Services;

public class ProdutoNaoConformidadeService : IProdutoNaoConformidadeService, ITransientDependency
{
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentEnvironment _currentEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceBus _serviceBus;
    private readonly IRepository<ProdutoNaoConformidade> _produtoNaoConformidades;
    private readonly ICurrentCompany _currentCompany;

    public ProdutoNaoConformidadeService(INaoConformidadeRepository naoConformidadeRepository, IDateTimeProvider dateTimeProvider,
        ICurrentTenant currentTenant, ICurrentEnvironment currentEnvironment, IUnitOfWork unitOfWork,
        IServiceBus serviceBus, IRepository<ProdutoNaoConformidade> produtoNaoConformidades, ICurrentCompany currentCompany)
    {
        _naoConformidadeRepository = naoConformidadeRepository;
        _dateTimeProvider = dateTimeProvider;
        _currentTenant = currentTenant;
        _currentEnvironment = currentEnvironment;
        _unitOfWork = unitOfWork;
        _serviceBus = serviceBus;
        _produtoNaoConformidades = produtoNaoConformidades;
        _currentCompany = currentCompany;
    }
    public async Task<ProdutoNaoConformidadeOutput> Get (Guid idNaoConformidade, Guid id)
    {
        var entity = await _produtoNaoConformidades.Where(entity => entity.IdNaoConformidade.Equals(idNaoConformidade))
            .Where(entity => entity.Id.Equals(id))
            .Select(entity => new ProdutoNaoConformidadeOutput(entity))
            .FirstOrDefaultAsync();
        return entity;
    }

    public async Task Update(Guid idNaoConformidade, Guid idProdutoNaoConformidade, ProdutoNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var atualizarCommand = new AlterarProdutoNaoConformidadeCommand(input);
        atualizarCommand.ProdutoNaoConformidade.Id = idProdutoNaoConformidade;
        atualizarCommand.ProdutoNaoConformidade.CompanyId = _currentCompany.Id;

        naoConformidade.Process(atualizarCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

    public async Task Insert(Guid idNaoConformidade, ProdutoNaoConformidadeInput input)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var inserirCommand = new InserirProdutoNaoConformidadeCommand(input);
        inserirCommand.ProdutoNaoConformidade.CompanyId = _currentCompany.Id;

        naoConformidade.Process(inserirCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }

    public async Task Remove(Guid idNaoConformidade, Guid idProdutoNaoConformidade)
    {
        var naoConformidade = await _naoConformidadeRepository.Get(idNaoConformidade);
        var removerCommand = new RemoverProdutoNaoConformidadeCommand(idProdutoNaoConformidade);
        naoConformidade.Process(removerCommand, _dateTimeProvider, _currentTenant.Id,
            _currentEnvironment.Id.Value);
        await naoConformidade.CommitUpdate(naoConformidade, _unitOfWork, _serviceBus, _naoConformidadeRepository);
    }
}