using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.Solucoes;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Solucoes.Services.SolucaoServiceTests;

public class ChangeStatusTests : SolucaoServiceTest
{
    [Fact(DisplayName = "Se isAtivo for enviado como false, deve desativar a causa")]
    public async Task ChangeStatusTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var causa = new Solucao
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = true
        };
        await mocker.Solucoes.InsertAsync(causa, true);

        var expectedResult = new Solucao
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
        var result = await mocker.Solucoes.FindAsync(TestUtils.ObjectMother.Guids[0]);
        result.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
    [Fact(DisplayName = "Se isAtivo for enviado como true, deve ativar a causa")]
    public async Task ChangeStatusTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var causa = new Solucao
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            IsAtivo = false
        };
        await mocker.Solucoes.InsertAsync(causa, true);

        var expectedResult = new Solucao
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
        var result = await mocker.Solucoes.FindAsync(TestUtils.ObjectMother.Guids[0]);
        result.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
}