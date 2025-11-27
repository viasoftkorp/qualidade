using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OperacoesRetrabalhos.Services.OperacaoRetrabalhoNaoConformidadeValidatorServices;

public class ValidateStatusRncTests: OperacaoRetrabalhoNaoConformidadeValidatorServiceTest
{
    [Fact(DisplayName = "Se status não conformidade for diferente de fechado, deve retornar Ok")]
    public void ValidateStatusRncTest1()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.Status = StatusNaoConformidade.Aberto;
        // Act
        var result =  service.ValidateStatusRnc(agregacao.NaoConformidade);

        // Assert
        result.Should().Be(OperacaoRetrabalhoNaoConformidadeValidationResult.Ok);
    }
    [Fact(DisplayName = "Se status não conformidade for fechado, deve retornar RncFechada")]
    public void ValidateStatusRncTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.Status = StatusNaoConformidade.Fechado;
        // Act
        var result =  service.ValidateStatusRnc(agregacao.NaoConformidade);

        // Assert
        result.Should().Be(OperacaoRetrabalhoNaoConformidadeValidationResult.RncFechada);
    }
}