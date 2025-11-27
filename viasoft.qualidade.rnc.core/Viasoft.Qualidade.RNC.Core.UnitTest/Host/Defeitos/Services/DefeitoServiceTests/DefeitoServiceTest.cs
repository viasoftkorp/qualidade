using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.Defeitos.Services;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Defeitos.Services.DefeitoServiceTests;

public abstract class DefeitoServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    protected DefeitoServiceMocker GetMocker()
    {
        var mocker = new DefeitoServiceMocker
        {
            Defeitos = ServiceProvider.GetService<IRepository<Defeito>>(),
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            CurrentEnvironment = ServiceProvider.GetService<ICurrentEnvironment>(),
            CurrentTenant = ServiceProvider.GetService<ICurrentTenant>(),
            DefeitoNaoConformidades = ServiceProvider.GetService<IRepository<DefeitoNaoConformidade>>(),
            Solucoes = ServiceProvider.GetService<IRepository<Solucao>>(),
            Causas = ServiceProvider.GetService<IRepository<Causa>>()
        };
        return mocker;
    }

    protected DefeitoService GetService(DefeitoServiceMocker mocker)
    {
        var service = new DefeitoService(mocker.Defeitos, ServiceBus, mocker.CurrentTenant, mocker.CurrentEnvironment,
            mocker.DateTimeProvider, mocker.DefeitoNaoConformidades, mocker.Solucoes, mocker.Causas);
        return service;
    }

    protected class DefeitoServiceMocker
    {
        public IRepository<Defeito> Defeitos { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }
        public ICurrentEnvironment CurrentEnvironment { get; set; }
        public ICurrentTenant CurrentTenant { get; set; }
        public IRepository<DefeitoNaoConformidade> DefeitoNaoConformidades { get; set; }
        public IRepository<Solucao> Solucoes { get; set; }
        public IRepository<Causa> Causas { get; set; }
    }
}