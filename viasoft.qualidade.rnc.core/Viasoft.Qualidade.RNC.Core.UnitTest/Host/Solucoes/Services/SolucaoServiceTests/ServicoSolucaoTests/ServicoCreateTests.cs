using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Rebus.TestHelpers.Events;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ServicoSolucoes;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests.ServicoSolucaoTests;

public class ServicoCreateTests : SolucaoServiceTest
{
    [Fact(DisplayName = "AddServico with Success")]
    public async Task AddServicoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var createInput = TestUtils.ObjectMother.GetServicoSolucaoInput(0);
        createInput.Horas = TestUtils.ObjectMother.Ints[2];
        createInput.Minutos = TestUtils.ObjectMother.Ints[1];
        MockValidarTempo(horas: TestUtils.ObjectMother.Ints[2], minutos: TestUtils.ObjectMother.Ints[1], true);
        //Act
        var output = await service.AddServico(createInput);

        //Assert
        var causa = await mocker.ServicoSolucoes.FindAsync(createInput.Id);
        ServiceBus.FakeBus.Events.OfType<MessagePublished<ServicoSolucaoCreated>>().Should().HaveCount(1);
        causa.Should().BeEquivalentTo(createInput);
        output.Should().Be(ServicoValidationResult.Ok);
    }
    
    [Fact(DisplayName = "Se falhar ao validar tempo, deve retornar TempoInvalido")]
    public async Task AddServicoTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var createInput = TestUtils.ObjectMother.GetServicoSolucaoInput(0);
        MockValidarTempo(TestUtils.ObjectMother.Ints[2], TestUtils.ObjectMother.Ints[1], false);
        //Act
        var output = await service.AddServico(createInput);

        //Assert
        output.Should().Be(ServicoValidationResult.TempoInvalido);
    }
}