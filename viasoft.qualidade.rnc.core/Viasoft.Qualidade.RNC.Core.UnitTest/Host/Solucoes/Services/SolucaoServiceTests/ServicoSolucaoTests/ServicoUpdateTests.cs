using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Rebus.TestHelpers.Events;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes.Events.ServicoSolucoes;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Servicos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests.ServicoSolucaoTests;

public class ServicoUpdateTests : SolucaoServiceTest
{
    [Fact(DisplayName = "UpdateServico with Success")]
    public async Task UpdateServicoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var solucaoInput = TestUtils.ObjectMother.GetServicoSolucao(0);

        await mocker.ServicoSolucoes.InsertAsync(solucaoInput);
        MockValidarTempo(TestUtils.ObjectMother.Ints[2], TestUtils.ObjectMother.Ints[3], true);

        var updateInput = new ServicoSolucaoInput
        {
            Id = solucaoInput.Id,
            Quantidade = TestUtils.ObjectMother.Ints[7],
            IdProduto = TestUtils.ObjectMother.Guids[4],
            IdSolucao = solucaoInput.IdSolucao,
            Horas = TestUtils.ObjectMother.Ints[2],
            Minutos = TestUtils.ObjectMother.Ints[3],
            IdRecurso = TestUtils.ObjectMother.Guids[5],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[5]
        };

        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.UpdateServico(updateInput.Id, updateInput);

        //Assert
        var causa = await mocker.ServicoSolucoes.FindAsync(updateInput.Id);
        ServiceBus.FakeBus.Events.OfType<MessagePublished<ServicoSolucaoUpdated>>().Should().HaveCount(1);
        causa.Should().BeEquivalentTo(updateInput);
        output.Should().Be(ServicoValidationResult.Ok);
    }

    [Fact(DisplayName = "UpdateServico Returns NotFound")]
    public async Task UpdateServicoWithNotFoundIdTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var updateInput = TestUtils.ObjectMother.GetServicoSolucaoInput(0);

        //Act
        var output = await service.UpdateServico(updateInput.Id, updateInput);

        //Assert
        output.Should().Be(ServicoValidationResult.NotFound);
    }
    
    [Fact(DisplayName = "Se falhar ao validar tempo, deve retornar TempoInvalido")]
    public async Task UpdateServicoTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var solucao = TestUtils.ObjectMother.GetServicoSolucao(0);

        await mocker.ServicoSolucoes.InsertAsync(solucao);
        MockValidarTempo(TestUtils.ObjectMother.Ints[2], TestUtils.ObjectMother.Ints[1], false);

        var updateInput = new ServicoSolucaoInput
        {
            Id = solucao.Id,
            Quantidade = TestUtils.ObjectMother.Ints[7],
            IdProduto = TestUtils.ObjectMother.Guids[4],
            IdSolucao = solucao.IdSolucao,
            Horas = TestUtils.ObjectMother.Ints[2],
            Minutos = TestUtils.ObjectMother.Ints[3],
            IdRecurso = TestUtils.ObjectMother.Guids[5],
            OperacaoEngenharia = TestUtils.ObjectMother.Strings[5]
        };

        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.UpdateServico(updateInput.Id, updateInput);

        //Assert
        output.Should().Be(ServicoValidationResult.TempoInvalido);
    }
}