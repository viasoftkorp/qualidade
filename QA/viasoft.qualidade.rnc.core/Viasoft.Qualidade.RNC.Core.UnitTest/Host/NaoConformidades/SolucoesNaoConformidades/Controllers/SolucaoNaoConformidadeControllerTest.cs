using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.SolucoesNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.SolucoesNaoConformidades.Controllers;

public class SolucaoNaoConformidadeControllerTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "Get Solucao com sucesso")]
    public async Task GetSolucaoControllerWithSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<ISolucaoNaoConformidadeViewService>();
        var fakeService = Substitute.For<ISolucaoNaoConformidadeService>();
        var solucaoOutput = new SolucaoNaoConformidadeOutput(TestUtils.ObjectMother.GetSolucaoNaoConformidade(0));

        fakeService.Get(solucaoOutput.IdNaoConformidade, solucaoOutput.Id).Returns(solucaoOutput);

        var controller = new SolucaoNaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.Get(solucaoOutput.IdNaoConformidade,solucaoOutput.Id);
        
        //Assert
        var result = new OkObjectResult(output);
        
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(output);
    }
    
    [Fact(DisplayName = "Get Solucao sem sucesso")]
    public async Task GetSolucaoControllerWithoutSuccessTest()
    {
        //Arrange
        var fakeViewService = Substitute.For<ISolucaoNaoConformidadeViewService>();
        var fakeService = Substitute.For<ISolucaoNaoConformidadeService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var id = TestUtils.ObjectMother.Guids[1];

        var controller = new SolucaoNaoConformidadeController(fakeService,fakeViewService);

        //Act
        var output = await controller.Get(idNaoConformidade,id);
        
        //Assert
        var result = output as NotFoundResult;
        result!.StatusCode.Should().Be(404);
    }
    
    [Fact(DisplayName = "GetViewList Controller")]
    public async Task GetListSolucaoController()
    {
        // Arrange
        var fakeViewService = Substitute.For<ISolucaoNaoConformidadeViewService>();
        var fakeService = Substitute.For<ISolucaoNaoConformidadeService>();
        var viewOutput = new SolucaoNaoConformidadeViewOutput(
            TestUtils.ObjectMother.GetSolucaoNaoConformidade(0), 
            TestUtils.ObjectMother.GetSolucao(0), 
            TestUtils.ObjectMother.GetUsuario(0));
        
        var getOutput = new PagedResultDto<SolucaoNaoConformidadeViewOutput>
        {
            Items = new List<SolucaoNaoConformidadeViewOutput> {viewOutput},
            TotalCount = 1
        };
    
        var input = new GetListWithDefeitoIdFlagInput { };
    
        fakeViewService.GetListView(viewOutput.IdNaoConformidade, viewOutput.IdDefeitoNaoConformidade, input).Returns(getOutput);
    
        var controller = new SolucaoNaoConformidadeController(fakeService,fakeViewService);
    
        // Act
        var output = await controller.GetListView(viewOutput.IdNaoConformidade, input, viewOutput.IdDefeitoNaoConformidade);
    
        // Assert
        var result = output as OkObjectResult;
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(getOutput);
    }
     [Fact(DisplayName = "Create Controller with Success")]
    public async Task CreateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoNaoConformidadeService>();
        var fakeViewService = Substitute.For<ISolucaoNaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var solucaoInput = new SolucaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
            SolucaoImediata = false,
            DataAnalise = default,
            DataPrevistaImplantacao = default,
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            CustoEstimado = 0,
            NovaData = default,
            DataVerificacao = default,
            IdAuditor = TestUtils.ObjectMother.Guids[0],
        };

        await fakeService.Insert(idNaoConformidade, solucaoInput);

        var controller = new SolucaoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Insert(solucaoInput.IdNaoConformidade, solucaoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Update Controller with Success")]
    public async Task UpdateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoNaoConformidadeService>();
        var fakeViewService = Substitute.For<ISolucaoNaoConformidadeViewService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var solucaoInput = new SolucaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdDefeitoNaoConformidade = TestUtils.ObjectMother.Guids[0],
            SolucaoImediata = false,
            DataAnalise = default,
            DataPrevistaImplantacao = default,
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            CustoEstimado = 0,
            NovaData = default,
            DataVerificacao = default,
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            
        };
        await fakeService.Update(idNaoConformidade, solucaoInput.Id, solucaoInput);

        var controller = new SolucaoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Update(idNaoConformidade, solucaoInput.Id, solucaoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Delete Controller with Success")]
    public async Task DeleteControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<ISolucaoNaoConformidadeService>();
        var fakeViewService = Substitute.For<ISolucaoNaoConformidadeViewService>();
        var idSolucao = TestUtils.ObjectMother.Guids[0];
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];

        await fakeService.Remove(idNaoConformidade, idSolucao);

        var controller = new SolucaoNaoConformidadeController(fakeService, fakeViewService);

        // Act
        var output = await controller.Remove(idNaoConformidade, idSolucao);

        // Assert
        var result = output as OkResult;
        result!.StatusCode.Should().Be(200);
    }
}