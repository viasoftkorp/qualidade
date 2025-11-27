using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Causas.Services.CausaServiceTests;

public class ChangeStatusTests : CausaServiceTest
{
    [Fact(DisplayName = "Se isAtivo for enviado como false, deve desativar a causa")]
    public async Task ChangeStatusTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var causa = new Causa
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = true
        };
        await mocker.Causas.InsertAsync(causa, true);

        var expectedResult = new Causa
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = false
        };
        //Act
        var output = await service.ChangeStatus(causa.Id, false);
        
        //Assert
        output.Should().Be(ValidationResult.Ok);
        var result = await mocker.Causas.FindAsync(TestUtils.ObjectMother.Guids[0]);
        result.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
    [Fact(DisplayName = "Se isAtivo for enviado como true, deve ativar a causa")]
    public async Task ChangeStatusTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var causa = new Causa
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = false
        };
        await mocker.Causas.InsertAsync(causa, true);

        var expectedResult = new Causa
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = true
        };
        //Act
        var output = await service.ChangeStatus(causa.Id, true);
        
        //Assert
        output.Should().Be(ValidationResult.Ok);
        var result = await mocker.Causas.FindAsync(TestUtils.ObjectMother.Guids[0]);
        result.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
}