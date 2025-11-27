using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Data.Seeder.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.CorrigirUsuariosSolucoesSeeder;

public class CorrigirUsuariosSolucoesSeeder : ISeedData
{
    private readonly IReadOnlyRepository<SeederManager> _seederManagers;
    private readonly IServiceBus _serviceBus;

    public CorrigirUsuariosSolucoesSeeder(
        IReadOnlyRepository<SeederManager> seederManagers,
        IServiceBus serviceBus)
    {
        _seederManagers = seederManagers;
        _serviceBus = serviceBus;
    }

    public async Task SeedDataAsync()
    {
        var finalizado = await _seederManagers.AsNoTracking()
            .AnyAsync(seederManager => seederManager.CorrigirUsuariosSolucoesSeederFinalizado);

        if (finalizado)
        {
            return;
        }

        await _serviceBus.SendLocal(new CorrigirUsuariosSolucoesCommand());
    }
}