using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.EntityFrameworkCore.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Dtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Events;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalHandlers.ProducaoApontamento.ApontamentoHandlerTests;

public class ApontamentoInicioProducaoEventTests: ApontamentoHandlerTest
{
    [Fact(DisplayName = "Se apontamento não se trata de retrabalho, nada deve ser feito")]
    public async Task OperacaoInicioProducaoEventTest()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        
        var operacao = GetOperacao(0);

        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        
        await mocker.Operacoes.InsertAsync(operacao);
        await UnitOfWork.CompleteAsync();

        var producaoIniciadaDto = GetInicioProducaoEventDto(0);
        producaoIniciadaDto.IsOperacaoRetrabalho = false;
        producaoIniciadaDto.IsOrdemRetrabalho = false;
        
        var evento = new ApontamentoInicioProducaoEvent
        {
            ApontamentoProducaoEventDto = producaoIniciadaDto
        };
        
        //Act
        await handler.Handle(evento);
        LimparTracker(mocker);
        //Assert
        mocker.Operacoes.GetUnderlyingDbContext().ChangeTracker.Clear();
        var statusOperacaoResult = (await mocker.Operacoes.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOperacaoResult.Should().Be(StatusProducaoRetrabalho.Aberta);
    }
    
    [Fact(DisplayName = "Se operacao não encontrada e ordemRetrabalho não encontrada, nada deve acontecer")]
    public async Task OperacaoInicioProducaoEventTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);

        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        
        var operacao = GetOperacao(0);
        operacao.NumeroOperacao = "020";
        await mocker.Operacoes.InsertAsync(operacao, true);
        
        var producaoIniciadaDto = GetInicioProducaoEventDto(0);
        
        var evento = new ApontamentoInicioProducaoEvent
        {
            ApontamentoProducaoEventDto = producaoIniciadaDto
        };
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOperacaoResult = (await mocker.Operacoes.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOperacaoResult.Should().Be(StatusProducaoRetrabalho.Aberta);
    }
    
    [Fact(DisplayName = "Se operacao encontrada, deve alterar status da operação para produzindo")]
    public async Task OperacaoInicioProducaoEventTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);

        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        
        var operacao = GetOperacao(0);
        await mocker.Operacoes.InsertAsync(operacao, true);
        
        var producaoIniciadaDto = GetInicioProducaoEventDto(0);
        
        var evento = new ApontamentoInicioProducaoEvent
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
    
    [Fact(DisplayName = "Se operacao não encontrada mas ordem retrabalho encontrada, deve alterar status da ordem de retrabalho para produzindo")]
    public async Task OperacaoInicioProducaoEventTest4()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);

        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);

        var ordemRetrabalho = GetOrdemRetrabalhoNaoConformidade(0);
        ordemRetrabalho.NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0];
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalho, true);
        
        var producaoIniciadaDto = GetInicioProducaoEventDto(0);
        producaoIniciadaDto.NumeroOdf = TestUtils.ObjectMother.Ints[0];

        var evento = new ApontamentoInicioProducaoEvent
        {
            ApontamentoProducaoEventDto = producaoIniciadaDto
        };
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOrdemRetrabalhoResult = (await mocker.OrdemRetrabalhoNaoConformidades.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOrdemRetrabalhoResult.Should().Be(StatusProducaoRetrabalho.Produzindo);
    }
    private ApontamentoInicioProducaoEventDto GetInicioProducaoEventDto(int index)
    {
        var inicioProducaoEventDto = new ApontamentoInicioProducaoEventDto
        {
            NumeroOdf = TestUtils.ObjectMother.Ints[index],
            NumeroOperacao = TestUtils.ObjectMother.Ints[index].ToString(),
            IsOperacaoRetrabalho = true,
            IsOrdemRetrabalho = true
        };
        return inicioProducaoEventDto;
    }
}