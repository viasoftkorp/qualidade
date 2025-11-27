using System.Threading.Tasks;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.OrdemRetrabalhoServicesTests;

public class CanGenerateTests : OrdemRetrabalhoServiceTest
{
    [Fact(DisplayName = "Se isFullValidation, deve realizar todas as validações")]
    public async Task CanGenerateTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        MockGetAgregacaoReturn(mocker, agregacao);
        var ordemRetrabalhoInput = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0]
        };
        //Act
        await service.CanGenerate(TestUtils.ObjectMother.Guids[0],ordemRetrabalhoInput, true);
        //Assert
        await mocker.GerarOrdemRetrabalhoValidatorService.Received(1)
            .ValidateStatusRnc()
            .ValidateOperacaoEngenhariaFinal()
            .ValidateOperacaoEngenhariaDuplicada()
            .ValidateOdf()
            .ValidateLote(ordemRetrabalhoInput)
            .ValidateAsync(agregacao);
    }
    [Fact(DisplayName = "Se não isFullValidation, deve validar odf")]
    public async Task CanGenerateTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        MockGetAgregacaoReturn(mocker, agregacao);
        var ordemRetrabalhoInput = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0]
        };
        //Act
        await service.CanGenerate(TestUtils.ObjectMother.Guids[0], ordemRetrabalhoInput, false);
        //Assert
        await mocker.GerarOrdemRetrabalhoValidatorService.Received(1)
            .ValidateStatusRnc()
            .ValidateOdf()
            .ValidateAsync(agregacao);
    }
}