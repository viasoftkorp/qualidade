using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Locais;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherLocaisSeeders;

public class PreencherLocaisHandler : IHandleMessages<SeedPreencherLocaisMessage>
{
    private readonly IRepository<OrdemRetrabalhoNaoConformidade> _ordemRetrabalhoNaoConformidades;
    private readonly IRepository<SeederManager> _seederManagers;
    private readonly ILocalService _localService;

    public PreencherLocaisHandler(IRepository<OrdemRetrabalhoNaoConformidade> ordemRetrabalhoNaoConformidades,
        IRepository<SeederManager> seederManagers, ILocalService localService)
    {
        _ordemRetrabalhoNaoConformidades = ordemRetrabalhoNaoConformidades;
        _seederManagers = seederManagers;
        _localService = localService;
    }
    public async Task Handle(SeedPreencherLocaisMessage message)
    {
        var ordensRetrabalho = await _ordemRetrabalhoNaoConformidades
            .AsNoTracking()
            .ToListAsync();

        var idsLocaisOridem = ordensRetrabalho.Select(e => e.IdLocalOrigem);
        var idsLocaisDestino = ordensRetrabalho.Select(e => e.IdLocalDestino);
        
        var idsLocais = idsLocaisOridem.Concat(idsLocaisDestino).Distinct().ToList();
        
        var seederManager = await _seederManagers.FirstAsync();

        seederManager.PreencherLocaisSeederFinalizado = true;
        
        await _localService.BatchInserirNaoCadastrados(idsLocais);
        await _seederManagers.UpdateAsync(seederManager, true);
    }
}