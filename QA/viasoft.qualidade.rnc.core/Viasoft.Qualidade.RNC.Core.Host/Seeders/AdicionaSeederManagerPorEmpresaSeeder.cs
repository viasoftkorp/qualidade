using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Data.Seeder.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders;

public class AdicionaSeederManagerPorEmpresaSeeder : ISeedDataByCompany
{
    private readonly IRepository<SeederManagerPorEmpresa> _seederManagerPorEmpresas;

    public AdicionaSeederManagerPorEmpresaSeeder(IRepository<SeederManagerPorEmpresa> seederManagerPorEmpresas)
    {
        _seederManagerPorEmpresas = seederManagerPorEmpresas;
    }

    public async Task SeedDataAsync()
    {
        var inserido = await _seederManagerPorEmpresas.AsNoTracking().AnyAsync();
        if (inserido)
        {
            return;
        }

        var seederManagerPorEmpresa = new SeederManagerPorEmpresa();
        await _seederManagerPorEmpresas.InsertAsync(seederManagerPorEmpresa, true);
    }
}