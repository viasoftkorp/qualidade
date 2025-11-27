using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.Configuracoes.Gerais;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.EstoqueLocais;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services.Gerar;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyParametros.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.
    GerarOrdemRetrabalhoValidatorServicesTests;

public abstract class GerarOrdemRetrabalhoValidatorServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    protected class Mocker
    {
        public IRepository<OrdemRetrabalhoNaoConformidade> OrdemRetrabalhoNaoConformidadeRepository { get; set; }
        public IRepository<ConfiguracaoGeral> ConfiguracaoGerais { get; set; }
        public ILegacyParametrosProvider LegacyParametrosProvider { get; set; }
        public IEstoqueLocalAclService EstoqueLocalAclService { get; set; }
        public IOrdemProducaoProvider OrdemProducaoProvider { get; set; }
        public IOperacaoService OperacaoService { get; set; }
    }

    protected Mocker GetMocker()
    {
        var mocker = new Mocker
        {
            OrdemRetrabalhoNaoConformidadeRepository =
                ServiceProvider.GetService<IRepository<OrdemRetrabalhoNaoConformidade>>(),
            ConfiguracaoGerais = ServiceProvider.GetService<IRepository<ConfiguracaoGeral>>(),
            LegacyParametrosProvider = Substitute.For<ILegacyParametrosProvider>(),
            EstoqueLocalAclService = Substitute.For<IEstoqueLocalAclService>(),
            OrdemProducaoProvider = Substitute.For<IOrdemProducaoProvider>(),
            OperacaoService = Substitute.For<IOperacaoService>()
        };
        return mocker;
    }

    protected GerarOrdemRetrabalhoValidatorService GetService(Mocker mocker)
    {
        var service = new GerarOrdemRetrabalhoValidatorService(mocker.OrdemRetrabalhoNaoConformidadeRepository,
            mocker.ConfiguracaoGerais, mocker.LegacyParametrosProvider, mocker.EstoqueLocalAclService,
            mocker.OrdemProducaoProvider,
            mocker.OperacaoService);
        return service;
    }
}