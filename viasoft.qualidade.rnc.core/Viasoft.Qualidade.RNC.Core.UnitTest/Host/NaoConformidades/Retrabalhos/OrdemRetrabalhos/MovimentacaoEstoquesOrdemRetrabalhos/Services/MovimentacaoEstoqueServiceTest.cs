using Microsoft.Extensions.Logging;
using NSubstitute;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyParametros.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalhos.Services;

public abstract class MovimentacaoEstoqueServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    protected class Mocker
    {
        public ILogger<MovimentacaoEstoqueOrdemRetrabalhoService> Logger { get; set; }
        public ICurrentEnvironment CurrentEnvironment { get; set; }
        public ICurrentTenant CurrentTenant { get; set; }
        public IMovimentacaoEstoqueAclService MovimentacaoEstoqueAclService { get; set; }
        public IOrdemProducaoProvider OrdemProducaoProvider { get; set; }
        public ILegacyParametrosProvider LegacyParametrosProvider { get; set; }
        public IExternalMovimentacaoService ExternalMovimentacaoService { get; set; }
    }

    protected Mocker GetMocker()
    {
        var mocker = new Mocker
        {
            Logger = Substitute.For<ILogger<MovimentacaoEstoqueOrdemRetrabalhoService>>(),
            CurrentEnvironment = Substitute.For<ICurrentEnvironment>(),
            CurrentTenant = Substitute.For<ICurrentTenant>(),
            MovimentacaoEstoqueAclService = Substitute.For<IMovimentacaoEstoqueAclService>(),
            OrdemProducaoProvider = Substitute.For<IOrdemProducaoProvider>(),
            LegacyParametrosProvider = Substitute.For<ILegacyParametrosProvider>(),
            ExternalMovimentacaoService = Substitute.For<IExternalMovimentacaoService>()
        };
        mocker.CurrentTenant.Id = TestUtils.ObjectMother.Guids[0];
        mocker.CurrentEnvironment.Id = TestUtils.ObjectMother.Guids[0];

        return mocker;
    }

    protected MovimentacaoEstoqueOrdemRetrabalhoService GetService(Mocker mocker)
    {
        var service = new MovimentacaoEstoqueOrdemRetrabalhoService(mocker.Logger,
            mocker.CurrentEnvironment, mocker.CurrentTenant, mocker.MovimentacaoEstoqueAclService, mocker.OrdemProducaoProvider,
            mocker.LegacyParametrosProvider, mocker.ExternalMovimentacaoService);
        return service;
    }
}