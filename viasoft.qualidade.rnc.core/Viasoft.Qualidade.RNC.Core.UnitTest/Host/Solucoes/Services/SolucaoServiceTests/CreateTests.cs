using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.UnitTest.Host.Causas.Services.CausaServiceTests;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests;

public class CreateTests : SolucaoServiceTest
{
    [Fact(DisplayName = "Create Solucao with Success")]
    public async Task CreateSolucaoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var createInput = TestUtils.ObjectMother.GetSolucaoInput(0);
        
        var expectedResult = new Solucao
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            Imediata = false,
            Codigo = 1,
            IsAtivo = true
        };
        //Act
        var output = await service.Create(createInput);

        //Assert
        var solucao = await mocker.Solucoes.FindAsync(createInput.Id);

        solucao.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
        output.Should().Be(ValidationResult.Ok);
    }
    [Fact(DisplayName = "Se causa criada, is ativo deve ser true por padrão")]
    public async Task CreateTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = TestUtils.ObjectMother.GetSolucaoInput(0);
        
        //Act
        var output = await service.Create(input);

        //Assert
        var result = await mocker.Solucoes.FindAsync(input.Id);
        result.IsAtivo.Should().BeTrue();
    }
}