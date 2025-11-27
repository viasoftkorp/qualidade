using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Naturezas.Services.NaturezaServiceTests;

public class DeleteTests : NaturezaServiceTest
{
    [Fact(DisplayName = "Delete Natureza Returns with Success")]
    public async Task DeleteNaturezaReturnsWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var natureza = TestUtils.ObjectMother.GetNatureza(0);

        await mocker.Naturezas.InsertAsync(natureza);
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Delete(natureza.Id);
        
        //Assert
        var result = await mocker.Naturezas.FindAsync(natureza.Id);

        output.Should().Be(ValidationResult.Ok);
        result.Should().BeNull();

    }

    [Fact(DisplayName = "Delete Natureza Returns NotFound")]
    public async Task DeleteNaturezaReturnsNotFoundTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var natureza = TestUtils.ObjectMother.GetNatureza(1);
        
        //Act
        var output = await service.Delete(natureza.Id);
        
        //Assert
        var result = await mocker.Naturezas.FindAsync(natureza.Id);

        result.Should().BeNull();
        output.Should().Be(ValidationResult.NotFound);
    }
    [Fact(DisplayName = "Se natureza utilizada por não conformidade, deve retornar EntidadeEmUso")]
    public async Task DeleteTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var natureza = new Natureza
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Codigo = TestUtils.ObjectMother.Ints[0],
            Descricao = TestUtils.ObjectMother.Strings[0]
        };
        await mocker.Naturezas.InsertAsync(natureza, true);

        await mocker.NaoConformidades.InsertAsync(new NaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdNatureza = TestUtils.ObjectMother.Guids[0],
            IdProduto = TestUtils.ObjectMother.Guids[0],
            Origem = OrigemNaoConformidade.Interno
        }, true);
        //Act
        var output = await service.Delete(natureza.Id);
        
        //Assert
        output.Should().Be(ValidationResult.EntidadeEmUso);
        mocker.Naturezas.Should().NotBeEmpty();
    }
}