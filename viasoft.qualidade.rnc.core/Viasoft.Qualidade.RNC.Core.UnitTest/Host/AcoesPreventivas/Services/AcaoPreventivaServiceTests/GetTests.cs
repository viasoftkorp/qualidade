using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.AcoesPreventivas.Services.AcaoPreventivaServiceTests;

public class GetTests : AcaoPreventivaServiceTest
{
    [Fact(DisplayName = "Se ação preventiva encontrada, deve retorna-la")]
    public async Task GetTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var acaoPreventiva = new AcaoPreventiva
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
        };
        await mocker.AcaoPreventiva.InsertAsync(acaoPreventiva, true);

        var expectedResult = new AcaoPreventivaOutput(acaoPreventiva);
        //Act
        var output = await service.Get(TestUtils.ObjectMother.Guids[0]);
        
        //Assert
        output.Should().BeEquivalentTo(expectedResult);
    }
}