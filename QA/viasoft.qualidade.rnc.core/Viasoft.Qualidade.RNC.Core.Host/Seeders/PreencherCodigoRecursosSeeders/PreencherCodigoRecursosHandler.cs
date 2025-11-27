using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Recursos;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherCodigoRecursosSeeders.Contracts;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherCodigoRecursosSeeders;

public class PreencherCodigoRecursosHandler : IHandleMessages<SeedPreencherCodigoRecursosMessage>
{
    private readonly IRepository<Recurso> _recursos;
    private readonly IRecursosProxyService _recursosProxyService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<SeederManager> _seederManagers;

    public PreencherCodigoRecursosHandler(IRepository<Recurso> recursos, IRecursosProxyService recursosProxyService,
        IUnitOfWork unitOfWork, IRepository<SeederManager> seederManagers)
    {
        _recursos = recursos;
        _recursosProxyService = recursosProxyService;
        _unitOfWork = unitOfWork;
        _seederManagers = seederManagers;
    }
    public async Task Handle(SeedPreencherCodigoRecursosMessage message)
    {
        var recursosToUpdate = await _recursos.Where(e => string.IsNullOrWhiteSpace(e.Codigo)).ToListAsync();
        var recursosToUpdateIds = recursosToUpdate.ConvertAll(e => e.Id);

        var codigoRecursosDictionary = await GetCodigoRecursosDictionary(recursosToUpdateIds);

        var seederManager = await _seederManagers.FirstAsync();
        seederManager.PreencherCodigoRecursosSeederFinalizado = true;
        using (_unitOfWork.Begin(op => op.LazyTransactionInitiation = false))
        {
            foreach (var recurso in recursosToUpdate)
            {
                recurso.Codigo = codigoRecursosDictionary[recurso.Id];
                await _recursos.UpdateAsync(recurso);
            }

            await _seederManagers.UpdateAsync(seederManager);

            await _unitOfWork.CompleteAsync();
        }
    }

    private async Task<Dictionary<Guid, string>> GetCodigoRecursosDictionary(List<Guid> idsRecursos)
    {
        var recursosFromEngenhariaCore = await _recursosProxyService.GetAllByIdsPaginando(idsRecursos);
        var output = recursosFromEngenhariaCore.ToDictionary(e => e.Id, e => e.Codigo);
        return output;
    }
}