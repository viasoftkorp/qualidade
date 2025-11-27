using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Host.EstoqueLocais.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.
    GerarOrdemRetrabalhoValidatorServicesTests;

public class ValidateQuantidadeTests : GerarOrdemRetrabalhoValidatorServiceTest
{
    [Fact(DisplayName =
        "Se utiliza reserva por perdido e quantidade reservada disponivel for menor que a quantidade digitada pelo usuário, deve retornar quantidadeInvalida")]
    public async Task ValidateOperacaoEngenhariaDuplicadaTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.NumeroPedido = TestUtils.ObjectMother.Strings[0];

        var input = new OrdemRetrabalhoInput
        {
            Quantidade = 5000,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdEstoqueLocalOrigem = TestUtils.ObjectMother.Guids[0]
        };
        mocker.LegacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque().Returns(true);
        mocker.EstoqueLocalAclService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(new EstoqueLocalOutput
        {
            Quantidade = 2000
        });
        
        //Act
        var result = await service
            .ValidateQuantidade(input)
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.QuantidadeInvalida);
    }
    
    [Fact(DisplayName =
        "Se utiliza reserva por perdido e quantidade reservada disponivel for maior que a quantidade digitada pelo usuário, deve retornar ok")]
    public async Task ValidateOperacaoEngenhariaDuplicadaTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.NumeroPedido = TestUtils.ObjectMother.Strings[0];

        var input = new OrdemRetrabalhoInput
        {
            Quantidade = 5000,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdEstoqueLocalOrigem = TestUtils.ObjectMother.Guids[0]
        };
        mocker.LegacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque().Returns(true);
       
        mocker.EstoqueLocalAclService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(new EstoqueLocalOutput
        {
            Quantidade = 10000
        });
        
        //Act
        var result = await service
            .ValidateQuantidade(input)
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.Ok);
    }
    
    [Fact(DisplayName =
        "Se não utiliza reserva por perdido e quantidade total disponivel for menor que a quantidade digitada pelo usuário, deve retornar quantidadeInvalida")]
    public async Task ValidateOperacaoEngenhariaDuplicadaTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.NumeroPedido = TestUtils.ObjectMother.Strings[0];
        var input = new OrdemRetrabalhoInput
        {
            Quantidade = 5000,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdEstoqueLocalOrigem = TestUtils.ObjectMother.Guids[0]
        };
        mocker.LegacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque().Returns(false);
        
        mocker.EstoqueLocalAclService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(new EstoqueLocalOutput
        {
            Quantidade = 2000
        });
        //Act
        var result = await service
            .ValidateQuantidade(input)
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.QuantidadeInvalida);
    }
    [Fact(DisplayName =
        "Se não utiliza reserva por perdido e quantidade total disponivel for maior que a quantidade digitada pelo usuário, deve retornar ok")]
    public async Task ValidateOperacaoEngenhariaDuplicadaTest4()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.NumeroPedido = TestUtils.ObjectMother.Strings[0];
        
        var input = new OrdemRetrabalhoInput
        {
            Quantidade = 5000,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdEstoqueLocalOrigem = TestUtils.ObjectMother.Guids[0]
        };
        mocker.LegacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque().Returns(false);

        mocker.EstoqueLocalAclService.GetById(TestUtils.ObjectMother.Guids[0]).Returns(new EstoqueLocalOutput
        {
            Quantidade = 10000
        });
        //Act
        var result = await service
            .ValidateQuantidade(input)
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.Ok);
    }
}