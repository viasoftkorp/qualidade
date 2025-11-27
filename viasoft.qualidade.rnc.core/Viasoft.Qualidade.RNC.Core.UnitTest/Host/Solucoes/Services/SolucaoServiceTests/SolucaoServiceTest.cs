using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DateTimeProvider;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Environment;
using Viasoft.Core.MultiTenancy.Abstractions.Tenant;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.UnidadeMedidaProdutos;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Services;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Services;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests;

public abstract class SolucaoServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    private SolucaoServiceMocker Mocker { get; set; }

    protected void MockValidarTempo(int horas, int minutos, bool result)
    {
        Mocker.ServicoValidatorService.ValidarTempo(horas, minutos).Returns(result);
    }
    
    protected SolucaoServiceMocker GetMocker()
    {
        var mocker = new SolucaoServiceMocker
        {
            Solucoes = ServiceProvider.GetService<IRepository<Solucao>>(),
            ServicoSolucoes = ServiceProvider.GetService<IRepository<ServicoSolucao>>(),
            ProdutoSolucoes = ServiceProvider.GetService<IRepository<ProdutoSolucao>>(),
            Produto = ServiceProvider.GetService<IRepository<Produto>>(),
            UnidadeMedida = ServiceProvider.GetService<IRepository<UnidadeMedidaProduto>>(),
            Recurso = ServiceProvider.GetService<IRepository<Recurso>>(),
            DateTimeProvider = Substitute.For<IDateTimeProvider>(),
            CurrentEnvironment = ServiceProvider.GetService<ICurrentEnvironment>(),
            CurrentTenant = ServiceProvider.GetService<ICurrentTenant>(),
            Defeitos = ServiceProvider.GetService<IRepository<Defeito>>(),
            SolucaoNaoConformidades = ServiceProvider.GetService<IRepository<SolucaoNaoConformidade>>(),
            ServicoValidatorService = Substitute.For<IServicoValidatorService>()
        };
        Mocker = mocker;
        return Mocker;
    }

    protected SolucaoService GetService(SolucaoServiceMocker mocker)
    {
        this.ConfigureServices();

        var service = new SolucaoService(mocker.Solucoes, mocker.ProdutoSolucoes, mocker.ServicoSolucoes,
            mocker.Produto, mocker.UnidadeMedida, mocker.Recurso, ServiceBus, mocker.DateTimeProvider,
            mocker.CurrentEnvironment, mocker.CurrentTenant, mocker.Defeitos, mocker.SolucaoNaoConformidades,
            mocker.ServicoValidatorService);
        return service;
    }

    protected class SolucaoServiceMocker
    {
        public IRepository<Solucao> Solucoes { get; set; }
        public IRepository<ServicoSolucao> ServicoSolucoes { get; set; }
        public IRepository<ProdutoSolucao> ProdutoSolucoes { get; set; }
        public IDateTimeProvider DateTimeProvider { get; set; }
        public ICurrentEnvironment CurrentEnvironment { get; set; }
        public ICurrentTenant CurrentTenant { get; set; }
        public IRepository<Produto> Produto { get; set; }
        public IRepository<UnidadeMedidaProduto> UnidadeMedida { get; set; }
        public IRepository<Recurso> Recurso { get; set; }
        public IRepository<Defeito> Defeitos { get; set; }
        public IRepository<SolucaoNaoConformidade> SolucaoNaoConformidades { get; set; }
        public IServicoValidatorService ServicoValidatorService { get; set; }
    }
}