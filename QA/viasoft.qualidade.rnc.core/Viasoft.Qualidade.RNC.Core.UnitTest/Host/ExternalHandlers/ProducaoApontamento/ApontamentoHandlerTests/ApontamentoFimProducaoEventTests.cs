using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Dtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Events;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalHandlers.ProducaoApontamento.ApontamentoHandlerTests;

public class ApontamentoFimProducaoEventTests: ApontamentoHandlerTest
{
    [Fact(DisplayName = "Se operacao não for retrabalho, nada deve ser feito")]
    public async Task OperacaoFimProducaoEventTest()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        
        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        var operacao = GetOperacao(0);
        operacao.Status = StatusProducaoRetrabalho.Produzindo;
        await mocker.Operacoes.InsertAsync(operacao);
        await UnitOfWork.CompleteAsync();

        var producaoIniciadaDto = GetFimProducaoEventDto(0);
        producaoIniciadaDto.IsOperacaoRetrabalho = false;
        
        var evento = new ApontamentoFimProducaoEvent
        {
            ApontamentoProducaoEventDto = producaoIniciadaDto
        };
        
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOperacaoResult = (await mocker.Operacoes.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOperacaoResult.Should().Be(StatusProducaoRetrabalho.Produzindo);
    }
    
    [Fact(DisplayName = "Se operacao nao for a ultima, nada deve ser feito")]
    public async Task OperacaoFimProducaoEventTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        
        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        var operacao = GetOperacao(0);
        operacao.Status = StatusProducaoRetrabalho.Produzindo;
        await mocker.Operacoes.InsertAsync(operacao);
        await UnitOfWork.CompleteAsync();

        var producaoIniciadaDto = GetFimProducaoEventDto(0);
        producaoIniciadaDto.IsUltimoApontamentoOperacao = false;
        
        var evento = new ApontamentoFimProducaoEvent
        {
            ApontamentoProducaoEventDto = producaoIniciadaDto
        };
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOperacaoResult = (await mocker.Operacoes.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOperacaoResult.Should().Be(StatusProducaoRetrabalho.Produzindo);
    }
    
    [Fact(DisplayName = "Se operacao não encontrada, nada deve ser feito")]
    public async Task OperacaoFimProducaoEventTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);

        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        
        var operacao = GetOperacao(0);
        operacao.Status = StatusProducaoRetrabalho.Produzindo;
        operacao.NumeroOperacao = "020";
        await mocker.Operacoes.InsertAsync(operacao, true);
        
        var producaoIniciadaDto = GetFimProducaoEventDto(0);
        
        var evento = new ApontamentoFimProducaoEvent
        {
            ApontamentoProducaoEventDto = producaoIniciadaDto
        };
        //Act
        LimparTracker(mocker);
        await handler.Handle(evento);
        //Assert
        var statusOperacaoResult = (await mocker.Operacoes.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOperacaoResult.Should().Be(StatusProducaoRetrabalho.Produzindo);
    }
    
    [Fact(DisplayName = "Se operacao encontrada, deve alterar status para produzindo")]
    public async Task OperacaoFimProducaoEventTest4()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);

        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        
        var operacao = GetOperacao(0);
        operacao.Status = StatusProducaoRetrabalho.Produzindo;

        await mocker.Operacoes.InsertAsync(operacao, true);
        
        var producaoIniciadaDto = GetFimProducaoEventDto(0);
        var evento = new ApontamentoFimProducaoEvent
        {
            ApontamentoProducaoEventDto = producaoIniciadaDto
        };
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOperacaoResult = (await mocker.Operacoes.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOperacaoResult.Should().Be(StatusProducaoRetrabalho.Encerrada);
    }
    private ApontamentoFimProducaoEventDto GetFimProducaoEventDto(int index)
    {
        var inicioProducaoEventDto = new ApontamentoFimProducaoEventDto
        {
            NumeroOdf = TestUtils.ObjectMother.Ints[index],
            NumeroOperacao = TestUtils.ObjectMother.Ints[index].ToString(),
            IsOperacaoRetrabalho = true,
            IsUltimoApontamentoOperacao = true
        };
        return inicioProducaoEventDto;
    }
}