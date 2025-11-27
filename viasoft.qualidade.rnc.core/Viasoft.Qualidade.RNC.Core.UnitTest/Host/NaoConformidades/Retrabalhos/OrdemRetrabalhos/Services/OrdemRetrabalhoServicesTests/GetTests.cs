using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.OrdemRetrabalhoServicesTests;

public class GetTests : OrdemRetrabalhoServiceTest
{
    [Fact(DisplayName = "Se houver ordem de produção de retrabalho, deve retorna-la")]
    public async Task GetTest()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        await mocker.Repository.InsertAsync(new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            Quantidade = 10,
            CodigoArmazem = TestUtils.ObjectMother.Ints[0].ToString(),
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = 10,
            DataFabricacao = TestUtils.ObjectMother.Datas[0],
            IdEstoqueLocalDestino = TestUtils.ObjectMother.Guids[0],
            DataValidade = TestUtils.ObjectMother.Datas[0],
            MovimentacaoEstoqueMensagemRetorno = ""
        }, true);

        var expectedResult = new OrdemRetrabalhoNaoConformidadeOutput
        {
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            IdLocalOrigem = TestUtils.ObjectMother.Guids[0],
            Quantidade = 10,
            CodigoArmazem = TestUtils.ObjectMother.Ints[0].ToString(),
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = 10,
            DataFabricacao = TestUtils.ObjectMother.Datas[0],
            IdEstoqueLocalDestino = TestUtils.ObjectMother.Guids[0],
            DataValidade = TestUtils.ObjectMother.Datas[0],
        };
        // Act
        var result = await service.Get(TestUtils.ObjectMother.Guids[0]);
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact(DisplayName = "Se não houver ordem de produção de retrabalho, deve retornar null")]
    public async Task GetTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        // Act
        var result = await service.Get(TestUtils.ObjectMother.Guids[0]);
        result.Should().BeNull();
    }
}