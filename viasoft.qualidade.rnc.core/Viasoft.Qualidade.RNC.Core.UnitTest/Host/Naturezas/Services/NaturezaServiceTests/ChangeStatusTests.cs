using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Naturezas.Services.NaturezaServiceTests;

public class ChangeStatusTests : NaturezaServiceTest
{
    [Fact(DisplayName = "Se isAtivo for enviado como false, deve desativar a natureza")]
    public async Task ChangeStatusTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var natureza = new Natureza
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = true
        };
        await mocker.Naturezas.InsertAsync(natureza, true);

        var expectedResult = new Natureza
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = false
        };
        //Act
        var output = await service.ChangeStatus(natureza.Id, false);
        
        //Assert
        output.Should().Be(ValidationResult.Ok);
        var result = await mocker.Naturezas.FindAsync(TestUtils.ObjectMother.Guids[0]);
        result.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
    [Fact(DisplayName = "Se isAtivo for enviado como true, deve ativar a natureza")]
    public async Task ChangeStatusTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var natureza = new Natureza
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = false
        };
        await mocker.Naturezas.InsertAsync(natureza, true);

        var expectedResult = new Natureza
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = true
        };
        //Act
        var output = await service.ChangeStatus(natureza.Id, true);
        
        //Assert
        output.Should().Be(ValidationResult.Ok);
        var result = await mocker.Naturezas.FindAsync(TestUtils.ObjectMother.Guids[0]);
        result.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
}