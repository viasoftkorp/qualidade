using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Naturezas.Services.NaturezaServiceTests;

public class CreateTests : NaturezaServiceTest
{
    [Fact(DisplayName = "Create Natureza with Success")]
    public async Task CreateNaturezaWithSuccessTest()
    {
        //Arrange
        var mock = GetMocker();
        var service = GetService(mock);

        var naturezaInput = TestUtils.ObjectMother.GetNaturezaInput(0);

        var expectedResult = new Natureza
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Codigo = 1,
            IsAtivo = true
        };
        //Act
        var output = await service.Create(naturezaInput);

        //Assert
        var natureza = await mock.Naturezas.FindAsync(naturezaInput.Id);
        natureza.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
        output.Should().Be(ValidationResult.Ok);
    }
    [Fact(DisplayName = "Se nova natureza cadastrada, isAtivo é true por padrão")]
    public async Task CreateTest1()
    {
        //Arrange
        var mock = GetMocker();
        var service = GetService(mock);

        var naturezaInput = TestUtils.ObjectMother.GetNaturezaInput(0);
        
        //Act
        await service.Create(naturezaInput);

        //Assert
        var natureza = await mock.Naturezas.FindAsync(naturezaInput.Id);
        natureza.IsAtivo.Should().BeTrue();
    }
}