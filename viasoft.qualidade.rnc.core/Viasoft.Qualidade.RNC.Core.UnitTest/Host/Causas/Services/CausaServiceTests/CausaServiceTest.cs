using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Services;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Causas.Services.CausaServiceTests;

public class CausaServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    protected CausaServiceMocker GetMocker()
    {
        var mocker = new CausaServiceMocker
        {
            Causas = ServiceProvider.GetService<IRepository<Causa>>(),
            Defeitos = ServiceProvider.GetService<IRepository<Defeito>>(),
            CausaNaoConformidades = ServiceProvider.GetService<IRepository<CausaNaoConformidade>>()
        };
        return mocker;
    }
    protected static CausaService GetService(CausaServiceMocker mocker)
    {
        var service = new CausaService(mocker.Causas, mocker.Defeitos, mocker.CausaNaoConformidades);
        return service;
    }
    protected class CausaServiceMocker
    {
        public IRepository<Causa> Causas { get; set; }
        public IRepository<Defeito> Defeitos { get; set; }
        public IRepository<CausaNaoConformidade> CausaNaoConformidades { get; set; }
    }
}