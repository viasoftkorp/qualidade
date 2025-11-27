using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Rebus.TestHelpers.Events;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ProdutoSolucoes;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests.ProdutoSolucaoTests;

public class ProdutoCreateTests : SolucaoServiceTest
{
    [Fact(DisplayName = "AddProduto with Success")]
    public async Task AddProdutoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var createInput = TestUtils.ObjectMother.GetProdutoSolucaoInput(0);

        //Act
        var output = await service.AddProduto(createInput);

        //Assert
        var causa = await mocker.ProdutoSolucoes.FindAsync(createInput.Id);
        ServiceBus.FakeBus.Events.OfType<MessagePublished<ProdutoSolucaoCreated>>().Should().HaveCount(1);
        causa.Should().BeEquivalentTo(createInput);
        output.Should().Be(ValidationResult.Ok);
    }
}