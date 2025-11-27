using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Providers;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services.NaoConformidadeValidationService;

public class NaoConformidadeValidationServiceCampoProdutoTests : NaoConformidadeValidationServiceTest
{
    [Fact(DisplayName = "Se origem não for inspeção de entrada, deve retornar Ok")]
    public async Task ValidarCampoProdutoTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InpecaoSaida,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0]
        };
        //Act
        var result = await service.ValidarCampoProduto(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.Ok);
    }
    [Fact(DisplayName = "Se origem for inspeção de entrada e não tiver buscado itens da nota fiscal, deve busca-los")]
    public async Task ValidarCampoProdutoTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InspecaoEntrada,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0]
        };
        mocker.ItemNotaFiscalEntradaProvider.GetList(
            Arg.Is<GetListItemNotaFiscalInput>(e => e.IdNotaFiscal == input.IdNotaFiscal))
            .Returns(new PagedResultDto<ItemNotaFiscalEntradaOutput>
            {
                Items = new List<ItemNotaFiscalEntradaOutput>
                {
                    new ItemNotaFiscalEntradaOutput
                    {
                        Id = TestUtils.ObjectMother.Guids[0],
                        IdProduto = TestUtils.ObjectMother.Guids[0],
                        Lote = "1"
                    },
                    new ItemNotaFiscalEntradaOutput
                    {
                        Id = TestUtils.ObjectMother.Guids[1],
                        IdProduto = TestUtils.ObjectMother.Guids[1],
                        Lote = "2"
                    }
                },
                TotalCount = 2
            });
        //Act
        await service.ValidarCampoLote(input);
        var result = await service.ValidarCampoProduto(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.Ok);
        await mocker.ItemNotaFiscalEntradaProvider.Received(1).GetList(
            Arg.Is<GetListItemNotaFiscalInput>(e => e.IdNotaFiscal == input.IdNotaFiscal));
    }
    
    [Fact(DisplayName = "Se origem for inspeção de entrada e produto conta em nota, deve retornar Ok")]
    public async Task ValidarCampoProdutoTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InspecaoEntrada,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0]
        };
        mocker.ItemNotaFiscalEntradaProvider.GetList(
                Arg.Is<GetListItemNotaFiscalInput>(e => e.IdNotaFiscal == input.IdNotaFiscal))
            .Returns(new PagedResultDto<ItemNotaFiscalEntradaOutput>
            {
                Items = new List<ItemNotaFiscalEntradaOutput>
                {
                    new ItemNotaFiscalEntradaOutput
                    {
                        Id = TestUtils.ObjectMother.Guids[0],
                        IdProduto = TestUtils.ObjectMother.Guids[0],
                        Lote = "1"
                    },
                    new ItemNotaFiscalEntradaOutput
                    {
                        Id = TestUtils.ObjectMother.Guids[1],
                        IdProduto = TestUtils.ObjectMother.Guids[1],
                        Lote = "2"
                    }
                },
                TotalCount = 2
            });
        //Act
        var result = await service.ValidarCampoProduto(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.Ok);
    }
    [Fact(DisplayName = "Se origem for inspeção de entrada e produto não conta em nota, deve retornar produto invalido")]
    public async Task ValidarCampoProdutoTest4()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InspecaoEntrada,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[2]
        };
        mocker.ItemNotaFiscalEntradaProvider.GetList(
                Arg.Is<GetListItemNotaFiscalInput>(e => e.IdNotaFiscal == input.IdNotaFiscal))
            .Returns(new PagedResultDto<ItemNotaFiscalEntradaOutput>
            {
                Items = new List<ItemNotaFiscalEntradaOutput>
                {
                    new ItemNotaFiscalEntradaOutput
                    {
                        Id = TestUtils.ObjectMother.Guids[0],
                        IdProduto = TestUtils.ObjectMother.Guids[0],
                        Lote = "1"
                    },
                    new ItemNotaFiscalEntradaOutput
                    {
                        Id = TestUtils.ObjectMother.Guids[1],
                        IdProduto = TestUtils.ObjectMother.Guids[1],
                        Lote = "2"
                    }
                },
                TotalCount = 2
            });
        //Act
        var result = await service.ValidarCampoProduto(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.ProdutoInvalido);
    }
}