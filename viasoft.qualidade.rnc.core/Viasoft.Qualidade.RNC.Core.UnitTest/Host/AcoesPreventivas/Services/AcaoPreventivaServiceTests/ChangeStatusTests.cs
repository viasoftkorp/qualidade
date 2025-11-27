using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.AcoesPreventivas.Services.AcaoPreventivaServiceTests;

public class ChangeStatusTests : AcaoPreventivaServiceTest
{
    [Fact(DisplayName = "Se isAtivo for enviado como false, deve desativar a acaoPreventiva")]
    public async Task ChangeStatusTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var acaoPreventiva = new AcaoPreventiva
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = true
        };
        await mocker.AcaoPreventiva.InsertAsync(acaoPreventiva, true);

        var expectedResult = new AcaoPreventiva
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = false
        };
        //Act
        var output = await service.ChangeStatus(acaoPreventiva.Id, false);
        
        //Assert
        output.Should().Be(ValidationResult.Ok);
        var result = await mocker.AcaoPreventiva.FindAsync(TestUtils.ObjectMother.Guids[0]);
        result.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
    [Fact(DisplayName = "Se isAtivo for enviado como true, deve ativar a acaoPreventiva")]
    public async Task ChangeStatusTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var acaoPreventiva = new AcaoPreventiva
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = false
        };
        await mocker.AcaoPreventiva.InsertAsync(acaoPreventiva, true);

        var expectedResult = new AcaoPreventiva
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = true
        };
        //Act
        var output = await service.ChangeStatus(acaoPreventiva.Id, true);
        
        //Assert
        output.Should().Be(ValidationResult.Ok);
        var result = await mocker.AcaoPreventiva.FindAsync(TestUtils.ObjectMother.Guids[0]);
        result.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
}