using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Services.NaoConformidadeValidationService;

public class NaoConformidadeValidationServiceCampoOdfTests : NaoConformidadeValidationServiceTest
{
    [Fact(DisplayName = "Se não houver numero e origem for inspeção de saida, deve retornar odf obrigatório")]
    public async Task ValidarCampoOdfTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InpecaoSaida,
            NumeroOdf = null
        };
        //Act
        var result = await service.ValidarCampoOdf(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.OdfObrigatorio);
    }
    
    [Fact(DisplayName = "Se houver odf na lista de ordem producao, deve retornar Ok")]
    public async Task ValidarCampoOdfTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InpecaoSaida,
            NumeroOdf = TestUtils.ObjectMother.Ints[0]
        };
        
        mocker.OrdemProducaoProvider.GetByNumeroOdf(TestUtils.ObjectMother.Ints[0], true)
            .Returns(new OrdemProducaoOutput());
        //Act
        var result = await service.ValidarCampoOdf(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.Ok);
    }
    
    [Fact(DisplayName = "Se não houver odf na lista de ordem de produção, deve retornar Odf Inexistente")]
    public async Task ValidarCampoOdfTest3()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var input = new NaoConformidadeInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.InpecaoSaida,
            NumeroOdf = TestUtils.ObjectMother.Ints[0]
        };
        
        //Act
        var result = await service.ValidarCampoOdf(input);
        //Assert
        result.Should().Be(NaoConformidadeValidationResult.OdfInexistente);
    }
}