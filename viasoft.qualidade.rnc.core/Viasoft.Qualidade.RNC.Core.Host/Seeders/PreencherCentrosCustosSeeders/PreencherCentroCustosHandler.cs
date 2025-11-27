using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.CentroCustos.Services;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherCentrosCustosSeeders.Contracts;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherCentrosCustosSeeders;

public class PreencherCentroCustosHandler : IHandleMessages<SeedPreencherCentroCustosMessage>
{
    private readonly IRepository<CentroCustoCausaNaoConformidade> _centroCustoCausaNaoConformidades;
    private readonly ICentroCustoService _centroCustoService;
    private readonly IRepository<SeederManager> _seederManagers;

    public PreencherCentroCustosHandler(IRepository<CentroCustoCausaNaoConformidade> centroCustoCausaNaoConformidades, 
        ICentroCustoService centroCustoService, IRepository<SeederManager> seederManagers)
    {
        _centroCustoCausaNaoConformidades = centroCustoCausaNaoConformidades;
        _centroCustoService = centroCustoService;
        _seederManagers = seederManagers;
    }
    public async Task Handle(SeedPreencherCentroCustosMessage message)
    {
        var idsCentroCustosUsados = await _centroCustoCausaNaoConformidades
            .AsNoTracking()
            .Select(e => e.IdCentroCusto)
            .ToListAsync();
        
        await _centroCustoService.BatchInserirNaoCadastrados(idsCentroCustosUsados);
        await AtualizarSeederManager();
    }

    private async Task AtualizarSeederManager()
    {
        var seederManager = await _seederManagers.FirstAsync();
        seederManager.PreencherCentroCustosSeederFinalizado = true;
        await _seederManagers.UpdateAsync(seederManager, true);
    }
}
