using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.MovimentacaoEstoqueAclServiceTests;

public class GetExternalMovimentarEstoqueListaInputTests : MovimentacaoEstoqueAclServiceTest
{
    [Fact(DisplayName = "Se método chamado deve converter input para ExternalMovimentarEstoqueListaInput")]
    public async Task GetExternalMovimentarEstoqueListaInputTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var service = GetService(dependencies);

        var localOrigem = GetLocalOutput(0);
        var localDestino = GetLocalOutput(1);
        
        var locais = new List<LocalOutput>
        {
            localOrigem, 
            localDestino
        };
        MockarRetornoGetLocais(dependencies, locais.ConvertAll(e => e.Id), 
            new PagedResultDto<LocalOutput>
        {
            Items = locais,
            TotalCount = locais.Count
        });
        await MockarRetornoGetProdutos(dependencies, TestUtils.ObjectMother.GetProduto(0));
        var input = GetMovimentarEstoqueListaInput(0);
        input.IdLocalDestino = localDestino.Id;

        var expectedResult = GetExternalMovimentarEstoqueListaInput(0);
        expectedResult.Itens[0].Lotes[0].CodigoLocalDestino = localDestino.Codigo;
        expectedResult.Itens[0].Lotes[0].Documento = $"Transferência estoque, local {localOrigem.Codigo} para local {localDestino.Codigo}";
        // Act
        var result = await service.GetExternalMovimentarEstoqueListaInput(input);
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    private MovimentarEstoqueListaInput GetMovimentarEstoqueListaInput(int index)
    {
        var movimentarEstoqueListaInput = new MovimentarEstoqueListaInput
        {
            Quantidade = TestUtils.ObjectMother.Ints[index],
            IdLocalDestino = TestUtils.ObjectMother.Guids[index],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[index],
            DataFabricacao = TestUtils.ObjectMother.Datas[index],
            DataValidade = TestUtils.ObjectMother.Datas[index],
            NumeroOdfOrigem = TestUtils.ObjectMother.Ints[index],
            NumeroPedido = TestUtils.ObjectMother.Ints[index].ToString(),
            CodigoArmazem = TestUtils.ObjectMother.Ints[index].ToString(),
            IdProduto = TestUtils.ObjectMother.Guids[index],
            NumeroLote = TestUtils.ObjectMother.Ints[index].ToString(),
            NumeroOdfDestino = TestUtils.ObjectMother.Ints[index]
        };
        return movimentarEstoqueListaInput;
    }
}