using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Dtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Events;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.Operacoes.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalHandlers.ProducaoApontamento.ApontamentoHandlerTests;

public class ApontamentoEstornadoEventTests: ApontamentoHandlerTest
{
    [Fact(DisplayName = "Se operacao não encontrada, nada deve ser feito")]
    public async Task OperacaoEstornadaEventTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);

        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        
        var operacao = GetOperacao(0);
        operacao.Status = StatusProducaoRetrabalho.Encerrada;
        operacao.NumeroOperacao = "020";
        await mocker.Operacoes.InsertAsync(operacao, true);
        
        var producaoIniciadaDto = GetOperacaoEstornadaDto(0);
        
        var evento = new ApontamentoEstornadoEvent
        {
            ApontamentoProducaoEventEventDto = producaoIniciadaDto
        };
        //Act
        await handler.Handle(evento);
        //Assert
        var statusOperacaoResult = (await mocker.Operacoes.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOperacaoResult.Should().Be(StatusProducaoRetrabalho.Encerrada);
    }
    
    [Fact(DisplayName = "Se operacao encontrada e saldo da operação diferente do saldo da operação de retrabalho, deve alterar status operação para produzindo")]
    public async Task OperacaoEstornadaEventTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);

        await InserirNaoConformidade(0);
        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);

        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        
        MockarRetornoGetByNumeroOdfENumeroOperacao(mocker, 0);
        var expectedOperacaoSaldoDto = GetOperacaoSaldoDto(0);
        expectedOperacaoSaldoDto.Saldo = 2;
        MockarRetornoGetApontamentoOperacaoByLegacyIdOperacao(mocker, 0, expectedOperacaoSaldoDto);
        
        var operacao = GetOperacao(0);
        operacao.Status = StatusProducaoRetrabalho.Encerrada;

        await mocker.Operacoes.InsertAsync(operacao, true);
        
        var producaoIniciadaDto = GetOperacaoEstornadaDto(0);
        
        var evento = new ApontamentoEstornadoEvent
        {
            ApontamentoProducaoEventEventDto = producaoIniciadaDto
        };
        //Act
        await handler.Handle(evento);
        //Assert
        var statusOperacaoResult = (await mocker.Operacoes.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOperacaoResult.Should().Be(StatusProducaoRetrabalho.Produzindo);
    }
    
    [Fact(DisplayName = "Se operacao não encontrada mas ordem retrabalho encontrada e saldo da operação igual operação de retrabalho, deve alterar status ordem retrabalho para aberta")]
    public async Task OperacaoEstornadaEventTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);

        await InserirNaoConformidade(0);

        var expectedOperacaoRetrabalho = GetOperacaoRetrabalhoNaoConformidade(0);
        expectedOperacaoRetrabalho.Quantidade = 2;
        
        await InserirOperacaoRetrabalhoNaoConformidade(expectedOperacaoRetrabalho);
        
        MockarRetornoGetByNumeroOdfENumeroOperacao(mocker, 0);
        var expectedOperacaoSaldoDto = GetOperacaoSaldoDto(0);
        expectedOperacaoSaldoDto.Saldo = 2;
        MockarRetornoGetApontamentoOperacaoByLegacyIdOperacao(mocker, 0, expectedOperacaoSaldoDto);
        
        var ordemRetrabalho = GetOrdemRetrabalhoNaoConformidade(0);
        ordemRetrabalho.Status = StatusProducaoRetrabalho.Encerrada;
        ordemRetrabalho.NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0];
        ordemRetrabalho.Quantidade = 2;
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalho, true);
        
        var producaoIniciadaDto = GetOperacaoEstornadaDto(0);
        producaoIniciadaDto.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        
        var evento = new ApontamentoEstornadoEvent
        {
            ApontamentoProducaoEventEventDto = producaoIniciadaDto
        };
        //Act
        await handler.Handle(evento);
        //Assert
        var statusOrdemRetrabalho = (await mocker.OrdemRetrabalhoNaoConformidades.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOrdemRetrabalho.Should().Be(StatusProducaoRetrabalho.Aberta);
    }
    
    private ApontamentoOperacaoEstornadaEventDto GetOperacaoEstornadaDto(int index)
    {
        var operacaoRemovidaDto = new ApontamentoOperacaoEstornadaEventDto
        {
            NumeroOdf = TestUtils.ObjectMother.Ints[index],
            NumeroOperacao = TestUtils.ObjectMother.Ints[index].ToString(),
        };
        return operacaoRemovidaDto;
    }

    private OperacaoDto GetOperacaoDto(int index)
    {
        var operacaoDto = new OperacaoDto
        {
            IdOperacao = TestUtils.ObjectMother.Ints[index],
            Operacao = TestUtils.ObjectMother.Strings[index],
            IdEmpresa = TestUtils.ObjectMother.Ints[index],
            Odf = TestUtils.ObjectMother.Ints[index]
        };
        return operacaoDto;
    }
    private void MockarRetornoGetByNumeroOdfENumeroOperacao(Mocker mocker, int index)
    {
        mocker.OperacaoService
            .GetByNumeroOdfENumeroOperacao(TestUtils.ObjectMother.Ints[index], TestUtils.ObjectMother.Ints[index].ToString())
            .Returns(GetOperacaoDto(index));
    }
    private void MockarRetornoGetApontamentoOperacaoByLegacyIdOperacao(Mocker mocker, int index, OperacaoSaldoDto expectedOperacaoSaldoDto)
    {
        var apontamentoOperacaoOutput = new ApontamentoOperacaoOutput
        {
            OperacaoSaldo = new List<OperacaoSaldoDto>
            {
                expectedOperacaoSaldoDto
            }
        };
        mocker.OperacaoService
            .GetApontamentoOperacaoByLegacyIdOperacao(TestUtils.ObjectMother.Ints[index])
            .Returns(apontamentoOperacaoOutput);
    }

    private OperacaoSaldoDto GetOperacaoSaldoDto(int index)
    {
        var operacaoSaldoDto = new OperacaoSaldoDto
        {
            SaldoUnidadePadrao = TestUtils.ObjectMother.Ints[index],
            QuantidadeOperacao999UnidadePadrao = TestUtils.ObjectMother.Ints[index],
            QuantidadeProduzidaOperacao999UnidadePadrao = TestUtils.ObjectMother.Ints[index],
            SaldoOperacaoToleranciaMaximoUnidadePadrao = TestUtils.ObjectMother.Ints[index],
            SaldoOperacaoToleranciaMinimoUnidadePadrao = TestUtils.ObjectMother.Ints[index],
            QuantidadeMaximaEncerrarOdfOperacao999UnidadePadrao = TestUtils.ObjectMother.Ints[index],
            QuantidadeMinimaEncerrarOdfOperacao999UnidadePadrao = TestUtils.ObjectMother.Ints[index],
            Saldo = TestUtils.ObjectMother.Ints[index],
            SaldoOperacaoToleranciaMaximo = TestUtils.ObjectMother.Ints[index],
            SaldoOperacaoToleranciaMinimo = TestUtils.ObjectMother.Ints[index],
            QuantidadeOperacao999 = TestUtils.ObjectMother.Ints[index],
            QuantidadeMaximaEncerrarOdfOperacao999 = TestUtils.ObjectMother.Ints[index],
            QuantidadeMinimaEncerrarOdfOperacao999 = TestUtils.ObjectMother.Ints[index],
            QuantidadeProduzidaOperacao999 = TestUtils.ObjectMother.Ints[index],
            PrimeiraOperacaoOdf = false,
            DivideNaConversao = false,
            Tolerancia = TestUtils.ObjectMother.Ints[index],
            Unidade = TestUtils.ObjectMother.Strings[index],
            Fator = TestUtils.ObjectMother.Ints[index],
            QuantidadeProduzidaOpSecundaria = TestUtils.ObjectMother.Ints[index]
        };
        return operacaoSaldoDto;
    }
}