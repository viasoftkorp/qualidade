using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.PushNotifications.Abstractions.Notification;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Pessoas.Services;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Produtos.Services;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Usuarios.Services;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Handlers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Services;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Handlers.NaoConformidadeViewHandlerTests;

public abstract class NaoConformidadeViewHandlerTest : TestUtils.UnitTestBaseWithDbContext
{
    protected Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            FakeNotaFiscalEntradaProvider = Substitute.For<INotaFiscalEntradaProvider>(),
            NaoConformidadesRepository = ServiceProvider.GetService<IRepository<NaoConformidade>>(),
            FakeNotaFiscalSaidaProvider = Substitute.For<INotaFiscalSaidaProvider>(),
            FakePushNotification = Substitute.For<IPushNotification>(),
            FakeUsuarioService = Substitute.For<IUsuarioService>(),
            ProdutoService = Substitute.For<IProdutoService>(),
            PessoaService = Substitute.For<IPessoaService>(),
        };
        return mocker;
    }

    protected NaoConformidadeViewHandler GetHandler(Mocker mocker)
    {
        var service = new NaoConformidadeViewHandler(mocker.FakeNotaFiscalEntradaProvider,
            mocker.NaoConformidadesRepository, mocker.FakeNotaFiscalSaidaProvider, mocker.FakePushNotification,
            mocker.FakeUsuarioService, mocker.ProdutoService, mocker.PessoaService);

        return service;
    }

    public class Mocker
    {
        public INotaFiscalEntradaProvider FakeNotaFiscalEntradaProvider { get; set; }
        public IRepository<NaoConformidade> NaoConformidadesRepository { get; set; }
        public INotaFiscalSaidaProvider FakeNotaFiscalSaidaProvider { get; set; }
        public IPushNotification FakePushNotification { get; set; }
        public IUsuarioService FakeUsuarioService { get; set; }
        public IProdutoService ProdutoService { get; set; }
        public IPessoaService PessoaService { get; set; }
    }
}