using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Rebus.TestHelpers.Events;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ProdutoSolucoes;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests.ProdutoSolucaoTests;

public class ProdutoDeleteTests : SolucaoServiceTest
{
    [Fact(DisplayName = "DeleteProduto with Success")]
    public async Task DeleteProdutoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var produtoSolucaoInserido = TestUtils.ObjectMother.GetProdutoSolucao(0);

        await mocker.ProdutoSolucoes.InsertAsync(produtoSolucaoInserido);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.DeleteProduto(produtoSolucaoInserido.Id);

        //Assert
        var produtoSolucaoEncontrado = await mocker.ProdutoSolucoes.AnyAsync(p => p.Id == produtoSolucaoInserido.Id);
        ServiceBus.FakeBus.Events.OfType<MessagePublished<ProdutoSolucaoDeleted>>().Should().HaveCount(1);
        produtoSolucaoEncontrado.Should().BeFalse();
        output.Should().Be(ValidationResult.Ok);
    }

    [Fact(DisplayName = "DeleteProduto with NotFound Id")]
    public async Task DeleteProdutoWithNotFoundId()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //Act
        var output = await service.DeleteProduto(TestUtils.ObjectMother.Guids[1]);

        //Assert
        output.Should().Be(ValidationResult.NotFound);
    }
}

    