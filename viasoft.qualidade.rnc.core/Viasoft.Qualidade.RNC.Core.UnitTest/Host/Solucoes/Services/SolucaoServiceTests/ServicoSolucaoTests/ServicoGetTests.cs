using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests.ServicoSolucaoTests;

public class ServicoGetTests : SolucaoServiceTest
{
    [Fact(DisplayName = "GetServicoSolucaoView with Success")]
    public async Task GetServicoSolucaoViewWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = TestUtils.ObjectMother.GetServicoSolucao(0);

        await mocker.ServicoSolucoes.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.GetServicoSolucaoView(TestUtils.ObjectMother.Guids[0]);

        //Assert
        var solucao = await mocker.ServicoSolucoes.FindAsync(TestUtils.ObjectMother.Guids[0]);
        solucao.Should().BeEquivalentTo(output);
    }

    [Fact(DisplayName = "GetServicoSolucaoView Returns Null")]
    public async Task GetServicoSolucaoViewWithReturnNullTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = TestUtils.ObjectMother.GetServicoSolucao(0);

        await mocker.ServicoSolucoes.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.GetServicoSolucaoView(TestUtils.ObjectMother.Guids[1]);

        //Assert
        output.Should().BeNull();
    }
}