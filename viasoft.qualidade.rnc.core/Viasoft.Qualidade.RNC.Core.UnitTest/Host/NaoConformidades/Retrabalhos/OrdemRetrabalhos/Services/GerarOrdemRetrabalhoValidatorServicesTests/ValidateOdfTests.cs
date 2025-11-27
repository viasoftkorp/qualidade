using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.GerarOrdemRetrabalhoValidatorServicesTests;

public class ValidateOdfTests : GerarOrdemRetrabalhoValidatorServiceTest
{
    [Fact(DisplayName = "Se não houver número odf, deve retornar OdfObrigatorio")]
    public async Task ValidateOdf1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.NumeroOdf = null;
        //Act
        var result = await service
            .ValidateOdf()
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.OdfObrigatorio);
    }
    
    [Fact(DisplayName = "Se odf de retrabalho já foi gerada, deve retornar OdfRetrabalhoJaGerada")]
    public async Task ValidateOdf3()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];
        
        await mocker.OrdemRetrabalhoNaoConformidadeRepository.InsertAsync(new OrdemRetrabalhoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            NumeroOdfRetrabalho = TestUtils.ObjectMother.Ints[0]
        }, true);
        
        //Act
        var result = await service
            .ValidateOdf()
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.OdfRetrabalhoJaGerada);
    }
    
    [Fact(DisplayName = "Se a odf de origem não for de retrabalho, odf não foi finalizada e ainda não foi gerada odf de retrabalho apartir dela, deve retornar ok")]
    public async Task ValidateOdf4()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        agregacaoNaoConformidade.NaoConformidade.NumeroOdf = TestUtils.ObjectMother.Ints[0];

        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], false)
            .Returns(new OrdemProducaoOutput
            {
                OdfFinalizada = true
            });
        //Act
        var result = await service
            .ValidateOdf()
            .ValidateAsync(agregacaoNaoConformidade);

        //Assert
        result.Should().Be(GerarOrdemRetrabalhoValidationResult.Ok);
    }
}