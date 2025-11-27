using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.EstornarOrdemRetrabalhoValidatorServicesTests;

public class ValidateOrigemInspecaoSaidaTests : EstornarOrdemRetrabalhoValidatorServiceTest
{
    [Fact(DisplayName = "Se origem for inspeção de saída, deve retornar RncComOrigemInspecaoSaida")]
    public async Task ValidateOrigemInspecaoSaidaTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.Origem = OrigemNaoConformidade.InpecaoSaida;
        //Act
        var result = await service
            .ValidateOrigemInspecaoSaida()
            .ValidateAsync(agregacaoNaoConformidade);
        
        //Assert
        result.Should().Be(EstornarOrdemRetrabalhoValidationResult.RncComOrigemInspecaoSaida);
    }
    [Fact(DisplayName = "Se origem não for inspeção de saída, deve retornar Ok")]
    public async Task ValidateOrigemInspecaoSaidaTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.Origem = OrigemNaoConformidade.Cliente;
        //Act
        var result = await service
            .ValidateOrigemInspecaoSaida()
            .ValidateAsync(agregacaoNaoConformidade);
        
        //Assert
        result.Should().Be(EstornarOrdemRetrabalhoValidationResult.Ok);
    }
}