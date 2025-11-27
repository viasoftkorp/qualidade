using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Controllers;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ReclamacoesNaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ReclamacoesNaoConformidades.Controllers;

public class ReclamacaoNaoConformidadeControllerTest : TestUtils.UnitTestBaseWithDbContext
{
    [Fact(DisplayName = "criar Reclamacao Nao Conformidade Controller with Success")]
    public async Task CriarReclamacaoNaoConformidadeControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IReclamacaoNaoConformidadeService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var reclamacaoInput = new ReclamacaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Procedentes = TestUtils.ObjectMother.Ints[0],
            Improcedentes = TestUtils.ObjectMother.Ints[0],
            QuantidadeLote = TestUtils.ObjectMother.Ints[0],
            QuantidadeNaoConformidade = TestUtils.ObjectMother.Ints[0],
            DisposicaoProdutosAprovados = TestUtils.ObjectMother.Ints[0],
            DisposicaoProdutosConcessao = TestUtils.ObjectMother.Ints[0],
            Retrabalho = TestUtils.ObjectMother.Ints[0],
            Rejeitado = TestUtils.ObjectMother.Ints[0],
            RetrabalhoComOnus = true,
            RetrabalhoSemOnus = true,
            DevolucaoFornecedor = true,
            Recodificar = true,
            Sucata = true,
            Observacao = TestUtils.ObjectMother.Strings[0],
        };
        await fakeService.Insert(idNaoConformidade, reclamacaoInput);

        var controller = new ReclamacaoNaoConformidadeController(fakeService);

        // Act
        var output = await controller.Insert(idNaoConformidade, reclamacaoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }
    [Fact(DisplayName = "Get Reclamacao com sucesso")]
    public async Task GetReclamacaoControllerWithSuccessTest()
    {
        //Arrange
        var fakeService = Substitute.For<IReclamacaoNaoConformidadeService>();
        var viewOutput = new ReclamacaoNaoConformidadeOutput(TestUtils.ObjectMother.GetReclamacaoNaoConformidade(0));

        fakeService.Get(viewOutput.IdNaoConformidade).Returns(viewOutput);

        var controller = new ReclamacaoNaoConformidadeController(fakeService);

        //Act
        var output = await controller.Get(viewOutput.IdNaoConformidade);
        
        //Assert
        var result = new OkObjectResult(output);

        result.Value.Should().BeEquivalentTo(output);
        result.StatusCode.Should().Be(200);
    }
    [Fact(DisplayName = "Update Controller with Success")]
    public async Task UpdateControllerWithSuccessTest()
    {
        // Arrange
        var fakeService = Substitute.For<IReclamacaoNaoConformidadeService>();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        var reclamacaoInput = new ReclamacaoNaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Procedentes = TestUtils.ObjectMother.Ints[0],
            Improcedentes = TestUtils.ObjectMother.Ints[0],
            QuantidadeLote = TestUtils.ObjectMother.Ints[0],
            QuantidadeNaoConformidade = TestUtils.ObjectMother.Ints[0],
            DisposicaoProdutosAprovados = TestUtils.ObjectMother.Ints[0],
            DisposicaoProdutosConcessao = TestUtils.ObjectMother.Ints[0],
            Retrabalho = TestUtils.ObjectMother.Ints[0],
            Rejeitado = TestUtils.ObjectMother.Ints[0],
            RetrabalhoComOnus = true,
            RetrabalhoSemOnus = true,
            DevolucaoFornecedor = true,
            Recodificar = true,
            Sucata = true,
            Observacao = TestUtils.ObjectMother.Strings[0]
        };
        await fakeService.Update(idNaoConformidade, reclamacaoInput);

        var controller = new ReclamacaoNaoConformidadeController(fakeService);

        // Act
        var output = await controller.Update(idNaoConformidade, reclamacaoInput);

        // Assert
        var result = output as OkResult;
        result.StatusCode.Should().Be(200);
    }
   
}