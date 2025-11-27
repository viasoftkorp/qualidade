using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.AcaoPreventivaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.AcoesPreventivas;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.AcoesPreventivas.Services.AcaoPreventivaServiceTests;

public class DeleteTests : AcaoPreventivaServiceTest
{
    [Fact(DisplayName = "Se ação preventiva não utilizada em não conformidade, deve deleta-la")]
    public async Task DeleteTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        await mocker.AcaoPreventiva.InsertAsync(new AcaoPreventiva
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0]
        }, true);
        //Act
        var output = await service.Delete(TestUtils.ObjectMother.Guids[0]);
        
        //Assert
        output.Should().Be(ValidationResult.Ok);

        mocker.AcaoPreventiva.Should().BeEmpty();
    }
    
    [Fact(DisplayName = "Se ação preventiva utilizada em não conformidade, deve retornar EntidadeEmUso")]
    public async Task DeleteTest2()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        await mocker.AcaoPreventiva.InsertAsync(new AcaoPreventiva
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0]
        }, true);
        await mocker.AcaoPreventivaNaoConformidades.InsertAsync(new AcaoPreventivaNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdResponsavel = TestUtils.ObjectMother.Guids[0],
            IdNaoConformidade = TestUtils.ObjectMother.Guids[0],
            Acao = TestUtils.ObjectMother.Strings[0],
            IdAcaoPreventiva = TestUtils.ObjectMother.Guids[0]
        }, true);
        //Act
        var output = await service.Delete(TestUtils.ObjectMother.Guids[0]);
        
        //Assert
        output.Should().Be(ValidationResult.EntidadeEmUso);

        mocker.AcaoPreventiva.Should().NotBeEmpty();
    }
}