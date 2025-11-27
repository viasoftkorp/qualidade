using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Dtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Events;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalHandlers.ProducaoApontamento.ApontamentoHandlerTests;

public class ApontamentoOperacaoRemovidaEventTests: ApontamentoHandlerTest
{
    [Fact(DisplayName = "Se operacao não for retrabalho, nada deve ser feito")]
    public async Task OperacaoRemovidaEventTest()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        
        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        
        var operacao = GetOperacao(0);
        await mocker.Operacoes.InsertAsync(operacao);
        await UnitOfWork.CompleteAsync();

        var producaoIniciadaDto = GetOperacaoRemovidaDto(0);
        producaoIniciadaDto.IsOperacaoRetrabalho = false;
        
        var evento = new ApontamentoOperacaoRemovidaEvent
        {
            ApontamentoProducaoEventEventDto = producaoIniciadaDto
        };
        
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOperacaoResult = (await mocker.Operacoes.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOperacaoResult.Should().Be(StatusProducaoRetrabalho.Aberta);
    }
    
    [Fact(DisplayName = "Se operacao não encontrada, nada deve ser feito")]
    public async Task OperacaoRemovidaEventTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);

        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        
        var operacao = GetOperacao(0);
        operacao.Status = StatusProducaoRetrabalho.Aberta;
        operacao.NumeroOperacao = "020";
        await mocker.Operacoes.InsertAsync(operacao, true);
        
        var producaoIniciadaDto = GetOperacaoRemovidaDto(0);
        
        var evento = new ApontamentoOperacaoRemovidaEvent
        {
            ApontamentoProducaoEventEventDto = producaoIniciadaDto
        };
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOperacaoResult = (await mocker.Operacoes.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOperacaoResult.Should().Be(StatusProducaoRetrabalho.Aberta);
    }
    
    [Fact(DisplayName = "Se operacao encontrada, deve alterar status para cancelada")]
    public async Task OperacaoRemovidaEventTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);

        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        
        var operacao = GetOperacao(0);
        operacao.Status = StatusProducaoRetrabalho.Aberta;

        await mocker.Operacoes.InsertAsync(operacao, true);
        
        var producaoIniciadaDto = GetOperacaoRemovidaDto(0);
        
        var evento = new ApontamentoOperacaoRemovidaEvent
        {
            ApontamentoProducaoEventEventDto = producaoIniciadaDto
        };
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOperacaoResult = (await mocker.Operacoes.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOperacaoResult.Should().Be(StatusProducaoRetrabalho.Cancelada);
    }
    private ApontamentoOperacaoRemovidaEventDto GetOperacaoRemovidaDto(int index)
    {
        var operacaoRemovidaDto = new ApontamentoOperacaoRemovidaEventDto
        {
            NumeroOperacao = TestUtils.ObjectMother.Ints[index].ToString(),
            NumeroOdf = TestUtils.ObjectMother.Ints[index],
            IsOperacaoRetrabalho = true
        };
        return operacaoRemovidaDto;
    }
}