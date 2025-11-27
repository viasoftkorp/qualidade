using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.DefeitosNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.DefeitosNaoConformidades.Controllers;

public class DefeitoNaoConformidadeControllerTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "GetView Defeito com sucesso")]
    public async Task GetDefeitoControllerWithSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<IDefeitoNaoConformidadeViewService>();
        var fakeService = Substitute.For<IDefeitoNaoConformidadeService>();
        var viewOutput = new DefeitoNaoConformidadeOutput(TestUtils.ObjectMother.GetDefeitoNaoConformidade(0));

        fakeService.Get(viewOutput.IdNaoConformidade,viewOutput.Id).Returns(viewOutput);

        var controller = new DefeitoNaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.GetView(viewOutput.IdNaoConformidade,viewOutput.Id);
        
        //Assert
        var result = new OkObjectResult(output);

        result.Value.Should().BeEquivalentTo(output);
        result.StatusCode.Should().Be(200);
    }
    
    [Fact(DisplayName = "GetView Defeito sem sucesso")]
    public async Task GetDefeitoControllerWithoutSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<IDefeitoNaoConformidadeViewService>();
        var fakeService = Substitute.For<IDefeitoNaoConformidadeService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var id = TestUtils.ObjectMother.Guids[1];

        var controller = new DefeitoNaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.GetView(idNaoConformidade,id);
        
        //Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }
    
    [Fact(DisplayName = "GetViewList Controller")]
    public async Task GetListDefeitoController()
    {
        // Arrange
        var fakeViewService = Substitute.For<IDefeitoNaoConformidadeViewService>();
        var fakeService = Substitute.For<IDefeitoNaoConformidadeService>();
        var viewOutput = new DefeitoNaoConformidadeViewOutput(
            TestUtils.ObjectMother.GetDefeitoNaoConformidade(0), 
            TestUtils.ObjectMother.GetDefeito(0));
        
        var getOutput = new PagedResultDto<DefeitoNaoConformidadeViewOutput>
        {
            Items = new List<DefeitoNaoConformidadeViewOutput> {viewOutput},
            TotalCount = 1
        };
    
        var input = new PagedFilteredAndSortedRequestInput { };
    
        fakeViewService.GetListView(viewOutput.IdNaoConformidade, input).Returns(getOutput);
    
        var controller = new DefeitoNaoConformidadeController(fakeService,fakeViewService);
    
        // Act
        var output = await controller.GetListView(viewOutput.IdNaoConformidade, input);
    
        // Assert
        var result = output as OkObjectResult;
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(getOutput);
    }
     [Fact(DisplayName = "Create Controller with Success")]
    public async Task CreateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IDefeitoNaoConformidadeService>();
        var fakeViewService = Substitute.For<IDefeitoNaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var defeitoInput = new DefeitoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdDefeito = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0]

        };

        await fakeService.Insert(idNaoConformidade, defeitoInput);

        var controller = new DefeitoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Insert(defeitoInput.IdNaoConformidade, defeitoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Update Controller with Success")]
    public async Task UpdateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IDefeitoNaoConformidadeService>();
        var fakeViewService = Substitute.For<IDefeitoNaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var defeitoInput = new DefeitoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdDefeito = TestUtils.ObjectMother.Guids[0],
            Quantidade = TestUtils.ObjectMother.Ints[0]
            
        };
        await fakeService.Update(idNaoConformidade, defeitoInput.Id, defeitoInput);

        var controller = new DefeitoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Update(idNaoConformidade, defeitoInput.Id, defeitoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Delete Controller with Success")]
    public async Task DeleteControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IDefeitoNaoConformidadeService>();
        var fakeViewService = Substitute.For<IDefeitoNaoConformidadeViewService>();
        var idDefeito = TestUtils.ObjectMother.Guids[0];
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];

        await fakeService.Remove(idNaoConformidade, idDefeito);

        var controller = new DefeitoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Remove(idNaoConformidade, idDefeito);

        // Assert
        var result = output as OkResult;
        result!.StatusCode.Should().Be(200);
    }
}