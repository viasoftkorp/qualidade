using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.
    EstornarOrdemRetrabalhoValidatorServicesTests;

public class ValidateHistoricoApontamentoTests : EstornarOrdemRetrabalhoValidatorServiceTest
{
    [Fact(DisplayName = "Se não for encontrado apontamento, deve retornar ok")]
    public async Task ValidateHistoricoApontamentoTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();

        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0]
        };

        await mocker.OrdemRetrabalhoNaoConformidadeRepository.InsertAsync(ordemRetrabalhoNaoConformidade, true);
        
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
                    Value = TestUtils.ObjectMother.Ints[0]
                }
            }
        };
        mocker.OperacaoService.ValidarOdfPossuiApontamento(TestUtils.ObjectMother.Ints[0]).Returns(false);

        //Act
        var result = await service
            .ValidateHistoricoApontamento()
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(EstornarOrdemRetrabalhoValidationResult.Ok);
    }

    [Fact(DisplayName = "Se for encontrado apontamento, deve retornar odfRetrabalhoJaApontada")]
    public async Task ValidateHistoricoApontamentoTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();

        var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0]
        };
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
                    Value = TestUtils.ObjectMother.Ints[0]
                }
            }
        };
        await mocker.OrdemRetrabalhoNaoConformidadeRepository.InsertAsync(ordemRetrabalhoNaoConformidade, true);

        mocker.OperacaoService.ValidarOdfPossuiApontamento(TestUtils.ObjectMother.Ints[0]).Returns(true);
        //Act
        var result = await service
            .ValidateHistoricoApontamento()
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(EstornarOrdemRetrabalhoValidationResult.OdfRetrabalhoJaApontada);
    }
}