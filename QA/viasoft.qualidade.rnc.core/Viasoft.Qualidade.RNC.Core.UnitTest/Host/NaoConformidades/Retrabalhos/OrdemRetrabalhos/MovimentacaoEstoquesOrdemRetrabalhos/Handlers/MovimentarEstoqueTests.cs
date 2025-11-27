using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.PushNotifications.Abstractions.Contracts;
using Viasoft.Qualidade.RNC.Core.Domain.MovimentacaoEstoques.UpdateNotifications;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalho.Commands;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoqueLocais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalhos.Handlers;

public class MovimentarEstoqueTests : MovimentacaoEstoqueHandlerTest
{
    [Fact(DisplayName = "Se erro ao movimentar estoque, deve salvar mensagem retorno")]
    public async Task MovimentarEstoqueTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new MovimentarEstoqueItemMessage
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IsEstorno = false
        };
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        MockGetAgregacaoReturn(mocker, agregacao);
        
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Quantidade = 500,
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0],
            IdEstoqueLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            MovimentacaoEstoqueMensagemRetorno = ""
        };
        await mocker.OrdemRetrabalhoNaoConformidades.InsertAsync(ordemRetrabalhoNaoConformidade, true);
        
        mocker.MovimentacaoEstoqueOrdemRetrabalhoService
            .MovimentarEstoqueLista(agregacao.NaoConformidade, Arg.Is<OrdemRetrabalhoNaoConformidade>(e => e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)))
            .Returns(new MovimentarEstoqueListaOutput
            {
                Success = false,
                Message = "Deu erro"
            });
        mocker.OrdemRetrabalhoService.EstornarOrdemRetrabalho(agregacao, 
            Arg.Is<OrdemRetrabalhoNaoConformidade>(e => e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)), false).Returns(new OrdemRetrabalhoNaoConformidadeOutput
        {
            Success = false
        });
        
        var expectedResult = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Quantidade = 500,
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0],
            MovimentacaoEstoqueMensagemRetorno = "Deu erro",
            IdEstoqueLocalDestino = TestUtils.ObjectMother.Guids[0]
        };
        //Act
        await handler.Handle(message);
        //Assert
        var ordemRetrabalhoNaoConformidadeResult =
            await mocker.OrdemRetrabalhoNaoConformidades.FindAsync(TestUtils.ObjectMother.Guids[0]);
        ordemRetrabalhoNaoConformidadeResult.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
    
    [Fact(DisplayName = "Se sucesso ao movimentar estoque, deve notificar o frontEnd com mensagem de sucesso")]
    public async Task MovimentarEstoqueTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new MovimentarEstoqueItemMessage
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IsEstorno = false,
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
            .MovimentarEstoqueLista(agregacao.NaoConformidade, 
                Arg.Is<OrdemRetrabalhoNaoConformidade>(e => e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)))
            .Returns(new MovimentarEstoqueListaOutput
            {
                Success = true,
                Message = "Deu certo",
                DtoRetorno = new ExternalMovimentarEstoqueItemResultado
                {
                    LegacyIdEstoqueLocalDestino = TestUtils.ObjectMother.Ints[0]
                }
            });
        MockGetAgregacaoReturn(mocker, agregacao);
        

        mocker.EstoqueLocalProvider.GetByLegacyId(TestUtils.ObjectMother.Ints[0]).Returns(new EstoqueLocal
        {
            Id = TestUtils.ObjectMother.Guids[0]
        });
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
    public async Task MovimentarEstoqueTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new MovimentarEstoqueItemMessage
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IsEstorno = false,
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
            .MovimentarEstoqueLista(agregacao.NaoConformidade, Arg.Is<OrdemRetrabalhoNaoConformidade>(e => 
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
        mocker.OrdemRetrabalhoService.EstornarOrdemRetrabalho(agregacao, 
            Arg.Is<OrdemRetrabalhoNaoConformidade>(e => e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)), false).Returns(new OrdemRetrabalhoNaoConformidadeOutput
        {
            Success = false
        });
        //Act
        await handler.Handle(message);
        //Assert
        await mocker.PushNotification.Received(1)
            .SendUpdateAsync(Arg.Is<MovimentacaoEstoqueProcessadaUpdateNotification>(e => 
                e.IsEquivalentTo(expectedResult)));
    }
    
    [Fact(DisplayName = "Se falha ao movimentar estoque, deve notificar o usuário a com mensagem de erro")]
    public async Task MovimentarEstoqueTest4()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new MovimentarEstoqueItemMessage
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IsEstorno = false,
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
            .MovimentarEstoqueLista(agregacao.NaoConformidade, Arg.Is<OrdemRetrabalhoNaoConformidade>(e => 
                e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)))
            .Returns(new MovimentarEstoqueListaOutput
            {
                Success = false,
                Message = "Deu erro"
            });
        MockGetAgregacaoReturn(mocker, agregacao);
        mocker.OrdemRetrabalhoService.EstornarOrdemRetrabalho(agregacao, 
            Arg.Is<OrdemRetrabalhoNaoConformidade>(e => e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)), false).Returns(new OrdemRetrabalhoNaoConformidadeOutput
        {
            Success = false
        });
        var expectedResult = new Payload
        {
            Header = $"Erro ao realizar movimentação de estoque, odf de retrabalho estornada.",
            Body = "Deu erro"
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
    [Fact(DisplayName = "Se falha ao movimentar estoque, deve estornar a ordem de retrabalho")]
    public async Task MovimentarEstoqueTest5()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new MovimentarEstoqueItemMessage
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IsEstorno = false,
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
            .MovimentarEstoqueLista(agregacao.NaoConformidade, Arg.Is<OrdemRetrabalhoNaoConformidade>(e => 
                e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)))
            .Returns(new MovimentarEstoqueListaOutput
            {
                Success = false,
                Message = "Deu erro"
            });
        MockGetAgregacaoReturn(mocker, agregacao);
        mocker.OrdemRetrabalhoService.EstornarOrdemRetrabalho(agregacao, 
            Arg.Is<OrdemRetrabalhoNaoConformidade>(e => e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)), false).Returns(new OrdemRetrabalhoNaoConformidadeOutput
        {
            Success = false
        });
        //Act
        await handler.Handle(message);
        //Assert
        await mocker.OrdemRetrabalhoService.Received(1).EstornarOrdemRetrabalho(agregacao, ordemRetrabalhoNaoConformidade, false);
    }
    
    [Fact(DisplayName = "Se sucesso ao estornar  odf de retrabalho, deve deletar ordem retrabalho nao conformidade")]
    public async Task MovimentarEstoqueTest6()
    {
        //Arrange
        var mocker = GetMocker();
        var handler = GetHandler(mocker);
        var message = new MovimentarEstoqueItemMessage
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IsEstorno = false,
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
            .MovimentarEstoqueLista(agregacao.NaoConformidade, Arg.Is<OrdemRetrabalhoNaoConformidade>(e => 
                e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)))
            .Returns(new MovimentarEstoqueListaOutput
            {
                Success = false,
                Message = "Deu erro"
            });
        MockGetAgregacaoReturn(mocker, agregacao);
        mocker.OrdemRetrabalhoService.EstornarOrdemRetrabalho(agregacao, 
            Arg.Is<OrdemRetrabalhoNaoConformidade>(e => e.IsEquivalentTo(ordemRetrabalhoNaoConformidade)), false).Returns(new OrdemRetrabalhoNaoConformidadeOutput
        {
            Success = true
        });
        //Act
        await handler.Handle(message);
        //Assert
        mocker.OrdemRetrabalhoNaoConformidades.Should().BeEmpty();
    }
}