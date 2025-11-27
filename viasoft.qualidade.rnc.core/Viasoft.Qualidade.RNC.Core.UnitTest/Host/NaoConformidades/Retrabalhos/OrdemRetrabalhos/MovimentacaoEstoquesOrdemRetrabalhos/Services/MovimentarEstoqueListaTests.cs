using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.Legacy;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Dtos;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.MovimentacaoEstoquesOrdemRetrabalhos.Services;

public class MovimentarEstoqueListaTests : MovimentacaoEstoqueServiceTest
{
    [Fact(DisplayName = "Se falhar ao movimentar estoque, deve logar o erro")]
    public async Task MovimentarEstoqueItemTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            CodigoArmazem = TestUtils.ObjectMother.Ints[0].ToString(),
            DataFabricacao = TestUtils.ObjectMother.Datas[0],
            DataValidade = TestUtils.ObjectMother.Datas[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0]
        };
        
        var expectedInput = new MovimentarEstoqueListaInput
        {
            Quantidade = ordemRetrabalhoNaoConformidade.Quantidade,
            IdLocalDestino = ordemRetrabalhoNaoConformidade.IdLocalDestino,
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            CodigoArmazem = ordemRetrabalhoNaoConformidade.CodigoArmazem,
            DataFabricacao = ordemRetrabalhoNaoConformidade.DataFabricacao,
            DataValidade = ordemRetrabalhoNaoConformidade.DataValidade,
            IdProduto = agregacao.NaoConformidade.IdProduto,
            NumeroLote = agregacao.NaoConformidade.NumeroLote,
            NumeroOdfOrigem = agregacao.NaoConformidade.NumeroOdf.Value,
            NumeroPedido = agregacao.NaoConformidade.NumeroPedido,
            NumeroOdfDestino = ordemRetrabalhoNaoConformidade.NumeroOdfRetrabalho
        };

        var externalMovimentarEstoqueListaInput = GetExternalMovimentarEstoqueListaInput(0);

        mocker.MovimentacaoEstoqueAclService
            .GetExternalMovimentarEstoqueListaInput(Arg.Is<MovimentarEstoqueListaInput>(e => e.IsEquivalentTo(expectedInput)))
            .Returns(externalMovimentarEstoqueListaInput);
        
        var externalMovimentarEstoqueItemOutput = new ExternalMovimentarEstoqueItemOutput
        {
            Resultado = null,
            Error = new KorpErro()
        };
        mocker.ExternalMovimentacaoService.MovimentarEstoqueLista(externalMovimentarEstoqueListaInput)
            .Returns(externalMovimentarEstoqueItemOutput);

        mocker.MovimentacaoEstoqueAclService.GetMovimentarEstoqueListaOutput(externalMovimentarEstoqueItemOutput)
            .Returns(new MovimentarEstoqueListaOutput
            {
                Success = false,
                Message = "Deu erro"
            });
        
        //Act
        await service.MovimentarEstoqueLista(agregacao.NaoConformidade, ordemRetrabalhoNaoConformidade);
        //Assert
        mocker.Logger.Received(1).LogError($"Erro ao realizar movimentação de estoque para o tenant {TestUtils.ObjectMother.Guids[0]} " +
                                           $"e environment {TestUtils.ObjectMother.Guids[0]}, mensagem de retorno: Deu erro");
    }
    
    [Fact(DisplayName = "Se sucesso ao movimentar estoque, deve retornar o dto de sucesso")]
    public async Task MovimentarEstoqueItemTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0]
        };
        
        var expectedInput = new MovimentarEstoqueListaInput
        {
            Quantidade = ordemRetrabalhoNaoConformidade.Quantidade,
            IdLocalDestino = ordemRetrabalhoNaoConformidade.IdLocalDestino,
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            CodigoArmazem = ordemRetrabalhoNaoConformidade.CodigoArmazem,
            DataFabricacao = ordemRetrabalhoNaoConformidade.DataFabricacao,
            DataValidade = ordemRetrabalhoNaoConformidade.DataValidade,
            IdProduto = agregacao.NaoConformidade.IdProduto,
            NumeroPedido = agregacao.NaoConformidade.NumeroPedido,
            NumeroLote = agregacao.NaoConformidade.NumeroLote,
            NumeroOdfOrigem = agregacao.NaoConformidade.NumeroOdf.Value,
            NumeroOdfDestino = ordemRetrabalhoNaoConformidade.NumeroOdfRetrabalho
        };
        
        var externalMovimentarEstoqueListaInput = GetExternalMovimentarEstoqueListaInput(0);

        mocker.MovimentacaoEstoqueAclService
            .GetExternalMovimentarEstoqueListaInput(Arg.Is<MovimentarEstoqueListaInput>(e => e.IsEquivalentTo(expectedInput)))
            .Returns(externalMovimentarEstoqueListaInput);
        
        var externalMovimentarEstoqueItemOutput = new ExternalMovimentarEstoqueItemOutput
        {
            Resultado = new List<ExternalMovimentarEstoqueItemOutputResultado>(),
            Error = null
        };
        mocker.ExternalMovimentacaoService.MovimentarEstoqueLista(externalMovimentarEstoqueListaInput)
            .Returns(externalMovimentarEstoqueItemOutput);

        mocker.MovimentacaoEstoqueAclService.GetMovimentarEstoqueListaOutput(externalMovimentarEstoqueItemOutput)
            .Returns(new MovimentarEstoqueListaOutput
            {
                Success = true,
                DtoRetorno = new ExternalMovimentarEstoqueItemResultado
                {
                    LegacyIdEstoqueLocalDestino = TestUtils.ObjectMother.Ints[0],
                    CodigoLocalDestino = TestUtils.ObjectMother.Ints[0],
                }            
            });
        
        //Act
        var result = await service.MovimentarEstoqueLista(agregacao.NaoConformidade, ordemRetrabalhoNaoConformidade);
        //Assert
        result.Success.Should().BeTrue();
        result.Message.Should().BeNull();
    }
    
    [Fact(DisplayName = "Se utiliza reserva de pedido, deve usar a odf de destino da ordem de producao como origem da movimentacao")]
    public async Task MovimentarEstoqueItemTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        mocker.LegacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque().Returns(true);

        var ordemProducao = new OrdemProducaoOutput
        {
            NumeroOdf = TestUtils.ObjectMother.Ints[0],
            NumeroOdfDestino = TestUtils.ObjectMother.Ints[1]
        };
        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], true).Returns(ordemProducao);
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0]
        };
        
        var expectedInput = new MovimentarEstoqueListaInput
        {
            Quantidade = ordemRetrabalhoNaoConformidade.Quantidade,
            IdLocalDestino = ordemRetrabalhoNaoConformidade.IdLocalDestino,
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            CodigoArmazem = ordemRetrabalhoNaoConformidade.CodigoArmazem,
            DataFabricacao = ordemRetrabalhoNaoConformidade.DataFabricacao,
            DataValidade = ordemRetrabalhoNaoConformidade.DataValidade,
            IdProduto = agregacao.NaoConformidade.IdProduto,
            NumeroPedido = agregacao.NaoConformidade.NumeroPedido,
            NumeroLote = agregacao.NaoConformidade.NumeroLote,
            NumeroOdfOrigem = ordemProducao.NumeroOdfDestino.Value,
            NumeroOdfDestino = ordemRetrabalhoNaoConformidade.NumeroOdfRetrabalho,
        };

        var externalMovimentarEstoqueListaInput = GetExternalMovimentarEstoqueListaInput(0);

        mocker.MovimentacaoEstoqueAclService
            .GetExternalMovimentarEstoqueListaInput(Arg.Is<MovimentarEstoqueListaInput>(e => e.IsEquivalentTo(expectedInput)))
            .Returns(externalMovimentarEstoqueListaInput);
        
        var externalMovimentarEstoqueItemOutput = new ExternalMovimentarEstoqueItemOutput
        {
            Resultado = null,
            Error = new KorpErro()
        };
        mocker.ExternalMovimentacaoService.MovimentarEstoqueLista(externalMovimentarEstoqueListaInput)
            .Returns(externalMovimentarEstoqueItemOutput);

        mocker.MovimentacaoEstoqueAclService.GetMovimentarEstoqueListaOutput(externalMovimentarEstoqueItemOutput)
            .Returns(new MovimentarEstoqueListaOutput
            {
                Success = true,
                DtoRetorno = new ExternalMovimentarEstoqueItemResultado
                {
                    LegacyIdEstoqueLocalDestino = TestUtils.ObjectMother.Ints[0],
                    CodigoLocalDestino = TestUtils.ObjectMother.Ints[0],
                }            
            });
        
        //Act
        await service.MovimentarEstoqueLista(agregacao.NaoConformidade, ordemRetrabalhoNaoConformidade);
        //Assert
        await mocker.MovimentacaoEstoqueAclService.Received(1).GetExternalMovimentarEstoqueListaInput(Arg.Is<MovimentarEstoqueListaInput>(e =>
            e.IsEquivalentTo(expectedInput)));
    }
    
    protected ExternalMovimentarEstoqueListaInput GetExternalMovimentarEstoqueListaInput(int index)
    {
        var externalMovimentarEstoqueListaInput = new ExternalMovimentarEstoqueListaInput
        {
            Itens = new List<ExternalMovimentarEstoqueItemInput>
            {
                new ExternalMovimentarEstoqueItemInput
                {
                    Lotes = new List<ExternalMovimentarEstoqueLoteInput>
                    {
                        new ExternalMovimentarEstoqueLoteInput()
                        {
                            Documento = $"Transferência de estoque do local 1 para o local 43",
                            Quantidade = TestUtils.ObjectMother.Ints[index],
                            DataFabricacao = TestUtils.ObjectMother.Datas[index].AddDateMask(),
                            PedidoVendaDestino = TestUtils.ObjectMother.Ints[index].ToString(),
                            PedidoVendaOrigem = TestUtils.ObjectMother.Ints[index].ToString(),
                            CodigoProduto = TestUtils.ObjectMother.Ints[index].ToString(),
                            OdfOrigem = TestUtils.ObjectMother.Ints[index],
                            OdfDestino = TestUtils.ObjectMother.Ints[index],
                            DataValidade = TestUtils.ObjectMother.Datas[index].AddDateMask(),
                            LoteOrigem = TestUtils.ObjectMother.Ints[index].ToString(),
                            LoteDestino = TestUtils.ObjectMother.Ints[index].ToString(),
                            CodigoLocalOrigem = TestUtils.ObjectMother.Ints[index],
                            CodigoLocalDestino = TestUtils.ObjectMother.Ints[index],
                            IdEmpresa = TestUtils.ObjectMother.Ints[index],
                            PesoBruto = 0,
                            PesoLiquido = 0,
                            CodigoArmazemDestino = TestUtils.ObjectMother.Ints[index].ToString(),
                            CodigoArmazemOrigem = TestUtils.ObjectMother.Ints[index].ToString(),
                            CodigoDeBarras = "",
                            IdEstoqueLocal = null,
                            PickingItemLoteId = "",
                            TransferindoParaLocalRetrabalho = true
                        }
                    }
                }
            }
        };
        return externalMovimentarEstoqueListaInput;
    }

}