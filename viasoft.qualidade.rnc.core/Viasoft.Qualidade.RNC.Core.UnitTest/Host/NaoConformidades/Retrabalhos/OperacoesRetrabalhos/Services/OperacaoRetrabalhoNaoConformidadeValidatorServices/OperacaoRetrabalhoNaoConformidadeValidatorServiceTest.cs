using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OperacoesRetrabalhos.Services.OperacaoRetrabalhoNaoConformidadeValidatorServices;

public class OperacaoRetrabalhoNaoConformidadeValidatorServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    protected class Mocker
    {
        public IOperacaoService OperacaoService { get; set; }
        public IOrdemProducaoProvider OrdemProducaoProvider { get; set; }
    }

    protected Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            OperacaoService = Substitute.For<IOperacaoService>(),
            OrdemProducaoProvider = Substitute.For<IOrdemProducaoProvider>()
        };

        return mocker;
    }

    protected OperacaoRetrabalhoNaoConformidadeValidatorService GetService(Mocker mocker)
    {
        var service = new OperacaoRetrabalhoNaoConformidadeValidatorService(mocker.OperacaoService, mocker.OrdemProducaoProvider);

        return service;
    }
}