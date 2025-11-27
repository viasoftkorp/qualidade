using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services.NaoConformidadeValidationService;

public class NaoConformidadeValidationServiceCampoNotaFiscalTests : NaoConformidadeValidationServiceTest
{
    [Fact(DisplayName = "Se ouver idNotaFiscal, deve retornar Ok")]
    public void ValidarCampoNotaFiscalTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InspecaoEntrada,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0]
        };
        //Act
        var result = service.ValidarCampoNotaFiscal(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.Ok);
    }
    [Fact(DisplayName = "Se não ouver idNotaFiscal e origem for inspeção de entrada, deve retornar nota fiscal obrigatório")]
    public void ValidarCampoNotaFiscalTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InspecaoEntrada,
            IdNotaFiscal = null
        };
        //Act
        var result = service.ValidarCampoNotaFiscal(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.NotaFiscalObrigatoria);
    }
}