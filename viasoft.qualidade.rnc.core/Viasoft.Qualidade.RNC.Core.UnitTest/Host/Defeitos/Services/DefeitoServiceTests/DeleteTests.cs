using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Rebus.TestHelpers.Events;
using Viasoft.Qualidade.RNC.Core.Domain.DefeitoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.Defeitos.Events;
using Viasoft.Qualidade.RNC.Core.Host.Dtos;
using Xunit;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Host.Defeitos.Services.DefeitoServiceTests;

public class DeleteTests : DefeitoServiceTest
{
    [Fact(DisplayName = "Delete Defeito with Success")]
    public async Task DeleteDefeitoWithSuccessTest()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        var defeitoInserida = TestUtils.ObjectMother.GetDefeito(0);
        
        await mocker.Defeitos.InsertAsync(defeitoInserida);
        await UnitOfWork.SaveChangesAsync();

        //Act
        var output = await service.Delete(defeitoInserida.Id);

        //Assert
        var defeitoEncontrada = await mocker.Defeitos.AnyAsync(defeito => defeito.Id == defeitoInserida.Id);
        ServiceBus.FakeBus.Events.OfType<MessagePublished<DefeitoDeleted>>().Should().HaveCount(1);
        defeitoEncontrada.Should().BeFalse();
        output.Should().Be(ValidationResult.Ok);
    }

    
    [Fact(DisplayName = "Delete Defeito with NotFound Id")]
    public async Task DeleteDefeitoWithNotFoundId()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        //Act
        var output = await service.Delete(TestUtils.ObjectMother.Guids[0]);

        //Assert
        output.Should().Be(ValidationResult.NotFound);
    }
    
    [Fact(DisplayName = "Se defeito utilizado em RNC, deve retornar EntidadeEmUso")]
    public async Task DeleteTest1()
    {
        //Arrange
        var mocker = GetMocker();
        var service = GetService(mocker);

        await mocker.DefeitoNaoConformidades.InsertAsync(new DefeitoNaoConformidade
        {
            Id = TestUtils.ObjectMother.Guids[0],
            IdDefeito = TestUtils.ObjectMother.Guids[0]
        }, true);
        //Act
        var output = await service.Delete(TestUtils.ObjectMother.Guids[0]);

        //Assert
        output.Should().Be(ValidationResult.EntidadeEmUso);
    }
}