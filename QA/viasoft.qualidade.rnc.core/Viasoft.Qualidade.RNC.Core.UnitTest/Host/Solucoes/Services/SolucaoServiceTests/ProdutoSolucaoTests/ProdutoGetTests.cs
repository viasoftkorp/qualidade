using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests.ProdutoSolucaoTests;

public class ProdutoGetTests : SolucaoServiceTest
{
    [Fact(DisplayName = "GetProdutoSolucaoView with Success")]
    public async Task ProdutoSolucaoViewWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = TestUtils.ObjectMother.GetProdutoSolucao(0);

        await mocker.ProdutoSolucoes.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.GetProdutoSolucaoView(TestUtils.ObjectMother.Guids[0]);

        //Assert
        var causa = await mocker.ProdutoSolucoes.FindAsync(TestUtils.ObjectMother.Guids[0]);
        causa.Should().BeEquivalentTo(output);
    }

    [Fact(DisplayName = "GetProdutoSolucaoView Returns Null")]
    public async Task ProdutoSolucaoViewWithReturnNullTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = TestUtils.ObjectMother.GetProdutoSolucao(0);

        await mocker.ProdutoSolucoes.InsertAsync(input);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.GetProdutoSolucaoView(TestUtils.ObjectMother.Guids[1]);

        //Assert
        output.Should().BeNull();
    }
}