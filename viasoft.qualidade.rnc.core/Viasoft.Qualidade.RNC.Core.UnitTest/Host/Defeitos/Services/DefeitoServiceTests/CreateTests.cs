using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Rebus.TestHelpers.Events;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos.Events;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Defeitos.Services.DefeitoServiceTests;

public class CreateTests : DefeitoServiceTest
{
    [Fact(DisplayName = "Create Defeito with Success")]
    public async Task CreateDefeitoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var createInput = TestUtils.ObjectMother.GetDefeitoInput(0);

        var expectedResult = new Defeito
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdSolucao = TestUtils.ObjectMother.Guids[0],
            Descricao = TestUtils.ObjectMother.Strings[0],
            Detalhamento = TestUtils.ObjectMother.Strings[0],
            IdCausa = TestUtils.ObjectMother.Guids[0],
            Codigo = 1,
            IsAtivo = true
        };
        //Act
        var output = await service.Create(createInput);

        //Assert
        var defeito = await mocker.Defeitos.FindAsync(createInput.Id);
        ServiceBus.FakeBus.Events.OfType<MessagePublished<DefeitoCreated>>().Should().HaveCount(1);
        defeito.Should().BeEquivalentTo(expectedResult, options => TestUtils.ExcludeAuditoria(options));
        output.Should().Be(ValidationResult.Ok);
    }

    [Fact(DisplayName = "Se defeito criada, is ativo deve ser true por padrão")]
    public async Task CreateTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var createInput = TestUtils.ObjectMother.GetDefeitoInput(0);
        
        //Act
        var output = await service.Create(createInput);

        //Assert
        var result = await mocker.Defeitos.FindAsync(createInput.Id);
        result.IsAtivo.Should().BeTrue();
    }
}