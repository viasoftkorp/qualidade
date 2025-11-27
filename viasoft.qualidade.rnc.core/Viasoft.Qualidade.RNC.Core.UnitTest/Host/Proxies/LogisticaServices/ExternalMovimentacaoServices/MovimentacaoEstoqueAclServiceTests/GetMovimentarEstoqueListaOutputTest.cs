using System.Collections.Generic;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Proxies.LogisticaServices.ExternalMovimentacaoServices.MovimentacaoEstoqueAclServiceTests;

public class GetMovimentarEstoqueListaOutputTests : MovimentacaoEstoqueAclServiceTest
{
    [Fact(DisplayName = "Se método chamado deve converter input para ExternalMovimentarEstoqueListaInput")]
    public void GetMovimentarEstoqueListaOutputTest()
    {
        // Arrange
        var dependencies = GetDependencies();
        var service = GetService(dependencies);

        var input = GetExternalMovimentarEstoqueItemOutput(0);

        var expectedResult = GetMovimentarEstoqueListaOutput(0);
        
        // Act
        var result = service.GetMovimentarEstoqueListaOutput(input);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    private MovimentarEstoqueListaOutput GetMovimentarEstoqueListaOutput(int index)
    {
        var movimentarEstoqueListaOutput = new MovimentarEstoqueListaOutput
        {
            Success = true,
            Message = "",
            DtoRetorno = new ExternalMovimentarEstoqueItemResultado
            {
                CodigoLocalDestino = TestUtils.ObjectMother.Ints[index],
                LegacyIdEstoqueLocalDestino = TestUtils.ObjectMother.Ints[index]
            }
        };
        return movimentarEstoqueListaOutput;
    }

    private ExternalMovimentarEstoqueItemOutput GetExternalMovimentarEstoqueItemOutput(int index)
    {
        var externalMovimentarEstoqueItemOutput = new ExternalMovimentarEstoqueItemOutput
        {
            Resultado = new List<ExternalMovimentarEstoqueItemOutputResultado>
            {
                new ExternalMovimentarEstoqueItemOutputResultado
                {
                    Resultados = new List<ExternalMovimentarEstoqueItemResultado>
                    {
                        new ExternalMovimentarEstoqueItemResultado
                        {
                            CodigoLocalDestino = TestUtils.ObjectMother.Ints[index],
                            LegacyIdEstoqueLocalDestino = TestUtils.ObjectMother.Ints[index]
                        }
                    }
                }
            },
            Error = null
        };
        return externalMovimentarEstoqueItemOutput;
    }
}