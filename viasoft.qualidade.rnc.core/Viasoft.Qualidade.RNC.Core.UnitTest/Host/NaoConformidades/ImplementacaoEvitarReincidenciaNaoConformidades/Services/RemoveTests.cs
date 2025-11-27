using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.
    Services;

public class RemoveTests : ImplementacaoEvitarReincidenciaNaoConformidadeServiceTest
{
    [Fact(DisplayName = "Se método chamado, deve remover a implementação")]
    public async Task RemoveTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var naoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0);
        var agregacaoCriada = naoConformidade.AgregacaoFromThis();
        var idNaoConformidade = TestUtils.ObjectMother.Guids[0];
        mocker.NaoConformidadeRepository.Get(idNaoConformidade)
            .Returns(agregacaoCriada);
        //Act
        await service.Remove(TestUtils.ObjectMother.Guids[0], TestUtils.ObjectMother.Guids[0]);
        //Assert
        naoConformidade.ImplementacoesEvitarReincidenciaNaoConformidades.Should().BeEmpty();

    }
}