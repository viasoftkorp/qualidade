using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Rebus.TestHelpers.Events;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos.Events;
using Viasoft.Qualidade.RNC.Core.Host.Defeitos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Defeitos.Services.DefeitoServiceTests;

public class UpdateTests : DefeitoServiceTest
{
    [Fact(DisplayName = "Update Defeito with Success")]
    public async Task UpdateDefeitoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var defeitoInput = TestUtils.ObjectMother.GetDefeito(0);
        
        await mocker.Defeitos.InsertAsync(defeitoInput);
        
        var updateInput = new DefeitoInput
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[2],
            Codigo = defeitoInput.Codigo,
            Detalhamento = TestUtils.ObjectMother.Strings[3],
            IsAtivo = false
        };

        var expectedResult = new Defeito
        {
            Id = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[2],
            Codigo = defeitoInput.Codigo,
            Detalhamento = TestUtils.ObjectMother.Strings[3],
            IsAtivo = true,
            TenantId = TestUtils.ObjectMother.Guids[0],
            EnvironmentId = TestUtils.ObjectMother.Guids[0],
        };
        
        await UnitOfWork.SaveChangesAsync();
        
        //Act
        var output = await service.Update(defeitoInput.Id, updateInput);

        //Assert
        var defeito = await mocker.Defeitos.FindAsync(updateInput.Id);
        ServiceBus.FakeBus.Events.OfType<MessagePublished<DefeitoUpdated>>().Should().HaveCount(1);
        defeito.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
        output.Should().Be(ValidationResult.Ok);
    }
    
   
    [Fact(DisplayName = "Update Defeito Returns NotFound")]
    public async Task UpdateDefeitoWithNotFoundIdTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var updateInput = TestUtils.ObjectMother.GetDefeitoInput(0);

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
        
        var defeito = TestUtils.ObjectMother.GetDefeito(0);
        defeito.IsAtivo = true;
        await mocker.Defeitos.InsertAsync(defeito, true);
        
        var input = new DefeitoInput()
        {
            Id = defeito.Id,
            Descricao = TestUtils.ObjectMother.Strings[1],
            Codigo = defeito.Codigo,
            IsAtivo = false
        };

        //Act
        var output = await service.Update(defeito.Id, input);

        //Assert
        var result = await mocker.Defeitos.FindAsync(defeito.Id);

        result.IsAtivo.Should().BeTrue();

    }
}