using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Host.AcoesPreventivas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.AcoesPreventivas.Services.AcaoPreventivaServiceTests;

public class UpdateTests : AcaoPreventivaServiceTest
{
    [Fact(DisplayName = "Se ação preventiva encontrada, deve atualiza-la com base no input")]
    public async Task UpdateTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var input = new AcaoPreventivaInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[1],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Detalhamento = TestUtils.ObjectMother.Strings[1],
            IdResponsavel = TestUtils.ObjectMother.Guids[1]
        };

        var acaoInicial = new AcaoPreventiva
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
        };
        await mocker.AcaoPreventiva.InsertAsync(acaoInicial, true);

        var expectedResult = new AcaoPreventiva
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[1],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Detalhamento = TestUtils.ObjectMother.Strings[1],
            IdResponsavel = TestUtils.ObjectMother.Guids[1],
            IsAtivo = true
        };
        //Act
        var output = await service.Update(TestUtils.ObjectMother.Guids[0], input);
        
        //Assert
        output.Should().Be(ValidationResult.Ok);
        var acaoResult = await mocker.AcaoPreventiva.FindAsync(TestUtils.ObjectMother.Guids[0]);
        acaoResult.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
    }
    [Fact(DisplayName = "Se isAtivo vier pelo input, o mesmo não deve ser alterado")]
    public async Task UpdateTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var acaoPreventiva = TestUtils.ObjectMother.GetAcaoPreventiva(0);
        acaoPreventiva.IsAtivo = true;
        await mocker.AcaoPreventiva.InsertAsync(acaoPreventiva, true);
        
        var input = new AcaoPreventivaInput()
        {
            Id = acaoPreventiva.Id,
            Descricao = TestUtils.ObjectMother.Strings[1],
            Codigo = acaoPreventiva.Codigo,
            IsAtivo = false
        };

        //Act
        var output = await service.Update(acaoPreventiva.Id, input);

        //Assert
        var result = await mocker.AcaoPreventiva.FindAsync(acaoPreventiva.Id);

        result.IsAtivo.Should().BeTrue();

    }
}