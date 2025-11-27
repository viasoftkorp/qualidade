using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Controllers;

public class SolucaoControllerTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get Controller with Success")]
    public async Task GetControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var getOutput = new SolucaoOutput(TestUtils.ObjectMother.GetSolucao(0));

        fakeService.Get(getOutput.Id).Returns(getOutput);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.Get(getOutput.Id);

        // Assert
        var result = output as OkObjectResult;
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(getOutput);
    }

    [Fact(DisplayName = "Get Controller with NotFound Id")]
    public async Task GetControllerNotFoundIdTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[1];

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.Get(id);

        // Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName = "GetList Controller with Success")]
    public async Task GetListControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var solucaoOutput = new SolucaoOutput(TestUtils.ObjectMother.GetSolucao(0));

        var getOutput = new PagedResultDto<SolucaoOutput>
        {
            Items = new List<SolucaoOutput> {solucaoOutput},
            TotalCount = 1
        };

        var input = new PagedFilteredAndSortedRequestInput { };

        fakeService.GetList(input).Returns(getOutput);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.GetList(input);

        // Assert
        var result = output as OkObjectResult;
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(getOutput);
    }

    [Fact(DisplayName = "Create Controller with Success")]
    public async Task CreateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var solucaoInput = TestUtils.ObjectMother.GetSolucaoInput(0);

        fakeService.Create(solucaoInput).Returns(ValidationResult.Ok);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.Create(solucaoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Update Controller with Success")]
    public async Task UpdateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var causaInput = TestUtils.ObjectMother.GetSolucaoInput(0);
        fakeService.Update(causaInput.Id, causaInput).Returns(ValidationResult.Ok);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.Update(causaInput.Id, causaInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Update Controller returns NotFound")]
    public async Task UpdateControllerWithNotFound()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[1];
        var causa = new SolucaoInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Codigo = 10,
            Detalhamento = TestUtils.ObjectMother.Strings[1]
        };
        fakeService.Update(id, causa).Returns(ValidationResult.NotFound);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.Update(id, causa);

        // Assert
        var result = output as NotFoundResult;
        result.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName = "Delete Controller with Success")]
    public async Task DeleteControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[0];

        fakeService.Delete(id).Returns(ValidationResult.Ok);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.Delete(id);

        // Assert
        var result = output as OkObjectResult;
        result!.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Delete controller Returns NotFound")]
    public async Task DeleteControllerWithNotFound()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[0];

        fakeService.Delete(id).Returns(ValidationResult.NotFound);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.Delete(id);

        // Assert
        var result = output as NotFoundObjectResult;
        result!.StatusCode.Should().Be(404);
    }
    [Fact(DisplayName = "Delete controller Returns UnprocessableEntity")]
    public async Task DeleteControllerWithUnprocessableEntity()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[0];

        fakeService.Delete(id).Returns(ValidationResult.EntidadeEmUso);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.Delete(id);

        // Assert
        var result = output as UnprocessableEntityObjectResult;
        result!.StatusCode.Should().Be(422);
    }

    [Fact(DisplayName = "GetProdutoSolucaoView Controller with NotFound Id")]
    public async Task GetProdutoSolucaoViewControllerNotFoundIdTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[7];

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.GetProdutoSolucaoView(id);

        // Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName = "AddProduto Controller with Success")]
    public async Task AddProdutoControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var solucaoInput = TestUtils.ObjectMother.GetProdutoSolucaoInput(0);

        fakeService.AddProduto(solucaoInput).Returns(ValidationResult.Ok);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.AddProduto(solucaoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "UpdateProduto Controller with Success")]
    public async Task UpdateProdutoControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var solucaoInput = TestUtils.ObjectMother.GetProdutoSolucaoInput(0);
        fakeService.UpdateProduto(solucaoInput.Id, solucaoInput).Returns(ValidationResult.Ok);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.UpdateProduto(solucaoInput.Id, solucaoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "UpdateProduto Controller returns NotFound")]
    public async Task UpdateProdutoControllerWithNotFound()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[1];
        var produtoSolucao = new ProdutoSolucaoInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Quantidade = 4,
            IdProduto = TestUtils.ObjectMother.Guids[3],
            IdSolucao = TestUtils.ObjectMother.Guids[2],
        };
        fakeService.UpdateProduto(id, produtoSolucao).Returns(ValidationResult.NotFound);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.UpdateProduto(id, produtoSolucao);

        // Assert
        var result = output as NotFoundResult;
        result.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName = "DeleteProduto Controller with Success")]
    public async Task DeleteProdutoControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[0];

        fakeService.DeleteProduto(id).Returns(ValidationResult.Ok);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.DeleteProduto(id);

        // Assert
        var result = output as OkResult;
        result!.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "DeleteProduto controller Returns NotFound")]
    public async Task DeleteProdutoControllerWithNotFound()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[0];

        fakeService.DeleteProduto(id).Returns(ValidationResult.NotFound);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.DeleteProduto(id);

        // Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName = "GetServicoSolucaoView Controller with NotFound Id")]
    public async Task GetServicoSolucaoViewControllerNotFoundIdTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[7];

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.GetServicoSolucaoView(id);

        // Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }
    
    [Fact(DisplayName = "AddServico Controller with Success")]
    public async Task AddServicoControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var solucaoInput = TestUtils.ObjectMother.GetServicoSolucaoInput(0);;

        fakeService.AddServico(solucaoInput).Returns(ServicoValidationResult.Ok);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.AddServico(solucaoInput);

        // Assert
        var result = output.Result as OkObjectResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "UpdateServico Controller with Success")]
    public async Task UpdateServicoControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var solucaoInput = TestUtils.ObjectMother.GetServicoSolucaoInput(0);;
        fakeService.UpdateServico(solucaoInput.Id, solucaoInput).Returns(ServicoValidationResult.Ok);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.UpdateServico(solucaoInput.Id, solucaoInput);

        // Assert
        var result = output.Result as OkObjectResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "UpdateServico Controller returns BadRequest")]
    public async Task UpdateServicoControllerWithNotFound()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[1];
        var produtoSolucao = new ServicoSolucaoInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Quantidade = 4,
            IdProduto = TestUtils.ObjectMother.Guids[3],
            IdSolucao = TestUtils.ObjectMother.Guids[2],
            Horas = TestUtils.ObjectMother.Ints[1],
            Minutos = TestUtils.ObjectMother.Ints[2],
            IdRecurso = TestUtils.ObjectMother.Guids[4],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[5]
        };
        fakeService.UpdateServico(id, produtoSolucao).Returns(ServicoValidationResult.NotFound);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.UpdateServico(id, produtoSolucao);

        // Assert
        var result = output.Result as BadRequestObjectResult;
        result.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName = "DeleteServico Controller with Success")]
    public async Task DeleteServicoControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[0];

        fakeService.DeleteServico(id).Returns(ValidationResult.Ok);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.DeleteServico(id);

        // Assert
        var result = output as OkResult;
        result!.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "DeleteServico controller Returns NotFound")]
    public async Task DeleteServicoControllerWithNotFound()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoService>();
        var id = TestUtils.ObjectMother.Guids[0];

        fakeService.DeleteServico(id).Returns(ValidationResult.NotFound);

        var controller = new SolucaoController(fakeService);

        // Act
        var output = await controller.DeleteServico(id);

        // Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }
}