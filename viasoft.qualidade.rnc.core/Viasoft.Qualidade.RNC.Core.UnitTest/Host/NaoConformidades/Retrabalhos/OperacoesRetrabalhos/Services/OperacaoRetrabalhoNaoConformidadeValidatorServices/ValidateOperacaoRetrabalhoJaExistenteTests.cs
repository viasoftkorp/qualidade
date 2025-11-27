using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OperacoesRetrabalhos.Services.OperacaoRetrabalhoNaoConformidadeValidatorServices;

public class ValidateOperacaoRetrabalhoJaExistenteTests : OperacaoRetrabalhoNaoConformidadeValidatorServiceTest
{
    [Fact(DisplayName = "Se operação retrabalho já foi criada, deve retornar OperacaoRetrabalhoJaExiste")]
    public void ValidateOperacaoRetrabalhoJaExistenteTest1()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();

        agregacao.NaoConformidade.OperacaoRetrabalho = new OperacaoRetrabalhoNaoConformidade();
        
        // Act
        var result =  service.ValidateOperacaoRetrabalhoJaExistente(agregacao);
        
        // Assert
        result.Should().Be(OperacaoRetrabalhoNaoConformidadeValidationResult.OperacaoRetrabalhoJaExiste);
    }
    
    [Fact(DisplayName = "Se operação retrabalho ainda não criada, deve retornar Ok")]
    public void ValidateOperacaoRetrabalhoJaExistenteTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();

        // Act
        var result =  service.ValidateOperacaoRetrabalhoJaExistente(agregacao);
        
        // Assert
        result.Should().Be(OperacaoRetrabalhoNaoConformidadeValidationResult.Ok);
    }
}