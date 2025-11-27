using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Rebus.TestHelpers.Events;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ProdutoSolucoes;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests.ProdutoSolucaoTests;

public class ProdutoUpdateTests : SolucaoServiceTest
{
    [Fact(DisplayName = "UpdateProduto with Success")]
    public async Task UpdateProdutoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var solucaoInput = TestUtils.ObjectMother.GetProdutoSolucao(0);

        await mocker.ProdutoSolucoes.InsertAsync(solucaoInput);

        var updateInput = new ProdutoSolucaoInput
        {
            Id = solucaoInput.Id,
            Quantidade = TestUtils.ObjectMother.Ints[7],
            IdProduto = TestUtils.ObjectMother.Guids[4],
            IdSolucao = solucaoInput.IdSolucao
        };

        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.UpdateProduto(updateInput.Id, updateInput);

        //Assert
        var causa = await mocker.ProdutoSolucoes.FindAsync(updateInput.Id);
        ServiceBus.FakeBus.Events.OfType<MessagePublished<ProdutoSolucaoUpdated>>().Should().HaveCount(1);
        causa.Should().BeEquivalentTo(updateInput);
        output.Should().Be(ValidationResult.Ok);
    }

    [Fact(DisplayName = "UpdateProduto Returns NotFound")]
    public async Task UpdateProdutoWithNotFoundIdTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var updateInput = TestUtils.ObjectMother.GetProdutoSolucaoInput(0);

        //Act
        var output = await service.UpdateProduto(updateInput.Id, updateInput);

        //Assert
        output.Should().Be(ValidationResult.NotFound);
    }
}