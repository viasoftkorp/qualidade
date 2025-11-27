using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.ConclusaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.CorrigirNaoConformidadesFechadasSemConclusaoSeeders;

public class CorrigirNaoConformidadesFechadasSemConclusaoHandler : IHandleMessages<CorrigirNaoConformidadesFechadasSemConclusaoMessage>
{
    private readonly IRepository<SeederManager> _seederManagers;
    private readonly IRepository<NaoConformidade> _naoConformidades;
    private readonly IRepository<ConclusaoNaoConformidade> _conclusaoNaoConformidades;
    private readonly IUnitOfWork _unitOfWork;

    public CorrigirNaoConformidadesFechadasSemConclusaoHandler(IRepository<SeederManager> seederManagers,
        IRepository<NaoConformidade> naoConformidades, IRepository<ConclusaoNaoConformidade> conclusaoNaoConformidades,
        IUnitOfWork unitOfWork)
    {
        _seederManagers = seederManagers;
        _naoConformidades = naoConformidades;
        _conclusaoNaoConformidades = conclusaoNaoConformidades;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(CorrigirNaoConformidadesFechadasSemConclusaoMessage message)
    {
        var seederManager = await _seederManagers.FirstAsync();
        seederManager.CorrigirNaoConformidadesFechadasSemConclusaoSeederFinalizado = true;
        
        using (_unitOfWork.Begin(e => e.LazyTransactionInitiation = false))
        {
            await _naoConformidades.BatchUpdateAsync(e => new NaoConformidade
            {
                Status = StatusNaoConformidade.Aberto
            }, e => e.Status == StatusNaoConformidade.Fechado 
                    && !_conclusaoNaoConformidades.Any(conclusao => conclusao.IdNaoConformidade == e.Id));

            await _seederManagers.UpdateAsync(seederManager);
            await _unitOfWork.CompleteAsync();
        }    
    }
}