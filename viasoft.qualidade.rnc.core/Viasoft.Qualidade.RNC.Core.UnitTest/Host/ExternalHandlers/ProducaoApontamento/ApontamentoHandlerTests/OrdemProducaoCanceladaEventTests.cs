using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.OrdemProducao.Dtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.OrdemProducao.Events;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Retrabalhos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.ExternalHandlers.ProducaoApontamento.ApontamentoHandlerTests;

public class OrdemProducaoCanceladaEventTests: ApontamentoHandlerTest
{
    [Fact(DisplayName = "Se ordem não for retrabalho, nada deve ser feito")]
    public async Task OrdemProducaoCanceladaEventTest()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        
        await InserirNaoConformidade(0);
        
        var ordemRetrabalho = GetOrdemRetrabalhoNaoConformidade(0);
        ordemRetrabalho.Status = StatusProducaoRetrabalho.Produzindo;
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalho);
        await UnitOfWork.CompleteAsync();

        var producaoCanceladaDto = GetProducaoCanceladaDto(0);
        producaoCanceladaDto.IsOrdemRetrabalho = false;
        
        var evento = new OrdemProducaoCanceladaEvent()
        {
             OrdemProducaoEventEventDto = producaoCanceladaDto
        };
        
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOrdemRetrabalhoResult = (await mocker.OrdemRetrabalhoNaoConformidades.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOrdemRetrabalhoResult.Should().Be(StatusProducaoRetrabalho.Produzindo);
    }
    
    [Fact(DisplayName = "Se ordem retrabalho não encontrada, nada deve ser feito")]
    public async Task OrdemProducaoCanceladaEventTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        
        await InserirNaoConformidade(0);
        
        var ordemRetrabalho = GetOrdemRetrabalhoNaoConformidade(0);
        ordemRetrabalho.Status = StatusProducaoRetrabalho.Produzindo;
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalho);
        await UnitOfWork.CompleteAsync();

        var producaoCanceladaDto = GetProducaoCanceladaDto(0);
        producaoCanceladaDto.IsOrdemRetrabalho = true;
        producaoCanceladaDto.NumeroOdf = 500;
        var evento = new OrdemProducaoCanceladaEvent()
        {
            OrdemProducaoEventEventDto = producaoCanceladaDto
        };
        
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOrdemRetrabalhoResult = (await mocker.OrdemRetrabalhoNaoConformidades.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOrdemRetrabalhoResult.Should().Be(StatusProducaoRetrabalho.Produzindo);
    }
    
    [Fact(DisplayName = "Se ordem retrabalho encontrada, deve alterar o status para Cancelada")]
    public async Task OrdemProducaoCanceladaEventTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        
        await InserirNaoConformidade(0);
        
        var ordemRetrabalho = GetOrdemRetrabalhoNaoConformidade(0);
        ordemRetrabalho.Status = StatusProducaoRetrabalho.Produzindo;
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalho);
        await UnitOfWork.CompleteAsync();

        var producaoCanceladaDto = GetProducaoCanceladaDto(0);
        var evento = new OrdemProducaoCanceladaEvent()
        {
            OrdemProducaoEventEventDto = producaoCanceladaDto
        };
        
        //Act
        await handler.Handle(evento);
        //Assert
        LimparTracker(mocker);
        var statusOrdemRetrabalhoResult = (await mocker.OrdemRetrabalhoNaoConformidades.FirstAsync(e => e.Id == TestUtils.ObjectMother.Guids[0])).Status;
        statusOrdemRetrabalhoResult.Should().Be(StatusProducaoRetrabalho.Cancelada);
    }
    
    private OrdemProducaoCanceladaEventDto GetProducaoCanceladaDto(int index)
    {
        var producaoCanceladaDto = new OrdemProducaoCanceladaEventDto
        {
            NumeroOdf = TestUtils.ObjectMother.Ints[index],
            IsOrdemRetrabalho = true
        };
        return producaoCanceladaDto;
    }
}