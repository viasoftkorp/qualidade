using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services.NaoConformidadeValidationService;

public class NaoConformidadeValidationServiceCampoClienteTests : NaoConformidadeValidationServiceTest
{
    [Fact(DisplayName = "Se houver idPessoa, deve retornar Ok")]
    public void ValidarCampoClienteTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.Cliente,
            IdPessoa = TestUtils.ObjectMother.Guids[0]
        };
        //Act
        var result = service.ValidarCampoCliente(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.Ok);
    }
    [Fact(DisplayName = "Se não houver idPessoa e a origem for cliente,deve retornar cliente obrigatório")]
    public void ValidarCampoClienteTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.Cliente,
            IdPessoa = null
        };
        //Act
        var result = service.ValidarCampoCliente(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.ClienteObrigatorio);
    }
}