using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.PushNotifications.Abstractions.Notification;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Handlers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoqueLocais.Providers;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalhos.Handlers;

public abstract class MovimentacaoEstoqueHandlerTest : TestUtils.UnitTestBaseWithDbContext
{
    protected void MockGetAgregacaoReturn(Mocker mocker, AgregacaoNaoConformidade agregacao)
    {
        mocker.NaoConformidadeRepository.Get(agregacao.NaoConformidade.Id)
            .Returns(agregacao);
    }

    protected class Mocker
    {
        public IMovimentacaoEstoqueOrdemRetrabalhoService MovimentacaoEstoqueOrdemRetrabalhoService { get; set; }
        public IRepository<OrdemRetrabalhoNaoConformidade> OrdemRetrabalhoNaoConformidades { get; set; }
        public IOrdemRetrabalhoService OrdemRetrabalhoService { get; set; }
        public INaoConformidadeRepository NaoConformidadeRepository { get; set; }
        public IPushNotification PushNotification { get; set; }
        public IEstoqueLocalProvider EstoqueLocalProvider { get; set; }
    }

    protected Mocker GetMocker()
    {
        var mocker = new Mocker
        {
            MovimentacaoEstoqueOrdemRetrabalhoService = Substitute.For<IMovimentacaoEstoqueOrdemRetrabalhoService>(),
            OrdemRetrabalhoNaoConformidades = ServiceProvider.GetService<IRepository<OrdemRetrabalhoNaoConformidade>>(),
            OrdemRetrabalhoService = Substitute.For<IOrdemRetrabalhoService>(),
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>(),
            PushNotification = Substitute.For<IPushNotification>(),
            EstoqueLocalProvider = Substitute.For<IEstoqueLocalProvider>()
        };

        return mocker;
    }

    protected MovimentacaoEstoqueOrdemRetrabalhoHandler GetHandler(Mocker mocker)
    {
        var handler = new MovimentacaoEstoqueOrdemRetrabalhoHandler(mocker.MovimentacaoEstoqueOrdemRetrabalhoService, mocker.OrdemRetrabalhoNaoConformidades,
            mocker.OrdemRetrabalhoService, mocker.NaoConformidadeRepository, mocker.PushNotification, mocker.EstoqueLocalProvider);
        return handler;
    }
}