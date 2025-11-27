using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.
    GerarOrdemRetrabalhoValidatorServicesTests;

public class ValidateOperacaoEngenhariaFinalTests : GerarOrdemRetrabalhoValidatorServiceTest
{
    [Fact(DisplayName = "Se for encontrado serviço de operação engenharia 999, deve retornar ok")]
    public async Task ValidateOperacaoEngenhariaFinalTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.ServicoNaoConformidades.Add(new ServicoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            OperacaoEngenharia = "999"
        });
        //Act
        var result = await service
            .ValidateOperacaoEngenhariaFinal()
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.Ok);
    }
    [Fact(DisplayName = "Se não for encontrado serviço de operação engenharia 999, deve retornar operacaoFinalNaoEncontrada")]
    public async Task ValidateOperacaoEngenhariaFinalTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();

        //Act
        var result = await service
            .ValidateOperacaoEngenhariaFinal()
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.OperacaoFinalNaoEncontrada);
    }
}