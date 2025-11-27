using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Services;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ServicosNaoConformidades.Services.ServicoNaoConformidadeServices;

public abstract class ServicoNaoConformidadeServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    private ServicoNaoConformidadeserviceMocker Mocker { get; set; }

    protected void MockValidarTempo(int horas, int minutos, bool result)
    {
        Mocker.ServicoValidatorService.ValidarTempo(horas, minutos).Returns(result);
    }
    protected ServicoNaoConformidadeserviceMocker GetMocker()
    {
        var mocker = new ServicoNaoConformidadeserviceMocker()
        {
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>(),
            CurrentTenant = TestUtils.ObjectMother.GetCurrentTenant(),
            CurrentEnvironment = TestUtils.ObjectMother.GetCurrentEnvironment(),
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            ServicoNaoConformidadeRepository = ServiceProvider.GetService<IRepository<ServicoNaoConformidade>>(),
            CurrentCompany = Substitute.For<ICurrentCompany>(),
            ServicoValidatorService = Substitute.For<IServicoValidatorService>()
        };
        mocker.CurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        Mocker = mocker;
        
        return Mocker;
    }

    protected ServicoNaoConformidadeService GetService(ServicoNaoConformidadeserviceMocker mocker)
    {
        var service = new ServicoNaoConformidadeService(mocker.NaoConformidadeRepository,
            mocker.DateTimeProvider,
            mocker.CurrentTenant, mocker.CurrentEnvironment, UnitOfWork, ServiceBus, mocker.ServicoNaoConformidadeRepository,
            mocker.CurrentCompany, mocker.ServicoValidatorService);

        return service;
    }

    protected class ServicoNaoConformidadeserviceMocker
    {
        public INaoConformidadeRepository NaoConformidadeRepository { get; set; }
        public ICurrentEnvironment CurrentEnvironment { get; set; }
        public ICurrentTenant CurrentTenant { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }
        public IRepository<ServicoNaoConformidade> ServicoNaoConformidadeRepository { get; set; }
        public ICurrentCompany CurrentCompany { get; set; }
        public IServicoValidatorService ServicoValidatorService { get; set; }

    }
}