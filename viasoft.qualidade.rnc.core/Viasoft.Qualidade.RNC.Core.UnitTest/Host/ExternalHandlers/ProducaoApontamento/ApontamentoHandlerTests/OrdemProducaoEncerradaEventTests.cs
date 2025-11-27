using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.OrdemProducao.Dtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.OrdemProducao.Events;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalHandlers.ProducaoApontamento.ApontamentoHandlerTests;

public class OrdemProducaoEncerradaEventTests: ApontamentoHandlerTest
{
    [Fact(DisplayName = "Se ordem não for retrabalho, nada deve ser feito")]
    public async Task OrdemProducaoEncerradaEventTest()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        
        await InserirNaoConformidade(0);
        
        var ordemRetrabalho = GetOrdemRetrabalhoNaoConformidade(0);
        ordemRetrabalho.Status = StatusProducaoRetrabalho.Aberta;
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalho);
        await UnitOfWork.CompleteAsync();

        var producaoEncerradaDto = GetProducaoEncerradaDto(0);
        producaoEncerradaDto.IsOrdemRetrabalho = false;
        
        var evento = new OrdemProducaoEncerradaEvent()
        {
            OrdemProducaoEventEventDto = producaoEncerradaDto
        };
        
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOrdemRetrabalhoResult = (await mocker.OrdemRetrabalhoNaoConformidades.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOrdemRetrabalhoResult.Should().Be(StatusProducaoRetrabalho.Aberta);
    }
    
    [Fact(DisplayName = "Se ordem retrabalho não encontrada, nada deve ser feito")]
    public async Task OrdemProducaoEncerradaEventTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        
        await InserirNaoConformidade(0);
        
        var ordemRetrabalho = GetOrdemRetrabalhoNaoConformidade(0);
        ordemRetrabalho.Status = StatusProducaoRetrabalho.Aberta;
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalho);
        await UnitOfWork.CompleteAsync();

        var producaoEncerradaDto = GetProducaoEncerradaDto(0);
        producaoEncerradaDto.IsOrdemRetrabalho = true;
        producaoEncerradaDto.NumeroOdf = 500;
        var evento = new OrdemProducaoEncerradaEvent()
        {
            OrdemProducaoEventEventDto = producaoEncerradaDto
        };
        
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOrdemRetrabalhoResult = (await mocker.OrdemRetrabalhoNaoConformidades.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOrdemRetrabalhoResult.Should().Be(StatusProducaoRetrabalho.Aberta);
    }
    
    [Fact(DisplayName = "Se ordem retrabalho encontrada, deve alterar o status para Encerrada")]
    public async Task OrdemProducaoEncerradaEventTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        
        await InserirNaoConformidade(0);
        
        var ordemRetrabalho = GetOrdemRetrabalhoNaoConformidade(0);
        ordemRetrabalho.Status = StatusProducaoRetrabalho.Aberta;
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalho);
        await UnitOfWork.CompleteAsync();

        var producaoEncerradaDto = GetProducaoEncerradaDto(0);
        var evento = new OrdemProducaoEncerradaEvent()
        {
            OrdemProducaoEventEventDto = producaoEncerradaDto
        };
        
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOrdemRetrabalhoResult = (await mocker.OrdemRetrabalhoNaoConformidades.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOrdemRetrabalhoResult.Should().Be(StatusProducaoRetrabalho.Encerrada);
    }
    
    private OrdemProducaoEncerradaEventDto GetProducaoEncerradaDto(int index)
    {
        var producaoEncerradaDto = new OrdemProducaoEncerradaEventDto
        {
            NumeroOdf = TestUtils.ObjectMother.Ints[index],
            IsOrdemRetrabalho = true
        };
        return producaoEncerradaDto;
    }
}