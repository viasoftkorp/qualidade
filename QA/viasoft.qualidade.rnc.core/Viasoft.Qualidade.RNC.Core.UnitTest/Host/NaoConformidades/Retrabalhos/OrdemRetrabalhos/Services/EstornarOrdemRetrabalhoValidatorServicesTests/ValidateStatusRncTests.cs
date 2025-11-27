using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.EstornarOrdemRetrabalhoValidatorServicesTests;

public class ValidateStatusRncTests : EstornarOrdemRetrabalhoValidatorServiceTest
{
    [Fact(DisplayName = "Se status rnc diferente de fechado, deve retornar ok")]
    public async Task ValidateStatusRncTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.Status = StatusNaoConformidade.Aberto;
        
        //Act
        var result = await service
            .ValidateStatusRnc()
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(EstornarOrdemRetrabalhoValidationResult.Ok);
    }    
    [Fact(DisplayName = "Se status rnc igual fechado, deve retornar RncFechada")]
    public async Task ValidateStatusRncTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.Status = StatusNaoConformidade.Fechado;
        
        //Act
        var result = await service
            .ValidateStatusRnc()
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(EstornarOrdemRetrabalhoValidationResult.RncFechada);
    } 
}