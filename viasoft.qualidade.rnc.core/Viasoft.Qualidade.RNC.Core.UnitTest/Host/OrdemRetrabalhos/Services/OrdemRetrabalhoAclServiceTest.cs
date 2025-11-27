using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Clientes;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.PedidoVendas;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyParametros.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsPreRegistrations.CategoriaProdutos.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Recursos;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.OrdemRetrabalhos.Services;

public abstract class OrdemRetrabalhoAclServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    public class Mocker
    {
        public ICurrentCompany CurrentCompany { get; set; }
        public ICategoriaProdutoProvider CategoriaProdutoProvider { get; set; }
        public IOrdemProducaoProvider OrdemProducaoProvider { get; set; }
        public IRecursosProxyService RecursosProxyService { get; set; }
        public IRepository<Cliente> ClientesRepository { get; set; }
        public IRepository<Produto> ProdutosRepository { get; set; }
        public ILocalProvider LocalProvider { get; set; }
        public ILegacyParametrosProvider LegacyParametrosProvider { get; set; }
    }

    protected Mocker GetMocker()
    {
        var mocker = new Mocker()
        {
            CurrentCompany = Substitute.For<ICurrentCompany>(),
            CategoriaProdutoProvider = Substitute.For<ICategoriaProdutoProvider>(),
            RecursosProxyService = Substitute.For<IRecursosProxyService>(),
            ClientesRepository = ServiceProvider.GetService<IRepository<Cliente>>(),
            ProdutosRepository = ServiceProvider.GetService<IRepository<Produto>>(),
            LocalProvider = Substitute.For<ILocalProvider>(),
            OrdemProducaoProvider = Substitute.For<IOrdemProducaoProvider>(),
            LegacyParametrosProvider = Substitute.For<ILegacyParametrosProvider>()
        };
        mocker.CurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        mocker.CurrentCompany.LegacyId = TestUtils.ObjectMother.Ints[0];
        return mocker;
    }

    protected OrdemRetrabalhoAclService GetService(Mocker mocker)
    {
        var service = new OrdemRetrabalhoAclService(mocker.CurrentCompany,
            mocker.CategoriaProdutoProvider, mocker.OrdemProducaoProvider,
            mocker.RecursosProxyService, mocker.ClientesRepository, mocker.ProdutosRepository,
            mocker.LocalProvider, mocker.LegacyParametrosProvider);

        return service;
    }
}