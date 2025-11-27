using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.GerarOrdemRetrabalhoValidatorServicesTests;

public class ValidateOperacaoEngenhariaDuplicadaTests : GerarOrdemRetrabalhoValidatorServiceTest
{
    [Fact(DisplayName = "Se não houver mais de um serviço com a mesma operação engenharia, deve retornar ok")]
    public async Task ValidateOperacaoEngenhariaDuplicadaTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();

        //Act
        var result = await service
            .ValidateOperacaoEngenhariaDuplicada()
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.Ok);
    }
    [Fact(DisplayName = "Se houver mais de um serviço com a mesma operação engenharia, deve retornar OperacaoEngenhariaDuplicada")]
    public async Task ValidateOperacaoEngenhariaDuplicadaTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        var servicosDuplicados = new List<ServicoNaoConformidade>
        {
            new ServicoNaoConformidade
            {
                Id = TestUtils.ObjectMother.Guids[0]
            },
            new ServicoNaoConformidade
            {
                Id = TestUtils.ObjectMother.Guids[1]
            }
        };
        agregacaoNaoConformidade.ServicoNaoConformidades.AddRange(servicosDuplicados);
        //Act
        var result = await service
            .ValidateOperacaoEngenhariaDuplicada()
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.OperacaoEngenhariaDuplicada);
    }
}