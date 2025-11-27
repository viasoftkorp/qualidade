using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.PushNotifications.Abstractions.Notification;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Locais;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.EstoqueLocais;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services.Estornos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services.Gerar;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Services;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.
    OrdemRetrabalhoServicesTests;

public abstract class OrdemRetrabalhoServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    protected void MockValidatorServiceReturn(Mocker mocker, AgregacaoNaoConformidade agregacao)
    {
        var ordemRetrabalhoInput = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0]
        };
        mocker.GerarOrdemRetrabalhoValidatorService
            .ValidateOperacaoEngenhariaFinal()
            .ValidateOperacaoEngenhariaDuplicada()
            .ValidateOdf()
            .ValidateLote(ordemRetrabalhoInput)
            .ValidateAsync(agregacao)
            .Returns(GerarOrdemRetrabalhoValidationResult.Ok);
    }

    protected void MockGetAgregacaoReturn(Mocker mocker, AgregacaoNaoConformidade agregacao)
    {
        mocker.NaoConformidadeRepository.Get(agregacao.NaoConformidade.Id)
            .Returns(agregacao);
    }

    protected class Mocker
    {
        public IRepository<OrdemRetrabalhoNaoConformidade> Repository { get; set; }
        public INaoConformidadeRepository NaoConformidadeRepository { get; set; }
        public IGerarOrdemRetrabalhoValidatorService GerarOrdemRetrabalhoValidatorService { get; set; }
        public IEstornarOrdemRetrabalhoValidatorService EstornarOrdemRetrabalhoValidatorService { get; set; }
        public ILocalProvider LocalProvider { get; set; }
        public IPushNotification PushNotification { get; set; }
        public IEstoqueLocalAclService EstoqueLocalAclService { get; set; }
        public IOrdemRetrabalhoAclService OrdemRetrabalhoAclService { get; set; }
        public IExternalOrdemRetrabalhoService ExternalOrdemRetrabalhoService { get; set; }
        public IRepository<Local> Locais { get; set; }
    }

    protected Mocker GetMocker()
    {
        var mocker = new Mocker
        {
            Repository = ServiceProvider.GetService<IRepository<OrdemRetrabalhoNaoConformidade>>(),
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>(),
            GerarOrdemRetrabalhoValidatorService = Substitute.For<IGerarOrdemRetrabalhoValidatorService>(),
            EstornarOrdemRetrabalhoValidatorService = Substitute.For<IEstornarOrdemRetrabalhoValidatorService>(),
            LocalProvider = Substitute.For<ILocalProvider>(),
            PushNotification = Substitute.For<IPushNotification>(),
            OrdemRetrabalhoAclService = Substitute.For<IOrdemRetrabalhoAclService>(),
            EstoqueLocalAclService = Substitute.For<IEstoqueLocalAclService>(),
            ExternalOrdemRetrabalhoService = Substitute.For<IExternalOrdemRetrabalhoService>(),
            Locais = ServiceProvider.GetService<IRepository<Local>>()
        };

        return mocker;
    }

    protected OrdemRetrabalhoService GetService(Mocker mocker)
    {
        var service = new OrdemRetrabalhoService(mocker.Repository, mocker.NaoConformidadeRepository,
            mocker.GerarOrdemRetrabalhoValidatorService, mocker.EstornarOrdemRetrabalhoValidatorService,
            mocker.LocalProvider, mocker.PushNotification, mocker.EstoqueLocalAclService, mocker.OrdemRetrabalhoAclService,
            mocker.ExternalOrdemRetrabalhoService, mocker.Locais, ServiceBus);
        return service;
    }
}