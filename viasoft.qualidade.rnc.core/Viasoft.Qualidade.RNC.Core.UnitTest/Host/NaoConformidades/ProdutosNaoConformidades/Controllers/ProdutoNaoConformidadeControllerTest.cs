using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ProdutosNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ProdutosNaoConformidades.Controllers;

public class ProdutoNaoConformidadeControllerTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get ProdutoNaoConformidade com sucesso")]
    public async Task GetProdutoNaoConformidadeControllerWithSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<IProdutoNaoConformidadeViewService>();
        var fakeService = Substitute.For<IProdutoNaoConformidadeService>();
        var produtoNaoConformidadeInput =
            new ProdutoNaoConformidadeOutput(TestUtils.ObjectMother.GetProdutoNaoConformidade(0));

        var expectedResult = new ProdutoNaoConformidadeOutput(TestUtils.ObjectMother.GetProdutoNaoConformidade(0));

        fakeService.Get(produtoNaoConformidadeInput.IdNaoConformidade, produtoNaoConformidadeInput.Id)
            .Returns(produtoNaoConformidadeInput);

        var controller = new ProdutoNaoConformidadeController(fakeService, fakeViewService);

        //Act
        var output = await controller
            .Get(produtoNaoConformidadeInput.IdNaoConformidade, produtoNaoConformidadeInput.Id);

        //Assert
        var result = output as OkObjectResult;

        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = "Get ProdutoSolucao sem sucesso")]
    public async Task GetProdutoSolucaoControllerWithoutSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<IProdutoNaoConformidadeViewService>();
        var fakeService = Substitute.For<IProdutoNaoConformidadeService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var id = TestUtils.ObjectMother.Guids[1];

        var controller = new ProdutoNaoConformidadeController(fakeService, fakeViewService);

        //Act
        var output = await controller.Get(idNaoConformidade, id);

        //Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName = "GetViewList Controller")]
    public async Task GetListProdutoNaoConformidadeController()
    {
        // Arrange
        var fakeViewService = Substitute.For<IProdutoNaoConformidadeViewService>();
        var fakeService = Substitute.For<IProdutoNaoConformidadeService>();
        var viewOutput = new ProdutoNaoConformidadeViewOutput(TestUtils.ObjectMother.GetProdutoNaoConformidade(0),
            TestUtils.ObjectMother.GetProduto(0), TestUtils.ObjectMother.GetUnidadeMedidaProduto(0));

        var produtosNaoConformidadesViewOutput = new PagedResultDto<ProdutoNaoConformidadeViewOutput>
        {
            Items = new List<ProdutoNaoConformidadeViewOutput> { viewOutput },
            TotalCount = 1
        };

        var expectedResult = new PagedResultDto<ProdutoNaoConformidadeViewOutput>
        {
            Items = new List<ProdutoNaoConformidadeViewOutput> { viewOutput },
            TotalCount = 1
        };

        var input = new PagedFilteredAndSortedRequestInput();

        fakeViewService.GetListView(viewOutput.IdNaoConformidade, input).Returns(produtosNaoConformidadesViewOutput);

        var controller = new ProdutoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output =
            await controller.GetListView(viewOutput.IdNaoConformidade, input);

        // Assert
        var result = output as OkObjectResult;
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = "Create Controller with Success")]
    public async Task CreateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IProdutoNaoConformidadeService>();
        var fakeViewService = Substitute.For<IProdutoNaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var produtoSolucaoInput = new ProdutoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
        };

        await fakeService.Insert(idNaoConformidade, produtoSolucaoInput);

        var controller = new ProdutoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Insert(produtoSolucaoInput.IdNaoConformidade, produtoSolucaoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Update Controller with Success")]
    public async Task UpdateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IProdutoNaoConformidadeService>();
        var fakeViewService = Substitute.For<IProdutoNaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var produtoNaoConformidadeInput = new ProdutoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0],
            IdProduto = TestUtils.ObjectMother.Guids[0]
        };
        await fakeService.Update(idNaoConformidade, produtoNaoConformidadeInput.Id, produtoNaoConformidadeInput);

        var controller = new ProdutoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Update(idNaoConformidade, produtoNaoConformidadeInput.Id,
            produtoNaoConformidadeInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Delete Controller with Success")]
    public async Task DeleteControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IProdutoNaoConformidadeService>();
        var fakeViewService = Substitute.For<IProdutoNaoConformidadeViewService>();
        var idProdutoNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];

        await fakeService.Remove(idNaoConformidade, idProdutoNaoConformidade);

        var controller = new ProdutoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Remove(idNaoConformidade, idProdutoNaoConformidade);

        // Assert
        var result = output as OkResult;
        result!.StatusCode.Should().Be(200);
    }
}