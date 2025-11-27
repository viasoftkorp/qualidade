using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Rebus.TestHelpers.Events;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.Identity.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services;

public class NaoConformidadeServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    protected Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            AgregacaoRepository = Substitute.For<INaoConformidadeRepository>(),
            CurrentTenant = TestUtils.ObjectMother.GetCurrentTenant(),
            CurrentEnvironment = TestUtils.ObjectMother.GetCurrentEnvironment(),
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            CurrentUser = Substitute.For<ICurrentUser>(),
            NaoConformidadeRepository = ServiceProvider.GetService<IRepository<NaoConformidade>>(),
            FakeNaoConformidadeValidationService = Substitute.For<INaoConformidadeValidationService>(),
            CurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.CurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        mocker.UnitOfWork = UnitOfWork;
        return mocker;
    }

    protected NaoConformidadeService GetService(Mocker mocker)
    {
        var service = new NaoConformidadeService(mocker.AgregacaoRepository, mocker.DateTimeProvider,
            mocker.CurrentTenant, mocker.CurrentEnvironment, mocker.UnitOfWork, ServiceBus, mocker.CurrentUser,
            mocker.NaoConformidadeRepository, mocker.FakeNaoConformidadeValidationService, mocker.CurrentCompany);

        return service;
    }

    protected class Mocker
    {
        public INaoConformidadeRepository AgregacaoRepository { get; set; }
        public ICurrentEnvironment CurrentEnvironment { get; set; }
        public ICurrentTenant CurrentTenant { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }
        public ICurrentUser CurrentUser { get; set; }
        public IRepository<NaoConformidade> NaoConformidadeRepository { get; set; }
        public INaoConformidadeValidationService FakeNaoConformidadeValidationService { get; set; }
        public ICurrentCompany CurrentCompany { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }

    }
}