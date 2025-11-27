using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OrdemRetrabalhos.Services.EstornarOrdemRetrabalhoValidatorServicesTests;

public class ValidateOdfRetrabalhoTests : EstornarOrdemRetrabalhoValidatorServiceTest
{
    [Fact(DisplayName = "Se for encontrada odf de retrabalho, deve retornar ok")]
    public async Task ValidateOdfRetrabalhoTest1()
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

        var ordemProducao = new OrdemProducaoOutput
        {
        
        };
        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], false).Returns(ordemProducao);
        //Act
        var result = await service
            .ValidateOdfRetrabalho()
            .ValidateAsync(agregacaoNaoConformidade);
        
        //Assert
        result.Should().Be(EstornarOrdemRetrabalhoValidationResult.Ok);
    }
    [Fact(DisplayName = "Se não for encontrada odf de retrabalho nao conformidade, deve retornar odfRetrabalhoNaoEncontrada")]
    public async Task ValidateOdfRetrabalhoTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var agregacaoNaoConformidade = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        //Act
        var result = await service
            .ValidateOdfRetrabalho()
            .ValidateAsync(agregacaoNaoConformidade);
        
        //Assert
        result.Should().Be(EstornarOrdemRetrabalhoValidationResult.OdfRetrabalhoNaoEncontrada);
    }
    
    [Fact(DisplayName = "Se não for encontrada odf de retrabalho, deve retornar odfRetrabalhoNaoEncontrada")]
    public async Task ValidateOdfRetrabalhoTest3()
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
        //Act
        var result = await service
            .ValidateOdfRetrabalho()
            .ValidateAsync(agregacaoNaoConformidade);
        
        //Assert
        result.Should().Be(EstornarOrdemRetrabalhoValidationResult.OdfRetrabalhoNaoEncontrada);
    }
    
}