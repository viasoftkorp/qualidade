using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services.NaoConformidadeValidationService;

public class NaoConformidadeValidationServiceCampoFornecedorTests : NaoConformidadeValidationServiceTest
{
    [Fact(DisplayName = "Se ouver idPessoa, deve retornar Ok")]
    public void ValidarCampoFornecedorTest()
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
        var result = service.ValidarCampoFornecedor(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.Ok);
    }
    [Fact(DisplayName = "Se não ouver idPessoa e a origem for inspeção de entrada,deve retornar fornecedor obrigatório")]
    public void ValidarCampoFornecedorTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InspecaoEntrada,
            IdPessoa = null
        };
        //Act
        var result = service.ValidarCampoFornecedor(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.FornecedorObrigatorio);
    }
}