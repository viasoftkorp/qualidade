using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services;

public class NaoConformidadeServiceDeleteTests : NaoConformidadeServiceTest
{
    [Fact(DisplayName = "Delete NaoConformidade with Success")]
    public async Task DeleteNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.AgregacaoRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);

        //Act
        await service.Delete(idNaoConformidade);

        //Assert
        agregacaoCriada.NaoConformidadeRemover.Should().BeEquivalentTo(agregacaoCriada.NaoConformidade);
    }
}