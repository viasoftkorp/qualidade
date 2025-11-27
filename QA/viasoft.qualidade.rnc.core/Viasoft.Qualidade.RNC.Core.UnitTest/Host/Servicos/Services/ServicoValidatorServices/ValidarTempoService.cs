using FluentAssertions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Servicos.Services.ServicoValidatorServices;

public class ValidarTempoService : ServicoValidatorServiceTest
{
    [Fact(DisplayName = "Se horas ou minutos menor que 0, deve retornar false")]
    public void ValidarTempoTest()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        // Act
        var result = service.ValidarTempo(-1, 2);
        
        // Assert
        result.Should().BeFalse();
    }
    [Fact(DisplayName = "Se horas e minutos iguais a 0, deve retornar false")]
    public void ValidarTempoTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        // Act
        var result = service.ValidarTempo(0 , 0);
        
        // Assert
        result.Should().BeFalse();
    }
    [Fact(DisplayName = "Se horas ou minutos diferentes de 0, deve retornar true")]
    public void ValidarTempoTest3()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        // Act
        var result = service.ValidarTempo(0, 1);
        
        // Assert
        result.Should().BeTrue();
    }
}