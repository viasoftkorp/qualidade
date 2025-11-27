using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Rebus.TestHelpers.Events;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ServicoSolucoes;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests.ServicoSolucaoTests;

public class ServicoDeleteTests : SolucaoServiceTest
{
    [Fact(DisplayName = "DeleteServico with Success")]
    public async Task DeleteServicoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var servicoSolucaoInserido = TestUtils.ObjectMother.GetServicoSolucao(0);

        await mocker.ServicoSolucoes.InsertAsync(servicoSolucaoInserido);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.DeleteServico(servicoSolucaoInserido.Id);

        //Assert
        var servicoSolucaoEncontrado = await mocker.ServicoSolucoes.AnyAsync(p => p.Id == servicoSolucaoInserido.Id);
        ServiceBus.FakeBus.Events.OfType<MessagePublished<ServicoSolucaoDeleted>>().Should().HaveCount(1);
        servicoSolucaoEncontrado.Should().BeFalse();
        output.Should().Be(ValidationResult.Ok);
    }

    [Fact(DisplayName = "DeleteServico with NotFound Id")]
    public async Task DeleteServicoWithNotFoundId()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //Act
        var output = await service.DeleteServico(TestUtils.ObjectMother.Guids[1]);

        //Assert
        output.Should().Be(ValidationResult.NotFound);
    }
}