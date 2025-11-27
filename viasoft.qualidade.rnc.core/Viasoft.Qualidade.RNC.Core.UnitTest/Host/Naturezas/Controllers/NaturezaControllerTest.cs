using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Naturezas.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.Naturezas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Naturezas.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Naturezas.Controllers;

public class NaturezaControllerTest: TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get Natureza Controller with Success")]
    public async Task GetNaturezaControllerWithSuccessTest()
    {
        //Arrange
        var fakeService = Substitute.For<INaturezaService>();
        var naturezaOutput = new NaturezaOutput(TestUtils.ObjectMother.GetNatureza(0));

        fakeService.Get(naturezaOutput.Id).Returns(naturezaOutput);

        var controller = new NaturezaController(fakeService);

        //Act
        var output = await controller.Get(naturezaOutput.Id);
        
        //Assert
        var result = output as OkObjectResult;

        result.Value.Should().BeEquivalentTo(naturezaOutput);
        result.StatusCode.Should().Be(200);

    }

    
    [Fact(DisplayName = "Get Natureza Controller with NotFound Id")]
    public async Task GetNaturezaControllerWithNotFoundIdTest()
    {
        //arrange
        var fakeservice = Substitute.For<INaturezaService>();

        var naturezaController = new NaturezaController(fakeservice);
        
        //Act
        var output = await naturezaController.Get(TestUtils.ObjectMother.Guids[0]);

        //Assert
        var result = output as NotFoundResult;
        result.StatusCode.Should().Be(404);
    }


    [Fact(DisplayName = "GetList Natureza Controller with Success")]
    public async Task GetlistNaturezaControllerWithSuccessTest()
    {
        //Arrange
        var fakeService = Substitute.For<INaturezaService>();
        
        var input = new PagedFilteredAndSortedRequestInput { };
        var getOutput = new PagedResultDto<NaturezaOutput>
        {
            TotalCount = 1,
            Items = new List<NaturezaOutput>(){ new NaturezaOutput(TestUtils.ObjectMother.GetNatureza(0)) }
        };

        fakeService.GetList(input).Returns(getOutput);

        var controller = new NaturezaController(fakeService);
        
        //Act
        var output = await controller.GetList(input);
        
        //Assert
        var result = output as OkObjectResult;

        result.Value.Should().BeEquivalentTo(getOutput);
        result.StatusCode.Should().Be(200);
    }

    
    [Fact(DisplayName = "Create Natureza Controller with Success")]
    public async Task CreateNaturezaContollerWithSuccessTest()
    {
        //Arrange
        var fakeService = Substitute.For<INaturezaService>();
        var naturezaInput = TestUtils.ObjectMother.GetNaturezaInput(0);

        fakeService.Create(naturezaInput).Returns(ValidationResult.Ok);

        var controller = new NaturezaController(fakeService);
        
        //Act
        var output = await controller.Create(naturezaInput);
        
        //Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);


    }

    
    [Fact(DisplayName = "Update Naturza Controller with Success")]
    public async Task UpdateNaturezaControllerWithSuccess()
    {
        //Arrange
        var fakeService = Substitute.For<INaturezaService>();
        var input = TestUtils.ObjectMother.GetNaturezaInput(0);

        fakeService.Update(input.Id, input).Returns(ValidationResult.Ok);

        var controller = new NaturezaController(fakeService);
        
        //Act
        var output = await controller.Update(input.Id, input);
        
        //Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    
    [Fact(DisplayName = "Update Natureza Controller Returns NotFound")]
    public async Task UpdateNAturezaControllerReturnsNotFound()
    {
        //Arrange
        var fakeService = Substitute.For<INaturezaService>();
        var natureza = TestUtils.ObjectMother.GetNaturezaInput(1);

        fakeService.Update(natureza.Id, natureza).Returns(ValidationResult.NotFound);

        var controller = new NaturezaController(fakeService);
        
        //Act
        var output = await controller.Update(natureza.Id, natureza);
        
        //Assert
        var result = output as NotFoundResult;
        result.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName = "Delete Natureza Controller with Success")]
    public async Task DeleteNaturezaControllerWithSuccess()
    {
        //Arrange 
        var fakeService = Substitute.For<INaturezaService>();

        fakeService.Delete(TestUtils.ObjectMother.Guids[0]).Returns(ValidationResult.Ok);

        var controller = new NaturezaController(fakeService);
        
        //Act
        var output = await controller.Delete(TestUtils.ObjectMother.Guids[0]);
        
        //Assert
        var result = output as OkObjectResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Delete Natureza Controller Returns NotFound")]
    public async Task DeleteNaturezaControllerReturnsNotfound()
    {
        //Arrange
        var fakeService = Substitute.For<INaturezaService>();

        fakeService.Delete(TestUtils.ObjectMother.Guids[1]).Returns(ValidationResult.NotFound);

        var controller = new NaturezaController(fakeService);
        
        //act
        var output = await controller.Delete(TestUtils.ObjectMother.Guids[1]);
        
        //Assert
        var result = output as NotFoundObjectResult;
        result.StatusCode.Should().Be(404);
    }
    [Fact(DisplayName = "Delete Natureza Controller Returns UnprocessableEntity")]
    public async Task DeleteNaturezaControllerReturnsUnprocessableEntity()
    {
        //Arrange
        var fakeService = Substitute.For<INaturezaService>();

        fakeService.Delete(TestUtils.ObjectMother.Guids[1]).Returns(ValidationResult.EntidadeEmUso);

        var controller = new NaturezaController(fakeService);
        
        //act
        var output = await controller.Delete(TestUtils.ObjectMother.Guids[1]);
        
        //Assert
        var result = output as UnprocessableEntityObjectResult;
        result.StatusCode.Should().Be(422);
    }
    
    [Fact(DisplayName = "Se endpoint chamado, deve chamar changeStatus passando \"true\" ")]
    public async Task AtivarNaturezaTest()
    {
        //Arrange
        var fakeService = Substitute.For<INaturezaService>();

        fakeService.ChangeStatus(TestUtils.ObjectMother.Guids[0], true).Returns(ValidationResult.Ok);

        var controller = new NaturezaController(fakeService);
        
        //act
        var output = await controller.Ativar(TestUtils.ObjectMother.Guids[0]);
        
        //Assert
        await fakeService.Received(1).ChangeStatus(TestUtils.ObjectMother.Guids[0], true);
        var result = output as OkObjectResult;
        result.StatusCode.Should().Be(200);
    }
    [Fact(DisplayName = "Se endpoint chamado, deve chamar changeStatus passando \"false\" ")]
    public async Task InativarNaturezaTest()
    {
        //Arrange
        var fakeService = Substitute.For<INaturezaService>();

        fakeService.ChangeStatus(TestUtils.ObjectMother.Guids[0], false).Returns(ValidationResult.Ok);

        var controller = new NaturezaController(fakeService);
        
        //act
        var output = await controller.Inativar(TestUtils.ObjectMother.Guids[0]);
        
        //Assert
        await fakeService.Received(1).ChangeStatus(TestUtils.ObjectMother.Guids[0], false);
        var result = output as OkObjectResult;
        result.StatusCode.Should().Be(200);
    }
}