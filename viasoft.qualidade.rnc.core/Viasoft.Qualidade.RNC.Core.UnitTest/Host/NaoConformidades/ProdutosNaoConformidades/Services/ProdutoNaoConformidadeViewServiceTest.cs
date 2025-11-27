using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.UnidadeMedidaProdutos;
using Viasoft.Qualidade.RNC.Core.Domain.ProdutoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ProdutosNaoConformidades.Services;

public class ProdutoNaoConformidadeViewServiceTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "GetList ProdutoSolucao with Success")]
    public async Task GetListProdutoSolucaoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var produtoNaoConformidade = TestUtils.ObjectMother.GetProdutoNaoConformidade(0);
        var produto = TestUtils.ObjectMother.GetProduto(0);
        var unidadeMedida = TestUtils.ObjectMother.GetUnidadeMedidaProduto(0);
        await mocker.ProdutoNaoConformidade.InsertAsync(produtoNaoConformidade);
        await mocker.Produto.InsertAsync(produto);
        await mocker.UnidadeMedida.InsertAsync(unidadeMedida);

        await UnitOfWork.SaveChangesAsync();

        var input = new PagedFilteredAndSortedRequestInput
        {
            MaxResultCount = 1,
            SkipCount = 0
        };
        var expected = new List<ProdutoNaoConformidadeViewOutput>{new(produtoNaoConformidade, produto, unidadeMedida)};

        //Act
        var output = await service
            .GetListView(TestUtils.ObjectMother.Guids[0], input);

        //Assert
        output.TotalCount.Should().Be(1);
        output.Items.Should().BeEquivalentTo(expected);
    }
    private ProdutoNaoConformidadeServiceMocker GetMocker()
    {
        var mocker = new ProdutoNaoConformidadeServiceMocker()
        {
            ProdutoNaoConformidade = ServiceProvider.GetService<IRepository<ProdutoNaoConformidade>>(),
            Produto = ServiceProvider.GetService<IRepository<Produto>>(),
            UnidadeMedida = ServiceProvider.GetService<IRepository<UnidadeMedidaProduto>>(),
            FakeCurrentCompany = Substitute.For<ICurrentCompany>()
        };
        mocker.FakeCurrentCompany.Id = TestUtils.ObjectMother.Guids[0];
        return mocker;
    }

    private ProdutoNaoConformidadeViewService GetService(ProdutoNaoConformidadeServiceMocker mocker)
    {
        this.ConfigureServices();
        var service = new ProdutoNaoConformidadeViewService(mocker.ProdutoNaoConformidade, mocker.Produto, mocker.UnidadeMedida,
            mocker.FakeCurrentCompany);
        return service;
    }

    public class ProdutoNaoConformidadeServiceMocker
    {
        public IRepository<ProdutoNaoConformidade> ProdutoNaoConformidade { get; set; }
        public IRepository<Produto> Produto { get; set; }
        public IRepository<UnidadeMedidaProduto> UnidadeMedida { get; set; }
        public ICurrentCompany FakeCurrentCompany { get; set; }
    }
}