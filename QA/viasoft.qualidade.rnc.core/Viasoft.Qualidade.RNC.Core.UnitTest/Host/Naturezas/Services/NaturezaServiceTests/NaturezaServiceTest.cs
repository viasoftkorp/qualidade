using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;
using Viasoft.Qualidade.RNC.Core.Host.Naturezas.Services;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Naturezas.Services.NaturezaServiceTests;

public abstract class NaturezaServiceTest: TestUtils.UnitTestBaseWithDbContext
{
    protected NaturezaServiceMocker GetMocker()
    {
        var mocker = new NaturezaServiceMocker
        {
            Naturezas = ServiceProvider.GetService<IRepository<Natureza>>(),
            NaoConformidades = ServiceProvider.GetService<IRepository<NaoConformidade>>()
        };
        return mocker;
    }
    protected static NaturezaService GetService(NaturezaServiceMocker mocker)
    {
        var service = new NaturezaService(mocker.Naturezas, mocker.NaoConformidades);
        return service;
    }
    protected class NaturezaServiceMocker
    {
        public IRepository<Natureza> Naturezas { get; set; }
        public IRepository<NaoConformidade> NaoConformidades { get; set; }
    }
}
    


