using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OperacoesRetrabalhos.Services.OperacaoRetrabalhoNaoConformidadeValidatorServices;

public class ValidateOdfApontadaTests : OperacaoRetrabalhoNaoConformidadeValidatorServiceTest
{
    [Fact(DisplayName = "Se odf ainda não apontada, deve retornar OdfNaoApontada")]
    public async Task ValidateOdfApontadaTest1()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();

        mocker.OperacaoService.ValidarOdfPossuiApontamento(agregacao.NaoConformidade.NumeroOdf.Value).Returns(false);
        agregacao.NaoConformidade.OperacaoRetrabalho = new OperacaoRetrabalhoNaoConformidade();
        
        // Act
        var result = await service.ValidateOdfApontada(agregacao);
        
        // Assert
        result.Should().Be(OperacaoRetrabalhoNaoConformidadeValidationResult.OdfNaoApontada);
    }
    
    [Fact(DisplayName = "Se odf já foi apontada, deve retornar ok")]
    public async Task ValidateOdfApontadaTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        mocker.OperacaoService.ValidarOdfPossuiApontamento(agregacao.NaoConformidade.NumeroOdf.Value).Returns(true);

        agregacao.ServicoNaoConformidades.Clear();

        agregacao.NaoConformidade.OperacaoRetrabalho = new OperacaoRetrabalhoNaoConformidade();
        
        // Act
        var result = await service.ValidateOdfApontada(agregacao);
        
        // Assert
        result.Should().Be(OperacaoRetrabalhoNaoConformidadeValidationResult.Ok);
    }
}