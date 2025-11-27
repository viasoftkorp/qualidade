using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Data.Seeder.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherIdsCausasCentrosCustosNaoConformidadesSeeders.Contracts;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherIdsCausasCentrosCustosNaoConformidadesSeeders;

public class PreencherIdsCausasCentrosCustosNaoConformidadesSeeder : ISeedData
{
    private readonly IRepository<SeederManager> _seederManagers;
    private readonly IServiceBus _serviceBus;

    public PreencherIdsCausasCentrosCustosNaoConformidadesSeeder(IRepository<SeederManager> seederManagers, IServiceBus serviceBus)
    {
        _seederManagers = seederManagers;
        _serviceBus = serviceBus;
    }
    public async Task SeedDataAsync()
    {
        var seederManager = await _seederManagers.AsNoTracking().FirstAsync();
        if (seederManager.PreencherIdsCausasCentrosCustosNaoConformidadesSeederFinalizado)
        {
            return;
        }

        await _serviceBus.SendLocal(new SeedPreencherIdsCausasCentrosCustosNaoConformidadesCommand());
    }
}
