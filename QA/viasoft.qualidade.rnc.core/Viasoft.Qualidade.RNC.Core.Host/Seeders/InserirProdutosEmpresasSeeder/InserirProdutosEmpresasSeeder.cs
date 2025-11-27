using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Data.Seeder.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.InserirProdutosEmpresasSeeder;

public class InserirProdutosEmpresasSeeder : ISeedDataByCompany
{
    private readonly IReadOnlyRepository<SeederManagerPorEmpresa> _seederManagerPorEmpresas;
    private readonly IServiceBus _serviceBus;

    public InserirProdutosEmpresasSeeder(
        IReadOnlyRepository<SeederManagerPorEmpresa> seederManagerPorEmpresas,
        IServiceBus serviceBus)
    {
        _seederManagerPorEmpresas = seederManagerPorEmpresas;
        _serviceBus = serviceBus;
    }

    public async Task SeedDataAsync()
    {
        var finalizado = await _seederManagerPorEmpresas
            .AsNoTracking()
            .AnyAsync(seederManager => seederManager.InserirProdutosEmpresasSeederFinalizado);

        if (finalizado)
        {
            return;
        }

        await _serviceBus.SendLocal(new InserirProdutosEmpresasSeederCommand());
    }
}