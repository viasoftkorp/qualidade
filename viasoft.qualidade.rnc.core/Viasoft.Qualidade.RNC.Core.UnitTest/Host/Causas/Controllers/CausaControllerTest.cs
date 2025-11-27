using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Services;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;
using OkResult = Microsoft.AspNetCore.Mvc.OkResult;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Causas.Controllers;

public class CausaControllerTest: TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get Controller with Success")]
    public async Task GetControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ICausaService>();
        var getOutput = new CausaOutput(TestUtils.ObjectMother.GetCausa(0));
        
        fakeService.Get(getOutput.Id).Returns(getOutput);
        
        var controller = new CausaController(fakeService);

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
        var fakeService = Substitute.For<ICausaService>();
        var id = TestUtils.ObjectMother.Guids[1];

        var controller = new CausaController(fakeService);

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
        var fakeService = Substitute.For<ICausaService>();
        var causaOutput = new CausaOutput(TestUtils.ObjectMother.GetCausa(0));
        
        var getOutput = new PagedResultDto<CausaOutput>
        {
            Items = new List<CausaOutput> { causaOutput },
            TotalCount = 1
        };

        var input = new PagedFilteredAndSortedRequestInput { };
        
        fakeService.GetList(input).Returns(getOutput);
        
        var controller = new CausaController(fakeService);

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
        var fakeService = Substitute.For<ICausaService>();
        var causaInput = new CausaInput(TestUtils.ObjectMother.GetCausa(0));
        
        fakeService.Create(causaInput).Returns(ValidationResult.Ok);
        
        var controller = new CausaController(fakeService);

        // Act
        var output = await controller.Create(causaInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
        
    }
    
   
    [Fact(DisplayName = "Update Controller with Success")]
    public async Task UpdateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ICausaService>();
        var causaInput = new CausaInput(TestUtils.ObjectMother.GetCausa(0));
        
        fakeService.Update(causaInput.Id, causaInput).Returns(ValidationResult.Ok);
        
        var controller = new CausaController(fakeService);

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
        var fakeService = Substitute.For<ICausaService>();
        var id = TestUtils.ObjectMother.Guids[1];
        var causa = new CausaInput {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Codigo = 10,
            Detalhamento = TestUtils.ObjectMother.Strings[1]
        };
        fakeService.Update(id, causa).Returns(ValidationResult.NotFound);
        
        var controller = new CausaController(fakeService);

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
        var fakeService = Substitute.For<ICausaService>();
        var id = TestUtils.ObjectMother.Guids[0];
        
        fakeService.Delete(id).Returns(ValidationResult.Ok);
        
        var controller = new CausaController(fakeService);

        // Act
        var output = await controller.Delete(id);

        // Assert
        var result = output as OkObjectResult;
        result!.StatusCode.Should().Be(200);
    }
    [Fact(DisplayName = "Delete Controller  returns NotFound")]
    public async Task DeleteControllerReturnsUnprocessableEntityTest()
    {
        // Arrange
        var fakeService = Substitute.For<ICausaService>();
        var id = TestUtils.ObjectMother.Guids[0];
        
        fakeService.Delete(id).Returns(ValidationResult.EntidadeEmUso);
        
        var controller = new CausaController(fakeService);

        // Act
        var output = await controller.Delete(id);

        // Assert
        var result = output as UnprocessableEntityObjectResult;
        result!.StatusCode.Should().Be(422);
    }
}