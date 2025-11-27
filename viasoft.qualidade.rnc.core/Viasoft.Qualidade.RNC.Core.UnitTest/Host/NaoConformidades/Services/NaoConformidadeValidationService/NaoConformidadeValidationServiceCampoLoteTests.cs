using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntradaRateioLote.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntradaRateioLote.Providers;
using Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services.NaoConformidadeValidationService;

public class NaoConformidadeValidationServiceCampoLoteTests : NaoConformidadeValidationServiceTest
{
    [Fact(DisplayName = "Se origem não for inspeção de entrada, deve retornar Ok")]
    public async Task ValidarCampoLoteTest()
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
        var result = await service.ValidarCampoLote(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.Ok);
    }
    [Fact(DisplayName = "Se origem for inspeção de entrada e já tiver buscado itens da nota fiscal, não deve busca-los")]
    public async Task ValidarCampoLoteTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InspecaoEntrada,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            NumeroLote = TestUtils.ObjectMother.Ints[0].ToString()
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
        await service.ValidarCampoProduto(input);
        var result = await service.ValidarCampoLote(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.Ok);
        await mocker.ItemNotaFiscalEntradaProvider.Received(1).GetList(
            Arg.Is<GetListItemNotaFiscalInput>(e => e.IdNotaFiscal == input.IdNotaFiscal));
    }
    
    [Fact(DisplayName = "Se origem for inspeção de entrada e lote conta em nota, deve retornar Ok")]
    public async Task ValidarCampoLoteTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InspecaoEntrada,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            NumeroLote = "1"
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
                        Lote = "1",
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
        var result = await service.ValidarCampoLote(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.Ok);
    }
    
    [Fact(DisplayName = "Se origem for inspeção de entrada, lote nao conta em nota e não há rateio do lote, deve retornar lote invalido")]
    public async Task ValidarCampoLoteTest4()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InspecaoEntrada,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[1],
            NumeroLote = "3"
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
        var expectedAdvancedFilter = new JsonNetFilterRule
        {
            Condition = "AND",
            Rules = new List<JsonNetFilterRule>
            {
                new JsonNetFilterRule()
                {
                    Field = "IdNotaFiscal",
                    Operator = "equal",
                    Type = "string",
                    Value = TestUtils.ObjectMother.Guids[0]
                },
                new JsonNetFilterRule()
                {
                    Field = "Lote",
                    Operator = "equal",
                    Type = "string",
                    Value = "3"
                }
            }
        };
        var expectedResult = new GetListItemNotaFiscalRateioLoteInput
        {
            SkipCount = 0,
            MaxResultCount = 50,
            AdvancedFilter = JsonConvert.SerializeObject(expectedAdvancedFilter)
        };
        mocker.ItemNotaFiscalEntradaRateioLoteProvider
            .GetList(Arg.Is<GetListItemNotaFiscalRateioLoteInput>(e => e.IsEquivalentTo(expectedResult)))
            .Returns(new PagedResultDto<ItemNotaFiscalEntradaRateioLoteOutput>
            {
                Items = new List<ItemNotaFiscalEntradaRateioLoteOutput>(),
                TotalCount = 0
            });
        //Act
        var result = await service.ValidarCampoLote(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.LoteInvalido);
    }
    [Fact(DisplayName = "Se origem for inspeção de entrada, lote nao conta em nota e há rateio do lote, deve retornar Ok")]
    public async Task ValidarCampoLoteTest5()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InspecaoEntrada,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[1],
            NumeroLote = "3"
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
        var expectedAdvancedFilter = new JsonNetFilterRule
        {
            Condition = "AND",
            Rules = new List<JsonNetFilterRule>
            {
                new JsonNetFilterRule()
                {
                    Field = "IdNotaFiscal",
                    Operator = "equal",
                    Type = "string",
                    Value = TestUtils.ObjectMother.Guids[0]
                },
                new JsonNetFilterRule()
                {
                    Field = "Lote",
                    Operator = "equal",
                    Type = "string",
                    Value = "3"
                }
            }
        };
        var expectedResult = new GetListItemNotaFiscalRateioLoteInput
        {
            SkipCount = 0,
            MaxResultCount = 50,
            AdvancedFilter = JsonConvert.SerializeObject(expectedAdvancedFilter)
        };
        mocker.ItemNotaFiscalEntradaRateioLoteProvider
            .GetList(Arg.Is<GetListItemNotaFiscalRateioLoteInput>(e => e.IsEquivalentTo(expectedResult)))
            .Returns(new PagedResultDto<ItemNotaFiscalEntradaRateioLoteOutput>
            {
                Items = new List<ItemNotaFiscalEntradaRateioLoteOutput>
                {
                    new ItemNotaFiscalEntradaRateioLoteOutput()
                },
                TotalCount = 1
            });
        //Act
        var result = await service.ValidarCampoLote(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.Ok);
    }
    [Fact(DisplayName = "Se origem for inspeção de entrada e não ouver numero lote, deve retornar lote invalido")]
    public async Task ValidarCampoLoteTest6()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InspecaoEntrada,
            IdNotaFiscal = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
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
        await service.ValidarCampoProduto(input);
        var result = await service.ValidarCampoLote(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.LoteInvalido);
    }
}