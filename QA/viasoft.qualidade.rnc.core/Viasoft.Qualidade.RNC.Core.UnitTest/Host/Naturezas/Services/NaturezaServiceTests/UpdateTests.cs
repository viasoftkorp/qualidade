using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Naturezas.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Naturezas.Services.NaturezaServiceTests;

public class UpdateTests : NaturezaServiceTest
{
    [Fact(DisplayName = "Update Natureza with Success")]
    public async Task UpdateNaturezaWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var natureza = TestUtils.ObjectMother.GetNatureza(0);
       
        await mocker.Naturezas.InsertAsync(natureza);
        await UnitOfWork.SaveChangesAsync();
        
        var naturezaUpdate = new NaturezaInput()
        {
            Id = natureza.Id,
            Descricao = TestUtils.ObjectMother.Strings[1],
            Codigo = natureza.Codigo,
        };
        var expectedResult = new Natureza
        {
            Id = natureza.Id,
            Descricao = TestUtils.ObjectMother.Strings[1],
            Codigo = natureza.Codigo,
            IsAtivo = true,
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };

        //Act
        var output = await service.Update(natureza.Id, naturezaUpdate);

        //Assert
        var result = await mocker.Naturezas.FindAsync(natureza.Id);

        result.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
        output.Should().Be(ValidationResult.Ok);
        
    }


    [Fact(DisplayName = "Update Natureza Returns NotFound")]
    public async Task UpdateNaturezaReturnsNotFoundTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var updateNatureza = TestUtils.ObjectMother.GetNaturezaInput(1);
        
        //Act
        var output = await service.Update(updateNatureza.Id, updateNatureza);
        
        //Assert
        output.Should().Be(ValidationResult.NotFound);
    }
    
    [Fact(DisplayName = "Se isAtivo vier pelo input, o mesmo não deve ser alterado")]
    public async Task UpdateTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var natureza = TestUtils.ObjectMother.GetNatureza(0);
       
        await mocker.Naturezas.InsertAsync(natureza);
        await UnitOfWork.SaveChangesAsync();
        
        var naturezaUpdate = new NaturezaInput()
        {
            Id = natureza.Id,
            Descricao = TestUtils.ObjectMother.Strings[1],
            Codigo = natureza.Codigo,
            IsAtivo = false
        };

        //Act
        var output = await service.Update(natureza.Id, naturezaUpdate);

        //Assert
        var result = await mocker.Naturezas.FindAsync(natureza.Id);

        result.IsAtivo.Should().BeTrue();

    }
}