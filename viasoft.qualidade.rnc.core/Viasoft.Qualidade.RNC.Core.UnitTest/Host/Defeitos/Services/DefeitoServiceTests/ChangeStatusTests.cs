using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Defeitos.Services.DefeitoServiceTests;

public class ChangeStatusTests : DefeitoServiceTest
{
    [Fact(DisplayName = "Se isAtivo for enviado como false, deve desativar a defeito")]
    public async Task ChangeStatusTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var defeito = new Defeito
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = true
        };
        await mocker.Defeitos.InsertAsync(defeito, true);

        var expectedResult = new Defeito
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = false
        };
        //Act
        var output = await service.ChangeStatus(defeito.Id, false);
        
        //Assert
        output.Should().Be(ValidationResult.Ok);
        var result = await mocker.Defeitos.FindAsync(TestUtils.ObjectMother.Guids[0]);
        result.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
    [Fact(DisplayName = "Se isAtivo for enviado como true, deve ativar a defeito")]
    public async Task ChangeStatusTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var defeito = new Defeito
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = false
        };
        await mocker.Defeitos.InsertAsync(defeito, true);

        var expectedResult = new Defeito
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = true
        };
        //Act
        var output = await service.ChangeStatus(defeito.Id, true);
        
        //Assert
        output.Should().Be(ValidationResult.Ok);
        var result = await mocker.Defeitos.FindAsync(TestUtils.ObjectMother.Guids[0]);
        result.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
}