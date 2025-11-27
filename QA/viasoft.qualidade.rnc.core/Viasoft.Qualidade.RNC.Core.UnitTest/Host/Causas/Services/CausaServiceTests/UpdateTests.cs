using System.Threading.Tasks;
using FluentAssertions;
using Viasoft.Qualidade.RNC.Core.Domain.Causas;
using Viasoft.Qualidade.RNC.Core.Host.Causas.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Causas.Services.CausaServiceTests;

public class UpdateTests : CausaServiceTest
{
    [Fact(DisplayName = "Update Causa with Success")]
    public async Task UpdateCausaWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var causaInput = TestUtils.ObjectMother.GetCausa(0);
        
        await mocker.Causas.InsertAsync(causaInput);
        
        var updateInput = new CausaInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[2],
            Codigo = causaInput.Codigo,
            Detalhamento = TestUtils.ObjectMother.Strings[3]
        };
        
        await UnitOfWork.SaveChangesAsync();
        
        var expectedResult = new Causa
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[2],
            Codigo = causaInput.Codigo,
            Detalhamento = TestUtils.ObjectMother.Strings[3],
            IsAtivo = true,
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };
        
        //Act
        var output = await service.Update(causaInput.Id, updateInput);

        //Assert
        var causa = await mocker.Causas.FindAsync(updateInput.Id);
    
        causa.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
        output.Should().Be(ValidationResult.Ok);
    }
    
   
    [Fact(DisplayName = "Update Causa Returns NotFound")]
    public async Task UpdateCausaWithNotFoundIdTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var updateInput = new CausaInput(TestUtils.ObjectMother.GetCausa(0));

        //Act
        var output = await service.Update(updateInput.Id, updateInput);

        //Assert
        output.Should().Be(ValidationResult.NotFound);
    }
    
    [Fact(DisplayName = "Se isAtivo vier pelo input, o mesmo não deve ser alterado")]
    public async Task UpdateTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);
        
        var causa = TestUtils.ObjectMother.GetCausa(0);
        causa.IsAtivo = true;
        await mocker.Causas.InsertAsync(causa, true);
        
        var input = new CausaInput()
        {
            Id = causa.Id,
            Descricao = TestUtils.ObjectMother.Strings[1],
            Codigo = causa.Codigo,
            IsAtivo = false
        };

        //Act
        var output = await service.Update(causa.Id, input);

        //Assert
        var result = await mocker.Causas.FindAsync(causa.Id);

        result.IsAtivo.Should().BeTrue();

    }
}