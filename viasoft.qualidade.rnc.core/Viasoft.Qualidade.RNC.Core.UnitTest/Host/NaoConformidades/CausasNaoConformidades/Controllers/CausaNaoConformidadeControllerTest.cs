using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.CausasNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.CausasNaoConformidades.Controllers;

public class CausaNaoConformidadeControllerTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get Causa com sucesso")]
    public async Task GetCausaControllerWithSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<ICausaNaoConformidadeViewService>();
        var fakeService = Substitute.For<ICausaNaoConformidadeService>();
        var causaOutput = new CausaNaoConformidadeOutput(TestUtils.ObjectMother.GetCausaNaoConformidade(0));

        var controller = new CausaNaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.Get(causaOutput.IdNaoConformidade,causaOutput.Id);
        
        //Assert
        var result = new OkObjectResult(output);
        
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(output);
    }
    
    [Fact(DisplayName = "Get Causa sem sucesso")]
    public async Task GetCausaControllerWithoutSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<ICausaNaoConformidadeViewService>();
        var fakeService = Substitute.For<ICausaNaoConformidadeService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var id = TestUtils.ObjectMother.Guids[1];

        var controller = new CausaNaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.Get(idNaoConformidade,id);
        
        //Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }
    
    [Fact(DisplayName = "GetViewList Controller")]
    public async Task GetListCausaController()
    {
        // Arrange
        var fakeViewService = Substitute.For<ICausaNaoConformidadeViewService>();
        var fakeService = Substitute.For<ICausaNaoConformidadeService>();
        var viewOutput = new CausaNaoConformidadeViewOutput(
            TestUtils.ObjectMother.GetCausaNaoConformidade(0), 
            TestUtils.ObjectMother.GetCausa(0));
        
        var getOutput = new PagedResultDto<CausaNaoConformidadeViewOutput>
        {
            Items = new List<CausaNaoConformidadeViewOutput> {viewOutput},
            TotalCount = 1
        };
    
        var PagedFilteredAndSortedRequestInput = new GetListWithDefeitoIdFlagInput { };

        var controller = new CausaNaoConformidadeController(fakeService,fakeViewService);
    
        // Act
        var output = await controller.GetListView(viewOutput.IdNaoConformidade, viewOutput.IdDefeitoNaoConformidade, PagedFilteredAndSortedRequestInput);
    
        // Assert
        var result = output as OkObjectResult;
        result!.StatusCode.Should().Be(200);
    }
     [Fact(DisplayName = "Create Controller with Success")]
    public async Task CreateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ICausaNaoConformidadeService>();
        var fakeViewService = Substitute.For<ICausaNaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var causaInput = new CausaNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdCausa = TestUtils.ObjectMother.Guids[0],

        };

        var controller = new CausaNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Insert(causaInput.IdNaoConformidade, causaInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Update Controller with Success")]
    public async Task UpdateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ICausaNaoConformidadeService>();
        var fakeViewService = Substitute.For<ICausaNaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var causaInput = new CausaNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdCausa = TestUtils.ObjectMother.Guids[0]
            
        };

        var controller = new CausaNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Update(idNaoConformidade, causaInput.Id, causaInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Delete Controller with Success")]
    public async Task DeleteControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ICausaNaoConformidadeService>();
        var fakeViewService = Substitute.For<ICausaNaoConformidadeViewService>();
        var idCausa = TestUtils.ObjectMother.Guids[0];
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];

        await fakeService.Remove(idNaoConformidade, idCausa);

        var controller = new CausaNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Remove(idNaoConformidade, idCausa);

        // Assert
        var result = output as OkResult;
        result!.StatusCode.Should().Be(200);
    }
}