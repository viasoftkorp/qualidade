using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Services;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.
    Services;

public abstract class ImplementacaoEvitarReincidenciaNaoConformidadeServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    protected class Mocker
    {
        public IRepository<ImplementacaoEvitarReincidenciaNaoConformidade> Repository {get;set;}
        public ICurrentCompany CurrentCompany {get;set;}
        public ICurrentTenant CurrentTenant {get;set;}
        public ICurrentEnvironment CurrentEnvironment {get;set;}
        public INaoConformidadeRepository NaoConformidadeRepository {get;set;}
        public IDateTimeProvider DateTimeProvider {get;set;}
    }
    
    protected Mocker GetMocker()
    {
        var mocker = new Mocker {
            Repository = ServiceProvider.GetService<IRepository<ImplementacaoEvitarReincidenciaNaoConformidade>>(),
            CurrentTenant = Substitute.For<ICurrentTenant>(),
            CurrentEnvironment = Substitute.For<ICurrentEnvironment>(),
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            CurrentCompany = Substitute.For<ICurrentCompany>(),
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>()
        };
        mocker.CurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        mocker.CurrentTenant.Id = TestUtils.ObjectMother.Guids[0];
        mocker.CurrentEnvironment.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    protected ImplementacaoEvitarReincidenciaNaoConformidadeService GetService(Mocker mocker)
    {
        var service = new ImplementacaoEvitarReincidenciaNaoConformidadeService(mocker.Repository, mocker.CurrentCompany, mocker.CurrentTenant,
            mocker.CurrentEnvironment, mocker.NaoConformidadeRepository,mocker.DateTimeProvider, UnitOfWork, ServiceBus);

        return service;
    }

    
}