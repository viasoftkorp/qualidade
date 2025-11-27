using Microsoft.Extensions.DependencyInjection;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Services;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.AcoesPreventivas.Services.AcaoPreventivaServiceTests;

public abstract class AcaoPreventivaServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    protected class Mocker
    {
        public IRepository<AcaoPreventiva> AcaoPreventiva { get; set; }
        public IRepository<AcaoPreventivaNaoConformidade> AcaoPreventivaNaoConformidades { get; set; }
        public IRepository<Usuario> Usuarios { get; set; }
    }

    protected Mocker GetMocker()
    {
        var mocker = new Mocker
        {
            AcaoPreventivaNaoConformidades = ServiceProvider.GetService<IRepository<AcaoPreventivaNaoConformidade>>(),
            AcaoPreventiva = ServiceProvider.GetService<IRepository<AcaoPreventiva>>(),
            Usuarios = ServiceProvider.GetService<IRepository<Usuario>>()
        };

        return mocker;
    }

    protected AcaoPreventivaService GetService(Mocker mocker)
    {
        var service = new AcaoPreventivaService(mocker.AcaoPreventiva, mocker.AcaoPreventivaNaoConformidades,
            mocker.Usuarios);

        return service;
    }
}