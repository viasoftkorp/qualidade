using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services.Estornos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.
    EstornarOrdemRetrabalhoValidatorServicesTests;

public abstract class EstornarOrdemRetrabalhoValidatorServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    protected class Mocker
    {
        public IRepository<OrdemRetrabalhoNaoConformidade> OrdemRetrabalhoNaoConformidadeRepository { get; set; }
        public IOrdemProducaoProvider OrdemProducaoProvider { get; set; }
        public IOperacaoService OperacaoService { get; set; }
    }

    protected Mocker GetMocker()
    {
        var mocker = new Mocker
        {
           OrdemRetrabalhoNaoConformidadeRepository = ServiceProvider.GetService<IRepository<OrdemRetrabalhoNaoConformidade>>(),
           OrdemProducaoProvider = Substitute.For<IOrdemProducaoProvider>(),
           OperacaoService = Substitute.For<IOperacaoService>()
        };
        return mocker;
    }

    protected EstornarOrdemRetrabalhoValidatorService GetService(Mocker mocker)
    {
        var controller = new EstornarOrdemRetrabalhoValidatorService(mocker.OrdemRetrabalhoNaoConformidadeRepository, mocker.OrdemProducaoProvider,
            mocker.OperacaoService);
        return controller;
    }
}