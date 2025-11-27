using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Causas.Services.CausaServiceTests;

public class CreateTests : CausaServiceTest
{
    [Fact(DisplayName = "Create Causa with Success")]
    public async Task CreateCausaWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var createInput = new CausaInput(TestUtils.ObjectMother.GetCausa(0));

        var expectedResult = new Causa
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IsAtivo = true
        };
        //Act
        var output = await service.Create(createInput);

        //Assert
        var causa = await mocker.Causas.FindAsync(createInput.Id);

        causa.Should().BeEquivalentTo(expectedResult, TestUtils.ExcludeAuditoria);
        output.Should().Be(ValidationResult.Ok);
    }

    [Fact(DisplayName = "Se causa criada, is ativo deve ser true por padrão")]
    public async Task CreateTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var createInput = new CausaInput(TestUtils.ObjectMother.GetCausa(0));
        
        //Act
        await service.Create(createInput);

        //Assert
        var result = await mocker.Causas.FindAsync(createInput.Id);
        result.IsAtivo.Should().BeTrue();
    }
}