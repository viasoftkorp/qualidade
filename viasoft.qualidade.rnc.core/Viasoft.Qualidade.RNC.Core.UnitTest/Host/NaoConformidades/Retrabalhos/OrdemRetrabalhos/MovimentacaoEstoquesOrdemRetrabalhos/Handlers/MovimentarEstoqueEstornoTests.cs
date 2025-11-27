using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.PushNotifications.Abstractions.Contracts;
using Viasoft.Qualidade.RNC.Core.Domain.MovimentacaoEstoques.UpdateNotifications;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Commands;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalhos.Handlers;

public class MovimentarEstoqueEstornoTests : MovimentacaoEstoqueHandlerTest
{
    [Fact(DisplayName = "Se sucesso ao movimentar estoque, deve notificar o frontEnd com mensagem de sucesso")]
    public async Task MovimentarEstoqueEstornoTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new MovimentarEstoqueItemMessage
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IsEstorno = true,
        };
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Quantidade = 500,
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0],
            MovimentacaoEstoqueMensagemRetorno = ""
        };
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalhoNaoConformidade, true);        
        mocker.MovimentacaoEstoqueOrdemRetrabalhoService
            .EstornarMovimentacaoEstoqueLista(agregacao.NaoConformidade, Arg.Is<OrdemRetrabalhoNaoConformidade>(e => 
                e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)))
            .Returns(new MovimentarEstoqueListaOutput
            {
                Success = true,
                Message = "Deu certo"
            });
        MockGetAgregacaoReturn(mocker, agregacao);
        var expectedResult = new MovimentacaoEstoqueProcessadaUpdateNotification
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Success = true,
            Message = null
        };
        //Act
        await handler.Handle(message);
        //Assert
        await mocker.PushNotification.Received(1)
            .SendUpdateAsync(Arg.Is<MovimentacaoEstoqueProcessadaUpdateNotification>(e => 
                e.IsEquivalentTo(expectedResult)));
    }
    
    [Fact(DisplayName = "Se falha ao movimentar estoque, deve notificar o frontEnd com mensagem de erro")]
    public async Task MovimentarEstoqueEstornoTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new MovimentarEstoqueItemMessage
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IsEstorno = true,
        };
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Quantidade = 500,
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0],
            MovimentacaoEstoqueMensagemRetorno = ""
        };
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalhoNaoConformidade, true);   
        mocker.MovimentacaoEstoqueOrdemRetrabalhoService
            .EstornarMovimentacaoEstoqueLista(agregacao.NaoConformidade, Arg.Is<OrdemRetrabalhoNaoConformidade>(e => 
                e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)))
            .Returns(new MovimentarEstoqueListaOutput
            {
                Success = false,
                Message = "Deu erro"
            });
        MockGetAgregacaoReturn(mocker, agregacao);
        var expectedResult = new MovimentacaoEstoqueProcessadaUpdateNotification
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Success = false,
            Message = "Deu erro"
        };
        //Act
        await handler.Handle(message);
        //Assert
        await mocker.PushNotification.Received(1)
            .SendUpdateAsync(Arg.Is<MovimentacaoEstoqueProcessadaUpdateNotification>(e => 
                e.IsEquivalentTo(expectedResult)));
    }
    
    [Fact(DisplayName = "Se falha ao movimentar estoque, deve notificar o usuário a com mensagem de erro")]
    public async Task MovimentarEstoqueEstornoTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Quantidade = 500,
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0],
            MovimentacaoEstoqueMensagemRetorno = ""
        };
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalhoNaoConformidade, true);   
        mocker.MovimentacaoEstoqueOrdemRetrabalhoService
            .EstornarMovimentacaoEstoqueLista(agregacao.NaoConformidade, Arg.Is<OrdemRetrabalhoNaoConformidade>(e =>
                e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)))
            .Returns(new MovimentarEstoqueListaOutput
            {
                Success = false,
                Message = "Deu erro"
            });
        MockGetAgregacaoReturn(mocker, agregacao);
        var expectedResult = new Payload
        {
            Header = $"Erro ao realizar movimentação de estoque",
            Body = "Deu erro"
        };
        var message = new MovimentarEstoqueItemMessage
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IsEstorno = true,
        };
        //Act
        await handler.Handle(message);
        //Assert
        await mocker.PushNotification.Received(1)
            .SendAsync(Arg.Is<Payload>(e => e.IsEquivalentTo(expectedResult, new List<string>
            {
                nameof(Payload.Data)
            })), TestUtils.ObjectMother.Guids[0], true);
    }
    
    
    
    [Fact(DisplayName = "Se sucesso ao estornar odf de retrabalho, deve deletar odf de retrabalho não conformidade")]
    public async Task GerarOrdemRetrabalhoTest3()
    {
        //Arrange 
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.CreatorId = TestUtils.ObjectMother.Guids[0];
        agregacao.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        MockGetAgregacaoReturn(mocker, agregacao);
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = 1,
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            MovimentacaoEstoqueMensagemRetorno = ""
        };
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalhoNaoConformidade, true);
        var message = new MovimentarEstoqueItemMessage
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IsEstorno = true,
        };
        mocker.MovimentacaoEstoqueOrdemRetrabalhoService
            .EstornarMovimentacaoEstoqueLista(agregacao.NaoConformidade, Arg.Is<OrdemRetrabalhoNaoConformidade>(e =>
                e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)))
            .Returns(new MovimentarEstoqueListaOutput
            {
                Success = true
            });
        //Act
        await handler.Handle(message);

        //Assert
        mocker.OrdemRetrabalhoNaoConformidades.Should().BeEmpty();
    }
}