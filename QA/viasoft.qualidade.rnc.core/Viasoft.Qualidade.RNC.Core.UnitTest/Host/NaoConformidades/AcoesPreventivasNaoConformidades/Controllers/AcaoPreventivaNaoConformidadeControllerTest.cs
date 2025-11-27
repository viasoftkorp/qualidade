using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Controllers;

public class AcaoPreventivaNaoConformidadeControllerTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get AcaoPreventiva com sucesso")]
    public async Task GetAcaoPreventivaControllerWithSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<IAcaoPreventivaNaoConformidadeViewService>();
        var fakeService = Substitute.For<IAcaoPreventivaNaoConformidadeService>();
        var causaOutput = new AcaoPreventivaNaoConformidadeOutput(TestUtils.ObjectMother.GetAcaoPreventivaNaoConformidade(0));

        fakeService.Get(causaOutput.IdNaoConformidade, causaOutput.Id).Returns(causaOutput);

        var controller = new AcaoPreventivaNaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.Get(causaOutput.IdNaoConformidade,causaOutput.Id);
        
        //Assert
        var result = new OkObjectResult(output);
        
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(output);
    }
    
    [Fact(DisplayName = "Get AcaoPreventiva sem sucesso")]
    public async Task GetAcaoPreventivaControllerWithoutSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<IAcaoPreventivaNaoConformidadeViewService>();
        var fakeService = Substitute.For<IAcaoPreventivaNaoConformidadeService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var id = TestUtils.ObjectMother.Guids[1];

        var controller = new AcaoPreventivaNaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.Get(idNaoConformidade,id);
        
        //Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }
    
    [Fact(DisplayName = "GetView AcoesPreventivas sem sucesso")]
    public async Task GetAcoesPreventivasControllerWithoutSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<IAcaoPreventivaNaoConformidadeViewService>();
        var fakeService = Substitute.For<IAcaoPreventivaNaoConformidadeService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var id = TestUtils.ObjectMother.Guids[1];

        var controller = new AcaoPreventivaNaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.Get(idNaoConformidade,id);
        
        //Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }
    
    [Fact(DisplayName = "GetViewList Controller")]
    public async Task GetListAcaoPreventivaController()
    {
        // Arrange
        var fakeViewService = Substitute.For<IAcaoPreventivaNaoConformidadeViewService>();
        var fakeService = Substitute.For<IAcaoPreventivaNaoConformidadeService>();
        var viewOutput = new AcaoPreventivaNaoConformidadeViewOutput(TestUtils.ObjectMother.GetAcaoPreventivaNaoConformidade(0), 
            TestUtils.ObjectMother.GetAcaoPreventiva(0),TestUtils.ObjectMother.GetUsuario(0));
        
        var getOutput = new PagedResultDto<AcaoPreventivaNaoConformidadeViewOutput>
        {
            Items = new List<AcaoPreventivaNaoConformidadeViewOutput> {viewOutput},
            TotalCount = 1
        };
    
        var input = new GetListWithDefeitoIdFlagInput { };
    
        fakeViewService.GetListView(viewOutput.IdNaoConformidade, viewOutput.IdDefeitoNaoConformidade, input).Returns(getOutput);
    
        var controller = new AcaoPreventivaNaoConformidadeController(fakeService,fakeViewService);
    
        // Act
        var output = await controller.GetListView(viewOutput.IdNaoConformidade, viewOutput.IdDefeitoNaoConformidade, input);
    
        // Assert
        var result = output as OkObjectResult;
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(getOutput);
    }
     [Fact(DisplayName = "Create Controller with Success")]
    public async Task CreateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IAcaoPreventivaNaoConformidadeService>();
        var fakeViewService = Substitute.For<IAcaoPreventivaNaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var acaoInput = new AcaoPreventivaNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = default,
            Acao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            DataAnalise = TestUtils.ObjectMother.Datas[0],
            DataPrevistaImplantacao = TestUtils.ObjectMother.Datas[0],
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Implementada = false,
            DataVerificacao = TestUtils.ObjectMother.Datas[0],
            NovaData = TestUtils.ObjectMother.Datas[0]
        };
    
        await fakeService.Insert(idNaoConformidade, acaoInput);
    
        var controller = new AcaoPreventivaNaoConformidadeController(fakeService, fakeViewService);
    
        // Act
        var output = await controller.Insert(acaoInput.IdNaoConformidade, acaoInput);
    
        // Assert
        var result = new OkResult();
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Update Controller with Success")]
    public async Task UpdateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IAcaoPreventivaNaoConformidadeService>();
        var fakeViewService = Substitute.For<IAcaoPreventivaNaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var acaoInput = new AcaoPreventivaNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = default,
            Acao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            DataAnalise = TestUtils.ObjectMother.Datas[0],
            DataPrevistaImplantacao = TestUtils.ObjectMother.Datas[0],
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Implementada = false,
            DataVerificacao = TestUtils.ObjectMother.Datas[0],
            NovaData = TestUtils.ObjectMother.Datas[0]
        };
        await fakeService.Update(idNaoConformidade, acaoInput.Id, acaoInput);

        var controller = new AcaoPreventivaNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Update(idNaoConformidade, acaoInput.Id, acaoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Delete Controller with Success")]
    public async Task DeleteControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IAcaoPreventivaNaoConformidadeService>();
        var fakeViewService = Substitute.For<IAcaoPreventivaNaoConformidadeViewService>();
        var idAcao = TestUtils.ObjectMother.Guids[0];
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];

        await fakeService.Remove(idNaoConformidade, idAcao);

        var controller = new AcaoPreventivaNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Remove(idNaoConformidade, idAcao);

        // Assert
        var result = output as OkResult;
        result!.StatusCode.Should().Be(200);
    }
}