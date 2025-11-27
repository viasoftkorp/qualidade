using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.PedidoVendas;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.OrdemRetrabalhos.Services;

public class EstornarOrdemRetrabalhoAclServiceTests : OrdemRetrabalhoAclServiceTest
{
    
    [Fact(DisplayName = "Se utilizarReservaDePedidoNaLocalizacaoDeEstoque, numero odf venda deve ser igual ao numero odf destino da odf de retrabalho")]
    public async Task GerarOrdemRetrabalhoTest2()
    {
        //Arrange 
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacao.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Quantidade = 1,
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            MovimentacaoEstoqueMensagemRetorno = "",
        };
        var odfRetrabalho = new OrdemProducaoOutput()
        {
            NumeroOdf = TestUtils.ObjectMother.Ints[0],
            Quantidade = 1,
            IdProduto = TestUtils.ObjectMother.Guids[0],
            NumeroPedido = TestUtils.ObjectMother.Strings[0]
        };
        var ordemProducaoRetrabalho = new OrdemProducaoOutput
        {
            NumeroOdf = TestUtils.ObjectMother.Ints[0],
            Quantidade = 10,
            IdProduto = TestUtils.ObjectMother.Guids[0],
            NumeroPedido = TestUtils.ObjectMother.Strings[0],
            NumeroOdfDestino = TestUtils.ObjectMother.Ints[1]
        };
        ordemProducaoRetrabalho.NumeroPedido = "0";
        
        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], false).Returns(ordemProducaoRetrabalho);

        await mocker.ProdutosRepository.InsertAsync(new Produto
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0].ToString()
        }, true);

        mocker.LegacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque().Returns(true);
        var expectedResult = new ExternalEstornarOrdemRetrabalhoInput
        {
            Odf = TestUtils.ObjectMother.Ints[0],
            OdfVenda = ordemProducaoRetrabalho.NumeroOdfDestino.Value,
            Quantidade = odfRetrabalho.Quantidade,
            SaldoOdf = ordemProducaoRetrabalho.Quantidade,
            Motivo = "Estornada Ordem de Retrabalho pelo RNC",
            Situacao = "991",
            CodigoProduto = TestUtils.ObjectMother.Ints[0].ToString(),
            PedidoVenda = "991"
            
        };
        //Act
        var result = await service.GetExternalEstornarOrdemRetrabalhoInput(TestUtils.ObjectMother.Ints[0], ordemRetrabalhoNaoConformidade);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}