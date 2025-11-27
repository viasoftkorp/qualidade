using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.NaoConformidades.Retrabalhos.OperacoesRetrabalhos.Services.OperacaoRetrabalhoNaoConformidadeValidatorServices;

public class ValidateOdfAbertaTests : OperacaoRetrabalhoNaoConformidadeValidatorServiceTest
{
    [Fact(DisplayName = "Se ordem de produção estiver finalizada, deve retornar OdfFinalizada")]
    public async Task ValidateOdfAbertaTest1()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        mocker.OrdemProducaoProvider.GetByNumeroOdf(agregacao.NaoConformidade.NumeroOdf.Value, false)
            .Returns(new OrdemProducaoOutput
            {
                OdfFinalizada = true
            });
        // Act
        var result = await service.ValidateOdfAberta(agregacao);
        
        //Assert
        result.Should().Be(OperacaoRetrabalhoNaoConformidadeValidationResult.OdfFinalizada);
    }
    [Fact(DisplayName = "Se ordem de produção não estiver finalizada, deve retornar Ok")]
    public async Task ValidateOdfAbertaTest2()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var agregacao = TestUtils.ObjectMother.GetAgregacaoNaoConformidadeMock(0).AgregacaoFromThis();
        mocker.OrdemProducaoProvider.GetByNumeroOdf(agregacao.NaoConformidade.NumeroOdf.Value, false)
            .Returns(new OrdemProducaoOutput
            {
                OdfFinalizada = false
            });
        // Act
        var result = await service.ValidateOdfAberta(agregacao);
        
        //Assert
        result.Should().Be(OperacaoRetrabalhoNaoConformidadeValidationResult.Ok);
    }
}