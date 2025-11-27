using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Data.Seeder.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders
{
    public class AdicionaSeederManagerSeeder : ISeedData
    {
        private readonly IRepository<SeederManager> _seederManagers;

        public AdicionaSeederManagerSeeder(IRepository<SeederManager> seederManagers)
        {
            _seederManagers = seederManagers;
        }

        public async Task SeedDataAsync()
        {
            var seederManager = await _seederManagers.FirstOrDefaultAsync();

            if (seederManager != null)
            {
                return;
            }

            seederManager = new SeederManager();
            await _seederManagers.InsertAsync(seederManager, true);
        }
    }
}