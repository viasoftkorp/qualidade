using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.
    Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Services;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OperacoesRetrabalhos.Services.
    OperacaoRetrabalhoNaoConformidadeServices;

public abstract class OperacaoRetrabalhoNaoConformidadeServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    protected class Mocker
    {
        public IOperacaoRetrabalhoAclService OperacaoRetrabalhoAclService {get;set;}
        public INaoConformidadeRepository NaoConformidadeRepository {get;set;}
        public IRepository<OperacaoRetrabalhoNaoConformidade> OperacaoRetrabalhoNaoConformidades {get;set;}
        public IOperacaoRetrabalhoNaoConformidadeValidatorService OperacaoRetrabalhoNaoConformidadeValidatorService {get;set;}
        public IRepository<Operacao> Operacoes {get;set;}
        public IRepository<Recurso> Recursos {get;set;}
        public IOperacaoRetrabalhoProxyService OperacaoRetrabalhoProxyService { get; set; }
    }

    protected Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            OperacaoRetrabalhoAclService = Substitute.For<IOperacaoRetrabalhoAclService>(),
            NaoConformidadeRepository = Substitute.For<INaoConformidadeRepository>(),
            OperacaoRetrabalhoNaoConformidades = ServiceProvider.GetService<IRepository<OperacaoRetrabalhoNaoConformidade>>(),
            OperacaoRetrabalhoNaoConformidadeValidatorService = Substitute.For<IOperacaoRetrabalhoNaoConformidadeValidatorService>(),
            Operacoes = ServiceProvider.GetService<IRepository<Operacao>>(),
            Recursos = ServiceProvider.GetService<IRepository<Recurso>>(),
            OperacaoRetrabalhoProxyService = Substitute.For<IOperacaoRetrabalhoProxyService>()
        };

        return mocker;
    }

    protected OperacaoRetrabalhoNaoConformidadeService GetService(Mocker mocker)
    {
        var service = new OperacaoRetrabalhoNaoConformidadeService(mocker.OperacaoRetrabalhoAclService, mocker.NaoConformidadeRepository,
            mocker.OperacaoRetrabalhoNaoConformidades, mocker.OperacaoRetrabalhoNaoConformidadeValidatorService,
            mocker.Operacoes, mocker.Recursos, mocker.OperacaoRetrabalhoProxyService);

        return service;
    }
}