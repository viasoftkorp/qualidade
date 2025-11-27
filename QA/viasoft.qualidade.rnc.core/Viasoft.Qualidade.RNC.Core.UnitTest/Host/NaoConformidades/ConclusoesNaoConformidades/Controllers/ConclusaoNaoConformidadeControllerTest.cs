using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ConclusoesNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ConclusoesNaoConformidades.Controllers;

public class ConclusaoNaoConformidadeControllerTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "ConcluirNaoConformidade Controller with Success")]
    public async Task ConcluirNaoConformidadeControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IConclusaoNaoConformidadeService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var conclusaoInput = new ConclusaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            NovaReuniao = false,
            DataReuniao = TestUtils.ObjectMother.Datas[0],
            DataVerificacao = TestUtils.ObjectMother.Datas[0],
            IdAuditor = TestUtils.ObjectMother.Guids[0],
            Eficaz = false,
            CicloDeTempo = TestUtils.ObjectMother.Ints[0],
            IdNovoRelatorio = TestUtils.ObjectMother.Guids[0],
        };
        await fakeService.ConcluirNaoConformidade(idNaoConformidade, conclusaoInput);

        var controller = new ConclusaoNaoConformidadeController(fakeService);

        // Act
        var output = await controller.ConcluirNaoConformidade(idNaoConformidade, conclusaoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }
    [Fact(DisplayName = "GetView Conclusao com sucesso")]
    public async Task GetConclusaoControllerWithSuccessTest()
    {
        //Arrange
        var fakeService = Substitute.For<IConclusaoNaoConformidadeService>();
        var viewOutput = new ConclusaoNaoConformidadeOutput(TestUtils.ObjectMother.GetConclusaoNaoConformidade(0));

        fakeService.Get(viewOutput.IdNaoConformidade).Returns(viewOutput);

        var controller = new ConclusaoNaoConformidadeController(fakeService);

        //Act
        var output = await controller.GetView(viewOutput.IdNaoConformidade);
        
        //Assert
        var result = new OkObjectResult(output);

        result.Value.Should().BeEquivalentTo(output);
        result.StatusCode.Should().Be(200);
    }
   
}