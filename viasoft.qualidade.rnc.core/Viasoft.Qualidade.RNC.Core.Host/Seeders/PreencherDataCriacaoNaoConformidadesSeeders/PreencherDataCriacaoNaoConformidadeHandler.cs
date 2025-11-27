using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherDataCriacaoNaoConformidadesSeeders.Contracts;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherDataCriacaoNaoConformidadesSeeders;

public class PreencherDataCriacaoNaoConformidadeHandler : IHandleMessages<SeedPreencherDataCriacaoNaoConformidadesMessage>
{
    private readonly IRepository<NaoConformidade> _naoConformidades;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<SeederManager> _seederManagers;

    public PreencherDataCriacaoNaoConformidadeHandler(IRepository<NaoConformidade> naoConformidades,
        IUnitOfWork unitOfWork, IRepository<SeederManager> seederManagers)
    {
        _naoConformidades = naoConformidades;
        _unitOfWork = unitOfWork;
        _seederManagers = seederManagers;
    }
    public async Task Handle(SeedPreencherDataCriacaoNaoConformidadesMessage message)
    {
        var seederManager = await _seederManagers.FirstAsync();
        seederManager.PreencherDataCriacaoNaoConformidadeSeederFinalizado = true;
        using (_unitOfWork.Begin(op => op.LazyTransactionInitiation = false))
        {
            await _naoConformidades.BatchUpdateAsync(e => new NaoConformidade
            {
                DataCriacao = e.CreationTime
            }, e => e.DataCriacao != e.CreationTime);
            await _seederManagers.UpdateAsync(seederManager);

            await _unitOfWork.CompleteAsync();
        }
    }
}