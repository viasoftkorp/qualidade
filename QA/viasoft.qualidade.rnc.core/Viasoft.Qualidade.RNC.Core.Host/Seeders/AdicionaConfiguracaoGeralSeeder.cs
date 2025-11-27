using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Data.Seeder.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.Configuracoes.Gerais;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders;

public class AdicionaConfiguracaoGeralSeeder : ISeedData
{
    private readonly IRepository<ConfiguracaoGeral> _configuracaoGerais;

    public AdicionaConfiguracaoGeralSeeder(IRepository<ConfiguracaoGeral> configuracaoGerais)
    {
        _configuracaoGerais = configuracaoGerais;
    }

    public async Task SeedDataAsync()
    {
        var configuracaoCriada = await _configuracaoGerais.AnyAsync();
        if (configuracaoCriada)
        {
            return;
        }
        await _configuracaoGerais.InsertAsync(new ConfiguracaoGeral(), true);
    }
}