using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Defeitos.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.Defeitos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Defeitos.Services;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Defeitos.Controllers;

public class DefeitoControllerTest : TestUtils.UnitTestBaseWithDbContext

{
    [Fact(DisplayName = "Get Controller with Success")]
    public async Task GetControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IDefeitoService>();
        var getOutput = new DefeitoOutput(TestUtils.ObjectMother.GetDefeito(0));

        fakeService.Get(getOutput.Id).Returns(getOutput);

        var controller = new DefeitoController(fakeService);

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
        var fakeService = Substitute.For<IDefeitoService>();
        var id = TestUtils.ObjectMother.Guids[1];

        var controller = new DefeitoController(fakeService);

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
        var fakeService = Substitute.For<IDefeitoService>();
        var getOutput = new PagedResultDto<DefeitoViewOutput>
        {
            Items = new List<DefeitoViewOutput> { new DefeitoViewOutput() },
            TotalCount = 1
        };

        var input = new PagedFilteredAndSortedRequestInput() { };

        fakeService.GetViewList(input).Returns(getOutput);

        var controller = new DefeitoController(fakeService);

        // Act
        var output = await controller.GetViewList(input);

        // Assert
        var result = output as OkObjectResult;
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(getOutput);
    }


    [Fact(DisplayName = "Create Controller with Success")]
    public async Task CreateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IDefeitoService>();
        var defeitoInput = TestUtils.ObjectMother.GetDefeitoInput(0);

        fakeService.Create(defeitoInput).Returns(ValidationResult.Ok);

        var controller = new DefeitoController(fakeService);

        // Act
        var output = await controller.Create(defeitoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }


    [Fact(DisplayName = "Update Controller with Success")]
    public async Task UpdateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IDefeitoService>();
        var defeitoInput = TestUtils.ObjectMother.GetDefeitoInput(0);
        
        fakeService.Update(defeitoInput.Id, defeitoInput).Returns(ValidationResult.Ok);

        var controller = new DefeitoController(fakeService);

        // Act
        var output = await controller.Update(defeitoInput.Id, defeitoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }


    [Fact(DisplayName = "Update Controller returns NotFound")]
    public async Task UpdateControllerWithNotFound()
    {
        // Arrange
        var fakeService = Substitute.For<IDefeitoService>();
        var id = TestUtils.ObjectMother.Guids[1];
        var defeito = new DefeitoInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Codigo = 10,
            Detalhamento = TestUtils.ObjectMother.Strings[1]
        };
        fakeService.Update(id, defeito).Returns(ValidationResult.NotFound);

        var controller = new DefeitoController(fakeService);

        // Act
        var output = await controller.Update(id, defeito);

        // Assert
        var result = output as NotFoundResult;
        result.StatusCode.Should().Be(404);
    }


    [Fact(DisplayName = "Delete Controller with Success")]
    public async Task DeleteControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IDefeitoService>();
        var id = TestUtils.ObjectMother.Guids[0];

        fakeService.Delete(id).Returns(ValidationResult.Ok);

        var controller = new DefeitoController(fakeService);

        // Act
        var output = await controller.Delete(id);

        // Assert
        var result = output.Result as OkObjectResult;
        result!.StatusCode.Should().Be(200);
    }
}