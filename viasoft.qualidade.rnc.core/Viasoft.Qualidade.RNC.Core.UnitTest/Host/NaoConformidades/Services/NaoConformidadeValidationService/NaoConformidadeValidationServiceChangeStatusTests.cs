using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services.NaoConformidadeValidationService;

public class NaoConformidadeValidationServiceChangeStatusTests : NaoConformidadeValidationServiceTest
{
    [Fact(DisplayName = "Se tentar modificar status nao conformidade para fechado, deve retornar statusFechado")]
    public void ValidarChangeStatusTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Status = StatusNaoConformidade.Fechado
        };
        //Act
        var result = service.ValidarChangeStatus(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.StatusFechado);
    }
    
    [Fact(DisplayName = "Se tentar modificar status nao conformidade para status diferente de fechado, deve retornar Ok")]
    public void ValidarChangeStatusTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Status = StatusNaoConformidade.Pendente
        };
        //Act
        var result = service.ValidarChangeStatus(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.Ok);
    }
}