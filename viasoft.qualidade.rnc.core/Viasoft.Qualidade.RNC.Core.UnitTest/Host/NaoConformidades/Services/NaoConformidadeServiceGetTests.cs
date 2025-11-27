using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services;

public class NaoConformidadeServiceGetTests : NaoConformidadeServiceTest
{
    [Fact(DisplayName = "Get NaoConformidade with Success")]
    public async Task GetNaoConformidadeWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = TestUtils.ObjectMother.GetNaoConformidade(0);
        await mocker.NaoConformidadeRepository.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();

        var expectedResult = new NaoConformidadeOutput(TestUtils.ObjectMother.GetNaoConformidade(0));
        //Act
        var output = await service.Get(input.Id);

        //Assert
        output.Should().BeEquivalentTo(expectedResult);
    }
}