using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Domain.Configuracoes.Gerais;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.EstoqueLocais.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.
    GerarOrdemRetrabalhoValidatorServicesTests;

public class ValidateLoteTests : GerarOrdemRetrabalhoValidatorServiceTest
{
    [Fact(DisplayName = "Se não tiver numero do lote, deve retornar LoteObrigatorio")]
    public async Task ValidateOdf1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.NumeroLote = null;
        var ordemRetrabalhoInput = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0]
        };
        //Act
        var result = await service
            .ValidateLote(ordemRetrabalhoInput)
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.LoteObrigatorio);
    }
    
    [Fact(DisplayName = "Se houver estoque local para o lote fornecido, deve retornar Ok")]
    public async Task ValidateOdf3()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.NumeroLote = TestUtils.ObjectMother.Ints[0].ToString();
        
        var ordemRetrabalhoInput = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdEstoqueLocalOrigem = TestUtils.ObjectMother.Guids[0]
        };
        var estoqueLocalOrigem = new EstoqueLocal
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdLocal = TestUtils.ObjectMother.Guids[0]
        };
        var expectedAdvancedFilter = new JsonNetFilterRule
        {
            Condition = "AND",
            Rules = new List<JsonNetFilterRule>
            {
                new JsonNetFilterRule
                {
                    Field = "IdProduto",
                    Type = "string",
                    Operator = "equal",
                    Value = agregacaoNaoConformidade.NaoConformidade.IdProduto
                },
                new JsonNetFilterRule
                {
                    Field = "Lote",
                    Type = "string",
                    Operator = "equal",
                    Value = agregacaoNaoConformidade.NaoConformidade.NumeroLote
                },
                new JsonNetFilterRule
                {
                    Field = "IdLocal",
                    Type = "string",
                    Operator = "equal",
                    Value = estoqueLocalOrigem.IdLocal
                },
            }
        };
        
        await mocker.ConfiguracaoGerais.InsertAsync(new ConfiguracaoGeral(), true);

        //Act
        var result = await service
            .ValidateLote(ordemRetrabalhoInput)
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.Ok);
    }
    
    [Fact(DisplayName = "Se considerar apenas saldo apontado for verdadeiro e não houve odf apontada, deve retornar OdfNaoApontada")]
    public async Task ValidateOdf4()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.NumeroLote = TestUtils.ObjectMother.Ints[0].ToString();
        
        var ordemRetrabalhoInput = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdEstoqueLocalOrigem = TestUtils.ObjectMother.Guids[0]
        };
        var estoqueLocalOrigem = new EstoqueLocal
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdLocal = TestUtils.ObjectMother.Guids[0]
        };
        
        await mocker.ConfiguracaoGerais.InsertAsync(new ConfiguracaoGeral
        {
            ConsiderarApenasSaldoApontado = true
        }, true);
        
        var expectedAdvancedFilter = new JsonNetFilterRule
        {
            Condition = "AND",
            Rules = new List<JsonNetFilterRule>
            {
                new JsonNetFilterRule
                {
                    Field = "Odf",
                    Type = "integer",
                    Operator = "equal",
                    Value = agregacaoNaoConformidade.NaoConformidade.NumeroOdf
                },
                new JsonNetFilterRule
                {
                    Field = "NumeroLote",
                    Type = "string",
                    Operator = "equal",
                    Value = agregacaoNaoConformidade.NaoConformidade.NumeroLote
                }
            }
        };
        mocker.OperacaoService
            .ValidarOdfPossuiApontamento(agregacaoNaoConformidade.NaoConformidade.NumeroOdf.Value)
            .Returns(false);

        //Act
        var result = await service
            .ValidateLote(ordemRetrabalhoInput)
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.OdfNaoApontada);
    }
    [Fact(DisplayName = "Se considerar apenas saldo apontado for verdadeiro e houver odf apontada, deve retornar Ok")]
    public async Task ValidateOdf5()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.NumeroLote = TestUtils.ObjectMother.Ints[0].ToString();
        
        var ordemRetrabalhoInput = new OrdemRetrabalhoInput
        {
            Quantidade = 1,
            IdLocalDestino = TestUtils.ObjectMother.Guids[0],
            IdEstoqueLocalOrigem = TestUtils.ObjectMother.Guids[0]
        };
        var estoqueLocalOrigem = new EstoqueLocal
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdLocal = TestUtils.ObjectMother.Guids[0]
        };
        
        await mocker.ConfiguracaoGerais.InsertAsync(new ConfiguracaoGeral
        {
            ConsiderarApenasSaldoApontado = true
        }, true);
       
        var expectedAdvancedFilter = new JsonNetFilterRule
        {
            Condition = "AND",
            Rules = new List<JsonNetFilterRule>
            {
                new JsonNetFilterRule
                {
                    Field = "Odf",
                    Type = "integer",
                    Operator = "equal",
                    Value = agregacaoNaoConformidade.NaoConformidade.NumeroOdf
                },
                new JsonNetFilterRule
                {
                    Field = "NumeroLote",
                    Type = "string",
                    Operator = "equal",
                    Value = agregacaoNaoConformidade.NaoConformidade.NumeroLote
                }
            }
        };
        mocker.OperacaoService
            .ValidarOdfPossuiApontamento(agregacaoNaoConformidade.NaoConformidade.NumeroOdf.Value)
            .Returns(true);
        
        //Act
        var result = await service
            .ValidateLote(ordemRetrabalhoInput)
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.Ok);
    }
}