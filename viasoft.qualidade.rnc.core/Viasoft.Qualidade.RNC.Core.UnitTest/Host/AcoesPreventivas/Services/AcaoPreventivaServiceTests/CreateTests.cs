using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.AcoesPreventivas.Services.AcaoPreventivaServiceTests;

public class CreateTests : AcaoPreventivaServiceTest
{
    [Fact(DisplayName = "Se campos preenchidos, deve criar nova ação preventiva")]
    public async Task CreateTest1()
    {
        // Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        var acaoPreventivaInput = new AcaoPreventivaInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0]
        };
        var expectedResult = new AcaoPreventiva
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            IsAtivo = true
        };

        // Act
        var validationResult = await service.Create(acaoPreventivaInput);

        // Assert
        validationResult.Should().Be(ValidationResult.Ok);
        var acaoPreventivaResult = await mocker.AcaoPreventiva.FindAsync(TestUtils.ObjectMother.Guids[0]);
        acaoPreventivaResult.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
    [Fact(DisplayName = "Se acaoPreventiva criada, is ativo deve ser true por padrão")]
    public async Task CreateTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = new AcaoPreventivaInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
        };
        //Act
        var output = await service.Create(input);

        //Assert
        var result = await mocker.AcaoPreventiva.FindAsync(input.Id);
        result.IsAtivo.Should().BeTrue();
    }
}